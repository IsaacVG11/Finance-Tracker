using FinanceTracker.Application.DTOs.Usuario;

namespace FinanceTracker.Api.Filters;

public class ActualizarUsuarioValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var request = context.GetArgument<ActualizarUsuarioRequest>(1);
        var errores = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.NombreUsuario))
            errores["nombreUsuario"] = ["El nombre de usuario es obligatorio."];

        if (string.IsNullOrWhiteSpace(request.Email))
            errores["email"] = ["El email es obligatorio."];
        else if (!System.Net.Mail.MailAddress.TryCreate(request.Email, out _))
            errores["email"] = ["El formato del email no es válido."];

        if (errores.Count > 0)
            return Results.ValidationProblem(errores);

        return await next(context);
    }
}
