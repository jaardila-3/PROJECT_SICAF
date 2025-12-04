using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para asignar múltiples instructores
/// </summary>
public class AssignInstructorsDto
{
    [Required(ErrorMessage = "El programa es requerido")]
    public Guid CourseId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar al menos un instructor")]
    public List<Guid> InstructorIds { get; set; } = [];

    // Campo para designar al líder de vuelo de ala fija
    [Display(Name = "Líder de Vuelo - Ala Fija")]
    public Guid? FlightLeaderFixedWingId { get; set; }

    // Campo para designar al líder de vuelo de ala rotatoria
    [Display(Name = "Líder de Vuelo - Ala Rotatoria")]
    public Guid? FlightLeaderRotaryWingId { get; set; }
}