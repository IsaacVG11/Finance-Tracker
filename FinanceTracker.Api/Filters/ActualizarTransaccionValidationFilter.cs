using FinanceTracker.Application.DTOs.Transaccion;

namespace FinanceTracker.Api.Filters
{
    public class ActualizarTransaccionValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var request = context.GetArgument<ActualizarTransaccionRequest>(1);

            var errores = new Dictionary<string, string[]>();

            if(request.CuentaId < 0)
            {
                errores["cuentaId"] = ["La cuenta Id debe ser mayor a 0"];
            }

            if (request.CategoriaId < 0)
            {
                errores["categoriaId"] = ["La categoria Id debe ser mayor a 0"];
            }

            if(request.Cantidad <= 0)
            {
                errores["cantidad"] = ["La cantidad ingresada debe de ser mayor a 0"];
            }

            if (string.IsNullOrWhiteSpace(request.Descripcion))
            {
                errores["descripcion"] = ["La descripcion es obligatoria"];
            }

            if(request.Descripcion.Length > 500)
            {
                errores["descripcion"] = ["La descripcion no puede superar los 500 caracteres"];
            }

            if (!Enum.IsDefined(request.Tipo))
            {
                errores["tipo"] = ["El tipo de transaccion no es valido"];
            }

            if(errores.Count > 0)
            {
                return Results.ValidationProblem(errores);
            }

            return await next(context);
        }
    }
}
