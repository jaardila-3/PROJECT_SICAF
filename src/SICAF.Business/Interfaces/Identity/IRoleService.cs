using SICAF.Common.DTOs.Identity;
using SICAF.Common.Models.Results;

namespace SICAF.Business.Interfaces.Identity;

/// <summary>
/// Interfaz del servicio de roles
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Obtiene todos los roles del sistema
    /// </summary>
    Task<Result<IEnumerable<RoleDto>>> GetAllRolesAsync();

    /// <summary>
    /// Obtiene un rol por su ID
    /// </summary>
    Task<Result<RoleDto>> GetRoleByIdAsync(Guid roleId);

    /// <summary>
    /// Obtiene los roles de aviación
    /// </summary>
    Task<List<RoleDto>> GetRolesAsync(List<Guid> roleIds);

    /// <summary>
    /// Valida las restricciones de asignación de roles
    /// </summary>
    Task<Result<bool>> ValidateRoleAssignmentAsync(Guid updateUserId, List<Guid> newRoleIds, string loginUserRole);
}
