using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Academic;

public class AviationProfileDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    [Display(Name = "PID")]
    [Required(ErrorMessage = "El PID es obligatorio")]
    [StringLength(20, ErrorMessage = "El PID no puede exceder los 20 caracteres")]
    public string PID { get; set; } = string.Empty;

    [Display(Name = "Posición de Vuelo")]
    [Required(ErrorMessage = "La posición de vuelo es obligatoria")]
    public string FlightPosition { get; set; } = string.Empty;

    [Display(Name = "Tipo de Ala")]
    [Required(ErrorMessage = "El tipo de ala es obligatorio")]
    public string WingType { get; set; } = string.Empty;
}