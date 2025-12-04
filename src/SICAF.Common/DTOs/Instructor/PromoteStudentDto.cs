using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Instructor;

public class PromoteStudentDto
{
    [Required]
    public Guid StudentId { get; set; }

    [Required]
    public Guid CourseId { get; set; }

    [Required]
    public Guid PhaseId { get; set; }

    [Required]
    public Guid LeaderId { get; set; }
}