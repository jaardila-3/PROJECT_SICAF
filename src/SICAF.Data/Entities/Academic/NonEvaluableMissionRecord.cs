using SICAF.Data.Entities.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Entities.Academic;

public class NonEvaluableMissionRecord : BaseEntity
{
    // Identificación
    public Guid StudentId { get; set; }
    public Guid InstructorId { get; set; }
    public Guid PhaseId { get; set; }
    public Guid CourseId { get; set; }
    public Guid AircraftId { get; set; }

    // Fecha
    public DateTime Date { get; set; }

    // Número de misión no evaluable (MNE1, MNE2, etc.)
    public int NonEvaluableMissionNumber { get; set; }

    // Observaciones generales
    public string Observations { get; set; } = string.Empty;

    // Horas de vuelo (opcional, default 1.0 máquina / 1.3 hombre)
    public double MachineFlightHours { get; set; }
    public double ManFlightHours { get; set; }

    // Relaciones
    public virtual User Student { get; set; } = null!;
    public virtual User Instructor { get; set; } = null!;
    public virtual Phase Phase { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
    public virtual Aircraft Aircraft { get; set; } = null!;
    public virtual ICollection<NonEvaluableTaskGrade> NonEvaluableTaskGrades { get; set; } = [];
}
