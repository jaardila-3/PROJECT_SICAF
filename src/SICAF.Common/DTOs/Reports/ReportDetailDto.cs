namespace SICAF.Common.DTOs.Reports;

/// <summary>
/// DTO base para vistas de detalle de gráficos
/// </summary>
public class ReportDetailDto
{
    public string ReportType { get; set; } = string.Empty; // "grades", "machine-hours", etc.
    public string SelectedValue { get; set; } = string.Empty; // "A", "FAC-1234", etc.
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;

    // Filtros aplicados
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string? Force { get; set; }
    public string? WingType { get; set; }
    public Guid? PhaseId { get; set; }
    public string? PhaseName { get; set; }
}

/// <summary>
/// Detalle de distribución de calificaciones
/// </summary>
public class GradeDistributionDetailDto : ReportDetailDto
{
    public List<GradeDetailRecordDto> Records { get; set; } = [];
}

public class GradeDetailRecordDto
{
    public int Number { get; set; }
    public string StudentName { get; set; } = string.Empty; // Grado + Nombre
    public string TaskName { get; set; } = string.Empty;
    public string MissionPhaseName { get; set; } = string.Empty;
    public string AircraftRegistration { get; set; } = string.Empty;
    public DateTime EvaluationDate { get; set; }
    public string InstructorName { get; set; } = string.Empty; // Grado + Nombre
}

/// <summary>
/// Detalle de horas de vuelo por máquina
/// </summary>
public class MachineFlightHoursDetailDto : ReportDetailDto
{
    public List<MachineFlightDetailRecordDto> Records { get; set; } = [];
    public double TotalHours { get; set; }
}

public class MachineFlightDetailRecordDto
{
    public int Number { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string MissionPhaseName { get; set; } = string.Empty;
    public DateTime FlightDate { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public double FlightHours { get; set; }
}

/// <summary>
/// Detalle de horas de vuelo por instructor
/// </summary>
public class InstructorFlightHoursDetailDto : ReportDetailDto
{
    public List<InstructorFlightDetailRecordDto> Records { get; set; } = [];
    public double TotalHours { get; set; }
}

public class InstructorFlightDetailRecordDto
{
    public int Number { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string MissionPhaseName { get; set; } = string.Empty;
    public string AircraftRegistration { get; set; } = string.Empty;
    public double FlightHours { get; set; }
    public DateTime FlightDate { get; set; }
}

/// <summary>
/// Detalle de misiones insatisfactorias por máquina
/// </summary>
public class MachineUnsatisfactoryDetailDto : ReportDetailDto
{
    public List<UnsatisfactoryDetailRecordDto> Records { get; set; } = [];
}

public class UnsatisfactoryDetailRecordDto
{
    public int Number { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string MissionPhaseName { get; set; } = string.Empty;
    public DateTime EvaluationDate { get; set; }
    public string InstructorOrAircraft { get; set; } = string.Empty; // Instructor o Aeronave según tipo

    // Para enlace a MissionEvaluationDetail
    public Guid MissionId { get; set; }
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
}

/// <summary>
/// Detalle de misiones insatisfactorias por instructor
/// </summary>
public class InstructorUnsatisfactoryDetailDto : ReportDetailDto
{
    public List<UnsatisfactoryDetailRecordDto> Records { get; set; } = [];
}

/// <summary>
/// Detalle de N ROJA por categorías
/// </summary>
public class NRedCategoriesDetailDto : ReportDetailDto
{
    public List<NRedDetailRecordDto> Records { get; set; } = [];
}

public class NRedDetailRecordDto
{
    public int Number { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string TaskName { get; set; } = string.Empty;
    public string MissionPhaseName { get; set; } = string.Empty;
    public string AircraftRegistration { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public DateTime EvaluationDate { get; set; }

    // Para enlace a MissionEvaluationDetail
    public Guid MissionId { get; set; }
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
}
