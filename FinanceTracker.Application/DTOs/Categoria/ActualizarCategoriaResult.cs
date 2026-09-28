namespace FinanceTracker.Application.DTOs.Categoria;

public class ActualizarCategoriaResult
{
    public bool Encontrada { get; set; }
    public bool CategoriaDuplicada { get; set; }
    public CategoriaDto? Categoria { get; set; }
}
