using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SICAF.Common.Configuration.Options;
using SICAF.Common.Constants;
using SICAF.Common.Interfaces;
using SICAF.Data.Entities.Identity;
using SICAF.Data.Interfaces.Repositories;
using SICAF.Data.Interfaces.Seeders;

namespace SICAF.Data.Initialization.Seeders;

public class UserSeeder(
    IUnitOfWork unitOfWork,
    IOptions<AdminSettings> adminSettings,
    ILogger<UserSeeder> logger,
    IPasswordHasher passwordHasher) : IDataSeeder
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly AdminSettings _adminSettings = adminSettings.Value;
    private readonly ILogger<UserSeeder> _logger = logger;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public int Priority => 2; // Debe ejecutarse después de los roles

    public async Task SeedAsync()
    {
        _logger.LogInformation("Iniciando seeding de usuarios...");

        await SeedAdminUserAsync();

        _logger.LogInformation("Seeding de usuarios completado.");
    }

    private async Task SeedAdminUserAsync()
    {
        var adminUser = await _unitOfWork.Repository<User>()
            .GetFirstAsync(u => u.Username.Equals(_adminSettings.Username));

        if (adminUser == null)
        {
            adminUser = new User
            {
                Username = _adminSettings.Username,
                PasswordHash = _passwordHasher.HashPassword(_adminSettings.Password),
                Name = _adminSettings.Name,
                LastName = _adminSettings.LastName,
                DocumentType = _adminSettings.DocumentType,
                IdentificationNumber = _adminSettings.IdentificationNumber,
                PhoneNumber = _adminSettings.PhoneNumber,
                Grade = _adminSettings.Grade,
                Nationality = _adminSettings.Nationality,
                BloodType = _adminSettings.BloodType,
                BirthDate = _adminSettings.BirthDate,
                Force = _adminSettings.Force
            };

            await _unitOfWork.Repository<User>().AddAsync(adminUser);
            await _unitOfWork.SaveChangesAsync();

            await AssignRoleToUserAsync(adminUser.Id, SystemRoles.USERS_ADMIN);

            _logger.LogInformation("Usuario administrador {Username} creado exitosamente.", adminUser.Username);
        }
        else
        {
            _logger.LogInformation("Usuario administrador {Username} ya existe.", adminUser.Username);
        }
    }

    private async Task AssignRoleToUserAsync(Guid userId, string roleName)
    {
        var role = await _unitOfWork.Repository<Role>()
            .GetFirstAsync(r => r.Name.Equals(roleName));

        if (role != null)
        {
            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = role.Id
            };

            await _unitOfWork.Repository<UserRole>().AddAsync(userRole);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Rol {RoleName} asignado al usuario ID {UserId}.", roleName, userId);
        }
    }

}