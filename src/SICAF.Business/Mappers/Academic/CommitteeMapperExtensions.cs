using SICAF.Common.DTOs.Academic;
using SICAF.Data.Entities.Academic;

namespace SICAF.Business.Mappers.Academic;

public static class CommitteeMapperExtensions
{
    public static StudentCommitteeRecordDto MapToDto(this StudentCommitteeRecord comitte)
    {
        ArgumentNullException.ThrowIfNull(comitte);

        return new StudentCommitteeRecordDto
        {
            Id = comitte.Id,
            CourseId = comitte.CourseId,
            PhaseId = comitte.PhaseId,
            StudentId = comitte.StudentId,
            CommitteeNumber = comitte.CommitteeNumber,
            Date = comitte.Date,
            ActaNumber = comitte.ActaNumber,
            Reason = comitte.Reason,
            Decision = comitte.Decision,
            DecisionDate = comitte.DecisionDate,
            LeaderId = comitte.LeaderId,
            DecisionObservations = comitte.DecisionObservations,
            IsResolved = comitte.IsResolved
        };
    }
}