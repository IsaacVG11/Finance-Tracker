using FinanceTracker.Application.DTOs.Categoria;

namespace FinanceTracker.Application.Interfaces;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaDto>> ObtenerPorUsuarioAsync(int usuarioId);
    Task<CategoriaDto?> ObtenerPorIdAsync(int categoriaId);
    Task<CrearCategoriaResult> CrearAsync(CrearCategoriaRequest request);
    Task<ActualizarCategoriaResult> ActualizarAsync(int categoriaId, ActualizarCategoriaRequest request);
    Task<bool> DesactivarAsync(int categoriaId);
}
