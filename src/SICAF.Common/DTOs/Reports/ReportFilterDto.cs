using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Reports;

public class ReportFilterDto
{
    [Required(ErrorMessage = "El programa es requerido")]
    public Guid? CourseId { get; set; }

    [Required(ErrorMessage = "La fuerza es requerida")]
    public string Force { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de ala es requerido")]
    public string WingType { get; set; } = string.Empty;

    public Guid? PhaseId { get; set; }

    public Guid? StudentId { get; set; }
}