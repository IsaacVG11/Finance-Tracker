using FinanceTracker.Web.Models;
using FinanceTracker.Web.Services.Interfaces;
using FinanceTracker.Web.Services.Http;
using System.Net.Http.Json;
using System.Net;

namespace FinanceTracker.Web.Services.Implementations
{
    public class CuentaApiService : ICuentaApiService
    {
        private readonly HttpClient _httpClient;

        public CuentaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CuentaDto>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            var response = await _httpClient.GetAsync($"api/cuentas/usuario/{usuarioId}");

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<List<CuentaDto>>() ?? [];

        }

        public async Task<CuentaDto?> ObtenerPorIdAsync(int cuentaId)
        {
            var response = await _httpClient.GetAsync($"api/cuentas/{cuentaId}");

            if(response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<CuentaDto>();
        }

        public async Task<CuentaDto> CrearAsync(CrearCuentaRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/cuentas", request);

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<CuentaDto>() 
                ?? throw new InvalidOperationException("La API no devolvió la cuenta creada.");
        }

        public async Task<CuentaDto> ActualizarAsync(int cuentaId, ActualizarCuentaRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/cuentas/{cuentaId}", request);

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<CuentaDto>()
                ?? throw new InvalidOperationException("La API no devolvió la cuenta actualizada.");
        }

        public async Task DesactivarAsync(int cuentaId)
        {
            var response = await _httpClient.DeleteAsync($"api/cuentas/{cuentaId}");

            await ApiResponseHandler.EnsureSuccessAsync(response);
        }

    }
}
