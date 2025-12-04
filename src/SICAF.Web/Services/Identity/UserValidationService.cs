using FluentValidation;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using SICAF.Business.Interfaces.Identity;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Identity;
using SICAF.Web.Interfaces.Files;
using SICAF.Web.Interfaces.Identity;

namespace SICAF.Web.Services.Identity;

/// <summary>
/// Servicio de validación de usuarios
/// </summary>
public class UserValidationService(
    IRoleService roleService,
    IImageValidationService imageValidationService,
    IValidator<RegisterDto> registerValidator,
    IValidator<UpdateDto> updateValidator,
    IValidator<ChangePasswordDto> changePasswordValidator
    ) : IUserValidationService
{
    private readonly IRoleService _roleService = roleService;
    private readonly IImageValidationService _imageValidationService = imageValidationService;
    private readonly IValidator<RegisterDto> _registerValidator = registerValidator;
    private readonly IValidator<UpdateDto> _updateValidator = updateValidator;
    private readonly IValidator<ChangePasswordDto> _changePasswordValidator = changePasswordValidator;

    public async Task<bool> ValidateCreateUserAsync(RegisterDto model, IFormFile? photoFile, string currentUserRoles, ModelStateDictionary modelState)
    {
        var isValid = true;

        // Validar fecha de nacimiento
        if (!DateTime.TryParse(model.BirthDateString, out DateTime birthDate))
        {
            modelState.AddModelError(nameof(model.BirthDateString), "La fecha de nacimiento debe ser una fecha válida.");
            isValid = false;
        }
        else
        {
            model.BirthDate = birthDate;
        }

        // Validación con FluentValidation
        var validationResult = await _registerValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            isValid = false;
        }

        // Validar roles seleccionados
        if (model.SelectedRoleIds?.Count == 0)
        {
            modelState.AddModelError(string.Empty, "Debe seleccionar al menos un rol.");
            isValid = false;
        }
        else if (model.SelectedRoleIds != null)
        {
            var roleValidationSuccess = await ValidateRoleAssignmentAsync(model.SelectedRoleIds, currentUserRoles, modelState);
            if (!roleValidationSuccess) isValid = false;
        }

        // Validar archivo de foto
        if (photoFile != null && photoFile.Length > 0)
        {
            var photoValidation = await _imageValidationService.ValidateAndProcessImageAsync(photoFile);
            if (!photoValidation.IsValid)
            {
                modelState.AddModelError(string.Empty, photoValidation.ErrorMessage!);
                isValid = false;
            }
            else
            {
                model.PhotoData = photoValidation.ImageData;
                model.PhotoContentType = photoValidation.ContentType;
                model.PhotoFileName = photoValidation.FileName;
            }
        }

        return isValid;
    }

    public async Task<bool> ValidateUpdateUserAsync(UpdateDto model, IFormFile? photoFile, string currentUserRoles, ModelStateDictionary modelState)
    {
        var isValid = true;

        // Validar fecha de nacimiento
        if (!DateTime.TryParse(model.BirthDateString, out DateTime birthDate))
        {
            modelState.AddModelError(nameof(model.BirthDateString), "La fecha de nacimiento debe ser una fecha válida.");
            isValid = false;
        }
        else
        {
            model.BirthDate = birthDate;
        }

        // Validación con FluentValidation
        var validationResult = await _updateValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            isValid = false;
        }

        // Validar permisos para asignar roles
        if (model.SelectedRoleIds?.Count == 0)
        {
            modelState.AddModelError(string.Empty, "Debe seleccionar al menos un rol.");
            isValid = false;
        }
        else if (model.SelectedRoleIds != null)
        {
            var validationRolesResult = await _roleService.ValidateRoleAssignmentAsync(model.Id, model.SelectedRoleIds, currentUserRoles);
            if (!validationRolesResult.IsSuccess)
            {
                modelState.AddModelError(string.Empty, validationRolesResult.Error.Message);
                isValid = false;
            }
        }

        // Validar archivo de foto
        if (photoFile != null && photoFile.Length > 0)
        {
            var photoValidation = await _imageValidationService.ValidateAndProcessImageAsync(photoFile);
            if (!photoValidation.IsValid)
            {
                modelState.AddModelError(string.Empty, photoValidation.ErrorMessage!);
                isValid = false;
            }
            else
            {
                model.PhotoData = photoValidation.ImageData;
                model.PhotoContentType = photoValidation.ContentType;
                model.PhotoFileName = photoValidation.FileName;
            }
        }

        return isValid;
    }

    public async Task<bool> ValidateChangePasswordAsync(ChangePasswordDto model, ModelStateDictionary modelState)
    {
        var validationResult = await _changePasswordValidator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return false;
        }

        return true;
    }

    private async Task<bool> ValidateRoleAssignmentAsync(List<Guid> selectedRoleIds, string currentUserRoles, ModelStateDictionary modelState)
    {
        var isValid = true;

        var roles = await _roleService.GetRolesAsync(selectedRoleIds);
        var aviationRoles = roles.Where(r => AviationConstants.AviationRoles.Contains(r.Name)).ToList();
        var aviationRolesNames = aviationRoles.Select(r => r.Name);

        if (aviationRoles.Count > 0)
        {
            // Validar que si es rol de aviación, solo lo puede asignar Admin Académico
            var currentUserRolesList = currentUserRoles.Split(',');
            if (!currentUserRolesList.Contains(SystemRoles.ACADEMIC_ADMIN))
            {
                modelState.AddModelError(string.Empty, $"Solo el Administrador Académico puede asignar los roles de: {string.Join(", ", aviationRolesNames)}.");
                isValid = false;
            }

            // Validar restricción del rol Estudiante, que no puede tener otros roles
            if (aviationRolesNames.Contains(SystemRoles.STUDENT) && aviationRoles.Count > 1)
            {
                modelState.AddModelError(string.Empty, "El rol Estudiante no puede tener otros roles.");
                isValid = false;
            }
        }

        return isValid;
    }
}