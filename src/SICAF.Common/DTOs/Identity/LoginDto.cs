using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Identity;

public class LoginDto
{
    [Display(Name = "Usuario")]
    [Required(ErrorMessage = "El usuario es obligatorio")]
    public string Username { get; set; } = string.Empty;

    [DataType(DataType.Password), Display(Name = "Contraseña")]
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; init; } = string.Empty;
}