using FinanceTracker.Web.Models;

namespace FinanceTracker.Web.Services.Interfaces
{
    public interface ICategoriaApiService
    {
        Task<IEnumerable<CategoriaDto>>ObtenerPorUsuarioAsync(int usuarioId);
        Task<CategoriaDto?> ObtenerPorIdAsync(int categoriaId);
        Task<CategoriaDto> CrearAsync(CrearCategoriaRequest request);
        Task<CategoriaDto> ActualizarAsync(int categoriaId, ActualizarCategoriaRequest request);
        Task DesactivarAsync(int categoriaId);
    }
}
