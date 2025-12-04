namespace SICAF.Common.DTOs.Academic;

public class AircraftDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// Tipo de aeronave: AIRPLANE o HELICOPTER
    /// </summary>
    public string AircraftType { get; set; } = string.Empty;

    /// <summary>
    /// Matrícula de la aeronave (ej: PNC0269, PNC0905)
    /// </summary>
    public string Registration { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de ala: FIXED_WING o ROTARY_WING
    /// Para facilitar búsquedas y filtros
    /// </summary>
    public string WingType { get; set; } = string.Empty;

    /// <summary>
    /// Horas totales de vuelo de la aeronave
    /// </summary>
    public double TotalFlightHours { get; set; } = 0d;
}