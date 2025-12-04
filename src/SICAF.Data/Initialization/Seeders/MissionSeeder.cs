using Microsoft.Extensions.Logging;

using SICAF.Data.Entities.Academic;
using SICAF.Data.Interfaces.Repositories;
using SICAF.Data.Interfaces.Seeders;

using SICAF.Common.Constants;

namespace SICAF.Data.Initialization.Seeders;

public class MissionSeeder(IUnitOfWork unitOfWork, ILogger<MissionSeeder> logger) : IDataSeeder
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<MissionSeeder> _logger = logger;
    public int Priority => 5;

    public async Task SeedAsync()
    {
        _logger.LogInformation("Iniciando seeding de misiones...");

        var phases = await _unitOfWork.Repository<Phase>().GetListAsync();

        foreach (var phase in phases)
        {
            _logger.LogInformation("Procesando misiones para fase {PhaseName} ({WingType}) - Total misiones: {TotalMissions}",
            phase.Name, phase.WingType, phase.TotalMissions);

            await SeedMissionsForPhaseAsync(phase);
        }

        _logger.LogInformation("Seeding de misiones completado.");
    }

    private async Task SeedMissionsForPhaseAsync(Phase phase)
    {
        // Obtener TODAS las misiones existentes
        var existingMissionNumbers = (await _unitOfWork.Repository<Mission>()
            .GetListAsync(m => m.PhaseId == phase.Id))
            .Select(m => m.MissionNumber)
            .ToHashSet();

        _logger.LogDebug("Fase {PhaseName} tiene {Count} misiones existentes.", phase.Name, existingMissionNumbers.Count);

        var newMissions = new List<Mission>();

        for (int i = 1; i <= phase.TotalMissions; i++)
        {
            // Verificar si la misión ya existe
            if (!existingMissionNumbers.Contains(i))
            {
                var mission = new Mission
                {
                    PhaseId = phase.Id,
                    Name = $"Misión {i}",
                    MissionNumber = i,
                    FlightHours = GetFlightHoursForMission(phase.WingType, phase.PhaseNumber, i),
                    WingType = phase.WingType
                };

                newMissions.Add(mission);

                _logger.LogDebug("Misión {MissionNumber} preparada para fase {PhaseName}", i, phase.Name);
            }
            else
            {
                _logger.LogDebug("Misión {MissionNumber} ya existe para fase {PhaseName}", i, phase.Name);
            }
        }

        // Agregar todas las misiones nuevas al UnitOfWork
        if (newMissions.Count != 0)
        {
            await _unitOfWork.Repository<Mission>().AddRangeAsync(newMissions);
            _logger.LogInformation("{Count} misiones nuevas agregadas para fase {PhaseName}", newMissions.Count, phase.Name);
        }

        // Guardar cambios por cada fase procesada
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Misiones guardadas exitosamente para fase {PhaseName}", phase.Name);
    }

    private static double GetFlightHoursForMission(string wingType, int phaseNumber, int missionNumber)
    {
        // Definir las horas de vuelo según el tipo de ala, fase y número de misión
        return wingType switch
        {
            AviationConstants.WingTypes.FIXED_WING => GetFixedWingFlightHours(phaseNumber, missionNumber),
            AviationConstants.WingTypes.ROTARY_WING => GetRotaryWingFlightHours(phaseNumber, missionNumber),
            _ => 1.0 // Valor por defecto
        };
    }

    private static double GetFixedWingFlightHours(int phaseNumber, int missionNumber)
    {
        return phaseNumber switch
        {
            1 => 1.0, // Fase pre-solo: todas las misiones 1 hora (default)

            2 => missionNumber switch // Fase maniobras
            {
                20 => 2.0,
                39 => 2.0,
                40 => 2.0,
                _ => 1.0 // Default para las demás misiones de la fase 2
            },

            3 => missionNumber switch // Fase instrumentos
            {
                1 => 1.5,
                2 => 1.5,
                >= 3 and <= 17 => 2.0, // Misiones 3 hasta 17
                18 => 3.0,
                19 => 3.0,
                _ => 1.0 // Default
            },

            4 => missionNumber switch // Fase cruceros
            {
                1 => 4.0,
                >= 2 and <= 5 => 3.5, // Misiones 2 hasta 5
                _ => 1.0 // Default
            },

            _ => 1.0 // Default
        };
    }

    private static double GetRotaryWingFlightHours(int phaseNumber, int missionNumber)
    {
        // Lógica específica para ala rotatoria
        return phaseNumber switch
        {
            1 => 1.0, // Fase pre-solo: 1 hora por misión
            2 => missionNumber switch // Fase vuelo básico
            {
                >= 1 and <= 36 => 1.3, // Misiones 1 hasta 36
                37 => 2.0,
                _ => 1.0 // Default
            },
            3 => missionNumber switch // Fase vuelo táctico
            {
                >= 1 and <= 7 => 1.2, // Misiones 1 hasta 7
                8 => 1.6,
                _ => 1.0 // Default
            },
            4 => 1.5, // Fase instrumentos: 1.5 horas por misión
            5 => missionNumber switch // Fase de cruceros
            {
                1 => 4.0,
                2 => 2.0,
                3 => 4.0,
                _ => 1.0 // Default
            },
            _ => 1.0 // Default
        };
    }
}