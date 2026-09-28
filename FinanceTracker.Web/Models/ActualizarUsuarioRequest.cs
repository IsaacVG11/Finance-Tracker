using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Web.Models
{
    public class ActualizarUsuarioRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no es válido.")]
        public string Email {  get; set; } = string.Empty;
    }
}
