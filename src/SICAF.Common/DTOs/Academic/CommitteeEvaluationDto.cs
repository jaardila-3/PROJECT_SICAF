namespace SICAF.Common.DTOs.Academic;

public class CommitteeEvaluationDto
{
    public bool RequiresCommittee { get; set; }
    public bool RequiresSuspension { get; set; }
    public string? Reason { get; set; }
    public int CommitteeNumber { get; set; }
}