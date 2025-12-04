using Microsoft.Extensions.Logging;

using SICAF.Data.Entities.Academic;
using SICAF.Data.Interfaces.Repositories;
using SICAF.Data.Interfaces.Seeders;

using SICAF.Common.Constants;

namespace SICAF.Data.Initialization.Seeders;

public class PhaseSeeder(IUnitOfWork unitOfWork, ILogger<PhaseSeeder> logger) : IDataSeeder
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<PhaseSeeder> _logger = logger;
    public int Priority => 4;

    public async Task SeedAsync()
    {
        _logger.LogInformation("Iniciando seeding de fases...");

        var phases = new List<Phase>();
        phases.AddRange(GetPhasesForFixedWing());
        phases.AddRange(GetPhasesForRotaryWing());

        foreach (var phase in phases)
        {
            await SeedPhaseAsync(phase);
        }

        _logger.LogInformation("Seeding de fases completado.");
    }

    private static List<Phase> GetPhasesForFixedWing()
    {
        var phase1 = new Phase()
        {
            Name = "FASE DE PRE-SOLO",
            PhaseNumber = 1,
            TotalMissions = 21,
            WingType = AviationConstants.WingTypes.FIXED_WING,
        };

        var phase2 = new Phase()
        {
            Name = "FASE DE MANIOBRAS",
            PhaseNumber = 2,
            TotalMissions = 40,
            WingType = AviationConstants.WingTypes.FIXED_WING,
            PrerequisitePhaseId = phase1.Id
        };

        var phase3 = new Phase()
        {
            Name = "FASE DE INSTRUMENTOS",
            PhaseNumber = 3,
            TotalMissions = 19,
            WingType = AviationConstants.WingTypes.FIXED_WING,
            PrerequisitePhaseId = phase2.Id
        };

        var phase4 = new Phase()
        {
            Name = "FASE DE CRUCEROS",
            PhaseNumber = 4,
            TotalMissions = 5,
            WingType = AviationConstants.WingTypes.FIXED_WING,
            PrerequisitePhaseId = phase3.Id
        };

        return
        [
            phase1,
            phase2,
            phase3,
            phase4
        ];
    }

    private static List<Phase> GetPhasesForRotaryWing()
    {
        var phase1 = new Phase()
        {
            Name = "FASE DE PRE-SOLO",
            PhaseNumber = 1,
            TotalMissions = 22,
            WingType = AviationConstants.WingTypes.ROTARY_WING,
        };

        var phase2 = new Phase()
        {
            Name = "FASE DE VUELO BÁSICO",
            PhaseNumber = 2,
            TotalMissions = 37,
            WingType = AviationConstants.WingTypes.ROTARY_WING,
            PrerequisitePhaseId = phase1.Id
        };

        var phase3 = new Phase()
        {
            Name = "FASE DE VUELO TÁCTICO",
            PhaseNumber = 3,
            TotalMissions = 8,
            WingType = AviationConstants.WingTypes.ROTARY_WING,
            PrerequisitePhaseId = phase2.Id
        };

        var phase4 = new Phase()
        {
            Name = "FASE DE INSTRUMENTOS",
            PhaseNumber = 4,
            TotalMissions = 20,
            WingType = AviationConstants.WingTypes.ROTARY_WING,
            PrerequisitePhaseId = phase3.Id
        };

        var phase5 = new Phase()
        {
            Name = "FASE DE CRUCEROS",
            PhaseNumber = 5,
            TotalMissions = 3,
            WingType = AviationConstants.WingTypes.ROTARY_WING,
            PrerequisitePhaseId = phase4.Id
        };

        return
        [
            phase1,
            phase2,
            phase3,
            phase4,
            phase5
        ];
    }

    private async Task SeedPhaseAsync(Phase phase)
    {
        var existingPhase = await _unitOfWork.Repository<Phase>().GetFirstAsync(p => p.PhaseNumber == phase.PhaseNumber && p.WingType == phase.WingType);

        if (existingPhase == null)
        {
            await _unitOfWork.Repository<Phase>().AddAsync(phase);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Fase {PhaseName} creada exitosamente.", phase.Name);
        }
        else
        {
            _logger.LogInformation("Fase {PhaseName} ya existe.", phase.Name);
        }
    }
}