using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Categoria
{
    public int CategoriaId { get; set; }
    public int UsuarioId { get; set; }
    public TipoTransaccion Tipo { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }

    public Usuario Usuario { get; set; } = null!;
    public ICollection<Transaccion> Transacciones { get; set; } =
        new List<Transaccion>();
}
