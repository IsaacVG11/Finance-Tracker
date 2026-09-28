using System.ComponentModel.DataAnnotations;
using FinanceTracker.Web.Models.Enums;

namespace FinanceTracker.Web.Models;

public class CrearCategoriaRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un usuario.")]
    public int UsuarioId { get; set; }

    public TipoTransaccion Tipo { get; set; }

    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    public string NombreCategoria { get; set; } = string.Empty;
}
