namespace SICAF.Common.DTOs.Instructor;

public class NonEvaluableMissionDto
{
    public Guid MissionId { get; set; }
    public Guid CourseId { get; set; }
    public int MissionNumber { get; set; } // Para mostrar MNE1, MNE2...
    public string DisplayName => $"MNE{MissionNumber}"; // MNE1, MNE2...
    public DateTime Date { get; set; }
    public double ManFlightHours { get; set; }
    public string Observations { get; set; } = string.Empty;
    public Guid InstructorId { get; set; }
    public bool CanEdit { get; set; } // Mismas reglas de edición (2 días, 2 ediciones)
}