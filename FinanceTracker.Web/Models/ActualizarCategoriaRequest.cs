using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Web.Models;

public class ActualizarCategoriaRequest
{
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    public string NombreCategoria { get; set; } = string.Empty;
}
