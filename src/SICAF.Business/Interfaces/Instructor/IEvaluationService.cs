using SICAF.Common.DTOs.Academic;
using SICAF.Common.DTOs.Instructor;
using SICAF.Common.Models.Results;

namespace SICAF.Business.Interfaces.Instructor;

/// <summary>
/// Interfaz para el servicio de evaluación de estudiantes
/// </summary>
public interface IEvaluationService
{
    /// <summary>
    /// Obtiene la información completa para mostrar al instructor la evaluación de un estudiante
    /// </summary>
    /// <param name="studentId">ID del estudiante</param>
    /// <param name="courseId">ID del curso</param>
    /// <param name="instructorId">ID del instructor que evalúa</param>
    /// <returns>Información completa para la vista de evaluación</returns>
    Task<Result<EvaluationOverviewDto>> GetStudentEvaluationOverviewAsync(Guid studentId, Guid courseId, Guid instructorId, string? viewType = null);

    /// <summary>
    /// Guarda las calificaciones de las tareas de la misión pendiente
    /// </summary>
    /// <param name="evaluationDto">Datos de la evaluación</param>
    /// <returns>Resultado de la operación</returns>
    Task<Result<bool>> SaveMissionEvaluationAsync(SaveMissionEvaluationDto evaluationDto);

    /// <summary>
    /// Obtiene y valida el tipo de participación de un instructor respecto a un estudiante en un curso.
    /// </summary>
    /// <param name="instructorId">ID del instructor</param>
    /// <param name="studentId">ID del estudiante</param>
    /// <param name="courseId">ID del curso</param>
    /// <returns>True si puede evaluar, false en caso contrario</returns>
    Task<Result<string>> GetInstructorParticipationTypeAsync(Guid instructorId, Guid studentId, Guid courseId);

    /// <summary>
    /// Promueve un estudiante a la siguiente fase
    /// </summary>
    Task<Result<string>> PromoteStudentToNextPhaseAsync(PromoteStudentDto request);

    /// <summary>
    /// Guarda la decisión del comité para un estudiante
    /// </summary>
    Task<Result<string>> SaveCommitteeDecisionAsync(SaveCommitteeDecisionDto request);

    /// <summary>
    /// Finaliza y aprueba el curso de un estudiante
    /// </summary>
    Task<Result<string>> FinalizeAndApproveCourseAsync(PromoteStudentDto request);

    /// <summary>
    /// Obtener datos para editar calificaciones
    /// </summary>
    /// <param name="missionId"></param>
    /// <param name="studentId"></param>
    /// <param name="courseId"></param>
    /// <param name="instructorId"></param>
    /// <returns></returns>
    Task<Result<MissionGradesEditViewDto>> GetMissionGradesForEditAsync(Guid missionId, Guid studentId, Guid courseId, Guid instructorId);

    /// <summary>
    /// Guardar ediciones de calificaciones
    /// </summary>
    /// <param name="editDto"></param>
    /// <returns></returns>
    Task<Result<bool>> SaveMissionGradesEditAsync(EditMissionGradesDto editDto);

    /// <summary>
    /// Guarda las calificaciones de las tareas de la misión no evaluable
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Result<bool>> SaveNonEvaluableMissionAsync(SaveMissionEvaluationDto dto);

    /// <summary>
    /// Obtiene los datos de una misión no evaluable para editar sus calificaciones
    /// </summary>
    Task<Result<MissionGradesEditViewDto>> GetNonEvaluableMissionGradesForEditAsync(
        Guid nonEvaluableMissionRecordId,
        Guid studentId,
        Guid courseId,
        Guid instructorId);

    /// <summary>
    /// Guarda las ediciones de calificaciones de una misión no evaluable
    /// </summary>
    Task<Result<bool>> SaveNonEvaluableMissionGradesEditAsync(EditMissionGradesDto editDto);
}