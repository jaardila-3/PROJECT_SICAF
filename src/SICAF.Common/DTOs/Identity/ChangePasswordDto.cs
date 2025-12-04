using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Identity;

public class ChangePasswordDto
{
    [Required(ErrorMessage = "La contraseña actual es obligatoria")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña Actual")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
    [DataType(DataType.Password)]
    [Display(Name = "Nueva Contraseña")]
    public string NewPassword { get; set; } = string.Empty;

    [Compare(nameof(NewPassword), ErrorMessage = "Las contraseñas no coinciden")]
    [Required(ErrorMessage = "La confirmación de la nueva contraseña es obligatoria")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar Nueva Contraseña")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}