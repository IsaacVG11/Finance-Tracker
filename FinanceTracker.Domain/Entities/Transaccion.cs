using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Transaccion
{
    public int TransaccionId { get; set; }
    public int CuentaId { get; set; }
    public int CategoriaId { get; set; }
    public decimal Cantidad { get; set; }
    public TipoTransaccion Tipo { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public DateOnly FechaTransaccion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public EstadoTransaccion Estado {  get; set; }
    public DateTime? FechaAnulacion { get; set; }

    public Cuenta Cuenta { get; set; } = null!;
    public Categoria Categoria { get; set; } = null!;
}
