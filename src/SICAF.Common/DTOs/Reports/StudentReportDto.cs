namespace SICAF.Common.DTOs.Reports;

public class StudentReportDto
{
    public int Number { get; set; } // Numeración
    public string GradeAndName { get; set; } = string.Empty; // "TE. RODRIGUEZ JUAN"
    public string Identification { get; set; } = string.Empty;
    public string PID { get; set; } = string.Empty;
    public string CurrentPhase { get; set; } = string.Empty;
    public string? PhaseStatus { get; set; } // "Fase Actual", "Completada", "Suspendido", "Graduado"
    public int CompletedMissions { get; set; }
    public int NonEvaluableMissionRecords { get; set; }
    public double ProgressPercentage { get; set; }
    public double Average { get; set; }
    public int SatisfactoryMissions { get; set; }
    public int UnsatisfactoryMissions { get; set; }
    public double TotalFlightHours { get; set; }
}