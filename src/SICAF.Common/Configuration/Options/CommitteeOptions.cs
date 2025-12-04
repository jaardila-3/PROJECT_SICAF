namespace SICAF.Common.Configuration.Options;

public class CommitteeOptions
{
    public const string SectionName = "CommitteeSettings";
    public int MaxCommitteesPerPhase { get; set; } = 3;
    public int MaxFailedMissionsForCommittee { get; set; } = 3;
    public int LastMissionsWindow { get; set; } = 5;
    public int MaxTotalFailedMissionsInCourse { get; set; } = 15;
}