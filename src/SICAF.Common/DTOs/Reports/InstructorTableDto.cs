namespace SICAF.Common.DTOs.Reports;

public class InstructorTableDto
{
    public Guid Id { get; set; }
    public int Number { get; set; }
    public string Grade { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PID { get; set; } = string.Empty;
    public string WingType { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
