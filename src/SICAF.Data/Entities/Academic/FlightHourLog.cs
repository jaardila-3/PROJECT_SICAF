using SICAF.Data.Entities.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Entities.Academic;

public class FlightHourLog : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    public Guid? MissionId { get; set; } // Nullable para misiones no evaluables
    public Guid? NonEvaluableMissionId { get; set; } // Nullable para misiones evaluables
    public Guid AircraftId { get; set; }
    public double MachineFlightHours { get; set; }
    public double ManFlightHours { get; set; }
    public double SilaboFlightHours { get; set; }
    public string Role { get; set; } = string.Empty; // Student, Instructor
    public string? Observations { get; set; }
    public DateTime Date { get; set; }

    // relaciones
    public virtual User User { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
    public virtual Mission? Mission { get; set; } // Nullable para misiones no evaluables
    public virtual NonEvaluableMissionRecord? NonEvaluableMission { get; set; } // Nullable para misiones evaluables
    public virtual Aircraft Aircraft { get; set; } = null!;
}