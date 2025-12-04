namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para mostrar el detalle completo de una misión evaluada
/// </summary>
public class MissionEvaluationDetailDto
{
    // Información del estudiante
    public Guid StudentId { get; set; }
    public string StudentFullName { get; set; } = string.Empty;
    public string StudentIdentification { get; set; } = string.Empty;
    public string? PhotoBase64 { get; set; }

    // Información del instructor
    public Guid InstructorId { get; set; }
    public string InstructorFullName { get; set; } = string.Empty;
    public string InstructorIdentification { get; set; } = string.Empty;

    // Información del curso y fase
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public Guid PhaseId { get; set; }
    public string PhaseName { get; set; } = string.Empty;
    public int PhaseNumber { get; set; }

    // Información de la misión
    public Guid MissionId { get; set; }
    public string MissionName { get; set; } = string.Empty;
    public int MissionNumber { get; set; }
    public double FlightHours { get; set; }
    public string WingType { get; set; } = string.Empty;

    // Información de la evaluación
    public Guid StudentMissionProgressId { get; set; }
    public DateTime EvaluationDate { get; set; }
    public bool MissionPassed { get; set; }
    public int CriticalFailures { get; set; }
    public string? GeneralObservations { get; set; }

    // Información de la aeronave
    public string AircraftRegistration { get; set; } = string.Empty;
    public string AircraftType { get; set; } = string.Empty;

    // Calificaciones de tareas
    public IEnumerable<TaskGradeDetailDto> TaskGrades { get; set; } = [];

    // Información de horas de vuelo
    public double MachineFlightHours { get; set; }
    public double ManFlightHours { get; set; }
    public double SilaboFlightHours { get; set; }
}

/// <summary>
/// DTO para detalle de calificación de tarea
/// </summary>
public class TaskGradeDetailDto
{
    public Guid TaskId { get; set; }
    public int TaskCode { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;

    // Motivos de N-Roja si aplica
    public IEnumerable<NRedReasonDto> NRedReasons { get; set; } = [];
}