using SICAF.Common.DTOs.Academic;
using SICAF.Data.Entities.Academic;

namespace SICAF.Business.Mappers.Academic;

public static class AircraftMapperExtensions
{
    public static AircraftDto MapToDto(this Aircraft aircraft)
    {
        ArgumentNullException.ThrowIfNull(aircraft);

        return new AircraftDto
        {
            Id = aircraft.Id,
            AircraftType = aircraft.AircraftType,
            Registration = aircraft.Registration,
            WingType = aircraft.WingType,
            TotalFlightHours = aircraft.TotalFlightHours
        };
    }
}