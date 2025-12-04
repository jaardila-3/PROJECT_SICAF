using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Academic;

public class NonEvaluableGradeReason : BaseEntity
{
    public Guid NonEvaluableTaskGradeId { get; set; }

    // Motivo principal de la calificación N (No Satisfactorio)
    public string ReasonCategory { get; set; } = string.Empty; // "Mental", "Fisica", "Emocional"

    // Descripción detallada del motivo
    public string ReasonDescription { get; set; } = string.Empty;

    // Fecha cuando se registró el motivo
    public DateTime Date { get; set; }

    // Relaciones
    public virtual NonEvaluableTaskGrade NonEvaluableTaskGrade { get; set; } = null!;
}
