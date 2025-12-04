using SICAF.Common.DTOs.Logging;

namespace SICAF.Business.Interfaces.Logging;

/// <summary>
/// Servicio para gestionar el registro de errores
/// </summary>
public interface IErrorLoggingService
{
    Task<bool> SaveErrorLogAsync(ErrorLogDto errorDto);
    ErrorLogDto CreateErrorLogFromException(Exception ex);
}