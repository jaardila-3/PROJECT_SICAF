using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Logging;

/// <summary>
/// Entidad de registro de errores
/// </summary>
public class ErrorLog : AuditableLogBase
{
    // Información del error
    public string Message { get; set; } = string.Empty;
    public string ExceptionType { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string? InnerException { get; set; }

    // Contexto de la solicitud
    public string? HttpMethod { get; set; }
    public string? Url { get; set; }

    // Clasificación del error
    public int StatusCode { get; set; }
    public string Severity { get; set; } = "Error";

    // Información adicional
    public string? MachineName { get; set; }
    public string? Environment { get; set; }

    // Estado y resolución
    public bool IsResolved { get; set; } = false;
    public string? ResolutionNotes { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolvedBy { get; set; }
}