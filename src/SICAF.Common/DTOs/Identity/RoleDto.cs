using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Identity;

/// <summary>
/// DTO de rol
/// </summary>
public class RoleDto
{
    public Guid Id { get; set; }

    [Display(Name = "Nombre del Rol")]
    [Required(ErrorMessage = "El nombre del rol es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre del rol no puede exceder los 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Descripción")]
    [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
    public string? Description { get; set; }

    /// <summary>
    /// Indica si el rol es del sistema (no se puede eliminar)
    /// </summary>
    public bool IsSystemRole { get; set; }

    /// <summary>
    /// Usuarios asociados al rol
    /// </summary>
    public List<UserDto> Users { get; set; } = [];
}
