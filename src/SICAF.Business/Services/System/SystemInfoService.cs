using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using SICAF.Common.DTOs.System;
using SICAF.Data.Context;

namespace SICAF.Business.Services.System;

public interface ISystemInfoService
{
    Task<SystemInfoDto> GetSystemInfoAsync();
}

public class SystemInfoService(
    SicafDbContext context,
    IConfiguration configuration
    ) : ISystemInfoService
{
    private readonly SicafDbContext _context = context;
    private readonly IConfiguration _configuration = configuration;

    public async Task<SystemInfoDto> GetSystemInfoAsync()
    {
        var systemInfo = new SystemInfoDto
        {
            Framework = GetFrameworkInfo(),
            Environment = _configuration["ASPNETCORE_ENVIRONMENT"] ?? "Not set",
            BuildDate = GetBuildDate(),
            ServerTime = DateTime.Now,
            ServerTimeZone = TimeZoneInfo.Local.Id,
            Database = await GetDatabaseInfoAsync(),
            Deployment = GetDeploymentInfo(),
        };

        return systemInfo;
    }

    private static DateTime GetBuildDate() => File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location);

    private SystemInfoDto.DeploymentInfo GetDeploymentInfo()
    {
        return new SystemInfoDto.DeploymentInfo
        {
            DeployedBy = _configuration["DeploymentInfo:DeployedBy"] ?? "Not set",
            Branch = _configuration["DeploymentInfo:Branch"] ?? "Not set",
            ReleaseNotes = _configuration["DeploymentInfo:ReleaseNotes"] ?? "Not set"
        };
    }


    private async Task<SystemInfoDto.DatabaseInfo> GetDatabaseInfoAsync()
    {
        var dbInfo = new SystemInfoDto.DatabaseInfo();

        try
        {
            // Verificar conexión
            await _context.Database.CanConnectAsync();
            dbInfo.ConnectionStatus = "Connected";

            // Verificar migraciones pendientes
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            dbInfo.PendingMigrations = pendingMigrations.Count();
        }
        catch (Exception ex)
        {
            dbInfo.ConnectionStatus = $"Error: {ex.Message}";
        }

        return dbInfo;
    }

    /// <summary>
    /// Obtiene toda la información del Framework .NET
    /// </summary>
    private static SystemInfoDto.FrameworkInfo GetFrameworkInfo()
    {
        var frameworkInfo = new SystemInfoDto.FrameworkInfo();

        try
        {
            // 1. Versión del Runtime de .NET
            frameworkInfo.RuntimeVersion = RuntimeInformation.FrameworkDescription;
            // Ejemplo: ".NET 8.0.1"

            // 2. Sistema Operativo donde corre
            frameworkInfo.OperatingSystem = RuntimeInformation.OSDescription;
            // Ejemplo: "Microsoft Windows 10.0.22631"

            // 3. Arquitectura del procesador
            frameworkInfo.ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString();
            // Ejemplo: "X64"

            // 4. Arquitectura del OS
            frameworkInfo.OSArchitecture = RuntimeInformation.OSArchitecture.ToString();
            // Ejemplo: "X64"

            // 5. Versión de .NET desde Environment
            frameworkInfo.DotNetVersion = Environment.Version.ToString();
            // Ejemplo: "8.0.1" (versión del CLR)

            // 6. Target Framework de la aplicación
            var assembly = Assembly.GetExecutingAssembly();
            var customAttributes = assembly.GetCustomAttributes(false);
            var targetFrameworkAttr = customAttributes.FirstOrDefault(a => a.GetType().Name == "TargetFrameworkAttribute");

            if (targetFrameworkAttr != null)
            {
                var frameworkNameProp = targetFrameworkAttr.GetType().GetProperty("FrameworkName");
                frameworkInfo.TargetFramework = frameworkNameProp?.GetValue(targetFrameworkAttr)?.ToString() ?? "Unknown";
            }

            // 7. Información adicional del proceso
            var process = Process.GetCurrentProcess();
            frameworkInfo.ProcessId = process.Id;
            frameworkInfo.ProcessName = process.ProcessName;
            frameworkInfo.MachineName = Environment.MachineName;
            frameworkInfo.Is64BitProcess = Environment.Is64BitProcess;

            // 8. Versiones de assemblies importantes
            frameworkInfo.AssemblyVersions = new Dictionary<string, string>
            {
                ["EntityFrameworkCore"] = typeof(DbContext).Assembly.GetName().Version?.ToString() ?? "Unknown",
                ["SICAF.Data"] = typeof(SicafDbContext).Assembly.GetName().Version?.ToString() ?? "Unknown"
            };
        }
        catch (Exception ex)
        {
            frameworkInfo.RuntimeVersion = $"Error: {ex.Message}";
        }

        return frameworkInfo;
    }
}