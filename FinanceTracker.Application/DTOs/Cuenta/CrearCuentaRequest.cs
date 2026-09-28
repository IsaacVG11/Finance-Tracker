namespace FinanceTracker.Application.DTOs.Cuenta;

public class CrearCuentaRequest
{
    public int UsuarioId { get; set; }
    public string NombreCuenta { get; set; } = string.Empty;
}
