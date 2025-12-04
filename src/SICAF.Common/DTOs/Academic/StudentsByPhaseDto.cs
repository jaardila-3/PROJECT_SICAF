namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para agrupar estudiantes por fase
/// </summary>
public class StudentsByPhaseDto
{
    public Guid PhaseId { get; set; }
    public int PhaseNumber { get; set; }
    public string PhaseName { get; set; } = string.Empty;
    public int StudentCount { get; set; }
    public IEnumerable<UserCourseDto> Students { get; set; } = [];
}