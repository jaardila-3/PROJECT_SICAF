using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Instructor;

/// <summary>
/// DTO para guardar la evaluación de una misión
/// </summary>
public class SaveMissionEvaluationDto
{
    [Required]
    public Guid StudentId { get; set; }

    [Required]
    public Guid InstructorId { get; set; }

    [Required]
    public Guid MissionId { get; set; }

    [Required]
    public Guid PhaseId { get; set; }

    [Required]
    public Guid CourseId { get; set; }

    [Required]
    public Guid AircraftId { get; set; }

    [Required]
    [Display(Name = "Fecha de evaluación")]
    public DateTime EvaluationDate { get; set; } = DateTime.Now;

    [Display(Name = "Observaciones generales de la misión")]
    public string? GeneralObservations { get; set; }

    // Horas de vuelo máquina
    [Required]
    public double MachineFlightHours { get; set; }

    // para saber desde que vista se va a guardar la evaluación
    public string ViewType { get; set; } = string.Empty;

    // Calificaciones de tareas regulares de la misión
    [Required]
    public IList<TaskGradeInputDto> TaskGrades { get; set; } = [];
}