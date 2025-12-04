using System.ComponentModel.DataAnnotations;

using SICAF.Common.DTOs.Academic;

namespace SICAF.Common.DTOs.Instructor;

/// <summary>
/// DTO para entrada de calificación de tarea regular
/// </summary>
public class TaskGradeInputDto
{
    public Guid TaskId { get; set; }

    [Required]
    [Display(Name = "Calificación")]
    public string Grade { get; set; } = string.Empty;

    [Display(Name = "Motivos N-Roja")]
    public IList<NRedReasonDto> NRedReasons { get; set; } = [];
}