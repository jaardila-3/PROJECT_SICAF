namespace SICAF.Common.DTOs.Reports;

public class IndividualReportDto
{
    // Información Personal del Estudiante
    public Guid StudentId { get; set; }
    public string? PhotoBase64 { get; set; }
    public string Grade { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string IdentificationType { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string PID { get; set; } = string.Empty;
    public string WingType { get; set; } = string.Empty;
    public string Force { get; set; } = string.Empty;

    // Información del Curso
    public string CourseName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StudentStatus { get; set; } = string.Empty; // Activo, Suspendido, Graduado
    public string StudentStatusDisplay { get; set; } = string.Empty; // Para mostrar en la vista

    // Información de Fase (cuando se filtra por fase)
    public Guid? PhaseId { get; set; }
    public Guid? CourseId { get; set; }
    public string? PhaseName { get; set; }
    public string? PhaseStatus { get; set; } // Estado del estudiante en esa fase: "Fase Actual", "Completada", "Suspendido", "Graduado"

    // Desempeño Académico
    public GradeDistributionDto GradeDistribution { get; set; } = new();
    public double Average { get; set; }

    // Progreso en el Curso
    public string? CurrentPhase { get; set; }
    public int CurrentPhaseMissionsCompleted { get; set; }
    public int CurrentPhaseMissionsPending { get; set; }
    public int CurrentPhaseTotalMissions { get; set; }
    public double CurrentPhaseProgressPercentage { get; set; }

    public double TotalFlightHours { get; set; }
    public int NonEvaluableMissionRecords { get; set; }
    public int PhasesCompleted { get; set; }
    public int TotalMissionsCompleted { get; set; }
    public int TotalMissionsInCourse { get; set; }
    public double TotalCourseProgressPercentage { get; set; }

    // Tablas
    public List<UnsatisfactoryMissionDto> UnsatisfactoryMissions { get; set; } = [];
    public List<AcademicCommitteeDto> AcademicCommittees { get; set; } = [];

    /// <summary>
    /// Historial de fases con sus misiones completadas (evaluables y no evaluables)
    /// </summary>
    public List<PhaseWithMissionsDto> PhaseHistory { get; set; } = [];
}

public class UnsatisfactoryMissionDto
{
    public string Phase { get; set; } = string.Empty;
    public string Mission { get; set; } = string.Empty;
    public List<string> TasksWithNRed { get; set; } = [];
    public DateTime Date { get; set; }
    public string InstructorGradeAndName { get; set; } = string.Empty;
    public string AircraftRegistration { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;
}

public class AcademicCommitteeDto
{
    public string ActNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Decision { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;
}