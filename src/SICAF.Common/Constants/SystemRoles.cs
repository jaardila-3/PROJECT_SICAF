namespace SICAF.Common.Constants;

/// <summary>
/// Catálogo centralizado de roles del sistema
/// </summary>
public static class SystemRoles
{
    /// <summary>
    /// Administrador de usuarios del sistema
    /// </summary>
    public const string USERS_ADMIN = "Administrador de Usuarios";

    /// <summary>
    /// Administrador académico con gestión de cursos y estudiantes
    /// </summary>
    public const string ACADEMIC_ADMIN = "Administrador Académico";

    /// <summary>
    /// Instructor de vuelo con capacidad de evaluación
    /// </summary>
    public const string INSTRUCTOR = "Instructor de Vuelo";

    /// <summary>
    /// Estudiante del programa de aviación policial
    /// </summary>
    public const string STUDENT = "Estudiante";

}

public static class ParticipationTypes
{
    /// <summary>
    /// Instructor coordinador de un curso
    /// </summary>
    public const string FLIGHT_LEADER = "Líder de vuelo";
}