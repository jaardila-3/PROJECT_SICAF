using SICAF.Data.Entities.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Entities.Academic;

/// <summary>
/// Perfil de aviación para usuarios con roles de vuelo (Estudiante, Instructor)
/// </summary>
public class AviationProfile : BaseEntity
{
    public Guid UserId { get; set; }

    /// <summary>
    /// PID - Identificación única de pilotos y estudiantes de vuelo
    /// </summary>
    public string PID { get; set; } = string.Empty;

    /// <summary>
    /// Posición de vuelo
    /// </summary>
    public string FlightPosition { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de ala: Fija o Rotatoria
    /// </summary>
    public string WingType { get; set; } = string.Empty; // "FIJA" o "ROTATORIA"

    /// <summary>
    /// Horas totales de vuelo
    /// </summary>
    public double TotalFlightHours { get; set; }

    // Relación con User
    public virtual User User { get; set; } = null!;
}