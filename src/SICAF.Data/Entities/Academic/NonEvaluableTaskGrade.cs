using SICAF.Data.Entities.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Entities.Academic;

public class NonEvaluableTaskGrade : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid InstructorId { get; set; }
    public Guid TaskId { get; set; }
    public Guid NonEvaluableMissionRecordId { get; set; }

    public string Grade { get; set; } = string.Empty; // Solo permite: A, B, C, N, DM, SC
    public DateTime Date { get; set; }

    // Campos para control de ediciones
    public int EditCount { get; set; } = 0;
    public DateTime? LastEditDate { get; set; }


    // Relaciones
    public virtual User Student { get; set; } = null!;
    public virtual User Instructor { get; set; } = null!;
    public virtual Tasks Task { get; set; } = null!;
    public virtual NonEvaluableMissionRecord NonEvaluableMissionRecord { get; set; } = null!;
    public virtual ICollection<NonEvaluableGradeReason> NonEvaluableGradeReasons { get; set; } = [];
}
