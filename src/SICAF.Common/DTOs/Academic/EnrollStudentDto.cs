using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para enrollar un estudiante en un programa
/// </summary>
public class EnrollStudentDto
{
    [Required(ErrorMessage = "Debe seleccionar un estudiante")]
    [Display(Name = "Estudiante")]
    public Guid StudentId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un programa")]
    [Display(Name = "programa")]
    public Guid CourseId { get; set; }

    [Display(Name = "Observaciones")]
    [StringLength(1000, ErrorMessage = "Las observaciones no pueden exceder los 1000 caracteres")]
    public string? Observations { get; set; }
}