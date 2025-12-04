namespace SICAF.Common.DTOs.Logging;

/// <summary>
/// DTO simplificado para información de error
/// </summary>
public class ErrorLogDto
{
    // Información del error
    public string Message { get; set; } = string.Empty;
    public string ExceptionType { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string? InnerException { get; set; }
    public int StatusCode { get; set; }
    public string Severity { get; set; } = "Error";

    // Información del contexto HTTP
    public string? HttpMethod { get; set; }
    public string? Url { get; set; }
    public string? IpAddress { get; set; }

    // Información del usuario
    public Guid? LoggedUserId { get; set; }
    public string? UserName { get; set; }
    public string? UserRole { get; set; }

    // Información de la ruta
    public string? Module { get; set; }
    public string? Controller { get; set; }
    public string? Action { get; set; }
}