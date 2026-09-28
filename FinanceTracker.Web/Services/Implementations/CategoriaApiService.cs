using System.Net;
using System.Net.Http.Json;
using FinanceTracker.Web.Models;
using FinanceTracker.Web.Services.Interfaces;
using FinanceTracker.Web.Services.Http;

namespace FinanceTracker.Web.Services.Implementations
{
    public class CategoriaApiService :ICategoriaApiService
    {
        private readonly HttpClient _httpClient;

        public CategoriaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CategoriaDto>>ObtenerPorUsuarioAsync(int usuarioId)
        {
            var response = await _httpClient.GetAsync($"api/categorias/usuario/{usuarioId}");

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<List<CategoriaDto>>(ApiJsonOptions.Default) ?? [];
        }

        public async Task<CategoriaDto?> ObtenerPorIdAsync(int categoriaId)
        {
            var response = await _httpClient.GetAsync($"api/categorias/{categoriaId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<CategoriaDto>(ApiJsonOptions.Default);
        }

        public async Task<CategoriaDto> CrearAsync(CrearCategoriaRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/categorias", request, ApiJsonOptions.Default);

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<CategoriaDto>(ApiJsonOptions.Default)
                ?? throw new InvalidOperationException("La API no devolvió la categoría creada.");
        }

        public async Task<CategoriaDto> ActualizarAsync(int categoriaId,ActualizarCategoriaRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/categorias/{categoriaId}", request, ApiJsonOptions.Default);

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<CategoriaDto>(ApiJsonOptions.Default)
                ?? throw new InvalidOperationException("La API no devolvió la categoría actualizada.");
        }

        public async Task DesactivarAsync(int categoriaId)
        {
            var response = await _httpClient.DeleteAsync($"api/categorias/{categoriaId}");

            await ApiResponseHandler.EnsureSuccessAsync(response);
        }
    }
}
