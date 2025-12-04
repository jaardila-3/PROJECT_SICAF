using System.Text.Json;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SICAF.Business.Interfaces.Student;
using SICAF.Common.Constants;
using SICAF.Web.Areas.Academic.Controllers;
using SICAF.Web.Controllers;

namespace SICAF.Web.Areas.Student.Controllers;

[Area("Student")]
[Authorize]
public class StudentController(
    IStudentService studentService,
    ILogger<StudentController> logger
    ) : BaseController
{
    private readonly IStudentService _studentService = studentService;
    private readonly ILogger<StudentController> _logger = logger;

    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Muestra el detalle de una misión evaluada
    /// </summary>
    /// <param name="studentMissionProgressId">ID del StudentMissionProgress</param>
    /// <returns>Vista con el detalle de la evaluación</returns>
    [HttpGet]
    public async Task<IActionResult> MissionEvaluationDetail(Guid missionId, Guid studentId, Guid courseId, string? viewType = null, string? returnUrl = null)
    {
        var result = viewType == AviationConstants.NonEvaluableMission
            ? await _studentService.GetNonEvaluableMissionDetailAsync(missionId, studentId, courseId)
            : await _studentService.GetMissionEvaluationDetailAsync(missionId, studentId, courseId);

        if (!result.IsSuccess)
        {
            _logger.LogError("Error al cargar el detalle de evaluación: {Error}", result.Error.Message);
            TempData[NotificationConstants.Error] = result.Error.Message;
            return RedirectToAction(nameof(CourseController.Index), "Course", new { area = "Academic" });
        }

        // Auditoría para el estudiante que consulta sus evaluaciones: notificación
        var currentUserRoles = AuditContext.UserRole?.Split(',') ?? [];
        if (currentUserRoles.Contains(SystemRoles.STUDENT))
        {
            var auditInfo = CreateUserAuditInfo(
                studentId, $"{AuditContext.IdentificationNumber} - {AuditContext.Name} {AuditContext.LastName}", DatabaseOperationType.Read);
            auditInfo.NewValues = JsonSerializer.Serialize("Estudiante consulta las calificaciones de la misión con ID: " + missionId);
            await SaveCustomAuditLogAsync(auditInfo);
        }

        ViewBag.ViewType = viewType;
        ViewBag.ReturnUrl = returnUrl;

        return View(result.Value);
    }

}