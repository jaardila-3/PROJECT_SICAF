using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SICAF.Data.Configuration.Common;
using SICAF.Data.Entities.Logging;

namespace SICAF.Data.Configuration.Logging;

public class ErrorLogConfiguration : AuditableLogBaseConfiguration<ErrorLog>
{
    public override void Configure(EntityTypeBuilder<ErrorLog> builder)
    {
        // Llamar configuración base
        base.Configure(builder);

        builder.ToTable("ErrorLogs");

        // Configuración de propiedades obligatorias
        builder.Property(e => e.Message)
            .HasMaxLength(2000)
            .HasComment("Mensaje descriptivo del error ocurrido")
            .IsRequired();

        builder.Property(e => e.ExceptionType)
            .HasMaxLength(200)
            .HasComment("Tipo de excepción que se produjo")
            .IsRequired();

        builder.Property(e => e.StatusCode)
            .HasComment("Código de estado HTTP asociado al error")
            .IsRequired();

        builder.Property(e => e.Severity)
            .HasMaxLength(20)
            .HasComment("Nivel de severidad del error")
            .HasDefaultValue("Error")
            .IsRequired();

        builder.Property(e => e.IsResolved)
            .HasComment("Indica si el error ha sido resuelto")
            .HasDefaultValue(false)
            .IsRequired();

        // Configuración de propiedades opcionales
        builder.Property(e => e.StackTrace)
            .HasColumnType("NVARCHAR(MAX)")
            .HasComment("Stack trace completo del error para debugging");

        builder.Property(e => e.InnerException)
            .HasMaxLength(2000)
            .HasComment("Detalles de la excepción interna si existe");

        // Contexto de la solicitud HTTP
        builder.Property(e => e.HttpMethod)
            .HasMaxLength(10)
            .HasComment("Método HTTP de la solicitud que causó el error");

        builder.Property(e => e.Url)
            .HasMaxLength(2000)
            .HasComment("URL completa de la solicitud que generó el error");

        // Información del entorno
        builder.Property(e => e.MachineName)
            .HasMaxLength(100)
            .HasComment("Nombre de la máquina donde ocurrió el error");

        builder.Property(e => e.Environment)
            .HasMaxLength(100)
            .HasComment("Ambiente donde ocurrió el error (Development, Staging, Production)");

        // Información de resolución
        builder.Property(e => e.ResolutionNotes)
            .HasMaxLength(2000)
            .HasComment("Notas sobre cómo se resolvió el error");

        builder.Property(e => e.ResolvedAt)
            .HasComment("Fecha y hora cuando se resolvió el error");

        builder.Property(e => e.ResolvedBy)
            .HasMaxLength(200)
            .HasComment("Usuario que marcó el error como resuelto");

        // Índices para mejorar rendimiento
        builder.HasIndex(e => e.Severity)
            .HasDatabaseName("IX_ErrorLogs_Severity");

        builder.HasIndex(e => e.ExceptionType)
            .HasDatabaseName("IX_ErrorLogs_ExceptionType");

        builder.HasIndex(e => e.StatusCode)
            .HasDatabaseName("IX_ErrorLogs_StatusCode");

        builder.HasIndex(e => e.IsResolved)
            .HasDatabaseName("IX_ErrorLogs_IsResolved");

        builder.HasIndex(e => e.Environment)
            .HasDatabaseName("IX_ErrorLogs_Environment");

        // Índices compuestos para consultas comunes
        builder.HasIndex(e => new { e.Severity, e.CreatedAt })
            .HasDatabaseName("IX_ErrorLogs_Severity_CreatedAt");

        builder.HasIndex(e => new { e.IsResolved, e.Severity })
            .HasDatabaseName("IX_ErrorLogs_IsResolved_Severity");

        builder.HasIndex(e => new { e.ExceptionType, e.CreatedAt })
            .HasDatabaseName("IX_ErrorLogs_ExceptionType_CreatedAt");

        // Índice para búsquedas por rango de fechas y usuario
        builder.HasIndex(e => new { e.LoggedUserId, e.CreatedAt, e.Severity })
            .HasDatabaseName("IX_ErrorLogs_UserId_CreatedAt_Severity");

        builder.HasIndex(e => new { e.IsResolved, e.ResolvedAt })
            .HasDatabaseName("IX_ErrorLogs_IsResolved_ResolvedAt");
    }
}