using System.ComponentModel.DataAnnotations;

namespace SICAF.Common.DTOs.Academic;

public class UpdateTasksDto
{
    [Required]
    public Guid PhaseId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Debe incluir al menos una tarea.")]
    public List<TaskUpdateDto> Tasks { get; set; } = [];
}

public class TaskUpdateDto
{
    [Required(ErrorMessage = "El ID de la tarea es requerido.")]
    public Guid TaskId { get; set; }

    [Required(ErrorMessage = "El nombre de la tarea es requerido.")]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 300 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El orden de visualización es requerido.")]
    [Range(1, 100, ErrorMessage = "El orden de visualización debe estar entre 1 y 100.")]
    public int DisplayOrder { get; set; }

    // Sistema simplificado de P3
    public bool IsP3InPhase { get; set; }

    [Range(1, 100, ErrorMessage = "El número de misión debe estar entre 1 y 100.")]
    public int? P3StartingFromMission { get; set; } // null si IsP3InPhase es false
}
