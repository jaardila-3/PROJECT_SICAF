using SICAF.Business.Mappers.Identity;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Academic;
using SICAF.Data.Entities.Academic;

namespace SICAF.Business.Mappers.Academic;

/// <summary>
/// Extensiones para mapear entidades de cursos a DTOs
/// </summary>
public static class CourseMapperExtensions
{
    /// <summary>
    /// Convierte una entidad Course a CourseDto
    /// </summary>
    public static CourseDto MapToDto(this Course course)
    {
        ArgumentNullException.ThrowIfNull(course);

        return new CourseDto
        {
            Id = course.Id,
            CourseNumber = course.CourseNumber,
            CourseName = course.CourseName,
            Description = course.Description,
            StartDate = course.StartDate,
            EndDate = course.EndDate,
            ActiveStudentsCount = course.UserCourses.Where(uc => uc.ParticipationType == SystemRoles.STUDENT)?.Count(sc => sc.IsActive) ?? 0,
            EnrolledInstructorsCount = course.UserCourses.Where(uc => uc.ParticipationType == SystemRoles.INSTRUCTOR)?.Count(sc => sc.IsActive) ?? 0,
            EnrolledLeadersCount = course.UserCourses.Where(uc => uc.ParticipationType == ParticipationTypes.FLIGHT_LEADER)?.Count(sc => sc.IsActive) ?? 0,
            UserCourses = course.UserCourses?
                .Where(sc => sc.IsActive)
                .Select(sc => sc.MapToDtoWithoutCourse())
                .ToList() ?? []
        };
    }

    /// <summary>
    /// Convierte CreateCourseDto a entidad Course
    /// </summary>
    public static Course MapToEntity(this CreateCourseDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new Course
        {
            CourseNumber = dto.CourseNumber,
            CourseName = dto.CourseName.Trim(),
            Description = dto.Description.Trim(),
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };
    }

    public static UserCourseDto MapToDto(this UserCourse userCourse)
    {
        ArgumentNullException.ThrowIfNull(userCourse);

        return new UserCourseDto
        {
            Id = userCourse.Id,
            UserId = userCourse.UserId,
            CourseId = userCourse.CourseId,
            ParticipationType = userCourse.ParticipationType,
            WingType = userCourse.WingType,
            AssignmentDate = userCourse.AssignmentDate,
            IsActive = userCourse.IsActive,
            UnassignmentDate = userCourse.UnassignmentDate,
            UnassignmentReason = userCourse.UnassignmentReason,
            User = userCourse.User.MapToDto(),
            Course = userCourse.Course.MapToDto()
        };
    }

    public static UserCourseDto MapToDtoWithoutCourse(this UserCourse userCourse)
    {
        ArgumentNullException.ThrowIfNull(userCourse);

        return new UserCourseDto
        {
            Id = userCourse.Id,
            UserId = userCourse.UserId,
            CourseId = userCourse.CourseId,
            ParticipationType = userCourse.ParticipationType,
            WingType = userCourse.WingType,
            AssignmentDate = userCourse.AssignmentDate,
            IsActive = userCourse.IsActive,
            UnassignmentDate = userCourse.UnassignmentDate,
            UnassignmentReason = userCourse.UnassignmentReason,
            User = userCourse.User.MapToDto()
            //Course = null
        };
    }
}