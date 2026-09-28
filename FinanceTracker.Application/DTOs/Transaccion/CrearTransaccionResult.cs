namespace FinanceTracker.Application.DTOs.Transaccion;

public class CrearTransaccionResult
{
    public EstadoTransaccionOperacion Estado { get; set; }
    public TransaccionDto? Transaccion { get; set; }
}
