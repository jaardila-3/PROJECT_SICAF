using System.ComponentModel.DataAnnotations;

using SICAF.Common.DTOs.Common;

namespace SICAF.Common.DTOs.Identity;

public class RegisterDto : UserBase
{
    [Display(Name = "Roles")]
    [Required(ErrorMessage = "Debe seleccionar al menos un rol")]
    public List<Guid> SelectedRoleIds { get; set; } = [];

    /// <summary>
    /// PID - Solo requerido para roles de aviación
    /// </summary>
    [Display(Name = "PID")]
    public string? PID { get; set; }

    /// <summary>
    /// Posición de vuelo - Solo requerido para roles de aviación
    /// </summary>
    [Display(Name = "Posición de Vuelo")]
    public string? FlightPosition { get; set; }

    /// <summary>
    /// Tipo de Ala - Solo requerido para roles de aviación
    /// </summary>
    [Display(Name = "Tipo de Ala")]
    public string? WingType { get; set; }

    /// <summary>
    /// programa al que se inscribirá el estudiante - Requerido si el rol es Estudiante
    /// </summary>
    [Display(Name = "Programa")]
    public Guid? CourseId { get; set; }
}