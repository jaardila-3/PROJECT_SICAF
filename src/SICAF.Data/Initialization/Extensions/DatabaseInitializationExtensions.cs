using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SICAF.Common.Configuration.Options;
using SICAF.Data.Context;

namespace SICAF.Data.Initialization.Extensions;

public static class DatabaseInitializationExtensions
{
    /// <summary>
    /// Inicializa la base de datos aplicando migraciones y ejecutando seeders.
    /// </summary>
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<DatabaseInitializer>>();
        var featureFlags = services.GetRequiredService<IOptions<FeatureFlagsOptions>>().Value;

        if (featureFlags.DatabaseMigrateAndSeedAtStartup)
        {
            try
            {
                if (await serviceProvider.WaitForDatabaseAsync())
                {
                    logger.LogInformation("Iniciando proceso de inicialización de base de datos...");

                    // Ejecutar seeders
                    var initializer = services.GetRequiredService<DatabaseInitializer>();
                    await initializer.InitializeAsync();

                    logger.LogInformation("Proceso de inicialización completado exitosamente.");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error fatal durante la inicialización de la base de datos.");
                throw;
            }
        }
    }

    /// <summary>
    /// Verifica si la base de datos está disponible y lista.
    /// </summary>
    private static async Task<bool> IsDatabaseReadyAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SicafDbContext>();

        try
        {
            return await context.Database.CanConnectAsync();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetService<ILogger<DatabaseInitializer>>();
            logger?.LogWarning(ex, "No se pudo conectar a la base de datos.");
            return false;
        }
    }

    /// <summary>
    /// Espera hasta que la base de datos esté disponible.
    /// </summary>
    private static async Task<bool> WaitForDatabaseAsync(
        this IServiceProvider serviceProvider,
        int maxRetries = 10,
        TimeSpan? delay = null)
    {
        delay ??= TimeSpan.FromSeconds(5);
        using var scope = serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseInitializer>>();

        for (int i = 0; i < maxRetries; i++)
        {
            if (await serviceProvider.IsDatabaseReadyAsync())
            {
                logger.LogInformation("Base de datos disponible y lista para usar.");
                return true;
            }

            logger.LogWarning(
                "Base de datos no disponible. Intento {CurrentAttempt} de {MaxAttempts}. " +
                "Esperando {Delay} segundos...",
                i + 1,
                maxRetries,
                delay.Value.TotalSeconds);

            await Task.Delay(delay.Value);
        }

        logger.LogError("La base de datos no está disponible después de {MaxAttempts} intentos.", maxRetries);
        return false;
    }

}