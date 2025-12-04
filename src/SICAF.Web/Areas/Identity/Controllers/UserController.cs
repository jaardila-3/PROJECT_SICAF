using System.Text.Json;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using SICAF.Business.Interfaces.Academic;
using SICAF.Business.Interfaces.Catalogs;
using SICAF.Business.Interfaces.Identity;
using SICAF.Business.Mappers.Identity;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Academic;
using SICAF.Common.DTOs.Common;
using SICAF.Common.DTOs.Identity;
using SICAF.Web.Controllers;
using SICAF.Web.Interfaces.Files;
using SICAF.Web.Interfaces.Identity;

using static SICAF.Common.Constants.CatalogConstants;

namespace SICAF.Web.Areas.Identity.Controllers;

/// <summary>
/// Controlador de usuarios
/// </summary>
[Area("Identity")]
[Authorize]
public class UserController(
    ILogger<UserController> logger,
    IUserService userService,
    IUserValidationService userValidationService,
    IRoleService roleService,
    ICourseService courseService,
    ICatalogService catalogService,
    IImageValidationService imageValidationService,
    IValidator<RegisterDto> registerValidator,
    IValidator<UpdateDto> updateValidator,
    IValidator<ChangePasswordDto> changePasswordValidator
    ) : BaseController
{
    private readonly ILogger<UserController> _logger = logger;
    private readonly IUserService _userService = userService;
    private readonly IUserValidationService _userValidationService = userValidationService;
    private readonly IRoleService _roleService = roleService;
    private readonly ICourseService _courseService = courseService;
    private readonly ICatalogService _catalogService = catalogService;
    private readonly IImageValidationService _imageValidationService = imageValidationService;
    private readonly IValidator<RegisterDto> _registerValidator = registerValidator;
    private readonly IValidator<UpdateDto> _updateValidator = updateValidator;
    private readonly IValidator<ChangePasswordDto> _changePasswordValidator = changePasswordValidator;

    /// <summary>
    /// Lista todos los usuarios
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN},{SystemRoles.USERS_ADMIN}")]
    public async Task<IActionResult> Index()
    {
        var result = await _userService.GetAllUsersAsync();
        List<UserDto> users = [];
        // si el usuario actual es Admin Académico, mostrar los usuarios con roles de estudiante e instructor
        var currentUserRoles = AuditContext.UserRole?.Split(',') ?? [];
        if (currentUserRoles.Contains(SystemRoles.ACADEMIC_ADMIN))
        {
            users = result.Value
            .Where(x => x.Roles.Any(r => r.Name == SystemRoles.STUDENT || r.Name == SystemRoles.INSTRUCTOR))
            .ToList();
        }
        else
        {
            users = (List<UserDto>)result.Value;
        }
        return View(users);
    }

    /// <summary>
    /// Muestra los detalles de un usuario específico
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Details(Guid id, Guid? courseId = null, string? returnUrl = null)
    {
        // Validar permisos: o es admin, o es el propio usuario viendo su perfil
        var currentUserId = AuditContext.LoggedUserId;
        var currentUserRoles = AuditContext.UserRole?.Split(',') ?? [];
        var isAdmin = currentUserRoles.Contains(SystemRoles.ACADEMIC_ADMIN) || currentUserRoles.Contains(SystemRoles.USERS_ADMIN);
        var isOwnProfile = currentUserId == id;

        if (!isAdmin && !isOwnProfile)
        {
            TempData[NotificationConstants.Error] = "No tiene permisos para ver este perfil";
            return RedirectToAction(nameof(HomeController.Index), "Home", new { area = "" });
        }

        var result = await _userService.GetUserByIdAsync(id);
        if (!result.IsSuccess)
        {
            _logger.LogError("Error: Usuario con id {id}, {code}: {message}", id, result.Error.Code, result.Error.Message);
            TempData[NotificationConstants.Error] = $"Error: {result.Error.Message}";
            return RedirectToAction(nameof(Index));
        }

        // Pasar información de navegación a la vista
        ViewBag.CourseId = courseId;
        ViewBag.ReturnUrl = returnUrl;

        return View(result.Value);
    }

    /// <summary>
    /// Muestra el perfil del usuario actual
    /// </summary>
    [HttpGet]
    [Authorize]
    public IActionResult Profile()
    {
        var currentUserId = AuditContext.LoggedUserId;
        if (currentUserId == Guid.Empty)
            return RedirectToAction(nameof(AccountController.Login), "Account", new { area = "Identity" });

        // Redirigir a Details con returnUrl indicando que viene del perfil
        return RedirectToAction(nameof(Details), new { id = currentUserId, returnUrl = "profile" });
    }

    /// <summary>
    /// Muestra el formulario para crear un nuevo usuario
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN},{SystemRoles.USERS_ADMIN}")]
    public async Task<IActionResult> Create()
    {
        // Preparar las listas desplegables
        await PrepareViewBags();
        return View();
    }

    /// <summary>
    /// Procesa la creación de un nuevo usuario
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN},{SystemRoles.USERS_ADMIN}")]
    public async Task<IActionResult> Create(RegisterDto model, IFormFile? photoFile = null)
    {
        // Validar modelo y permisos
        model.Force ??= "PONAL";
        model.IdentificationNumber = model.IdentificationNumber.Trim();
        model.Name = model.Name.Trim();
        model.LastName = model.LastName.Trim();
        model.Username = model.Username.Trim().ToLower();
        var isValid = await _userValidationService.ValidateCreateUserAsync(model, photoFile, AuditContext.UserRole ?? "", ModelState);
        if (!isValid)
        {
            await PrepareViewBags(model);
            return View(model);
        }

        // Crear usuario
        var result = await _userService.CreateUserAsync(model);
        if (!result.IsSuccess)
        {
            _logger.LogError("Error: {code}: {message}", result.Error.Code, result.Error.Message);
            ModelState.AddModelError(string.Empty, result.Error.Message);
            await PrepareViewBags(model);
            return View(model);
        }

        // Auditoría con usuario afectado
        var auditInfo = CreateUserAuditInfo(result.Value.Id, $"{result.Value.FullIdentification} - {result.Value.FullName}", DatabaseOperationType.Create);
        auditInfo.NewValues = JsonSerializer.Serialize(result.Value);
        await SaveCustomAuditLogAsync(auditInfo);

        TempData[NotificationConstants.Success] = "Usuario creado exitosamente. La contraseña es el número de identificación.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Muestra el formulario para editar un usuario
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN},{SystemRoles.USERS_ADMIN}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        if (!result.IsSuccess)
        {
            var error = result.Error;
            _logger.LogError("Error: Usuario con id {id}, {code}: {message}", id, error.Code, error.Message);
            TempData[NotificationConstants.Error] = $"Error: {error.Message}";
            return RedirectToAction(nameof(Index));
        }

        var user = result.Value;
        var model = user.MapUserBaseProperties<UserDto, UpdateDto>();
        model.Id = id;
        model.SelectedRoleIds = [.. result.Value.Roles.Select(r => r.Id)];
        model.WantToLock = model.IsLockedOut;

        // Mapear datos del perfil de aviación si existe
        if (user.AviationProfile != null)
        {
            model.PID = user.AviationProfile.PID;
            model.FlightPosition = user.AviationProfile.FlightPosition;
            model.WingType = user.AviationProfile.WingType;
        }

        // Pasar información adicional para el script de la vista
        ViewBag.HasAviationProfile = user.AviationProfile != null;
        ViewBag.CurrentRoles = JsonSerializer.Serialize(user.Roles.Select(r => r.Name).ToList());

        // Preparar las listas desplegables
        await PrepareViewBags(model, model.WingType, model.FlightPosition);

        return View(model);
    }

    /// <summary>
    /// Procesa la edición de un usuario
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN},{SystemRoles.USERS_ADMIN}")]
    public async Task<IActionResult> Edit(UpdateDto model, IFormFile? photoFile = null)
    {
        // Validar modelo y permisos
        model.IdentificationNumber = model.IdentificationNumber.Trim();
        model.Name = model.Name.Trim();
        model.LastName = model.LastName.Trim();
        model.Username = model.Username.Trim().ToLower();
        var isValid = await _userValidationService.ValidateUpdateUserAsync(model, photoFile, AuditContext.UserRole ?? "", ModelState);
        if (!isValid)
        {
            await PrepareViewBags(model, model.WingType, model.FlightPosition);
            return View(model);
        }

        // Obtener valores anteriores para auditoría
        var oldUserResult = await _userService.GetUserByIdAsync(model.Id);
        var oldUser = oldUserResult.IsSuccess ? oldUserResult.Value : null;

        // Actualizar usuario
        var result = await _userService.UpdateUserAsync(model);
        if (!result.IsSuccess)
        {
            _logger.LogError("Error: {code}: {message}", result.Error.Code, result.Error.Message);
            ModelState.AddModelError(string.Empty, result.Error.Message);
            // Preparar las listas desplegables
            await PrepareViewBags(model, model.WingType, model.FlightPosition);
            return View(model);
        }

        // Manejar auditoría de forma centralizada
        await HandleUserUpdateAuditAsync(model, oldUser, result.Value);

        // retorno
        TempData[NotificationConstants.Success] = "El registro se actualizó correctamente.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Muestra el formulario para cambiar contraseña
    /// </summary>
    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    /// <summary>
    /// Procesa el cambio de contraseña del usuario actual
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
    {
        // Obtener el ID del usuario actual
        var currentUserId = AuditContext.LoggedUserId;
        if (currentUserId == Guid.Empty)
            return RedirectToAction(nameof(AccountController.Login), "Account", new { area = "Identity" });

        // Validar modelo
        var isValid = await _userValidationService.ValidateChangePasswordAsync(model, ModelState);
        if (!isValid)
        {
            return View(model);
        }

        // Cambiar la contraseña
        var result = await _userService.ChangePasswordAsync(currentUserId, model.CurrentPassword, model.NewPassword);
        if (!result.IsSuccess)
        {
            _logger.LogError("Error al cambiar contraseña del usuario con id {id}, {code}: {message}", currentUserId, result.Error.Code, result.Error.Message);

            // Manejar errores específicos
            if (result.Error.Code == SystemErrors.UserError.InvalidPassword.Code)
                ModelState.AddModelError(nameof(model.CurrentPassword), result.Error.Message);
            else
                ModelState.AddModelError(string.Empty, result.Error.Message);

            return View(model);
        }

        // Auditoría
        await SaveAuditLogAsync(EntityNames.Identity.User, currentUserId, DatabaseOperationType.ChangePassword, null, "Usuario cambió su contraseña.");

        TempData[NotificationConstants.Success] = "Su contraseña ha sido cambiada exitosamente.";

        return RedirectToAction(nameof(Profile));
    }

    /// <summary>
    /// Restablece la contraseña de un usuario
    /// </summary>
    [HttpPost]
    [Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN},{SystemRoles.USERS_ADMIN}")]
    public async Task<JsonResult> ResetPassword(Guid id)
    {
        try
        {
            var result = await _userService.AdminResetPasswordAsync(id);
            if (!result.IsSuccess)
            {
                return JsonError("Error al restablecer la contraseña");
            }

            // Auditoría
            // Obtener información del usuario afectado
            var userResult = await _userService.GetUserByIdAsync(id);
            if (userResult.IsSuccess)
            {
                var auditInfo = CreateUserAuditInfo(id, $"{userResult.Value.FullIdentification} - {userResult.Value.FullName}", DatabaseOperationType.AdminResetPassword);
                await SaveCustomAuditLogAsync(auditInfo);
            }

            return JsonSuccess("Contraseña restablecida exitosamente. " +
                "La nueva contraseña es el número de identificación del usuario.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al restablecer la contraseña del usuario con id {id}", id);
            await SaveLogErrorAsync(ex, $"Error al restablecer contraseña del usuario {id}");
            return JsonError("Error al restablecer la contraseña");
        }
    }

    #region Métodos privados auxiliares
    private async Task PrepareViewBags(UserBase? model = null, string? wingTypes = null, string? flightPosition = null)
    {
        var result = await _catalogService.GetByCatalogTypesAsync(
            CatalogTypes.DOCUMENT_TYPE, CatalogTypes.MILITARY_GRADE, CatalogTypes.FORCE,
            CatalogTypes.NATIONALITY, CatalogTypes.BLOOD_TYPE, CatalogTypes.FLIGHT_POSITION
        );

        if (!result.IsSuccess)
        {
            _logger.LogError("Error al obtener catálogos: {code}: {message}", result.Error.Code, result.Error.Message);
            return;
        }

        var catalogs = result.Value;

        ViewBag.DocumentTypes = new SelectList(catalogs[CatalogTypes.DOCUMENT_TYPE], "Code", "Name", model?.DocumentType);
        ViewBag.MilitaryGrades = new SelectList(catalogs[CatalogTypes.MILITARY_GRADE], "Code", "Name", model?.Grade);
        ViewBag.BloodTypes = new SelectList(catalogs[CatalogTypes.BLOOD_TYPE], "Code", "Name", model?.BloodType);

        // el select2 de la vista no permite que se muestre el item selected si no hace parte del modelo, por eso se agrega manualmente en la vista
        ViewBag.NationalityLog = catalogs[CatalogTypes.NATIONALITY].FirstOrDefault(c => c.Name == model?.Nationality);
        ViewBag.FlightPositionLog = catalogs[CatalogTypes.FLIGHT_POSITION].FirstOrDefault(c => c.Name == flightPosition);
        ViewBag.ForceLog = catalogs[CatalogTypes.FORCE].FirstOrDefault(c => c.Name == model?.Force);
        catalogs[CatalogTypes.NATIONALITY].Remove(ViewBag.NationalityLog!);
        catalogs[CatalogTypes.FLIGHT_POSITION].Remove(ViewBag.FlightPositionLog!);
        catalogs[CatalogTypes.FORCE].Remove(ViewBag.ForceLog!);

        ViewBag.Forces = new SelectList(catalogs[CatalogTypes.FORCE], "Code", "Name");
        ViewBag.Nationalities = new SelectList(catalogs[CatalogTypes.NATIONALITY], "Code", "Name");
        ViewBag.FlightPositions = new SelectList(catalogs[CatalogTypes.FLIGHT_POSITION], "Code", "Name");

        var wingTypesItems = new List<string> { AviationConstants.WingTypes.FIXED_WING, AviationConstants.WingTypes.ROTARY_WING };
        ViewBag.WingTypes = new SelectList(wingTypesItems, wingTypes);

        await PrepareRolesAsync();
        await PrepareActiveCoursesAsync(model);
    }

    private async Task PrepareRolesAsync()
    {
        try
        {
            var rolesResult = await _roleService.GetAllRolesAsync();
            if (rolesResult.IsSuccess)
            {
                List<RoleDto> availableRoles = [];
                // si el usuario actual es Admin Académico, mostrar los roles de estudiante, instructor
                var currentUserRoles = AuditContext.UserRole?.Split(',') ?? [];
                if (currentUserRoles.Contains(SystemRoles.ACADEMIC_ADMIN))
                {
                    availableRoles.AddRange([.. rolesResult.Value.Where(x => x.Name == SystemRoles.STUDENT || x.Name == SystemRoles.INSTRUCTOR)]);
                }

                if (currentUserRoles.Contains(SystemRoles.USERS_ADMIN))
                {
                    availableRoles.AddRange([.. rolesResult.Value.Where(x => x.Name == SystemRoles.USERS_ADMIN || x.Name == SystemRoles.ACADEMIC_ADMIN)]);
                }

                ViewBag.Roles = new SelectList(availableRoles, "Id", "Name");
            }
            else
            {
                ViewBag.Roles = new SelectList(new List<RoleDto>(), "Id", "Name");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar roles");
            ViewBag.Roles = new SelectList(new List<RoleDto>(), "Id", "Name");
        }
    }

    private async Task PrepareActiveCoursesAsync(UserBase? model)
    {
        try
        {
            // Solo cargar cursos si el usuario actual es Admin Académico
            var currentUserRoles = AuditContext.UserRole?.Split(',') ?? [];
            if (currentUserRoles.Contains(SystemRoles.ACADEMIC_ADMIN))
            {
                var coursesResult = await _courseService.GetActiveCoursesAsync();
                if (coursesResult.IsSuccess && coursesResult.Value.Any())
                {
                    var courses = coursesResult.Value
                        .Select(c => new SelectListItem
                        {
                            Value = c.Id.ToString(),
                            Text = $"{c.CourseNumber} - {c.CourseName}"
                        }).ToList();

                    // Si estamos editando y el modelo tiene CourseId, seleccionarlo
                    Guid? selectedCourseId = null;
                    if (model is RegisterDto registerDto) selectedCourseId = registerDto.CourseId;
                    else if (model is UpdateDto updateDto) selectedCourseId = updateDto.CourseId;

                    ViewBag.ActiveCourses = new SelectList(courses, "Value", "Text", selectedCourseId);
                }
                else
                {
                    ViewBag.ActiveCourses = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            else
            {
                ViewBag.ActiveCourses = new SelectList(new List<SelectListItem>(), "Value", "Text");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar programas activos");
            ViewBag.ActiveCourses = new SelectList(new List<SelectListItem>(), "Value", "Text");
        }
    }

    private async Task HandleUserUpdateAuditAsync(UpdateDto model, UserDto? oldUser, UserDto newUser)
    {
        // Verificar si hubo cambios en los datos del usuario (excluyendo el bloqueo)
        bool hasDataChanges = HasUserDataChanges(oldUser, newUser);
        bool hasLockStatusChange = oldUser?.IsLockedOut != model.WantToLock;

        // Si hubo cambios en los datos, registrar auditoría de actualización
        if (hasDataChanges)
        {
            var auditInfo = CreateUserAuditInfo(model.Id, $"{newUser.FullIdentification} - {newUser.FullName}", DatabaseOperationType.Update);
            auditInfo.OldValues = JsonSerializer.Serialize(oldUser);
            auditInfo.NewValues = JsonSerializer.Serialize(newUser);
            await SaveCustomAuditLogAsync(auditInfo);
        }

        // Si hubo cambio en el estado de bloqueo Y no hubo otros cambios, registrar solo el bloqueo/desbloqueo
        if (hasLockStatusChange && !hasDataChanges)
        {
            if (model.WantToLock)
            {
                var fechaBloqueo = model.LockoutEnd.HasValue
                    ? $"hasta {model.LockoutEnd.Value:dd/MM/yyyy}"
                    : "indefinidamente";
                string newValue = $"Usuario bloqueado {fechaBloqueo}. Razón: {model.LockoutReason}";

                var auditInfo = CreateUserAuditInfo(
                    model.Id,
                    $"{newUser.FullIdentification} - {newUser.FullName}",
                    DatabaseOperationType.Lock
                );
                auditInfo.NewValues = JsonSerializer.Serialize(newValue);
                await SaveCustomAuditLogAsync(auditInfo);
            }
            else
            {
                var auditInfo = CreateUserAuditInfo(
                    model.Id,
                    $"{newUser.FullIdentification} - {newUser.FullName}",
                    DatabaseOperationType.Unlock
                );
                await SaveCustomAuditLogAsync(auditInfo);
            }
        }
        // Si hubo cambios en datos Y en el estado de bloqueo
        else if (hasLockStatusChange && hasDataChanges)
        {
            // La información del bloqueo ya está incluida en el registro de Update
            // Al existir cambios en los datos, se registra la actualización junto con el bloqueo
            _logger.LogInformation(
                "Usuario {UserId} actualizado con cambio de estado de bloqueo: {LockStatus}",
                model.Id,
                model.WantToLock ? "Bloqueado" : "Desbloqueado"
            );
        }
    }

    private bool HasUserDataChanges(UserDto? oldUser, UserDto newUser)
    {
        if (oldUser == null) return true;

        // Comparar propiedades relevantes (excluyendo IsLockedOut ya que se maneja aparte)
        return
               oldUser.DocumentType != newUser.DocumentType ||
               oldUser.IdentificationNumber != newUser.IdentificationNumber ||
               oldUser.Grade != newUser.Grade ||
               oldUser.Name != newUser.Name ||
               oldUser.LastName != newUser.LastName ||
               oldUser.Nationality != newUser.Nationality ||
               oldUser.BloodType != newUser.BloodType ||
               oldUser.BirthDate != newUser.BirthDate ||
               oldUser.Force != newUser.Force ||
               oldUser.SeniorityOrder != newUser.SeniorityOrder ||
               oldUser.Email != newUser.Email ||
               oldUser.Username != newUser.Username ||
               oldUser.PhoneNumber != newUser.PhoneNumber ||
               oldUser.PhotoData != newUser.PhotoData ||
               !AreRolesEqual(oldUser.Roles, newUser.Roles) ||
               HasAviationProfileChanges(oldUser.AviationProfile, newUser.AviationProfile);
    }

    private bool AreRolesEqual(IEnumerable<RoleDto> oldRoles, IEnumerable<RoleDto> newRoles)
    {
        var oldRoleIds = oldRoles?.Select(r => r.Id).OrderBy(id => id).ToList() ?? [];
        var newRoleIds = newRoles?.Select(r => r.Id).OrderBy(id => id).ToList() ?? [];

        return oldRoleIds.SequenceEqual(newRoleIds);
    }

    private static bool HasAviationProfileChanges(AviationProfileDto? oldProfile, AviationProfileDto? newProfile)
    {
        if (oldProfile == null && newProfile == null) return false;
        if (oldProfile == null || newProfile == null) return true;

        return oldProfile.PID != newProfile.PID || oldProfile.WingType != newProfile.WingType || oldProfile.FlightPosition != newProfile.FlightPosition;
    }
    #endregion
}