using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Auditing;

/// <summary>
/// Entidad de auditoría
/// </summary>
public class AuditLog : AuditableLogBase
{
    // Información de la entidad a la que se hace referencia
    public string EntityName { get; set; } = string.Empty;
    public Guid EntityId { get; set; }

    // Tipo de operación
    public string OperationType { get; set; } = string.Empty; // Create, Update, Delete, Read

    // Información del usuario
    public string? UserRole { get; set; }

    // Datos de la operación
    public string? OldValues { get; set; } // JSON con valores anteriores
    public string? NewValues { get; set; } // JSON con valores nuevos

    // Información del usuario afectado
    public Guid? AffectedUserId { get; set; }
    public string? AffectedUserIdentificationName { get; set; }
}