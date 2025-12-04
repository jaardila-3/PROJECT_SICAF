namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para la misión que se va a evaluar
/// </summary>
public class MissionToEvaluateDto
{
    public Guid MissionId { get; set; }
    public Guid PhaseId { get; set; }
    public string MissionName { get; set; } = string.Empty;
    public int MissionNumber { get; set; }
    public double FlightHours { get; set; }
    public string WingType { get; set; } = string.Empty;

    // Aeronaves de la misión
    public IEnumerable<AircraftDto> Aircrafts { get; set; } = null!;

    // Tareas regulares de la misión
    public IEnumerable<TaskToEvaluateDto> MissionTasks { get; set; } = [];
}