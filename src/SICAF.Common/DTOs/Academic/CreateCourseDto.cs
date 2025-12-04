using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para crear un nuevo programa
/// </summary>
public class CreateCourseDto
{
    [Display(Name = "Número del programa")]
    [Required(ErrorMessage = "El número del programa es obligatorio")]
    [Range(1, 999, ErrorMessage = "El número debe estar entre 1 y 999")]
    public int CourseNumber { get; set; }

    [Display(Name = "Nombre del programa")]
    [Required(ErrorMessage = "El nombre del programa es obligatorio")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "El nombre debe tener entre 5 y 200 caracteres")]
    public string CourseName { get; set; } = string.Empty;

    [Display(Name = "Descripción")]
    [Required(ErrorMessage = "La descripción es obligatoria")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 500 caracteres")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Fecha de Inicio")]
    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    [DataType(DataType.Date)]
    public string StartDateString { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");

    [Display(Name = "Fecha de Finalización")]
    [Required(ErrorMessage = "La fecha de finalización es obligatoria")]
    [DataType(DataType.Date)]
    public string EndDateString { get; set; } = DateTime.Today.AddMonths(6).ToString("yyyy-MM-dd");

    // Propiedades calculadas para el mapeo
    public DateTime StartDate => DateTime.Parse(StartDateString);
    public DateTime EndDate => DateTime.Parse(EndDateString);
}