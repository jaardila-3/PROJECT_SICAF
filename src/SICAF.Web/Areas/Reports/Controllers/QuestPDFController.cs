using System.Net.Mime;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SICAF.Business.Interfaces.Reports;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Reports;
using SICAF.Web.Controllers;
using SICAF.Web.Interfaces.Pdf;

namespace SICAF.Web.Areas.Reports.Controllers;

[Area("Reports")]
[Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN},{SystemRoles.INSTRUCTOR}")]
public partial class QuestPDFController(
    ILogger<QuestPDFController> logger,
    IReportService reportService,
    IQuestPDFService questPdfService) : BaseController
{
    private readonly ILogger<QuestPDFController> _logger = logger;
    private readonly IReportService _reportService = reportService;
    private readonly IQuestPDFService _questPdfService = questPdfService;

    [HttpPost]
    public async Task<IActionResult> GeneralReportPDF(ReportFilterDto filter)
    {
        if (!ModelState.IsValid)
        {
            TempData[NotificationConstants.Error] = "Error al generar el reporte";
            return RedirectToAction("Index", "Report");
        }

        try
        {
            // Obtener datos del reporte
            var result = await _reportService.GetGeneralReportDataAsync(
                filter.CourseId!.Value,
                filter.Force,
                filter.WingType,
                filter.PhaseId
            );

            if (result.IsFailure)
            {
                _logger.LogError("Error al obtener datos del reporte general: {ErrorCode} - {ErrorMessage}",
                    result.Error.Code, result.Error.Message);
                TempData[NotificationConstants.Error] = $"Error al generar el reporte: {result.Error.Message}";
                return RedirectToAction("Index", "Report");
            }

            // Generar PDF
            var pdfBytes = await _questPdfService.GenerateGeneralReportPdfAsync(result.Value);

            // Generar nombre de archivo
            var fileName = SanitizeFileName($"Informe_General_{result.Value.CourseName}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            // Retornar archivo PDF
            return File(pdfBytes, MediaTypeNames.Application.Pdf, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al generar el PDF del reporte general");
            TempData[NotificationConstants.Error] = "Error inesperado al generar el PDF. Por favor, intente nuevamente.";
            return RedirectToAction("Index", "Report");
        }
    }

    [HttpPost]
    public async Task<IActionResult> IndividualReportPDF(ReportFilterDto filter)
    {
        if (!ModelState.IsValid || !filter.StudentId.HasValue)
        {
            TempData[NotificationConstants.Error] = "Error al generar el informe individual";
            return RedirectToAction("Index", "Report");
        }

        try
        {
            // Obtener datos del reporte
            var result = await _reportService.GetIndividualReportDataAsync(
                filter.StudentId.Value,
                filter.CourseId!.Value,
                filter.PhaseId
            );

            if (result.IsFailure)
            {
                _logger.LogError("Error al obtener datos del informe individual: {ErrorCode} - {ErrorMessage}",
                    result.Error.Code, result.Error.Message);
                TempData[NotificationConstants.Error] = $"Error al generar el informe: {result.Error.Message}";
                return RedirectToAction("Index", "Report");
            }

            // Generar PDF
            var pdfBytes = await _questPdfService.GenerateIndividualReportPdfAsync(result.Value);

            // Generar nombre de archivo
            var fileName = SanitizeFileName($"Informe_Individual_{result.Value.FullName}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            // Retornar archivo PDF
            return File(pdfBytes, MediaTypeNames.Application.Pdf, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al generar el PDF del informe individual");
            TempData[NotificationConstants.Error] = "Error inesperado al generar el PDF. Por favor, intente nuevamente.";
            return RedirectToAction("Index", "Report");
        }
    }

    // Helper method para sanitizar nombres de archivo
    private static string SanitizeFileName(string fileName)
    {
        // Remover caracteres no válidos para nombres de archivo
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));

        // Remover espacios múltiples y reemplazar espacios con guiones bajos
        sanitized = FileNameRegex().Replace(sanitized, "_");

        // Limitar longitud del nombre
        if (sanitized.Length > 200)
        {
            var extension = Path.GetExtension(sanitized);
            sanitized = sanitized[..(200 - extension.Length)] + extension;
        }

        return sanitized;
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex FileNameRegex();
}