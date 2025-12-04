using SICAF.Data.Entities.Common;
using SICAF.Data.Entities.Identity;

namespace SICAF.Data.Entities.Academic;

/// <summary>
/// Entidad que representa la relación entre estudiante y curso
/// </summary>
public class UserCourse : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime AssignmentDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Tipo de participación en el curso
    /// "STUDENT", "INSTRUCTOR", "FLIGHT_LEADER"
    /// </summary>
    public string ParticipationType { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de ala que manejan
    /// (redundante con AviationProfile pero útil para consultas)
    /// </summary>
    public string? WingType { get; set; }

    /// <summary>
    /// Indica si es la inscripción activa del estudiante en el curso, no se ha retirado
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Fecha en que se desactivó (cambio de curso o desasigno)
    /// </summary>
    public DateTime? UnassignmentDate { get; set; }

    /// <summary>
    /// Razón de la desactivación
    /// </summary>
    public string? UnassignmentReason { get; set; }

    // Relaciones
    public virtual User User { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}