using SICAF.Common.DTOs.Logging;
using SICAF.Common.Helpers;
using SICAF.Data.Entities.Logging;

namespace SICAF.Business.Mappers.Logging;

public static class ErrorLogMapperExtensions
{
    /// <summary>
    /// Mapea el DTO a la entidad ErrorLog
    /// </summary>
    public static ErrorLog ToEntity(this ErrorLogDto dto)
    {
        var errorLog = new ErrorLog
        {
            // Información del error
            Message = dto.Message,
            ExceptionType = dto.ExceptionType,
            StackTrace = dto.StackTrace,
            InnerException = dto.InnerException,
            StatusCode = dto.StatusCode,
            Severity = dto.Severity,

            // Información del contexto HTTP
            HttpMethod = dto.HttpMethod,
            Url = dto.Url,
            IpAddress = dto.IpAddress,

            // Información del usuario
            LoggedUserId = dto.LoggedUserId,
            UserName = dto.UserName,

            // Información de la ruta
            Module = dto.Module,
            Controller = dto.Controller,
            Action = dto.Action,

            // Metadata
            CreatedAt = DateTimeHelper.Now,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
            MachineName = Environment.MachineName
        };

        return errorLog;
    }
}