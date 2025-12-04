using SICAF.Common.DTOs.Academic;
using SICAF.Data.Entities.Academic;

namespace SICAF.Business.Mappers.Academic;

public static class TaskMapperExtensions
{
    public static StudentPhaseInfoDto MapToDto(this StudentPhaseProgress studentPhaseProgress)
    {
        ArgumentNullException.ThrowIfNull(studentPhaseProgress);

        return new StudentPhaseInfoDto
        {
            Id = studentPhaseProgress.PhaseId,
            Name = studentPhaseProgress.Phase.Name,
            PhaseNumber = studentPhaseProgress.Phase.PhaseNumber,
            WingType = studentPhaseProgress.Phase.WingType,
            StartDate = studentPhaseProgress.StartDate,
            IsCurrentPhase = studentPhaseProgress.IsCurrentPhase,
            Status = studentPhaseProgress.Status
        };
    }

    /// <summary>
    /// Mapea una misión a DTO con información de editabilidad
    /// </summary>
    public static MissionStatusDto MapToDto(
        this Mission mission,
        IReadOnlyList<StudentMissionProgress> studentMissionsProgress,
        Guid currentInstructorId,
        Guid currentPhaseId)
    {
        ArgumentNullException.ThrowIfNull(mission);
        var progress = studentMissionsProgress.First(smp => smp.Mission.MissionNumber == mission.MissionNumber);
        var gradeNameInstructor = progress.Instructor.Grade + ". " + progress.Instructor.Name + " " + progress.Instructor.LastName;

        // Calcular si puede editar basándose en las 4 reglas:
        // 1. Mismo instructor que calificó
        // 2. Dentro de 2 días desde la evaluación
        // 3. Misión de la fase actual
        // 4. Máximo 2 ediciones (esto se valida a nivel de tarea, no de misión)

        var daysSinceEvaluation = (DateTime.Now - progress.Date).TotalDays;
        bool canEdit = progress.InstructorId == currentInstructorId  // 1. Mismo instructor
                    && daysSinceEvaluation <= 2                       // 2. Dentro de 2 días
                    && progress.PhaseId == currentPhaseId;            // 3. Fase actual

        return new MissionStatusDto
        {
            MissionId = mission.Id,
            MissionName = mission.Name,
            MissionNumber = mission.MissionNumber,
            GradeNameInstructor = gradeNameInstructor,
            IsCompleted = true,
            CompletionDate = progress.Date,
            MissionPassed = progress.MissionPassed,
            CriticalFailures = progress.CriticalFailures,
            EvaluatorInstructorId = progress.InstructorId,
            EvaluationDate = progress.Date,
            EditCount = 0,
            CanEdit = canEdit
        };
    }
}