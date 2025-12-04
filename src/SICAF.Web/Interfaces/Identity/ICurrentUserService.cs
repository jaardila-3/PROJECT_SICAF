using SICAF.Common.DTOs.Identity;

namespace SICAF.Web.Interfaces.Identity;

public interface ICurrentUserService
{
    Task<UserDto?> GetCurrentUserAsync();
}