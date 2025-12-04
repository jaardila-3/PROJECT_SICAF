using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SICAF.Business.Interfaces.Auditing;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Auditing;
using SICAF.Web.Controllers;

namespace SICAF.Web.Areas.Identity.Controllers;

/// <summary>
/// Controlador para la gestión de auditoría
/// </summary>
[Area("Identity")]
[Authorize(Roles = SystemRoles.USERS_ADMIN)]
public class AuditController(
    ILogger<AuditController> logger,
    IAuditReadService auditReadService
    ) : BaseController
{
    private readonly ILogger<AuditController> _logger = logger;
    private readonly IAuditReadService _auditReadService = auditReadService;

    /// Lista todos los logs de auditoría con filtros opcionales
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(AuditFilterDto? filters = null)
    {
        var result = await _auditReadService.GetAuditLogsAsync(filters);

        if (!result.IsSuccess)
        {
            _logger.LogError("Error al obtener logs de auditoría: {Code} - {Message}", result.Error.Code, result.Error.Message);
            TempData[NotificationConstants.Error] = "Error al cargar los registros de auditoría.";
            return View(new List<AuditInfoDto>());
        }

        // Registrar acceso a auditoría
        //await SaveAuditLogAsync(EntityNames.Identity.User, AuditContext.UserId, "ViewAuditLogs");

        // Pasar los filtros a la vista
        ViewBag.Filters = filters ?? new AuditFilterDto();
        ViewBag.HasFilters = filters?.HasFilters ?? false;

        // Preparar listas desplegables para filtros
        await PrepareFilterViewBags();

        return View(result.Value);
    }

    /// <summary>
    /// Muestra los detalles de un log específico
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _auditReadService.GetAuditLogByIdAsync(id);

        if (!result.IsSuccess)
        {
            _logger.LogError("Error al obtener log {Id}: {Code} - {Message}",
                id, result.Error.Code, result.Error.Message);
            TempData[NotificationConstants.Error] = "Registro no encontrado.";
            return RedirectToAction(nameof(Index));
        }

        return View(result.Value);
    }

    /// <summary>
    /// Limpia los filtros y redirige al índice
    /// </summary>
    [HttpPost]
    public IActionResult ClearFilters() => RedirectToAction(nameof(Index));

    #region Métodos privados auxiliares
    /// <summary>
    /// Prepara las listas desplegables para los filtros
    /// </summary>
    private async Task PrepareFilterViewBags()
    {
        // Tipos de operación
        var resultOperationTypes = await _auditReadService.GetOperationTypesAsync();

        if (!resultOperationTypes.IsSuccess)
        {
            _logger.LogError("Error al obtener tipos de operación: {Code} - {Message}", resultOperationTypes.Error.Code, resultOperationTypes.Error.Message);
            TempData[NotificationConstants.Error] = "Error al obtener tipos de operación.";
            ViewBag.OperationTypes = DatabaseOperationType.ValidTypes;
            return;
        }

        ViewBag.OperationTypes = resultOperationTypes.Value;
    }
    #endregion
}