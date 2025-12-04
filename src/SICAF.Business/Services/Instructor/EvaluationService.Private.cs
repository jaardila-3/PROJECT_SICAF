using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SICAF.Business.Mappers.Academic;
using SICAF.Business.Mappers.Identity;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Academic;
using SICAF.Common.DTOs.Instructor;
using SICAF.Data.Entities.Academic;
using SICAF.Data.Entities.Identity;

using static SICAF.Common.Constants.GradeConstants;

namespace SICAF.Business.Services.Instructor;

/// <summary>
/// Servicio para la evaluación de estudiantes, parte privada
/// </summary>
public partial class EvaluationService
{
    #region Métodos Privados    
    private async Task<IEnumerable<TaskToEvaluateDto>> GetMissionTasksForEvaluationAsync(Guid missionId)
    {
        var missionTasks = await _unitOfWork.Repository<MissionTask>()
            .GetListAsync(mt => mt.MissionId == missionId, includeFunc: query => query.Include(mt => mt.Task), orderBy: query => query.OrderBy(mt => mt.Task.Code));

        var taskDtos = new List<TaskToEvaluateDto>();

        foreach (var missionTask in missionTasks)
        {
            taskDtos.Add(new TaskToEvaluateDto
            {
                MissionTaskId = missionTask.Id,
                TaskId = missionTask.TaskId,
                TaskCode = missionTask.Task.Code,
                TaskName = missionTask.Task.Name,
                IsP3InThisMission = missionTask.IsP3Task,
                DisplayOrder = missionTask.DisplayOrder
            });
        }

        return taskDtos;
    }

    private async Task<StudentMissionProgress> CreateUpdateProgressAsync(SaveMissionEvaluationDto evaluationDto)
    {
        var isMissionFailed = evaluationDto.TaskGrades.Any(tg => tg.Grade == GradeTypes.NR);
        var countTasksFailed = evaluationDto.TaskGrades.Count(tg => tg.Grade == GradeTypes.NR);

        // Crear StudentMissionProgress
        var missionProgress = new StudentMissionProgress
        {
            StudentId = evaluationDto.StudentId,
            InstructorId = evaluationDto.InstructorId,
            MissionId = evaluationDto.MissionId,
            PhaseId = evaluationDto.PhaseId,
            CourseId = evaluationDto.CourseId,
            AircraftId = evaluationDto.AircraftId,
            Date = evaluationDto.EvaluationDate,
            Observations = evaluationDto.GeneralObservations,
            MissionPassed = !isMissionFailed,
            CriticalFailures = countTasksFailed
        };

        await _unitOfWork.Repository<StudentMissionProgress>().AddAsync(missionProgress);

        // evaluar comité si FALLÓ la misión actual o si tiene un factor de riesgo emocional
        CommitteeEvaluationDto? committeeEvaluation = null;
        var isFactorEmotional = evaluationDto.TaskGrades.Any(tg => tg.NRedReasons != null && tg.NRedReasons.Any(nr => nr.ReasonCategory == NRedCategories.Emotional));

        if (isMissionFailed || isFactorEmotional)
        {
            // Evaluar si requiere comité o suspensión
            committeeEvaluation = await EvaluateCommitteeRequirementsAsync(evaluationDto, isMissionFailed, isFactorEmotional);
        }

        // Actualizar StudentPhaseProgress
        var phaseProgress = await _unitOfWork.Repository<StudentPhaseProgress>()
            .GetFirstAsync(pp => pp.StudentId == evaluationDto.StudentId
                               && pp.CourseId == evaluationDto.CourseId
                               && pp.IsCurrentPhase);

        if (phaseProgress != null)
        {
            phaseProgress.CompletedMissions++;
            if (isMissionFailed)
                phaseProgress.FailedMissions++;

            // Determinar el estado basado en la evaluación del comité
            if (committeeEvaluation != null)
            {
                if (committeeEvaluation.RequiresSuspension)
                {
                    phaseProgress.Status = UserConstants.StudentStatus.Suspended;
                    phaseProgress.IsSuspended = true;
                    phaseProgress.SuspensionDate = DateTime.Now;
                    phaseProgress.SuspensionReason = $"{committeeEvaluation.Reason}";
                    phaseProgress.EndDate = DateTime.Now;
                    await CreateSuspensionRecordAsync(evaluationDto, committeeEvaluation);
                }
                else if (committeeEvaluation.RequiresCommittee)
                {
                    phaseProgress.Status = UserConstants.StudentStatus.PendingCommittee;
                    await CreateCommitteeRecordAsync(evaluationDto, committeeEvaluation);
                }
                else
                {
                    // Falló pero aún no requiere comité
                    phaseProgress.Status = UserConstants.StudentStatus.Active;
                }
            }
            else
            {
                // Aprobó la misión, se mantiene activo
                phaseProgress.Status = UserConstants.StudentStatus.Active;
            }

            _unitOfWork.Repository<StudentPhaseProgress>().Update(phaseProgress);
        }

        return missionProgress;
    }

    private async Task SaveTaskGradesAsync(Guid missionProgressId, SaveMissionEvaluationDto evaluationDto)
    {
        foreach (var taskGrade in evaluationDto.TaskGrades)
        {
            // Crear nueva calificación
            var newGrade = new StudentTaskGrade
            {
                StudentId = evaluationDto.StudentId,
                InstructorId = evaluationDto.InstructorId,
                MissionId = evaluationDto.MissionId,
                TaskId = taskGrade.TaskId,
                StudentMissionProgressId = missionProgressId,
                Grade = taskGrade.Grade.ToUpper(),
                Date = evaluationDto.EvaluationDate
            };

            await _unitOfWork.Repository<StudentTaskGrade>().AddAsync(newGrade);

            // Guardar motivos de N-Roja si aplica
            if (taskGrade.NRedReasons.Any() && (taskGrade.Grade == GradeTypes.NR || taskGrade.Grade == GradeTypes.N))
            {
                foreach (var nRedReason in taskGrade.NRedReasons)
                {
                    var studentNRedReason = new StudentGradeNRedReason
                    {
                        StudentTaskGradeId = newGrade.Id,
                        ReasonCategory = nRedReason.ReasonCategory,
                        ReasonDescription = nRedReason.ReasonDescription,
                        Date = evaluationDto.EvaluationDate
                    };

                    await _unitOfWork.Repository<StudentGradeNRedReason>().AddAsync(studentNRedReason);
                }
            }
        }
    }

    private async Task CreateUpdateFlightHourLogsAsync(SaveMissionEvaluationDto evaluationDto)
    {
        // Determinar las horas a usar
        double manHours = Math.Round(evaluationDto.MachineFlightHours * 1.3, 1);

        // Crear logs de vuelo
        var flightLogStudent = evaluationDto.MapToEntity(evaluationDto.MachineFlightHours);
        var flightLogInstructor = evaluationDto.MapToEntity(evaluationDto.MachineFlightHours, false);
        await _unitOfWork.Repository<FlightHourLog>().AddRangeAsync([flightLogStudent, flightLogInstructor]);

        // Actualizar horas del estudiante (HORAS HOMBRE)
        var studentProfile = await _unitOfWork.Repository<AviationProfile>()
            .GetFirstAsync(ap => ap.UserId == evaluationDto.StudentId);

        if (studentProfile != null)
        {
            studentProfile.TotalFlightHours += manHours;
            _unitOfWork.Repository<AviationProfile>().Update(studentProfile);
        }

        // Actualizar horas del instructor (HORAS HOMBRE)
        var instructorProfile = await _unitOfWork.Repository<AviationProfile>()
            .GetFirstAsync(ap => ap.UserId == evaluationDto.InstructorId);

        if (instructorProfile != null)
        {
            instructorProfile.TotalFlightHours += manHours;
            _unitOfWork.Repository<AviationProfile>().Update(instructorProfile);
        }

        // Actualizar horas del aeronave (HORAS MÁQUINA)
        var aircraft = await _unitOfWork.Repository<Aircraft>().GetFirstAsync(a => a.Id == evaluationDto.AircraftId);
        if (aircraft != null)
        {
            aircraft.TotalFlightHours += evaluationDto.MachineFlightHours;
            _unitOfWork.Repository<Aircraft>().Update(aircraft);
        }
    }

    private async Task<CommitteeEvaluationDto> EvaluateCommitteeRequirementsAsync(SaveMissionEvaluationDto evaluationDto, bool isMissionFailed, bool isFactorEmotional)
    {
        var result = new CommitteeEvaluationDto
        {
            RequiresCommittee = false,
            RequiresSuspension = false,
            Reason = null
        };

        // Calcular total de misiones fallidas en todo el programa y de las últimas 4 misiones (porque la actual sería la 5ta)
        int windowSize = _options.LastMissionsWindow - 1;
        var (totalFailedInCourse, failedInWindow) = await GetStudentFailureMetricsAsync(evaluationDto.StudentId, evaluationDto.CourseId, evaluationDto.PhaseId, windowSize);

        // agregar mision actual
        if (isMissionFailed)
        {
            totalFailedInCourse++;
            failedInWindow++;
        }

        // REGLA: Verificar el total de misiones fallidas en todo el programa
        if (totalFailedInCourse >= _options.MaxTotalFailedMissionsInCourse)
        {
            result.RequiresSuspension = true;
            result.Reason = UserConstants.CommitteeReasons.MaxTotalFailedMissionsInCourse;
            return result;
        }

        // REGLA: Verificar misiones fallidas en la ventana de las últimas misiones de la fase actual
        // Verificar cuántas veces ha ido a comité en esta fase
        var committeeRecordsInPhase = await _unitOfWork.Repository<StudentCommitteeRecord>()
            .GetListAsync(c => c.StudentId == evaluationDto.StudentId
                                && c.CourseId == evaluationDto.CourseId
                                && c.PhaseId == evaluationDto.PhaseId);

        var committeeCount = committeeRecordsInPhase
            .Select(c => c.CommitteeNumber)
            .DefaultIfEmpty(0)
            .Max();

        if (failedInWindow >= _options.MaxFailedMissionsForCommittee)
        {
            if (committeeCount >= _options.MaxCommitteesPerPhase - 1)
            {
                // Ya fue 2 veces, esta sería la 3ra = suspensión automática
                result.RequiresSuspension = true;
                result.Reason = UserConstants.CommitteeReasons.MaxCommitteesPerPhase;
                result.CommitteeNumber = committeeCount + 1;
            }
            else
            {
                result.RequiresCommittee = true;
                result.Reason = UserConstants.CommitteeReasons.MaxFailedMissionsForCommittee;
                result.CommitteeNumber = committeeCount + 1;
            }
            return result;
        }

        // REGLA: Verificar si tiene factor de riesgo emocional
        if (isFactorEmotional)
        {
            if (committeeCount >= _options.MaxCommitteesPerPhase - 1)
            {
                // Ya fue 2 veces, esta sería la 3ra = suspensión automática
                result.RequiresSuspension = true;
                result.Reason = UserConstants.CommitteeReasons.FactorEmotional + " - " + UserConstants.CommitteeReasons.MaxCommitteesPerPhase;
                result.CommitteeNumber = committeeCount + 1;
            }
            else
            {
                result.RequiresCommittee = true;
                result.Reason = UserConstants.CommitteeReasons.FactorEmotional;
                result.CommitteeNumber = committeeCount + 1;
            }
        }

        return result;
    }

    private async Task CreateCommitteeRecordAsync(SaveMissionEvaluationDto evaluationDto, CommitteeEvaluationDto evaluation)
    {
        var committeeRecord = new StudentCommitteeRecord
        {
            StudentId = evaluationDto.StudentId,
            CourseId = evaluationDto.CourseId,
            PhaseId = evaluationDto.PhaseId,
            LeaderId = evaluationDto.InstructorId,
            CommitteeNumber = evaluation.CommitteeNumber,
            Reason = evaluation.Reason!,
            Date = DateTime.Now,
            IsResolved = false,
            Decision = null // Pendiente de decisión
        };

        await _unitOfWork.Repository<StudentCommitteeRecord>().AddAsync(committeeRecord);

        // Bloquear temporalmente al usuario
        var user = await _unitOfWork.Repository<User>()
            .GetFirstAsync(u => u.Id == evaluationDto.StudentId);

        if (user != null)
        {
            user.LockoutEnd = DateTime.Now.AddYears(50); // Bloqueo temporal
            user.LockoutReason = evaluation.Reason;
            _unitOfWork.Repository<User>().Update(user);
        }

        _logger.LogWarning("Estudiante {StudentId} enviado a comité #{CommitteeNumber} por: {Reason}",
            evaluationDto.StudentId, evaluation.CommitteeNumber, evaluation.Reason);
    }

    private async Task CreateSuspensionRecordAsync(SaveMissionEvaluationDto evaluationDto, CommitteeEvaluationDto evaluation)
    {
        var suspensionRecord = new StudentCommitteeRecord
        {
            StudentId = evaluationDto.StudentId,
            CourseId = evaluationDto.CourseId,
            PhaseId = evaluationDto.PhaseId,
            LeaderId = evaluationDto.InstructorId,
            CommitteeNumber = evaluation.CommitteeNumber,
            Reason = evaluation.Reason!,
            Decision = UserConstants.CommitteeDecisions.Suspendecourse,
            Date = DateTime.Now,
            IsResolved = true,
            DecisionDate = DateTime.Now,
            DecisionObservations = "Suspensión automática por sistema"
        };

        await _unitOfWork.Repository<StudentCommitteeRecord>().AddAsync(suspensionRecord);

        // Bloquear permanentemente al usuario
        var user = await _unitOfWork.Repository<User>().GetFirstAsync(u => u.Id == evaluationDto.StudentId);

        if (user != null)
        {
            user.LockoutEnd = DateTime.Now.AddYears(50);
            user.LockoutReason = evaluation.Reason;
            _unitOfWork.Repository<User>().Update(user);
        }

        _logger.LogWarning("Estudiante {StudentId} SUSPENDIDO definitivamente por: {Reason}", evaluationDto.StudentId, evaluation.Reason);
    }

    private async Task<(int totalFailedInCourse, int failedInWindow)> GetStudentFailureMetricsAsync(Guid studentId, Guid courseId, Guid phaseId, int windowSize)
    {
        // Obtener el progreso de todas las fases del estudiante en el programa
        var allPhasesProgress = await _unitOfWork.Repository<StudentPhaseProgress>().GetListAsync(pp => pp.CourseId == courseId && pp.StudentId == studentId);

        // Obtener progreso de misiones de la fase actual
        var missionProgressesInPhase = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetListAsync(
                sm => sm.CourseId == courseId
                    && sm.PhaseId == phaseId
                    && sm.StudentId == studentId,
                q => q.OrderByDescending(sm => sm.Mission.MissionNumber),
                query => query.Include(sm => sm.Mission)
            );

        // Cálculo 1: TotalFailedInCourse
        int totalFailedInCourse = allPhasesProgress.Sum(pp => pp.FailedMissions);

        // Cálculo 2: FailedInWindow
        var missionsInWindow = missionProgressesInPhase.Take(windowSize).ToList();
        var failedInWindow = missionsInWindow.Count(mp => !mp.MissionPassed);

        // retorno
        return (totalFailedInCourse, failedInWindow);
    }

    private async Task<int> GetNextNonEvaluableMissionNumberAsync(Guid studentId, Guid phaseId, Guid courseId)
    {
        var records = await _unitOfWork.Repository<NonEvaluableMissionRecord>()
            .GetListAsync(
                predicate: m => m.StudentId == studentId && m.PhaseId == phaseId && m.CourseId == courseId,
                orderBy: q => q.OrderByDescending(m => m.NonEvaluableMissionNumber)
            );

        return records.Any() ? records[0].NonEvaluableMissionNumber + 1 : 1;
    }

    private async Task SaveNonEvaluableTaskGradesAsync(SaveMissionEvaluationDto dto)
    {
        foreach (var taskGrade in dto.TaskGrades)
        {
            var grade = new NonEvaluableTaskGrade
            {
                StudentId = dto.StudentId,
                InstructorId = dto.InstructorId,
                TaskId = taskGrade.TaskId,
                NonEvaluableMissionRecordId = dto.MissionId,
                Grade = taskGrade.Grade.ToUpper(),
                Date = dto.EvaluationDate
            };

            await _unitOfWork.Repository<NonEvaluableTaskGrade>().AddAsync(grade);

            // Guardar razones si tiene nota N (NO puede tener NR)
            if (taskGrade.NRedReasons.Any() && taskGrade.Grade == GradeTypes.N)
            {
                foreach (var reason in taskGrade.NRedReasons)
                {
                    var gradeReason = new NonEvaluableGradeReason
                    {
                        NonEvaluableTaskGradeId = grade.Id,
                        ReasonCategory = reason.ReasonCategory,
                        ReasonDescription = reason.ReasonDescription,
                        Date = dto.EvaluationDate
                    };

                    await _unitOfWork.Repository<NonEvaluableGradeReason>().AddAsync(gradeReason);
                }
            }
        }
    }

    /// <summary>
    /// Obtiene la última fecha de evaluación del estudiante en todo el programa, considerando
    /// tanto misiones regulares como no evaluables
    /// </summary>
    private async Task<DateTime?> GetLastEvaluationDateAsync(Guid studentId, Guid courseId)
    {
        // Obtener última fecha de misiones regulares
        var lastRegularMission = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetListAsync(
                predicate: smp => smp.StudentId == studentId && smp.CourseId == courseId,
                orderBy: q => q.OrderByDescending(smp => smp.Date)
            );

        var lastRegularDate = lastRegularMission.FirstOrDefault()?.Date;

        // Obtener última fecha de misiones no evaluables
        var lastNonEvaluableMission = await _unitOfWork.Repository<NonEvaluableMissionRecord>()
            .GetListAsync(
                predicate: nem => nem.StudentId == studentId && nem.CourseId == courseId,
                orderBy: q => q.OrderByDescending(nem => nem.Date)
            );

        var lastNonEvaluableDate = lastNonEvaluableMission.FirstOrDefault()?.Date;

        // Retornar la fecha más reciente entre ambas
        if (lastRegularDate.HasValue && lastNonEvaluableDate.HasValue)
        {
            return lastRegularDate > lastNonEvaluableDate ? lastRegularDate : lastNonEvaluableDate;
        }

        return lastRegularDate ?? lastNonEvaluableDate;
    }

    #endregion
}