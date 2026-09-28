namespace FinanceTracker.Application.DTOs.Cuenta;

public class CrearCuentaResult
{
    public bool UsuarioEncontrado { get; set; }
    public bool NombreEnUso { get; set; }
    public CuentaDto? Cuenta { get; set; }
}
