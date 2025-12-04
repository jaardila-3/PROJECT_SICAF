using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SICAF.Business.Interfaces.Academic;
using SICAF.Business.Mappers.Academic;
using SICAF.Business.Mappers.Identity;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Academic;
using SICAF.Common.DTOs.Identity;
using SICAF.Common.Models.Results;
using SICAF.Data.Entities.Academic;
using SICAF.Data.Entities.Identity;
using SICAF.Data.Interfaces.Repositories;

using static SICAF.Common.Constants.AviationConstants;

namespace SICAF.Business.Services.Academic;

/// <summary>
/// Servicio para la gestión de cursos académicos
/// </summary>
public class CourseService(
    IUnitOfWork unitOfWork,
    ILogger<CourseService> logger
) : ICourseService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CourseService> _logger = logger;

    #region Operaciones de programa

    public async Task<Result<IEnumerable<CourseDto>>> GetAllCoursesAsync()
    {
        var courses = await _unitOfWork.Repository<Course>()
            .GetListAsync(
                predicate: null,
                orderBy: q => q.OrderBy(c => c.CourseNumber),
                includeFunc: q => q
                    .Include(c => c.UserCourses)
                    .ThenInclude(sc => sc.User)
            );

        if (courses is null)
            return Result<IEnumerable<CourseDto>>.Failure(SystemErrors.CourseError.NotFound);

        var courseDtos = courses?.Select(c => c.MapToDto()).ToList() ?? [];
        return Result<IEnumerable<CourseDto>>.Success(courseDtos);
    }

    public async Task<Result<IEnumerable<CourseDto>>> GetActiveCoursesAsync()
    {
        var currentDate = DateTime.Now;
        var courses = await _unitOfWork.Repository<Course>()
            .GetListAsync(
                predicate: c => c.EndDate > currentDate,
                orderBy: q => q.OrderBy(c => c.CourseNumber),
                includeFunc: q => q.Include(c => c.UserCourses)
                .ThenInclude(sc => sc.User)
            );

        var courseDtos = courses?.Select(c => c.MapToDto()).ToList() ?? [];
        return Result<IEnumerable<CourseDto>>.Success(courseDtos);
    }

    public async Task<Result<CourseDto>> GetCourseByIdAsync(Guid courseId)
    {
        var course = await _unitOfWork.Repository<Course>()
            .GetFirstAsync(
                predicate: c => c.Id == courseId,
                includeFunc: q => q
                    .Include(c => c.UserCourses).ThenInclude(sc => sc.User).ThenInclude(u => u.AviationProfile)
            );

        return course is null ? Result<CourseDto>.Failure(SystemErrors.CourseError.NotFound) : Result<CourseDto>.Success(course.MapToDto());
    }

    public async Task<Result<CourseDto>> CreateCourseAsync(CreateCourseDto createCourseDto)
    {
        // Validar que el número no exista
        var numberAvailable = await IsCourseNumberAvailableAsync(createCourseDto.CourseNumber);
        if (!numberAvailable.Value)
            return Result<CourseDto>.Failure(SystemErrors.CourseError.NumberExists);

        // Validar fechas
        if (createCourseDto.EndDate <= createCourseDto.StartDate)
            return Result<CourseDto>.Failure(SystemErrors.CourseError.InvalidDateRange);

        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var course = createCourseDto.MapToEntity();
            await _unitOfWork.Repository<Course>().AddAsync(course);
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            return await GetCourseByIdAsync(course.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al crear programa");
            throw;
        }
    }

    public async Task<Result<bool>> UpdateEndDateAsync(Guid courseId, DateTime newEndDate)
    {
        var course = await _unitOfWork.Repository<Course>().GetByIdAsync(courseId);
        if (course == null)
            return Result<bool>.Failure(SystemErrors.CourseError.NotFound);

        // Validar que la nueva fecha sea posterior a la fecha de inicio
        if (newEndDate <= course.StartDate)
            return Result<bool>.Failure(SystemErrors.CourseError.InvalidDateRange);

        course.EndDate = newEndDate;
        _unitOfWork.Repository<Course>().Update(course);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<List<CourseDto>>> GetCoursesForUserAsync(Guid userId, string[] userRoles)
    {
        List<CourseDto> courses;

        // Administradores ven todos los cursos
        if (userRoles.Contains(SystemRoles.ACADEMIC_ADMIN))
        {
            var allCoursesResult = await GetAllCoursesAsync();
            if (!allCoursesResult.IsSuccess)
            {
                return Result<List<CourseDto>>.Failure(allCoursesResult.Error);
            }
            courses = (List<CourseDto>)allCoursesResult.Value;
        }
        else
        {
            // Para otros roles, obtener cursos específicos
            courses = await GetUserParticipationCoursesAsync(userId);
        }

        return Result<List<CourseDto>>.Success(courses);
    }

    public async Task<Result<bool>> ChangeStudentCourseAsync(ChangeCourseDto changeDto)
    {
        // Obtener inscripción actual
        var currentEnrollment = await _unitOfWork.Repository<UserCourse>().GetFirstAsync(sc => sc.UserId == changeDto.StudentId);

        if (currentEnrollment == null)
            return Result<bool>.Failure(SystemErrors.EnrollmentError.StudentNotEnrolled);

        // Validar que el nuevo programa exista y esté activo o que no haya finalizado
        var newCourse = await _unitOfWork.Repository<Course>().GetByIdAsync(changeDto.NewCourseId);
        if (newCourse == null)
            return Result<bool>.Failure(SystemErrors.CourseError.NotFound);

        if (newCourse.EndDate < DateTime.Now)
            return Result<bool>.Failure(SystemErrors.CourseError.NotActive);

        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Desactivar inscripción actual
            currentEnrollment.IsActive = false;
            currentEnrollment.UnassignmentDate = DateTime.Now;
            currentEnrollment.UnassignmentReason = changeDto.Reason;
            _unitOfWork.Repository<UserCourse>().Update(currentEnrollment);

            // Desactivar la fase actual
            var studentPhaseProgress = await _unitOfWork.Repository<StudentPhaseProgress>().GetFirstAsync(spp => spp.StudentId == changeDto.StudentId && spp.CourseId == currentEnrollment.CourseId && spp.IsCurrentPhase);
            if (studentPhaseProgress != null)
            {
                studentPhaseProgress.IsCurrentPhase = false;
                studentPhaseProgress.EndDate = DateTime.Now;
                studentPhaseProgress.Status = UserConstants.StudentStatus.ChangeCourse;
                studentPhaseProgress.IsSuspended = true;
                studentPhaseProgress.SuspensionDate = DateTime.Now;
                studentPhaseProgress.SuspensionReason = changeDto.Reason;
                _unitOfWork.Repository<StudentPhaseProgress>().Update(studentPhaseProgress);
            }

            // Crear nueva inscripción en el programa
            var newEnrollment = new UserCourse
            {
                UserId = changeDto.StudentId,
                CourseId = changeDto.NewCourseId,
                ParticipationType = currentEnrollment.ParticipationType,
                WingType = currentEnrollment.WingType,
                AssignmentDate = DateTime.Now,
                IsActive = true
            };
            await _unitOfWork.Repository<UserCourse>().AddAsync(newEnrollment);

            // Crear registro para la nueva fase
            // inscribir en la fase 1
            var phase2 = await _unitOfWork.Repository<Phase>().GetFirstAsync(p => p.PhaseNumber == 2 && p.WingType == currentEnrollment.WingType);
            if (phase2 == null)
                return Result<bool>.Failure(SystemErrors.PhaseError.PhaseNotFound);

            var newPhaseProgress = new StudentPhaseProgress
            {
                StudentId = changeDto.StudentId,
                CourseId = changeDto.NewCourseId,
                PhaseId = phase2.PrerequisitePhaseId!.Value,
                NextPhaseId = phase2.Id,
                IsCurrentPhase = true,
                StartDate = DateTime.Now
            };
            await _unitOfWork.Repository<StudentPhaseProgress>().AddAsync(newPhaseProgress);

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al cambiar estudiante de programa");
            throw;
        }
    }

    public async Task<Result<UserCourseDto>> GetStudentCurrentCourseAsync(Guid studentId)
    {
        var enrollment = await _unitOfWork.Repository<UserCourse>()
            .GetFirstAsync(
                predicate: sc => sc.UserId == studentId,
                includeFunc: q => q
                    .Include(sc => sc.Course)
                    .Include(sc => sc.User)
                        .ThenInclude(s => s.AviationProfile)
            );

        if (enrollment == null)
            return Result<UserCourseDto>.Failure(SystemErrors.EnrollmentError.StudentNotEnrolled);

        return Result<UserCourseDto>.Success(enrollment.MapToDto());
    }

    public async Task<Result<IEnumerable<UserCourseDto>>> GetStudentsAsync(List<UserCourseDto> students)
    {
        if (students == null || students.Count == 0)
            return Result<IEnumerable<UserCourseDto>>.Success([]);

        // Obtener IDs de estudiantes
        var studentIds = students.Select(e => e.UserId).ToList();
        Guid courseId = students[0].CourseId;

        // Consulta para fases actuales
        var currentPhases = await _unitOfWork.Repository<StudentPhaseProgress>()
            .GetListAsync(
                predicate: spp => studentIds.Contains(spp.StudentId) && spp.CourseId == courseId && spp.IsCurrentPhase,
                includeFunc: q => q.Include(spp => spp.Phase)
            );

        // Consulta para estudiantes con programa completado
        var courseCompleted = await _unitOfWork.Repository<StudentPhaseProgress>()
            .GetListAsync(
                predicate: spp => studentIds.Contains(spp.StudentId) && spp.CourseId == courseId && spp.Status == UserConstants.StudentStatus.CourseCompleted
            );

        // Identificar estudiantes con programa completado
        var studentsWithCourseCompleted = courseCompleted.Select(cc => cc.StudentId).ToHashSet();

        // Crear diccionario para búsqueda rápida
        var phasesByStudent = currentPhases.ToDictionary(
            cp => cp.StudentId,
            cp => cp
        );

        // Mapear estudiantes a DTOs con información de fase
        var studentDtos = students.Select(sc =>
        {
            if (phasesByStudent.TryGetValue(sc.UserId, out var currentPhase))
            {
                sc.CurrentPhaseId = currentPhase.PhaseId;
                sc.CurrentPhaseNumber = currentPhase.Phase.PhaseNumber;
                sc.CurrentPhaseName = currentPhase.Phase.Name;
                sc.Status = currentPhase.Status;
            }
            else if (studentsWithCourseCompleted.Contains(sc.UserId))
            {
                sc.CurrentPhaseId = Guid.Empty;
                sc.CurrentPhaseNumber = 999; // Número alto para ordenar al final
                sc.CurrentPhaseName = UserConstants.StudentStatus.CourseCompleted;
                sc.Status = UserConstants.StudentStatus.CourseCompleted;
            }

            return sc;
        }).ToList();

        // Agrupar por fase: SE OMITE POR SOLICITUD DE MURILLO
        var groupedByPhase = studentDtos
            .GroupBy(s => new { s.CurrentPhaseId, s.CurrentPhaseNumber, s.CurrentPhaseName })
            .OrderBy(g => g.Key.CurrentPhaseNumber)
            .Select(g => new StudentsByPhaseDto
            {
                PhaseId = g.Key.CurrentPhaseId,
                PhaseNumber = g.Key.CurrentPhaseNumber,
                PhaseName = g.Key.CurrentPhaseName,
                StudentCount = g.Count(),
                Students = g.OrderBy(s => s.User?.LastName).ToList()
            })
            .ToList();

        return Result<IEnumerable<UserCourseDto>>.Success(studentDtos);
    }


    public async Task<Result<bool>> IsCourseNumberAvailableAsync(int courseNumber)
    {
        var exists = await _unitOfWork.Repository<Course>().AnyAsync(c => c.CourseNumber == courseNumber);
        return Result<bool>.Success(!exists);
    }

    public async Task<Result<bool>> CanEnrollStudentAsync(Guid studentId)
    {
        // Verificar si el estudiante ya tiene un programa activo
        var hasActiveCourse = await _unitOfWork.Repository<UserCourse>().AnyAsync(sc => sc.UserId == studentId);

        return Result<bool>.Success(!hasActiveCourse);
    }

    public async Task<Result<List<UserDto>>> GetAvailableInstructorsByCourseAsync(Guid courseId)
    {
        // Obtener usuarios con roles de instructor que no estén inscritos en el programa
        var users = await _unitOfWork.Repository<User>().GetListAsync(
            u => u.UserRoles.Any(ur => ur.Role.Name == SystemRoles.INSTRUCTOR)
                && !u.UserCourses.Any(uc => uc.CourseId == courseId),
            includeFunc: q => q
                        .Include(u => u.UserRoles)
                            .ThenInclude(ur => ur.Role)
                        .Include(u => u.AviationProfile)
        );

        var instructors = users.Select(u => u.MapToDto()).ToList();

        return Result<List<UserDto>>.Success(instructors);

    }

    public async Task<Result<List<CourseInstructorDto>>> GetCourseInstructorsAsync(Guid courseId)
    {
        var instructorAssignments = await _unitOfWork
            .Repository<UserCourse>()
            .GetListAsync(
                uc => uc.CourseId == courseId && (uc.ParticipationType == SystemRoles.INSTRUCTOR
                    || uc.ParticipationType == ParticipationTypes.FLIGHT_LEADER),
                includeFunc: q => q
                    .Include(uc => uc.User)
            );

        var instructors = instructorAssignments.Select(ua => new CourseInstructorDto
        {
            CourseId = ua.CourseId,
            InstructorId = ua.UserId,
            InstructorName = $"{ua.User.Grade}. {ua.User.Name} {ua.User.LastName}",
            InstructorIdentification = ua.User.IdentificationNumber,
            ParticipationType = ua.ParticipationType,
            WingType = ua.WingType,
            AssignmentDate = ua.AssignmentDate,
            IsActive = ua.IsActive
        })
        .OrderByDescending(x => x.ParticipationType)
        .ToList();

        return Result<List<CourseInstructorDto>>.Success(instructors);
    }

    public async Task<Result<bool>> AssignInstructorsToCourseAsync(AssignInstructorsDto assignDto)
    {
        // Validar que el programa exista
        var course = await _unitOfWork.Repository<Course>().GetByIdAsync(assignDto.CourseId);
        if (course == null)
            return Result<bool>.Failure(SystemErrors.CourseError.NotFound);

        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            // verificar si ya existen lideres de vuelo asignados
            bool hasFixedWingLeader = false;
            bool hasRotaryWingLeader = false;
            if (assignDto.FlightLeaderFixedWingId.HasValue || assignDto.FlightLeaderRotaryWingId.HasValue)
            {
                var currentLeaders = await _unitOfWork.Repository<UserCourse>()
                    .GetListAsync(uc => uc.CourseId == assignDto.CourseId && uc.ParticipationType == ParticipationTypes.FLIGHT_LEADER);
                hasFixedWingLeader = currentLeaders.Any(uc => uc.WingType == WingTypes.FIXED_WING);
                hasRotaryWingLeader = currentLeaders.Any(uc => uc.WingType == WingTypes.ROTARY_WING);
                if (hasFixedWingLeader && hasRotaryWingLeader)
                {
                    return Result<bool>.Failure("FLIGHT_LEADERS_ALREADY_ASSIGNED", "Ya existen lideres de vuelo asignados al programa");
                }
            }

            foreach (var instructorId in assignDto.InstructorIds)
            {
                // Verificar que no esté ya asignado y activo en ese programa
                var existingAssignment = await _unitOfWork.Repository<UserCourse>()
                    .GetFirstAsync(uc => uc.UserId == instructorId && uc.CourseId == assignDto.CourseId);

                if (existingAssignment != null)
                    continue; // Ya está asignado, continuar con el siguiente

                // Obtener el usuario
                var user = await _unitOfWork.Repository<User>().GetFirstAsync(
                        u => u.Id == instructorId,
                        includeFunc: q => q.Include(u => u.AviationProfile)
                    );

                if (user == null)
                {
                    _logger.LogWarning("El instructor con ID {InstructorId} no existe", instructorId);
                    continue;
                }

                // Determinar el tipo de participación
                string participationType = assignDto.FlightLeaderFixedWingId.HasValue && assignDto.FlightLeaderFixedWingId.Value == instructorId
                    ? ParticipationTypes.FLIGHT_LEADER
                    : assignDto.FlightLeaderRotaryWingId.HasValue && assignDto.FlightLeaderRotaryWingId.Value == instructorId
                        ? ParticipationTypes.FLIGHT_LEADER
                        : SystemRoles.INSTRUCTOR;

                // verificar si es lider de vuelo y ya existe un lider de vuelo para su tipo de ala
                if (participationType == ParticipationTypes.FLIGHT_LEADER
                    && user.AviationProfile?.WingType == WingTypes.FIXED_WING
                    && hasFixedWingLeader)
                {
                    _logger.LogWarning("Ya existe un lider de vuelo para el tipo de ala {WingType}", WingTypes.FIXED_WING);
                    return Result<bool>.Failure("FLIGHT_LEADERS_ALREADY_ASSIGNED", $"Ya existe un lider de vuelo para el tipo de ala {WingTypes.FIXED_WING}");
                }

                if (participationType == ParticipationTypes.FLIGHT_LEADER
                    && user.AviationProfile?.WingType == WingTypes.ROTARY_WING
                    && hasRotaryWingLeader)
                {
                    _logger.LogWarning("Ya existe un lider de vuelo para el tipo de ala {WingType}", WingTypes.ROTARY_WING);
                    return Result<bool>.Failure("FLIGHT_LEADERS_ALREADY_ASSIGNED", $"Ya existe un lider de vuelo para el tipo de ala {WingTypes.ROTARY_WING}");
                }

                // Crear la asignación
                var userCourse = new UserCourse
                {
                    UserId = instructorId,
                    CourseId = assignDto.CourseId,
                    ParticipationType = participationType,
                    WingType = user.AviationProfile?.WingType,
                    AssignmentDate = DateTime.Now,
                    IsActive = true
                };

                await _unitOfWork.Repository<UserCourse>().AddAsync(userCourse);
            }

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al asignar instructores al programa {CourseId}", assignDto.CourseId);
            return Result<bool>.Failure("INSTRUCTOR_ASSIGNMENT_ERROR", "Error al asignar instructores al programa");
        }
    }

    public async Task<Result<bool>> UnassignInstructorFromCourseAsync(Guid courseId, Guid instructorId)
    {
        var assignment = await _unitOfWork.Repository<UserCourse>()
            .GetFirstAsync(uc => uc.UserId == instructorId &&
                               uc.CourseId == courseId);

        if (assignment == null)
            return Result<bool>.Failure("INSTRUCTOR_NOT_ASSIGNED", "El instructor no estaba asignado al programa");

        // Desactivar la asignación
        assignment.IsActive = false;
        assignment.UnassignmentDate = DateTime.Now;
        assignment.UnassignmentReason = "Desasignación manual por administrador";

        _unitOfWork.Repository<UserCourse>().Update(assignment);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    #endregion

    #region Privados

    /// <summary>
    /// Obtiene cursos donde el usuario participa según sus roles
    /// </summary>
    private async Task<List<CourseDto>> GetUserParticipationCoursesAsync(Guid userId)
    {
        // Obtener inscripciones del usuario
        var userCourses = await _unitOfWork.Repository<UserCourse>().GetListAsync(uc => uc.UserId == userId);

        if (!userCourses.Any()) return [];

        // Extraer IDs únicos de cursos (eliminar duplicados)
        var uniqueCourseIds = userCourses.Select(uc => uc.CourseId).Distinct().ToList();

        // Obtener los cursos completos con sus UserCourses activos
        var courses = await _unitOfWork.Repository<Course>()
            .GetListAsync(
                predicate: c => uniqueCourseIds.Contains(c.Id),
                includeFunc: q => q
                    .Include(c => c.UserCourses)
                        .ThenInclude(sc => sc.User)
            );

        // Convertir a DTOs
        var courseDtos = courses.Select(c => c.MapToDto()).ToList();

        return courseDtos;
    }

    #endregion
}