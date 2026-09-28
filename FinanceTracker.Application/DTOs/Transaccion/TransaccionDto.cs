using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs.Transaccion;

public class TransaccionDto
{
    public int TransaccionId { get; set; }
    public int CuentaId { get; set; }
    public int CategoriaId { get; set; }
    public decimal Cantidad { get; set; }
    public TipoTransaccion Tipo { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public DateOnly FechaTransaccion { get; set; }
    public EstadoTransaccion Estado { get; set; }
    public DateTime? FechaAnulacion {  get; set; }
}
