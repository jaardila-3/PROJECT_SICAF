namespace SICAF.Common.Constants;

/// <summary>
/// Constantes para el manejo de notificaciones en TempData
/// </summary>
public static class NotificationConstants
{
    // Tipos de notificaciones
    public const string Success = "SuccessMessage";
    public const string Error = "ErrorMessage";
    public const string Warning = "WarningMessage";
    public const string Info = "InfoMessage";
    public const string ForcePasswordChange = "ForcePasswordChangeMessage";

    // Títulos por defecto
    public static class Titles
    {
        public const string Success = "¡Éxito!";
        public const string Error = "Error";
        public const string Warning = "Advertencia";
        public const string Info = "Información";
    }

    // Botones
    public static class Buttons
    {
        public const string Accept = "Aceptar";
        public const string Cancel = "Cancelar";
        public const string Understood = "Entendido";
        public const string Yes = "Sí";
        public const string No = "No";
    }

    // Colores de botones (Bootstrap)
    public static class Colors
    {
        public const string Success = "#198754";
        public const string Error = "#dc3545";
        public const string Warning = "#ffc107";
        public const string Info = "#0dcaf0";
        public const string Primary = "#0d6efd";
    }

    // Tiempos de duración (milisegundos)
    public static class TimerDuration
    {
        public const int Fast = 2500;
        public const int Normal = 3500;
        public const int Slow = 5000;
    }
}
