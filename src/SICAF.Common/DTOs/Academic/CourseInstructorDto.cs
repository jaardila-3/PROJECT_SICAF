using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para asignación de instructor a curso
/// </summary>
public class CourseInstructorDto
{
    public Guid CourseId { get; set; }
    public Guid InstructorId { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public string InstructorIdentification { get; set; } = string.Empty;
    public string ParticipationType { get; set; } = string.Empty; // INSTRUCTOR o FLIGHT_LEADER
    public string? WingType { get; set; }
    public DateTime AssignmentDate { get; set; }
    public bool IsActive { get; set; }
}