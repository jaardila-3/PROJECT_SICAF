using SICAF.Common.DTOs.Academic;

namespace SICAF.Common.DTOs.Instructor;

/// <summary>
/// DTO principal para la vista de evaluación del estudiante
/// </summary>
public class EvaluationOverviewDto
{
    // Información del estudiante
    public Guid StudentId { get; set; }
    public string StudentFullName { get; set; } = string.Empty;
    public string StudentIdentification { get; set; } = string.Empty;
    public string? PhotoBase64 { get; set; }

    // Información del instructor
    public Guid InstructorId { get; set; }

    // Información del curso
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;

    // Información de la fase actual
    public bool IsLastPhase { get; set; }
    public StudentPhaseInfoDto CurrentPhase { get; set; } = null!;

    // Misiones de la fase
    public IEnumerable<MissionStatusDto> CompletedMissions { get; set; } = [];
    public MissionToEvaluateDto? NextMissionToEvaluate { get; set; }

    // Estado general
    public int CompletedMissionsCount { get; set; }
    public int TotalMissionsInPhase { get; set; }

    // Información del estado del estudiante y si esta en comite
    public string Status { get; set; } = string.Empty; // Active, PendingCommittee, Suspended, etc.
    public StudentCommitteeRecordDto? PendingCommittee { get; set; }

    // Registros de comités de la fase actual
    public IEnumerable<StudentCommitteeRecordDto> CommitteeRecordsInCurrentPhase { get; set; } = [];

    /// <summary>
    /// Total de misiones fallidas en todo el curso
    /// </summary>
    public int TotalFailedMissionsInCourse { get; set; }

    /// <summary>
    /// Total de misiones fallidas en la ventana de las últimas misiones
    /// </summary>
    public int FailedMissionsInWindow { get; set; }

    public List<NonEvaluableMissionDto> NonEvaluableMissions { get; set; } = [];
}
