using FinanceTracker.Api.Filters;
using FinanceTracker.Application.DTOs.Usuario;
using FinanceTracker.Application.Interfaces;

namespace FinanceTracker.Api.Endpoints;

public static class UsuarioEndpoints
{
    public static RouteGroupBuilder MapUsuarioEndpoints(
        this IEndpointRouteBuilder routes)
    {
        var group = routes
            .MapGroup("/api/usuarios")
            .WithTags("Usuarios");

        group.MapGet("", async (IUsuarioService usuarioService) =>
        {
            var usuarios = await usuarioService.ObtenerTodosAsync();
            return Results.Ok(usuarios);
        });

        group.MapGet("/{id:int}", async (
            int id,
            IUsuarioService usuarioService) =>
        {
            var usuario = await usuarioService.ObtenerPorIdAsync(id);
            return usuario is null
                ? Results.NotFound()
                : Results.Ok(usuario);
        });

        group.MapPost("", async (
            CrearUsuarioRequest request,
            IUsuarioService usuarioService) =>
        {
            var usuario = await usuarioService.CrearAsync(request);

            if (usuario is null)
            {
                return Results.Conflict(new
                {
                    mensaje = "Ya existe un usuario con ese email."
                });
            }

            return Results.Created(
                $"/api/usuarios/{usuario.UsuarioId}",
                usuario);
        })
        .AddEndpointFilter<CrearUsuarioValidationFilter>();

        group.MapPut("/{id:int}", async (
            int id,
            ActualizarUsuarioRequest request,
            IUsuarioService usuarioService) =>
        {
            var resultado = await usuarioService.ActualizarAsync(id, request);

            if (!resultado.Encontrado)
                return Results.NotFound();

            if (resultado.EmailEnUso)
            {
                return Results.Conflict(new
                {
                    mensaje = "Ya existe otro usuario con ese email."
                });
            }

            return Results.Ok(resultado.Usuario);
        })
        .AddEndpointFilter<ActualizarUsuarioValidationFilter>();

        group.MapDelete("/{id:int}", async (
            int id,
            IUsuarioService usuarioService) =>
        {
            var eliminado = await usuarioService.DesactivarAsync(id);
            return eliminado
                ? Results.NoContent()
                : Results.NotFound();
        });

        return group;
    }
}
