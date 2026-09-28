using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Web.Models
{
    public class CrearCuentaRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar usuario.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre de la cuenta es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre de la cuenta no puede superar los 50 caracteres.")]
        public string NombreCuenta { get; set; } = string.Empty; 
    }
}
