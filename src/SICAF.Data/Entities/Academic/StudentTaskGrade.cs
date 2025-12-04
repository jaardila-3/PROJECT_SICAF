using SICAF.Data.Entities.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Entities.Academic;

public class StudentTaskGrade : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid InstructorId { get; set; }
    public Guid MissionId { get; set; }
    public Guid TaskId { get; set; }
    public Guid StudentMissionProgressId { get; set; }

    public string Grade { get; set; } = string.Empty;
    public DateTime Date { get; set; }

    // campos para control de ediciones
    public int EditCount { get; set; } = 0;
    public DateTime? LastEditDate { get; set; }

    // Relaciones
    public virtual User Student { get; set; } = null!;
    public virtual User Instructor { get; set; } = null!;
    public virtual Mission Mission { get; set; } = null!;
    public virtual Tasks Task { get; set; } = null!;
    public virtual StudentMissionProgress StudentMissionProgress { get; set; } = null!;
    public virtual ICollection<StudentGradeNRedReason> StudentGradeNRedReasons { get; set; } = [];

}