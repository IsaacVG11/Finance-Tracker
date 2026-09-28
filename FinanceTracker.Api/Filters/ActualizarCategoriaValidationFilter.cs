using FinanceTracker.Application.DTOs.Categoria;

namespace FinanceTracker.Api.Filters;

public class ActualizarCategoriaValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var request = context.GetArgument<ActualizarCategoriaRequest>(1);
        var errores = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.NombreCategoria))
            errores["nombreCategoria"] = ["El nombre de la categoría es obligatorio."];
        else if (request.NombreCategoria.Length > 100)
            errores["nombreCategoria"] = ["El nombre de la categoría no puede superar 100 caracteres."];

        if (errores.Count > 0)
            return Results.ValidationProblem(errores);

        return await next(context);
    }
}
