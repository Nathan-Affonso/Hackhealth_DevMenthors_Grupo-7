using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class BlhDbContext(DbContextOptions<BlhDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Instituicao> Instituicoes => Set<Instituicao>();
    public DbSet<Doadora> Doadoras => Set<Doadora>();
    public DbSet<TriagemDoadora> TriagensDoadora => Set<TriagemDoadora>();
    public DbSet<ExameDoadora> ExamesDoadora => Set<ExameDoadora>();
    public DbSet<RotaColeta> RotasColeta => Set<RotaColeta>();
    public DbSet<CaixaTermica> CaixasTermicas => Set<CaixaTermica>();
    public DbSet<Coleta> Coletas => Set<Coleta>();
    public DbSet<Frasco> Frascos => Set<Frasco>();
    public DbSet<RegistroRastreamentoCaixa> RegistrosRastreamentoCaixa => Set<RegistroRastreamentoCaixa>();
    public DbSet<RecepcaoBLH> RecepcoesBLH => Set<RecepcaoBLH>();
    public DbSet<LoteProcessamento> LotesProcessamento => Set<LoteProcessamento>();
    public DbSet<ProcessamentoFrasco> ProcessamentosFrasco => Set<ProcessamentoFrasco>();
    public DbSet<ControleQualidade> ControlesQualidade => Set<ControleQualidade>();
    public DbSet<Estoque> Estoques => Set<Estoque>();
    public DbSet<Receptor> Receptores => Set<Receptor>();
    public DbSet<PrescricaoLeite> PrescricoesLeite => Set<PrescricaoLeite>();
    public DbSet<Distribuicao> Distribuicoes => Set<Distribuicao>();
    public DbSet<DistribuicaoItem> DistribuicaoItens => Set<DistribuicaoItem>();
    public DbSet<RecebimentoHospital> RecebimentosHospital => Set<RecebimentoHospital>();
    public DbSet<AdministracaoFrasco> AdministracoesFrasco => Set<AdministracaoFrasco>();
    public DbSet<Auditoria> Auditorias => Set<Auditoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<PerfilUsuario>();
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var id = entity.FindProperty(nameof(Entity.Id));
            if (id is not null) id.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never;
        }

        modelBuilder.Entity<Usuario>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Frasco>().HasIndex(x => x.CodigoUnico).IsUnique();
        modelBuilder.Entity<RegistroRastreamentoCaixa>().HasIndex(x => new { x.CaixaTermicaId, x.DataHoraRegistro });

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder b)
    {
        var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var blhId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var hospId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var doadoraId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var rotaId = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var caixaId = Guid.Parse("66666666-6666-6666-6666-666666666666");
        var coletaId = Guid.Parse("77777777-7777-7777-7777-777777777777");
        var frascoId = Guid.Parse("88888888-8888-8888-8888-888888888888");
        var receptorId = Guid.Parse("99999999-9999-9999-9999-999999999999");
        var prescricaoId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var loteId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var now = DateTime.SpecifyKind(new DateTime(2026, 5, 23, 12, 0, 0), DateTimeKind.Utc);

        b.Entity<Usuario>().HasData(new Usuario { Id = adminId, Nome = "Administrador BLH", Email = "admin@blh.local", SenhaHash = "seed-admin", Perfil = PerfilUsuario.Admin, Ativo = true, CriadoEm = DateTime.SpecifyKind(new DateTime(2026, 5, 23, 10, 0, 0), DateTimeKind.Utc) });
        b.Entity<Instituicao>().HasData(
            new Instituicao { Id = blhId, Nome = "Banco de Leite Humano Exemplo", Tipo = TipoInstituicao.BLH, CNES = "0000001", CNPJ = "00000000000100", Endereco = "Av. Central, 100", Municipio = "Marilia", UF = "SP", Latitude = -22.2135m, Longitude = -49.9450m, Ativa = true },
            new Instituicao { Id = hospId, Nome = "Hospital Materno Infantil", Tipo = TipoInstituicao.Hospital, CNES = "0000002", CNPJ = "00000000000200", Endereco = "Rua Saude, 200", Municipio = "Marilia", UF = "SP", Latitude = -22.2170m, Longitude = -49.9505m, Ativa = true });
        b.Entity<Doadora>().HasData(new Doadora { Id = doadoraId, Nome = "Maria Doadora", CPF = "12345678901", CNS = "700000000000001", DataNascimento = new DateTime(1995, 3, 10), Telefone = "(14) 99999-0001", Email = "maria@example.com", Endereco = "Rua das Flores, 50", Latitude = -22.2088m, Longitude = -49.9345m, DataParto = new DateTime(2026, 4, 12), TipoParto = "Normal", Status = StatusDoadora.Apta, CriadoEm = now });
        b.Entity<RotaColeta>().HasData(new RotaColeta { Id = rotaId, Nome = "Rota Norte - Manha", DiaSemana = "Segunda", Veiculo = "Van BLH 01", Motorista = "Joao", ResponsavelId = adminId, Status = StatusRota.EmAndamento, DataInicio = DateTime.SpecifyKind(new DateTime(2026, 5, 23, 10, 0, 0), DateTimeKind.Utc) });
        b.Entity<CaixaTermica>().HasData(new CaixaTermica { Id = caixaId, Codigo = "CX-001", Descricao = "Caixa termica principal", CapacidadeLitros = 12, Status = StatusCaixaTermica.EmRota });
        b.Entity<Coleta>().HasData(new Coleta { Id = coletaId, DoadoraId = doadoraId, RotaColetaId = rotaId, CaixaTermicaId = caixaId, ResponsavelId = adminId, DataHoraInicio = DateTime.SpecifyKind(new DateTime(2026, 5, 23, 10, 20, 0), DateTimeKind.Utc), DataHoraFim = DateTime.SpecifyKind(new DateTime(2026, 5, 23, 10, 35, 0), DateTimeKind.Utc), Latitude = -22.2088m, Longitude = -49.9345m, TemperaturaCaixaInicial = 3.8m, TemperaturaCaixaFinal = 4.2m, TemperaturaAmbiente = 24m, Status = StatusColeta.Realizada });
        b.Entity<Frasco>().HasData(new Frasco { Id = frascoId, CodigoUnico = "BLH-2026-0001", QrCode = "BLH-2026-0001", DoadoraId = doadoraId, ColetaId = coletaId, CaixaTermicaId = caixaId, LoteId = loteId, TipoLeite = TipoLeite.Maduro, VolumeMl = 120, Status = StatusFrasco.Aprovado, TemperaturaColeta = 4.1m, TemperaturaRecepcao = 4.4m, CriadoEm = now });
        b.Entity<LoteProcessamento>().HasData(new LoteProcessamento { Id = loteId, CodigoLote = "LOTE-2026-001", ResponsavelId = adminId, Status = StatusLote.Aprovado, DataInicio = now.AddDays(-1), DataFim = now, Observacoes = "Lote seed aprovado" });
        b.Entity<ControleQualidade>().HasData(new ControleQualidade { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), FrascoId = frascoId, LoteId = loteId, AcidezDornic = 4, Crematocrito = 6, CorConforme = true, OdorConforme = true, SujidadeAusente = true, ResultadoMicrobiologico = "Ausente", Aprovado = true, ResponsavelId = adminId, DataHora = now });
        b.Entity<Estoque>().HasData(new Estoque { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), FrascoId = frascoId, TipoEstoque = TipoEstoque.Pasteurizado, Freezer = "FZ-01", Prateleira = "A", Posicao = "01", TemperaturaAtual = -18, DataEntrada = now, DataValidade = now.AddMonths(6), Status = StatusEstoque.Reservado });
        b.Entity<Receptor>().HasData(new Receptor { Id = receptorId, Nome = "Bebe Receptor", CNS = "700000000000002", Prontuario = "PR-001", DataNascimento = new DateTime(2026, 5, 1), IdadeGestacional = 31, PesoNascimento = 1.25m, Diagnostico = "Prematuridade", HospitalId = hospId, Setor = "UTI Neonatal", Leito = "03", Status = StatusReceptor.Elegivel });
        b.Entity<PrescricaoLeite>().HasData(new PrescricaoLeite { Id = prescricaoId, ReceptorId = receptorId, ProfissionalSolicitanteId = adminId, TipoLeiteSolicitado = "Pasteurizado", VolumeDiarioMl = 120, VolumePorHorarioMl = 20, Horarios = "06:00,09:00,12:00,15:00,18:00,21:00", JustificativaClinica = "Prematuridade e baixo peso", Status = StatusPrescricao.Aberta, CriadoEm = now });

        var t = DateTime.SpecifyKind(new DateTime(2026, 5, 23, 10, 0, 0), DateTimeKind.Utc);
        b.Entity<RegistroRastreamentoCaixa>().HasData(
            Track("eeeeeeee-0001-0000-0000-000000000001", caixaId, rotaId, coletaId, adminId, TipoRegistroCaixa.SaidaBLH, -22.2135m, -49.9450m, 3.7m, t, "Saida do BLH"),
            Track("eeeeeeee-0002-0000-0000-000000000002", caixaId, rotaId, coletaId, adminId, TipoRegistroCaixa.TemperaturaDuranteRota, -22.2116m, -49.9418m, 3.9m, t.AddMinutes(8), "Temperatura durante deslocamento"),
            Track("eeeeeeee-0003-0000-0000-000000000003", caixaId, rotaId, coletaId, adminId, TipoRegistroCaixa.ChegadaDoadora, -22.2088m, -49.9345m, 4.1m, t.AddMinutes(20), "Chegada na doadora"),
            Track("eeeeeeee-0004-0000-0000-000000000004", caixaId, rotaId, coletaId, adminId, TipoRegistroCaixa.ColetaRealizada, -22.2088m, -49.9345m, 4.2m, t.AddMinutes(35), "Coleta realizada"),
            Track("eeeeeeee-0005-0000-0000-000000000005", caixaId, rotaId, coletaId, adminId, TipoRegistroCaixa.RetornoBLH, -22.2135m, -49.9450m, 4.4m, t.AddMinutes(55), "Retorno ao BLH"));
    }

    private static RegistroRastreamentoCaixa Track(string id, Guid caixaId, Guid rotaId, Guid coletaId, Guid userId, TipoRegistroCaixa tipo, decimal lat, decimal lng, decimal temp, DateTime quando, string obs) =>
        new() { Id = Guid.Parse(id), CaixaTermicaId = caixaId, RotaColetaId = rotaId, ColetaId = coletaId, UsuarioId = userId, TipoRegistro = tipo, Latitude = lat, Longitude = lng, Temperatura = temp, DataHoraRegistro = quando, Observacao = obs, OrigemRegistro = OrigemRegistro.Mobile, CriadoEm = quando };
}
