using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs.Categoria;

public class CategoriaDto
{
    public int CategoriaId { get; set; }
    public int UsuarioId { get; set; }
    public TipoTransaccion Tipo { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
