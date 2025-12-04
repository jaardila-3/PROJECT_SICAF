namespace SICAF.Common.Helpers;

/// <summary>
/// Helper para el manejo de días festivos en Colombia
/// </summary>
public static class ColombianHolidayHelper
{
    /// <summary>
    /// Obtiene todos los días festivos de un año específico en Colombia
    /// </summary>
    public static List<DateTime> GetHolidays(int year)
    {
        var holidays = new List<DateTime>
        {
            // FESTIVOS FIJOS
            new(year, 1, 1),   // Año Nuevo
            new(year, 5, 1),   // Día del Trabajo
            new(year, 7, 20),  // Día de la Independencia
            new(year, 8, 7),   // Batalla de Boyacá
            new(year, 12, 8),  // Inmaculada Concepción
            new(year, 12, 25), // Navidad

            // FESTIVOS QUE SE TRASLADAN AL LUNES (Ley Emiliani)
            // 6 de enero - Reyes Magos
            GetNextMonday(new DateTime(year, 1, 6)),

            // 19 de marzo - San José
            GetNextMonday(new DateTime(year, 3, 19)),

            // 29 de junio - San Pedro y San Pablo
            GetNextMonday(new DateTime(year, 6, 29)),

            // 15 de agosto - Asunción de la Virgen
            GetNextMonday(new DateTime(year, 8, 15)),

            // 12 de octubre - Día de la Raza
            GetNextMonday(new DateTime(year, 10, 12)),

            // 1 de noviembre - Todos los Santos
            GetNextMonday(new DateTime(year, 11, 1)),

            // 11 de noviembre - Independencia de Cartagena
            GetNextMonday(new DateTime(year, 11, 11))
        };

        // FESTIVOS BASADOS EN PASCUA (SEMANA SANTA)
        var easter = CalculateEasterSunday(year);

        // Jueves Santo (3 días antes de Pascua)
        holidays.Add(easter.AddDays(-3));

        // Viernes Santo (2 días antes de Pascua)
        holidays.Add(easter.AddDays(-2));

        // Ascensión del Señor (43 días después de Pascua, se traslada al lunes)
        holidays.Add(GetNextMonday(easter.AddDays(42)));

        // Corpus Christi (64 días después de Pascua, se traslada al lunes)
        holidays.Add(GetNextMonday(easter.AddDays(63)));

        // Sagrado Corazón (71 días después de Pascua, se traslada al lunes)
        holidays.Add(GetNextMonday(easter.AddDays(70)));

        return holidays.Distinct().OrderBy(d => d).ToList();
    }

    /// <summary>
    /// Verifica si una fecha es festivo en Colombia
    /// </summary>
    public static bool IsHoliday(DateTime date)
    {
        var holidays = GetHolidays(date.Year);
        return holidays.Any(h => h.Date == date.Date);
    }

    /// <summary>
    /// Verifica si una fecha es día hábil (no es fin de semana ni festivo)
    /// </summary>
    public static bool IsWorkingDay(DateTime date)
    {
        // No es fin de semana
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return false;

        // No es festivo
        return !IsHoliday(date);
    }

    /// <summary>
    /// Obtiene el siguiente lunes de una fecha (para festivos que se trasladan)
    /// Si la fecha ya es lunes, devuelve la misma fecha
    /// </summary>
    private static DateTime GetNextMonday(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Monday)
            return date;

        int daysUntilMonday = ((int)DayOfWeek.Monday - (int)date.DayOfWeek + 7) % 7;

        return date.AddDays(daysUntilMonday);
    }

    /// <summary>
    /// Calcula el Domingo de Pascua usando el algoritmo de Meeus/Jones/Butcher
    /// </summary>
    private static DateTime CalculateEasterSunday(int year)
    {
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;

        int month = (h + l - 7 * m + 114) / 31;
        int day = ((h + l - 7 * m + 114) % 31) + 1;

        return new DateTime(year, month, day);
    }

    /// <summary>
    /// Obtiene información detallada sobre un festivo
    /// </summary>
    public static string GetHolidayName(DateTime date)
    {
        var year = date.Year;
        var easter = CalculateEasterSunday(year);

        // Festivos fijos
        if (date.Date == new DateTime(year, 1, 1)) return "Año Nuevo";
        if (date.Date == new DateTime(year, 5, 1)) return "Día del Trabajo";
        if (date.Date == new DateTime(year, 7, 20)) return "Día de la Independencia";
        if (date.Date == new DateTime(year, 8, 7)) return "Batalla de Boyacá";
        if (date.Date == new DateTime(year, 12, 8)) return "Inmaculada Concepción";
        if (date.Date == new DateTime(year, 12, 25)) return "Navidad";

        // Festivos trasladados
        if (date.Date == GetNextMonday(new DateTime(year, 1, 6))) return "Reyes Magos";
        if (date.Date == GetNextMonday(new DateTime(year, 3, 19))) return "San José";
        if (date.Date == GetNextMonday(new DateTime(year, 6, 29))) return "San Pedro y San Pablo";
        if (date.Date == GetNextMonday(new DateTime(year, 8, 15))) return "Asunción de la Virgen";
        if (date.Date == GetNextMonday(new DateTime(year, 10, 12))) return "Día de la Raza";
        if (date.Date == GetNextMonday(new DateTime(year, 11, 1))) return "Todos los Santos";
        if (date.Date == GetNextMonday(new DateTime(year, 11, 11))) return "Independencia de Cartagena";

        // Festivos de Semana Santa
        if (date.Date == easter.AddDays(-3).Date) return "Jueves Santo";
        if (date.Date == easter.AddDays(-2).Date) return "Viernes Santo";
        if (date.Date == GetNextMonday(easter.AddDays(42))) return "Ascensión del Señor";
        if (date.Date == GetNextMonday(easter.AddDays(63))) return "Corpus Christi";
        if (date.Date == GetNextMonday(easter.AddDays(70))) return "Sagrado Corazón";

        return string.Empty;
    }
}