using FinanceTracker.Application.DTOs.Usuario;

namespace FinanceTracker.Api.Filters;

public class CrearUsuarioValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var request = context.GetArgument<CrearUsuarioRequest>(0);
        var errores = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.NombreUsuario))
            errores["nombreUsuario"] = ["El nombre de usuario es obligatorio."];

        if (string.IsNullOrWhiteSpace(request.Email))
            errores["email"] = ["El email es obligatorio."];
        else if (!System.Net.Mail.MailAddress.TryCreate(request.Email, out _))
            errores["email"] = ["El formato del email no es válido."];

        if (string.IsNullOrWhiteSpace(request.Contrasena))
            errores["contrasena"] = ["La contraseña es obligatoria."];
        else if (request.Contrasena.Length < 8)
            errores["contrasena"] = ["La contraseña debe tener al menos 8 caracteres."];

        if (errores.Count > 0)
            return Results.ValidationProblem(errores);

        return await next(context);
    }
}
