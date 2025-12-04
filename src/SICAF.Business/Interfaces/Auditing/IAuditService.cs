using SICAF.Common.DTOs.Auditing;

namespace SICAF.Business.Interfaces.Auditing;

/// <summary>
/// Servicio para gestionar la auditoría en la capa de negocio
/// </summary>
public interface IAuditService
{
    /// <summary>
    /// Guarda un log de auditoría directamente
    /// </summary>
    Task<bool> SaveAuditLogAsync(AuditInfoDto auditInfo);

    /// <summary>
    /// Guarda un log de auditoría para operaciones CRUD con valores serializados automáticamente
    /// </summary>
    Task<bool> SaveCrudAuditAsync(AuditInfoDto auditInfo, object? oldValues = null, object? newValues = null);
}