
using Microsoft.EntityFrameworkCore;

using SICAF.Business.Interfaces.Identity;
using SICAF.Business.Mappers.Identity;
using SICAF.Common.Constants;
using SICAF.Common.DTOs.Identity;
using SICAF.Common.Models.Results;
using SICAF.Data.Entities.Catalogs;
using SICAF.Data.Entities.Identity;
using SICAF.Data.Interfaces.Repositories;

namespace SICAF.Business.Services.Identity;

public class RoleService(IUnitOfWork unitOfWork) : IRoleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<RoleDto>>> GetAllRolesAsync()
    {
        var roles = await _unitOfWork.Repository<Role>().GetListAsync(
           predicate: null,
           orderBy: q => q.OrderBy(u => u.Name)) ?? [];

        var rolesResponses = roles.Select(r => new RoleDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            IsSystemRole = r.IsSystemRole
        }).ToList();

        return Result<IEnumerable<RoleDto>>.Success(rolesResponses);
    }

    public async Task<Result<RoleDto>> GetRoleByIdAsync(Guid roleId)
    {
        var role = await _unitOfWork.Repository<Role>().GetFirstAsync(
            predicate: r => r.Id == roleId,
            includeFunc: q => q.Include(r => r.UserRoles).ThenInclude(ur => ur.User));

        var userDtos = new List<UserDto>();

        foreach (var ur in role?.UserRoles ?? [])
        {
            var userDto = ur.User.MapToDto();
            var catalogForce = await _unitOfWork.Repository<MasterCatalog>().GetFirstAsync(c => c.Code == userDto.Force);

            userDto.Force = catalogForce?.Name ?? userDto.Force;
            userDtos.Add(userDto);
        }

        return role is null
            ? Result<RoleDto>.Failure(SystemErrors.GeneralError.NotFound)
            : Result<RoleDto>.Success(new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                IsSystemRole = role.IsSystemRole,
                Users = userDtos ?? []
            });
    }

    /// <summary>
    /// Obtiene los roles de aviación
    /// </summary>
    public async Task<List<RoleDto>> GetRolesAsync(List<Guid> roleIds)
    {
        var roles = await _unitOfWork.Repository<Role>()
            .GetListAsync(r => roleIds.Contains(r.Id));

        return [.. roles.Select(r => r.MapToDto())];
    }

    /// <summary>
    /// Valida las restricciones de asignación de roles
    /// </summary>
    public async Task<Result<bool>> ValidateRoleAssignmentAsync(Guid updateUserId, List<Guid> newRoleIds, string loginUserRoles)
    {
        // Obtener roles actuales del usuario a editar
        var currentUserRoles = await _unitOfWork.Repository<UserRole>()
            .GetListAsync(
                ur => ur.UserId == updateUserId,
                includeFunc: q => q.Include(ur => ur.Role)
            );

        var currentRoleNames = currentUserRoles.Select(ur => ur.Role.Name).ToList();

        // Obtener nombres de los nuevos roles
        var newRoles = await _unitOfWork.Repository<Role>().GetListAsync(r => newRoleIds.Contains(r.Id));
        var newRoleNames = newRoles.Select(r => r.Name).ToList();

        // Validar que solo Admin Académico puede asignar roles de aviación
        var aviationRolesNames = newRoleNames.Intersect(AviationConstants.AviationRoles).ToList();
        var academicAdminRole = loginUserRoles.Split(',').Contains(SystemRoles.ACADEMIC_ADMIN);
        if (aviationRolesNames.Count > 0 && !academicAdminRole)
        {
            return Result<bool>.Failure(new Error("UNAUTHORIZED_AVIATION_ROLE",
                $"Solo el Administrador Académico puede asignar los roles: {string.Join(", ", aviationRolesNames)}"));
        }

        // Validar restricción del rol Estudiante
        var hasStudentRole = currentRoleNames.Contains(SystemRoles.STUDENT) || newRoleNames.Contains(SystemRoles.STUDENT);
        var hasOtherRoles = currentRoleNames.Except([SystemRoles.STUDENT]).Any() || newRoleNames.Except([SystemRoles.STUDENT]).Any();

        if (hasStudentRole && hasOtherRoles)
        {
            return Result<bool>.Failure(new Error("INVALID_STUDENT_ROLE_COMBINATION", "Un estudiante no puede tener otros roles"));
        }

        return Result<bool>.Success(true);
    }
}