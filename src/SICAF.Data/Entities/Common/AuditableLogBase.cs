using SICAF.Common.Helpers;

namespace SICAF.Data.Entities.Common;

// <summary>
/// Clase base abstracta para entidades de auditoría y registro del sistema
/// </summary>
public abstract class AuditableLogBase
{
    /// <summary>
    /// Identificador único del registro
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Información del usuario que ejecuta la acción
    /// </summary>
    public Guid? LoggedUserId { get; set; }

    /// <summary>
    /// Nombre del usuario
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Dirección IP desde donde se realiza la operación
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// Módulo o area del sistema donde ocurre la operación
    /// </summary>
    public string? Module { get; set; }

    /// <summary>
    /// Controlador donde ocurre la operación
    /// </summary>
    public string? Controller { get; set; }

    /// <summary>
    /// Acción ejecutada
    /// </summary>
    public string? Action { get; set; }

    /// <summary>
    /// Fecha y hora de creación del registro
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTimeHelper.Now;
}