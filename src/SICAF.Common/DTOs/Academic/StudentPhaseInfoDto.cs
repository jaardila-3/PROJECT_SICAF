namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para información de la fase del estudiante
/// </summary>
public class StudentPhaseInfoDto
{
    // Propiedades básicas
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public int PhaseNumber { get; set; }
    public int TotalMissions { get; set; }
    public string WingType { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public bool IsCurrentPhase { get; set; }
    public string Status { get; set; } = string.Empty;

    // Prerrequisitos - Una fase puede requerir completar otra fase antes
    public Guid? PrerequisitePhaseId { get; set; }
}