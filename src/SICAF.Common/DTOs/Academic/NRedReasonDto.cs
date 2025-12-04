using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para motivos de N-Roja
/// </summary>
public class NRedReasonDto
{
    [Required]
    [Display(Name = "Categoría")]
    public string ReasonCategory { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Descripción")]
    public string ReasonDescription { get; set; } = string.Empty;
}