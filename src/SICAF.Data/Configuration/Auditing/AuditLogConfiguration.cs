using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Auditing;

namespace SICAF.Data.Configuration.Auditing;

public class AuditLogConfiguration : AuditableLogBaseConfiguration<AuditLog>
{
    public override void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        builder.ToTable("AuditLogs");

        // Configuración de propiedades
        builder.Property(e => e.EntityName)
            .HasMaxLength(100)
            .HasComment("Nombre de la entidad sobre la cual se realiza la operación")
            .IsRequired();

        builder.Property(e => e.EntityId)
            .HasColumnType("uniqueidentifier")
            .HasComment("Identificador único de la entidad referenciada")
            .IsRequired();

        builder.Property(e => e.OperationType)
            .HasMaxLength(50)
            .HasComment("Tipo de operación realizada (Create, Update, Delete, Read)")
            .IsRequired();

        builder.Property(e => e.UserRole)
            .HasMaxLength(100)
            .HasComment("Rol del usuario al momento de realizar la operación");

        // Configuración de propiedades JSON
        builder.Property(e => e.OldValues)
            .HasColumnType("NVARCHAR(MAX)")
            .HasComment("Valores anteriores de la entidad en formato JSON");

        builder.Property(e => e.NewValues)
            .HasColumnType("NVARCHAR(MAX)")
            .HasComment("Nuevos valores de la entidad en formato JSON");

        // Propiedades del usuario afectado
        builder.Property(a => a.AffectedUserId)
            .HasColumnType("uniqueidentifier")
            .HasComment("ID del usuario afectado por la operación");

        builder.Property(a => a.AffectedUserIdentificationName)
            .HasMaxLength(100)
            .HasComment("Identificación y nombre del usuario afectado por la operación");

        // Índices para mejorar rendimiento
        builder.HasIndex(e => e.EntityId)
            .HasDatabaseName("IX_AuditLogs_EntityId");

        builder.HasIndex(e => e.EntityName)
            .HasDatabaseName("IX_AuditLogs_EntityName");

        builder.HasIndex(e => e.OperationType)
            .HasDatabaseName("IX_AuditLogs_OperationType");

        // Índice compuesto para consultas comunes
        builder.HasIndex(e => new { e.EntityName, e.EntityId })
            .HasDatabaseName("IX_AuditLogs_EntityName_EntityId");

        builder.HasIndex(e => new { e.EntityName, e.OperationType, e.CreatedAt })
            .HasDatabaseName("IX_AuditLogs_EntityName_OperationType_CreatedAt");

        // Índice para búsquedas por usuario afectado
        builder.HasIndex(a => a.AffectedUserId)
            .HasDatabaseName("IX_AuditLogs_AffectedUserId");

        // Índice compuesto para búsquedas de "quién modificó a quién"
        builder.HasIndex(a => new { a.LoggedUserId, a.AffectedUserId })
            .HasDatabaseName("IX_AuditLogs_UserId_AffectedUserId");
    }
}