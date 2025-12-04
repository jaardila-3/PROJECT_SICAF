using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Common;

public abstract class AuditBase
{
    // Información del usuario ejecutor
    [Display(Name = "Usuario")]
    public string? UserName { get; set; }

    public Guid LoggedUserId { get; set; } = Guid.Empty;

    // Información del usuario afectado
    [Display(Name = "Usuario Afectado")]
    public string? AffectedUserIdentificationName { get; set; }

    [Display(Name = "Usuario Afectado")]
    public Guid? AffectedUserId { get; set; }

    // Información de la entidad afectada
    [Display(Name = "Entidad")]
    public string EntityName { get; set; } = string.Empty;

    public Guid EntityId { get; set; }

    // Información del contexto
    [Display(Name = "Operación")]
    public string OperationType { get; set; } = string.Empty;

    [Display(Name = "Módulo")]
    public string? Module { get; set; }

    [Display(Name = "Controlador")]
    public string? Controller { get; set; }

    [Display(Name = "Acción")]
    public string? Action { get; set; }

    [Display(Name = "Dirección IP")]
    public string? IpAddress { get; set; }
}