using SICAF.Common.DTOs.Auditing;
using SICAF.Common.Models.Results;

namespace SICAF.Business.Interfaces.Auditing;

/// <summary>
/// Interfaz para el servicio de lectura de auditoría
/// </summary>
public interface IAuditReadService
{
    /// <summary>
    /// Obtiene todos los logs de auditoría con filtros opcionales
    /// </summary>
    Task<Result<IEnumerable<AuditInfoDto>>> GetAuditLogsAsync(AuditFilterDto? filters = null);

    /// <summary>
    /// Obtiene un log de auditoría por su ID
    /// </summary>
    Task<Result<AuditInfoDto>> GetAuditLogByIdAsync(Guid id);


    /// <summary>
    /// Obtiene los tipos de operación de auditoría
    /// </summary>
    Task<Result<IEnumerable<string>>> GetOperationTypesAsync();
}