using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using SICAF.Business.Interfaces.Reports;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Reports;
using SICAF.Web.Controllers;

namespace SICAF.Web.Areas.Reports.Controllers;

[Area("Reports")]
[Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN}, {SystemRoles.INSTRUCTOR}")]
public class ReportController(
    ILogger<ReportController> logger,
    IReportService reportService
    ) : BaseController
{
    private readonly ILogger<ReportController> _logger = logger;
    private readonly IReportService _reportService = reportService;


    public async Task<IActionResult> Index()
    {
        var currentUserId = AuditContext.LoggedUserId;
        var currentUserRoles = AuditContext.UserRole?.Split(',') ?? [];

        var courses = currentUserRoles.Contains(SystemRoles.ACADEMIC_ADMIN)
            ? await _reportService.GetCoursesAsync()
            : await _reportService.GetCoursesAsync(currentUserId);

        if (courses.Count == 0)
            TempData[NotificationConstants.Warning] = "No se encontraron Programas";

        // Convertir tuplas a objetos anónimos para SelectList
        ViewBag.Courses = new SelectList(
            courses.Select(c => new { c.Id, c.Name }),
            "Id",
            "Name"
        );

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GeneralReport(ReportFilterDto filter)
    {
        if (!ModelState.IsValid)
        {
            TempData[NotificationConstants.Error] = "Error al generar el reporte";
            return RedirectToAction(nameof(Index));
        }

        if (!string.IsNullOrEmpty(AuditContext.WingType) && filter.WingType != AuditContext.WingType)
        {
            filter.WingType = AuditContext.WingType;
        }

        var result = await _reportService.GetGeneralReportDataAsync(
            filter.CourseId!.Value,
            filter.Force,
            filter.WingType,
            filter.PhaseId
        );

        if (result.IsFailure)
        {
            _logger.LogError("Error al generar el reporte general: {ErrorCode} - {ErrorMessage}",
                result.Error.Code, result.Error.Message);
            TempData[NotificationConstants.Error] = $"Error al generar el reporte: {result.Error.Message}";
            return RedirectToAction(nameof(Index));
        }

        return View(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> IndividualReport(ReportFilterDto filter)
    {
        if (!ModelState.IsValid || !filter.StudentId.HasValue)
        {
            TempData[NotificationConstants.Error] = "Error al generar el informe individual";
            return RedirectToAction(nameof(Index));
        }

        if (!string.IsNullOrEmpty(AuditContext.WingType) && filter.WingType != AuditContext.WingType)
        {
            filter.WingType = AuditContext.WingType;
        }

        var result = await _reportService.GetIndividualReportDataAsync(
            filter.StudentId.Value,
            filter.CourseId!.Value,
            filter.PhaseId
        );

        if (result.IsFailure)
        {
            _logger.LogError("Error al generar el informe individual: {ErrorCode} - {ErrorMessage}",
                result.Error.Code, result.Error.Message);
            TempData[NotificationConstants.Error] = $"Error al generar el informe: {result.Error.Message}";
            return RedirectToAction(nameof(Index));
        }

        return View(result.Value);
    }

    /// <summary>
    /// Vista de detalle unificada para todos los tipos de gráficos
    /// </summary>
    /// <param name="type">Tipo de reporte: grades, machine-hours, instructor-hours, machine-unsatisfactory, instructor-unsatisfactory, nred-categories</param>
    /// <param name="value">Valor seleccionado en el gráfico (ej: "A", "FAC-1234", nombre instructor, categoría)</param>
    [HttpGet]
    public async Task<IActionResult> ReportDetail(string type, string value, Guid courseId, string? force, string? wingType, Guid? phaseId)
    {
        if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(value))
        {
            TempData[NotificationConstants.Error] = "Parámetros inválidos";
            return RedirectToAction(nameof(Index));
        }

        dynamic? result = type.ToLower() switch
        {
            ReportDetailConstants.Grades => await _reportService.GetGradeDistributionDetailAsync(courseId, value, force, wingType, phaseId),
            ReportDetailConstants.MachineHours => await _reportService.GetMachineFlightHoursDetailAsync(courseId, value, force, wingType, phaseId),
            ReportDetailConstants.InstructorHours => await _reportService.GetInstructorFlightHoursDetailAsync(courseId, value, force, wingType, phaseId),
            ReportDetailConstants.MachineUnsatisfactory => await _reportService.GetMachineUnsatisfactoryDetailAsync(courseId, value, force, wingType, phaseId),
            ReportDetailConstants.InstructorUnsatisfactory => await _reportService.GetInstructorUnsatisfactoryDetailAsync(courseId, value, force, wingType, phaseId),
            ReportDetailConstants.NRedCategories => await _reportService.GetNRedCategoriesDetailAsync(courseId, value, force, wingType, phaseId),
            _ => null
        };

        if (result == null)
        {
            TempData[NotificationConstants.Error] = "Tipo de reporte inválido";
            return RedirectToAction(nameof(Index));
        }

        if (result.IsFailure)
        {
            string errorMessage = result.Error.Message ?? "Error desconocido";

            _logger.LogError("Error al generar reporte de detalle: {Type} - {Message}", type, errorMessage);
            TempData[NotificationConstants.Error] = $"Error: {errorMessage}";
            return RedirectToAction(nameof(Index));
        }

        var data = result.Value;

        // Pasar el tipo de reporte a la vista para decidir qué partial renderizar
        ViewBag.ReportType = type.ToLower();

        return View(data);
    }
}