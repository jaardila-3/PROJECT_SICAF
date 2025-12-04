using SICAF.Data.Entities.Common;

namespace SICAF.Data.Entities.Academic;

/// <summary>
/// Entidad que representa una fase del programa de aviación
/// Las fases son predefinidas y no modificables por los usuarios
/// </summary>
public class Phase : BaseEntity
{
    // Propiedades básicas
    public string Name { get; set; } = string.Empty;
    public int PhaseNumber { get; set; }
    public int TotalMissions { get; set; }
    public string WingType { get; set; } = string.Empty;

    // Prerrequisitos - Una fase puede requerir completar otra fase antes
    public Guid? PrerequisitePhaseId { get; set; }
    public virtual Phase? PrerequisitePhase { get; set; }

    // Fases que dependen de esta (inversa de prerrequisito)
    public virtual ICollection<Phase> DependentPhases { get; set; } = [];

    // Relaciones
    public virtual ICollection<Mission> Missions { get; set; } = [];
    public virtual ICollection<StudentPhaseProgress> StudentPhaseProgresses { get; set; } = [];

}