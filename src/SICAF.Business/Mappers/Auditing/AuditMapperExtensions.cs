using SICAF.Common.DTOs.Auditing;
using SICAF.Data.Entities.Auditing;

namespace SICAF.Business.Mappers.Auditing;

/// <summary>
/// Extensiones para mapear entidades de auditoría a DTOs
/// </summary>
public static class AuditMapperExtensions
{
    /// <summary>
    /// Convierte una entidad AuditLog a AuditLogDto
    /// </summary>
    public static AuditInfoDto MapToDto(this AuditLog entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new AuditInfoDto
        {
            Id = entity.Id,
            EntityName = entity.EntityName,
            EntityId = entity.EntityId,
            OperationType = entity.OperationType,
            LoggedUserId = entity.LoggedUserId ?? Guid.Empty,
            UserName = entity.UserName,
            UserRole = entity.UserRole,
            IpAddress = entity.IpAddress,
            Module = entity.Module,
            Controller = entity.Controller,
            Action = entity.Action,
            OldValues = entity.OldValues,
            NewValues = entity.NewValues,
            CreatedAt = entity.CreatedAt,
            AffectedUserId = entity.AffectedUserId,
            AffectedUserIdentificationName = entity.AffectedUserIdentificationName
        };
    }

    /// <summary>
    /// Convierte una lista de entidades AuditLog a lista de DTOs
    /// </summary>
    public static List<AuditInfoDto> MapToDtos(this IEnumerable<AuditLog> entities)
    {
        return [.. entities.Select(e => e.MapToDto())];
    }
}