using FinanceTracker.Web.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Web.Models
{
    public class ActualizarTransaccionRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta.")]
        public int CuentaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría.")]
        public int CategoriaId { get; set; }

        [Range(typeof(decimal), "0.01", "9999999999999999.99", ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public decimal Cantidad { get; set; }

        public TipoTransaccion Tipo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [MaxLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        public DateOnly FechaTransaccion { get; set; }
    }
}
