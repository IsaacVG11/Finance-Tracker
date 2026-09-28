using FinanceTracker.Web.Models.Enums;

namespace FinanceTracker.Web.Models
{
    public class TransaccionFiltroDto
    {
        public DateOnly? Desde { get; set; }
        public DateOnly? Hasta { get; set; }
        public TipoTransaccion? Tipo { get; set; }
        public int? CategoriaId { get; set; }
        public EstadoTransaccion? Estado { get; set; }
    }
}
