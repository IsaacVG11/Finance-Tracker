using System.ComponentModel.DataAnnotations;
using FinanceTracker.Web.Models.Enums;

namespace FinanceTracker.Web.Models
{
    public class CrearTransaccionRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta.")]
        public int CuentaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoria.")]
        public int CategoriaId { get; set; }

        [Range(typeof(decimal), "0.01", "9999999999999999.99", ErrorMessage = "La cantidad debe de ser mayor a 0.")]
        public decimal Cantidad { get; set; }

        public TipoTransaccion Tipo { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria.")]
        [MaxLength(500, ErrorMessage = "La descripcion no puede superar los 500 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        public DateOnly FechaTransaccion { get; set; }
    }
}
