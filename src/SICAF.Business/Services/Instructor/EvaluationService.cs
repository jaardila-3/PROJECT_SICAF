using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SICAF.Business.Interfaces.Instructor;
using SICAF.Business.Interfaces.Student;
using SICAF.Business.Mappers.Academic;
using SICAF.Business.Mappers.Identity;
using SICAF.Common.Configuration.Options;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Academic;
using SICAF.Common.DTOs.Instructor;
using SICAF.Common.Models.Results;
using SICAF.Data.Entities.Academic;
using SICAF.Data.Entities.Identity;
using SICAF.Data.Interfaces.Repositories;

using static SICAF.Common.Constants.GradeConstants;

namespace SICAF.Business.Services.Instructor;

/// <summary>
/// Servicio para la evaluación de estudiantes
/// </summary>
public partial class EvaluationService(
    IUnitOfWork unitOfWork,
    IStudentService studentService,
    IOptions<CommitteeOptions> options,
    ILogger<EvaluationService> logger
) : IEvaluationService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStudentService _studentService = studentService;
    private readonly CommitteeOptions _options = options.Value;
    private readonly ILogger<EvaluationService> _logger = logger;
    private const int MaxEditCount = 2;

    public async Task<Result<string>> GetInstructorParticipationTypeAsync(Guid instructorId, Guid studentId, Guid courseId)
    {
        // Verificar que el instructor esté asignado al programa
        var instructorAssignment = await _unitOfWork.Repository<UserCourse>().GetFirstAsync(uc => uc.UserId == instructorId && uc.CourseId == courseId);

        if (instructorAssignment == null)
            return Result<string>.Failure("INSTRUCTOR_NOT_ASSIGNED", "El instructor no esta asignado al Programa");

        // Verificar que el estudiante esté inscrito en el programa
        var studentEnrollment = await _unitOfWork.Repository<UserCourse>()
            .GetFirstAsync(uc => uc.UserId == studentId && uc.CourseId == courseId && uc.ParticipationType == SystemRoles.STUDENT);

        if (studentEnrollment == null)
            return Result<string>.Failure(SystemErrors.EnrollmentError.StudentNotEnrolled);

        // Verificar compatibilidad de tipo de ala
        if (instructorAssignment.WingType != studentEnrollment.WingType)
            return Result<string>.Failure("COMPATIBILITY_ERROR", "El instructor y el estudiante no tienen la misma ala");

        return Result<string>.Success(instructorAssignment.ParticipationType);
    }

    public async Task<Result<EvaluationOverviewDto>> GetStudentEvaluationOverviewAsync(Guid studentId, Guid courseId, Guid instructorId, string? viewType = null)
    {
        // Obtener información del estudiante
        var student = await _unitOfWork.Repository<User>().GetFirstAsync(u => u.Id == studentId);

        if (student is null)
            return Result<EvaluationOverviewDto>.Failure(SystemErrors.UserError.NotFound);

        var dataStudent = student.MapToDto();

        // Obtener la fase actual del estudiante
        var currentPhaseProgress = await _unitOfWork.Repository<StudentPhaseProgress>()
            .GetFirstAsync(sp => sp.StudentId == studentId && sp.CourseId == courseId && sp.IsCurrentPhase, includeFunc: query => query.Include(sp => sp.Phase));

        if (currentPhaseProgress is null)
        {
            var phaseProgress = await _unitOfWork.Repository<StudentPhaseProgress>()
            .GetListAsync(sp => sp.StudentId == studentId && sp.CourseId == courseId,
                orderBy: query => query.OrderBy(sp => sp.Phase.PhaseNumber),
                includeFunc: query => query.Include(sp => sp.Phase));

            currentPhaseProgress = phaseProgress.LastOrDefault();

            if (currentPhaseProgress is null)
                return Result<EvaluationOverviewDto>.Failure("NO_PHASE", "El estudiante no tiene una fase");
        }

        // Obtener registros de comités de la fase actual
        var committeeRecordsResult = await _studentService.GetCommitteeRecordsByPhaseAsync(studentId, courseId, currentPhaseProgress.PhaseId);
        var committeeRecords = committeeRecordsResult.IsSuccess ? committeeRecordsResult.Value : [];

        // obtener el numero de fases para el programa y el ala y determinar si es la ultima fase
        var phasesCount = await _unitOfWork.Repository<Phase>().CountAsync(p => p.WingType == currentPhaseProgress.Phase.WingType);
        var isLastPhase = currentPhaseProgress.Phase.PhaseNumber == phasesCount;

        // Obtener información del programa
        var course = await _unitOfWork.Repository<Course>().GetFirstAsync(c => c.Id == courseId);

        // Obtener todas las misiones de la fase actual
        var missionsCurrentPhase = await _unitOfWork.Repository<Mission>()
            .GetListAsync(m => m.PhaseId == currentPhaseProgress.PhaseId, orderBy: query => query.OrderBy(m => m.MissionNumber));

        // Obtener misiones completadas en la fase actual
        var completedMissionsProgress = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetListAsync(smp => smp.StudentId == studentId && smp.CourseId == courseId && smp.Mission.PhaseId == currentPhaseProgress.PhaseId,
                includeFunc: query => query.Include(smp => smp.Mission).Include(smp => smp.Instructor));

        // Obtener número de las misiones completadas en la fase actual
        var completedMissionNumbers = completedMissionsProgress.Select(smp => smp.Mission.MissionNumber).ToHashSet();

        // Crear DTOs de misiones completadas con información de editabilidad
        var completedMissions = missionsCurrentPhase
            .Where(m => completedMissionNumbers.Contains(m.MissionNumber))
            .Select(m => m.MapToDto(completedMissionsProgress, instructorId, currentPhaseProgress.PhaseId))
            .ToList();

        // Obtener misiones no evaluables guardadas de la fase actual
        var nonEvaluableMissions = await _unitOfWork.Repository<NonEvaluableMissionRecord>()
            .GetListAsync(
                nem => nem.StudentId == studentId
                    && nem.CourseId == courseId
                    && nem.PhaseId == currentPhaseProgress.PhaseId,
                orderBy: q => q.OrderBy(nem => nem.Date)
            );

        var nonEvaluableMissionsDto = nonEvaluableMissions.Select(nem => new NonEvaluableMissionDto
        {
            MissionId = nem.Id,
            CourseId = nem.CourseId,
            MissionNumber = nem.NonEvaluableMissionNumber,
            Date = nem.Date,
            ManFlightHours = nem.ManFlightHours,
            Observations = nem.Observations ?? string.Empty,
            InstructorId = nem.InstructorId,
            CanEdit = (DateTime.Now - nem.Date).TotalDays <= 2 && nem.InstructorId == instructorId
        }).ToList();

        // Verificar si hay misiones pendientes
        Mission? nextMission = null;

        if (viewType == "nonEvaluable")
        {
            nextMission = missionsCurrentPhase.FirstOrDefault(m => m.MissionNumber == missionsCurrentPhase.Min(m => m.MissionNumber));
        }
        else if (completedMissionNumbers.Count < currentPhaseProgress.Phase.TotalMissions)
        {
            // Buscar la siguiente misión a evaluar en fase actual
            var nextMissionNumber = completedMissionNumbers.Count != 0
                ? completedMissionNumbers.Max() + 1
                : missionsCurrentPhase.Min(m => m.MissionNumber);

            nextMission = missionsCurrentPhase.FirstOrDefault(m => m.MissionNumber == nextMissionNumber);
        }

        // Verificar si está pendiente de comité
        string currentStatus = currentPhaseProgress.Status;
        StudentCommitteeRecordDto? committeeRecordDto = null;
        if (currentStatus == UserConstants.StudentStatus.PendingCommittee)
        {
            var pendingCommittee = await _unitOfWork.Repository<StudentCommitteeRecord>()
                .GetFirstAsync(cr => cr.StudentId == studentId && cr.PhaseId == currentPhaseProgress.PhaseId && cr.CourseId == courseId && !cr.IsResolved);

            if (pendingCommittee != null)
            {
                committeeRecordDto = pendingCommittee.MapToDto();
            }
        }

        MissionToEvaluateDto? nextMissionDto = null;
        if (nextMission != null && currentStatus == UserConstants.StudentStatus.Active)
        {
            // Obtener tareas de la misión
            var missionTasks = await GetMissionTasksForEvaluationAsync(nextMission.Id);

            // obtener aerronaves asociadas a la misión
            var planes = await _unitOfWork.Repository<Aircraft>().GetListAsync(p => p.WingType == nextMission.WingType);

            nextMissionDto = new MissionToEvaluateDto
            {
                MissionId = nextMission.Id,
                PhaseId = nextMission.PhaseId,
                MissionName = nextMission.Name,
                MissionNumber = nextMission.MissionNumber,
                FlightHours = nextMission.FlightHours,
                WingType = nextMission.WingType,
                MissionTasks = missionTasks,
                Aircrafts = planes.Select(p => p.MapToDto())
            };
        }

        // Calcular total de misiones fallidas en todo el programa y de las últimas 4 misiones (porque la actual sería la 5ta)
        int windowSize = _options.LastMissionsWindow - 1;
        var (totalFailedInCourse, failedInWindow) = await GetStudentFailureMetricsAsync(studentId, courseId, currentPhaseProgress.PhaseId, windowSize);

        // objeto de retorno
        var overview = new EvaluationOverviewDto
        {
            StudentId = studentId,
            StudentFullName = $"{dataStudent.Grade}. {dataStudent.Name} {dataStudent.LastName}",
            StudentIdentification = dataStudent.IdentificationNumber,
            PhotoBase64 = dataStudent.PhotoBase64,
            InstructorId = instructorId,
            CourseId = courseId,
            CourseName = course?.CourseName ?? string.Empty,
            CurrentPhase = currentPhaseProgress.MapToDto(),
            IsLastPhase = isLastPhase,
            CompletedMissions = completedMissions,
            NextMissionToEvaluate = nextMissionDto,
            CompletedMissionsCount = completedMissions.Count,
            TotalMissionsInPhase = missionsCurrentPhase.Count,
            Status = currentStatus,
            PendingCommittee = committeeRecordDto,
            CommitteeRecordsInCurrentPhase = committeeRecords,
            NonEvaluableMissions = nonEvaluableMissionsDto,
            TotalFailedMissionsInCourse = totalFailedInCourse,
            FailedMissionsInWindow = failedInWindow
        };

        return Result<EvaluationOverviewDto>.Success(overview);
    }

    public async Task<Result<bool>> SaveMissionEvaluationAsync(SaveMissionEvaluationDto evaluationDto)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            // verificar si ya existe una evaluación con la misma fecha de evaluación
            bool exists = await _unitOfWork.Repository<StudentMissionProgress>()
                .AnyAsync(
                    smp => smp.CourseId == evaluationDto.CourseId
                    && smp.StudentId == evaluationDto.StudentId
                    && smp.Date.Date == evaluationDto.EvaluationDate.Date);

            if (exists)
                return Result<bool>.Failure(SystemErrors.EvaluationError.EvaluationExists);

            // Validar que la fecha de evaluación no sea futura
            if (evaluationDto.EvaluationDate.Date > DateTime.Now.Date)
            {
                return Result<bool>.Failure(SystemErrors.EvaluationError.FutureEvaluationDate);
            }

            // Validar que la fecha de evaluación no tenga más de 30 días de atraso
            var daysDifference = (DateTime.Now.Date - evaluationDto.EvaluationDate.Date).TotalDays;
            if (daysDifference > 30)
            {
                return Result<bool>.Failure(SystemErrors.EvaluationError.EvaluationDateTooOld);
            }

            // Validar que la fecha de evaluación sea posterior a la última misión evaluada
            var lastEvaluationDate = await GetLastEvaluationDateAsync(evaluationDto.StudentId, evaluationDto.CourseId);
            if (lastEvaluationDate.HasValue && evaluationDto.EvaluationDate.Date <= lastEvaluationDate.Value.Date)
            {
                return Result<bool>.Failure(SystemErrors.EvaluationError.InvalidChronologicalOrder(lastEvaluationDate.Value));
            }

            // Crear StudentMissionProgress primero
            var missionProgress = await CreateUpdateProgressAsync(evaluationDto);

            // Guardar calificaciones de tareas de la misión
            await SaveTaskGradesAsync(missionProgress.Id, evaluationDto);

            // Registrar horas de vuelo y actualizar horas totales
            await CreateUpdateFlightHourLogsAsync(evaluationDto);

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Evaluación guardada exitosamente para estudiante {StudentId}, misión {MissionId}",
                evaluationDto.StudentId, evaluationDto.MissionId);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al guardar evaluación para estudiante {StudentId}", evaluationDto.StudentId);
            return Result<bool>.Failure(SystemErrors.EvaluationError.EvaluationSaveError);
        }
    }

    public async Task<Result<string>> PromoteStudentToNextPhaseAsync(PromoteStudentDto request)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Obtener el progreso de la fase actual
            var currentPhaseProgress = await _unitOfWork.Repository<StudentPhaseProgress>()
                .GetFirstAsync(
                    predicate: spp => spp.StudentId == request.StudentId
                        && spp.CourseId == request.CourseId
                        && spp.PhaseId == request.PhaseId
                        && spp.IsCurrentPhase,
                    includeFunc: q => q.Include(spp => spp.Phase)
                );

            if (currentPhaseProgress == null)
                return Result<string>.Failure(SystemErrors.PhaseError.PhaseNotFound);

            // Validar que el estudiante haya completado todas las misiones
            if (currentPhaseProgress.CompletedMissions < currentPhaseProgress.Phase.TotalMissions)
                return Result<string>.Failure(
                    "INCOMPLETE_MISSIONS",
                    $"El estudiante debe completar todas las misiones. Completadas: {currentPhaseProgress.CompletedMissions}/{currentPhaseProgress.Phase.TotalMissions}"
                );

            // Verificar que exista una siguiente fase
            if (!currentPhaseProgress.NextPhaseId.HasValue)
                return Result<string>.Failure("NO_NEXT_PHASE", "El estudiante ya se encuentra en la última fase del programa");

            // Obtener información de la siguiente fase
            var nextPhase = await _unitOfWork.Repository<Phase>().GetFirstAsync(p => p.Id == currentPhaseProgress.NextPhaseId.Value);

            if (nextPhase == null)
                return Result<string>.Failure("NEXT_PHASE_NOT_FOUND", "No se encontró la siguiente fase");

            // Obtener la fase siguiente a la próxima (para el campo NextPhaseId del nuevo registro)
            var phaseAfterNext = await _unitOfWork.Repository<Phase>()
                .GetFirstAsync(p => p.WingType == nextPhase.WingType && p.PhaseNumber == nextPhase.PhaseNumber + 1 && p.PrerequisitePhaseId == nextPhase.Id);

            // Cerrar la fase actual
            currentPhaseProgress.IsCurrentPhase = false;
            currentPhaseProgress.EndDate = DateTime.Now;
            currentPhaseProgress.Status = UserConstants.StudentStatus.PhaseCompleted;
            currentPhaseProgress.PhasePassed = true;
            currentPhaseProgress.LeaderId = request.LeaderId;
            _unitOfWork.Repository<StudentPhaseProgress>().Update(currentPhaseProgress);

            // Crear registro para la nueva fase
            var newPhaseProgress = new StudentPhaseProgress
            {
                StudentId = request.StudentId,
                CourseId = request.CourseId,
                PhaseId = nextPhase.Id,
                LeaderId = request.LeaderId,
                PreviousPhaseId = request.PhaseId,
                NextPhaseId = phaseAfterNext?.Id, // Puede ser null si es la última fase
                StartDate = DateTime.Now,
                IsCurrentPhase = true,
                Status = UserConstants.StudentStatus.Active,
                CompletedMissions = 0,
                FailedMissions = 0,
                PhasePassed = false
            };

            await _unitOfWork.Repository<StudentPhaseProgress>().AddAsync(newPhaseProgress);

            // Guardar cambios
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation(
                "Estudiante {StudentId} promovido de fase {CurrentPhase} a fase {NextPhase}", request.StudentId, currentPhaseProgress.Phase.Name, nextPhase.Name
            );

            return Result<string>.Success($"Estudiante promovido exitosamente a {nextPhase.Name}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al promover estudiante a siguiente fase");
            return Result<string>.Failure("PROMOTION_ERROR", "Error al promover al estudiante: " + ex.Message);
        }
    }

    public async Task<Result<string>> SaveCommitteeDecisionAsync(SaveCommitteeDecisionDto request)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Validar que existe el registro de comité
            var committeeRecord = await _unitOfWork.Repository<StudentCommitteeRecord>()
                .GetFirstAsync(c => c.Id == request.CommitteeId && !c.IsResolved);

            if (committeeRecord == null)
                return Result<string>.Failure("COMMITTEE_NOT_FOUND", "No se encontró el registro de comité o ya fue resuelto");

            // Actualizar el registro del comité con la decisión
            committeeRecord.ActaNumber = request.ActaNumber;
            committeeRecord.Decision = request.Decision;
            committeeRecord.DecisionDate = DateTime.Now;
            committeeRecord.DecisionObservations = request.DecisionObservations;
            committeeRecord.IsResolved = true;
            committeeRecord.LeaderId = request.LeaderId;

            _unitOfWork.Repository<StudentCommitteeRecord>().Update(committeeRecord);

            // Obtener el progreso de la fase actual
            var phaseProgress = await _unitOfWork.Repository<StudentPhaseProgress>()
                .GetFirstAsync(pp => pp.StudentId == committeeRecord.StudentId
                                   && pp.CourseId == committeeRecord.CourseId
                                   && pp.PhaseId == committeeRecord.PhaseId
                                   && pp.IsCurrentPhase);

            if (phaseProgress == null)
                return Result<string>.Failure(SystemErrors.PhaseError.PhaseNotFound);

            // Obtener el usuario
            var user = await _unitOfWork.Repository<User>()
                .GetFirstAsync(u => u.Id == committeeRecord.StudentId);

            if (user == null)
                return Result<string>.Failure(SystemErrors.UserError.NotFound);

            // Procesar según la decisión del comité
            if (request.Decision == UserConstants.CommitteeDecisions.ContinueCourse)
            {
                // Desbloquear al estudiante
                user.LockoutEnd = null;
                user.LockoutReason = null;
                _unitOfWork.Repository<User>().Update(user);

                // actualizar el estatus
                phaseProgress.Status = UserConstants.StudentStatus.Active;

                _unitOfWork.Repository<StudentPhaseProgress>().Update(phaseProgress);

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result<string>.Success("Decisión registrada exitosamente. El estudiante puede continuar con el programa.");
            }
            else if (request.Decision == UserConstants.CommitteeDecisions.Suspendecourse)
            {
                // Mantener al estudiante bloqueado
                user.LockoutEnd = DateTime.Now.AddYears(50);
                user.LockoutReason = $"Suspendido por decisión de comité mediante acta #{committeeRecord.ActaNumber}";
                _unitOfWork.Repository<User>().Update(user);

                // Cambiar estado a suspendido
                phaseProgress.Status = UserConstants.StudentStatus.Suspended;
                phaseProgress.IsCurrentPhase = false;
                phaseProgress.IsSuspended = true;
                phaseProgress.SuspensionDate = DateTime.Now;
                phaseProgress.SuspensionReason = $"SUSPENDIDO POR DECISION DE COMITE ACTA # {committeeRecord.ActaNumber}";
                phaseProgress.EndDate = DateTime.Now;

                _unitOfWork.Repository<StudentPhaseProgress>().Update(phaseProgress);

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result<string>.Success("Decisión registrada exitosamente. El estudiante ha sido suspendido del programa.");
            }
            else
            {
                return Result<string>.Failure("INVALID_DECISION", "La decisión del comité no es válida");
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al guardar decisión de comité para CommitteeId: {CommitteeId}", request.CommitteeId);
            return Result<string>.Failure("SAVE_ERROR", "Error al guardar la decisión del comité: " + ex.Message);
        }
    }

    public async Task<Result<string>> FinalizeAndApproveCourseAsync(PromoteStudentDto request)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Obtener el progreso de la fase actual
            var currentPhaseProgress = await _unitOfWork.Repository<StudentPhaseProgress>()
                .GetFirstAsync(
                    predicate: spp => spp.StudentId == request.StudentId
                        && spp.CourseId == request.CourseId
                        && spp.PhaseId == request.PhaseId
                        && spp.IsCurrentPhase,
                    includeFunc: q => q.Include(spp => spp.Phase)
                );

            if (currentPhaseProgress == null)
                return Result<string>.Failure(SystemErrors.PhaseError.PhaseNotFound);

            // Validar que el estudiante haya completado todas las misiones
            if (currentPhaseProgress.CompletedMissions < currentPhaseProgress.Phase.TotalMissions)
                return Result<string>.Failure(
                    "INCOMPLETE_MISSIONS",
                    $"El estudiante debe completar todas las misiones. Completadas: {currentPhaseProgress.CompletedMissions}/{currentPhaseProgress.Phase.TotalMissions}"
                );

            // Validar que sea la última fase (no debe tener NextPhaseId)
            if (currentPhaseProgress.NextPhaseId.HasValue)
                return Result<string>.Failure(
                    "NOT_LAST_PHASE",
                    "El estudiante no se encuentra en la última fase del programa. Use 'Promover a siguiente fase' en su lugar."
                );

            // Verificar que sea realmente la última fase contando las fases del wing type
            var totalPhases = await _unitOfWork.Repository<Phase>().CountAsync(p => p.WingType == currentPhaseProgress.Phase.WingType);

            if (currentPhaseProgress.Phase.PhaseNumber < totalPhases)
                return Result<string>.Failure("NOT_LAST_PHASE", "El estudiante no se encuentra en la última fase del programa");

            // Cerrar la fase actual como completada y aprobada
            currentPhaseProgress.IsCurrentPhase = false;
            currentPhaseProgress.EndDate = DateTime.Now;
            currentPhaseProgress.Status = UserConstants.StudentStatus.CourseCompleted;
            currentPhaseProgress.PhasePassed = true;
            currentPhaseProgress.LeaderId = request.LeaderId;
            currentPhaseProgress.Observations = $"Programa completado exitosamente el {DateTime.Now:dd/MM/yyyy} y aprobado por el líder {request.LeaderId}";

            _unitOfWork.Repository<StudentPhaseProgress>().Update(currentPhaseProgress);

            // Guardar cambios
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result<string>.Success($"¡Felicitaciones! El estudiante ha completado exitosamente el programa.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al finalizar y aprobar el programa para estudiante {StudentId}", request.StudentId);
            return Result<string>.Failure("FINALIZATION_ERROR", "Error al finalizar el programa: " + ex.Message);
        }
    }

    public async Task<Result<bool>> SaveNonEvaluableMissionAsync(SaveMissionEvaluationDto dto)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Validar que NO haya N-Rojas
            var hasNRed = dto.TaskGrades.Any(tg => tg.Grade == GradeTypes.NR);
            if (hasNRed) return Result<bool>.Failure("INVALID_GRADE", "Las misiones no evaluables no pueden tener calificaciones N-Roja");

            // Verificar que no exista otra evaluación en la misma fecha
            bool exists = await _unitOfWork.Repository<NonEvaluableMissionRecord>()
                .AnyAsync(m => m.CourseId == dto.CourseId
                            && m.StudentId == dto.StudentId
                            && m.Date.Date == dto.EvaluationDate.Date);

            if (exists) return Result<bool>.Failure(SystemErrors.EvaluationError.EvaluationExists);

            // Validar que la fecha de evaluación no sea futura
            if (dto.EvaluationDate.Date > DateTime.Now.Date)
            {
                return Result<bool>.Failure(SystemErrors.EvaluationError.FutureEvaluationDate);
            }

            // Validar que la fecha de evaluación no tenga más de 30 días de atraso
            var daysDifference = (DateTime.Now.Date - dto.EvaluationDate.Date).TotalDays;
            if (daysDifference > 30)
            {
                return Result<bool>.Failure(SystemErrors.EvaluationError.EvaluationDateTooOld);
            }

            // Validar que la fecha de evaluación sea posterior a la última misión evaluada
            var lastEvaluationDate = await GetLastEvaluationDateAsync(dto.StudentId, dto.CourseId);
            if (lastEvaluationDate.HasValue && dto.EvaluationDate.Date <= lastEvaluationDate.Value.Date)
            {
                return Result<bool>.Failure(SystemErrors.EvaluationError.InvalidChronologicalOrder(lastEvaluationDate.Value));
            }

            // Calcular número de misión no evaluable (MNE1, MNE2...)
            int missionNumber = await GetNextNonEvaluableMissionNumberAsync(dto.StudentId, dto.PhaseId, dto.CourseId);

            // Crear registro de misión no evaluable
            var missionRecord = new NonEvaluableMissionRecord
            {
                StudentId = dto.StudentId,
                InstructorId = dto.InstructorId,
                PhaseId = dto.PhaseId,
                CourseId = dto.CourseId,
                AircraftId = dto.AircraftId,
                NonEvaluableMissionNumber = missionNumber,
                Date = dto.EvaluationDate,
                Observations = dto.GeneralObservations ?? string.Empty,
                ManFlightHours = Math.Round(dto.MachineFlightHours * 1.3, 1),
                MachineFlightHours = dto.MachineFlightHours
            };

            await _unitOfWork.Repository<NonEvaluableMissionRecord>().AddAsync(missionRecord);

            // agregar el Id de la mision no evaluable al dto
            dto.MissionId = missionRecord.Id;

            // Guardar calificaciones de tareas
            await SaveNonEvaluableTaskGradesAsync(dto);

            // Registrar horas de vuelo
            await CreateUpdateFlightHourLogsAsync(dto);

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation(
                "Misión no evaluable MNE{MissionNumber} guardada - Estudiante: {StudentId}, programa: {CourseId}",
                missionNumber, dto.StudentId, dto.CourseId);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al guardar misión no evaluable - Estudiante: {StudentId}", dto.StudentId);
            return Result<bool>.Failure(SystemErrors.EvaluationError.EvaluationSaveError);
        }
    }

    #region Métodos de Edición de Calificaciones

    /// <summary>
    /// Obtiene los datos de una misión para editar sus calificaciones
    /// </summary>
    public async Task<Result<MissionGradesEditViewDto>> GetMissionGradesForEditAsync(Guid missionId, Guid studentId, Guid courseId, Guid instructorId)
    {
        // 1. Obtener el progreso de la misión
        var missionProgress = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetFirstAsync(
                smp => smp.MissionId == missionId
                    && smp.StudentId == studentId
                    && smp.CourseId == courseId,
                includeFunc: query => query
                    .Include(smp => smp.Mission)
                    .Include(smp => smp.Student)
                    .Include(smp => smp.StudentTaskGrades)
                        .ThenInclude(stg => stg.Task)
                    .Include(smp => smp.StudentTaskGrades)
                        .ThenInclude(stg => stg.StudentGradeNRedReasons)
            );

        if (missionProgress == null)
            return Result<MissionGradesEditViewDto>.Failure(SystemErrors.MissionError.MissionNotFound);

        // 2. Validar que la misión sea de la fase actual del estudiante
        var currentPhaseProgress = await _unitOfWork.Repository<StudentPhaseProgress>()
            .GetFirstAsync(pp => pp.StudentId == studentId && pp.CourseId == courseId && pp.IsCurrentPhase);

        if (currentPhaseProgress == null || currentPhaseProgress.PhaseId != missionProgress.PhaseId)
            return Result<MissionGradesEditViewDto>.Failure(SystemErrors.PhaseError.NotCurrentPhase);

        // 3. Validar que no hayan pasado más de los días estipulados desde la evaluación
        var daysSinceEvaluation = (DateTime.Now - missionProgress.Date).TotalDays;
        if (daysSinceEvaluation > MaxEditCount)
            return Result<MissionGradesEditViewDto>.Failure(SystemErrors.TaskError.EvaluationEditTimeLimitExceeded);

        // 4. Validar que el instructor sea el mismo que calificó
        if (missionProgress.InstructorId != instructorId)
            return Result<MissionGradesEditViewDto>.Failure(SystemErrors.TaskError.InstructorMismatch);

        // 5. Obtener información de las tareas de la misión para saber cuáles son P3
        var missionTasks = await _unitOfWork.Repository<MissionTask>()
            .GetListAsync(mt => mt.MissionId == missionId);

        var p3TaskIds = missionTasks
            .Where(mt => mt.IsP3Task)
            .Select(mt => mt.TaskId)
            .ToHashSet();

        // 6. Construir la lista de tareas con información de editabilidad
        var taskGrades = new List<TaskGradeEditDto>();

        foreach (var taskGrade in missionProgress.StudentTaskGrades.OrderBy(stg => stg.Task.Code))
        {
            bool canEdit = true;
            string? reasonCannotEdit = null;

            // No se pueden editar notas que ya son N o N-Roja
            if (taskGrade.Grade == GradeTypes.N || taskGrade.Grade == GradeTypes.NR)
            {
                canEdit = false;
                reasonCannotEdit = "No se pueden editar calificaciones N o N-Roja";
            }
            // Validar límite de ediciones
            else if (taskGrade.EditCount >= MaxEditCount)
            {
                canEdit = false;
                reasonCannotEdit = $"Se alcanzó el límite máximo de ediciones ({MaxEditCount})";
            }

            // Obtener razones actuales de N o N-Roja si existen
            var currentNRedReasons = taskGrade.StudentGradeNRedReasons
                .Select(r => new NRedReasonDto
                {
                    ReasonCategory = r.ReasonCategory,
                    ReasonDescription = r.ReasonDescription
                })
                .ToList();

            taskGrades.Add(new TaskGradeEditDto
            {
                TaskGradeId = taskGrade.Id,
                TaskId = taskGrade.TaskId,
                TaskCode = taskGrade.Task.Code,
                TaskName = taskGrade.Task.Name,
                CurrentGrade = taskGrade.Grade,
                IsP3 = p3TaskIds.Contains(taskGrade.TaskId),
                CanEdit = canEdit,
                ReasonCannotEdit = reasonCannotEdit,
                EditCount = taskGrade.EditCount,
                CurrentNRedReasons = currentNRedReasons
            });
        }

        // 7. Calcular misiones fallidas en ventana y totales para las alertas
        // Calcular total de misiones fallidas en todo el programa y de las últimas 5 misiones
        int windowSize = _options.LastMissionsWindow;
        var (totalFailedInCourse, failedInWindow) = await GetStudentFailureMetricsAsync(studentId, courseId, currentPhaseProgress.PhaseId, windowSize);

        // 8. Construir el DTO de respuesta
        var viewDto = new MissionGradesEditViewDto
        {
            MissionId = missionProgress.MissionId,
            MissionName = missionProgress.Mission.Name,
            MissionNumber = missionProgress.Mission.MissionNumber,
            EvaluationDate = missionProgress.Date,
            GeneralObservations = missionProgress.Observations ?? string.Empty,
            StudentId = studentId,
            StudentFullIdentification = $"{missionProgress.Student.DocumentType} - {missionProgress.Student.IdentificationNumber}",
            StudentFullName = $"{missionProgress.Student.Grade}. {missionProgress.Student.Name} {missionProgress.Student.LastName}",
            CourseId = courseId,
            PhaseId = missionProgress.PhaseId,
            EvaluatorInstructorId = missionProgress.InstructorId,
            TaskGrades = taskGrades,
            CurrentFailedMissionsInWindow = failedInWindow,
            CurrentTotalFailedMissions = totalFailedInCourse,
            IsMissionCurrentlyFailed = !missionProgress.MissionPassed
        };

        return Result<MissionGradesEditViewDto>.Success(viewDto);
    }

    /// <summary>
    /// Guarda las ediciones de calificaciones de una misión
    /// </summary>
    public async Task<Result<bool>> SaveMissionGradesEditAsync(EditMissionGradesDto editDto)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Obtener el progreso de la misión
            var missionProgress = await _unitOfWork.Repository<StudentMissionProgress>()
                .GetFirstAsync(
                    smp => smp.MissionId == editDto.MissionId
                        && smp.StudentId == editDto.StudentId
                        && smp.CourseId == editDto.CourseId,
                    includeFunc: query => query
                        .Include(smp => smp.StudentTaskGrades)
                            .ThenInclude(stg => stg.StudentGradeNRedReasons)
                );

            if (missionProgress == null)
                return Result<bool>.Failure(SystemErrors.MissionError.MissionNotFound);

            // Procesar cada tarea editada
            foreach (var editedTask in editDto.TaskGrades)
            {
                // Solo procesar si la nota cambió
                if (editedTask.OldGrade == editedTask.NewGrade)
                    continue;

                var taskGrade = missionProgress.StudentTaskGrades
                    .FirstOrDefault(tg => tg.Id == editedTask.TaskGradeId);

                if (taskGrade == null)
                    continue;

                // Validar que puede editar esta tarea específica
                if (!editedTask.CanEdit)
                    continue;

                // Validar que no sea N o N-Roja (doble validación)
                if (taskGrade.Grade == GradeTypes.N || taskGrade.Grade == GradeTypes.NR)
                    continue;

                // Validar límite de ediciones
                if (taskGrade.EditCount >= 2)
                    continue;

                // Actualizar la calificación
                taskGrade.Grade = editedTask.NewGrade.ToUpper();
                taskGrade.EditCount++;
                taskGrade.LastEditDate = DateTime.Now;

                // Si cambió a N o N-Roja, agregar razones
                if (editedTask.NewGrade == GradeTypes.N || editedTask.NewGrade == GradeTypes.NR)
                {
                    // Eliminar razones anteriores si existen
                    if (taskGrade.StudentGradeNRedReasons.Count != 0)
                    {
                        foreach (var oldReason in taskGrade.StudentGradeNRedReasons.ToList())
                        {
                            _unitOfWork.Repository<StudentGradeNRedReason>().Delete(oldReason);
                        }
                        taskGrade.StudentGradeNRedReasons.Clear();
                    }

                    // Agregar nuevas razones
                    foreach (var reason in editedTask.NRedReasons)
                    {
                        var newReason = new StudentGradeNRedReason
                        {
                            StudentTaskGradeId = taskGrade.Id,
                            ReasonCategory = reason.ReasonCategory,
                            ReasonDescription = reason.ReasonDescription,
                            Date = DateTime.Now
                        };
                        await _unitOfWork.Repository<StudentGradeNRedReason>().AddAsync(newReason);
                    }
                }
                // Si cambió de N/NR a otra nota, eliminar razones
                else if (taskGrade.StudentGradeNRedReasons.Count != 0)
                {
                    foreach (var oldReason in taskGrade.StudentGradeNRedReasons.ToList())
                    {
                        _unitOfWork.Repository<StudentGradeNRedReason>().Delete(oldReason);
                    }
                    taskGrade.StudentGradeNRedReasons.Clear();
                }

                // Registrar en log
                _logger.LogInformation("Calificación editada - TaskGradeId: {TaskGradeId}, Estudiante: {StudentId}, Old: {OldGrade}, New: {NewGrade}",
                    taskGrade.Id, editDto.StudentId, editedTask.OldGrade, editedTask.NewGrade);

                _unitOfWork.Repository<StudentTaskGrade>().Update(taskGrade);
            }

            // Recalcular StudentMissionProgress
            // IMPORTANTE: missionProgress.StudentTaskGrades ya contiene TODAS las notas actualizadas
            // (las editadas en el bucle anterior + las que no se tocaron)
            // Por eso contamos de aquí y no de editDto.TaskGrades que solo tiene las tareas editadas
            var nRedCount = missionProgress.StudentTaskGrades.Count(tg => tg.Grade == GradeTypes.NR);
            bool wasMissionFailed = !missionProgress.MissionPassed;

            missionProgress.CriticalFailures = nRedCount;
            missionProgress.MissionPassed = nRedCount == 0;
            missionProgress.Observations = editDto.GeneralObservations;

            _unitOfWork.Repository<StudentMissionProgress>().Update(missionProgress);

            // 5. Recalcular StudentPhaseProgress
            var currentPhaseProgress = await _unitOfWork.Repository<StudentPhaseProgress>()
                .GetFirstAsync(pp => pp.StudentId == editDto.StudentId && pp.CourseId == editDto.CourseId && pp.IsCurrentPhase);

            if (currentPhaseProgress != null)
            {
                // 6. Evaluar si cambió de aprobada a fallida o viceversa
                bool nowMissionFailed = !missionProgress.MissionPassed;

                // Si cambió de aprobada a fallida, evaluar comité/suspensión
                if (!wasMissionFailed && nowMissionFailed)
                {
                    _logger.LogWarning("Misión cambió de APROBADA a FALLIDA por edición - MissionId: {MissionId}, StudentId: {StudentId}",
                        editDto.MissionId, editDto.StudentId);

                    // Verificar si tiene factor emocional las nuevas notas
                    var hasEmotionalFactor = editDto.TaskGrades.Any(tg => tg.NRedReasons != null && tg.NRedReasons.Any(nr => nr.ReasonCategory == NRedCategories.Emotional));

                    // Crear un DTO temporal para evaluar comité
                    var tempEvaluationDto = new SaveMissionEvaluationDto
                    {
                        StudentId = editDto.StudentId,
                        CourseId = editDto.CourseId,
                        PhaseId = missionProgress.PhaseId,
                        MissionId = editDto.MissionId,
                        InstructorId = editDto.InstructorId,
                        EvaluationDate = missionProgress.Date,
                        TaskGrades = []
                    };

                    var committeeEvaluation = await EvaluateCommitteeRequirementsAsync(tempEvaluationDto, true, hasEmotionalFactor);

                    if (committeeEvaluation.RequiresSuspension)
                    {
                        currentPhaseProgress.Status = UserConstants.StudentStatus.Suspended;
                        currentPhaseProgress.IsCurrentPhase = false;
                        currentPhaseProgress.IsSuspended = true;
                        currentPhaseProgress.SuspensionDate = DateTime.Now;
                        currentPhaseProgress.SuspensionReason = committeeEvaluation.Reason;
                        currentPhaseProgress.EndDate = DateTime.Now;
                        await CreateSuspensionRecordAsync(tempEvaluationDto, committeeEvaluation);
                    }
                    else if (committeeEvaluation.RequiresCommittee)
                    {
                        currentPhaseProgress.Status = UserConstants.StudentStatus.PendingCommittee;
                        await CreateCommitteeRecordAsync(tempEvaluationDto, committeeEvaluation);
                    }
                }
                // Si cambió de fallida a aprobada
                else if (wasMissionFailed && !nowMissionFailed)
                {
                    _logger.LogInformation("Misión cambió de FALLIDA a APROBADA por edición - MissionId: {MissionId}, StudentId: {StudentId}",
                        editDto.MissionId, editDto.StudentId);
                }

                _unitOfWork.Repository<StudentPhaseProgress>().Update(currentPhaseProgress);
            }

            // Guardar todos los cambios
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Calificaciones editadas exitosamente - MissionId: {MissionId}, StudentId: {StudentId}, InstructorId: {InstructorId}",
                editDto.MissionId, editDto.StudentId, editDto.InstructorId);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al guardar ediciones de calificaciones - MissionId: {MissionId}, StudentId: {StudentId}",
                editDto.MissionId, editDto.StudentId);
            return Result<bool>.Failure(SystemErrors.EvaluationError.EvaluationSaveError);
        }
    }


    #endregion

    #region Métodos de Edición de Calificaciones para Misiones No Evaluables

    /// <summary>
    /// Obtiene los datos de una misión no evaluable para editar sus calificaciones
    /// </summary>
    public async Task<Result<MissionGradesEditViewDto>> GetNonEvaluableMissionGradesForEditAsync(
        Guid nonEvaluableMissionRecordId,
        Guid studentId,
        Guid courseId,
        Guid instructorId)
    {
        // 1. Obtener el registro de la misión no evaluable
        var missionRecord = await _unitOfWork.Repository<NonEvaluableMissionRecord>()
            .GetFirstAsync(
                nem => nem.Id == nonEvaluableMissionRecordId
                    && nem.StudentId == studentId
                    && nem.CourseId == courseId,
                includeFunc: query => query
                    .Include(nem => nem.Student)
                    .Include(nem => nem.NonEvaluableTaskGrades)
                        .ThenInclude(ntg => ntg.Task)
                    .Include(nem => nem.NonEvaluableTaskGrades)
                        .ThenInclude(ntg => ntg.NonEvaluableGradeReasons)
            );

        if (missionRecord == null)
            return Result<MissionGradesEditViewDto>.Failure(SystemErrors.MissionError.MissionNotFound);

        // 2. Validar que la misión es de la fase actual del estudiante
        var currentPhaseProgress = await _unitOfWork.Repository<StudentPhaseProgress>()
            .GetFirstAsync(pp => pp.StudentId == studentId && pp.CourseId == courseId && pp.IsCurrentPhase);

        if (currentPhaseProgress == null || currentPhaseProgress.PhaseId != missionRecord.PhaseId)
            return Result<MissionGradesEditViewDto>.Failure(SystemErrors.PhaseError.NotCurrentPhase);

        // 3. Validar que no hayan pasado más de los días estipulados desde la evaluación
        var daysSinceEvaluation = (DateTime.Now - missionRecord.Date).TotalDays;
        if (daysSinceEvaluation > MaxEditCount)
            return Result<MissionGradesEditViewDto>.Failure(SystemErrors.TaskError.EvaluationEditTimeLimitExceeded);

        // 4. Validar que el instructor sea el mismo que calificó
        if (missionRecord.InstructorId != instructorId)
            return Result<MissionGradesEditViewDto>.Failure(SystemErrors.TaskError.InstructorMismatch);

        // 5. Construir la lista de tareas con información de editabilidad
        var taskGrades = new List<TaskGradeEditDto>();

        foreach (var taskGrade in missionRecord.NonEvaluableTaskGrades.OrderBy(ntg => ntg.Task.Code))
        {
            bool canEdit = true;
            string? reasonCannotEdit = null;

            // No se pueden editar notas que ya son N (no hay N-Roja en misiones no evaluables)
            if (taskGrade.Grade == GradeTypes.N)
            {
                canEdit = false;
                reasonCannotEdit = "No se pueden editar calificaciones N";
            }
            // Validar límite de ediciones
            else if (taskGrade.EditCount >= MaxEditCount)
            {
                canEdit = false;
                reasonCannotEdit = $"Se alcanzó el límite máximo de ediciones ({MaxEditCount})";
            }

            // Obtener razones actuales de N si existen
            var currentNReasons = taskGrade.NonEvaluableGradeReasons
                .Select(r => new NRedReasonDto
                {
                    ReasonCategory = r.ReasonCategory,
                    ReasonDescription = r.ReasonDescription
                })
                .ToList();

            taskGrades.Add(new TaskGradeEditDto
            {
                TaskGradeId = taskGrade.Id,
                TaskId = taskGrade.TaskId,
                TaskCode = taskGrade.Task.Code,
                TaskName = taskGrade.Task.Name,
                CurrentGrade = taskGrade.Grade,
                IsP3 = false, // Las misiones no evaluables no tienen tareas P3
                CanEdit = canEdit,
                ReasonCannotEdit = reasonCannotEdit,
                EditCount = taskGrade.EditCount,
                CurrentNRedReasons = currentNReasons
            });
        }

        // 6. Construir el DTO de respuesta
        var viewDto = new MissionGradesEditViewDto
        {
            MissionId = missionRecord.Id,
            MissionName = $"Misión No Evaluable MNE{missionRecord.NonEvaluableMissionNumber}",
            MissionNumber = missionRecord.NonEvaluableMissionNumber,
            EvaluationDate = missionRecord.Date,
            GeneralObservations = missionRecord.Observations ?? string.Empty,
            StudentId = studentId,
            StudentFullIdentification = $"{missionRecord.Student.DocumentType} - {missionRecord.Student.IdentificationNumber}",
            StudentFullName = $"{missionRecord.Student.Grade}. {missionRecord.Student.Name} {missionRecord.Student.LastName}",
            CourseId = courseId,
            PhaseId = missionRecord.PhaseId,
            EvaluatorInstructorId = missionRecord.InstructorId,
            TaskGrades = taskGrades,
            CurrentFailedMissionsInWindow = 0, // No aplica para misiones no evaluables
            CurrentTotalFailedMissions = 0, // No aplica para misiones no evaluables
            IsMissionCurrentlyFailed = false // No aplica para misiones no evaluables
        };

        return Result<MissionGradesEditViewDto>.Success(viewDto);
    }

    /// <summary>
    /// Guarda las ediciones de calificaciones de una misión no evaluable
    /// </summary>
    public async Task<Result<bool>> SaveNonEvaluableMissionGradesEditAsync(EditMissionGradesDto editDto)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Obtener el registro de la misión no evaluable
            var missionRecord = await _unitOfWork.Repository<NonEvaluableMissionRecord>()
                .GetFirstAsync(
                    nem => nem.Id == editDto.MissionId
                        && nem.StudentId == editDto.StudentId
                        && nem.CourseId == editDto.CourseId,
                    includeFunc: query => query
                        .Include(nem => nem.NonEvaluableTaskGrades)
                            .ThenInclude(ntg => ntg.NonEvaluableGradeReasons)
                );

            if (missionRecord == null)
                return Result<bool>.Failure(SystemErrors.MissionError.MissionNotFound);

            // Procesar cada tarea editada
            foreach (var editedTask in editDto.TaskGrades)
            {
                // Solo procesar si la nota cambió
                if (editedTask.OldGrade == editedTask.NewGrade)
                    continue;

                var taskGrade = missionRecord.NonEvaluableTaskGrades
                    .FirstOrDefault(tg => tg.Id == editedTask.TaskGradeId);

                if (taskGrade == null)
                    continue;

                // Validar que puede editar esta tarea específica
                if (!editedTask.CanEdit)
                    continue;

                // Validar que no sea N (doble validación)
                if (taskGrade.Grade == GradeTypes.N)
                    continue;

                // Validar límite de ediciones
                if (taskGrade.EditCount >= MaxEditCount)
                    continue;

                // Actualizar la calificación
                taskGrade.Grade = editedTask.NewGrade.ToUpper();
                taskGrade.EditCount++;
                taskGrade.LastEditDate = DateTime.Now;

                // Si cambió a N, agregar razones
                if (editedTask.NewGrade == GradeTypes.N)
                {
                    // Eliminar razones anteriores si existen
                    if (taskGrade.NonEvaluableGradeReasons.Count != 0)
                    {
                        foreach (var oldReason in taskGrade.NonEvaluableGradeReasons.ToList())
                        {
                            _unitOfWork.Repository<NonEvaluableGradeReason>().Delete(oldReason);
                        }
                        taskGrade.NonEvaluableGradeReasons.Clear();
                    }

                    // Agregar nuevas razones
                    foreach (var reason in editedTask.NRedReasons)
                    {
                        var newReason = new NonEvaluableGradeReason
                        {
                            NonEvaluableTaskGradeId = taskGrade.Id,
                            ReasonCategory = reason.ReasonCategory,
                            ReasonDescription = reason.ReasonDescription,
                            Date = DateTime.Now
                        };
                        await _unitOfWork.Repository<NonEvaluableGradeReason>().AddAsync(newReason);
                    }
                }
                // Si cambió de N a otra nota, eliminar razones
                else if (taskGrade.NonEvaluableGradeReasons.Count != 0)
                {
                    foreach (var oldReason in taskGrade.NonEvaluableGradeReasons.ToList())
                    {
                        _unitOfWork.Repository<NonEvaluableGradeReason>().Delete(oldReason);
                    }
                    taskGrade.NonEvaluableGradeReasons.Clear();
                }

                // Registrar en log
                _logger.LogInformation("Calificación de misión no evaluable editada - TaskGradeId: {TaskGradeId}, Estudiante: {StudentId}, Old: {OldGrade}, New: {NewGrade}",
                    taskGrade.Id, editDto.StudentId, editedTask.OldGrade, editedTask.NewGrade);

                _unitOfWork.Repository<NonEvaluableTaskGrade>().Update(taskGrade);
            }

            // Actualizar observaciones generales
            missionRecord.Observations = editDto.GeneralObservations;
            _unitOfWork.Repository<NonEvaluableMissionRecord>().Update(missionRecord);

            // Guardar todos los cambios
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Calificaciones de misión no evaluable editadas exitosamente - MissionRecordId: {MissionId}, StudentId: {StudentId}, InstructorId: {InstructorId}",
                editDto.MissionId, editDto.StudentId, editDto.InstructorId);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al guardar ediciones de calificaciones de misión no evaluable - MissionRecordId: {MissionId}, StudentId: {StudentId}",
                editDto.MissionId, editDto.StudentId);
            return Result<bool>.Failure(SystemErrors.EvaluationError.EvaluationSaveError);
        }
    }

    #endregion

}