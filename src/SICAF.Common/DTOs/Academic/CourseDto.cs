using System.ComponentModel.DataAnnotations;

using SICAF.Common.Constants;
using SICAF.Common.DTOs.Reports;

namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para mostrar información de un programa
/// </summary>
public class CourseDto
{
    public Guid Id { get; set; }

    [Display(Name = "Número del programa")]
    public int CourseNumber { get; set; }

    [Display(Name = "Nombre del programa")]
    public string CourseName { get; set; } = string.Empty;

    [Display(Name = "Descripción")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Fecha de Inicio")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime StartDate { get; set; }

    [Display(Name = "Fecha de Finalización")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime EndDate { get; set; }

    [Display(Name = "Estudiantes Activos")]
    public int ActiveStudentsCount { get; set; }

    [Display(Name = "Instructores Inscritos")]
    public int EnrolledInstructorsCount { get; set; }

    [Display(Name = "Lideres Inscritos")]
    public int EnrolledLeadersCount { get; set; }

    /// <summary>
    /// Texto descriptivo del estado
    /// </summary>
    [Display(Name = "Estado del programa")]
    public string StatusCourse => DateTime.Today >= StartDate.Date && DateTime.Today <= EndDate.Date ? CourseConstants.CourseStatus.Active :
                                StartDate > DateTime.Now ? CourseConstants.CourseStatus.Pending : CourseConstants.CourseStatus.Finished;

    /// <summary>
    /// Clase CSS para el badge del estado
    /// </summary>
    public string StatusBadgeClass => DateTime.Today >= StartDate.Date && DateTime.Today <= EndDate.Date ? "bg-success" :
                                    StartDate > DateTime.Now ? "bg-secondary" : "bg-danger";

    /// <summary>
    /// Lista de estudiantes en el programa
    /// </summary>
    public List<UserCourseDto> UserCourses { get; set; } = [];

    /// <summary>
    /// Reporte individual del estudiante (solo cuando el usuario es estudiante)
    /// </summary>
    public IndividualReportDto? StudentReport { get; set; }
}