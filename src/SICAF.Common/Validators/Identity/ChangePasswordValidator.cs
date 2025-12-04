using FluentValidation;

using SICAF.Common.DTOs.Identity;

namespace SICAF.Common.Validators.Identity;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword).ValidatePasswordLogin();
        RuleFor(x => x.NewPassword).ValidatePassword();
        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword).WithMessage("Las contraseñas no coinciden");
    }
}