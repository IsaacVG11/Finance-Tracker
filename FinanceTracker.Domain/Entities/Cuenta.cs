namespace FinanceTracker.Domain.Entities;

public class Cuenta
{
    public int CuentaId { get; set; }
    public int UsuarioId { get; set; }
    public string NombreCuenta { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }

    public Usuario Usuario { get; set; } = null!;
    public ICollection<Transaccion> Transacciones { get; set; } =
        new List<Transaccion>();
}
