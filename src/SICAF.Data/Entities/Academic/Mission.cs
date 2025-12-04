using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Academic;

public class Mission : BaseEntity
{
    public Guid PhaseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MissionNumber { get; set; }
    public double FlightHours { get; set; }
    public string WingType { get; set; } = string.Empty;

    // Relaciones
    public virtual Phase Phase { get; set; } = null!;
    public virtual ICollection<MissionTask> MissionTasks { get; set; } = [];
    public virtual ICollection<StudentTaskGrade> StudentTaskGrades { get; set; } = [];
    public virtual ICollection<StudentMissionProgress> StudentMissionProgresses { get; set; } = [];
    public virtual ICollection<FlightHourLog> FlightHourLogs { get; set; } = [];
}