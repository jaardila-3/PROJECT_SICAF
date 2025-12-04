using System.Net;

using Microsoft.Extensions.Logging;

using SICAF.Business.Interfaces.Logging;
using SICAF.Business.Mappers.Logging;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Logging;
using SICAF.Data.Context;

namespace SICAF.Business.Services.Logging;

public class ErrorLoggingService(SicafDbContext context, ILogger<ErrorLoggingService> logger) : IErrorLoggingService
{
    private readonly SicafDbContext _context = context;
    private readonly ILogger<ErrorLoggingService> _logger = logger;

    /// <summary>
    /// Guarda un log de error desde un DTO
    /// </summary>
    public async Task<bool> SaveErrorLogAsync(ErrorLogDto errorDto)
    {
        try
        {
            var errorLog = errorDto.ToEntity();
            await _context.ErrorLogs.AddAsync(errorLog);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error crítico al guardar log de error en BD");
            throw;
        }
    }

    /// <summary>
    /// Crea un DTO de error desde una excepción (sin contexto HTTP)
    /// </summary>
    public ErrorLogDto CreateErrorLogFromException(Exception ex)
    {
        // Obtener el código de estado HTTP
        int statusCode = GetHttpStatusCode(ex);

        // Determinar la severidad basada en el LogLevel
        LogLevel logLevel = GetLogLevel(ex);

        var errorLogDto = new ErrorLogDto
        {
            Message = ex.Message,
            ExceptionType = ex.GetType().Name,
            StackTrace = ex.StackTrace,
            InnerException = ex.InnerException?.Message,
            StatusCode = statusCode,
            Severity = logLevel.ToString()
        };

        return errorLogDto;
    }

    #region Private Methods

    /// <summary>
    /// Obtiene el código de estado HTTP
    /// </summary>
    private static int GetHttpStatusCode(Exception exception)
    {
        return exception switch
        {
            // Excepciones de validación y argumentos
            ArgumentException or
            ArgumentNullException or
            ArgumentOutOfRangeException or
            FormatException or
            InvalidOperationException => (int)HttpStatusCode.BadRequest,

            // Excepciones de autorización
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,

            // Excepciones de recursos no encontrados
            FileNotFoundException or
            DirectoryNotFoundException or
            KeyNotFoundException => (int)HttpStatusCode.NotFound,

            // Excepciones de operaciones no permitidas
            NotSupportedException or
            PlatformNotSupportedException => (int)HttpStatusCode.MethodNotAllowed,

            // Excepciones de timeout
            TimeoutException or
            TaskCanceledException => (int)HttpStatusCode.RequestTimeout,

            // Excepciones de implementación
            NotImplementedException => (int)HttpStatusCode.NotImplemented,

            // Excepciones de servicio no disponible
            HttpRequestException => (int)HttpStatusCode.ServiceUnavailable,

            // Por defecto: Error interno del servidor
            _ => (int)HttpStatusCode.InternalServerError
        };
    }

    /// <summary>
    /// Determina el LogLevel basado en el tipo de excepción
    /// Esto sigue las convenciones de logging de ASP.NET Core
    /// </summary>
    private static LogLevel GetLogLevel(Exception exception)
    {
        return exception switch
        {
            // Excepciones críticas del sistema
            OutOfMemoryException or
            StackOverflowException or
            AccessViolationException or
            AppDomainUnloadedException => LogLevel.Critical,

            // Excepciones de error que requieren atención
            NullReferenceException or
            IndexOutOfRangeException or
            InvalidCastException or
            DivideByZeroException => LogLevel.Error,

            // Excepciones de validación y lógica de negocio
            ArgumentException or
            ArgumentNullException or
            ArgumentOutOfRangeException or
            InvalidOperationException or
            UnauthorizedAccessException => LogLevel.Warning,

            // Excepciones informativas
            NotImplementedException or
            NotSupportedException => LogLevel.Information,

            // Por defecto: Error
            _ => LogLevel.Error
        };
    }
    #endregion
}