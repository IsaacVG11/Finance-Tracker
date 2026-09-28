namespace FinanceTracker.Application.DTOs.Categoria;

public class CrearCategoriaResult
{
    public bool UsuarioEncontrado { get; set; }
    public bool CategoriaDuplicada { get; set; }
    public CategoriaDto? Categoria { get; set; }
}
