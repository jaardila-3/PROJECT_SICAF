using System.Text.Json;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using SICAF.Business.Interfaces.Academic;
using SICAF.Business.Interfaces.Identity;
using SICAF.Business.Interfaces.Reports;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Academic;
using SICAF.Web.Controllers;

namespace SICAF.Web.Areas.Academic.Controllers;

/// <summary>
/// Controlador para la gestión de cursos académicos
/// </summary>
[Area("Academic")]
[Authorize]
public class CourseController(
    ILogger<CourseController> logger,
    ICourseService courseService,
    IUserService userService,
    IReportService reportService,
    IValidator<CreateCourseDto> createCourseValidator
    ) : BaseController
{
    private readonly ILogger<CourseController> _logger = logger;
    private readonly ICourseService _courseService = courseService;
    private readonly IUserService _userService = userService;
    private readonly IReportService _reportService = reportService;
    private readonly IValidator<CreateCourseDto> _createCourseValidator = createCourseValidator;

    /// <summary>
    /// Lista todos los cursos
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN},{SystemRoles.INSTRUCTOR},{SystemRoles.STUDENT}")]
    public async Task<IActionResult> Index()
    {
        var currentUserId = AuditContext.LoggedUserId;
        var currentUserRoles = AuditContext.UserRole?.Split(',') ?? [];
        var result = await _courseService.GetCoursesForUserAsync(currentUserId, currentUserRoles);

        if (!result.IsSuccess)
        {
            if (result.Error.Code == SystemErrors.CourseError.NotFound.Code)
            {
                TempData[NotificationConstants.Warning] = "No se encontraron programas";
            }
            else
            {
                _logger.LogError("Error al obtener cursos: {Error}", result.Error.Message);
                TempData[NotificationConstants.Error] = "Error al cargar los programas";
            }
            return View(new List<CourseDto>());
        }

        return View(result.Value);
    }

    /// <summary>
    /// Muestra los detalles de un programa
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN},{SystemRoles.INSTRUCTOR},{SystemRoles.STUDENT}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var courseResult = await _courseService.GetCourseByIdAsync(id);
        if (!courseResult.IsSuccess)
        {
            _logger.LogError("Error al obtener programa {Id}: {Error}", id, courseResult.Error.Message);
            TempData[NotificationConstants.Error] = courseResult.Error.Message;
            return RedirectToAction(nameof(Index));
        }

        var course = courseResult.Value;
        var currentUserId = AuditContext.LoggedUserId;
        var currentUserRoles = AuditContext.UserRole?.Split(',') ?? [];

        // Determinar el tipo de vista según el rol principal del usuario
        ViewBag.IsAdminView = currentUserRoles.Contains(SystemRoles.ACADEMIC_ADMIN);
        ViewBag.IsInstructorView = currentUserRoles.Contains(SystemRoles.INSTRUCTOR);
        ViewBag.IsStudentView = currentUserRoles.Contains(SystemRoles.STUDENT);

        // Configuración específica según el rol
        if (ViewBag.IsInstructorView)
        {
            // Para instructores: filtrar por tipo de ala
            var wingTypeResult = await _userService.GetWingTypeAsync(currentUserId);
            var wingTypeFilter = wingTypeResult.Value;
            course.UserCourses = [.. course.UserCourses.Where(uc => uc.WingType == wingTypeFilter && uc.ParticipationType == SystemRoles.STUDENT)];
            ViewBag.WingTypeFilter = wingTypeFilter;

            var studentsResult = await _courseService.GetStudentsAsync(course.UserCourses);
            if (studentsResult.IsSuccess)
            {
                course.UserCourses = [.. studentsResult.Value];
            }
        }
        else if (ViewBag.IsStudentView)
        {
            // Para estudiantes: obtener reporte individual
            var studentReportResult = await _reportService.GetIndividualReportDataAsync(currentUserId, id);
            if (!studentReportResult.IsSuccess)
            {
                TempData[NotificationConstants.Warning] = studentReportResult.Error.Message;
                return RedirectToAction(nameof(Index));
            }

            course.StudentReport = studentReportResult.Value;
        }
        else if (ViewBag.IsAdminView)
        {
            // Para administradores: obtener todos los estudiantes
            course.UserCourses = [.. course.UserCourses.Where(uc => uc.ParticipationType == SystemRoles.STUDENT)];
            var studentsResult = await _courseService.GetStudentsAsync(course.UserCourses);
            if (studentsResult.IsSuccess)
            {
                course.UserCourses = [.. studentsResult.Value];
            }
        }

        return View(course);
    }

    /// <summary>
    /// Muestra el formulario para crear un programa
    /// </summary>
    [HttpGet]
    [Authorize(Roles = SystemRoles.ACADEMIC_ADMIN)]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Procesa la creación de un programa
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = SystemRoles.ACADEMIC_ADMIN)]
    public async Task<IActionResult> Create(CreateCourseDto model)
    {
        // Validar con FluentValidation
        var validationResult = await ValidateAsync(_createCourseValidator, model);
        if (!validationResult.IsValid)
        {
            return View(model);
        }

        // Crear el programa
        var result = await _courseService.CreateCourseAsync(model);
        if (!result.IsSuccess)
        {
            _logger.LogError("Error al crear programa: {Error}", result.Error.Message);
            ModelState.AddModelError(string.Empty, result.Error.Message);
            return View(model);
        }

        await SaveAuditLogAsync(EntityNames.Academic.Course, result.Value.Id, DatabaseOperationType.Create, null, result.Value);

        TempData[NotificationConstants.Success] = $"Programa {model.CourseNumber} - {model.CourseName} creado exitosamente";
        return RedirectToAction(nameof(Details), new { id = result.Value.Id });
    }

    /// <summary>
    /// Muestra el formulario para cambiar un estudiante de programa
    /// </summary>
    [HttpGet]
    [Authorize(Roles = SystemRoles.ACADEMIC_ADMIN)]
    public async Task<IActionResult> ChangeCourse(Guid studentId)
    {
        // Obtener el programa actual del estudiante
        var currentCourseResult = await _courseService.GetStudentCurrentCourseAsync(studentId);
        if (!currentCourseResult.IsSuccess)
        {
            TempData[NotificationConstants.Error] = "El estudiante no está inscrito en ningún programa";
            return RedirectToAction(nameof(Index));
        }

        var studentCourse = currentCourseResult.Value;
        var model = new ChangeCourseDto
        {
            StudentId = studentId,
            StudentIdentificationName = $"{studentCourse.User?.FullIdentification} - {studentCourse.User?.FullName}",
            CurrentCourseName = studentCourse.Course?.CourseName ?? string.Empty,
            CurrentCourseId = studentCourse.CourseId
        };

        await PrepareCoursesSelectList(studentCourse.CourseId);
        return View(model);
    }

    /// <summary>
    /// Procesa el cambio de programa de un estudiante
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = SystemRoles.ACADEMIC_ADMIN)]
    public async Task<IActionResult> ChangeCourse(ChangeCourseDto model)
    {
        if (!ModelState.IsValid)
        {
            await PrepareCoursesSelectList(model.CurrentCourseId);
            return View(model);
        }

        // datos para auditoría con usuario afectado
        var resultNewCourse = await _courseService.GetCourseByIdAsync(model.NewCourseId);
        if (!resultNewCourse.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        var result = await _courseService.ChangeStudentCourseAsync(model);
        if (!result.IsSuccess)
        {
            _logger.LogError("Error al cambiar estudiante de programa: {Error}", result.Error.Message);
            ModelState.AddModelError(string.Empty, result.Error.Message);
            await PrepareCoursesSelectList(model.CurrentCourseId);
            return View(model);
        }

        // Auditoría con usuario afectado
        var auditInfo = CreateUserAuditInfo(model.StudentId, model.StudentIdentificationName, DatabaseOperationType.ChangeCourse);
        auditInfo.OldValues = JsonSerializer.Serialize(new { CourseId = model.CurrentCourseId, CourseName = model.CurrentCourseName });
        auditInfo.NewValues = JsonSerializer.Serialize(new { CourseId = model.NewCourseId, CourseName = resultNewCourse.Value.CourseName });
        await SaveCustomAuditLogAsync(auditInfo);

        TempData[NotificationConstants.Success] = "Estudiante cambiado de programa exitosamente";
        return RedirectToAction(nameof(Details), new { id = model.NewCourseId });
    }

    /// <summary>
    /// Actualiza la fecha de finalización de un programa
    /// </summary>
    [HttpPost]
    [Authorize(Roles = SystemRoles.ACADEMIC_ADMIN)]
    public async Task<JsonResult> UpdateEndDate(Guid courseId, string newEndDate)
    {
        try
        {
            if (!DateTime.TryParse(newEndDate, out var endDate))
            {
                return JsonError("Fecha inválida");
            }

            var result = await _courseService.UpdateEndDateAsync(courseId, endDate);
            if (!result.IsSuccess)
            {
                return JsonError(result.Error.Message);
            }

            await SaveAuditLogAsync(EntityNames.Academic.Course, courseId,
                DatabaseOperationType.Update, null, new { EndDate = endDate });

            return JsonSuccess("Fecha actualizada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar fecha del programa");
            await SaveLogErrorAsync(ex, $"Error al actualizar fecha del programa {courseId}");
            return JsonError("Error al actualizar la fecha");
        }
    }

    #region Gestión de Instructores

    /// <summary>
    /// Vista para asignar instructores a un programa
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN}")]
    public async Task<IActionResult> AssignInstructors(Guid courseId)
    {
        var courseResult = await _courseService.GetCourseByIdAsync(courseId);
        if (!courseResult.IsSuccess)
        {
            TempData[NotificationConstants.Error] = "Programa no encontrado";
            return RedirectToAction(nameof(Index));
        }

        var instructorsResult = await _courseService.GetAvailableInstructorsByCourseAsync(courseId);
        if (!instructorsResult.IsSuccess)
        {
            TempData[NotificationConstants.Error] = "Error al cargar instructores disponibles";
            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        // Verificar si ya hay un líder de vuelo asignado para ala fija
        var hasLeaderFixedWing = courseResult.Value.UserCourses
            .Any(uc => uc.ParticipationType == ParticipationTypes.FLIGHT_LEADER && uc.ParticipationType == AviationConstants.WingTypes.FIXED_WING);
        // Verificar si ya hay un líder de vuelo asignado para ala rotatoria
        var hasLeaderRotaryWing = courseResult.Value.UserCourses
            .Any(uc => uc.ParticipationType == ParticipationTypes.FLIGHT_LEADER && uc.ParticipationType == AviationConstants.WingTypes.ROTARY_WING);

        ViewBag.Course = courseResult.Value;
        ViewBag.HasLeaderFixedWing = hasLeaderFixedWing;
        ViewBag.HasLeaderRotaryWing = hasLeaderRotaryWing;
        ViewBag.AvailableInstructors = instructorsResult.Value;
        ViewBag.WingTypes = new SelectList(new[] { AviationConstants.WingTypes.FIXED_WING, AviationConstants.WingTypes.ROTARY_WING });

        return View(new AssignInstructorsDto { CourseId = courseId });
    }

    /// <summary>
    /// Procesa la asignación de instructores
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN}")]
    public async Task<IActionResult> AssignInstructors(AssignInstructorsDto model)
    {
        if (!ModelState.IsValid)
        {
            var courseResult = await _courseService.GetCourseByIdAsync(model.CourseId);
            var instructorsResult = await _courseService.GetAvailableInstructorsByCourseAsync(model.CourseId);

            ViewBag.Course = courseResult.Value;
            ViewBag.AvailableInstructors = instructorsResult.Value;
            ViewBag.WingTypes = new SelectList(new[] { AviationConstants.WingTypes.FIXED_WING, AviationConstants.WingTypes.ROTARY_WING });

            return View(model);
        }

        var result = await _courseService.AssignInstructorsToCourseAsync(model);

        if (result.IsSuccess)
        {
            TempData[NotificationConstants.Success] = "Instructores asignados exitosamente";
            foreach (var instructorId in model.InstructorIds)
            {
                // Obtener valores para auditoría
                var userResult = await _userService.GetUserByIdAsync(instructorId);
                var user = userResult.IsSuccess ? userResult.Value : null;
                var participationType = (model.FlightLeaderFixedWingId.HasValue && model.FlightLeaderFixedWingId.Value == instructorId)
                                    || (model.FlightLeaderRotaryWingId.HasValue && model.FlightLeaderRotaryWingId.Value == instructorId)
                                        ? ParticipationTypes.FLIGHT_LEADER
                                        : SystemRoles.INSTRUCTOR;

                var auditInfo = CreateUserAuditInfo(instructorId, $"{user?.FullIdentification} - {user?.FullName}", DatabaseOperationType.AssignedInstructor);
                auditInfo.NewValues = JsonSerializer.Serialize(new { courseId = model.CourseId, instructorId, participationType });
                await SaveCustomAuditLogAsync(auditInfo);
            }
        }
        else
        {
            TempData[NotificationConstants.Error] = result.Error.Message;
        }

        return RedirectToAction(nameof(Details), new { id = model.CourseId });
    }

    /// <summary>
    /// Desasigna un instructor de un programa
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN}")]
    public async Task<IActionResult> UnassignInstructor(Guid courseId, Guid instructorId)
    {
        var result = await _courseService.UnassignInstructorFromCourseAsync(courseId, instructorId);

        if (result.IsSuccess)
        {
            TempData[NotificationConstants.Success] = "Instructor desasignado exitosamente";

            // Obtener valores para auditoría
            var userResult = await _userService.GetUserByIdAsync(instructorId);
            var user = userResult.IsSuccess ? userResult.Value : null;

            var auditInfo = CreateUserAuditInfo(instructorId, $"{user?.FullIdentification} - {user?.FullName}", DatabaseOperationType.UnassignedInstructor);
            auditInfo.NewValues = JsonSerializer.Serialize(new { courseId, instructorId });
            await SaveCustomAuditLogAsync(auditInfo);
        }
        else
        {
            TempData[NotificationConstants.Error] = result.Error.Message;
        }

        return RedirectToAction(nameof(Details), new { id = courseId });
    }

    /// <summary>
    /// Obtiene instructores asignados al programa (AJAX)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN}")]
    public async Task<JsonResult> GetCourseInstructors(Guid courseId)
    {
        var result = await _courseService.GetCourseInstructorsAsync(courseId);

        if (result.IsSuccess)
        {
            return JsonSuccess("Instructores obtenidos exitosamente", result.Value);
        }
        return JsonError(result.Error.Message);
    }

    #endregion

    #region Métodos privados auxiliares

    /// <summary>
    /// Prepara la lista de estudiantes disponibles para inscripción
    /// </summary>
    private async Task PrepareStudentsSelectList()
    {
        // Obtener solo usuarios con rol estudiante que no estén en un programa activo
        var studentsResult = await _userService.GetAllUsersAsync();
        if (studentsResult.IsSuccess)
        {
            var availableStudents = new List<SelectListItem>();

            foreach (var user in studentsResult.Value)
            {
                // Verificar si tiene rol de estudiante
                if (user.Roles.Any(r => r.Name == SystemRoles.STUDENT))
                {
                    // Verificar si puede ser inscrito (no tiene programa activo)
                    var canEnroll = await _courseService.CanEnrollStudentAsync(user.Id);
                    if (canEnroll.Value)
                    {
                        availableStudents.Add(new SelectListItem
                        {
                            Value = user.Id.ToString(),
                            Text = $"{user.FullIdentification} - {user.FullName}"
                        });
                    }
                }
            }

            ViewBag.Students = new SelectList(availableStudents, "Value", "Text");
        }
        else
        {
            ViewBag.Students = new SelectList(new List<SelectListItem>(), "Value", "Text");
        }
    }

    /// <summary>
    /// Prepara la lista de cursos activos
    /// </summary>
    private async Task PrepareCoursesSelectList(Guid? excludeCourseId = null)
    {
        var coursesResult = await _courseService.GetActiveCoursesAsync();
        if (coursesResult.IsSuccess)
        {
            var courses = coursesResult.Value
                .Where(c => c.Id != excludeCourseId)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.CourseNumber} - {c.CourseName}"
                })
                .ToList();

            ViewBag.Courses = new SelectList(courses, "Value", "Text");
        }
        else
        {
            ViewBag.Courses = new SelectList(new List<SelectListItem>(), "Value", "Text");
        }
    }

    #endregion
}