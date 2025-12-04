using SICAF.Common.Constants;
using SICAF.Data.Entities.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Entities.Academic;

public class StudentPhaseProgress : BaseEntity
{
    // Referencias
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public Guid PhaseId { get; set; }
    public Guid? LeaderId { get; set; } // Líder de vuelo asignado para esta fase

    // Identificar fase anterior y siguiente
    public Guid? PreviousPhaseId { get; set; }
    public Guid? NextPhaseId { get; set; }

    // Estado y progreso
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentPhase { get; set; } = false; // Indica si es la fase actual del estudiante
    public string Status { get; set; } = UserConstants.StudentStatus.Active;
    public int CompletedMissions { get; set; } = 0;
    public int FailedMissions { get; set; } = 0;
    public bool PhasePassed { get; set; }
    public string? Observations { get; set; }

    // Suspensiones o interrupciones
    public bool IsSuspended { get; set; } = false;
    public DateTime? SuspensionDate { get; set; }
    public string? SuspensionReason { get; set; }

    // Relaciones
    public virtual User Student { get; set; } = null!;
    public virtual User? Leader { get; set; }
    public virtual Phase Phase { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}