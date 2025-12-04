using SICAF.Common.DTOs.Identity;
using SICAF.Common.Models.Results;

namespace SICAF.Business.Interfaces.Identity;

public interface IAuthentication
{
    Task<Result<UserDto>> HandleLogin(LoginDto request);
}