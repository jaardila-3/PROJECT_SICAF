using FluentValidation;

using SICAF.Common.DTOs.Identity;

namespace SICAF.Common.Validators.Identity;

public class UpdateValidator : AbstractValidator<UpdateDto>
{
    public UpdateValidator()
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
        // Validación de BirthDateString
        RuleFor(x => x.BirthDateString)
            .Must(BeAValidDate)
            .WithMessage("La fecha de nacimiento debe ser una fecha válida.")
            .When(x => !string.IsNullOrWhiteSpace(x.BirthDateString));

        // Validación de roles seleccionados
        RuleFor(x => x.SelectedRoleIds)
            .NotEmpty()
            .WithMessage("Debe seleccionar al menos un rol.")
            .Must(x => x != null && x.Count > 0)
            .WithMessage("Debe seleccionar al menos un rol.");

        // Validación personalizada para bloqueo/desbloqueo
        RuleFor(x => x)
            .Custom((model, context) =>
            {
                // Si el usuario quiere bloquear
                if (model.WantToLock)
                {
                    // Debe proporcionar una razón
                    if (string.IsNullOrWhiteSpace(model.LockoutReason))
                    {
                        context.AddFailure(nameof(model.LockoutReason),
                            "Debe proporcionar una razón para el bloqueo.");
                    }

                    // Si proporciona fecha, debe ser futura
                    if (model.LockoutEnd.HasValue && model.LockoutEnd.Value <= DateTime.Now)
                    {
                        context.AddFailure(nameof(model.LockoutEnd),
                            "La fecha de bloqueo debe ser posterior a la fecha actual.");
                    }
                }
            });
    }

    private bool BeAValidDate(string dateString) => DateTime.TryParse(dateString, out _);

}