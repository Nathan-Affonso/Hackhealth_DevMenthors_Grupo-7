using Application;
using System.Net.Http.Json;

namespace Infrastructure;

public class PublicIntegrations(HttpClient http) : IAddressLookupService
{
    public async Task<object> BuscarCepAsync(string cep, CancellationToken ct = default)
        => await http.GetFromJsonAsync<object>($"https://viacep.com.br/ws/{cep}/json/", ct) ?? new { erro = true };

    public async Task<object> BuscarMunicipiosAsync(string uf, CancellationToken ct = default)
        => await http.GetFromJsonAsync<object>($"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{uf}/municipios", ct) ?? Array.Empty<object>();
}

public interface ICnesService { Task<object> BuscarAsync(string cnes); }
public interface ICnsService { Task<object> ValidarAsync(string cns); }
public interface ICnpjService { Task<object> BuscarAsync(string cnpj); }

public class SimulatedCnesService : ICnesService { public Task<object> BuscarAsync(string cnes) => Task.FromResult<object>(new { cnes, nome = "Servico CNES simulado", integrado = false }); }
public class SimulatedCnsService : ICnsService { public Task<object> ValidarAsync(string cns) => Task.FromResult<object>(new { cns, valido = true, integrado = false }); }
public class SimulatedCnpjService : ICnpjService { public Task<object> BuscarAsync(string cnpj) => Task.FromResult<object>(new { cnpj, razaoSocial = "Consulta CNPJ simulada", integrado = false }); }
