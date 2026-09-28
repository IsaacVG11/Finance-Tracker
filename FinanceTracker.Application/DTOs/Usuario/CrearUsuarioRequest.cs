namespace FinanceTracker.Application.DTOs.Usuario;

public class CrearUsuarioRequest
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
}
