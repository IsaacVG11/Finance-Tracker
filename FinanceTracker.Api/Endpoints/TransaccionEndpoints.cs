using FinanceTracker.Api.Filters;
using FinanceTracker.Application.DTOs.Transaccion;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Api.Endpoints;

public static class TransaccionEndpoints
{
    public static RouteGroupBuilder MapTransaccionEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("/api/transacciones")
            .WithTags("Transacciones");

        group.MapGet("/cuenta/{cuentaId:int}", async (int cuentaId,
                                                      DateOnly? desde,
                                                      DateOnly? hasta,
                                                      TipoTransaccion? tipo,
                                                      int? categoriaId,
                                                      EstadoTransaccion? estado,
                                                      ITransaccionService transaccionService) =>
        {
            if(desde.HasValue && hasta.HasValue && desde.Value > hasta.Value)
            {
                return Results.BadRequest(new
                {
                    mensaje = "La fecha 'desde' no puede ser posterior a la fecha 'hasta'"
                });
            }

            var filtro = new TransaccionFiltroDto
            {
                Desde = desde,
                Hasta = hasta,
                Tipo = tipo,
                CategoriaId = categoriaId,
                Estado = estado,
            };

            var transacciones = await transaccionService.ObtenerPorCuentaAsync(cuentaId, filtro);

            return Results.Ok(transacciones);
        });

        group.MapGet("/{id:int}", async (int id, ITransaccionService transaccionService) =>
        {
            var transaccion = await transaccionService.ObtenerPorIdAsync(id);

            return transaccion is null
                ? Results.NotFound()
                : Results.Ok(transaccion);
        });

        group.MapPost("", async (CrearTransaccionRequest request, ITransaccionService transaccionService) =>
        {
            var resultado = await transaccionService.CrearAsync(request);

            return resultado.Estado switch
            {
                EstadoTransaccionOperacion.CuentaNoEncontrada =>
                    Results.NotFound(new
                    {
                        mensaje = "La cuenta no existe o está inactiva."
                    }),

                EstadoTransaccionOperacion.CategoriaNoEncontrada =>
                    Results.NotFound(new
                    {
                        mensaje = "La categoría no existe o está inactiva."
                    }),

                EstadoTransaccionOperacion.UsuarioNoCoincide =>
                    Results.BadRequest(new
                    {
                        mensaje = "La cuenta y la categoría no pertenecen al mismo usuario."
                    }),

                EstadoTransaccionOperacion.TipoNoCoincide =>
                    Results.BadRequest(new
                    {
                        mensaje = "El tipo de la transacción no coincide con el tipo de la categoría."
                    }),

                _ => Results.Created($"/api/transacciones/{resultado.Transaccion!.TransaccionId}", resultado.Transaccion)
            };
        })
        .AddEndpointFilter<CrearTransaccionValidationFilter>();

        group.MapPut("/{transaccionId:int}", async (int transaccionId, ActualizarTransaccionRequest request, ITransaccionService transaccionService) =>
        {
            var resultado = await transaccionService.ActualizarAsync(transaccionId, request);

            return resultado.Estado switch
            {
                EstadoTransaccionOperacion.CuentaNoEncontrada =>
                    Results.NotFound(new
                    {
                        mensaje = "La cuenta no existe o está inactiva."
                    }),

                EstadoTransaccionOperacion.CategoriaNoEncontrada =>
                    Results.NotFound(new
                    {
                        mensaje = "La categoría no existe o está inactiva."
                    }),

                EstadoTransaccionOperacion.UsuarioNoCoincide =>
                    Results.BadRequest(new
                    {
                        mensaje = "La cuenta y la categoría no pertenecen al mismo usuario."
                    }),

                EstadoTransaccionOperacion.TipoNoCoincide =>
                    Results.BadRequest(new
                    {
                        mensaje = "El tipo de la transacción no coincide con el tipo de la categoría."
                    }),
                EstadoTransaccionOperacion.TransaccionNoEncontrada =>
                    Results.BadRequest(new
                    {
                        mensaje = "La transacción no existe"
                    }),
                EstadoTransaccionOperacion.TransaccionAnulada =>
                    Results.Conflict(new
                    {
                        mensaje = "Una transaccion anulada no puede modificarse"
                    }),

                _ => Results.Ok(resultado.Transaccion)
            };
        })
            .AddEndpointFilter<ActualizarTransaccionValidationFilter>();

        group.MapDelete("/{transaccionId:int}", async (int transaccionId, ITransaccionService transaccionService) =>
        {
            var estado = await transaccionService.AnularAsync(transaccionId);

            return estado switch
            {
                EstadoTransaccionOperacion.TransaccionNoEncontrada =>
                    Results.NotFound(new
                    {
                        mensaje = "La transaccion no existe"
                    }),

                EstadoTransaccionOperacion.TransaccionAnulada =>
                    Results.Conflict(new
                    {
                        mensaje = "La transaccion ya está anulada"
                    }),

                _ => Results.NoContent()
            };
        });

        group.MapGet("/cuenta/{cuentaId:int}/saldo", async (int cuentaId, ITransaccionService transaccionService) =>
        {
            var saldo = await transaccionService.ObtenerSaldoCuentaAsync(cuentaId);

            if(saldo is null)
            {
                return Results.NotFound(new
                {
                    mensaje = "La cuenta no existe o está inactiva"
                });
            }

            return Results.Ok(saldo);
        });

        group.MapGet("/usuario/{usuarioId:int}/resumen", async (int usuarioId, ITransaccionService transaccionService) =>
        {
            var resumen = await transaccionService.ObtenerResumenFinancieroUsuarioAsync(usuarioId);

            if(resumen is null)
            {
                return Results.NotFound(new
                {
                    mensaje = "El usuario no existe o está inactivo"
                });
            }

            return Results.Ok(resumen);
        });

        return group;
    }
}
