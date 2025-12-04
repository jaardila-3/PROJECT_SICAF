using System.Net;
using System.Text.Json;

using SICAF.Business.Interfaces.Logging;
using SICAF.Web.Interfaces.Audit;

namespace SICAF.Web.Middlewares;

/// <summary>
/// Middleware para manejar excepciones globales
/// </summary>
public class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger,
    IWebHostEnvironment environment)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;
    private readonly IWebHostEnvironment _environment = environment;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Error no manejado: {Message}", exception.Message);

        // Intentar guardar en BD (si el servicio está disponible)
        await TryLogToDatabase(context, exception);

        // Determinar si es petición AJAX
        var isAjaxRequest = IsAjaxRequest(context);

        if (isAjaxRequest)
            await HandleAjaxException(context, exception);
        else
            await HandleWebException(context, exception);
    }

    private async Task TryLogToDatabase(HttpContext context, Exception exception)
    {
        try
        {
            // Obtener servicios necesarios
            var errorLogService = context.RequestServices.GetService<IErrorLoggingService>();
            var auditContext = context.RequestServices.GetService<IAuditContext>();

            if (errorLogService == null) return;

            // Crear DTO con información del error
            var errorDto = errorLogService.CreateErrorLogFromException(exception);

            // Si tenemos HttpAuditContext, usar su información
            if (auditContext != null)
            {
                errorDto.LoggedUserId = auditContext.LoggedUserId;
                errorDto.UserName = auditContext.UserName;
                errorDto.UserRole = auditContext.UserRole;
                errorDto.IpAddress = auditContext.IpAddress;
                errorDto.HttpMethod = auditContext.HttpMethod;
                errorDto.Url = auditContext.Url;
                errorDto.Module = auditContext.Module;
                errorDto.Controller = auditContext.Controller;
                errorDto.Action = auditContext.Action;
            }
            else
            {
                // Si no hay HttpAuditContext, obtener información básica
                errorDto.IpAddress = context.Connection.RemoteIpAddress?.ToString();
                errorDto.HttpMethod = context.Request.Method;
                errorDto.Url = $"{context.Request.Path}{context.Request.QueryString}";
                errorDto.Module = context.GetRouteValue("area")?.ToString();
                errorDto.Controller = context.GetRouteValue("controller")?.ToString();
                errorDto.Action = context.GetRouteValue("action")?.ToString();
            }

            await errorLogService.SaveErrorLogAsync(errorDto);
        }
        catch (Exception logEx)
        {
            _logger.LogWarning(logEx, "No se pudo guardar el error en la base de datos");
        }
    }

    private async Task HandleAjaxException(HttpContext context, Exception exception)
    {
        var statusCode = GetHttpStatusCode(exception);
        var message = GetUserFriendlyMessage(exception);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            success = false,
            message = message,
            errors = _environment.IsDevelopment() ? exception.ToString() : null
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private async Task HandleWebException(HttpContext context, Exception exception)
    {
        var statusCode = GetHttpStatusCode(exception);
        var message = GetUserFriendlyMessage(exception);

        // Guardar en sesión para mostrar en la página de error
        if (context.Session != null)
        {
            context.Session.SetString("ErrorMessage", message);
            context.Session.SetInt32("ErrorStatusCode", statusCode);
        }

        // Redirigir a la página de error
        context.Response.Redirect("/Home/Error");
        await Task.CompletedTask;
    }

    /// <summary>
    /// Obtiene el código de estado HTTP apropiado para la excepción
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
            InvalidOperationException => StatusCodes.Status400BadRequest,

            // Excepciones de autorización
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,

            // Excepciones de recursos no encontrados
            FileNotFoundException or
            DirectoryNotFoundException or
            KeyNotFoundException => StatusCodes.Status404NotFound,

            // Excepciones de operaciones no permitidas
            NotSupportedException or
            PlatformNotSupportedException => StatusCodes.Status405MethodNotAllowed,

            // Excepciones de timeout
            TimeoutException or
            TaskCanceledException => StatusCodes.Status408RequestTimeout,

            // Excepciones de implementación
            NotImplementedException => StatusCodes.Status501NotImplemented,

            // Excepciones de servicio no disponible
            HttpRequestException => StatusCodes.Status503ServiceUnavailable,

            // Por defecto: Error interno del servidor
            _ => StatusCodes.Status500InternalServerError
        };
    }

    /// <summary>
    /// Obtiene un mensaje amigable para el usuario basado en el tipo de excepción
    /// </summary>
    private static string GetUserFriendlyMessage(Exception exception)
    {
        return exception switch
        {
            UnauthorizedAccessException => "Acceso no autorizado. Por favor, inicie sesión.",
            ArgumentException or ArgumentNullException => "Los datos proporcionados no son válidos. Por favor, verifique e intente nuevamente.",
            InvalidOperationException => "La operación solicitada no es válida en este momento.",
            NotImplementedException => "Esta funcionalidad aún no está disponible.",
            FileNotFoundException => "El recurso solicitado no fue encontrado.",
            TimeoutException => "La operación tardó demasiado tiempo. Por favor, intente nuevamente.",
            TaskCanceledException => "La operación fue cancelada.",
            HttpRequestException => "Error al comunicarse con el servicio externo.",
            _ => "Ha ocurrido un error inesperado. Por favor, intente nuevamente."
        };
    }

    private bool IsAjaxRequest(HttpContext context)
    {
        return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
               context.Request.ContentType?.Contains("application/json") == true ||
               context.Request.Headers.Accept.Any(h => h?.Contains("application/json") == true);
    }
}