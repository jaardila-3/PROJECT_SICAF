using SICAF.Common.DTOs.Academic;
using SICAF.Common.Models.Results;

namespace SICAF.Business.Interfaces.Academic;

public interface IPhaseService
{
    Task<Result<StudentPhaseInfoDto>> GetPhaseByNumberAndWingTypeAsync(int phaseNumber, string wingType);
}