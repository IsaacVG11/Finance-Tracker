using FinanceTracker.Application.DTOs.Transaccion;

namespace FinanceTracker.Api.Filters;

public class CrearTransaccionValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var request = context.GetArgument<CrearTransaccionRequest>(0);
        var errores = new Dictionary<string, string[]>();

        if (request.CuentaId <= 0)
            errores["cuentaId"] = ["El cuentaId debe ser mayor que cero."];

        if (request.CategoriaId <= 0)
            errores["categoriaId"] = ["El categoriaId debe ser mayor que cero."];

        if (request.Cantidad <= 0)
            errores["cantidad"] = ["La cantidad debe ser mayor que cero."];

        if (string.IsNullOrWhiteSpace(request.Descripcion))
            errores["descripcion"] = ["La descripción es obligatoria."];
        else if (request.Descripcion.Length > 500)
            errores["descripcion"] = ["La descripción no puede superar 500 caracteres."];

        if (!Enum.IsDefined(request.Tipo))
            errores["tipo"] = ["El tipo de transacción no es válido."];

        if(request.FechaTransaccion == default)
            errores["fechaTransaccion"] = ["La fecha es obligatoria"];

        if (errores.Count > 0)
            return Results.ValidationProblem(errores);

        return await next(context);
    }
}
