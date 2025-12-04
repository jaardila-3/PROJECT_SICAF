using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SICAF.Business.Services.System;
using SICAF.Web.Models;

namespace SICAF.Web.Controllers;

public class HomeController(
    ISystemInfoService systemInfoService,
    ILogger<HomeController> logger
    ) : Controller
{
    private readonly ISystemInfoService _systemInfoService = systemInfoService;
    private readonly ILogger<HomeController> _logger = logger;

    [Authorize]
    public IActionResult Index() => View();

    [AllowAnonymous]
    [HttpGet("api/alexander_ardila")]
    public async Task<IActionResult> SystemStatusJson()
    {
        try
        {
            var systemInfo = await _systemInfoService.GetSystemInfoAsync();
            return Json(systemInfo);
        }
        catch
        {
            return NotFound();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public IActionResult Error()
    {
        var errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };

        // Obtener información del error desde la sesión si existe
        if (HttpContext.Session != null)
        {
            var errorMessage = HttpContext.Session.GetString("ErrorMessage");
            var errorStatusCode = HttpContext.Session.GetInt32("ErrorStatusCode");

            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorViewModel.Message = errorMessage;
                errorViewModel.StatusCode = errorStatusCode ?? 500;

                // Limpiar la sesión después de usar los valores
                HttpContext.Session.Remove("ErrorMessage");
                HttpContext.Session.Remove("ErrorStatusCode");
            }
        }

        return View(errorViewModel);
    }

}
