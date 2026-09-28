using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Web.Models
{
    public class ActualizarCuentaRequest
    {
        [Required(ErrorMessage = "El nombre de la cuenta es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string NombreCuenta { get; set; } = string.Empty;
    }
}
