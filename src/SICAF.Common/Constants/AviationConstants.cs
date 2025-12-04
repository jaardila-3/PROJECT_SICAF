namespace SICAF.Common.Constants;

public static class AviationConstants
{
    /// <summary>
    /// Tipos de ala de la aeronave
    /// </summary>
    public static class WingTypes
    {
        public const string FIXED_WING = "FIJA";
        public const string ROTARY_WING = "ROTATORIA";

        public static readonly string[] ValidTypes = [FIXED_WING, ROTARY_WING];
    }

    /// <summary>
    /// Tipos de aeronave
    /// </summary>
    public static class AircraftTypes
    {
        public const string AIRPLANE = "AVION";
        public const string HELICOPTER = "HELICOPTERO";

        public static readonly string[] ValidTypes = [AIRPLANE, HELICOPTER];
    }

    /// <summary>
    /// Roles que requieren perfil de aviación
    /// </summary>
    public static readonly string[] AviationRoles =
    [
        SystemRoles.INSTRUCTOR,
        SystemRoles.STUDENT
    ];

    public const string NonEvaluableMission = "NonEvaluableMission";
}