using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using SICAF.Business.Interfaces.Reports;
using SICAF.Business.Mappers.Identity;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Reports;
using SICAF.Common.Models.Results;
using SICAF.Data.Entities.Academic;
using SICAF.Data.Entities.Catalogs;
using SICAF.Data.Entities.Identity;
using SICAF.Data.Interfaces.Repositories;

namespace SICAF.Business.Services.Reports;

public partial class ReportService(
    IUnitOfWork unitOfWork
) : IReportService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private const string ALL_OPTION = UserConstants.ALL_OPTION;

    public async Task<List<(Guid Id, string Name)>> GetCoursesAsync(Guid? userId = null)
    {
        Expression<Func<UserCourse, bool>>? predicate = userId.HasValue ? uc => uc.UserId == userId.Value : null;
        // Obtener inscripciones del usuario
        var userCourses = await _unitOfWork.Repository<UserCourse>().GetListAsync(predicate, sc => sc.OrderBy(uc => uc.AssignmentDate));

        if (!userCourses.Any()) return [];

        // Extraer IDs únicos de cursos (eliminar duplicados)
        var uniqueCourseIds = userCourses.Select(uc => uc.CourseId).Distinct().ToList();

        // Obtener los cursos
        var courses = await _unitOfWork.Repository<Course>()
            .GetListAsync(predicate: c => uniqueCourseIds.Contains(c.Id));

        return courses.Select(c => (c.Id, c.CourseName)).ToList();
    }

    public async Task<List<(string Id, string Name)>> GetForcesByCourseAsync(Guid courseId)
    {
        // Obtener estudiantes del programa con su información de User
        var userCourses = await GetUserCoursesAsync(courseId);
        userCourses = [.. userCourses.Where(uc => uc.ParticipationType == SystemRoles.STUDENT)];

        // Extraer fuerzas únicas (excluyendo nulos)
        var forces = userCourses
            .Where(uc => !string.IsNullOrWhiteSpace(uc.User.Force))
            .Select(uc => uc.User.Force!)
            .Distinct()
            .OrderBy(f => f)
            .Select(f => (Id: f, Name: f))
            .ToList();

        for (int i = 0; i < forces.Count; i++)
        {
            var force = await _unitOfWork.Repository<MasterCatalog>()
                .GetFirstAsync(c => c.Code == forces[i].Name);

            forces[i] = (forces[i].Id, force?.Name ?? forces[i].Name);
        }

        // Agregar opción "TODAS" al inicio
        forces.Insert(0, (ALL_OPTION, "TODAS LAS FUERZAS"));

        return forces;
    }

    public async Task<List<(string Id, string Name)>> GetWingTypesByForceAsync(Guid courseId, string force)
    {
        var userCourses = await GetUserCoursesAsync(courseId, force);
        userCourses = [.. userCourses.Where(uc => uc.ParticipationType == SystemRoles.STUDENT)];

        // Extraer tipos de ala únicos (excluyendo nulos)
        var wingTypes = userCourses
            .Where(uc => !string.IsNullOrWhiteSpace(uc.WingType))
            .Select(uc => uc.WingType!)
            .Distinct()
            .OrderBy(wt => wt)
            .Select(wt => (Id: wt, Name: GetWingTypeName(wt)))
            .ToList();

        // Agregar opción "TODAS" al inicio
        wingTypes.Insert(0, (ALL_OPTION, "TODOS LOS TIPOS DE ALA"));

        return wingTypes;
    }

    public async Task<List<(Guid Id, string Name)>> GetPhasesByWingTypeAsync(string wingType)
    {
        // Obtener fases filtradas por tipo de ala
        var phases = await _unitOfWork.Repository<Phase>()
            .GetListAsync(
                predicate: p => p.WingType == wingType,
                orderBy: q => q.OrderBy(p => p.PhaseNumber)
            );

        var phaseList = phases
            .Select(p => (Id: p.Id, Name: p.Name))
            .ToList();

        // Agregar opción "TODAS LAS FASES" al inicio
        phaseList.Insert(0, (Guid.Empty, "TODAS LAS FASES"));

        return phaseList;
    }

    public async Task<List<(Guid Id, string Name)>> GetStudentsByFiltersAsync(Guid courseId, string force, string wingType, Guid? phaseId = null)
    {
        var userCourses = await GetUserCoursesAsync(courseId, force, wingType);
        userCourses = [.. userCourses.Where(uc => uc.ParticipationType == SystemRoles.STUDENT)];

        var studentIds = userCourses.Select(uc => uc.UserId).ToList();

        // Si hay filtro por fase, filtrar estudiantes que están o pasaron por esa fase
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            var phaseProgresses = await _unitOfWork.Repository<StudentPhaseProgress>()
                .GetListAsync(spp =>
                    studentIds.Contains(spp.StudentId) &&
                    spp.CourseId == courseId &&
                    spp.PhaseId == phaseId.Value
                );

            var filteredStudentIds = phaseProgresses.Select(spp => spp.StudentId).Distinct().ToList();
            userCourses = [.. userCourses.Where(uc => filteredStudentIds.Contains(uc.UserId))];
        }

        // Ordenar estudiantes por grado y antigüedad
        var students = userCourses
            .Select(uc => uc.User)
            .OrderBy(u => UserMapperExtensions.GetGradeOrder(u.Grade))
            .ThenBy(u => u.SeniorityOrder ?? int.MaxValue)
            .ThenBy(u => u.LastName)
            .ThenBy(u => u.Name)
            .Select(u => (Id: u.Id, Name: GetStudentDisplayName(u)))
            .ToList();

        return students;
    }

    public async Task<Result<GeneralReportDto>> GetGeneralReportDataAsync(Guid courseId, string force, string wingType, Guid? phaseId = null)
    {
        // Obtener información del programa
        var course = await _unitOfWork.Repository<Course>().GetFirstAsync(c => c.Id == courseId);
        var forceEntity = await _unitOfWork.Repository<MasterCatalog>().GetFirstAsync(c => c.Code == force || c.Name == force);
        force = forceEntity?.Code ?? force;

        if (course is null) return Result<GeneralReportDto>.Failure(SystemErrors.CourseError.NotFound);

        // Obtener información de la fase si se especificó
        Phase? phase = null;
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            phase = await _unitOfWork.Repository<Phase>().GetFirstAsync(p => p.Id == phaseId.Value);
        }

        var report = new GeneralReportDto
        {
            CourseId = course.Id,
            CourseName = course.CourseName,
            StartDate = course.StartDate,
            EndDate = course.EndDate,
            Force = forceEntity?.Name ?? force,
            WingType = wingType,
            PhaseId = phase?.Id,
            PhaseName = phase?.Name
        };

        // Obtener inscripciones del programa y ids de estudiantes
        var userCourses = await GetUserCoursesAsync(courseId, force, wingType);
        var studentIds = GetFilteredStudentIdsAsync(userCourses);

        // Obtener tabla de instructores y líderes de vuelo
        report.InstructorsTable = GetInstructorsAndLeadersTableAsync(userCourses);

        // Obtener estadísticas de estudiantes (con filtro de fase)
        var (students, totalStudents, activeStudents, suspendedStudents) = await GetStudentStatisticsAsync(userCourses, courseId, phaseId);
        report.Students = students;
        report.TotalStudents = totalStudents;
        report.ActiveStudents = activeStudents;
        report.SuspendedStudents = suspendedStudents;

        // Obtener total de comités (filtrado por fase si aplica)
        report.TotalCommittees = phaseId.HasValue && phaseId.Value != Guid.Empty
            ? await _unitOfWork.Repository<StudentCommitteeRecord>()
                .CountAsync(scr => scr.CourseId == courseId && studentIds.Contains(scr.StudentId) && scr.PhaseId == phaseId.Value)
            : await _unitOfWork.Repository<StudentCommitteeRecord>()
                .CountAsync(scr => scr.CourseId == courseId && studentIds.Contains(scr.StudentId));

        // Calcular distribución de calificaciones (con filtro de fase)
        report.GradeDistribution = await GetGradeDistributionAsync(studentIds, phaseId);

        // Datos de gráficas por máquina (con filtro de fase)
        report.MachineFlightHours = await GetMachineFlightHoursAsync(studentIds, courseId, phaseId);
        report.MachineUnsatisfactoryMissions = await GetMachineUnsatisfactoryMissionsAsync(studentIds, courseId, phaseId);

        // Datos de gráficas por instructor (con filtro de fase)
        report.InstructorFlightHours = await GetInstructorFlightHoursAsync(courseId, phaseId, wingType);
        report.InstructorUnsatisfactoryMissions = await GetInstructorUnsatisfactoryMissionsAsync(studentIds, courseId, phaseId);

        // Razones de N ROJA (con filtro de fase)
        report.NRedReasons = await GetNRedReasonsAsync(studentIds, phaseId);

        return Result<GeneralReportDto>.Success(report);
    }

    public async Task<Result<IndividualReportDto>> GetIndividualReportDataAsync(Guid studentId, Guid courseId, Guid? phaseId = null)
    {
        // Obtener información del estudiante
        var student = await _unitOfWork.Repository<User>()
            .GetFirstAsync(u => u.Id == studentId, includeFunc: q => q.Include(u => u.AviationProfile));

        if (student is null)
            return Result<IndividualReportDto>.Failure(SystemErrors.UserError.NotFound);

        // Obtener información del programa
        var course = await _unitOfWork.Repository<Course>().GetFirstAsync(c => c.Id == courseId);
        if (course is null)
            return Result<IndividualReportDto>.Failure(SystemErrors.CourseError.NotFound);

        // Obtener inscripción del estudiante al programa
        var userCourse = await _unitOfWork.Repository<UserCourse>()
            .GetFirstAsync(uc => uc.UserId == studentId && uc.CourseId == courseId && uc.ParticipationType == SystemRoles.STUDENT);
        if (userCourse is null)
            return Result<IndividualReportDto>.Failure(SystemErrors.EnrollmentError.StudentNotEnrolled);

        var forceEntity = await _unitOfWork.Repository<MasterCatalog>().GetFirstAsync(c => c.Code == student.Force);

        // Obtener información de la fase si se especificó
        Phase? phase = null;
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            phase = await _unitOfWork.Repository<Phase>().GetFirstAsync(p => p.Id == phaseId.Value);
        }

        var report = new IndividualReportDto
        {
            StudentId = studentId,
            PhotoBase64 = student.PhotoData != null && student.PhotoData.Length > 0
                ? $"data:{student.PhotoContentType};base64,{Convert.ToBase64String(student.PhotoData)}"
                : null,
            Grade = student.Grade ?? "",
            FullName = $"{student.Name} {student.LastName}",
            IdentificationType = student.DocumentType,
            IdentificationNumber = student.IdentificationNumber,
            PID = student.AviationProfile?.PID ?? "N/A",
            WingType = userCourse.WingType ?? "N/A",
            Force = forceEntity?.Name ?? "N/A",
            CourseName = course.CourseName,
            StartDate = course.StartDate,
            EndDate = course.EndDate,
            PhaseId = phase?.Id,
            CourseId = courseId,
            PhaseName = phase?.Name
        };

        // Obtener progreso de fases del estudiante
        var phaseProgresses = await _unitOfWork.Repository<StudentPhaseProgress>()
            .GetListAsync(spp => spp.StudentId == studentId && spp.CourseId == courseId, includeFunc: q => q.Include(spp => spp.Phase));

        var currentPhase = phaseProgresses.FirstOrDefault(pp => pp.IsCurrentPhase);
        var completedPhases = phaseProgresses.Where(pp => pp.PhasePassed).ToList();

        // Determinar estado del estudiante
        if (currentPhase != null)
        {
            report.StudentStatus = currentPhase.Status;
            report.CurrentPhase = currentPhase.Phase.Name;
            report.CurrentPhaseMissionsCompleted = currentPhase.CompletedMissions;
            report.CurrentPhaseTotalMissions = currentPhase.Phase.TotalMissions;
            report.CurrentPhaseMissionsPending = currentPhase.Phase.TotalMissions - currentPhase.CompletedMissions;
            report.StudentStatusDisplay = currentPhase.Status;
            report.CurrentPhaseProgressPercentage = currentPhase.Phase.TotalMissions > 0
                ? Math.Round((double)currentPhase.CompletedMissions / currentPhase.Phase.TotalMissions * 100, 2)
                : 0;
        }
        else
        {
            // No tiene fase actual, verificar si completó o fue suspendido
            var lastPhase = phaseProgresses.OrderByDescending(pp => pp.EndDate).FirstOrDefault();
            if (lastPhase != null)
            {
                report.StudentStatus = lastPhase.Status;
                report.StudentStatusDisplay = lastPhase.Status;
                report.CurrentPhase = null;
            }
            else
            {
                report.StudentStatus = "Sin Fase Asignada";
                report.StudentStatusDisplay = "Sin Fase Asignada";
            }
        }

        report.PhasesCompleted = completedPhases.Count;

        // Obtener misiones completadas
        var missionProgresses = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetListAsync(smp => smp.StudentId == studentId && smp.CourseId == courseId,
                includeFunc: q => q.Include(smp => smp.Mission).ThenInclude(m => m.Phase).Include(smp => smp.Aircraft)
            );

        // Si hay filtro por fase, filtrar solo misiones de esa fase
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            missionProgresses = [.. missionProgresses.Where(mp => mp.Mission.PhaseId == phaseId.Value)];

            // Obtener PhaseStatus del estudiante en esa fase
            var specificPhaseProgress = phaseProgresses.FirstOrDefault(pp => pp.PhaseId == phaseId.Value);
            report.PhaseStatus = specificPhaseProgress?.Status;
        }

        var missionIds = missionProgresses.Select(mp => mp.MissionId).ToList();

        report.TotalMissionsCompleted = missionProgresses.Count;

        // Obtener horas de vuelo (filtradas por misiones de la fase si aplica)
        var flightHours = phaseId.HasValue && phaseId.Value != Guid.Empty
            ? await _unitOfWork.Repository<FlightHourLog>()
                .GetListAsync(fhl =>
                    fhl.UserId == studentId &&
                    fhl.CourseId == courseId &&
                    fhl.Role == SystemRoles.STUDENT &&
                    fhl.MissionId.HasValue &&
                    missionIds.Contains(fhl.MissionId.Value)
                )
            : await _unitOfWork.Repository<FlightHourLog>()
                .GetListAsync(fhl =>
                    fhl.UserId == studentId &&
                    fhl.CourseId == courseId &&
                    fhl.Role == SystemRoles.STUDENT
                // Incluye todas las horas (evaluables y no evaluables)
                );

        report.TotalFlightHours = Math.Round((double)flightHours.Sum(fh => fh.ManFlightHours), 1);

        // Calcular total de misiones de la fase escogida o del programa para el tipo de ala del estudiante
        var allPhases = phaseId.HasValue && phaseId.Value != Guid.Empty
            ? await _unitOfWork.Repository<Phase>().GetListAsync(p => p.Id == phaseId.Value)
            : await _unitOfWork.Repository<Phase>().GetListAsync(p => p.WingType == userCourse.WingType);

        report.TotalMissionsInCourse = allPhases.Sum(p => p.TotalMissions);

        report.TotalCourseProgressPercentage = report.TotalMissionsInCourse > 0
            ? Math.Round((double)report.TotalMissionsCompleted / report.TotalMissionsInCourse * 100, 2)
            : 0;

        // Obtener calificaciones y calcular distribución (ya filtradas por las misiones)
        var taskGrades = await _unitOfWork.Repository<StudentTaskGrade>()
            .GetListAsync(stg => stg.StudentId == studentId && missionIds.Contains(stg.MissionId));

        report.GradeDistribution = new GradeDistributionDto
        {
            GradeA = taskGrades.Count(g => g.Grade == GradeConstants.GradeTypes.A),
            GradeB = taskGrades.Count(g => g.Grade == GradeConstants.GradeTypes.B),
            GradeC = taskGrades.Count(g => g.Grade == GradeConstants.GradeTypes.C),
            GradeN = taskGrades.Count(g => g.Grade == GradeConstants.GradeTypes.N),
            GradeNR = taskGrades.Count(g => g.Grade == GradeConstants.GradeTypes.NR),
            TotalGrades = taskGrades.Count(g => g.Grade != GradeConstants.GradeTypes.DM && g.Grade != GradeConstants.GradeTypes.SC)
        };

        // Calcular promedio académico
        report.Average = CalculateAverageGrade(report.GradeDistribution);

        // Obtener misiones insatisfactorias (ya filtradas por fase en missionProgresses)
        report.UnsatisfactoryMissions = await GetStudentUnsatisfactoryMissionsAsync(studentId, missionProgresses);

        // Obtener comités académicos (con filtro de fase)
        report.AcademicCommittees = await GetStudentCommitteesAsync(studentId, courseId, phaseId);

        // Construir historial de fases con sus misiones (solo si NO hay filtro por fase específica) para la vista de estudiante
        if (!phaseId.HasValue || phaseId.Value == Guid.Empty)
        {
            report.PhaseHistory = await BuildPhaseHistoryAsync(studentId, courseId, phaseProgresses, missionProgresses, taskGrades);
        }

        return Result<IndividualReportDto>.Success(report);
    }

    #region Métodos para los detalles de cada gráfico del reporte general    

    public async Task<Result<GradeDistributionDetailDto>> GetGradeDistributionDetailAsync(Guid courseId, string grade, string? force, string? wingType, Guid? phaseId)
    {
        var (course, forceEntity, userCourses, studentIds) = await GetReportInitialDataDetailAsync(courseId, force ?? ALL_OPTION, wingType ?? ALL_OPTION);

        force = forceEntity?.Code ?? force;

        // Normalizar el valor de grade
        var gradeValue = grade.ToUpper();

        // Consultar evaluaciones de tareas con la calificación específica
        var taskGrades = await _unitOfWork.Repository<StudentTaskGrade>()
            .GetListAsync(
                stg => studentIds.Contains(stg.StudentId) && stg.Grade == gradeValue,
                includeFunc: q => q
                    .Include(stg => stg.Student)
                    .Include(stg => stg.Task)
                    .Include(stg => stg.StudentMissionProgress)
                        .ThenInclude(smp => smp.Mission)
                        .ThenInclude(m => m.Phase)
                    .Include(stg => stg.StudentMissionProgress)
                        .ThenInclude(smp => smp.Aircraft)
                    .Include(stg => stg.StudentMissionProgress)
                        .ThenInclude(smp => smp.Instructor)
            );

        // Filtrar por fase si aplica
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            taskGrades = [.. taskGrades.Where(tg => tg.StudentMissionProgress.Mission.PhaseId == phaseId.Value)];
        }

        // Ordenar por fecha descendente
        taskGrades = [.. taskGrades.OrderByDescending(tg => tg.StudentMissionProgress.Date)];

        // Mapear a DTOs
        var records = taskGrades.Select((tg, index) => new GradeDetailRecordDto
        {
            Number = index + 1,
            StudentName = $"{tg.Student.Grade}. {tg.Student.LastName} {tg.Student.Name}",
            TaskName = tg.Task.Name,
            MissionPhaseName = $"{tg.StudentMissionProgress.Mission.Name} / {tg.StudentMissionProgress.Mission.Phase.Name}",
            AircraftRegistration = tg.StudentMissionProgress.Aircraft?.Registration ?? "N/A",
            EvaluationDate = tg.StudentMissionProgress.Date,
            InstructorName = tg.StudentMissionProgress.Instructor != null
                ? $"{tg.StudentMissionProgress.Instructor.Grade}. {tg.StudentMissionProgress.Instructor.LastName} {tg.StudentMissionProgress.Instructor.Name}"
                : "N/A"
        }).ToList();

        // Obtener nombre de fase si aplica
        string? phaseName = null;
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            var phase = await _unitOfWork.Repository<Phase>().GetFirstAsync(p => p.Id == phaseId.Value);
            phaseName = phase?.Name;
        }

        var result = new GradeDistributionDetailDto
        {
            ReportType = ReportDetailConstants.Grades,
            SelectedValue = gradeValue,
            Title = $"Distribución de Calificaciones: {gradeValue}",
            Subtitle = $"Total de registros: {records.Count}",
            CourseId = courseId,
            CourseName = course.CourseName,
            Force = forceEntity?.Name ?? force,
            WingType = wingType,
            PhaseId = phaseId,
            PhaseName = phaseName,
            Records = records
        };

        return Result<GradeDistributionDetailDto>.Success(result);
    }

    public async Task<Result<MachineFlightHoursDetailDto>> GetMachineFlightHoursDetailAsync(Guid courseId, string aircraftRegistration, string? force, string? wingType, Guid? phaseId)
    {
        var (course, forceEntity, userCourses, studentIds) = await GetReportInitialDataDetailAsync(courseId, force ?? ALL_OPTION, wingType ?? ALL_OPTION);

        force = forceEntity?.Code ?? force;

        // Consultar horas de vuelo en la aeronave específica (incluye misiones evaluables y no evaluables)
        var flightHours = await _unitOfWork.Repository<FlightHourLog>()
            .GetListAsync(
                fhl => fhl.CourseId == courseId
                    && studentIds.Contains(fhl.UserId)
                    && fhl.Aircraft.Registration == aircraftRegistration,
                includeFunc: q => q
                    .Include(fhl => fhl.User)
                    .Include(fhl => fhl.Aircraft)
                    .Include(fhl => fhl.Mission)
                        .ThenInclude(m => m!.Phase)
                    .Include(fhl => fhl.NonEvaluableMission)
                        .ThenInclude(nem => nem!.Phase)
                    .Include(fhl => fhl.NonEvaluableMission)
                        .ThenInclude(nem => nem!.Instructor)
            );

        // Filtrar por fase si aplica
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            flightHours = [.. flightHours.Where(fh =>
                (fh.Mission != null && fh.Mission.PhaseId == phaseId.Value) ||
                (fh.NonEvaluableMission != null && fh.NonEvaluableMission.PhaseId == phaseId.Value))];
        }

        // Obtener StudentMissionProgress para obtener instructores de misiones evaluables
        var missionProgressList = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetListAsync(
                smp => smp.CourseId == courseId && studentIds.Contains(smp.StudentId),
                includeFunc: q => q.Include(smp => smp.Instructor)
            );

        // Ordenar por fecha descendente
        flightHours = [.. flightHours.OrderByDescending(fh => fh.Date)];

        // Mapear a DTOs
        var records = flightHours.Select((fh, index) =>
        {
            string missionPhaseName;
            string instructorName;

            if (fh.Mission != null)
            {
                // Misión evaluable
                missionPhaseName = $"{fh.Mission.Name} / {fh.Mission.Phase.Name}";
                var progress = missionProgressList.FirstOrDefault(mp =>
                    mp.StudentId == fh.UserId && mp.MissionId == fh.MissionId);
                instructorName = progress?.Instructor != null
                    ? $"{progress.Instructor.Grade}. {progress.Instructor.LastName} {progress.Instructor.Name}"
                    : "N/A";
            }
            else if (fh.NonEvaluableMission != null)
            {
                // Misión no evaluable
                missionPhaseName = $"MNE{fh.NonEvaluableMission.NonEvaluableMissionNumber} / {fh.NonEvaluableMission.Phase.Name}";
                instructorName = $"{fh.NonEvaluableMission.Instructor.Grade}. {fh.NonEvaluableMission.Instructor.LastName} {fh.NonEvaluableMission.Instructor.Name}";
            }
            else
            {
                missionPhaseName = "N/A";
                instructorName = "N/A";
            }

            return new MachineFlightDetailRecordDto
            {
                Number = index + 1,
                StudentName = $"{fh.User.Grade}. {fh.User.LastName} {fh.User.Name}",
                MissionPhaseName = missionPhaseName,
                FlightDate = fh.Date,
                InstructorName = instructorName,
                FlightHours = fh.MachineFlightHours
            };
        }).ToList();

        var totalHours = records.Sum(r => r.FlightHours);

        // Obtener nombre de fase si aplica
        string? phaseName = null;
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            var phase = await _unitOfWork.Repository<Phase>().GetFirstAsync(p => p.Id == phaseId.Value);
            phaseName = phase?.Name;
        }

        var result = new MachineFlightHoursDetailDto
        {
            ReportType = ReportDetailConstants.MachineHours,
            SelectedValue = aircraftRegistration,
            Title = $"Horas de Vuelo por Aeronave: {aircraftRegistration}",
            Subtitle = $"Total de vuelos: {records.Count} | Total de horas: {totalHours:0.00}",
            CourseId = courseId,
            CourseName = course.CourseName,
            Force = forceEntity?.Name ?? force,
            WingType = wingType,
            PhaseId = phaseId,
            PhaseName = phaseName,
            Records = records,
            TotalHours = totalHours
        };

        return Result<MachineFlightHoursDetailDto>.Success(result);
    }

    public async Task<Result<InstructorFlightHoursDetailDto>> GetInstructorFlightHoursDetailAsync(Guid courseId, string instructorName, string? force, string? wingType, Guid? phaseId)
    {
        var (course, forceEntity, userCourses, studentIds) = await GetReportInitialDataDetailAsync(courseId, force ?? ALL_OPTION, wingType ?? ALL_OPTION);

        force = forceEntity?.Code ?? force;

        // Consultar horas de vuelo del instructor (incluye misiones evaluables y no evaluables)
        var flightHours = await _unitOfWork.Repository<FlightHourLog>()
            .GetListAsync(
                fhl => fhl.CourseId == courseId && studentIds.Contains(fhl.UserId),
                includeFunc: q => q
                    .Include(fhl => fhl.User)
                    .Include(fhl => fhl.Aircraft)
                    .Include(fhl => fhl.Mission)
                        .ThenInclude(m => m!.Phase)
                    .Include(fhl => fhl.NonEvaluableMission)
                        .ThenInclude(nem => nem!.Phase)
                    .Include(fhl => fhl.NonEvaluableMission)
                        .ThenInclude(nem => nem!.Instructor)
            );

        // Obtener StudentMissionProgress para misiones evaluables
        var missionProgressList = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetListAsync(
                smp => smp.CourseId == courseId && studentIds.Contains(smp.StudentId),
                includeFunc: q => q.Include(smp => smp.Instructor)
            );

        // Filtrar por instructor (evaluables y no evaluables)
        flightHours = [.. flightHours.Where(fh =>
        {
            // Verificar si es misión evaluable con el instructor buscado
            if (fh.Mission != null)
            {
                var progress = missionProgressList.FirstOrDefault(mp =>
                    mp.StudentId == fh.UserId && mp.MissionId == fh.MissionId);
                if (progress?.Instructor != null)
                {
                    var instructorFullName = $"{progress.Instructor.Grade}. {progress.Instructor.LastName} {progress.Instructor.Name}";
                    if (instructorFullName.Contains(instructorName, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            // Verificar si es misión no evaluable con el instructor buscado
            else if (fh.NonEvaluableMission?.Instructor != null)
            {
                var instructorFullName = $"{fh.NonEvaluableMission.Instructor.Grade}. {fh.NonEvaluableMission.Instructor.LastName} {fh.NonEvaluableMission.Instructor.Name}";
                if (instructorFullName.Contains(instructorName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        })];

        // Filtrar por fase si aplica
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            flightHours = [.. flightHours.Where(fh =>
                (fh.Mission != null && fh.Mission.PhaseId == phaseId.Value) ||
                (fh.NonEvaluableMission != null && fh.NonEvaluableMission.PhaseId == phaseId.Value))];
        }

        // Ordenar por fecha descendente
        flightHours = [.. flightHours.OrderByDescending(fh => fh.Date)];

        // Mapear a DTOs
        var records = flightHours.Select((fh, index) =>
        {
            string missionPhaseName;

            if (fh.Mission != null)
            {
                // Misión evaluable
                missionPhaseName = $"{fh.Mission.Name} / {fh.Mission.Phase.Name}";
            }
            else if (fh.NonEvaluableMission != null)
            {
                // Misión no evaluable
                missionPhaseName = $"MNE{fh.NonEvaluableMission.NonEvaluableMissionNumber} / {fh.NonEvaluableMission.Phase.Name}";
            }
            else
            {
                missionPhaseName = "N/A";
            }

            return new InstructorFlightDetailRecordDto
            {
                Number = index + 1,
                StudentName = $"{fh.User.Grade}. {fh.User.LastName} {fh.User.Name}",
                MissionPhaseName = missionPhaseName,
                AircraftRegistration = fh.Aircraft?.Registration ?? "N/A",
                FlightHours = fh.ManFlightHours,
                FlightDate = fh.Date
            };
        }).ToList();

        var totalHours = records.Sum(r => r.FlightHours);

        // Obtener nombre de fase si aplica
        string? phaseName = null;
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            var phase = await _unitOfWork.Repository<Phase>().GetFirstAsync(p => p.Id == phaseId.Value);
            phaseName = phase?.Name;
        }

        var result = new InstructorFlightHoursDetailDto
        {
            ReportType = ReportDetailConstants.InstructorHours,
            SelectedValue = instructorName,
            Title = $"Horas de Vuelo por Instructor: {instructorName}",
            Subtitle = $"Total de vuelos: {records.Count} | Total de horas: {totalHours:0.00}",
            CourseId = courseId,
            CourseName = course.CourseName,
            Force = forceEntity?.Name ?? force,
            WingType = wingType,
            PhaseId = phaseId,
            PhaseName = phaseName,
            Records = records,
            TotalHours = totalHours
        };

        return Result<InstructorFlightHoursDetailDto>.Success(result);
    }

    public async Task<Result<MachineUnsatisfactoryDetailDto>> GetMachineUnsatisfactoryDetailAsync(Guid courseId, string aircraftRegistration, string? force, string? wingType, Guid? phaseId)
    {
        var (course, forceEntity, userCourses, studentIds) = await GetReportInitialDataDetailAsync(courseId, force ?? ALL_OPTION, wingType ?? ALL_OPTION);

        force = forceEntity?.Code ?? force;

        // Consultar misiones insatisfactorias en la aeronave específica
        var missionProgresses = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetListAsync(
                smp => smp.CourseId == courseId
                    && studentIds.Contains(smp.StudentId)
                    && !smp.MissionPassed
                    && smp.Aircraft.Registration == aircraftRegistration,
                includeFunc: q => q
                    .Include(smp => smp.Student)
                    .Include(smp => smp.Mission)
                        .ThenInclude(m => m.Phase)
                    .Include(smp => smp.Aircraft)
                    .Include(smp => smp.Instructor)
            );

        // Filtrar por fase si aplica
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            missionProgresses = [.. missionProgresses.Where(mp => mp.Mission.PhaseId == phaseId.Value)];
        }

        // Ordenar por fecha descendente
        missionProgresses = [.. missionProgresses.OrderByDescending(mp => mp.Date)];

        // Mapear a DTOs
        var records = missionProgresses.Select((mp, index) => new UnsatisfactoryDetailRecordDto
        {
            Number = index + 1,
            StudentName = $"{mp.Student.Grade}. {mp.Student.LastName} {mp.Student.Name}",
            MissionPhaseName = $"{mp.Mission.Name} / {mp.Mission.Phase.Name}",
            EvaluationDate = mp.Date,
            InstructorOrAircraft = mp.Instructor != null
                ? $"{mp.Instructor.Grade}. {mp.Instructor.LastName} {mp.Instructor.Name}"
                : "N/A",
            MissionId = mp.MissionId,
            StudentId = mp.StudentId,
            CourseId = mp.CourseId
        }).ToList();

        // Obtener nombre de fase si aplica
        string? phaseName = null;
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            var phase = await _unitOfWork.Repository<Phase>().GetFirstAsync(p => p.Id == phaseId.Value);
            phaseName = phase?.Name;
        }

        var result = new MachineUnsatisfactoryDetailDto
        {
            ReportType = ReportDetailConstants.MachineUnsatisfactory,
            SelectedValue = aircraftRegistration,
            Title = $"Misiones Insatisfactorias por Aeronave: {aircraftRegistration}",
            Subtitle = $"Total de misiones: {records.Count}",
            CourseId = courseId,
            CourseName = course.CourseName,
            Force = forceEntity?.Name ?? force,
            WingType = wingType,
            PhaseId = phaseId,
            PhaseName = phaseName,
            Records = records
        };

        return Result<MachineUnsatisfactoryDetailDto>.Success(result);
    }

    public async Task<Result<InstructorUnsatisfactoryDetailDto>> GetInstructorUnsatisfactoryDetailAsync(Guid courseId, string instructorName, string? force, string? wingType, Guid? phaseId)
    {
        var (course, forceEntity, userCourses, studentIds) = await GetReportInitialDataDetailAsync(courseId, force ?? ALL_OPTION, wingType ?? ALL_OPTION);

        force = forceEntity?.Code ?? force;

        // Consultar misiones insatisfactorias con el instructor específico
        var missionProgresses = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetListAsync(
                smp => smp.CourseId == courseId
                    && studentIds.Contains(smp.StudentId)
                    && !smp.MissionPassed,
                includeFunc: q => q
                    .Include(smp => smp.Student)
                    .Include(smp => smp.Mission)
                        .ThenInclude(m => m.Phase)
                    .Include(smp => smp.Aircraft)
                    .Include(smp => smp.Instructor)
            );

        // Filtrar por nombre de instructor (coincidencia aproximada)
        missionProgresses = [.. missionProgresses.Where(mp =>
                mp.Instructor != null &&
                $"{mp.Instructor.Grade}. {mp.Instructor.LastName} {mp.Instructor.Name}".Contains(instructorName, StringComparison.OrdinalIgnoreCase))];

        // Filtrar por fase si aplica
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            missionProgresses = [.. missionProgresses.Where(mp => mp.Mission.PhaseId == phaseId.Value)];
        }

        // Ordenar por fecha descendente
        missionProgresses = [.. missionProgresses.OrderByDescending(mp => mp.Date)];

        // Mapear a DTOs
        var records = missionProgresses.Select((mp, index) => new UnsatisfactoryDetailRecordDto
        {
            Number = index + 1,
            StudentName = $"{mp.Student.Grade}. {mp.Student.LastName} {mp.Student.Name}",
            MissionPhaseName = $"{mp.Mission.Name} / {mp.Mission.Phase.Name}",
            EvaluationDate = mp.Date,
            InstructorOrAircraft = mp.Aircraft?.Registration ?? "N/A",
            MissionId = mp.MissionId,
            StudentId = mp.StudentId,
            CourseId = mp.CourseId
        }).ToList();

        // Obtener nombre de fase si aplica
        string? phaseName = null;
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            var phase = await _unitOfWork.Repository<Phase>().GetFirstAsync(p => p.Id == phaseId.Value);
            phaseName = phase?.Name;
        }

        var result = new InstructorUnsatisfactoryDetailDto
        {
            ReportType = ReportDetailConstants.InstructorUnsatisfactory,
            SelectedValue = instructorName,
            Title = $"Misiones Insatisfactorias por Instructor: {instructorName}",
            Subtitle = $"Total de misiones: {records.Count}",
            CourseId = courseId,
            CourseName = course.CourseName,
            Force = forceEntity?.Name ?? force,
            WingType = wingType,
            PhaseId = phaseId,
            PhaseName = phaseName,
            Records = records
        };

        return Result<InstructorUnsatisfactoryDetailDto>.Success(result);
    }

    public async Task<Result<NRedCategoriesDetailDto>> GetNRedCategoriesDetailAsync(Guid courseId, string category, string? force, string? wingType, Guid? phaseId)
    {
        var (course, forceEntity, userCourses, studentIds) = await GetReportInitialDataDetailAsync(courseId, force ?? ALL_OPTION, wingType ?? ALL_OPTION);

        force = forceEntity?.Code ?? force;

        // Consultar evaluaciones de tareas con N ROJA en la categoría específica
        var taskGrades = await _unitOfWork.Repository<StudentTaskGrade>()
            .GetListAsync(
                stg => studentIds.Contains(stg.StudentId)
                    && stg.Grade == GradeConstants.GradeTypes.NR,
                includeFunc: q => q
                    .Include(stg => stg.Student)
                    .Include(stg => stg.Task)
                    .Include(stg => stg.StudentMissionProgress)
                        .ThenInclude(smp => smp.Mission)
                        .ThenInclude(m => m.Phase)
                    .Include(stg => stg.StudentMissionProgress)
                        .ThenInclude(smp => smp.Aircraft)
                    .Include(stg => stg.StudentMissionProgress)
                        .ThenInclude(smp => smp.Instructor)
                    .Include(stg => stg.StudentGradeNRedReasons)
            );

        // Filtrar por categoría
        taskGrades = [.. taskGrades.Where(tg =>
                tg.StudentGradeNRedReasons.Any(sgnr =>
                    sgnr.ReasonCategory.Equals(category, StringComparison.OrdinalIgnoreCase)))];

        // Filtrar por fase si aplica
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            taskGrades = [.. taskGrades.Where(tg => tg.StudentMissionProgress.Mission.PhaseId == phaseId.Value)];
        }

        // Ordenar por fecha descendente
        taskGrades = [.. taskGrades.OrderByDescending(tg => tg.StudentMissionProgress.Date)];

        // Mapear a DTOs
        var records = taskGrades.Select((tg, index) => new NRedDetailRecordDto
        {
            Number = index + 1,
            StudentName = $"{tg.Student.Grade}. {tg.Student.LastName} {tg.Student.Name}",
            TaskName = tg.Task.Name,
            MissionPhaseName = $"{tg.StudentMissionProgress.Mission.Name} / {tg.StudentMissionProgress.Mission.Phase.Name}",
            AircraftRegistration = tg.StudentMissionProgress.Aircraft?.Registration ?? "N/A",
            InstructorName = tg.StudentMissionProgress.Instructor != null
                ? $"{tg.StudentMissionProgress.Instructor.Grade}. {tg.StudentMissionProgress.Instructor.LastName} {tg.StudentMissionProgress.Instructor.Name}"
                : "N/A",
            EvaluationDate = tg.StudentMissionProgress.Date,
            MissionId = tg.StudentMissionProgress.MissionId,
            StudentId = tg.StudentId,
            CourseId = courseId
        }).ToList();

        // Obtener nombre de fase si aplica
        string? phaseName = null;
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            var phase = await _unitOfWork.Repository<Phase>().GetFirstAsync(p => p.Id == phaseId.Value);
            phaseName = phase?.Name;
        }

        var result = new NRedCategoriesDetailDto
        {
            ReportType = ReportDetailConstants.NRedCategories,
            SelectedValue = category,
            Title = $"N ROJA por Categorías: {category}",
            Subtitle = $"Total de registros: {records.Count}",
            CourseId = courseId,
            CourseName = course.CourseName,
            Force = forceEntity?.Name ?? force,
            WingType = wingType,
            PhaseId = phaseId,
            PhaseName = phaseName,
            Records = records
        };

        return Result<NRedCategoriesDetailDto>.Success(result);
    }

    #endregion

}