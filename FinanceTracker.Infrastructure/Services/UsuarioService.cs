using FinanceTracker.Application.DTOs.Usuario;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Services;

public class UsuarioService : IUsuarioService
{
    private readonly FinanceDbContext _dbContext;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuarioService(
        FinanceDbContext dbContext,
        IPasswordHasher<Usuario> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync()
    {
        return await _dbContext.Usuarios
            .Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                NombreUsuario = u.NombreUsuario,
                Email = u.Email,
                Activo = u.Activo
            })
            .ToListAsync();
    }

    public async Task<UsuarioDto?> ObtenerPorIdAsync(int usuarioId)
    {
        return await _dbContext.Usuarios
            .Where(u => u.UsuarioId == usuarioId)
            .Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                NombreUsuario = u.NombreUsuario,
                Email = u.Email,
                Activo = u.Activo
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UsuarioDto?> CrearAsync(CrearUsuarioRequest request)
    {
        var emailExiste = await _dbContext.Usuarios
            .IgnoreQueryFilters() //Aquí ignoramos el query global que tenemos en configurations de solo mostrar los activos, aquí si ocupamos saber de todos los usuarios hasta los inactivos
            .AnyAsync(u => u.Email == request.Email);

        if (emailExiste)
        {
            return null;
        }

        var usuario = new Usuario
        {
            NombreUsuario = request.NombreUsuario,
            Email = request.Email,
            Activo = true
        };

        usuario.ContrasenaHash = _passwordHasher.HashPassword(usuario, request.Contrasena);

        _dbContext.Usuarios.Add(usuario);
        await _dbContext.SaveChangesAsync();

        return new UsuarioDto
        {
            UsuarioId = usuario.UsuarioId,
            NombreUsuario = usuario.NombreUsuario,
            Email = usuario.Email,
            Activo = usuario.Activo
        };
    }

    public async Task<ActualizarUsuarioResult> ActualizarAsync(int usuarioId, ActualizarUsuarioRequest request)
    {
        var usuario = await _dbContext.Usuarios
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

        if (usuario is null)
        {
            return new ActualizarUsuarioResult
            {
                Encontrado = false
            };
        }

        var emailEnUso = await _dbContext.Usuarios
            .IgnoreQueryFilters()
            .AnyAsync(u =>
                u.Email == request.Email &&
                u.UsuarioId != usuarioId);

        if (emailEnUso)
        {
            return new ActualizarUsuarioResult
            {
                Encontrado = true,
                EmailEnUso = true
            };
        }

        usuario.NombreUsuario = request.NombreUsuario;
        usuario.Email = request.Email;

        await _dbContext.SaveChangesAsync();

        return new ActualizarUsuarioResult
        {
            Encontrado = true,
            Usuario = new UsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Activo = usuario.Activo
            }
        };
    }

    public async Task<bool> DesactivarAsync(int usuarioId)
    {

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var usuario = await _dbContext.Usuarios
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

            if (usuario is null)
                return false;

            var cuentas = await _dbContext.Cuentas
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();

            var categorias = await _dbContext.Categorias
                .Where(c => c.UsuarioId == usuarioId)
                .ToListAsync();

            usuario.Activo = false;

            foreach ( var cuenta in cuentas)
            {
                cuenta.Activo = false;
            }

            foreach (var categoria in categorias)
            {
                categoria.Activo = false;
            }

            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
