using SICAF.Common.DTOs.Academic;
using SICAF.Common.DTOs.Identity;
using SICAF.Common.Models.Results;

namespace SICAF.Business.Interfaces.Academic;

/// <summary>
/// Interface para el servicio de gestión de cursos
/// </summary>
public interface ICourseService
{
    Task<Result<IEnumerable<CourseDto>>> GetAllCoursesAsync();
    Task<Result<IEnumerable<CourseDto>>> GetActiveCoursesAsync();
    Task<Result<CourseDto>> GetCourseByIdAsync(Guid courseId);
    Task<Result<CourseDto>> CreateCourseAsync(CreateCourseDto createCourseDto);
    Task<Result<bool>> UpdateEndDateAsync(Guid courseId, DateTime newEndDate);

    /// <summary>
    /// Obtiene instructores y líderes de vuelo disponibles
    /// </summary>
    Task<Result<List<UserDto>>> GetAvailableInstructorsByCourseAsync(Guid courseId);

    /// <summary>
    /// Obtiene instructores asignados a un curso
    /// </summary>
    Task<Result<List<CourseInstructorDto>>> GetCourseInstructorsAsync(Guid courseId);

    /// <summary>
    /// Asigna instructores a un curso
    /// </summary>
    Task<Result<bool>> AssignInstructorsToCourseAsync(AssignInstructorsDto assignDto);

    /// <summary>
    /// Desasigna un instructor de un curso
    /// </summary>
    Task<Result<bool>> UnassignInstructorFromCourseAsync(Guid courseId, Guid instructorId);

    Task<Result<bool>> ChangeStudentCourseAsync(ChangeCourseDto changeDto);

    Task<Result<UserCourseDto>> GetStudentCurrentCourseAsync(Guid studentId);

    Task<Result<IEnumerable<UserCourseDto>>> GetStudentsAsync(List<UserCourseDto> students);

    /// <summary>
    /// Obtiene los cursos filtrados según el rol del usuario
    /// </summary>
    Task<Result<List<CourseDto>>> GetCoursesForUserAsync(Guid userId, string[] userRoles);

    Task<Result<bool>> IsCourseNumberAvailableAsync(int courseNumber);
    Task<Result<bool>> CanEnrollStudentAsync(Guid studentId);
}