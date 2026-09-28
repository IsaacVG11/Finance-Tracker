using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs.Categoria;

public class CrearCategoriaRequest
{
    public int UsuarioId { get; set; }
    public TipoTransaccion Tipo { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
}
