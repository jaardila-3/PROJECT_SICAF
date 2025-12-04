using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SICAF.Data.Context;
using SICAF.Data.Initialization;
using SICAF.Data.Initialization.Seeders;
using SICAF.Data.Interfaces.Repositories;
using SICAF.Data.Interfaces.Seeders;
using SICAF.Data.Repositories;

namespace SICAF.Data;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Context data base
        services.AddDbContext<SicafDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            options.UseSqlServer(connectionString);
        });

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Registrar seeders
        services.AddScoped<IDataSeeder, RoleSeeder>();
        services.AddScoped<IDataSeeder, UserSeeder>();
        services.AddScoped<IDataSeeder, MasterCatalogSeeder>();
        services.AddScoped<IDataSeeder, PhaseSeeder>();
        services.AddScoped<IDataSeeder, MissionSeeder>();
        services.AddScoped<IDataSeeder, TaskSeeder>();
        services.AddScoped<IDataSeeder, MissionTaskSeeder>();
        services.AddScoped<IDataSeeder, AircraftSeeder>();

        // Registrar el inicializador de la base de datos
        services.AddScoped<DatabaseInitializer>();

        return services;
    }
}