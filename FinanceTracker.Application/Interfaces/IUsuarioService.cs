using FinanceTracker.Application.DTOs.Usuario;

namespace FinanceTracker.Application.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync();
    Task<UsuarioDto?> ObtenerPorIdAsync(int usuarioId);
    Task<UsuarioDto?> CrearAsync(CrearUsuarioRequest request);
    Task<ActualizarUsuarioResult> ActualizarAsync(int usuarioId, ActualizarUsuarioRequest request);
    Task<bool> DesactivarAsync(int usuarioId);
}
