using FinanceTracker.Api.Filters;
using FinanceTracker.Application.DTOs.Cuenta;
using FinanceTracker.Application.Interfaces;

namespace FinanceTracker.Api.Endpoints;

public static class CuentaEndpoints
{
    public static RouteGroupBuilder MapCuentaEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("/api/cuentas")
            .WithTags("Cuentas");

        group.MapGet("/usuario/{usuarioId:int}", async (
            int usuarioId,
            ICuentaService cuentaService) =>
        {
            var cuentas = await cuentaService.ObtenerPorUsuarioAsync(usuarioId);
            return Results.Ok(cuentas);
        });

        group.MapGet("/{id:int}", async (
            int id,
            ICuentaService cuentaService) =>
        {
            var cuenta = await cuentaService.ObtenerPorIdAsync(id);
            return cuenta is null
                ? Results.NotFound()
                : Results.Ok(cuenta);
        });

        group.MapPost("", async (
            CrearCuentaRequest request,
            ICuentaService cuentaService) =>
        {
            var resultado = await cuentaService.CrearAsync(request);

            if (!resultado.UsuarioEncontrado)
            {
                return Results.NotFound(new
                {
                    mensaje = "El usuario no existe o está inactivo."
                });
            }

            if (resultado.NombreEnUso)
            {
                return Results.Conflict(new
                {
                    mensaje = "Ya existe una cuenta con ese nombre para el usuario."
                });
            }

            return Results.Created(
                $"/api/cuentas/{resultado.Cuenta!.CuentaId}",
                resultado.Cuenta);
        })
        .AddEndpointFilter<CrearCuentaValidationFilter>();

        group.MapPut("/{id:int}", async (
            int id,
            ActualizarCuentaRequest request,
            ICuentaService cuentaService) =>
        {
            var resultado = await cuentaService.ActualizarAsync(id, request);

            if (!resultado.Encontrada)
                return Results.NotFound();

            if (resultado.NombreEnUso)
            {
                return Results.Conflict(new
                {
                    mensaje = "Ya existe otra cuenta con ese nombre para el usuario."
                });
            }

            return Results.Ok(resultado.Cuenta);
        })
        .AddEndpointFilter<ActualizarCuentaValidationFilter>();

        group.MapDelete("/{id:int}", async (
            int id,
            ICuentaService cuentaService) =>
        {
            var eliminada = await cuentaService.DesactivarAsync(id);
            return eliminada
                ? Results.NoContent()
                : Results.NotFound();
        });

        return group;
    }
}
