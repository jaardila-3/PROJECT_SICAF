namespace SICAF.Common.Helpers;

/// <summary>
/// Helper para manejo de fecha y hora con zona horaria de Colombia
/// </summary>
public static class DateTimeHelper
{
    /// <summary>
    /// Zona horaria de Colombia (UTC-5)
    /// </summary>
    private static readonly TimeZoneInfo ColombianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");

    /// <summary>
    /// Obtiene la fecha y hora actual en la zona horaria de Colombia
    /// </summary>
    /// <returns>DateTime con la hora actual de Colombia</returns>
    public static DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ColombianTimeZone);

    /// <summary>
    /// Convierte una fecha UTC a la zona horaria de Colombia
    /// </summary>
    /// <param name="utcDateTime">Fecha en UTC</param>
    /// <returns>Fecha convertida a hora de Colombia</returns>
    public static DateTime FromUtc(DateTime utcDateTime)
    {
        if (utcDateTime.Kind != DateTimeKind.Utc)
        {
            utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, ColombianTimeZone);
    }

    /// <summary>
    /// Convierte una fecha de Colombia a UTC
    /// </summary>
    /// <param name="colombianDateTime">Fecha en hora de Colombia</param>
    /// <returns>Fecha convertida a UTC</returns>
    public static DateTime ToUtc(DateTime colombianDateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(colombianDateTime, ColombianTimeZone);
    }
}
