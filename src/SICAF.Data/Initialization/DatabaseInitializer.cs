using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SICAF.Data.Context;
using SICAF.Data.Interfaces.Seeders;

namespace SICAF.Data.Initialization;

public class DatabaseInitializer(IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<DatabaseInitializer> _logger = logger;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SicafDbContext>();

        try
        {
            _logger.LogInformation("Iniciando proceso de inicialización de base de datos...");

            // Aplicar migraciones pendientes
            if (context.Database.GetPendingMigrations().Any())
            {
                _logger.LogInformation("Aplicando migraciones pendientes...");
                await context.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Migraciones aplicadas exitosamente.");
            }

            // Ejecutar seeders
            await SeedDataAsync(scope);

            _logger.LogInformation("Proceso de inicialización completado exitosamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante la inicialización de la base de datos.");
            throw;
        }
    }

    private async Task SeedDataAsync(IServiceScope scope)
    {
        // Obtener todos los seeders registrados
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>()
            .OrderBy(s => s.Priority)
            .ToList();

        if (!seeders.Any())
        {
            _logger.LogWarning("No se encontraron seeders registrados.");
            return;
        }

        _logger.LogInformation($"Ejecutando {seeders.Count} seeders...");

        foreach (var seeder in seeders)
        {
            var seederName = seeder.GetType().Name;
            try
            {
                _logger.LogInformation($"Ejecutando {seederName}...");
                await seeder.SeedAsync();
                _logger.LogInformation($"{seederName} ejecutado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al ejecutar {seederName}.");
                throw;
            }
        }
    }
}