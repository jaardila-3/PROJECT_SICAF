using System.ComponentModel.DataAnnotations;

using SICAF.Common.DTOs.Common;

namespace SICAF.Common.DTOs.Auditing;

/// <summary>
/// DTO para filtros de búsqueda de auditoría
/// </summary>
public class AuditFilterDto : AuditBase
{
    [Display(Name = "Fecha Desde")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateFrom { get; set; }

    [Display(Name = "Fecha Hasta")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateTo { get; set; }

    /// <summary>
    /// Indica si hay algún filtro aplicado
    /// </summary>
    public bool HasFilters =>
        !string.IsNullOrWhiteSpace(UserName) ||
        !string.IsNullOrWhiteSpace(AffectedUserIdentificationName) ||
        !string.IsNullOrWhiteSpace(EntityName) ||
        !string.IsNullOrWhiteSpace(OperationType) ||
        !string.IsNullOrWhiteSpace(Module) ||
        !string.IsNullOrWhiteSpace(IpAddress) ||
        DateFrom.HasValue ||
        DateTo.HasValue;
}