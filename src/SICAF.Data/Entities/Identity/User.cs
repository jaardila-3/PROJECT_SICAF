using SICAF.Data.Entities.Academic;
using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Identity;

/// <summary>
/// Entidad de usuario
/// BaseEntity: Entidad base con propiedades comunes Id, IsDeleted
/// </summary>
public class User : BaseEntity
{
    public string DocumentType { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string? Grade { get; set; }
    public int? SeniorityOrder { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string BloodType { get; set; } = string.Empty; //RH
    public DateTime BirthDate { get; set; }
    public string? Force { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime? LockoutEnd { get; set; }
    public string? LockoutReason { get; set; }
    public int AccessFailedCount { get; set; }
    public bool IsPasswordSetByAdmin { get; set; } = false;
    public DateTime? PasswordChangeDate { get; set; }

    public byte[]? PhotoData { get; set; }
    public string? PhotoContentType { get; set; }
    public string? PhotoFileName { get; set; }

    /*** relationships ***/
    public virtual AviationProfile? AviationProfile { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    public virtual ICollection<UserCourse> UserCourses { get; set; } = [];
    public virtual ICollection<StudentPhaseProgress> StudentPhaseProgresses { get; set; } = [];

    public virtual ICollection<StudentMissionProgress> StudentMissionProgressesAsStudent { get; set; } = [];

    public virtual ICollection<StudentTaskGrade> StudentTaskGradesAsStudent { get; set; } = [];
    public virtual ICollection<StudentTaskGrade> StudentTaskGradesAsInstructor { get; set; } = [];

    public virtual ICollection<FlightHourLog> FlightHourLogs { get; set; } = [];
}