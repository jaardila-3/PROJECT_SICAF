using System.ComponentModel.DataAnnotations;

using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Catalogs;

/// <summary>
/// Entidad maestra para todos los catálogos del sistema
/// Implementa estructura recursiva para categorías padre-hijo
/// </summary>
public class MasterCatalog : BaseEntity
{
    /// <summary>
    /// Tipo de catálogo (DocumentType, Force, MilitaryGrade, etc.)
    /// </summary>
    [Required]
    [StringLength(50)]
    public required string CatalogType { get; set; }

    /// <summary>
    /// Código único del registro dentro del tipo de catálogo
    /// </summary>
    [Required]
    [StringLength(10)]
    public required string Code { get; set; }

    /// <summary>
    /// Nombre o descripción del elemento
    /// </summary>
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// Descripción extendida (opcional)
    /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Orden de visualización
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Fecha de creación
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Usuario que creó el registro
    /// </summary>
    [StringLength(50)]
    public string CreatedBy { get; set; } = string.Empty;
}