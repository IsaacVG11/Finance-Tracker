using FinanceTracker.Application.DTOs.Cuenta;

namespace FinanceTracker.Api.Filters;

public class CrearCuentaValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var request = context.GetArgument<CrearCuentaRequest>(0);
        var errores = new Dictionary<string, string[]>();

        if (request.UsuarioId <= 0)
            errores["usuarioId"] = ["El usuarioId debe ser mayor que cero."];

        if (string.IsNullOrWhiteSpace(request.NombreCuenta))
            errores["nombreCuenta"] = ["El nombre de la cuenta es obligatorio."];
        else if (request.NombreCuenta.Length > 50)
            errores["nombreCuenta"] = ["El nombre de la cuenta no puede superar 50 caracteres."];

        if (errores.Count > 0)
            return Results.ValidationProblem(errores);

        return await next(context);
    }
}
