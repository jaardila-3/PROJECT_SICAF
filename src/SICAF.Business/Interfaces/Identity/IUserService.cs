using SICAF.Common.DTOs.Identity;
using SICAF.Common.Models.Results;

namespace SICAF.Business.Interfaces.Identity;

/// <summary>
/// Interfaz del servicio de usuarios
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Obtiene todos los usuarios del sistema
    /// </summary>
    Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync();

    /// <summary>
    /// Obtiene un usuario por su ID
    /// </summary>
    Task<Result<UserDto>> GetUserByIdAsync(Guid userId);

    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    Task<Result<UserDto>> CreateUserAsync(RegisterDto createUserDto);

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    Task<Result<UserDto>> UpdateUserAsync(UpdateDto updateUserDto);

    /// <summary>
    /// Restablece la contraseña de un usuario usando su número de identificación
    /// </summary>
    Task<Result<bool>> AdminResetPasswordAsync(Guid userId);

    /// <summary>
    /// Cambia la contraseña de un usuario
    /// </summary>
    Task<Result<bool>> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);

    /// <summary>
    /// Obtiene el tipo de aeronave de un usuario
    /// </summary>
    Task<Result<string>> GetWingTypeAsync(Guid userId);
}
