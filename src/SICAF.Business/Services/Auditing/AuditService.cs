using System.Text.Json;

using Microsoft.Extensions.Logging;

using SICAF.Business.Interfaces.Auditing;
using SICAF.Common.DTOs.Auditing;
using SICAF.Data.Context;
using SICAF.Data.Entities.Auditing;

namespace SICAF.Business.Services.Auditing;

public class AuditService : IAuditService
{
    private readonly SicafDbContext _context;
    private readonly ILogger<AuditService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public AuditService(SicafDbContext context, ILogger<AuditService> logger)
    {
        _context = context;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<bool> SaveAuditLogAsync(AuditInfoDto auditInfo)
    {
        try
        {
            var auditLog = new AuditLog
            {
                EntityName = auditInfo.EntityName,
                EntityId = auditInfo.EntityId,
                OperationType = auditInfo.OperationType,
                LoggedUserId = auditInfo.LoggedUserId,
                UserName = auditInfo.UserName,
                UserRole = auditInfo.UserRole,
                IpAddress = auditInfo.IpAddress,
                Module = auditInfo.Module,
                Controller = auditInfo.Controller,
                Action = auditInfo.Action,
                OldValues = auditInfo.OldValues,
                NewValues = auditInfo.NewValues,
                CreatedAt = auditInfo.CreatedAt,
                AffectedUserId = auditInfo.AffectedUserId,
                AffectedUserIdentificationName = auditInfo.AffectedUserIdentificationName
            };

            await _context.AuditLogs.AddAsync(auditLog);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar log de auditoría para {EntityName} - {EntityId}", auditInfo.EntityName, auditInfo.EntityId);
            throw;
        }
    }

    public async Task<bool> SaveCrudAuditAsync(AuditInfoDto auditInfo, object? oldValues = null, object? newValues = null)
    {
        try
        {
            // Serializar valores si existen
            if (oldValues != null)
            {
                auditInfo.OldValues = JsonSerializer.Serialize(oldValues, _jsonOptions);
            }

            if (newValues != null)
            {
                auditInfo.NewValues = JsonSerializer.Serialize(newValues, _jsonOptions);
            }

            return await SaveAuditLogAsync(auditInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar auditoría CRUD para {EntityName} - {EntityId}",
                auditInfo.EntityName, auditInfo.EntityId);
            return false;
        }
    }
}