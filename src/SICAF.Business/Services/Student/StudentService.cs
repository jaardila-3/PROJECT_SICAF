using Microsoft.EntityFrameworkCore;

using SICAF.Business.Interfaces.Student;
using SICAF.Business.Mappers.Identity;
using SICAF.Common.DTOs.Academic;
using SICAF.Common.Models.Results;
using SICAF.Data.Entities.Academic;
using SICAF.Data.Interfaces.Repositories;

namespace SICAF.Business.Services.Student;

public class StudentService(
    IUnitOfWork unitOfWork
) : IStudentService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<MissionEvaluationDetailDto>> GetMissionEvaluationDetailAsync(Guid missionId, Guid studentId, Guid courseId)
    {
        // Obtener el progreso de la misión con todas las relaciones necesarias
        var missionProgress = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetFirstAsync(
                predicate: smp => smp.MissionId == missionId && smp.StudentId == studentId && smp.CourseId == courseId,
                includeFunc: query => query
                    .Include(smp => smp.Student)
                        .ThenInclude(s => s.AviationProfile)
                    .Include(smp => smp.Instructor)
                    .Include(smp => smp.Mission)
                    .Include(smp => smp.Phase)
                    .Include(smp => smp.Course)
                    .Include(smp => smp.Aircraft)
            );

        if (missionProgress == null)
            return Result<MissionEvaluationDetailDto>.Failure(
                "MISSION_PROGRESS_NOT_FOUND",
                "No se encontró la evaluación de la misión"
            );

        // Obtener todas las calificaciones de tareas de esta misión
        var taskGrades = await _unitOfWork.Repository<StudentTaskGrade>()
            .GetListAsync(
                predicate: stg => stg.StudentMissionProgressId == missionProgress.Id,
                includeFunc: query => query
                    .Include(stg => stg.Task)
                    .Include(stg => stg.StudentGradeNRedReasons)
            );

        // Obtener información de horas de vuelo del estudiante
        var flightHourLog = await _unitOfWork.Repository<FlightHourLog>()
            .GetFirstAsync(fhl =>
                fhl.UserId == missionProgress.StudentId &&
                fhl.MissionId == missionProgress.MissionId &&
                fhl.CourseId == missionProgress.CourseId
            );

        // Construir el DTO de detalle
        var student = missionProgress.Student.MapToDto();
        var detail = new MissionEvaluationDetailDto
        {
            // Estudiante                
            StudentId = missionProgress.StudentId,
            StudentFullName = student.FullName,
            StudentIdentification = student.FullIdentification,
            PhotoBase64 = student.PhotoBase64,

            // Instructor
            InstructorId = missionProgress.InstructorId,
            InstructorFullName = $"{missionProgress.Instructor.Grade}. {missionProgress.Instructor.Name} {missionProgress.Instructor.LastName}",
            InstructorIdentification = missionProgress.Instructor.IdentificationNumber,

            // Curso y fase
            CourseId = missionProgress.CourseId,
            CourseName = missionProgress.Course.CourseName,
            PhaseId = missionProgress.PhaseId,
            PhaseName = missionProgress.Phase.Name,
            PhaseNumber = missionProgress.Phase.PhaseNumber,

            // Misión
            MissionId = missionProgress.MissionId,
            MissionName = missionProgress.Mission.Name,
            MissionNumber = missionProgress.Mission.MissionNumber,
            FlightHours = missionProgress.Mission.FlightHours,
            WingType = missionProgress.Mission.WingType,

            // Evaluación
            StudentMissionProgressId = missionProgress.Id,
            EvaluationDate = missionProgress.Date,
            MissionPassed = missionProgress.MissionPassed,
            CriticalFailures = missionProgress.CriticalFailures,
            GeneralObservations = missionProgress.Observations,

            // Aeronave
            AircraftRegistration = missionProgress.Aircraft.Registration,
            AircraftType = missionProgress.Aircraft.AircraftType,

            // Horas de vuelo
            MachineFlightHours = flightHourLog?.MachineFlightHours ?? 0,
            ManFlightHours = flightHourLog?.ManFlightHours ?? 0,
            SilaboFlightHours = flightHourLog?.SilaboFlightHours ?? 0,

            // Calificaciones
            TaskGrades = taskGrades.Select(tg => new TaskGradeDetailDto
            {
                TaskId = tg.TaskId,
                TaskCode = tg.Task.Code,
                TaskName = tg.Task.Name,
                Grade = tg.Grade,
                NRedReasons = tg.StudentGradeNRedReasons.Select(nr => new NRedReasonDto
                {
                    ReasonCategory = nr.ReasonCategory,
                    ReasonDescription = nr.ReasonDescription
                })
            })
        };

        return Result<MissionEvaluationDetailDto>.Success(detail);

    }

    public async Task<Result<IEnumerable<StudentCommitteeRecordDto>>> GetCommitteeRecordsByPhaseAsync(Guid studentId, Guid courseId, Guid phaseId)
    {
        var committeeRecords = await _unitOfWork.Repository<StudentCommitteeRecord>()
            .GetListAsync(
                predicate: cr => cr.StudentId == studentId
                              && cr.CourseId == courseId
                              && cr.PhaseId == phaseId,
                orderBy: query => query.OrderBy(cr => cr.Date)
            );

        var committeeRecordDtos = committeeRecords.Select(cr => new StudentCommitteeRecordDto
        {
            Id = cr.Id,
            StudentId = cr.StudentId,
            CourseId = cr.CourseId,
            PhaseId = cr.PhaseId,
            LeaderId = cr.LeaderId,
            CommitteeNumber = cr.CommitteeNumber,
            Reason = cr.Reason,
            ActaNumber = cr.ActaNumber,
            Decision = cr.Decision,
            DecisionDate = cr.DecisionDate,
            DecisionObservations = cr.DecisionObservations,
            IsResolved = cr.IsResolved,
            Date = cr.Date,
        }).ToList();

        return Result<IEnumerable<StudentCommitteeRecordDto>>.Success(committeeRecordDtos);
    }

    public async Task<Result<MissionEvaluationDetailDto>> GetNonEvaluableMissionDetailAsync(Guid nonEvaluableMissionRecordId, Guid studentId, Guid courseId)
    {
        // Obtener el registro de la misión no evaluable con todas las relaciones necesarias
        var missionRecord = await _unitOfWork.Repository<NonEvaluableMissionRecord>()
            .GetFirstAsync(
                predicate: nem => nem.Id == nonEvaluableMissionRecordId
                    && nem.StudentId == studentId
                    && nem.CourseId == courseId,
                includeFunc: query => query
                    .Include(nem => nem.Student)
                        .ThenInclude(s => s.AviationProfile)
                    .Include(nem => nem.Instructor)
                    .Include(nem => nem.Phase)
                    .Include(nem => nem.Course)
                    .Include(nem => nem.Aircraft)
            );

        if (missionRecord == null)
            return Result<MissionEvaluationDetailDto>.Failure(
                "NON_EVALUABLE_MISSION_NOT_FOUND",
                "No se encontró la misión no evaluable"
            );

        // Obtener todas las calificaciones de tareas de esta misión no evaluable
        var taskGrades = await _unitOfWork.Repository<NonEvaluableTaskGrade>()
            .GetListAsync(
                predicate: ntg => ntg.NonEvaluableMissionRecordId == nonEvaluableMissionRecordId,
                includeFunc: query => query
                    .Include(ntg => ntg.Task)
                    .Include(ntg => ntg.NonEvaluableGradeReasons)
            );

        // Obtener información de horas de vuelo del estudiante
        var flightHourLog = await _unitOfWork.Repository<FlightHourLog>()
            .GetFirstAsync(fhl =>
                fhl.UserId == missionRecord.StudentId &&
                fhl.NonEvaluableMissionId == nonEvaluableMissionRecordId &&
                fhl.CourseId == missionRecord.CourseId
            );

        // Construir el DTO de detalle
        var student = missionRecord.Student.MapToDto();
        var detail = new MissionEvaluationDetailDto
        {
            // Estudiante
            StudentId = missionRecord.StudentId,
            StudentFullName = student.FullName,
            StudentIdentification = student.FullIdentification,
            PhotoBase64 = student.PhotoBase64,

            // Instructor
            InstructorId = missionRecord.InstructorId,
            InstructorFullName = $"{missionRecord.Instructor.Grade}. {missionRecord.Instructor.Name} {missionRecord.Instructor.LastName}",
            InstructorIdentification = missionRecord.Instructor.IdentificationNumber,

            // Curso y fase
            CourseId = missionRecord.CourseId,
            CourseName = missionRecord.Course.CourseName,
            PhaseId = missionRecord.PhaseId,
            PhaseName = missionRecord.Phase.Name,
            PhaseNumber = missionRecord.Phase.PhaseNumber,

            // Misión no evaluable - usamos campos personalizados
            MissionId = missionRecord.Id, // Usamos el ID del registro
            MissionName = $"Misión No Evaluable MNE{missionRecord.NonEvaluableMissionNumber}",
            MissionNumber = missionRecord.NonEvaluableMissionNumber,
            FlightHours = missionRecord.MachineFlightHours,
            WingType = missionRecord.Phase.WingType,

            // Evaluación
            StudentMissionProgressId = missionRecord.Id, // Reutilizamos el campo
            EvaluationDate = missionRecord.Date,
            MissionPassed = true, // Las misiones no evaluables no tienen estado de aprobado/fallado
            CriticalFailures = 0, // No hay N-Rojas en misiones no evaluables
            GeneralObservations = missionRecord.Observations,

            // Aeronave
            AircraftRegistration = missionRecord.Aircraft.Registration,
            AircraftType = missionRecord.Aircraft.AircraftType,

            // Horas de vuelo
            MachineFlightHours = flightHourLog?.MachineFlightHours ?? missionRecord.MachineFlightHours,
            ManFlightHours = flightHourLog?.ManFlightHours ?? missionRecord.ManFlightHours,
            SilaboFlightHours = flightHourLog?.SilaboFlightHours ?? 0,

            // Calificaciones
            TaskGrades = taskGrades.Select(tg => new TaskGradeDetailDto
            {
                TaskId = tg.TaskId,
                TaskCode = tg.Task.Code,
                TaskName = tg.Task.Name,
                Grade = tg.Grade,
                NRedReasons = tg.NonEvaluableGradeReasons.Select(nr => new NRedReasonDto
                {
                    ReasonCategory = nr.ReasonCategory,
                    ReasonDescription = nr.ReasonDescription
                })
            })
        };

        return Result<MissionEvaluationDetailDto>.Success(detail);
    }
}