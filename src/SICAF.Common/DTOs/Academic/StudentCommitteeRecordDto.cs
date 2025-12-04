using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Academic;

public class StudentCommitteeRecordDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public Guid PhaseId { get; set; }
    public Guid? LeaderId { get; set; } // Líder que registró (para auditoría)

    public int CommitteeNumber { get; set; } // 1, 2, 3
    public DateTime Date { get; set; } // Fecha en que fue a comité (automático)

    // EL COMITÉ GENERA EL ACTA
    public string? ActaNumber { get; set; } // El líder lo ingresa (ej: "ACTA-COM-2025-015")

    // Razón de ir a comité
    public string Reason { get; set; } = string.Empty;

    // DECISIÓN (registrada por el líder después de que el comité decide)
    public string? Decision { get; set; } // "Continuar Curso" o "Terminar Curso"
    public DateTime? DecisionDate { get; set; }
    public string? DecisionObservations { get; set; }

    public bool IsResolved { get; set; } = false;
}

public class SaveCommitteeDecisionDto
{
    [Required]
    public Guid CommitteeId { get; set; }

    [Required]
    public Guid LeaderId { get; set; }

    [Required]
    public string ActaNumber { get; set; } = string.Empty;

    [Required]
    public string Decision { get; set; } = string.Empty;

    public string DecisionObservations { get; set; } = string.Empty;
}