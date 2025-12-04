using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Entities.Common;

namespace SICAF.Data.Configuration.Common;

/// <summary>
/// Configuración de la entidad base
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        // Configuración de la clave primaria
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedNever() // No generar en BD
            .HasColumnType("uniqueidentifier")
            .HasComment("Identificador único de la entidad")
            ;

        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false)
            .HasComment("Indica si la entidad ha sido eliminada lógicamente (soft delete)")
            ;

        // Índices para optimizar consultas
        builder.HasIndex(e => e.IsDeleted)
            .HasDatabaseName($"IX_{typeof(T).Name}_IsDeleted");

        // Filtro global para soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}