namespace SICAF.Common.Constants;

/// <summary>
/// Constantes para el sistema de calificaciones
/// </summary>
public static class GradeConstants
{
    /// <summary>
    /// Tipos de calificaciones válidas
    /// </summary>
    public static class GradeTypes
    {
        public const string A = "A";
        public const string B = "B";
        public const string C = "C";
        public const string N = "N";
        public const string DM = "DM"; // Demostrativo
        public const string SC = "SC"; // Sin calificación
        public const string NR = "NR"; // N-Roja (No aprobado)

        public static readonly string[] All = [A, B, C, N, DM, SC, NR];
    }

    /// <summary>
    /// Categorías de motivos para N-Roja
    /// </summary>
    public static class NRedCategories
    {
        public const string Mental = "FACTOR MENTAL";
        public const string Physical = "FACTOR FÍSICO";
        public const string Emotional = "FACTOR EMOCIONAL";

        public static readonly string[] All = [Mental, Physical, Emotional];
    }

    /// <summary>
    /// Descripciones de las calificaciones
    /// </summary>
    public static class GradeDescriptions
    {
        public const string DM_DESCRIPTION = "Demostrativo";
        public const string SC_DESCRIPTION = "Sin calificación";
        public const string NR_DESCRIPTION = "N Roja";
    }
}