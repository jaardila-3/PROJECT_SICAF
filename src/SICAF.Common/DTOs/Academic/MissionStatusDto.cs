namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para mostrar el estado de misiones completadas
/// </summary>
public class MissionStatusDto
{
    public Guid MissionId { get; set; }
    public string MissionName { get; set; } = string.Empty;
    public string GradeNameInstructor { get; set; } = string.Empty;
    public int MissionNumber { get; set; }
    public double FlightHours { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CompletionDate { get; set; }
    public bool MissionPassed { get; set; }
    public int CriticalFailures { get; set; }

    // Propiedades para edición de calificaciones
    public Guid? EvaluatorInstructorId { get; set; }
    public DateTime? EvaluationDate { get; set; }
    public int EditCount { get; set; }
    public bool CanEdit { get; set; }
}