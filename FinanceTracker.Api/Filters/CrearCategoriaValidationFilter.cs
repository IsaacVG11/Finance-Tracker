using FinanceTracker.Application.DTOs.Categoria;

namespace FinanceTracker.Api.Filters;

public class CrearCategoriaValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var request = context.GetArgument<CrearCategoriaRequest>(0);
        var errores = new Dictionary<string, string[]>();

        if (request.UsuarioId <= 0)
            errores["usuarioId"] = ["El usuarioId debe ser mayor que cero."];

        if (string.IsNullOrWhiteSpace(request.NombreCategoria))
            errores["nombreCategoria"] = ["El nombre de la categoría es obligatorio."];
        else if (request.NombreCategoria.Length > 100)
            errores["nombreCategoria"] = ["El nombre de la categoría no puede superar 100 caracteres."];

        if (!Enum.IsDefined(request.Tipo))
            errores["tipo"] = ["El tipo de transacción no es válido."];

        if (errores.Count > 0)
            return Results.ValidationProblem(errores);

        return await next(context);
    }
}
