using FinanceTracker.Web.Models;
using FinanceTracker.Web.Services.Interfaces;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using FinanceTracker.Web.Services.Http;

namespace FinanceTracker.Web.Services.Implementations
{
    public class UsuarioApiService : IUsuarioApiService
    {
        private readonly HttpClient _httpClient;

        public UsuarioApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync()
        {
            var usuarios = await _httpClient.GetFromJsonAsync<List<UsuarioDto>>("api/usuarios");

            return usuarios ?? [];
        }

        public async Task<UsuarioDto?> ObtenerUsuarioPorIdAsync(int usuarioId)
        {
            var response = await _httpClient.GetAsync($"api/usuarios/{usuarioId}");

            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<UsuarioDto>();
        }

        public async Task<UsuarioDto> CrearUsuarioAsync(CrearUsuarioRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/usuarios", request);

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<UsuarioDto>()
                ?? throw new InvalidOperationException("La API no devolvió el usuario creado");
        }

        public async Task<UsuarioDto> ActualizarUsuarioAsync(int usuarioId, ActualizarUsuarioRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/usuarios/{usuarioId}", request);

            await ApiResponseHandler.EnsureSuccessAsync(response);

            return await response.Content.ReadFromJsonAsync<UsuarioDto>()
                ?? throw new InvalidOperationException("La API no devolvió usuario actualizado");
        }

        public async Task DesactivarUsuarioAsync(int usuarioId)
        {
            var response = await _httpClient.DeleteAsync($"api/usuarios({usuarioId}");

            await ApiResponseHandler.EnsureSuccessAsync(response);
        }
    }
}
