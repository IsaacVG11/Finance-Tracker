namespace FinanceTracker.Domain.Entities;

public class Usuario
{
    public int UsuarioId { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ContrasenaHash { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }

    public ICollection<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
    public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
}
