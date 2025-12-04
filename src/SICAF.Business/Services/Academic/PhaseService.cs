using Microsoft.Extensions.Logging;

using SICAF.Business.Interfaces.Academic;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Academic;
using SICAF.Common.Models.Results;
using SICAF.Data.Entities.Academic;
using SICAF.Data.Interfaces.Repositories;

namespace SICAF.Business.Services.Academic;

public class PhaseService(
    IUnitOfWork unitOfWork,
    ILogger<PhaseService> logger
    ) : IPhaseService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<PhaseService> _logger = logger;


    public async Task<Result<StudentPhaseInfoDto>> GetPhaseByNumberAndWingTypeAsync(int phaseNumber, string wingType)
    {
        var phase = await _unitOfWork.Repository<Phase>().GetFirstAsync(p => p.PhaseNumber == phaseNumber && p.WingType == wingType);

        return phase is null
            ? Result<StudentPhaseInfoDto>.Failure(SystemErrors.PhaseError.PhaseNotFound)
            : Result<StudentPhaseInfoDto>.Success(new StudentPhaseInfoDto
            {
                Id = phase.Id,
                PhaseNumber = phase.PhaseNumber,
                Name = phase.Name,
                TotalMissions = phase.TotalMissions,
                WingType = phase.WingType,
                PrerequisitePhaseId = phase.PrerequisitePhaseId
            });
    }
}