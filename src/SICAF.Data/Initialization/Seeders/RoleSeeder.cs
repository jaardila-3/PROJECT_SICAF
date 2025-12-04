using Microsoft.Extensions.Logging;

using SICAF.Common.Constants;
using SICAF.Data.Entities.Identity;
using SICAF.Data.Interfaces.Repositories;
using SICAF.Data.Interfaces.Seeders;

namespace SICAF.Data.Initialization.Seeders;

public class RoleSeeder(IUnitOfWork unitOfWork, ILogger<RoleSeeder> logger) : IDataSeeder
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<RoleSeeder> _logger = logger;

    public int Priority => 1; // Alta prioridad - los roles deben crearse primero

    public async Task SeedAsync()
    {
        _logger.LogInformation("Iniciando seeding de roles...");

        var roles = GetDefaultRoles();

        foreach (var role in roles)
        {
            await SeedRoleAsync(role);
        }

        _logger.LogInformation("Seeding de roles completado.");
    }

    #region Métodos Privados
    private static List<Role> GetDefaultRoles()
    {
        return
        [
            new()
            {
                Name = SystemRoles.ACADEMIC_ADMIN,
                Description = "Administrador académico con gestión de cursos y estudiantes",
                IsSystemRole = true
            },
            new()
            {
                Name = SystemRoles.USERS_ADMIN,
                Description = "Administrador de usuarios con gestión de cuentas y roles",
                IsSystemRole = true
            },
            new()
            {
                Name = SystemRoles.INSTRUCTOR,
                Description = "Instructor de vuelo con capacidad de evaluación",
                IsSystemRole = true
            },
            new()
            {
                Name = SystemRoles.STUDENT,
                Description = "Estudiante del programa de aviación policial",
                IsSystemRole = true
            }
        ];
    }

    private async Task SeedRoleAsync(Role role)
    {
        var existingRole = await _unitOfWork.Repository<Role>()
            .GetFirstAsync(r => r.Name.Equals(role.Name));

        if (existingRole == null)
        {
            await _unitOfWork.Repository<Role>().AddAsync(role);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Rol {RoleName} creado exitosamente.", role.Name);
        }
        else
        {
            _logger.LogInformation("Rol {RoleName} ya existe.", role.Name);
        }
    }
    #endregion
}