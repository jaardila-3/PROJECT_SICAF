using FluentValidation;

using SICAF.Common.DTOs.Identity;

namespace SICAF.Common.Validators.Identity;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Name).ValidateName();
        RuleFor(x => x.LastName).ValidateLastname();
        RuleFor(x => x.Username).ValidateUsername();
        RuleFor(x => x.DocumentType).NotEmpty().WithMessage("El tipo de documento es obligatorio");
        RuleFor(x => x.IdentificationNumber).ValidateIdentificationNumber();
        RuleFor(x => x.Nationality).ValidateNationality();
        RuleFor(x => x.BloodType).ValidateBloodType();
        RuleFor(x => x.BirthDate).ValidateBirthDate();
        RuleFor(x => x.PhoneNumber).ValidatePhoneNumber();
    }
}