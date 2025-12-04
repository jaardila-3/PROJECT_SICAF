using FluentValidation;

using SICAF.Common.Constants;
using SICAF.Common.DTOs.Academic;

namespace SICAF.Common.Validators.Instructor;

/// <summary>
/// Validador para los motivos de N-Roja
/// </summary>
public class NRedReasonValidator : AbstractValidator<NRedReasonDto>
{
    public NRedReasonValidator()
    {
        RuleFor(x => x.ReasonCategory)
            .NotEmpty()
            .WithMessage("La categoría del motivo N-Roja es requerida")
            .Must(BeValidCategory)
            .WithMessage("La categoría debe ser una de las categorías válidas: Mental, Física o Emocional");

        RuleFor(x => x.ReasonDescription)
            .NotEmpty()
            .WithMessage("La descripción del motivo N-Roja es requerida")
            .MaximumLength(100)
            .WithMessage("La descripción no puede exceder 100 caracteres");
    }

    private static bool BeValidCategory(string category)
    {
        return GradeConstants.NRedCategories.All.Contains(category);
    }
}