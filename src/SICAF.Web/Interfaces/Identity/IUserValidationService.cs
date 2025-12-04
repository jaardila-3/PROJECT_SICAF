using Microsoft.AspNetCore.Mvc.ModelBinding;

using SICAF.Common.DTOs.Identity;

namespace SICAF.Web.Interfaces.Identity;

/// <summary>
/// Interfaz para el servicio de validación de usuarios
/// </summary>
public interface IUserValidationService
{
    /// <summary>
    /// Valida la creación de un usuario y actualiza el ModelState
    /// </summary>
    Task<bool> ValidateCreateUserAsync(RegisterDto model, IFormFile? photoFile, string currentUserRoles, ModelStateDictionary modelState);

    /// <summary>
    /// Valida la actualización de un usuario y actualiza el ModelState
    /// </summary>
    Task<bool> ValidateUpdateUserAsync(UpdateDto model, IFormFile? photoFile, string currentUserRoles, ModelStateDictionary modelState);

    /// <summary>
    /// Valida el cambio de contraseña y actualiza el ModelState
    /// </summary>
    Task<bool> ValidateChangePasswordAsync(ChangePasswordDto model, ModelStateDictionary modelState);
}