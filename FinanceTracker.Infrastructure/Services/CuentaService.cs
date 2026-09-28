using FinanceTracker.Application.DTOs.Cuenta;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Services;

public class CuentaService : ICuentaService
{
    private readonly FinanceDbContext _dbContext;

    public CuentaService(FinanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<CuentaDto>> ObtenerPorUsuarioAsync(int usuarioId)
    {
        return await _dbContext.Cuentas
            .Where(c => c.UsuarioId == usuarioId)
            .Select(c => new CuentaDto
            {
                CuentaId = c.CuentaId,
                UsuarioId = c.UsuarioId,
                NombreCuenta = c.NombreCuenta,
                Activo = c.Activo
            })
            .ToListAsync();
    }

    public async Task<CuentaDto?> ObtenerPorIdAsync(int cuentaId)
    {
        return await _dbContext.Cuentas
            .Where(c => c.CuentaId == cuentaId)
            .Select(c => new CuentaDto
            {
                CuentaId = c.CuentaId,
                UsuarioId = c.UsuarioId,
                NombreCuenta = c.NombreCuenta,
                Activo = c.Activo
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CrearCuentaResult> CrearAsync(CrearCuentaRequest request)
    {
        var usuarioExiste = await _dbContext.Usuarios
            .AnyAsync(u => u.UsuarioId == request.UsuarioId);

        if (!usuarioExiste)
        {
            return new CrearCuentaResult
            {
                UsuarioEncontrado = false
            };
        }

        var nombreEnUso = await _dbContext.Cuentas
            .IgnoreQueryFilters()
            .AnyAsync(c =>
                c.UsuarioId == request.UsuarioId &&
                c.NombreCuenta == request.NombreCuenta);

        if (nombreEnUso)
        {
            return new CrearCuentaResult
            {
                UsuarioEncontrado = true,
                NombreEnUso = true
            };
        }

        var cuenta = new Cuenta
        {
            UsuarioId = request.UsuarioId,
            NombreCuenta = request.NombreCuenta,
            Activo = true
        };

        _dbContext.Cuentas.Add(cuenta);
        await _dbContext.SaveChangesAsync();

        return new CrearCuentaResult
        {
            UsuarioEncontrado = true,
            Cuenta = new CuentaDto
            {
                CuentaId = cuenta.CuentaId,
                UsuarioId = cuenta.UsuarioId,
                NombreCuenta = cuenta.NombreCuenta,
                Activo = cuenta.Activo
            }
        };
    }

    public async Task<ActualizarCuentaResult> ActualizarAsync(int cuentaId, ActualizarCuentaRequest request)
    {
        var cuenta = await _dbContext.Cuentas
            .FirstOrDefaultAsync(c => c.CuentaId == cuentaId);

        if (cuenta is null)
        {
            return new ActualizarCuentaResult
            {
                Encontrada = false
            };
        }

        var nombreEnUso = await _dbContext.Cuentas
            .IgnoreQueryFilters()
            .AnyAsync(c =>
                c.UsuarioId == cuenta.UsuarioId &&
                c.NombreCuenta == request.NombreCuenta &&
                c.CuentaId != cuentaId);

        if (nombreEnUso)
        {
            return new ActualizarCuentaResult
            {
                Encontrada = true,
                NombreEnUso = true
            };
        }

        cuenta.NombreCuenta = request.NombreCuenta;
        await _dbContext.SaveChangesAsync();

        return new ActualizarCuentaResult
        {
            Encontrada = true,
            Cuenta = new CuentaDto
            {
                CuentaId = cuenta.CuentaId,
                UsuarioId = cuenta.UsuarioId,
                NombreCuenta = cuenta.NombreCuenta,
                Activo = cuenta.Activo
            }
        };
    }

    public async Task<bool> DesactivarAsync(int cuentaId)
    {
        var cuenta = await _dbContext.Cuentas
            .FirstOrDefaultAsync(c => c.CuentaId == cuentaId);

        if (cuenta is null)
        {
            return false;
        }

        cuenta.Activo = false;
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
