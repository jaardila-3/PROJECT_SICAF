namespace SICAF.Data.Interfaces.Seeders;

public interface IDataSeeder
{
    /// <summary>
    /// Ejecuta el proceso de seeding de datos
    /// </summary>
    /// <returns>Tarea asíncrona</returns>
    Task SeedAsync();

    /// <summary>
    /// Obtiene la prioridad de ejecución del seeder (menor número = mayor prioridad)
    /// </summary>
    int Priority { get; }
}