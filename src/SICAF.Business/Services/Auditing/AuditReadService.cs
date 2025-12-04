using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SICAF.Business.Interfaces.Auditing;
using SICAF.Business.Mappers.Auditing;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Auditing;
using SICAF.Common.Models.Results;
using SICAF.Data.Context;
using SICAF.Data.Entities.Auditing;

namespace SICAF.Business.Services.Auditing;

/// <summary>
/// Servicio de solo lectura para auditoría
/// </summary>
public class AuditReadService(SicafDbContext context, ILogger<AuditReadService> logger) : IAuditReadService
{
    private readonly SicafDbContext _context = context;
    private readonly ILogger<AuditReadService> _logger = logger;

    public async Task<Result<IEnumerable<AuditInfoDto>>> GetAuditLogsAsync(AuditFilterDto? filters = null)
    {
        var query = BuildQuery(_context.AuditLogs, filters);
        var logs = await query.OrderByDescending(a => a.CreatedAt).ToListAsync();

        // Obtener usuarios de una sola vez
        var loginLogs = logs.Where(log => log.OperationType == DatabaseOperationType.Login).ToList();

        if (loginLogs.Count != 0)
        {
            var userIds = loginLogs.Select(log => log.EntityId).Distinct().ToList();

            var users = await _context.Users
                .AsNoTracking()
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Username);

            // Usar PLINQ para procesamiento paralelo
            loginLogs.AsParallel().ForAll(log =>
            {
                if (users.TryGetValue(log.EntityId, out var username))
                    log.UserName = username;
            });
        }

        var logsDto = logs?.MapToDtos() ?? [];

        return Result<IEnumerable<AuditInfoDto>>.Success(logsDto);
    }

    public async Task<Result<AuditInfoDto>> GetAuditLogByIdAsync(Guid id)
    {
        var log = await _context.AuditLogs.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

        if (log is null)
            return Result<AuditInfoDto>.Failure(SystemErrors.GeneralError.NotFound);

        if (log.OperationType == DatabaseOperationType.Login)
        {
            var user = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == log.EntityId)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync();

            if (user != null)
            {
                log.UserName = user.Username;
                log.UserRole = user.UserRoles.FirstOrDefault()?.Role.Name;
            }
        }

        return Result<AuditInfoDto>.Success(log.MapToDto());
    }

    /// <summary>
    /// Obtiene los tipos de operación de auditoría
    /// </summary>
    /// <returns></returns>
    public async Task<Result<IEnumerable<string>>> GetOperationTypesAsync()
    {
        var query = _context.AuditLogs.AsNoTracking().Select(a => a.OperationType);
        return Result<IEnumerable<string>>.Success(await query.Distinct().ToListAsync());
    }

    /// <summary>
    /// Construye la consulta aplicando los filtros de manera secuencial
    /// </summary>
    /// <param name="query">Query base de AuditLogs</param>
    /// <param name="filters">Filtros a aplicar</param>
    /// <param name="disableTracking">Indica si se debe deshabilitar el tracking (por defecto true)</param>
    /// <returns>Query con filtros aplicados</returns>
    private IQueryable<AuditLog> BuildQuery(
        IQueryable<AuditLog> query,
        AuditFilterDto? filters = null,
        bool disableTracking = true)
    {
        // Configurar tracking
        if (disableTracking)
            query = query.AsNoTracking();

        // Si no hay filtros, retornar query básica
        if (filters == null || !filters.HasFilters)
            return query;

        // Aplicar filtros de manera secuencial
        if (!string.IsNullOrWhiteSpace(filters.UserName))
        {
            var userName = filters.UserName.ToLower();
            query = query.Where(a => a.UserName != null && a.UserName.ToLower().Contains(userName));
        }

        if (!string.IsNullOrWhiteSpace(filters.AffectedUserIdentificationName))
        {
            var affectedUser = filters.AffectedUserIdentificationName;
            query = query.Where(a => a.AffectedUserIdentificationName != null && a.AffectedUserIdentificationName.Contains(affectedUser));
        }

        if (!string.IsNullOrWhiteSpace(filters.EntityName))
            query = query.Where(a => a.EntityName == filters.EntityName);

        if (!string.IsNullOrWhiteSpace(filters.OperationType))
            query = query.Where(a => a.OperationType == filters.OperationType);

        if (!string.IsNullOrWhiteSpace(filters.Module))
            query = query.Where(a => a.Module == filters.Module);

        if (!string.IsNullOrWhiteSpace(filters.IpAddress))
            query = query.Where(a => a.IpAddress == filters.IpAddress);

        if (filters.DateFrom.HasValue)
        {
            var dateFrom = filters.DateFrom.Value.Date;
            query = query.Where(a => a.CreatedAt >= dateFrom);
        }

        if (filters.DateTo.HasValue)
        {
            var dateTo = filters.DateTo.Value.Date.AddDays(1);
            query = query.Where(a => a.CreatedAt < dateTo);
        }

        return query;
    }

}