using System.ComponentModel.DataAnnotations;

using SICAF.Common.DTOs.Identity;

namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para mostrar la relación usuario-curso
/// </summary>
public class UserCourseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }

    /// <summary>
    /// Tipo de participación en el curso
    /// "STUDENT", "INSTRUCTOR", "FLIGHT_LEADER"
    /// </summary>
    public string ParticipationType { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de ala que manejan
    /// (redundante con AviationProfile pero útil para consultas)
    /// </summary>
    public string? WingType { get; set; }

    [Display(Name = "Fecha de Inscripción")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime AssignmentDate { get; set; }
    public bool IsActive { get; set; }

    [Display(Name = "Fecha de Baja")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? UnassignmentDate { get; set; }

    [Display(Name = "Motivo de Baja")]
    public string? UnassignmentReason { get; set; }

    [Display(Name = "Tipo de Ala")]
    public string StudentWingType => User?.AviationProfile?.WingType ?? "N/A";

    public string Status { get; set; } = string.Empty;

    // fase actual
    public Guid CurrentPhaseId { get; set; }
    public int CurrentPhaseNumber { get; set; }
    public string CurrentPhaseName { get; set; } = string.Empty;

    // Información del usuario
    public UserDto User { get; set; } = new UserDto();

    // Información del curso
    public CourseDto Course { get; set; } = new CourseDto();

}