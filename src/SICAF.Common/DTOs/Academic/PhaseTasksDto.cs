namespace SICAF.Common.DTOs.Academic;

public class PhaseTasksDto
{
    public Guid PhaseId { get; set; }
    public string PhaseName { get; set; } = string.Empty;
    public int PhaseNumber { get; set; }
    public string WingType { get; set; } = string.Empty;
    public List<MissionBasicInfoDto> Missions { get; set; } = [];
    public List<TaskWithMissionsDto> Tasks { get; set; } = [];
}

public class MissionBasicInfoDto
{
    public Guid MissionId { get; set; }
    public int MissionNumber { get; set; }
    public string MissionName { get; set; } = string.Empty;
}

public class TaskWithMissionsDto
{
    public Guid TaskId { get; set; }
    public int Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TotalPhasesUsingThisTask { get; set; }
    public int TotalMissionsUsingThisTask { get; set; }

    // Información por misión en ESTA fase
    public List<TaskMissionInfoDto> MissionInfo { get; set; } = [];

    // Para el sistema de P3 simplificado
    public bool IsP3InThisPhase { get; set; }
    public int? P3StartingFromMission { get; set; } // null si no es P3
}

public class TaskMissionInfoDto
{
    public Guid MissionTaskId { get; set; }
    public Guid MissionId { get; set; }
    public int MissionNumber { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsP3Task { get; set; }
}
