using System.Security.Claims;

using FluentValidation;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SICAF.Business.Interfaces.Identity;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Identity;
using SICAF.Web.Controllers;

namespace SICAF.Web.Areas.Identity.Controllers;

[Area("Identity")]
[AllowAnonymous]
public class AccountController(
    ILogger<AccountController> logger,
    IAuthentication authenticationService,
    IValidator<LoginDto> loginValidator
    ) : BaseController
{
    private readonly ILogger<AccountController> _logger = logger;
    private readonly IAuthentication _authentication = authenticationService;
    private readonly IValidator<LoginDto> _loginValidator = loginValidator;


    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        returnUrl ??= Url.Content("~/");

        #region Validation con FluentValidation
        model.Username = model.Username.Trim().ToLower();
        var validationResult = await ValidateAsync(_loginValidator, model);
        if (!validationResult.IsValid) return View(model);
        #endregion

        var result = await _authentication.HandleLogin(model);

        if (!result.IsSuccess)
        {
            //ModelState.AddModelError(string.Empty, result.Error!.Message);
            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            return View(model);
        }

        var userResponse = result.Value;

        if (userResponse.IsPasswordSetByAdmin)
        {
            TempData[NotificationConstants.ForcePasswordChange] = "Su contraseña temporal debe ser cambiada. Por favor, actualice su contraseña.";
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userResponse.Id.ToString()),
            new(ClaimTypes.Name, userResponse.Username),
            new(ClaimTypes.GivenName, userResponse.Name),
            new(ClaimTypes.Surname, userResponse.LastName),
            new(CustomClaimTypes.IdentificationNumber, userResponse.IdentificationNumber ?? string.Empty),
            new(CustomClaimTypes.Grade, userResponse.Grade ?? string.Empty),
            new(CustomClaimTypes.WingType, userResponse.AviationProfile?.WingType ?? string.Empty)
        };
        claims.AddRange(userResponse.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        _logger.LogInformation("Usuario {Username} autenticado correctamente", userResponse.Username);
        await SaveAuditLogAsync(EntityNames.Identity.User, userResponse.Id, DatabaseOperationType.Login);

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }
}