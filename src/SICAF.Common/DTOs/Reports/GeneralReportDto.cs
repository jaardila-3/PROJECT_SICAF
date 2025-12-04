namespace SICAF.Common.DTOs.Reports;

public class GeneralReportDto
{
    // Información del curso
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Force { get; set; } = string.Empty;
    public string WingType { get; set; } = string.Empty;
    public Guid? PhaseId { get; set; }
    public string? PhaseName { get; set; }

    // Estadísticas generales
    public int TotalStudents { get; set; }
    public int ActiveStudents { get; set; }
    public int SuspendedStudents { get; set; }
    public int TotalCommittees { get; set; }

    // Tabla de instructores y líderes de vuelo
    public List<InstructorTableDto> InstructorsTable { get; set; } = [];

    // Tabla de estudiantes
    public List<StudentReportDto> Students { get; set; } = [];

    // Datos para gráficas
    public GradeDistributionDto? GradeDistribution { get; set; }
    public List<MachineFlightHoursDto> MachineFlightHours { get; set; } = [];
    public List<InstructorFlightHoursDto> InstructorFlightHours { get; set; } = [];
    public List<MachineUnsatisfactoryMissionsDto> MachineUnsatisfactoryMissions { get; set; } = [];
    public List<InstructorUnsatisfactoryMissionsDto> InstructorUnsatisfactoryMissions { get; set; } = [];
    public NRedReasonsDto? NRedReasons { get; set; }
}