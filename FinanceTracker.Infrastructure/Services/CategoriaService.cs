using FinanceTracker.Application.DTOs.Categoria;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Services;

public class CategoriaService : ICategoriaService
{
    private readonly FinanceDbContext _dbContext;

    public CategoriaService(FinanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CategoriaDto>> ObtenerPorUsuarioAsync(int usuarioId)
    {
        return await _dbContext.Categorias
            .Where(c => c.UsuarioId == usuarioId)
            .Select(c => new CategoriaDto
            {
                CategoriaId = c.CategoriaId,
                UsuarioId = c.UsuarioId,
                Tipo = c.Tipo,
                NombreCategoria = c.NombreCategoria,
                Activo = c.Activo
            })
            .ToListAsync();
    }

    public async Task<CategoriaDto?> ObtenerPorIdAsync(int categoriaId)
    {
        return await _dbContext.Categorias
            .Where(c => c.CategoriaId == categoriaId)
            .Select(c => new CategoriaDto
            {
                CategoriaId = c.CategoriaId,
                UsuarioId = c.UsuarioId,
                Tipo = c.Tipo,
                NombreCategoria = c.NombreCategoria,
                Activo = c.Activo
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CrearCategoriaResult> CrearAsync(CrearCategoriaRequest request)
    {
        var usuarioExiste = await _dbContext.Usuarios
            .AnyAsync(u => u.UsuarioId == request.UsuarioId);

        if (!usuarioExiste)
        {
            return new CrearCategoriaResult
            {
                UsuarioEncontrado = false
            };
        }

        var categoriaDuplicada = await _dbContext.Categorias
            .IgnoreQueryFilters()
            .AnyAsync(c =>
                c.UsuarioId == request.UsuarioId &&
                c.NombreCategoria == request.NombreCategoria &&
                c.Tipo == request.Tipo);

        if (categoriaDuplicada)
        {
            return new CrearCategoriaResult
            {
                UsuarioEncontrado = true,
                CategoriaDuplicada = true
            };
        }

        var categoria = new Categoria
        {
            UsuarioId = request.UsuarioId,
            Tipo = request.Tipo,
            NombreCategoria = request.NombreCategoria,
            Activo = true
        };

        _dbContext.Categorias.Add(categoria);
        await _dbContext.SaveChangesAsync();

        return new CrearCategoriaResult
        {
            UsuarioEncontrado = true,
            Categoria = new CategoriaDto
            {
                CategoriaId = categoria.CategoriaId,
                UsuarioId = categoria.UsuarioId,
                Tipo = categoria.Tipo,
                NombreCategoria = categoria.NombreCategoria,
                Activo = categoria.Activo
            }
        };
    }

    public async Task<ActualizarCategoriaResult> ActualizarAsync(int categoriaId, ActualizarCategoriaRequest request)
    {
        var categoria = await _dbContext.Categorias
            .FirstOrDefaultAsync(c => c.CategoriaId == categoriaId);

        if (categoria is null)
        {
            return new ActualizarCategoriaResult
            {
                Encontrada = false
            };
        }

        var categoriaDuplicada = await _dbContext.Categorias
            .IgnoreQueryFilters()
            .AnyAsync(c =>
                c.UsuarioId == categoria.UsuarioId &&
                c.NombreCategoria == request.NombreCategoria &&
                c.Tipo == categoria.Tipo &&
                c.CategoriaId != categoriaId);

        if (categoriaDuplicada)
        {
            return new ActualizarCategoriaResult
            {
                Encontrada = true,
                CategoriaDuplicada = true
            };
        }

        categoria.NombreCategoria = request.NombreCategoria;

        await _dbContext.SaveChangesAsync();

        return new ActualizarCategoriaResult
        {
            Encontrada = true,
            Categoria = new CategoriaDto
            {
                CategoriaId = categoria.CategoriaId,
                UsuarioId = categoria.UsuarioId,
                Tipo = categoria.Tipo,
                NombreCategoria = categoria.NombreCategoria,
                Activo = categoria.Activo
            }
        };
    }

    public async Task<bool> DesactivarAsync(int categoriaId)
    {
        var categoria = await _dbContext.Categorias
            .FirstOrDefaultAsync(c => c.CategoriaId == categoriaId);

        if (categoria is null)
        {
            return false;
        }

        categoria.Activo = false;
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
