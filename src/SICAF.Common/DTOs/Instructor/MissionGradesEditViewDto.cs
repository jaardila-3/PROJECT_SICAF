using SICAF.Common.DTOs.Academic;

namespace SICAF.Common.DTOs.Instructor;

public class MissionGradesEditViewDto
{
    public Guid MissionId { get; set; }
    public string MissionName { get; set; } = string.Empty;
    public int MissionNumber { get; set; }
    public DateTime EvaluationDate { get; set; }
    public Guid StudentId { get; set; }
    public string StudentFullName { get; set; } = string.Empty;
    public string StudentFullIdentification { get; set; } = string.Empty;
    public Guid CourseId { get; set; }
    public Guid PhaseId { get; set; }
    public Guid EvaluatorInstructorId { get; set; }
    public string GeneralObservations { get; set; } = string.Empty;
    public List<TaskGradeEditDto> TaskGrades { get; set; } = [];

    // Info para alertas
    public int CurrentFailedMissionsInWindow { get; set; }
    public int CurrentTotalFailedMissions { get; set; }
    public bool IsMissionCurrentlyFailed { get; set; }
}

public class TaskGradeEditDto
{
    public Guid TaskGradeId { get; set; }
    public Guid TaskId { get; set; }
    public int TaskCode { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string CurrentGrade { get; set; } = string.Empty;
    public bool IsP3 { get; set; }
    public bool CanEdit { get; set; }
    public string? ReasonCannotEdit { get; set; }
    public int EditCount { get; set; }
    public List<NRedReasonDto> CurrentNRedReasons { get; set; } = [];
}