using System.ComponentModel.DataAnnotations;

using SICAF.Common.DTOs.Common;
using SICAF.Common.Helpers;

namespace SICAF.Common.DTOs.Auditing;

public class AuditInfoDto : AuditBase
{
    public Guid Id { get; set; }

    public string? UserRole { get; set; }

    // Datos de la operación
    [Display(Name = "Valores Anteriores")]
    public string? OldValues { get; set; } // JSON con valores anteriores

    [Display(Name = "Valores Nuevos")]
    public string? NewValues { get; set; } // JSON con valores nuevos

    // Timestamp
    [Display(Name = "Fecha y Hora")]
    public DateTime CreatedAt { get; set; } = DateTimeHelper.Now;

    /// <summary>
    /// Indica si hay valores anteriores para mostrar
    /// </summary>
    public bool HasOldValues => !string.IsNullOrWhiteSpace(OldValues);

    /// <summary>
    /// Indica si hay valores nuevos para mostrar
    /// </summary>
    public bool HasNewValues => !string.IsNullOrWhiteSpace(NewValues);
}