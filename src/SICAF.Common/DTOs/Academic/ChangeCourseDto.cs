using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para cambiar un estudiante de programa
/// </summary>
public class ChangeCourseDto
{
    [Required]
    public Guid StudentId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar el nuevo programa")]
    [Display(Name = "Nuevo programa")]
    public Guid NewCourseId { get; set; }

    [Required(ErrorMessage = "Debe especificar el motivo del cambio")]
    [Display(Name = "Motivo del Cambio")]
    [StringLength(200, MinimumLength = 10, ErrorMessage = "El motivo debe tener entre 10 y 200 caracteres")]
    public string Reason { get; set; } = string.Empty;

    // Información de solo lectura
    public Guid CurrentCourseId { get; set; }
    public string StudentIdentificationName { get; set; } = string.Empty;
    public string CurrentCourseName { get; set; } = string.Empty;
}