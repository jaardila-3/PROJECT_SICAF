using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SICAF.Business.Interfaces.Reports;
using SICAF.Common.Constants;
using SICAF.Web.Controllers;

namespace SICAF.Web.Areas.Reports.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = $"{SystemRoles.ACADEMIC_ADMIN}, {SystemRoles.INSTRUCTOR}")]
public class ReportsApiController(
    IReportService reportService
) : BaseController
{
    private readonly IReportService _reportService = reportService;
    private const string ALL_OPTION = UserConstants.ALL_OPTION;

    [HttpGet("forces")]
    public async Task<IActionResult> GetForcesByCourse([FromQuery] Guid courseId)
    {
        try
        {
            if (courseId == Guid.Empty)
            {
                return JsonError("El ID del programa es requerido");
            }

            var forces = await _reportService.GetForcesByCourseAsync(courseId);

            var data = forces.Select(f => new
            {
                id = f.Id,
                text = f.Name
            });

            return JsonSuccess("Fuerzas obtenidas exitosamente", data);
        }
        catch (Exception ex)
        {
            return JsonError($"Error al obtener las fuerzas: {ex.Message}");
        }
    }

    [HttpGet("wing-types")]
    public async Task<IActionResult> GetWingTypesByForce([FromQuery] Guid courseId, [FromQuery] string force)
    {
        try
        {
            if (courseId == Guid.Empty)
            {
                return JsonError("El ID del programa es requerido");
            }

            if (string.IsNullOrWhiteSpace(force))
            {
                return JsonError("La fuerza es requerida");
            }

            var wingTypes = await _reportService.GetWingTypesByForceAsync(courseId, force);
            if (!string.IsNullOrEmpty(AuditContext.WingType))
            {
                wingTypes = wingTypes.Where(wt => wt.Id == AuditContext.WingType).ToList();
            }

            var data = wingTypes.Select(wt => new
            {
                id = wt.Id,
                text = wt.Name
            });

            return JsonSuccess("Tipos de ala obtenidos exitosamente", data);
        }
        catch (Exception ex)
        {
            return JsonError($"Error al obtener los tipos de ala: {ex.Message}");
        }
    }

    [HttpGet("phases")]
    public async Task<IActionResult> GetPhasesByWingType([FromQuery] string wingType)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(wingType))
            {
                return JsonError("El tipo de ala es requerido");
            }

            var phases = await _reportService.GetPhasesByWingTypeAsync(wingType);

            var data = phases.Select(p => new
            {
                id = p.Id,
                text = p.Name
            });

            return JsonSuccess("Fases obtenidas exitosamente", data);
        }
        catch (Exception ex)
        {
            return JsonError($"Error al obtener las fases: {ex.Message}");
        }
    }

    [HttpGet("students")]
    public async Task<IActionResult> GetStudents([FromQuery] Guid courseId, [FromQuery] string force = ALL_OPTION, [FromQuery] string wingType = ALL_OPTION, [FromQuery] Guid? phaseId = null)
    {
        try
        {
            if (courseId == Guid.Empty)
            {
                return JsonError("El ID del programa es requerido");
            }

            if (!string.IsNullOrEmpty(AuditContext.WingType))
            {
                wingType = AuditContext.WingType;
            }

            var students = await _reportService.GetStudentsByFiltersAsync(courseId, force, wingType, phaseId);

            var data = students.Select(s => new
            {
                id = s.Id,
                text = s.Name
            });

            return JsonSuccess("Estudiantes obtenidos exitosamente", data);
        }
        catch (Exception ex)
        {
            return JsonError($"Error al obtener los estudiantes: {ex.Message}");
        }
    }

}