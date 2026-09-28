using FinanceTracker.Application.DTOs.Cuenta;

namespace FinanceTracker.Application.Interfaces;

public interface ICuentaService
{
    Task<IEnumerable<CuentaDto>> ObtenerPorUsuarioAsync(int usuarioId);
    Task<CuentaDto?> ObtenerPorIdAsync(int cuentaId);
    Task<CrearCuentaResult> CrearAsync(CrearCuentaRequest request);
    Task<ActualizarCuentaResult> ActualizarAsync(int cuentaId, ActualizarCuentaRequest request);
    Task<bool> DesactivarAsync(int cuentaId);
}
