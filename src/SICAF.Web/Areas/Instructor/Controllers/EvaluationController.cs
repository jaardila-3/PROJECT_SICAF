using System.Text.Json;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using SICAF.Business.Interfaces.Instructor;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Academic;
using SICAF.Common.DTOs.Instructor;
using SICAF.Web.Areas.Academic.Controllers;
using SICAF.Web.Controllers;

namespace SICAF.Web.Areas.Instructor.Controllers;

[Area("Instructor")]
[Authorize(Roles = SystemRoles.INSTRUCTOR)]
public class EvaluationController(
    IEvaluationService evaluationService,
    IValidator<SaveMissionEvaluationDto> saveMissionEvaluationValidator,
    ILogger<EvaluationController> logger
    ) : BaseController
{
    private readonly ILogger<EvaluationController> _logger = logger;
    private readonly IEvaluationService _studentEvaluationService = evaluationService;
    private readonly IValidator<SaveMissionEvaluationDto> _saveMissionEvaluationValidator = saveMissionEvaluationValidator;

    /// <summary>
    /// Muestra la vista de evaluación de un estudiante
    /// </summary>
    /// <param name="studentId">ID del estudiante a evaluar</param>
    /// <param name="courseId">ID del programa</param>
    /// <returns>Vista con la información de evaluación</returns>
    [HttpGet]
    public async Task<IActionResult> EvaluateStudent(Guid studentId, Guid courseId, string? viewType = null)
    {
        // Verificar que el instructor puede evaluar a este estudiante y el tipo de participación
        var instructorId = AuditContext.LoggedUserId;
        var instructorParticipationResult = await _studentEvaluationService.GetInstructorParticipationTypeAsync(instructorId, studentId, courseId);

        if (!instructorParticipationResult.IsSuccess)
        {
            TempData[NotificationConstants.Error] = "No tienes permisos para evaluar a este estudiante";
            return RedirectToAction(nameof(CourseController.Details), "Course", new { area = "Academic", id = courseId });
        }

        // Obtener la información completa para la evaluación
        var evaluationOverviewResult = await _studentEvaluationService.GetStudentEvaluationOverviewAsync(studentId, courseId, instructorId, viewType);

        if (!evaluationOverviewResult.IsSuccess)
        {
            _logger.LogError("Error al cargar información de evaluación: {Error}", evaluationOverviewResult.Error.Message);
            TempData[NotificationConstants.Error] = evaluationOverviewResult.Error.Message;
            return RedirectToAction(nameof(CourseController.Details), "Course", new { area = "Academic", id = courseId });
        }

        var overview = evaluationOverviewResult.Value;

        // Información para mostrar matricula de la aeronave y estadísticas
        ViewBag.IsFlightLeader = instructorParticipationResult.Value == ParticipationTypes.FLIGHT_LEADER;
        ViewBag.PhaseProgress = $"{overview.CompletedMissionsCount}/{overview.TotalMissionsInPhase} misiones completadas";
        ViewBag.SelectAircraft = overview.NextMissionToEvaluate?.Aircrafts != null
                ? new SelectList(overview.NextMissionToEvaluate.Aircrafts, "Id", "Registration")
                : new SelectList(Enumerable.Empty<object>(), "Id", "Registration");

        ViewBag.ViewType = viewType;
        return viewType == AviationConstants.NonEvaluableMission ? View(AviationConstants.NonEvaluableMission, overview) : View(overview);
    }

    /// <summary>
    /// Guarda la evaluación de una misión
    /// </summary>
    /// <param name="model">Datos de la evaluación</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> SaveEvaluation([FromBody] SaveMissionEvaluationDto model)
    {
        // Asegurar que el instructor sea el usuario logueado
        model.InstructorId = AuditContext.LoggedUserId;

        // Validar el modelo con FluentValidation
        var validationResult = await ValidateAsync(_saveMissionEvaluationValidator, model);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            return JsonError(errors);
        }

        // Verificar permisos y el tipo de participación
        var instructorParticipationResult = await _studentEvaluationService.GetInstructorParticipationTypeAsync(model.InstructorId, model.StudentId, model.CourseId);

        if (!instructorParticipationResult.IsSuccess)
        {
            return JsonError("No tienes permisos para evaluar a este estudiante");
        }

        // Guardar la evaluación
        var result = model.ViewType == AviationConstants.NonEvaluableMission
            ? await _studentEvaluationService.SaveNonEvaluableMissionAsync(model)
            : await _studentEvaluationService.SaveMissionEvaluationAsync(model);

        if (result.IsSuccess)
        {
            _logger.LogInformation("Evaluación guardada - Instructor: {InstructorId}, Estudiante: {StudentId}, Misión: {MissionId}",
                model.InstructorId, model.StudentId, model.MissionId);

            var redirectUrl = Url.Action(nameof(CourseController.Details), "Course", new { area = "Academic", id = model.CourseId });

            return JsonSuccess("Evaluación guardada exitosamente", new { redirectUrl });
        }
        else
        {
            _logger.LogError("Error al guardar evaluación: {Error}", result.Error.Message);
            return JsonError(result.Error.Message);
        }
    }

    /// <summary>
    /// Promueve un estudiante a la siguiente fase
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{SystemRoles.INSTRUCTOR}")]
    public async Task<IActionResult> PromoteStudentToNextPhase([FromBody] PromoteStudentDto request)
    {
        var result = await _studentEvaluationService.PromoteStudentToNextPhaseAsync(request);
        return !result.IsSuccess ? JsonError(result.Error.Message) : JsonSuccess(result.Value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{SystemRoles.INSTRUCTOR}")]
    public async Task<JsonResult> SaveCommitteeDecision([FromBody] SaveCommitteeDecisionDto request)
    {
        if (!ModelState.IsValid)
        {
            return JsonError("Datos inválidos");
        }

        var result = await _studentEvaluationService.SaveCommitteeDecisionAsync(request);

        return !result.IsSuccess ? JsonError(result.Error.Message) : JsonSuccess(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = $"{SystemRoles.INSTRUCTOR}")]
    public async Task<IActionResult> FinalizeAndApproveCourse([FromBody] PromoteStudentDto request)
    {
        var result = await _studentEvaluationService.FinalizeAndApproveCourseAsync(request);
        return !result.IsSuccess ? JsonError(result.Error.Message) : JsonSuccess(result.Value);
    }

    /// <summary>
    /// Muestra la vista para editar calificaciones de una misión
    /// </summary>
    /// <param name="missionId">ID de la misión</param>
    /// <param name="studentId">ID del estudiante</param>
    /// <param name="courseId">ID del programa</param>
    /// <returns>Vista de edición de calificaciones</returns>
    [HttpGet]
    public async Task<IActionResult> EditMissionGrades(Guid missionId, Guid studentId, Guid courseId, string? viewType = null)
    {
        var instructorId = AuditContext.LoggedUserId;

        // Obtener datos para editar
        var result = viewType == AviationConstants.NonEvaluableMission
            ? await _studentEvaluationService.GetNonEvaluableMissionGradesForEditAsync(missionId, studentId, courseId, instructorId)
            : await _studentEvaluationService.GetMissionGradesForEditAsync(missionId, studentId, courseId, instructorId);

        if (!result.IsSuccess)
        {
            TempData[NotificationConstants.Error] = result.Error.Message;
            return RedirectToAction(nameof(EvaluateStudent), new { studentId, courseId });
        }

        ViewBag.ViewType = viewType;
        return View(result.Value);
    }

    /// <summary>
    /// Guarda las ediciones de calificaciones de una misión
    /// </summary>
    /// <param name="model">Datos de las calificaciones editadas</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveMissionGradesEdit(EditMissionGradesDto model)
    {
        model.InstructorId = AuditContext.LoggedUserId;

        var result = model.ViewType == AviationConstants.NonEvaluableMission
            ? await _studentEvaluationService.SaveNonEvaluableMissionGradesEditAsync(model)
            : await _studentEvaluationService.SaveMissionGradesEditAsync(model);

        if (result.IsSuccess)
        {
            TempData[NotificationConstants.Success] = "Calificaciones actualizadas exitosamente";
            _logger.LogInformation("Calificaciones editadas - Instructor: {InstructorId}, Estudiante: {StudentId}, Misión: {MissionId}", model.InstructorId, model.StudentId, model.MissionId);
            // Auditoría con usuario afectado
            var auditInfo = CreateUserAuditInfo(model.StudentId, model.StudentIdentificationName, "Editar calificaciones");
            auditInfo.NewValues = JsonSerializer.Serialize(model);
            await SaveCustomAuditLogAsync(auditInfo);
        }
        else
        {
            TempData[NotificationConstants.Error] = result.Error.Message;
            _logger.LogError("Error al editar calificaciones: {Error}", result.Error.Message);
        }

        return RedirectToAction(nameof(EvaluateStudent), new { studentId = model.StudentId, courseId = model.CourseId });
    }

}