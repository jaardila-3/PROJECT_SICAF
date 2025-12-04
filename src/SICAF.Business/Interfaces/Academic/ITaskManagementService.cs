using SICAF.Common.DTOs.Academic;
using SICAF.Common.Models.Results;

namespace SICAF.Business.Interfaces.Academic;

public interface ITaskManagementService
{
    /// <summary>
    /// Obtiene todas las fases con información básica para el selector
    /// </summary>
    Task<Result<List<PhaseBasicDto>>> GetPhasesAsync(string? wingType = null);

    /// <summary>
    /// Obtiene todas las tareas de una fase con sus relaciones a misiones
    /// </summary>
    Task<Result<PhaseTasksDto>> GetPhaseTasksAsync(Guid phaseId);

    /// <summary>
    /// Actualiza nombres de tareas, DisplayOrder y configuración de P3
    /// </summary>
    Task<Result<bool>> UpdatePhaseTasksAsync(UpdateTasksDto dto);
}
