using SICAF.Common.DTOs.Academic;
using SICAF.Common.DTOs.Instructor;

namespace SICAF.Common.DTOs.Reports;

/// <summary>
/// DTO para agrupar misiones completadas por fase
/// </summary>
public class PhaseWithMissionsDto
{
    public Guid PhaseId { get; set; }
    public string PhaseName { get; set; } = string.Empty;
    public int PhaseNumber { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? PhaseStatus { get; set; } // "Fase Actual", "Completada", "Suspendido"

    /// <summary>
    /// Misiones evaluables completadas en esta fase
    /// </summary>
    public List<MissionStatusDto> CompletedMissions { get; set; } = [];

    /// <summary>
    /// Misiones no evaluables en esta fase
    /// </summary>
    public List<NonEvaluableMissionDto> NonEvaluableMissions { get; set; } = [];

    /// <summary>
    /// Total de misiones en la fase
    /// </summary>
    public int TotalMissionsInPhase { get; set; }

    /// <summary>
    /// Progreso de completitud de la fase
    /// </summary>
    public int CompletedMissionsCount => CompletedMissions.Count;
}
