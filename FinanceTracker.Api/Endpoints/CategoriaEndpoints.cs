using FinanceTracker.Api.Filters;
using FinanceTracker.Application.DTOs.Categoria;
using FinanceTracker.Application.Interfaces;

namespace FinanceTracker.Api.Endpoints;

public static class CategoriaEndpoints
{
    public static RouteGroupBuilder MapCategoriaEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("/api/categorias")
            .WithTags("Categorias");

        group.MapGet("/usuario/{usuarioId:int}", async (
            int usuarioId,
            ICategoriaService categoriaService) =>
        {
            var categorias =
                await categoriaService.ObtenerPorUsuarioAsync(usuarioId);

            return Results.Ok(categorias);
        });

        group.MapGet("/{id:int}", async (
            int id,
            ICategoriaService categoriaService) =>
        {
            var categoria =
                await categoriaService.ObtenerPorIdAsync(id);

            return categoria is null
                ? Results.NotFound()
                : Results.Ok(categoria);
        });

        group.MapPost("", async (
            CrearCategoriaRequest request,
            ICategoriaService categoriaService) =>
        {
            var resultado = await categoriaService.CrearAsync(request);

            if (!resultado.UsuarioEncontrado)
            {
                return Results.NotFound(new
                {
                    mensaje = "El usuario no existe o está inactivo."
                });
            }

            if (resultado.CategoriaDuplicada)
            {
                return Results.Conflict(new
                {
                    mensaje = "Ya existe una categoría con ese nombre y tipo para el usuario."
                });
            }

            return Results.Created(
                $"/api/categorias/{resultado.Categoria!.CategoriaId}",
                resultado.Categoria);
        })
        .AddEndpointFilter<CrearCategoriaValidationFilter>();

        group.MapPut("/{id:int}", async (
            int id,
            ActualizarCategoriaRequest request,
            ICategoriaService categoriaService) =>
        {
            var resultado =
                await categoriaService.ActualizarAsync(id, request);

            if (!resultado.Encontrada)
                return Results.NotFound();

            if (resultado.CategoriaDuplicada)
            {
                return Results.Conflict(new
                {
                    mensaje = "Ya existe otra categoría con ese nombre y tipo para el usuario."
                });
            }

            return Results.Ok(resultado.Categoria);
        })
        .AddEndpointFilter<ActualizarCategoriaValidationFilter>();

        group.MapDelete("/{id:int}", async (
            int id,
            ICategoriaService categoriaService) =>
        {
            var eliminada = await categoriaService.DesactivarAsync(id);
            return eliminada
                ? Results.NoContent()
                : Results.NotFound();
        });

        return group;
    }
}
