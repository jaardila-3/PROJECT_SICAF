using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SICAF.Business.Interfaces.Identity;
using SICAF.Business.Mappers.Identity;
using SICAF.Common.Configuration.Options;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Identity;
using SICAF.Common.Interfaces;
using SICAF.Common.Models.Results;
using SICAF.Data.Entities.Identity;
using SICAF.Data.Interfaces.Repositories;

namespace SICAF.Business.Services.Identity;

public class AuthenticationService(
    ILogger<AuthenticationService> logger,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IOptions<LockoutSettings> lockoutSettings
) : IAuthentication
{
    #region Fields
    private readonly ILogger<AuthenticationService> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly LockoutSettings _lockoutSettings = lockoutSettings.Value;
    #endregion

    #region  Methods
    public async Task<Result<UserDto>> HandleLogin(LoginDto request)
    {
        var user = await _unitOfWork.Repository<User>()
            .GetFirstAsync(u => u.Username == request.Username,
                q => q.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).Include(u => u.AviationProfile)
            );
        if (user is null)
        {
            _logger.LogWarning("Usuario {Username} no encontrado", request.Username);
            return Result<UserDto>.Failure(SystemErrors.UserError.NotFound);
        }

        var isLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.Now;
        if (isLocked)
        {
            _logger.LogWarning("Usuario {Username} bloqueado", request.Username);
            return Result<UserDto>.Failure(SystemErrors.UserError.Blocked);
        }

        if (!_passwordHasher.VerifyPassword(user.PasswordHash!, request.Password))
        {
            _logger.LogWarning("Contraseña incorrecta para el usuario {Username}", request.Username);
            var failedAttempts = user.AccessFailedCount + 1;
            user.AccessFailedCount = failedAttempts;
            if (failedAttempts >= _lockoutSettings.MaxFailedAttempts)
            {
                user.LockoutEnd = DateTime.Now.AddMinutes(_lockoutSettings.LockoutDurationMinutes);
                user.LockoutReason = "Demasiados intentos fallidos de inicio de sesión";
                user.AccessFailedCount = 0;
            }
            _unitOfWork.Repository<User>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Result<UserDto>.Failure(SystemErrors.UserError.InvalidPassword);
        }

        if (user.AccessFailedCount > 0)
        {
            user.AccessFailedCount = 0;
            user.LockoutReason = null;
            _unitOfWork.Repository<User>().Update(user);
            await _unitOfWork.SaveChangesAsync();
        }

        _logger.LogInformation("Usuario {Username} autenticado correctamente", request.Username);
        return Result<UserDto>.Success(user.MapToDto());
    }
    #endregion
}