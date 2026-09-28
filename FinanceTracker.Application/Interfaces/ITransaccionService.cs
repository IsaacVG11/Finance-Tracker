using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.DTOs.Transaccion;

namespace FinanceTracker.Application.Interfaces;

public interface ITransaccionService
{
    Task<IEnumerable<TransaccionDto>> ObtenerPorCuentaAsync(int cuentaId, TransaccionFiltroDto filtro);
    Task<TransaccionDto?> ObtenerPorIdAsync(int transaccionId);
    Task<CrearTransaccionResult> CrearAsync(CrearTransaccionRequest request);
    Task<ActualizarTransaccionResult> ActualizarAsync(int transaccionId, ActualizarTransaccionRequest request);
    Task<EstadoTransaccionOperacion> AnularAsync(int transaccionId);
    Task<SaldoCuentaDto?> ObtenerSaldoCuentaAsync(int cuentaId);
    Task<ResumenFinancieroUsuarioDto?> ObtenerResumenFinancieroUsuarioAsync(int usuarioId);
}
