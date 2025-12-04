using SICAF.Common.DTOs.Academic;
using SICAF.Common.Models.Results;

namespace SICAF.Business.Interfaces.Student;

public interface IStudentService
{
    /// <summary>
    /// Obtiene el detalle completo de una misión evaluada
    /// </summary>
    /// <param name="studentMissionProgressId">ID del progreso de misión del estudiante</param>
    /// <param name="instructorId">ID del instructor que consulta</param>
    /// <returns>Detalle de la evaluación de la misión</returns>
    Task<Result<MissionEvaluationDetailDto>> GetMissionEvaluationDetailAsync(Guid missionId, Guid studentId, Guid courseId);

    /// <summary>
    /// Obtiene el resumen de los comites de un estudiante en la fase
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="courseId"></param>
    /// <param name="phaseId"></param>
    /// <returns></returns>
    Task<Result<IEnumerable<StudentCommitteeRecordDto>>> GetCommitteeRecordsByPhaseAsync(Guid studentId, Guid courseId, Guid phaseId);

    /// <summary>
    /// Obtiene el detalle de una misión no evaluable
    /// </summary>
    Task<Result<MissionEvaluationDetailDto>> GetNonEvaluableMissionDetailAsync(
        Guid nonEvaluableMissionRecordId,
        Guid studentId,
        Guid courseId);
}