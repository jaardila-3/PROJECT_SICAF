using SICAF.Data.Entities.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Entities.Academic;

public class StudentCommitteeRecord : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public Guid PhaseId { get; set; }
    public Guid? LeaderId { get; set; } // Líder que registró (para auditoría)

    public int CommitteeNumber { get; set; } // 1, 2, 3

    // EL COMITÉ GENERA EL ACTA
    public string? ActaNumber { get; set; } // El líder lo ingresa (ej: "ACTA-COM-2025-015")

    // Razón de ir a comité
    public string Reason { get; set; } = string.Empty;

    // DECISIÓN (registrada por el líder después de que el comité decide)
    public string? Decision { get; set; } // "Continuar Curso" o "Terminar Curso"
    public DateTime? DecisionDate { get; set; }
    public string? DecisionObservations { get; set; }

    public bool IsResolved { get; set; } = false;
    public DateTime Date { get; set; } // Fecha en que fue a comité (automático)

    // Relaciones
    public virtual User Student { get; set; } = null!;
    public virtual User? RegisteredByLeader { get; set; }
    public virtual Course Course { get; set; } = null!;
    public virtual Phase Phase { get; set; } = null!;
}