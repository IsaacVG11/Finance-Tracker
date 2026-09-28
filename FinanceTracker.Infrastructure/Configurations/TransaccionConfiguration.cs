using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Infrastructure.Configurations;

public class TransaccionConfiguration : IEntityTypeConfiguration<Transaccion>
{
    public void Configure(EntityTypeBuilder<Transaccion> builder)
    {
        builder.ToTable("Transaccion");
        builder.HasKey(t => t.TransaccionId);

        builder.Property(t => t.TransaccionId)
            .HasColumnName("transaccion_id");

        builder.Property(t => t.CuentaId)
            .HasColumnName("cuenta_id")
            .IsRequired();

        builder.Property(t => t.CategoriaId)
            .HasColumnName("categoria_id")
            .IsRequired();

        builder.Property(t => t.Cantidad)
            .HasColumnName("cantidad")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(t => t.Tipo)
            .HasColumnName("tipo")
            .HasConversion(
                tipo => tipo == TipoTransaccion.Ingreso ? "ingreso" : "gasto",
                valor => valor == "ingreso" ? TipoTransaccion.Ingreso : TipoTransaccion.Gasto)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(t => t.Descripcion)
            .HasColumnName("descripcion")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(t => t.FechaTransaccion)
            .HasColumnName("fecha_transaccion")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(t => t.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSDATETIME()");

        builder.Property(t => t.Estado)
            .HasColumnName("estado")
            .HasConversion(
                tipo => tipo == EstadoTransaccion.Activa ? "activa" : "anulada",
                valor => valor == "activa" ? EstadoTransaccion.Activa : EstadoTransaccion.Anulada)
            .HasMaxLength(15)
            .HasDefaultValue(EstadoTransaccion.Activa)
            .IsRequired();

        builder.Property(t => t.FechaAnulacion)
            .HasColumnName("fecha_anulacion")
            .HasColumnType("datetime2");

        builder.HasOne(t => t.Cuenta)
            .WithMany(c => c.Transacciones)
            .HasForeignKey(t => t.CuentaId)
            .HasConstraintName("FK_Transaccion_Cuenta")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Categoria)
            .WithMany(c => c.Transacciones)
            .HasForeignKey(t => t.CategoriaId)
            .HasConstraintName("FK_Transaccion_Categoria")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
