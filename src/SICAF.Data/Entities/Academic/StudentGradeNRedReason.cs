using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Academic;

public class StudentGradeNRedReason : BaseEntity
{
    public Guid StudentTaskGradeId { get; set; }

    // Motivo principal de la N-Roja
    public string ReasonCategory { get; set; } = string.Empty; // "Mental", "Fisica", "Emocional"

    // Descripción detallada del motivo
    public string ReasonDescription { get; set; } = string.Empty;

    // Fecha cuando se registró el motivo
    public DateTime Date { get; set; }

    // Relaciones
    public virtual StudentTaskGrade StudentTaskGrade { get; set; } = null!;
}