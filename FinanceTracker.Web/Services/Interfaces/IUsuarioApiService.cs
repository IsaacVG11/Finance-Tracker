using FinanceTracker.Web.Models;

namespace FinanceTracker.Web.Services.Interfaces
{
    public interface IUsuarioApiService
    {
        Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync();
        Task<UsuarioDto?> ObtenerUsuarioPorIdAsync(int usuarioId);
        Task<UsuarioDto> CrearUsuarioAsync(CrearUsuarioRequest request);
        Task<UsuarioDto> ActualizarUsuarioAsync(int usuarioId, ActualizarUsuarioRequest request);
        Task DesactivarUsuarioAsync(int usuarioId);
    }
}
