using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SICAF.Business.Interfaces.Academic;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Academic;
using SICAF.Web.Controllers;

namespace SICAF.Web.Areas.Academic.Controllers;

/// <summary>
/// Controlador para la gestión de tareas académicas
/// </summary>
[Area("Academic")]
[Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN}, {SystemRoles.INSTRUCTOR}")]
public class TaskManagementController(
    ITaskManagementService taskManagementService,
    IValidator<UpdateTasksDto> updateTasksValidator,
    ILogger<TaskManagementController> logger
) : BaseController
{
    private readonly ITaskManagementService _taskManagementService = taskManagementService;
    private readonly IValidator<UpdateTasksDto> _updateTasksValidator = updateTasksValidator;
    private readonly ILogger<TaskManagementController> _logger = logger;

    /// <summary>
    /// Vista principal: selector de fases
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _taskManagementService.GetPhasesAsync(AuditContext.WingType);

        if (!result.IsSuccess)
        {
            _logger.LogError("Error al obtener fases: {Error}", result.Error.Message);
            TempData[NotificationConstants.Error] = result.Error.Message;
            return View(new List<PhaseBasicDto>());
        }

        return View(result.Value);
    }

    /// <summary>
    /// Vista de edición de tareas para una fase específica
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ManageTasks(Guid phaseId)
    {
        var result = await _taskManagementService.GetPhaseTasksAsync(phaseId);

        if (!result.IsSuccess)
        {
            _logger.LogError("Error al obtener tareas de la fase {PhaseId}: {Error}", phaseId, result.Error.Message);
            TempData[NotificationConstants.Error] = result.Error.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Value);
    }

    /// <summary>
    /// Guardar cambios en tareas
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> SaveTasks([FromBody] UpdateTasksDto dto)
    {
        try
        {
            // Validar con FluentValidation
            var validationResult = await ValidateAsync(_updateTasksValidator, dto);
            if (!validationResult.IsValid)
            {
                return JsonValidationError(validationResult);
            }

            var result = await _taskManagementService.UpdatePhaseTasksAsync(dto);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Error al actualizar tareas: {Error}", result.Error.Message);
                return JsonError(result.Error.Message);
            }

            // Auditoría
            await SaveAuditLogAsync(
                EntityNames.Academic.MissionTask,
                dto.PhaseId,
                DatabaseOperationType.Update,
                null,
                dto
            );

            return JsonSuccess("Tareas actualizadas exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al guardar tareas");
            await SaveLogErrorAsync(ex, "Error al guardar tareas de la fase");
            return JsonError("Error inesperado al guardar las tareas");
        }
    }
}
