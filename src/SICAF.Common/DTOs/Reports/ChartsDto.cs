namespace SICAF.Common.DTOs.Reports;

public class GradeDistributionDto
{
    public int GradeA { get; set; }
    public int GradeB { get; set; }
    public int GradeC { get; set; }
    public int GradeN { get; set; }
    public int GradeNR { get; set; } // N ROJA
    public int TotalGrades { get; set; }
}

public class MachineFlightHoursDto
{
    public string AircraftRegistration { get; set; } = string.Empty;
    public string AircraftType { get; set; } = string.Empty;
    public double TotalHours { get; set; }
}

public class InstructorFlightHoursDto
{
    public string InstructorName { get; set; } = string.Empty; // Grade + Name
    public double TotalHours { get; set; }
}

public class MachineUnsatisfactoryMissionsDto
{
    public string AircraftRegistration { get; set; } = string.Empty;
    public int UnsatisfactoryCount { get; set; }
}

public class InstructorUnsatisfactoryMissionsDto
{
    public string InstructorName { get; set; } = string.Empty;
    public int UnsatisfactoryCount { get; set; }
}

public class NRedReasonsDto
{
    public List<CategoryCountDto> Categories { get; set; } = [];
    public List<ReasonDetailDto> Reasons { get; set; } = [];
}

public class CategoryCountDto
{
    public string Category { get; set; } = string.Empty; // "FACTOR MENTAL", etc.
    public int Count { get; set; }
}

public class ReasonDetailDto
{
    public string Category { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public int Count { get; set; }
}