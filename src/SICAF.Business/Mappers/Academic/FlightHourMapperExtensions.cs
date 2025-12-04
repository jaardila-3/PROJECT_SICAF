using SICAF.Common.Constants;
using SICAF.Common.DTOs.Instructor;
using SICAF.Common.Helpers;
using SICAF.Data.Entities.Academic;

namespace SICAF.Business.Mappers.Academic;

public static class FlightHourMapperExtensions
{
    public static FlightHourLog MapToEntity(this SaveMissionEvaluationDto evaluationDto, double machineHours, bool isStudent = true)
    {
        ArgumentNullException.ThrowIfNull(evaluationDto);

        return new FlightHourLog
        {
            UserId = isStudent ? evaluationDto.StudentId : evaluationDto.InstructorId,
            CourseId = evaluationDto.CourseId,
            MissionId = evaluationDto.ViewType == AviationConstants.NonEvaluableMission ? null : evaluationDto.MissionId,
            NonEvaluableMissionId = evaluationDto.ViewType == AviationConstants.NonEvaluableMission ? evaluationDto.MissionId : null,
            AircraftId = evaluationDto.AircraftId,
            Date = evaluationDto.EvaluationDate,
            ManFlightHours = Math.Round(machineHours * 1.3, 1),
            MachineFlightHours = machineHours,
            SilaboFlightHours = ColombianHolidayHelper.IsWorkingDay(evaluationDto.EvaluationDate) ? 1 : 0,
            Role = isStudent ? SystemRoles.STUDENT : SystemRoles.INSTRUCTOR,
            Observations = evaluationDto.GeneralObservations
        };
    }
}