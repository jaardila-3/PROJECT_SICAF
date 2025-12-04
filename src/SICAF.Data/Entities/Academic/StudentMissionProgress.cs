using SICAF.Data.Entities.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Entities.Academic;

public class StudentMissionProgress : BaseEntity
{
    // Identificación
    public Guid StudentId { get; set; }
    public Guid InstructorId { get; set; } // Instructor asignado para esta misión (evitamos consultar en cada tarea)
    public Guid MissionId { get; set; }
    public Guid PhaseId { get; set; }
    public Guid CourseId { get; set; }
    public Guid AircraftId { get; set; }

    // Fecha
    public DateTime Date { get; set; }

    // Evaluación general de la misión
    public bool MissionPassed { get; set; } = false;
    public int CriticalFailures { get; set; } = 0; // Cantidad de N-Rojas
    public string? Observations { get; set; }

    // Relaciones
    public virtual User Student { get; set; } = null!;
    public virtual User Instructor { get; set; } = null!;
    public virtual Mission Mission { get; set; } = null!;
    public virtual Phase Phase { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;

    // Relaciones
    public virtual Aircraft Aircraft { get; set; } = null!;
    public virtual ICollection<StudentTaskGrade> StudentTaskGrades { get; set; } = [];
}