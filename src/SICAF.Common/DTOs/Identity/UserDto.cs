using System.ComponentModel.DataAnnotations;

using SICAF.Common.DTOs.Academic;
using SICAF.Common.DTOs.Common;

namespace SICAF.Common.DTOs.Identity;

/// <summary>
/// DTO de usuario
/// </summary>
public class UserDto : UserBase
{
    public Guid Id { get; set; }
    public bool IsPasswordSetByAdmin { get; set; }

    /// <summary>
    /// Roles asignados al usuario
    /// </summary>
    [Display(Name = "Roles")]
    public List<RoleDto> Roles { get; set; } = [];

    /// <summary>
    /// Perfil de aviación (para estudiantes, instructores y líderes de vuelo)
    /// </summary>
    public AviationProfileDto? AviationProfile { get; set; }

}