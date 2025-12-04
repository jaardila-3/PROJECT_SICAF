using SICAF.Common.DTOs.Academic;

namespace SICAF.Common.DTOs.Instructor;

public class EditMissionGradesDto
{
    public Guid MissionId { get; set; }
    public Guid StudentId { get; set; }
    public string StudentIdentificationName { get; set; } = string.Empty;
    public Guid CourseId { get; set; }
    public Guid InstructorId { get; set; }
    public string GeneralObservations { get; set; } = string.Empty;

    // para saber desde que vista se va a guardar la evaluación
    public string ViewType { get; set; } = string.Empty;

    public List<EditTaskGradeDto> TaskGrades { get; set; } = [];
}

public class EditTaskGradeDto
{
    public Guid TaskGradeId { get; set; }
    public Guid TaskId { get; set; }
    public string OldGrade { get; set; } = string.Empty;
    public string NewGrade { get; set; } = string.Empty;
    public bool IsP3 { get; set; }
    public bool CanEdit { get; set; }
    public string? ReasonCannotEdit { get; set; }
    public List<NRedReasonDto> NRedReasons { get; set; } = [];
}