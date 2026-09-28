using FinanceTracker.Web.Models;

namespace FinanceTracker.Web.Services.Interfaces
{
    public interface ICuentaApiService
    {
        Task<IEnumerable<CuentaDto>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<CuentaDto?> ObtenerPorIdAsync(int cuentaId);
        Task<CuentaDto> CrearAsync(CrearCuentaRequest request);
        Task<CuentaDto> ActualizarAsync(int cuentaId, ActualizarCuentaRequest request);
        Task DesactivarAsync(int cuentaId);
    }
}
