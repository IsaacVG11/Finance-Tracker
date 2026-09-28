using FinanceTracker.Application.DTOs.Cuenta;

namespace FinanceTracker.Api.Filters;

public class ActualizarCuentaValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var request = context.GetArgument<ActualizarCuentaRequest>(1);
        var errores = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.NombreCuenta))
            errores["nombreCuenta"] = ["El nombre de la cuenta es obligatorio."];
        else if (request.NombreCuenta.Length > 50)
            errores["nombreCuenta"] = ["El nombre de la cuenta no puede superar 50 caracteres."];

        if (errores.Count > 0)
            return Results.ValidationProblem(errores);

        return await next(context);
    }
}
