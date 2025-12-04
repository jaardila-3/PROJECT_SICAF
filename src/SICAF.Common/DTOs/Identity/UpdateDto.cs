using System.ComponentModel.DataAnnotations;

using SICAF.Common.DTOs.Common;

namespace SICAF.Common.DTOs.Identity;

public class UpdateDto : UserBase
{
    public Guid Id { get; set; }

    [Display(Name = "Roles")]
    [Required(ErrorMessage = "Debe seleccionar al menos un rol")]
    public List<Guid> SelectedRoleIds { get; set; } = [];

    /// <summary>
    /// Indica la acción deseada: true = bloquear, false = desbloquear
    /// Esta es diferente a IsLockedOut que indica el estado actual
    /// </summary>
    [Display(Name = "Acción de Bloqueo")]
    public bool WantToLock { get; set; }

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
    /// programa al que se inscribirá el estudiante - Requerido si el rol es Estudiante y no tiene programa activo
    /// </summary>
    [Display(Name = "Programa")]
    public Guid? CourseId { get; set; }
}