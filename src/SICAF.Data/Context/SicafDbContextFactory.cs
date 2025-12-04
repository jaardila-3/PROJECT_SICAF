using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SICAF.Data.Context;

public class SicafDbContextFactory : IDesignTimeDbContextFactory<SicafDbContext>
{
    public SicafDbContext CreateDbContext(string[] args)
    {
        // Obtener el directorio base del proyecto
        var basePath = Directory.GetCurrentDirectory();
        Console.WriteLine($"Directorio actual: {basePath}");

        // Buscar el archivo appsettings.json navegando hacia arriba si es necesario
        var webProjectPath = FindWebProject(basePath);

        if (string.IsNullOrEmpty(webProjectPath))
        {
            throw new InvalidOperationException(
                "No se pudo encontrar el proyecto SICAF.Web. Asegúrate de ejecutar el comando desde la raíz de la solución.");
        }

        Console.WriteLine($"Proyecto Web encontrado en: {webProjectPath}");

        // Construir la configuración
        var configuration = new ConfigurationBuilder()
            .SetBasePath(webProjectPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Crear el DbContextOptionsBuilder
        var optionsBuilder = new DbContextOptionsBuilder<SicafDbContext>();

        // Obtener la cadena de conexión
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "No se encontró la cadena de conexión 'DefaultConnection' en la configuración.");
        }

        Console.WriteLine("Cadena de conexión encontrada.");
        Console.WriteLine(connectionString);

        // Configurar SQL Server
        optionsBuilder.UseSqlServer(connectionString);

        return new SicafDbContext(optionsBuilder.Options);
    }

    private string FindWebProject(string basePath)
    {
        // Primero, verificar si estamos en el directorio del proyecto web
        if (File.Exists(Path.Combine(basePath, "appsettings.json")))
        {
            return basePath;
        }

        // Buscar en subdirectorios
        var webPath = Path.Combine(basePath, "src", "SICAF.Web");
        if (Directory.Exists(webPath) && File.Exists(Path.Combine(webPath, "appsettings.json")))
        {
            return webPath;
        }

        // Buscar navegando hacia arriba
        var parentDirectory = Directory.GetParent(basePath);
        var attempts = 0;

        while (parentDirectory != null && attempts < 5)
        {
            webPath = Path.Combine(parentDirectory.FullName, "src", "SICAF.Web");
            if (Directory.Exists(webPath) && File.Exists(Path.Combine(webPath, "appsettings.json")))
            {
                return webPath;
            }

            // También verificar si llegamos a la raíz de la solución
            var solutionFile = Directory.GetFiles(parentDirectory.FullName, "*.sln");
            if (solutionFile.Length > 0)
            {
                webPath = Path.Combine(parentDirectory.FullName, "src", "SICAF.Web");
                if (Directory.Exists(webPath))
                {
                    return webPath;
                }
            }

            parentDirectory = parentDirectory.Parent;
            attempts++;
        }

        return string.Empty;
    }
}