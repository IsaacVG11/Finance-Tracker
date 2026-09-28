namespace FinanceTracker.Application.DTOs.Transaccion;

public class ActualizarTransaccionResult
{
    public EstadoTransaccionOperacion Estado { get; set; }
    public TransaccionDto? Transaccion { get; set; }
}
