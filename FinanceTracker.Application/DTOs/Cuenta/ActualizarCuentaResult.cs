namespace FinanceTracker.Application.DTOs.Cuenta;

public class ActualizarCuentaResult
{
    public bool Encontrada { get; set; }
    public bool NombreEnUso { get; set; }
    public CuentaDto? Cuenta { get; set; }
}
