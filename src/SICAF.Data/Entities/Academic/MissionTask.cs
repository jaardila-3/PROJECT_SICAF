using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Academic;

public class MissionTask : BaseEntity
{
    public Guid MissionId { get; set; }
    public Guid TaskId { get; set; }

    // Indica si en ESTE periodo específico, ESTA tarea es P3
    public bool IsP3Task { get; set; } = false;
    public int DisplayOrder { get; set; }

    // Relaciones
    public virtual Mission Mission { get; set; } = null!;
    public virtual Tasks Task { get; set; } = null!;
}