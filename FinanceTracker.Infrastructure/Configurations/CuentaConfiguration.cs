using FinanceTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Infrastructure.Configurations;

public class CuentaConfiguration : IEntityTypeConfiguration<Cuenta>
{
    public void Configure(EntityTypeBuilder<Cuenta> builder)
    {
        builder.ToTable("Cuenta");
        builder.HasKey(c => c.CuentaId);

        builder.Property(c => c.CuentaId)
            .HasColumnName("cuenta_id");

        builder.Property(c => c.UsuarioId)
            .HasColumnName("usuario_id")
            .IsRequired();

        builder.Property(c => c.NombreCuenta)
            .HasColumnName("nombre_cuenta")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSDATETIME()");

        builder.Property(c => c.Activo)
            .HasColumnName("activo")
            .HasDefaultValue(true);

        builder.HasIndex(c => new { c.UsuarioId, c.NombreCuenta })
            .IsUnique();

        builder.HasOne(c => c.Usuario)
            .WithMany(u => u.Cuentas)
            .HasForeignKey(c => c.UsuarioId)
            .HasConstraintName("FK_Cuenta_Usuario")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Transacciones)
            .WithOne(t => t.Cuenta)
            .HasForeignKey(t => t.CuentaId)
            .HasConstraintName("FK_Transaccion_Cuenta")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(c => c.Activo);
    }
}
