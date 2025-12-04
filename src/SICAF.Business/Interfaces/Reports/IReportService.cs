using SICAF.Common.DTOs.Reports;
using SICAF.Common.Models.Results;

namespace SICAF.Business.Interfaces.Reports;

public interface IReportService
{
    Task<List<(Guid Id, string Name)>> GetCoursesAsync(Guid? userId = null);

    /// <summary>
    /// Obtiene las fuerzas disponibles en un curso
    /// </summary>
    /// <param name="courseId">ID del curso</param>
    /// <returns>Lista de fuerzas (Id, Name)</returns>
    Task<List<(string Id, string Name)>> GetForcesByCourseAsync(Guid courseId);

    /// <summary>
    /// Obtiene los tipos de ala disponibles filtrados por curso y fuerza
    /// </summary>
    /// <param name="courseId">ID del curso</param>
    /// <param name="force">Fuerza seleccionada (o "TODAS" para todas)</param>
    /// <returns>Lista de tipos de ala (Id, Name)</returns>
    Task<List<(string Id, string Name)>> GetWingTypesByForceAsync(Guid courseId, string force);

    /// <summary>
    /// Obtiene las fases disponibles filtradas por tipo de ala
    /// </summary>
    /// <param name="wingType">Tipo de ala seleccionado</param>
    /// <returns>Lista de fases (Id, Name) ordenadas por número de fase</returns>
    Task<List<(Guid Id, string Name)>> GetPhasesByWingTypeAsync(string wingType);

    /// <summary>
    /// Obtiene los estudiantes filtrados por curso, fuerza, tipo de ala y fase
    /// </summary>
    /// <param name="courseId">ID del curso</param>
    /// <param name="force">Fuerza seleccionada (o "TODAS" para todas)</param>
    /// <param name="wingType">Tipo de ala (o "TODAS" para todos)</param>
    /// <param name="phaseId">ID de la fase (opcional)</param>
    /// <returns>Lista de estudiantes (Id, FullName)</returns>
    Task<List<(Guid Id, string Name)>> GetStudentsByFiltersAsync(Guid courseId, string force, string wingType, Guid? phaseId = null);

    /// <summary>
    /// Obtiene los datos para el informe general del curso
    /// </summary>
    /// <param name="courseId">ID del curso</param>
    /// <param name="force">Fuerza seleccionada (o "TODAS" para todas)</param>
    /// <param name="wingType">Tipo de ala (o "TODAS" para todos)</param>
    /// <param name="phaseId">ID de la fase (opcional, null = todas las fases)</param>
    /// <returns>Datos del informe general</returns>
    Task<Result<GeneralReportDto>> GetGeneralReportDataAsync(Guid courseId, string force, string wingType, Guid? phaseId = null);

    /// <summary>
    /// Obtiene los datos para el informe individual de un estudiante
    /// </summary>
    /// <param name="studentId">ID del estudiante</param>
    /// <param name="courseId">ID del curso</param>
    /// <param name="phaseId">ID de la fase (opcional, null = todas las fases)</param>
    /// <returns>Datos del informe individual del estudiante</returns>
    Task<Result<IndividualReportDto>> GetIndividualReportDataAsync(Guid studentId, Guid courseId, Guid? phaseId = null);

    /// <summary>
    /// Obtiene el detalle de distribución de calificaciones
    /// </summary>
    /// <param name="courseId">ID del curso</param>
    /// <param name="grade">Calificación seleccionada: "A", "B", "C", "N", "NR"</param>
    /// <param name="force">Fuerza seleccionada (o "TODAS" para todas)</param>
    /// <param name="wingType">Tipo de ala (o "TODAS" para todos)</param>
    /// <param name="phaseId">ID de la fase (opcional)</param>
    /// <returns>Detalle de evaluaciones con la calificación seleccionada</returns>
    Task<Result<GradeDistributionDetailDto>> GetGradeDistributionDetailAsync(
        Guid courseId,
        string grade,
        string? force,
        string? wingType,
        Guid? phaseId
    );

    /// <summary>
    /// Obtiene el detalle de horas de vuelo por máquina
    /// </summary>
    /// <param name="courseId">ID del curso</param>
    /// <param name="aircraftRegistration">Registro de la aeronave</param>
    /// <param name="force">Fuerza seleccionada (o "TODAS" para todas)</param>
    /// <param name="wingType">Tipo de ala (o "TODAS" para todos)</param>
    /// <param name="phaseId">ID de la fase (opcional)</param>
    /// <returns>Detalle de vuelos en la aeronave seleccionada</returns>
    Task<Result<MachineFlightHoursDetailDto>> GetMachineFlightHoursDetailAsync(
        Guid courseId,
        string aircraftRegistration,
        string? force,
        string? wingType,
        Guid? phaseId
    );

    /// <summary>
    /// Obtiene el detalle de horas de vuelo por instructor
    /// </summary>
    /// <param name="courseId">ID del curso</param>
    /// <param name="instructorName">Nombre completo del instructor (Grado + Nombre)</param>
    /// <param name="force">Fuerza seleccionada (o "TODAS" para todas)</param>
    /// <param name="wingType">Tipo de ala (o "TODAS" para todos)</param>
    /// <param name="phaseId">ID de la fase (opcional)</param>
    /// <returns>Detalle de vuelos con el instructor seleccionado</returns>
    Task<Result<InstructorFlightHoursDetailDto>> GetInstructorFlightHoursDetailAsync(
        Guid courseId,
        string instructorName,
        string? force,
        string? wingType,
        Guid? phaseId
    );

    /// <summary>
    /// Obtiene el detalle de misiones insatisfactorias por máquina
    /// </summary>
    /// <param name="courseId">ID del curso</param>
    /// <param name="aircraftRegistration">Registro de la aeronave</param>
    /// <param name="force">Fuerza seleccionada (o "TODAS" para todas)</param>
    /// <param name="wingType">Tipo de ala (o "TODAS" para todos)</param>
    /// <param name="phaseId">ID de la fase (opcional)</param>
    /// <returns>Detalle de misiones insatisfactorias en la aeronave seleccionada</returns>
    Task<Result<MachineUnsatisfactoryDetailDto>> GetMachineUnsatisfactoryDetailAsync(
        Guid courseId,
        string aircraftRegistration,
        string? force,
        string? wingType,
        Guid? phaseId
    );

    /// <summary>
    /// Obtiene el detalle de misiones insatisfactorias por instructor
    /// </summary>
    /// <param name="courseId">ID del curso</param>
    /// <param name="instructorName">Nombre completo del instructor (Grado + Nombre)</param>
    /// <param name="force">Fuerza seleccionada (o "TODAS" para todas)</param>
    /// <param name="wingType">Tipo de ala (o "TODAS" para todos)</param>
    /// <param name="phaseId">ID de la fase (opcional)</param>
    /// <returns>Detalle de misiones insatisfactorias con el instructor seleccionado</returns>
    Task<Result<InstructorUnsatisfactoryDetailDto>> GetInstructorUnsatisfactoryDetailAsync(
        Guid courseId,
        string instructorName,
        string? force,
        string? wingType,
        Guid? phaseId
    );

    /// <summary>
    /// Obtiene el detalle de N ROJA por categorías
    /// </summary>
    /// <param name="courseId">ID del curso</param>
    /// <param name="category">Categoría seleccionada (ej: "NAVEGACIÓN", "FACTOR MENTAL", etc.)</param>
    /// <param name="force">Fuerza seleccionada (o "TODAS" para todas)</param>
    /// <param name="wingType">Tipo de ala (o "TODAS" para todos)</param>
    /// <param name="phaseId">ID de la fase (opcional)</param>
    /// <returns>Detalle de evaluaciones N ROJA en la categoría seleccionada</returns>
    Task<Result<NRedCategoriesDetailDto>> GetNRedCategoriesDetailAsync(
        Guid courseId,
        string category,
        string? force,
        string? wingType,
        Guid? phaseId
    );
}