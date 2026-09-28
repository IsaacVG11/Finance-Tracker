using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using FinanceTracker.Domain.Enums;
using System.Transactions;
using System.Linq.Expressions;
using FinanceTracker.Application.DTOs.Transaccion;

namespace FinanceTracker.Infrastructure.Services;

public class TransaccionService : ITransaccionService
{
    private readonly FinanceDbContext _dbContext;

    public TransaccionService(FinanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private async Task<EstadoTransaccionOperacion> ValidarRelacionesAsync(int cuentaId, int categoriaId, TipoTransaccion tipo)
    {
        var cuenta = await _dbContext.Cuentas
            .FirstOrDefaultAsync(c => c.CuentaId == cuentaId);

        if(cuenta is null)
        {
            return EstadoTransaccionOperacion.CuentaNoEncontrada;
        }

        var categoria = await _dbContext.Categorias
            .FirstOrDefaultAsync(c => c.CategoriaId == categoriaId);

        if (categoria is null)
        {
            return EstadoTransaccionOperacion.CategoriaNoEncontrada;
        }

        if(cuenta.UsuarioId != categoria.UsuarioId)
        {
            return EstadoTransaccionOperacion.UsuarioNoCoincide;
        }

        if(categoria.Tipo != tipo)
        {
            return EstadoTransaccionOperacion.TipoNoCoincide;
        }

        return EstadoTransaccionOperacion.Exitoso;
    }

    public async Task<IEnumerable<TransaccionDto>> ObtenerPorCuentaAsync(int cuentaId, TransaccionFiltroDto filtro)
    {
        var query = _dbContext.Transacciones
            .Where(t => t.CuentaId == cuentaId)
            .AsQueryable();

        if (filtro.Desde.HasValue)
        {
            query = query.Where(t => t.FechaTransaccion ==  filtro.Desde.Value);
        }

        if (filtro.Hasta.HasValue)
        {
            query = query.Where(t => t.FechaTransaccion == filtro.Hasta.Value);
        }

        if (filtro.Tipo.HasValue)
        {
            query = query.Where(t => t.Tipo == filtro.Tipo.Value);
        }

        if (filtro.CategoriaId.HasValue)
        {
            query = query.Where(t => t.CategoriaId ==  filtro.CategoriaId.Value);
        }

        if (filtro.Estado.HasValue)
        {
            query = query.Where(t => t.Estado == filtro.Estado.Value);
        }

        return await query
            .OrderByDescending(t => t.FechaTransaccion)
            .ThenByDescending(t => t.TransaccionId)
            .Select(t => new TransaccionDto
            {
                TransaccionId = t.TransaccionId,
                CuentaId = t.CuentaId,
                CategoriaId = t.CategoriaId,
                Cantidad = t.Cantidad,
                Tipo = t.Tipo,
                Descripcion = t.Descripcion,
                FechaTransaccion = t.FechaTransaccion,
                Estado = t.Estado,
                FechaAnulacion = t.FechaAnulacion
            })
            .ToListAsync();
    }

    public async Task<TransaccionDto?> ObtenerPorIdAsync(int transaccionId)
    {
        return await _dbContext.Transacciones
            .Where(t => t.TransaccionId == transaccionId)
            .Select(t => new TransaccionDto
            {
                TransaccionId = t.TransaccionId,
                CuentaId = t.CuentaId,
                CategoriaId = t.CategoriaId,
                Cantidad = t.Cantidad,
                Tipo = t.Tipo,
                Descripcion = t.Descripcion,
                FechaTransaccion = t.FechaTransaccion,
                Estado = t.Estado,
                FechaAnulacion = t.FechaAnulacion
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CrearTransaccionResult> CrearAsync(CrearTransaccionRequest request)
    {

        var estado = await ValidarRelacionesAsync(request.CuentaId, request.CategoriaId, request.Tipo);

        if(estado != EstadoTransaccionOperacion.Exitoso)
        {
            return new CrearTransaccionResult
            {
                Estado = estado
            };
        }

        var transaccion = new Transaccion
        {
            CuentaId = request.CuentaId,
            CategoriaId = request.CategoriaId,
            Cantidad = request.Cantidad,
            Tipo = request.Tipo,
            Descripcion = request.Descripcion,
            FechaTransaccion = request.FechaTransaccion,
            Estado = EstadoTransaccion.Activa
        };

        _dbContext.Transacciones.Add(transaccion);
        await _dbContext.SaveChangesAsync();

        return new CrearTransaccionResult
        {
            Estado = EstadoTransaccionOperacion.Exitoso,
            Transaccion = new TransaccionDto
            {
                TransaccionId = transaccion.TransaccionId,
                CuentaId = transaccion.CuentaId,
                CategoriaId = transaccion.CategoriaId,
                Cantidad = transaccion.Cantidad,
                Tipo = transaccion.Tipo,
                Descripcion = transaccion.Descripcion,
                FechaTransaccion = transaccion.FechaTransaccion,
                Estado = transaccion.Estado,
                FechaAnulacion = transaccion.FechaAnulacion
            }
        };
    }

    public async Task<ActualizarTransaccionResult> ActualizarAsync(int transaccionId, ActualizarTransaccionRequest request)
    {
        var transaccion = await _dbContext.Transacciones
            .FirstOrDefaultAsync(t => t.TransaccionId == transaccionId);

        if(transaccion is null)
        {
            return new ActualizarTransaccionResult
            {
                Estado = EstadoTransaccionOperacion.TransaccionNoEncontrada
            };
        }

        if (transaccion.Estado == EstadoTransaccion.Anulada)
        {
            return new ActualizarTransaccionResult
            {
                Estado = EstadoTransaccionOperacion.TransaccionAnulada
            };
        }

        var estado = await ValidarRelacionesAsync(request.CuentaId, request.CategoriaId, request.Tipo);

        if(estado != EstadoTransaccionOperacion.Exitoso)
        {
            return new ActualizarTransaccionResult
            {
                Estado = estado
            };
        }

        transaccion.CuentaId = request.CuentaId;
        transaccion.CategoriaId = request.CategoriaId;
        transaccion.Cantidad = request.Cantidad;
        transaccion.Tipo = request.Tipo;
        transaccion.Descripcion = request.Descripcion;
        transaccion.FechaTransaccion = request.FechaTransaccion;

        await _dbContext.SaveChangesAsync();

        return new ActualizarTransaccionResult
        {
            Estado = EstadoTransaccionOperacion.Exitoso,
            Transaccion = new TransaccionDto
            {
                TransaccionId = transaccion.TransaccionId,
                CuentaId = transaccion.CuentaId,
                CategoriaId = transaccion.CategoriaId,
                Cantidad = transaccion.Cantidad,
                Tipo = transaccion.Tipo,
                Descripcion = transaccion.Descripcion,
                FechaTransaccion = transaccion.FechaTransaccion,
                Estado = transaccion.Estado,
                FechaAnulacion = transaccion.FechaAnulacion
            }
        };
    }

    public async Task<EstadoTransaccionOperacion> AnularAsync(int transaccionId)
    {
        var transaccion = await _dbContext.Transacciones
            .FirstOrDefaultAsync(t => t.TransaccionId == transaccionId);

        if(transaccion is null)
        {
            return EstadoTransaccionOperacion.TransaccionNoEncontrada;
        }

        if(transaccion.Estado == EstadoTransaccion.Anulada)
        {
            return EstadoTransaccionOperacion.TransaccionAnulada;
        }

        transaccion.Estado = EstadoTransaccion.Anulada;
        transaccion.FechaAnulacion = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return EstadoTransaccionOperacion.Exitoso;
    }

    public async Task<SaldoCuentaDto?> ObtenerSaldoCuentaAsync(int cuentaId)
    {
        var cuenta = await _dbContext.Cuentas
            .AnyAsync(c => c.CuentaId == cuentaId);

        if (!cuenta)
        {
            return null;
        }

        var totalIngresos = await _dbContext.Transacciones
            .Where(t => t.CuentaId == cuentaId
                        && t.Estado == EstadoTransaccion.Activa 
                        && t.Tipo == TipoTransaccion.Ingreso)
            .SumAsync(t => (decimal?)t.Cantidad) ?? 0m;

        var totalGastos = await _dbContext.Transacciones
            .Where(t => t.CuentaId == cuentaId
                        && t.Estado == EstadoTransaccion.Activa
                        && t.Tipo == TipoTransaccion.Gasto)
            .SumAsync(t => (decimal?)t.Cantidad) ?? 0m;

        return new SaldoCuentaDto
        {
            CuentaId = cuentaId,
            TotalIngresos = totalIngresos,
            TotalGastos = totalGastos,
            Saldo = totalIngresos - totalGastos
        };
    }

    public async Task<ResumenFinancieroUsuarioDto?> ObtenerResumenFinancieroUsuarioAsync(int usuarioId)
    {
        var usuario = await _dbContext.Usuarios
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

        if (usuario is null)
        {
            return null;
        }

        var cuentas = await _dbContext.Cuentas
            .Where(c => c.UsuarioId == usuarioId)
            .Select(c => new ResumenCuentaDto
            {
                CuentaId = c.CuentaId,
                NombreCuenta = c.NombreCuenta,
                TotalIngresos = c.Transacciones
                                    .Where(t => t.Estado == EstadoTransaccion.Activa 
                                                && t.Tipo == TipoTransaccion.Ingreso)
                                    .Sum(t => (decimal?)t.Cantidad) ?? 0m,
                TotalGastos = c.Transacciones
                                    .Where(t => t.Estado == EstadoTransaccion.Activa
                                                && t.Tipo == TipoTransaccion.Gasto)
                                    .Sum(t => (decimal?)t.Cantidad) ?? 0m,
            })
            .ToListAsync();

        var totalIngresos = cuentas.Sum(c => c.TotalIngresos);
        var totalGastos = cuentas.Sum(c => c.TotalGastos);

        return new ResumenFinancieroUsuarioDto
        {
            UsuarioId = usuario.UsuarioId,
            NombreUsuario = usuario.NombreUsuario,
            TotalIngresos = totalIngresos,
            TotalGastos = totalGastos,
            SaldoTotal = totalIngresos - totalGastos,
            Cuentas = cuentas
        };
    }
}
