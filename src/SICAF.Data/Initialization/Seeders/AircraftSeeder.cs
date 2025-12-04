using Microsoft.Extensions.Logging;

using SICAF.Common.Constants;
using SICAF.Data.Entities.Academic;
using SICAF.Data.Interfaces.Repositories;
using SICAF.Data.Interfaces.Seeders;

namespace SICAF.Data.Initialization.Seeders;

public class AircraftSeeder(IUnitOfWork unitOfWork, ILogger<AircraftSeeder> logger) : IDataSeeder
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<AircraftSeeder> _logger = logger;

    public int Priority => 8;

    public async Task SeedAsync()
    {
        _logger.LogInformation("Iniciando seeding de aeronaves...");

        var aircrafts = GetDefaultAircrafts();

        foreach (var aircraft in aircrafts)
        {
            await SeedAircraftAsync(aircraft);
        }

        _logger.LogInformation("Seeding de aeronaves completado.");
    }

    #region Métodos Privados
    private static List<Aircraft> GetDefaultAircrafts()
    {
        return
        [
            // Aviones (Ala Fija)
            new()
            {
                AircraftType = AviationConstants.AircraftTypes.AIRPLANE,
                Registration = "PNC0269",
                WingType = AviationConstants.WingTypes.FIXED_WING,
                TotalFlightHours = 0d
            },
            new()
            {
                AircraftType = AviationConstants.AircraftTypes.AIRPLANE,
                Registration = "SIMULADOR",
                WingType = AviationConstants.WingTypes.FIXED_WING,
                TotalFlightHours = 0d
            },

            // Helicópteros (Ala Rotatoria)
            new()
            {
                AircraftType = AviationConstants.AircraftTypes.HELICOPTER,
                Registration = "PNC0905",
                WingType = AviationConstants.WingTypes.ROTARY_WING,
                TotalFlightHours = 0d
            },
            new()
            {
                AircraftType = AviationConstants.AircraftTypes.HELICOPTER,
                Registration = "PNC0906",
                WingType = AviationConstants.WingTypes.ROTARY_WING,
                TotalFlightHours = 0d
            },
            new()
            {
                AircraftType = AviationConstants.AircraftTypes.HELICOPTER,
                Registration = "PNC0907",
                WingType = AviationConstants.WingTypes.ROTARY_WING,
                TotalFlightHours = 0d
            },
            new()
            {
                AircraftType = AviationConstants.AircraftTypes.HELICOPTER,
                Registration = "SIMULADOR",
                WingType = AviationConstants.WingTypes.ROTARY_WING,
                TotalFlightHours = 0d
            }
        ];
    }

    private async Task SeedAircraftAsync(Aircraft aircraft)
    {
        var existingAircraft = await _unitOfWork.Repository<Aircraft>()
            .GetFirstAsync(a => a.Registration.Equals(aircraft.Registration) && a.WingType.Equals(aircraft.WingType));

        if (existingAircraft == null)
        {
            await _unitOfWork.Repository<Aircraft>().AddAsync(aircraft);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Aeronave {Registration} ({AircraftType}) creada exitosamente.", aircraft.Registration, aircraft.AircraftType);
        }
        else
        {
            _logger.LogInformation("Aeronave {Registration} ya existe.", aircraft.Registration);
        }
    }
    #endregion
}