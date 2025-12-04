using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Entities.Common;

namespace SICAF.Data.Configuration.Common;

/// <summary>
/// Configuración de la entidad base de auditoría
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AuditableLogBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : AuditableLogBase
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

        // Configuración de propiedades de usuario
        builder.Property(ur => ur.LoggedUserId)
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario");

        builder.Property(e => e.UserName)
            .HasMaxLength(50)
            .HasComment("Nombre de usuario");


        builder.Property(e => e.IpAddress)
            .HasMaxLength(45) // IPv6 máximo
            .HasComment("Dirección IP desde donde se realiza la operación");

        // Configuración de ubicación
        builder.Property(e => e.Module)
            .HasMaxLength(100)
            .HasComment("Módulo del sistema donde ocurre la operación");

        builder.Property(e => e.Controller)
            .HasMaxLength(100)
            .HasComment("Controlador donde ocurre la operación");

        builder.Property(e => e.Action)
            .HasMaxLength(100)
            .HasComment("Acción ejecutada en el controlador");

        // Timestamp
        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasComment("Fecha y hora de creación del registro");

        // Índices para optimizar consultas
        builder.HasIndex(e => e.LoggedUserId)
            .HasDatabaseName($"IX_{typeof(T).Name}_UserId");

        builder.HasIndex(e => e.CreatedAt)
            .HasDatabaseName($"IX_{typeof(T).Name}_CreatedAt");

        builder.HasIndex(e => new { e.Module, e.Controller, e.Action })
            .HasDatabaseName($"IX_{typeof(T).Name}_Module_Controller_Action");

        // Índice para consultas por usuario y fecha
        builder.HasIndex(e => new { e.LoggedUserId, e.CreatedAt })
            .HasDatabaseName($"IX_{typeof(T).Name}_UserId_CreatedAt");
    }
}