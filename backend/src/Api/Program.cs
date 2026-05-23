using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Application;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlhDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Postgres") ?? "Host=localhost;Port=5432;Database=blh;Username=blh;Password=blh"));
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddHttpClient<IAddressLookupService, PublicIntegrations>();
builder.Services.AddScoped<ICnesService, SimulatedCnesService>();
builder.Services.AddScoped<ICnsService, SimulatedCnsService>();
builder.Services.AddScoped<ICnpjService, SimulatedCnpjService>();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtKey = builder.Configuration["Jwt:Key"] ?? "BLH-dev-key-change-in-production-with-at-least-32-chars";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

await SeedRuntime.EnsureDatabaseAsync(app.Services);

var api = app.MapGroup("/api");

api.MapPost("/auth/login", async (LoginRequest req, BlhDbContext db, IPasswordHasher hasher, ITokenService tokens) =>
{
    var user = await db.Usuarios.FirstOrDefaultAsync(x => x.Email == req.Email && x.Ativo);
    if (user is null || !hasher.Verify(req.Senha, user.SenhaHash)) return Results.Unauthorized();
    return Results.Ok(new LoginResponse(tokens.CreateToken(user), user));
});
api.MapGet("/auth/me", (ClaimsPrincipal user) => Results.Ok(new { id = user.FindFirstValue(ClaimTypes.NameIdentifier), nome = user.Identity?.Name, email = user.FindFirstValue(ClaimTypes.Email), perfil = user.FindFirstValue(ClaimTypes.Role) })).RequireAuthorization();

MapCrud<Usuario>(api, "usuarios");
MapCrud<Instituicao>(api, "instituicoes");
MapCrud<Doadora>(api, "doadoras");
MapCrud<Receptor>(api, "receptores");
MapCrud<CaixaTermica>(api, "caixas-termicas");
MapCrud<RotaColeta>(api, "rotas");
MapCrud<Coleta>(api, "coletas");
MapCrud<LoteProcessamento>(api, "lotes");
MapCrud<PrescricaoLeite>(api, "prescricoes");

api.MapPost("/doadoras/{id:guid}/triagem", async (Guid id, TriagemDoadora input, BlhDbContext db) =>
{
    input.Id = Guid.NewGuid(); input.DoadoraId = id; input.CriadoEm = DateTime.UtcNow;
    db.TriagensDoadora.Add(input);
    var doadora = await db.Doadoras.FindAsync(id);
    if (doadora is not null) doadora.Status = input.Aprovada ? StatusDoadora.Apta : StatusDoadora.Inapta;
    await db.SaveChangesAsync();
    return Results.Created($"/api/doadoras/{id}/triagem", input);
});
api.MapGet("/doadoras/{id:guid}/triagem", async (Guid id, BlhDbContext db) => await db.TriagensDoadora.Where(x => x.DoadoraId == id).OrderByDescending(x => x.CriadoEm).ToListAsync());
api.MapPost("/doadoras/{id:guid}/aprovar", async (Guid id, BlhDbContext db) => await SetDoadoraStatus(id, StatusDoadora.Apta, db));
api.MapPost("/doadoras/{id:guid}/suspender", async (Guid id, BlhDbContext db) => await SetDoadoraStatus(id, StatusDoadora.Suspensa, db));

api.MapPost("/rotas/{id:guid}/iniciar", async (Guid id, BlhDbContext db) =>
{
    var rota = await db.RotasColeta.FindAsync(id); if (rota is null) return Results.NotFound();
    rota.Status = StatusRota.EmAndamento; rota.DataInicio = DateTime.UtcNow; await db.SaveChangesAsync(); return Results.Ok(rota);
});
api.MapPost("/rotas/{id:guid}/finalizar", async (Guid id, BlhDbContext db) =>
{
    var rota = await db.RotasColeta.FindAsync(id); if (rota is null) return Results.NotFound();
    rota.Status = StatusRota.Finalizada; rota.DataFim = DateTime.UtcNow; await db.SaveChangesAsync(); return Results.Ok(rota);
});
api.MapPost("/coletas/{id:guid}/iniciar", async (Guid id, BlhDbContext db) =>
{
    var coleta = await db.Coletas.FindAsync(id); if (coleta is null) return Results.NotFound();
    var doadora = await db.Doadoras.FindAsync(coleta.DoadoraId);
    if (doadora is null || doadora.Status is StatusDoadora.Inapta or StatusDoadora.Suspensa or StatusDoadora.Inativa) return Results.BadRequest("Doadora inapta, suspensa ou inativa nao pode ter coleta.");
    coleta.Status = StatusColeta.EmAndamento; coleta.DataHoraInicio = DateTime.UtcNow; await db.SaveChangesAsync(); return Results.Ok(coleta);
});
api.MapPost("/coletas/{id:guid}/finalizar", async (Guid id, BlhDbContext db) =>
{
    var coleta = await db.Coletas.FindAsync(id); if (coleta is null) return Results.NotFound();
    coleta.Status = StatusColeta.Realizada; coleta.DataHoraFim = DateTime.UtcNow; await db.SaveChangesAsync(); return Results.Ok(coleta);
});

api.MapPost("/frascos", async (Frasco frasco, BlhDbContext db) => { frasco.Id = Guid.NewGuid(); frasco.CriadoEm = DateTime.UtcNow; frasco.QrCode ??= frasco.CodigoUnico; db.Frascos.Add(frasco); await db.SaveChangesAsync(); return Results.Created($"/api/frascos/{frasco.Id}", frasco); });
api.MapGet("/frascos", async (BlhDbContext db) => await db.Frascos.OrderByDescending(x => x.CriadoEm).ToListAsync());
api.MapGet("/frascos/{id:guid}", async (Guid id, BlhDbContext db) => await db.Frascos.FindAsync(id) is { } x ? Results.Ok(x) : Results.NotFound());
api.MapGet("/frascos/codigo/{codigo}", async (string codigo, BlhDbContext db) => await db.Frascos.FirstOrDefaultAsync(x => x.CodigoUnico == codigo) is { } x ? Results.Ok(x) : Results.NotFound());
api.MapPost("/frascos/{id:guid}/alterar-status", async (Guid id, AlterarStatusFrascoRequest req, BlhDbContext db) =>
{
    var frasco = await db.Frascos.FindAsync(id); if (frasco is null) return Results.NotFound();
    if (frasco.Status == StatusFrasco.Descartado) return Results.BadRequest("Frasco descartado nao pode voltar ao fluxo.");
    frasco.Status = req.Status; await db.SaveChangesAsync(); return Results.Ok(frasco);
});

api.MapPost("/rastreamento-caixa", async (RastreioCaixaRequest req, ClaimsPrincipal user, BlhDbContext db) =>
{
    if (req.Latitude == 0 || req.Longitude == 0) return Results.BadRequest("Latitude e longitude sao obrigatorias.");
    var usuarioId = Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var uid) ? uid : Guid.Parse("11111111-1111-1111-1111-111111111111");
    var item = new RegistroRastreamentoCaixa { CaixaTermicaId = req.CaixaTermicaId, RotaColetaId = req.RotaColetaId, ColetaId = req.ColetaId, DistribuicaoId = req.DistribuicaoId, UsuarioId = usuarioId, TipoRegistro = req.TipoRegistro, Latitude = req.Latitude, Longitude = req.Longitude, Temperatura = req.Temperatura, DataHoraRegistro = DateTime.SpecifyKind(req.DataHoraRegistro, DateTimeKind.Utc), Observacao = req.Observacao, OrigemRegistro = req.OrigemRegistro };
    db.RegistrosRastreamentoCaixa.Add(item); await db.SaveChangesAsync(); return Results.Created($"/api/rastreamento-caixa/{item.Id}", item);
});
api.MapGet("/rastreamento-caixa/rota/{id:guid}", async (Guid id, BlhDbContext db) => await Rastreio(db).Where(x => x.RotaColetaId == id).ToListAsync());
api.MapGet("/rastreamento-caixa/coleta/{id:guid}", async (Guid id, BlhDbContext db) => await Rastreio(db).Where(x => x.ColetaId == id).ToListAsync());
api.MapGet("/rastreamento-caixa/distribuicao/{id:guid}", async (Guid id, BlhDbContext db) => await Rastreio(db).Where(x => x.DistribuicaoId == id).ToListAsync());
api.MapGet("/rastreamento-caixa/caixa/{id:guid}", async (Guid id, BlhDbContext db) => await Rastreio(db).Where(x => x.CaixaTermicaId == id).ToListAsync());

api.MapPost("/recepcao-blh", async (RecepcaoBLH rec, BlhDbContext db) =>
{
    rec.Id = Guid.NewGuid(); rec.DataHora = DateTime.UtcNow; db.RecepcoesBLH.Add(rec);
    var f = await db.Frascos.FindAsync(rec.FrascoId); if (f is not null) { f.TemperaturaRecepcao = rec.TemperaturaChegada; f.Status = rec.Conforme ? StatusFrasco.Recebido : StatusFrasco.Descartado; }
    await db.SaveChangesAsync(); return Results.Created($"/api/recepcao-blh/{rec.Id}", rec);
});
api.MapGet("/recepcao-blh/frasco/{id:guid}", async (Guid id, BlhDbContext db) => await db.RecepcoesBLH.Where(x => x.FrascoId == id).ToListAsync());

api.MapPost("/lotes/{id:guid}/adicionar-frasco", async (Guid id, AdicionarFrascoLoteRequest req, BlhDbContext db) =>
{
    var f = await db.Frascos.FindAsync(req.FrascoId); if (f is null) return Results.NotFound();
    f.LoteId = id; f.Status = StatusFrasco.EmProcessamento;
    db.ProcessamentosFrasco.Add(new ProcessamentoFrasco { LoteId = id, FrascoOrigemId = f.Id, TipoProcessamento = TipoProcessamento.SelecaoClassificacao, ResponsavelId = req.ResponsavelId });
    await db.SaveChangesAsync(); return Results.Ok(f);
});
api.MapPost("/lotes/{id:guid}/reenvase", async (Guid id, ReenvaseRequest req, BlhDbContext db) =>
{
    var origem = await db.Frascos.FindAsync(req.FrascoOrigemId); if (origem is null) return Results.NotFound();
    var destino = new Frasco { CodigoUnico = $"{origem.CodigoUnico}-R{DateTime.UtcNow:HHmmss}", QrCode = $"{origem.CodigoUnico}-R", DoadoraId = origem.DoadoraId, ColetaId = origem.ColetaId, CaixaTermicaId = origem.CaixaTermicaId, LoteId = id, TipoLeite = origem.TipoLeite, VolumeMl = req.VolumeMl, Status = StatusFrasco.EmProcessamento };
    db.Frascos.Add(destino); db.ProcessamentosFrasco.Add(new ProcessamentoFrasco { LoteId = id, FrascoOrigemId = origem.Id, FrascoDestinoId = destino.Id, TipoProcessamento = TipoProcessamento.Reenvase, ResponsavelId = req.ResponsavelId });
    await db.SaveChangesAsync(); return Results.Ok(destino);
});
api.MapPost("/lotes/{id:guid}/pasteurizar", async (Guid id, PasteurizarRequest req, BlhDbContext db) =>
{
    var f = await db.Frascos.FindAsync(req.FrascoId); if (f is null) return Results.NotFound();
    f.Status = StatusFrasco.Pasteurizado; db.ProcessamentosFrasco.Add(new ProcessamentoFrasco { LoteId = id, FrascoOrigemId = f.Id, TipoProcessamento = TipoProcessamento.Pasteurizacao, ResponsavelId = req.ResponsavelId }); await db.SaveChangesAsync(); return Results.Ok(f);
});

api.MapPost("/controle-qualidade", async (ControleQualidade cq, BlhDbContext db) => { cq.Id = Guid.NewGuid(); cq.DataHora = DateTime.UtcNow; db.ControlesQualidade.Add(cq); await db.SaveChangesAsync(); return Results.Created($"/api/controle-qualidade/{cq.Id}", cq); });
api.MapGet("/controle-qualidade/frasco/{id:guid}", async (Guid id, BlhDbContext db) => await db.ControlesQualidade.Where(x => x.FrascoId == id).ToListAsync());
api.MapPost("/controle-qualidade/{id:guid}/aprovar", async (Guid id, BlhDbContext db) => await SetQualidade(id, true, db));
api.MapPost("/controle-qualidade/{id:guid}/reprovar", async (Guid id, BlhDbContext db) => await SetQualidade(id, false, db));

api.MapGet("/estoque", async (BlhDbContext db) => await db.Estoques.ToListAsync());
api.MapPost("/estoque/entrada", async (Estoque e, BlhDbContext db) =>
{
    var f = await db.Frascos.FindAsync(e.FrascoId);
    var rec = await db.RecepcoesBLH.OrderByDescending(x => x.DataHora).FirstOrDefaultAsync(x => x.FrascoId == e.FrascoId);
    var cq = await db.ControlesQualidade.OrderByDescending(x => x.DataHora).FirstOrDefaultAsync(x => x.FrascoId == e.FrascoId);
    if (f is null || rec is not { Conforme: true } || cq is not { Aprovado: true }) return Results.BadRequest("Frasco precisa de recepcao conforme e qualidade aprovada para entrar em estoque.");
    e.Id = Guid.NewGuid(); db.Estoques.Add(e); f.Status = e.TipoEstoque == TipoEstoque.Cru ? StatusFrasco.CruEstocado : StatusFrasco.Aprovado; await db.SaveChangesAsync(); return Results.Created($"/api/estoque/{e.Id}", e);
});
api.MapPost("/estoque/{id:guid}/bloquear", async (Guid id, BlhDbContext db) => await SetEstoque(id, StatusEstoque.Bloqueado, db));
api.MapPost("/estoque/{id:guid}/reservar", async (Guid id, BlhDbContext db) => await SetEstoque(id, StatusEstoque.Reservado, db));

api.MapPost("/distribuicoes", async (Distribuicao d, BlhDbContext db) => { d.Id = Guid.NewGuid(); db.Distribuicoes.Add(d); await db.SaveChangesAsync(); return Results.Created($"/api/distribuicoes/{d.Id}", d); });
api.MapPost("/distribuicoes/{id:guid}/separar-frasco", async (Guid id, SepararFrascoRequest req, BlhDbContext db) =>
{
    var f = await db.Frascos.FindAsync(req.FrascoId); var cq = await db.ControlesQualidade.OrderByDescending(x => x.DataHora).FirstOrDefaultAsync(x => x.FrascoId == req.FrascoId);
    if (f is null || cq is not { Aprovado: true }) return Results.BadRequest("Frasco sem qualidade aprovada nao pode ser distribuido.");
    db.DistribuicaoItens.Add(new DistribuicaoItem { DistribuicaoId = id, FrascoId = req.FrascoId, VolumeMl = req.VolumeMl }); f.Status = StatusFrasco.Distribuido;
    var est = await db.Estoques.FirstOrDefaultAsync(x => x.FrascoId == req.FrascoId); if (est is not null) est.Status = StatusEstoque.Reservado;
    await db.SaveChangesAsync(); return Results.Ok();
});
api.MapPost("/distribuicoes/{id:guid}/enviar", async (Guid id, EnviarDistribuicaoRequest req, BlhDbContext db) =>
{
    var d = await db.Distribuicoes.FindAsync(id); if (d is null) return Results.NotFound();
    d.Status = StatusDistribuicao.EmTransporte; d.ResponsavelEntregaId = req.ResponsavelEntregaId; d.TemperaturaSaida = req.TemperaturaSaida; d.DataHoraSaida = DateTime.UtcNow; await db.SaveChangesAsync(); return Results.Ok(d);
});
api.MapPost("/distribuicoes/{id:guid}/receber-hospital", async (Guid id, ReceberHospitalRequest req, BlhDbContext db) =>
{
    var d = await db.Distribuicoes.FindAsync(id); if (d is null) return Results.NotFound();
    d.Status = StatusDistribuicao.Entregue;
    var rec = new RecebimentoHospital { DistribuicaoId = id, HospitalId = d.HospitalId, ResponsavelRecebimentoId = req.ResponsavelRecebimentoId, TemperaturaChegada = req.TemperaturaChegada, Conforme = req.Conforme, Observacoes = req.Observacoes };
    db.RecebimentosHospital.Add(rec);
    foreach (var item in await db.DistribuicaoItens.Where(x => x.DistribuicaoId == id).ToListAsync()) if (await db.Frascos.FindAsync(item.FrascoId) is { } f) f.Status = StatusFrasco.RecebidoHospital;
    await db.SaveChangesAsync(); return Results.Ok(rec);
});

api.MapPost("/administracoes", async (AdministracaoFrasco adm, BlhDbContext db) =>
{
    var item = await db.DistribuicaoItens.FirstOrDefaultAsync(x => x.FrascoId == adm.FrascoId);
    if (item is null) return Results.BadRequest("Frasco nao foi separado para distribuicao.");
    var dist = await db.Distribuicoes.FindAsync(item.DistribuicaoId);
    var presc = dist is null ? null : await db.PrescricoesLeite.FindAsync(dist.PrescricaoLeiteId);
    if (presc is null || presc.ReceptorId != adm.ReceptorId || adm.PrescricaoLeiteId != presc.Id) return Results.BadRequest("Frasco nao reservado/distribuido para este receptor.");
    adm.Id = Guid.NewGuid(); adm.DataHora = DateTime.UtcNow; db.AdministracoesFrasco.Add(adm);
    if (await db.Frascos.FindAsync(adm.FrascoId) is { } f) f.Status = StatusFrasco.Administrado;
    await db.SaveChangesAsync(); return Results.Created($"/api/administracoes/{adm.Id}", adm);
});

api.MapGet("/rastreabilidade/frasco/{codigo}", async (string codigo, BlhDbContext db) =>
{
    var f = await db.Frascos.FirstOrDefaultAsync(x => x.CodigoUnico == codigo); if (f is null) return Results.NotFound();
    return Results.Ok(new { frasco = f, doadora = await db.Doadoras.FindAsync(f.DoadoraId), coleta = await db.Coletas.FindAsync(f.ColetaId), caixa = await db.CaixasTermicas.FindAsync(f.CaixaTermicaId), recepcao = await db.RecepcoesBLH.Where(x => x.FrascoId == f.Id).ToListAsync(), qualidade = await db.ControlesQualidade.Where(x => x.FrascoId == f.Id).ToListAsync(), estoque = await db.Estoques.Where(x => x.FrascoId == f.Id).ToListAsync(), distribuicoes = await db.DistribuicaoItens.Where(x => x.FrascoId == f.Id).ToListAsync(), administracoes = await db.AdministracoesFrasco.Where(x => x.FrascoId == f.Id).ToListAsync(), rastreamento = await Rastreio(db).Where(x => x.CaixaTermicaId == f.CaixaTermicaId).ToListAsync() });
});
api.MapGet("/rastreabilidade/receptor/{id:guid}", async (Guid id, BlhDbContext db) => Results.Ok(new { receptor = await db.Receptores.FindAsync(id), prescricoes = await db.PrescricoesLeite.Where(x => x.ReceptorId == id).ToListAsync(), administracoes = await db.AdministracoesFrasco.Where(x => x.ReceptorId == id).ToListAsync() }));
api.MapGet("/rastreabilidade/doadora/{id:guid}", async (Guid id, BlhDbContext db) => Results.Ok(new { doadora = await db.Doadoras.FindAsync(id), coletas = await db.Coletas.Where(x => x.DoadoraId == id).ToListAsync(), frascos = await db.Frascos.Where(x => x.DoadoraId == id).ToListAsync() }));
api.MapGet("/rastreabilidade/caixa/{id:guid}", async (Guid id, BlhDbContext db) => Results.Ok(new { caixa = await db.CaixasTermicas.FindAsync(id), registros = await Rastreio(db).Where(x => x.CaixaTermicaId == id).ToListAsync(), frascos = await db.Frascos.Where(x => x.CaixaTermicaId == id).ToListAsync() }));

api.MapGet("/dashboard/resumo", async (BlhDbContext db) => Results.Ok(new
{
    doadorasAtivas = await db.Doadoras.CountAsync(x => x.Status == StatusDoadora.Apta),
    coletasHoje = await db.Coletas.CountAsync(x => x.DataHoraInicio != null && x.DataHoraInicio.Value.Date == DateTime.UtcNow.Date),
    frascosColetados = await db.Frascos.CountAsync(),
    frascosEmEstoque = await db.Estoques.CountAsync(x => x.Status == StatusEstoque.Disponivel || x.Status == StatusEstoque.Reservado),
    frascosDescartados = await db.Frascos.CountAsync(x => x.Status == StatusFrasco.Descartado),
    litrosColetados = await db.Frascos.SumAsync(x => x.VolumeMl) / 1000m,
    litrosDistribuidos = await db.DistribuicaoItens.SumAsync(x => x.VolumeMl) / 1000m,
    bebesAtendidos = await db.AdministracoesFrasco.Select(x => x.ReceptorId).Distinct().CountAsync(),
    alertasTemperatura = await db.RegistrosRastreamentoCaixa.CountAsync(x => x.Temperatura < 2 || x.Temperatura > 8),
    prescricoesAbertas = await db.PrescricoesLeite.CountAsync(x => x.Status == StatusPrescricao.Aberta)
}));
api.MapGet("/dashboard/coletas", async (BlhDbContext db) => await db.Coletas.GroupBy(x => x.DataHoraInicio!.Value.Date).Select(g => new { data = g.Key, total = g.Count() }).ToListAsync());
api.MapGet("/dashboard/estoque", async (BlhDbContext db) => await db.Estoques.GroupBy(x => x.Status).Select(g => new { status = g.Key.ToString(), total = g.Count() }).ToListAsync());
api.MapGet("/dashboard/temperaturas", async (BlhDbContext db) => await db.RegistrosRastreamentoCaixa.OrderBy(x => x.DataHoraRegistro).Select(x => new { x.DataHoraRegistro, x.Temperatura, x.RotaColetaId }).ToListAsync());
api.MapGet("/dashboard/alertas", async (BlhDbContext db) => await db.RegistrosRastreamentoCaixa.Where(x => x.Temperatura < 2 || x.Temperatura > 8).OrderByDescending(x => x.DataHoraRegistro).ToListAsync());

api.MapGet("/integracoes/viacep/{cep}", async (string cep, IAddressLookupService s) => await s.BuscarCepAsync(cep));
api.MapGet("/integracoes/ibge/municipios/{uf}", async (string uf, IAddressLookupService s) => await s.BuscarMunicipiosAsync(uf));
api.MapGet("/integracoes/cnes/{cnes}", async (string cnes, ICnesService s) => await s.BuscarAsync(cnes));
api.MapGet("/integracoes/cns/{cns}", async (string cns, ICnsService s) => await s.ValidarAsync(cns));
api.MapGet("/integracoes/cnpj/{cnpj}", async (string cnpj, ICnpjService s) => await s.BuscarAsync(cnpj));

app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();

static void MapCrud<T>(RouteGroupBuilder api, string route) where T : Entity
{
    var g = api.MapGroup($"/{route}");
    g.MapGet("/", async (BlhDbContext db) => await db.Set<T>().ToListAsync());
    g.MapGet("/{id:guid}", async (Guid id, BlhDbContext db) => await db.Set<T>().FindAsync(id) is { } x ? Results.Ok(x) : Results.NotFound());
    g.MapPost("/", async (T item, BlhDbContext db) => { item.Id = Guid.NewGuid(); db.Set<T>().Add(item); await db.SaveChangesAsync(); return Results.Created($"/api/{route}/{item.Id}", item); });
    g.MapPut("/{id:guid}", async (Guid id, T item, BlhDbContext db) => { if (await db.Set<T>().FindAsync(id) is not { } existing) return Results.NotFound(); item.Id = id; db.Entry(existing).CurrentValues.SetValues(item); await db.SaveChangesAsync(); return Results.Ok(existing); });
    g.MapDelete("/{id:guid}", async (Guid id, BlhDbContext db) => { if (await db.Set<T>().FindAsync(id) is not { } item) return Results.NotFound(); db.Remove(item); await db.SaveChangesAsync(); return Results.NoContent(); });
}

static IQueryable<RegistroRastreamentoCaixa> Rastreio(BlhDbContext db) => db.RegistrosRastreamentoCaixa.OrderBy(x => x.DataHoraRegistro);

static async Task<IResult> SetDoadoraStatus(Guid id, StatusDoadora status, BlhDbContext db)
{
    var d = await db.Doadoras.FindAsync(id); if (d is null) return Results.NotFound();
    d.Status = status; await db.SaveChangesAsync(); return Results.Ok(d);
}

static async Task<IResult> SetQualidade(Guid id, bool aprovado, BlhDbContext db)
{
    var cq = await db.ControlesQualidade.FindAsync(id); if (cq is null) return Results.NotFound();
    cq.Aprovado = aprovado;
    if (await db.Frascos.FindAsync(cq.FrascoId) is { } f) f.Status = aprovado ? StatusFrasco.Aprovado : StatusFrasco.Reprovado;
    await db.SaveChangesAsync(); return Results.Ok(cq);
}

static async Task<IResult> SetEstoque(Guid id, StatusEstoque status, BlhDbContext db)
{
    var e = await db.Estoques.FindAsync(id); if (e is null) return Results.NotFound();
    e.Status = status; await db.SaveChangesAsync(); return Results.Ok(e);
}

public class JwtTokenService(IConfiguration configuration) : ITokenService
{
    public string CreateToken(Usuario usuario)
    {
        var key = configuration["Jwt:Key"] ?? "BLH-dev-key-change-in-production-with-at-least-32-chars";
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Perfil.ToString())
        };
        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddHours(12), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
