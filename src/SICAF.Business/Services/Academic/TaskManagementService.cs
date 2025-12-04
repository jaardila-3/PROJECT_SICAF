using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SICAF.Business.Interfaces.Academic;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Academic;
using SICAF.Common.Models.Results;
using SICAF.Data.Entities.Academic;
using SICAF.Data.Interfaces.Repositories;

namespace SICAF.Business.Services.Academic;

public class TaskManagementService(
    IUnitOfWork unitOfWork,
    ILogger<TaskManagementService> logger
) : ITaskManagementService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<TaskManagementService> _logger = logger;

    public async Task<Result<List<PhaseBasicDto>>> GetPhasesAsync(string? wingType = null)
    {
        Expression<Func<Phase, bool>>? predicate = null;
        if (!string.IsNullOrEmpty(wingType))
            predicate = p => p.WingType == wingType;

        var phases = await _unitOfWork.Repository<Phase>()
            .GetListAsync(
                predicate,
                orderBy: q => q.OrderBy(p => p.PhaseNumber),
                includeFunc: q => q.Include(p => p.Missions).ThenInclude(m => m.MissionTasks)
            );

        var phaseDtos = phases.Select(p => new PhaseBasicDto
        {
            PhaseId = p.Id,
            PhaseName = p.Name,
            PhaseNumber = p.PhaseNumber,
            WingType = p.WingType,
            TotalMissions = p.TotalMissions,
            TotalTasks = p.Missions
                .SelectMany(m => m.MissionTasks)
                .Select(mt => mt.TaskId)
                .Distinct()
                .Count()
        }).ToList();

        return Result<List<PhaseBasicDto>>.Success(phaseDtos);
    }

    public async Task<Result<PhaseTasksDto>> GetPhaseTasksAsync(Guid phaseId)
    {
        var phase = await _unitOfWork.Repository<Phase>()
            .GetFirstAsync(
                p => p.Id == phaseId,
                includeFunc: q => q
                    .Include(p => p.Missions.OrderBy(m => m.MissionNumber))
                        .ThenInclude(m => m.MissionTasks)
                            .ThenInclude(mt => mt.Task)
            );

        if (phase == null)
            return Result<PhaseTasksDto>.Failure(SystemErrors.PhaseError.PhaseNotFound);

        // Obtener todas las tareas únicas de la fase
        var allMissionTasks = phase.Missions
            .SelectMany(m => m.MissionTasks)
            .ToList();

        var uniqueTasks = allMissionTasks
            .GroupBy(mt => mt.TaskId)
            .Select(g => g.First())
            .OrderBy(mt => mt.DisplayOrder)
            .ThenBy(mt => mt.Task.Name)
            .ToList();

        var tasks = new List<TaskWithMissionsDto>();

        foreach (var mt in uniqueTasks)
        {
            // Obtener info de uso global de esta tarea
            var usageInfo = await GetTaskUsageInfoAsync(mt.TaskId);

            // Obtener info de misiones en esta fase
            var missionInfoList = allMissionTasks
                .Where(x => x.TaskId == mt.TaskId)
                .OrderBy(x => x.Mission.MissionNumber)
                .Select(x => new TaskMissionInfoDto
                {
                    MissionTaskId = x.Id,
                    MissionId = x.MissionId,
                    MissionNumber = x.Mission.MissionNumber,
                    DisplayOrder = x.DisplayOrder,
                    IsP3Task = x.IsP3Task
                }).ToList();

            // Determinar si es P3 y desde qué misión
            var p3Missions = missionInfoList.Where(mi => mi.IsP3Task).ToList();
            bool isP3InPhase = p3Missions.Count != 0;
            int? p3StartingMission = isP3InPhase
                ? p3Missions.Min(mi => mi.MissionNumber)
                : null;

            tasks.Add(new TaskWithMissionsDto
            {
                TaskId = mt.TaskId,
                Code = mt.Task.Code,
                Name = mt.Task.Name,
                TotalPhasesUsingThisTask = usageInfo.IsSuccess ? usageInfo.Value.TotalPhases : 0,
                TotalMissionsUsingThisTask = usageInfo.IsSuccess ? usageInfo.Value.TotalMissions : 0,
                MissionInfo = missionInfoList,
                IsP3InThisPhase = isP3InPhase,
                P3StartingFromMission = p3StartingMission
            });
        }

        var dto = new PhaseTasksDto
        {
            PhaseId = phase.Id,
            PhaseName = phase.Name,
            PhaseNumber = phase.PhaseNumber,
            WingType = phase.WingType,
            Missions = phase.Missions
                .OrderBy(m => m.MissionNumber)
                .Select(m => new MissionBasicInfoDto
                {
                    MissionId = m.Id,
                    MissionNumber = m.MissionNumber,
                    MissionName = m.Name
                }).ToList(),
            Tasks = tasks
        };

        return Result<PhaseTasksDto>.Success(dto);
    }

    public async Task<Result<bool>> UpdatePhaseTasksAsync(UpdateTasksDto dto)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Validar DisplayOrder
            var validationResult = await ValidateDisplayOrdersAsync(dto.PhaseId, dto.Tasks);
            if (!validationResult.IsSuccess)
                return validationResult;

            // Obtener todas las misiones de la fase
            var missions = await _unitOfWork.Repository<Mission>()
                .GetListAsync(m => m.PhaseId == dto.PhaseId, orderBy: q => q.OrderBy(m => m.MissionNumber));

            var missionIds = missions.Select(m => m.Id).ToHashSet();

            // Actualizar cada tarea
            foreach (var taskUpdate in dto.Tasks)
            {
                // Actualizar nombre de la tarea (afecta TODAS las fases)
                var task = await _unitOfWork.Repository<Tasks>().GetByIdAsync(taskUpdate.TaskId);
                if (task != null && task.Name != taskUpdate.Name)
                {
                    task.Name = taskUpdate.Name;
                    _unitOfWork.Repository<Tasks>().Update(task);
                }

                // Actualizar MissionTasks de esta fase
                var missionTasksForThisTask = await _unitOfWork.Repository<MissionTask>()
                    .GetListAsync(mt => missionIds.Contains(mt.MissionId) && mt.TaskId == taskUpdate.TaskId);

                foreach (var missionTask in missionTasksForThisTask)
                {
                    // Actualizar DisplayOrder (igual para todas las misiones de la fase)
                    missionTask.DisplayOrder = taskUpdate.DisplayOrder;

                    // Actualizar IsP3Task para que sea true a partir de la misión indicada
                    var mission = missions.First(m => m.Id == missionTask.MissionId);
                    missionTask.IsP3Task = taskUpdate.IsP3InPhase && taskUpdate.P3StartingFromMission.HasValue && mission.MissionNumber >= taskUpdate.P3StartingFromMission.Value;

                    _unitOfWork.Repository<MissionTask>().Update(missionTask);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Tareas actualizadas exitosamente para fase {PhaseId}", dto.PhaseId);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error al actualizar tareas de la fase {PhaseId}", dto.PhaseId);
            return Result<bool>.Failure("UPDATE_ERROR", "Error al actualizar las tareas");
        }
    }

    /// <summary>
    /// Obtiene información de uso de una tarea (cuántas fases/misiones la usan)
    /// </summary>
    private async Task<Result<(int TotalPhases, int TotalMissions)>> GetTaskUsageInfoAsync(Guid taskId)
    {
        var missionTasks = await _unitOfWork.Repository<MissionTask>()
            .GetListAsync(
                mt => mt.TaskId == taskId,
                includeFunc: q => q.Include(mt => mt.Mission).ThenInclude(m => m.Phase)
            );

        var totalPhases = missionTasks
            .Select(mt => mt.Mission.PhaseId)
            .Distinct()
            .Count();

        var totalMissions = missionTasks.Count;

        return Result<(int, int)>.Success((totalPhases, totalMissions));
    }

    /// <summary>
    /// Valida que no haya DisplayOrder duplicados en una misma misión
    /// </summary>
    private async Task<Result<bool>> ValidateDisplayOrdersAsync(Guid phaseId, List<TaskUpdateDto> tasks)
    {
        var missions = await _unitOfWork.Repository<Mission>()
            .GetListAsync(m => m.PhaseId == phaseId);

        var missionIds = missions.Select(m => m.Id).ToHashSet();

        // Obtener todos los MissionTask de esta fase
        var allMissionTasks = await _unitOfWork.Repository<MissionTask>()
            .GetListAsync(mt => missionIds.Contains(mt.MissionId));

        // Crear un diccionario para mapear TaskId -> nuevo DisplayOrder
        var newOrders = tasks.ToDictionary(t => t.TaskId, t => t.DisplayOrder);

        // Validar por cada misión
        foreach (var mission in missions)
        {
            var tasksInMission = allMissionTasks
                .Where(mt => mt.MissionId == mission.Id)
                .Select(mt => new
                {
                    mt.TaskId,
                    DisplayOrder = newOrders.TryGetValue(mt.TaskId, out int value) ? value : mt.DisplayOrder
                })
                .ToList();

            var duplicates = tasksInMission
                .GroupBy(t => t.DisplayOrder)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicates.Count != 0)
            {
                return Result<bool>.Failure(
                    "DUPLICATE_DISPLAY_ORDER",
                    $"Hay tareas con numeros de orden duplicados en la misión {mission.MissionNumber}: {string.Join(", ", duplicates)}"
                );
            }
        }

        return Result<bool>.Success(true);
    }

}
