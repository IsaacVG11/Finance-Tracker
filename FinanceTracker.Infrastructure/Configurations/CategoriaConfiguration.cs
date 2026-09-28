using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.Infrastructure.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categoria");
        builder.HasKey(c => c.CategoriaId);

        builder.Property(c => c.CategoriaId)
            .HasColumnName("categoria_id");

        builder.Property(c => c.UsuarioId)
            .HasColumnName("usuario_id")
            .IsRequired();

        builder.Property(c => c.Tipo)
            .HasColumnName("tipo")
            .HasConversion(
                tipo => tipo == TipoTransaccion.Ingreso ? "ingreso" : "gasto",
                valor => valor == "ingreso" ? TipoTransaccion.Ingreso : TipoTransaccion.Gasto)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(c => c.NombreCategoria)
            .HasColumnName("nombre_categoria")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.FechaCreacion)
            .HasColumnName("fecha_creacion")
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSDATETIME()");

        builder.Property(c => c.Activo)
            .HasColumnName("activo")
            .HasDefaultValue(true);

        builder.HasIndex(c => new { c.UsuarioId, c.NombreCategoria, c.Tipo })
            .IsUnique();

        builder.HasOne(c => c.Usuario)
            .WithMany(u => u.Categorias)
            .HasForeignKey(c => c.UsuarioId)
            .HasConstraintName("FK_Categorias_Usuario")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Transacciones)
            .WithOne(t => t.Categoria)
            .HasForeignKey(t => t.CategoriaId)
            .HasConstraintName("FK_Transaccion_Categoria")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(c => c.Activo);
    }
}
