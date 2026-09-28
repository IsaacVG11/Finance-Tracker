using FinanceTracker.Web.Models;

namespace FinanceTracker.Web.Services.Interfaces
{
    public interface ITransaccionApiService
    {
        Task<IEnumerable<TransaccionDto>> ObtenerPorCuentaAsync(int cuentaId, TransaccionFiltroDto? filtro = null);
        Task<TransaccionDto?> ObtenerPorIdAsync(int transaccionId);
        Task<SaldoCuentaDto?> ObtenerSaldoCuentaAsync(int cuentaId);
        Task<TransaccionDto> CrearAsync(CrearTransaccionRequest request);
        Task<TransaccionDto> ActualizarAsync(int transaccionId, ActualizarTransaccionRequest request);
        Task AnularAsync(int transaccionId);
        Task<ResumenFinancieroUsuarioDto?> ObtenerResumenUsuarioAsync(int usuarioId);
    }
}
