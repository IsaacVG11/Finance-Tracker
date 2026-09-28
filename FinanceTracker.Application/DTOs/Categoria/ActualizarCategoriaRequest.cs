using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs.Categoria;

public class ActualizarCategoriaRequest
{
    public string NombreCategoria { get; set; } = string.Empty;
}
