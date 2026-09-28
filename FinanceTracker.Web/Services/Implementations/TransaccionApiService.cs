using FinanceTracker.Web.Models;
using FinanceTracker.Web.Services.Http;
using FinanceTracker.Web.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace FinanceTracker.Web.Services.Implementations
{
    public class TransaccionApiService : ITransaccionApiService
    {
        private readonly HttpClient _httpClient;

        public TransaccionApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TransaccionDto>> ObtenerPorCuentaAsync(int cuentaId, TransaccionFiltroDto? filtro = null)
        {
            var url = ConstruirUrlFiltro(cuentaId, filtro);

            var response = await _httpClient.GetAsync(url);

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<List<TransaccionDto>>(ApiJsonOptions.Default)
                ?? [];
        }

        public async Task<TransaccionDto?> ObtenerPorIdAsync(int transaccionId)
        {
            var response = await _httpClient.GetAsync($"api/transacciones/{transaccionId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<TransaccionDto>(ApiJsonOptions.Default);
        }

        public async Task<SaldoCuentaDto?> ObtenerSaldoCuentaAsync(int cuentaId)
        {
            var response = await _httpClient.GetAsync($"api/transacciones/cuenta/{cuentaId}/saldo");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<SaldoCuentaDto>(ApiJsonOptions.Default);
        }

        public async Task<TransaccionDto> CrearAsync(CrearTransaccionRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/transacciones", request, ApiJsonOptions.Default);

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<TransaccionDto>(ApiJsonOptions.Default)
                ?? throw new InvalidOperationException("La API no devolvió la transaccion creada.");
        }

        private static string ConstruirUrlFiltro(int cuentaId, TransaccionFiltroDto? filtro)
        {
            var parametros = new List<string>();

            if (filtro?.Desde is not null)
            {
                parametros.Add($"desde={filtro.Desde.Value:yyyy-MM-dd}");
            }

            if (filtro?.Hasta is not null)
            {
                parametros.Add($"hasta={filtro.Hasta.Value:yyyy-MM-dd}");
            }

            if (filtro?.Tipo is not null)
            {
                parametros.Add($"tipo={filtro.Tipo.Value}");
            }

            if (filtro?.CategoriaId is not null)
            {
                parametros.Add($"categoriaId={filtro.CategoriaId.Value}");
            }

            if (filtro?.Estado is not null)
            {
                parametros.Add($"estado={filtro.Estado.Value}");
            }

            var url = $"api/transacciones/cuenta/{cuentaId}";

            if (parametros.Count > 0)
            {
                url += "?" + string.Join("&", parametros);
            }

            return url;
        }

        public async Task<TransaccionDto> ActualizarAsync(int transaccionId, ActualizarTransaccionRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/transacciones/{transaccionId}", request, ApiJsonOptions.Default);

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<TransaccionDto>(ApiJsonOptions.Default)
                ?? throw new InvalidOperationException("La API no devolvió la transacción actualizada.");
        }

        public async Task AnularAsync(int transaccionId)
        {
            var response = await _httpClient.DeleteAsync($"api/transacciones/{transaccionId}");

            await ApiResponseHandler.EnsureSuccessAsync(response);
        }

        public async Task<ResumenFinancieroUsuarioDto?> ObtenerResumenUsuarioAsync(int usuarioId)
        {
            var response = await _httpClient.GetAsync($"api/transacciones/usuario/{usuarioId}/resumen");

            if(response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<ResumenFinancieroUsuarioDto>(ApiJsonOptions.Default);
        }
    }
}
