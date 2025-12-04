using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using SICAF.Common.Constants;
using SICAF.Data.Entities.Academic;
using SICAF.Data.Interfaces.Repositories;
using SICAF.Data.Interfaces.Seeders;

using static SICAF.Common.Constants.AviationConstants;

namespace SICAF.Data.Initialization.Seeders;

public class MissionTaskSeeder(IUnitOfWork unitOfWork, ILogger<MissionTaskSeeder> logger) : IDataSeeder
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<MissionTaskSeeder> _logger = logger;
    public int Priority => 7;

    public async Task SeedAsync()
    {
        _logger.LogInformation("Iniciando seeding de relaciones misión-tarea...");

        // Procesar ala fija
        await SeedMissionTasksForWingTypeAsync(WingTypes.FIXED_WING);

        // Procesar ala rotatoria:
        await SeedMissionTasksForWingTypeAsync(WingTypes.ROTARY_WING);

        _logger.LogInformation("Seeding de relaciones misión-tarea completado.");
    }

    private async Task SeedMissionTasksForWingTypeAsync(string wingType)
    {
        _logger.LogInformation("Procesando mission tasks para: {WingType}", wingType);

        var phases = await _unitOfWork.Repository<Phase>()
            .GetListAsync(p => p.WingType == wingType, orderBy: q => q.OrderBy(p => p.PhaseNumber));

        var missions = await _unitOfWork.Repository<Mission>()
            .GetListAsync(m => m.WingType == wingType, includeFunc: q => q.Include(m => m.Phase));

        var tasks = await _unitOfWork.Repository<Tasks>()
            .GetListAsync(t => t.WingType == wingType);

        // 2. Crear estructuras de búsqueda eficientes
        var tasksByCode = tasks.ToDictionary(t => t.Code);
        var missionLookup = missions.ToDictionary(m => (m.Phase.PhaseNumber, m.MissionNumber));

        // 3. Obtener los mappings
        var mappingData = GetDataByWingType(wingType);

        // 4. OBTENER RELACIONES EXISTENTES
        // Obtenemos los Ids de las misiones que acabamos de cargar
        var missionIds = missionLookup.Values.Select(m => m.Id).ToHashSet();

        // Consultamos la BD por relaciones (MissionTask) que ya existen 
        // para cualquiera de estas misiones.
        var existingRelations = (await _unitOfWork.Repository<MissionTask>()
            .GetListAsync(mt => missionIds.Contains(mt.MissionId)))
            .Select(mt => (mt.MissionId, mt.TaskId)) // Seleccionamos solo los Ids
            .ToHashSet();

        _logger.LogDebug("{Count} relaciones existentes encontradas para {WingType}", existingRelations.Count, wingType);

        // 5. Procesar mappings y construir lista de nuevos MissionTasks
        var newMissionTasks = new List<MissionTask>();
        var missionsNotFound = new HashSet<(int Phase, int Mission)>();
        var tasksNotFound = new HashSet<int>();

        foreach (var ((phaseNumber, missionNumber), taskMappings) in mappingData)
        {
            // Verificar si existe la misión
            if (!missionLookup.TryGetValue((phaseNumber, missionNumber), out var mission))
            {
                missionsNotFound.Add((phaseNumber, missionNumber));
                continue;
            }

            // Procesar cada task mapping
            foreach (var taskMapping in taskMappings)
            {
                // Verificar si existe la tarea
                if (!tasksByCode.TryGetValue(taskMapping.TaskCode, out var task))
                {
                    tasksNotFound.Add(taskMapping.TaskCode);
                    continue;
                }

                // Verificar si ya existe la relación
                if (existingRelations.Contains((mission.Id, task.Id)))
                {
                    continue; // Ya existe, la omitimos
                }

                // Agregar nueva relación
                newMissionTasks.Add(new MissionTask
                {
                    MissionId = mission.Id,
                    TaskId = task.Id,
                    IsP3Task = taskMapping.IsP3,
                    DisplayOrder = taskMapping.DisplayOrder
                });

                // Agregar a existentes para evitar duplicados en la misma ejecución
                existingRelations.Add((mission.Id, task.Id));
            }
        }

        // 6. Logging de elementos no encontrados
        if (missionsNotFound.Count != 0)
        {
            _logger.LogWarning("Misiones no encontradas para {WingType}: {Missions}",
                wingType, string.Join(", ", missionsNotFound.Select(m => $"F{m.Phase}M{m.Mission}")));
        }

        if (tasksNotFound.Count != 0)
        {
            _logger.LogWarning("Tareas no encontradas para {WingType}: {Tasks}",
                wingType, string.Join(", ", tasksNotFound.OrderBy(t => t)));
        }

        // 7. Insertar todas las nuevas relaciones en batch
        if (newMissionTasks.Count != 0)
        {
            try
            {
                // Dividir en lotes para evitar problemas con grandes volúmenes (batches es lista de una lista de MissionTasks)
                const int batchSize = 100;
                var batches = newMissionTasks
                    .Select((item, index) => new { item, index })
                    .GroupBy(x => x.index / batchSize)
                    .Select(g => g.Select(x => x.item).ToList());

                foreach (var batch in batches)
                {
                    await _unitOfWork.Repository<MissionTask>().AddRangeAsync(batch);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Se crearon {Count} mission tasks para {WingType} exitosamente", newMissionTasks.Count, wingType
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar mission tasks para {WingType}", wingType);
                throw;
            }
        }
        else
        {
            _logger.LogInformation("No se requieren nuevos mission tasks para {WingType}", wingType);
        }
    }

    private static Dictionary<(int PhaseNumber, int MissionNumber), List<TaskMapping>> GetDataByWingType(string wingType)
    {
        return wingType switch
        {
            WingTypes.FIXED_WING => MissionTaskFixedWingData.GetFixedWingMappings(),
            WingTypes.ROTARY_WING => MissionTaskRotaryWingData.GetRotaryWingMappings(),
            _ => []
        };
    }
}