namespace FinanceTracker.Application.DTOs.Usuario;

public class ActualizarUsuarioResult
{
    public bool Encontrado { get; set; }
    public bool EmailEnUso { get; set; }
    public UsuarioDto? Usuario { get; set; }
}
