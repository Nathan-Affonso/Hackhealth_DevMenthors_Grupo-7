using Domain;

namespace Application;

public record LoginRequest(string Email, string Senha);
public record LoginResponse(string Token, Usuario Usuario);
public record AlterarStatusFrascoRequest(StatusFrasco Status);
public record RastreioCaixaRequest(
    Guid CaixaTermicaId,
    Guid? RotaColetaId,
    Guid? ColetaId,
    Guid? DistribuicaoId,
    TipoRegistroCaixa TipoRegistro,
    decimal Latitude,
    decimal Longitude,
    decimal Temperatura,
    DateTime DataHoraRegistro,
    string? Observacao,
    OrigemRegistro OrigemRegistro = OrigemRegistro.Web);
public record AdicionarFrascoLoteRequest(Guid FrascoId, Guid ResponsavelId);
public record ReenvaseRequest(Guid FrascoOrigemId, decimal VolumeMl, Guid ResponsavelId);
public record PasteurizarRequest(Guid FrascoId, Guid ResponsavelId);
public record SepararFrascoRequest(Guid FrascoId, decimal VolumeMl);
public record EnviarDistribuicaoRequest(Guid ResponsavelEntregaId, decimal TemperaturaSaida);
public record ReceberHospitalRequest(Guid ResponsavelRecebimentoId, decimal TemperaturaChegada, bool Conforme, string? Observacoes);

public interface IPasswordHasher
{
    string Hash(string senha);
    bool Verify(string senha, string hash);
}

public interface ITokenService
{
    string CreateToken(Usuario usuario);
}

public interface IAddressLookupService
{
    Task<object> BuscarCepAsync(string cep, CancellationToken ct = default);
    Task<object> BuscarMunicipiosAsync(string uf, CancellationToken ct = default);
}
