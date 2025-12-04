using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc;

using SICAF.Business.Interfaces.Auditing;
using SICAF.Business.Interfaces.Logging;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Auditing;
using SICAF.Common.Helpers;
using SICAF.Web.Interfaces.Audit;

namespace SICAF.Web.Controllers;

/// <summary>
/// Controlador base
/// </summary>
public abstract class BaseController : Controller
{
    // Servicios lazy-loaded
    private IAuditService? _auditService;
    private IErrorLoggingService? _errorLogService;
    private IAuditContext? _auditContext;

    protected IAuditService AuditService => _auditService ??= HttpContext.RequestServices.GetRequiredService<IAuditService>();
    protected IErrorLoggingService ErrorLogService => _errorLogService ??= HttpContext.RequestServices.GetRequiredService<IErrorLoggingService>();
    protected IAuditContext AuditContext => _auditContext ??= HttpContext.RequestServices.GetRequiredService<IAuditContext>();

    #region Auditoria
    /// <summary>
    /// Crea un objeto AuditInfoDto con la información del contexto actual
    /// </summary>
    protected AuditInfoDto CreateAuditInfo(string entityName, Guid entityId, string operationType, Guid? affectedUserId = null, string? affectedUser = null)
    {
        return new AuditInfoDto
        {
            EntityName = entityName,
            EntityId = entityId,
            OperationType = operationType,
            LoggedUserId = AuditContext.LoggedUserId,
            UserName = AuditContext.UserName,
            UserRole = AuditContext.UserRole,
            IpAddress = AuditContext.IpAddress,
            Module = AuditContext.Module,
            Controller = AuditContext.Controller,
            Action = AuditContext.Action,
            CreatedAt = DateTimeHelper.Now,
            AffectedUserId = affectedUserId,
            AffectedUserIdentificationName = affectedUser
        };
    }

    /// <summary>
    /// Crea un objeto AuditInfoDto cuando la entidad afectada es un usuario
    /// </summary>
    protected AuditInfoDto CreateUserAuditInfo(Guid affectedUserId, string affectedUser, string operationType)
    {
        return CreateAuditInfo(EntityNames.Identity.User, affectedUserId, operationType, affectedUserId, affectedUser);
    }

    /// <summary>
    /// Guarda un log de auditoría para una operación CRUD
    /// </summary>
    protected async Task SaveAuditLogAsync(string entityName, Guid entityId, string operationType,
        object? oldValues = null, object? newValues = null)
    {
        try
        {
            var auditInfo = CreateAuditInfo(entityName, entityId, operationType);
            await AuditService.SaveCrudAuditAsync(auditInfo, oldValues, newValues);
        }
        catch (Exception ex)
        {
            // Si falla la auditoría, registrarlo pero no interrumpir el flujo
            await SaveLogErrorAsync(ex, $"Error al guardar auditoría de {operationType} para {entityName}");
        }

        /*
            // USO:
            try
            {
                // Código que realiza una operación CRUD
                await SaveAuditLogAsync("User", userId, "Update", oldValues, newValues);
                await SaveAuditLogAsync("Course", id, "Read");
            }
            catch (Exception ex)
            {
                await SaveLogErrorAsync(ex, "Error al guardar auditoría de actualización de usuario");
            }
        */
    }

    /// <summary>
    /// Guarda un log de auditoría personalizado
    /// </summary>
    protected async Task SaveCustomAuditLogAsync(AuditInfoDto auditInfo)
    {
        try
        {
            // Completar información del contexto si no está presente
            if (Guid.Empty.Equals(auditInfo.LoggedUserId))
            {
                auditInfo.LoggedUserId = AuditContext.LoggedUserId;
                auditInfo.UserName = AuditContext.UserName;
                auditInfo.UserRole = AuditContext.UserRole;
                auditInfo.IpAddress = AuditContext.IpAddress;
                auditInfo.Module = AuditContext.Module;
                auditInfo.Controller = AuditContext.Controller;
                auditInfo.Action = AuditContext.Action;
            }

            await AuditService.SaveAuditLogAsync(auditInfo);
        }
        catch (Exception ex)
        {
            await SaveLogErrorAsync(ex, "Error al guardar auditoría personalizada");
        }

        /*
            // USO 1:
            var auditInfo = CreateAuditInfo("Order", orderId, "Create");
            auditInfo.NewValues = JsonSerializer.Serialize(order);
            await SaveCustomAuditLogAsync(auditInfo);
            // USO 2:
            var auditInfo = CreateAuditInfo("CourseEnrollment", courseId, "Enroll");
            auditInfo.NewValues = System.Text.Json.JsonSerializer.Serialize(new 
            { 
                courseId, 
                studentId,
                enrollmentDate = DateTime.Now
            });            
            await SaveCustomAuditLogAsync(auditInfo);
        */
    }
    #endregion

    #region log de error
    /// <summary>
    /// Registra un error manualmente con contexto HTTP completo
    /// </summary>
    protected async Task SaveLogErrorAsync(Exception ex, string? mensaje = null)
    {
        try
        {
            // Crear DTO de error con información de la excepción
            var errorDto = ErrorLogService.CreateErrorLogFromException(ex);

            // Usar HttpAuditContext para información del usuario
            errorDto.LoggedUserId = AuditContext.LoggedUserId;
            errorDto.UserName = AuditContext.UserName;
            errorDto.UserRole = AuditContext.UserRole;
            errorDto.IpAddress = AuditContext.IpAddress;
            errorDto.HttpMethod = AuditContext.HttpMethod;
            errorDto.Url = AuditContext.Url;
            errorDto.Module = AuditContext.Module;
            errorDto.Controller = AuditContext.Controller;
            errorDto.Action = AuditContext.Action;

            if (!string.IsNullOrEmpty(mensaje))
            {
                errorDto.Message = $"{mensaje}: {errorDto.Message}";
            }

            await ErrorLogService.SaveErrorLogAsync(errorDto);
        }
        catch (Exception logEx)
        {
            // Si falla el logging, usar el logger tradicional
            var logger = HttpContext.RequestServices.GetRequiredService<ILogger<BaseController>>();
            logger.LogError(logEx, "Error al registrar error en BD");
            logger.LogError(ex, "Error original: {Message}", ex.Message);
        }

        /*
            // USO:
            try
            {
                // Código que puede lanzar una excepción
            }
            catch (Exception ex)
            {
                await SaveLogErrorAsync(ex, "Error al procesar solicitud");
            }
        */
    }
    #endregion

    #region Validación
    /// <summary>
    /// Valida un modelo usando FluentValidation
    /// </summary>
    protected async Task<ValidationResult> ValidateAsync<T>(IValidator<T> validator, T model)
    {
        var result = await validator.ValidateAsync(model);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

        return result;
    }
    #endregion

    #region Respuestas JSON
    /// <summary>
    /// Retorna una respuesta JSON de error
    /// </summary>
    protected JsonResult JsonError(string message, object? errors = null)
    {
        return Json(new
        {
            success = false,
            message = message,
            errors = errors
        });

        /*
            //USO:
            return JsonError("Credenciales inválidas");
        */
    }

    /// <summary>
    /// Retorna una respuesta JSON de éxito
    /// </summary>
    protected JsonResult JsonSuccess(string message, object? data = null)
    {
        return Json(new
        {
            success = true,
            message = message,
            data = data
        });

        /*
            //USO:
            return JsonSuccess("Inicio de sesión exitoso", new { user, token });

            try
            {
                await _userService.AdminResetPasswordAsync(id, newPassword);                
                return JsonSuccess("Contraseña restablecida exitosamente. El usuario deberá cambiarla en su próximo inicio de sesión.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al restablecer contraseña");
                await SaveLogErrorAsync(ex, $"Error al restablecer contraseña del usuario {id}");
                return JsonError("Error al restablecer la contraseña");
            }
        */
    }

    /// <summary>
    /// Retorna una respuesta JSON con errores de validación
    /// </summary>
    protected JsonResult JsonValidationError(ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );

        return Json(new
        {
            success = false,
            message = "Error de validación",
            errors = errors
        });

        /*
            //USO:
            // Si es AJAX, devolver JSON
            var isAjaxRequest = IsAjaxRequest();                           
            if (isAjaxRequest) return JsonValidationError(validationResult);            
        */
    }

    /// <summary>
    /// Determina si la petición actual es AJAX
    /// </summary>
    protected bool IsAjaxRequest()
    {
        return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
               Request.Headers.Accept.Any(h => h?.Contains("application/json") == true) ||
               Request.ContentType?.Contains("application/json") == true;
    }
    #endregion
}