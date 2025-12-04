using Microsoft.Extensions.Logging;

using SICAF.Data.Entities.Academic;
using SICAF.Data.Interfaces.Repositories;
using SICAF.Data.Interfaces.Seeders;

namespace SICAF.Data.Initialization.Seeders;

public class TaskSeeder(IUnitOfWork unitOfWork, ILogger<TaskSeeder> logger) : IDataSeeder
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<TaskSeeder> _logger = logger;
    public int Priority => 6;

    public async Task SeedAsync()
    {
        _logger.LogInformation("Iniciando seeding de tareas...");

        var tasks = new List<Tasks>();
        tasks.AddRange(MissionTaskFixedWingData.GetFixedWingTasks());
        tasks.AddRange(MissionTaskRotaryWingData.GetRotaryWingTasks());

        foreach (var task in tasks)
        {
            await SeedTaskAsync(task);
        }

        _logger.LogInformation("Seeding de tareas completado.");
    }

    private async Task SeedTaskAsync(Tasks workTask)
    {
        var existingTask = await _unitOfWork.Repository<Tasks>()
            .GetFirstAsync(wt => wt.Code == workTask.Code && wt.WingType == workTask.WingType);

        if (existingTask == null)
        {
            await _unitOfWork.Repository<Tasks>().AddAsync(workTask);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Tarea {TaskCode} - {TaskName} creada exitosamente.", workTask.Code, workTask.Name);
        }
        else
        {
            _logger.LogInformation("Tarea {TaskCode} - {TaskName} ya existe.", workTask.Code, workTask.Name);
        }
    }
}