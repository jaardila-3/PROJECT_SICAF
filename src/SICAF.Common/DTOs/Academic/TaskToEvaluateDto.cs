namespace SICAF.Common.DTOs.Academic;

/// <summary>
/// DTO para una tarea regular de la misión a evaluar
/// </summary>
public class TaskToEvaluateDto
{
    public Guid MissionTaskId { get; set; }
    public Guid TaskId { get; set; }
    public int TaskCode { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public bool IsP3InThisMission { get; set; }
    public int DisplayOrder { get; set; }
}

/// <summary>
/// DTO para tareas P3 acumuladas de misiones anteriores en la misma fase
/// </summary>
public class P3TaskToEvaluateDto
{
    public Guid TaskId { get; set; }
    public int TaskCode { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string TaskDescription { get; set; } = string.Empty;
}