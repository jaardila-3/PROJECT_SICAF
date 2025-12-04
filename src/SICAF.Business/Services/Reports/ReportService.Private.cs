using Microsoft.EntityFrameworkCore;

using SICAF.Business.Mappers.Identity;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Academic;
using SICAF.Common.DTOs.Instructor;
using SICAF.Common.DTOs.Reports;
using SICAF.Data.Entities.Academic;
using SICAF.Data.Entities.Catalogs;
using SICAF.Data.Entities.Identity;

namespace SICAF.Business.Services.Reports;

public partial class ReportService
{
    /// <summary>
    /// Obtiene el nombre legible del tipo de ala
    /// </summary>
    private static string GetWingTypeName(string wingType)
    {
        return wingType switch
        {
            AviationConstants.WingTypes.FIXED_WING => "Ala Fija",
            AviationConstants.WingTypes.ROTARY_WING => "Ala Rotatoria",
            _ => wingType
        };
    }

    /// <summary>
    /// Obtiene el nombre completo del estudiante para mostrar
    /// </summary>
    private static string GetStudentDisplayName(User user)
    {
        var grade = !string.IsNullOrWhiteSpace(user.Grade) ? $"{user.Grade}. " : "";
        return $"{grade}{user.LastName} {user.Name}";
    }

    private async Task<IReadOnlyList<UserCourse>> GetUserCoursesAsync(Guid courseId, string force = ALL_OPTION, string wingType = ALL_OPTION)
    {
        var userCourses = await _unitOfWork.Repository<UserCourse>()
            .GetListAsync(uc => uc.CourseId == courseId, includeFunc: q => q.Include(uc => uc.User).ThenInclude(u => u.AviationProfile));

        if (force != ALL_OPTION)
            userCourses = [.. userCourses.Where(uc => uc.User.Force == force)];

        if (wingType != ALL_OPTION)
            userCourses = [.. userCourses.Where(uc => uc.WingType == wingType)];

        return userCourses;
    }

    /// <summary>
    /// Obtiene la lista de instructores del programa
    /// </summary>
    private static List<InstructorTableDto> GetInstructorsAndLeadersTableAsync(IReadOnlyList<UserCourse> userCourses)
    {
        // Obtener instructores y líderes de vuelo
        var instructorsAndLeaders = userCourses
            .Where(uc => uc.ParticipationType == SystemRoles.INSTRUCTOR || uc.ParticipationType == ParticipationTypes.FLIGHT_LEADER)
            .ToList();

        // Ordenar por grado, antigüedad, apellido y nombre
        var sortedList = instructorsAndLeaders
            .OrderBy(uc => UserMapperExtensions.GetGradeOrder(uc.User.Grade))
            .ThenBy(uc => uc.User.SeniorityOrder ?? int.MaxValue)
            .ThenBy(uc => uc.User.LastName)
            .ThenBy(uc => uc.User.Name)
            .ToList();

        var result = new List<InstructorTableDto>();
        int number = 1;

        foreach (var uc in sortedList)
        {
            result.Add(new InstructorTableDto
            {
                Id = uc.UserId,
                Number = number++,
                Grade = uc.User.Grade ?? "",
                FullName = $"{uc.User.LastName} {uc.User.Name}",
                PID = uc.User.AviationProfile?.PID ?? "N/A",
                WingType = uc.WingType ?? "",
                Role = uc.ParticipationType
            });
        }

        return result;
    }

    private async Task<(List<StudentReportDto> students, int totalStudents, int activeStudents, int suspendedStudents)>
        GetStudentStatisticsAsync(IReadOnlyList<UserCourse> userCourses, Guid courseId, Guid? phaseId = null)
    {
        userCourses = [.. userCourses.Where(uc => uc.ParticipationType == SystemRoles.STUDENT)];

        var studentIds = userCourses.Select(uc => uc.UserId).ToList();

        var phaseProgresses = await _unitOfWork.Repository<StudentPhaseProgress>()
            .GetListAsync(
                spp => studentIds.Contains(spp.StudentId) && spp.CourseId == courseId,
                includeFunc: q => q.Include(spp => spp.Phase)
            );

        // Si hay filtro por fase, filtrar estudiantes que están o pasaron por esa fase
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            var filteredStudentIds = phaseProgresses
                .Where(spp => spp.PhaseId == phaseId.Value)
                .Select(spp => spp.StudentId)
                .Distinct()
                .ToList();

            userCourses = [.. userCourses.Where(uc => filteredStudentIds.Contains(uc.UserId))];
            studentIds = filteredStudentIds;
        }

        var totalStudents = userCourses.Count;

        var suspendedStudentIds = phaseProgresses
            .Where(spp => spp.IsSuspended)
            .Select(spp => spp.StudentId)
            .Distinct()
            .ToList();

        var suspendedStudents = suspendedStudentIds.Count;
        var activeStudents = totalStudents - suspendedStudents;

        // Obtener misiones con Include de Mission para poder filtrar por fase
        var missionProgresses = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetListAsync(
                smp => studentIds.Contains(smp.StudentId) && smp.CourseId == courseId,
                includeFunc: q => q.Include(smp => smp.Mission)
            );

        // Si hay filtro por fase, filtrar solo misiones de esa fase
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            missionProgresses = [.. missionProgresses.Where(mp => mp.Mission.PhaseId == phaseId.Value)];
        }

        var missionIds = missionProgresses.Select(mp => mp.MissionId).Distinct().ToList();

        // Obtener horas de vuelo (filtradas por misiones de la fase si aplica)
        var flightHours = phaseId.HasValue && phaseId.Value != Guid.Empty
            ? await _unitOfWork.Repository<FlightHourLog>()
                .GetListAsync(fhl =>
                    studentIds.Contains(fhl.UserId) &&
                    fhl.CourseId == courseId &&
                    fhl.MissionId.HasValue &&
                    missionIds.Contains(fhl.MissionId.Value)
                )
            : await _unitOfWork.Repository<FlightHourLog>()
                .GetListAsync(fhl =>
                    studentIds.Contains(fhl.UserId) &&
                    fhl.CourseId == courseId
                // Incluye todas las horas (evaluables y no evaluables)
                );

        // Obtener registros de misiones no evaluables (filtradas por fase si aplica)
        var nonEvaluableRecords = phaseId.HasValue && phaseId.Value != Guid.Empty
            ? await _unitOfWork.Repository<NonEvaluableMissionRecord>()
                .GetListAsync(ner =>
                    studentIds.Contains(ner.StudentId) &&
                    ner.CourseId == courseId &&
                    ner.PhaseId == phaseId.Value
                )
            : await _unitOfWork.Repository<NonEvaluableMissionRecord>()
                .GetListAsync(ner =>
                    studentIds.Contains(ner.StudentId) &&
                    ner.CourseId == courseId
                );

        var students = new List<StudentReportDto>();
        int number = 1;

        foreach (var uc in userCourses.OrderBy(uc => UserMapperExtensions.GetGradeOrder(uc.User.Grade))
                                       .ThenBy(uc => uc.User.SeniorityOrder ?? int.MaxValue)
                                       .ThenBy(uc => uc.User.LastName)
                                       .ThenBy(uc => uc.User.Name))
        {
            var currentPhase = phaseProgresses.FirstOrDefault(pp => pp.StudentId == uc.UserId && pp.IsCurrentPhase);
            var studentMissions = missionProgresses.Where(mp => mp.StudentId == uc.UserId).ToList();
            var studentFlightHours = flightHours.Where(fh => fh.UserId == uc.UserId).Sum(fh => fh.ManFlightHours);
            var studentNonEvaluableRecords = nonEvaluableRecords.Count(ner => ner.StudentId == uc.UserId);

            currentPhase ??= phaseProgresses.FirstOrDefault(pp => pp.Status == UserConstants.StudentStatus.CourseCompleted && pp.StudentId == uc.UserId);

            var completedMissions = currentPhase?.CompletedMissions ?? 0;
            var totalPhaseMissions = currentPhase?.Phase.TotalMissions ?? 1;
            var progressPercentage = totalPhaseMissions > 0 ? (double)completedMissions / totalPhaseMissions * 100 : 0;

            // Obtener PhaseStatus si se está filtrando por fase
            string? phaseStatus = null;
            if (phaseId.HasValue && phaseId.Value != Guid.Empty)
            {
                var specificPhaseProgress = phaseProgresses.FirstOrDefault(pp => pp.StudentId == uc.UserId && pp.PhaseId == phaseId.Value);
                phaseStatus = specificPhaseProgress?.Status;
            }

            // obtener promedio
            var taskGrades = await _unitOfWork.Repository<StudentTaskGrade>()
                .GetListAsync(stg => stg.StudentId == uc.UserId && missionIds.Contains(stg.MissionId));

            var gradeDistribution = new GradeDistributionDto
            {
                GradeA = taskGrades.Count(g => g.Grade == GradeConstants.GradeTypes.A),
                GradeB = taskGrades.Count(g => g.Grade == GradeConstants.GradeTypes.B),
                GradeC = taskGrades.Count(g => g.Grade == GradeConstants.GradeTypes.C),
                GradeN = taskGrades.Count(g => g.Grade == GradeConstants.GradeTypes.N),
                GradeNR = taskGrades.Count(g => g.Grade == GradeConstants.GradeTypes.NR),
                TotalGrades = taskGrades.Count(g => g.Grade != GradeConstants.GradeTypes.DM && g.Grade != GradeConstants.GradeTypes.SC)
            };

            var average = CalculateAverageGrade(gradeDistribution);

            // crear el reporte de estudiantes
            students.Add(new StudentReportDto
            {
                Number = number++,
                GradeAndName = GetStudentDisplayName(uc.User),
                Identification = $"{uc.User.DocumentType} {uc.User.IdentificationNumber}",
                PID = uc.User.AviationProfile?.PID ?? "N/A",
                CurrentPhase = currentPhase?.Phase.Name ?? "Sin fase asignada",
                CompletedMissions = completedMissions,
                NonEvaluableMissionRecords = studentNonEvaluableRecords,
                ProgressPercentage = Math.Round((double)progressPercentage, 2),
                Average = average,
                SatisfactoryMissions = studentMissions.Count(m => m.MissionPassed),
                UnsatisfactoryMissions = studentMissions.Count(m => !m.MissionPassed),
                TotalFlightHours = Math.Round(studentFlightHours, 1),
                PhaseStatus = phaseStatus
            });
        }

        return (students, totalStudents, activeStudents, suspendedStudents);
    }

    private async Task<GradeDistributionDto> GetGradeDistributionAsync(List<Guid> studentIds, Guid? phaseId = null)
    {
        var grades = await _unitOfWork.Repository<StudentTaskGrade>()
            .GetListAsync(
                stg => studentIds.Contains(stg.StudentId),
                includeFunc: q => q.Include(stg => stg.StudentMissionProgress).ThenInclude(smp => smp.Mission)
            );

        // Si hay filtro por fase, filtrar solo calificaciones de misiones de esa fase
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            grades = [.. grades.Where(g => g.StudentMissionProgress.Mission.PhaseId == phaseId.Value)];
        }

        return new GradeDistributionDto
        {
            GradeA = grades.Count(g => g.Grade == GradeConstants.GradeTypes.A),
            GradeB = grades.Count(g => g.Grade == GradeConstants.GradeTypes.B),
            GradeC = grades.Count(g => g.Grade == GradeConstants.GradeTypes.C),
            GradeN = grades.Count(g => g.Grade == GradeConstants.GradeTypes.N),
            GradeNR = grades.Count(g => g.Grade == GradeConstants.GradeTypes.NR)
        };
    }

    private async Task<List<MachineFlightHoursDto>> GetMachineFlightHoursAsync(List<Guid> studentIds, Guid courseId, Guid? phaseId = null)
    {
        // Incluye TODAS las horas (evaluables y no evaluables)
        var flightHours = await _unitOfWork.Repository<FlightHourLog>()
            .GetListAsync(
                fhl => fhl.CourseId == courseId && studentIds.Contains(fhl.UserId),
                includeFunc: q => q.Include(fhl => fhl.Aircraft).Include(fhl => fhl.Mission)
            );

        // Si hay filtro por fase, filtrar solo horas de misiones de esa fase
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            flightHours = [.. flightHours.Where(fh => fh.Mission != null && fh.Mission.PhaseId == phaseId.Value)];
        }

        return flightHours
            .GroupBy(fh => new { fh.Aircraft.Registration, fh.Aircraft.AircraftType })
            .Select(g => new MachineFlightHoursDto
            {
                AircraftRegistration = g.Key.Registration,
                AircraftType = g.Key.AircraftType,
                TotalHours = g.Sum(fh => fh.MachineFlightHours)
            })
            .OrderByDescending(m => m.TotalHours)
            .ToList();
    }

    private async Task<List<MachineUnsatisfactoryMissionsDto>> GetMachineUnsatisfactoryMissionsAsync(List<Guid> studentIds, Guid courseId, Guid? phaseId = null)
    {
        var missionProgresses = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetListAsync(
                smp => smp.CourseId == courseId && studentIds.Contains(smp.StudentId) && !smp.MissionPassed,
                includeFunc: q => q.Include(smp => smp.Aircraft).Include(smp => smp.Mission)
            );

        // Si hay filtro por fase, filtrar solo misiones de esa fase
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            missionProgresses = [.. missionProgresses.Where(mp => mp.Mission.PhaseId == phaseId.Value)];
        }

        return missionProgresses
            .GroupBy(mp => mp.Aircraft.Registration)
            .Select(g => new MachineUnsatisfactoryMissionsDto
            {
                AircraftRegistration = g.Key,
                UnsatisfactoryCount = g.Count()
            })
            .OrderByDescending(m => m.UnsatisfactoryCount)
            .ToList();
    }

    private async Task<List<InstructorFlightHoursDto>> GetInstructorFlightHoursAsync(Guid courseId, Guid? phaseId = null, string wingType = ALL_OPTION)
    {
        // Incluye TODAS las horas (evaluables y no evaluables)
        var flightHours = await _unitOfWork.Repository<FlightHourLog>()
            .GetListAsync(
                fhl => fhl.CourseId == courseId && fhl.Role == SystemRoles.INSTRUCTOR,
                includeFunc: q => q.Include(fhl => fhl.User).Include(fhl => fhl.Mission)
            );

        // Si hay filtro por fase, filtrar solo horas de misiones de esa fase
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            flightHours = [.. flightHours.Where(fh => fh.Mission != null && fh.Mission.PhaseId == phaseId.Value)];
        }

        // Si hay filtro por wingType, filtrar solo horas de misiones de ese wingType
        if (wingType != ALL_OPTION)
        {
            flightHours = [.. flightHours.Where(fh => fh.Mission != null && fh.Mission.WingType == wingType)];
        }

        return flightHours
            .GroupBy(fh => new { fh.UserId, fh.User.Grade, fh.User.LastName, fh.User.Name })
            .Select(g => new InstructorFlightHoursDto
            {
                InstructorName = $"{g.Key.Grade}. {g.Key.LastName} {g.Key.Name}",
                TotalHours = Math.Round(g.Sum(fh => fh.ManFlightHours), 1)
            })
            .OrderByDescending(i => i.TotalHours)
            .ToList();
    }

    private async Task<List<InstructorUnsatisfactoryMissionsDto>> GetInstructorUnsatisfactoryMissionsAsync(List<Guid> studentIds, Guid courseId, Guid? phaseId = null)
    {
        var missionProgresses = await _unitOfWork.Repository<StudentMissionProgress>()
            .GetListAsync(
                smp => smp.CourseId == courseId && studentIds.Contains(smp.StudentId) && !smp.MissionPassed,
                includeFunc: q => q.Include(smp => smp.Instructor).Include(smp => smp.Mission)
            );

        // Si hay filtro por fase, filtrar solo misiones de esa fase
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            missionProgresses = [.. missionProgresses.Where(mp => mp.Mission.PhaseId == phaseId.Value)];
        }

        return missionProgresses
            .GroupBy(mp => new { mp.InstructorId, mp.Instructor.Grade, mp.Instructor.LastName, mp.Instructor.Name })
            .Select(g => new InstructorUnsatisfactoryMissionsDto
            {
                InstructorName = $"{g.Key.Grade}. {g.Key.LastName} {g.Key.Name}",
                UnsatisfactoryCount = g.Count()
            })
            .OrderByDescending(i => i.UnsatisfactoryCount)
            .ToList();
    }

    private async Task<NRedReasonsDto> GetNRedReasonsAsync(List<Guid> studentIds, Guid? phaseId = null)
    {
        var grades = await _unitOfWork.Repository<StudentTaskGrade>()
            .GetListAsync(
                stg => studentIds.Contains(stg.StudentId) && stg.Grade == GradeConstants.GradeTypes.NR,
                includeFunc: q => q.Include(stg => stg.StudentMissionProgress).ThenInclude(smp => smp.Mission)
                                   .Include(stg => stg.StudentGradeNRedReasons)
            );

        // Si hay filtro por fase, filtrar solo calificaciones de misiones de esa fase
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            grades = [.. grades.Where(g => g.StudentMissionProgress.Mission.PhaseId == phaseId.Value)];
        }

        var allReasons = grades.SelectMany(g => g.StudentGradeNRedReasons).ToList();

        var categories = allReasons
            .GroupBy(r => r.ReasonCategory)
            .Select(g => new CategoryCountDto
            {
                Category = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(c => c.Count)
            .ToList();

        var reasons = allReasons
            .GroupBy(r => new { r.ReasonCategory, r.ReasonDescription })
            .Select(g => new ReasonDetailDto
            {
                Category = g.Key.ReasonCategory,
                Reason = g.Key.ReasonDescription,
                Count = g.Count()
            })
            .OrderByDescending(r => r.Count)
            .ToList();

        return new NRedReasonsDto
        {
            Categories = categories,
            Reasons = reasons
        };
    }

    private static List<Guid> GetFilteredStudentIdsAsync(IReadOnlyList<UserCourse> userCourses)
    {
        userCourses = [.. userCourses.Where(uc => uc.ParticipationType == SystemRoles.STUDENT)];
        return [.. userCourses.Select(uc => uc.UserId)];
    }

    private static double CalculateAverageGrade(GradeDistributionDto distribution)
    {
        if (distribution.TotalGrades == 0)
            return 0;

        // Equivalencias numéricas según TE Murillo:
        // A: 5.0
        // B: 4.0
        // C: 3.5
        // N: 3.0
        // N-Roja: 2.0

        double sumGrades = (distribution.GradeA * 5.0d) +
                            (distribution.GradeB * 4.0d) +
                            (distribution.GradeC * 3.5d) +
                            (distribution.GradeN * 3.0d) +
                            (distribution.GradeNR * 2.0d);

        return Math.Round(sumGrades / distribution.TotalGrades, 2);
    }

    private async Task<List<UnsatisfactoryMissionDto>> GetStudentUnsatisfactoryMissionsAsync(Guid studentId, IReadOnlyList<StudentMissionProgress> missionProgresses)
    {
        var unsatisfactoryMissions = missionProgresses.Where(mp => !mp.MissionPassed).ToList();

        if (unsatisfactoryMissions.Count == 0)
            return [];

        var missionIds = unsatisfactoryMissions.Select(mp => mp.MissionId).ToList();

        // Obtener tareas con N Roja de esas misiones
        var taskGrades = await _unitOfWork.Repository<StudentTaskGrade>()
            .GetListAsync(
                stg => stg.StudentId == studentId && missionIds.Contains(stg.MissionId) && stg.Grade == GradeConstants.GradeTypes.NR,
                includeFunc: q => q.Include(stg => stg.Task)
            );

        var result = new List<UnsatisfactoryMissionDto>();

        foreach (var mission in unsatisfactoryMissions.OrderBy(m => m.Date))
        {
            var tasksWithNRed = taskGrades
                .Where(tg => tg.MissionId == mission.MissionId)
                .Select(tg => $"{tg.Task.Name}")
                .ToList();

            var instructor = await _unitOfWork.Repository<User>().GetFirstAsync(u => u.Id == mission.InstructorId);

            result.Add(new UnsatisfactoryMissionDto
            {
                Phase = mission.Mission.Phase.Name,
                Mission = mission.Mission.Name,
                TasksWithNRed = tasksWithNRed ?? [],
                Date = mission.Date,
                InstructorGradeAndName = instructor != null
                    ? $"{instructor.Grade}. {instructor.LastName} {instructor.Name}"
                    : "N/A",
                AircraftRegistration = mission.Aircraft.Registration,
                Observations = mission.Observations ?? ""
            });
        }

        return result;
    }

    private async Task<List<AcademicCommitteeDto>> GetStudentCommitteesAsync(Guid studentId, Guid courseId, Guid? phaseId = null)
    {
        var committees = await _unitOfWork.Repository<StudentCommitteeRecord>()
            .GetListAsync(
                scr => scr.StudentId == studentId && scr.CourseId == courseId,
                orderBy: q => q.OrderBy(scr => scr.Date)
            );

        // Si hay filtro por fase, filtrar solo comités de esa fase
        if (phaseId.HasValue && phaseId.Value != Guid.Empty)
        {
            committees = [.. committees.Where(c => c.PhaseId == phaseId.Value)];
        }

        return committees.Select(c => new AcademicCommitteeDto
        {
            ActNumber = c.ActaNumber ?? "Pendiente",
            Date = c.Date,
            Decision = c.Decision ?? "Pendiente",
            Observations = c.DecisionObservations ?? ""
        }).ToList();
    }

    private async Task<(Course course, MasterCatalog forceEntity, IReadOnlyList<UserCourse> userCourses, List<Guid> studentIds)> GetReportInitialDataDetailAsync(Guid courseId, string force, string wingType)
    {
        // Obtener información del curso
        var course = await _unitOfWork.Repository<Course>().GetFirstAsync(c => c.Id == courseId) ?? throw new Exception(SystemErrors.CourseError.NotFound.Message);
        // obtener informacion de la fuerza
        var forceEntity = await _unitOfWork.Repository<MasterCatalog>().GetFirstAsync(c => c.Code == force || c.Name == force);
        forceEntity ??= null!;
        // Obtener inscripciones filtradas
        var userCourses = await GetUserCoursesAsync(courseId, force, wingType);
        var studentIds = GetFilteredStudentIdsAsync(userCourses);
        return (course, forceEntity, userCourses, studentIds);
    }

    /// <summary>
    /// Construye el historial de fases con sus misiones completadas (evaluables y no evaluables)
    /// </summary>
    private async Task<List<PhaseWithMissionsDto>> BuildPhaseHistoryAsync(
        Guid studentId,
        Guid courseId,
        IReadOnlyList<StudentPhaseProgress> phaseProgresses,
        IReadOnlyList<StudentMissionProgress> allMissionProgresses,
        IReadOnlyList<StudentTaskGrade> allTaskGrades)
    {
        var phaseHistory = new List<PhaseWithMissionsDto>();

        // Obtener todas las misiones no evaluables del estudiante en este curso
        var allNonEvaluableMissions = await _unitOfWork.Repository<NonEvaluableMissionRecord>()
            .GetListAsync(
                ner => ner.StudentId == studentId && ner.CourseId == courseId,
                includeFunc: q => q.Include(ner => ner.Instructor)
            );

        // Obtener todos los instructores de misiones evaluables de una sola vez
        var instructorIds = allMissionProgresses.Select(mp => mp.InstructorId).Distinct().ToList();
        var instructors = await _unitOfWork.Repository<User>()
            .GetListAsync(u => instructorIds.Contains(u.Id));

        // Iterar sobre cada fase por la que ha pasado el estudiante
        foreach (var phaseProgress in phaseProgresses.OrderBy(pp => pp.Phase.PhaseNumber))
        {
            var phaseId = phaseProgress.PhaseId;

            // Filtrar misiones evaluables de esta fase
            var phaseMissions = allMissionProgresses
                .Where(mp => mp.Mission.PhaseId == phaseId)
                .OrderBy(mp => mp.Mission.MissionNumber)
                .ToList();

            // Mapear a DTOs de misiones completadas
            var completedMissions = phaseMissions.Select(mission =>
            {
                var instructor = instructors.FirstOrDefault(i => i.Id == mission.InstructorId);
                var taskGradesForMission = allTaskGrades.Where(tg => tg.MissionId == mission.MissionId).ToList();
                var criticalFailures = taskGradesForMission.Count(tg => tg.Grade == GradeConstants.GradeTypes.NR);

                return new MissionStatusDto
                {
                    MissionId = mission.MissionId,
                    MissionName = mission.Mission.Name,
                    GradeNameInstructor = instructor != null
                        ? $"{instructor.Grade}. {instructor.LastName} {instructor.Name}"
                        : "N/A",
                    MissionNumber = mission.Mission.MissionNumber,
                    FlightHours = mission.Mission.FlightHours,
                    IsCompleted = true,
                    CompletionDate = mission.Date,
                    MissionPassed = mission.MissionPassed,
                    CriticalFailures = criticalFailures,
                    EvaluatorInstructorId = mission.InstructorId,
                    EvaluationDate = mission.Date,
                    EditCount = 0,
                    CanEdit = false
                };
            }).ToList();

            // Filtrar misiones no evaluables de esta fase
            var phaseNonEvaluableMissions = allNonEvaluableMissions
                .Where(nem => nem.PhaseId == phaseId)
                .OrderBy(nem => nem.NonEvaluableMissionNumber)
                .Select(nem => new NonEvaluableMissionDto
                {
                    MissionId = nem.Id,
                    CourseId = nem.CourseId,
                    MissionNumber = nem.NonEvaluableMissionNumber,
                    Date = nem.Date,
                    ManFlightHours = nem.ManFlightHours,
                    Observations = nem.Observations,
                    InstructorId = nem.InstructorId,
                    CanEdit = false
                })
                .ToList();

            // Agregar la fase al historial
            phaseHistory.Add(new PhaseWithMissionsDto
            {
                PhaseId = phaseProgress.PhaseId,
                PhaseName = phaseProgress.Phase.Name,
                PhaseNumber = phaseProgress.Phase.PhaseNumber,
                StartDate = phaseProgress.StartDate,
                EndDate = phaseProgress.EndDate,
                PhaseStatus = phaseProgress.Status,
                CompletedMissions = completedMissions,
                NonEvaluableMissions = phaseNonEvaluableMissions,
                TotalMissionsInPhase = phaseProgress.Phase.TotalMissions
            });
        }

        return phaseHistory;
    }
}
