using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SICAF.Common.Constants;
using SICAF.Web.Interfaces.Pdf;

namespace SICAF.Web.Areas.Reports.Controllers;

[Area("Reports")]
[Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN},{SystemRoles.USERS_ADMIN}")]
public class PuppeteerController(ILogger<PuppeteerController> logger, IPuppeteerService pdfService) : Controller
{
    private readonly ILogger<PuppeteerController> _logger = logger;
    private readonly IPuppeteerService _pdfService = pdfService;

    // Metodo para generar PDF
    [HttpPost]
    public async Task<IActionResult> GeneratePdf()
    {
        try
        {
            // Construir URL para que Puppeteer navegue y genere el PDF mediante chromium
            var url = Url.Action("ViewForPdf", "ControllerName", new { }, Request.Scheme);

            if (string.IsNullOrEmpty(url))
                throw new InvalidOperationException("No se pudo construir la URL del reporte");

            // Obtener TODAS las cookies para autenticarse
            var cookies = Request.Cookies
                .Select(c => new KeyValuePair<string, string>(c.Key, c.Value))
                .ToList();

            // Generar el PDF navegando a la URL con las cookies de autenticación
            var pdfBytes = await _pdfService.GeneratePdfFromUrlAsync(url, cookies: cookies);
            var fileName = $"Informe_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar PDF del informe general");
            TempData[NotificationConstants.Error] = "Error al generar el PDF. Por favor intente nuevamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}