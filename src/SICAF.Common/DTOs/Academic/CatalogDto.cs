using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Academic;

public class CatalogDto
{
    /// <summary>
    /// Tipo de catálogo (DocumentType, Force, MilitaryGrade, etc.)
    /// </summary>
    [StringLength(50)]
    public required string CatalogType { get; set; }

    /// <summary>
    /// Código único del registro dentro del tipo de catálogo
    /// </summary>
    [StringLength(20)]
    public required string Code { get; set; }

    /// <summary>
    /// Nombre o descripción del elemento
    /// </summary>
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
}