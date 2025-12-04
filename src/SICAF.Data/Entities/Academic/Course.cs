using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Academic;

/// <summary>
/// Entidad que representa un curso académico
/// </summary>
public class Course : BaseEntity
{
    public string CourseName { get; set; } = string.Empty;
    public int CourseNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Relaciones
    public virtual ICollection<UserCourse> UserCourses { get; set; } = [];
    public virtual ICollection<StudentPhaseProgress> StudentPhaseProgresses { get; set; } = [];
}