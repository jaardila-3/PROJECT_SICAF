using SICAF.Common.DTOs.Identity;
using SICAF.Data.Entities.Identity;

namespace SICAF.Business.Mappers.Identity;

public static class RoleMapperExtensions
{
    public static RoleDto MapToDto(this Role role)
    {
        ArgumentNullException.ThrowIfNull(role);

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };
    }
}