using FluentValidation;

using SICAF.Common.DTOs.Identity;

namespace SICAF.Common.Validators.Identity;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).ValidateUsername();
        RuleFor(x => x.Password).ValidatePasswordLogin();
    }
}