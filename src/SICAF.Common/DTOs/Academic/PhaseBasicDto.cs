namespace SICAF.Common.DTOs.Academic;

public class PhaseBasicDto
{
    public Guid PhaseId { get; set; }
    public string PhaseName { get; set; } = string.Empty;
    public int PhaseNumber { get; set; }
    public string WingType { get; set; } = string.Empty;
    public int TotalMissions { get; set; }
    public int TotalTasks { get; set; }
}
