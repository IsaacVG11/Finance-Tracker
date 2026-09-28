namespace FinanceTracker.Application.DTOs.Cuenta;

public class CuentaDto
{
    public int CuentaId { get; set; }
    public int UsuarioId { get; set; }
    public string NombreCuenta { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
