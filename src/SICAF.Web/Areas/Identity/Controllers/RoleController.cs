using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SICAF.Business.Interfaces.Identity;
using SICAF.Common.Constants;
using SICAF.Web.Controllers;

namespace SICAF.Web.Areas.Identity.Controllers;

[Area("Identity")]
[Authorize(Roles = $"{SystemRoles.USERS_ADMIN}")]
public class RoleController(
    ILogger<RoleController> logger,
    IRoleService roleService
    ) : BaseController
{
    private readonly ILogger<RoleController> _logger = logger;
    private readonly IRoleService _roleService = roleService;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _roleService.GetAllRolesAsync();
        return View(result.Value);
    }

    /// <summary>
    /// Muestra los detalles de un rol específico
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _roleService.GetRoleByIdAsync(id);
        if (!result.IsSuccess)
        {
            _logger.LogError("Error: rol con id {id}, {code}: {message}", id, result.Error.Code, result.Error.Message);
            TempData[NotificationConstants.Error] = $"Error: {result.Error.Message}";
            return RedirectToAction(nameof(Index));
        }

        return View(result.Value);
    }
}