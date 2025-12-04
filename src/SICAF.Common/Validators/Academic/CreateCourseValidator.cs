using FluentValidation;

using SICAF.Common.DTOs.Academic;

namespace SICAF.Common.Validators.Academic;

/// <summary>
/// Validador para la creación de cursos
/// </summary>
public class CreateCourseValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseValidator()
    {
        RuleFor(x => x.CourseNumber)
            .NotEmpty().WithMessage("El número del programa es obligatorio")
            .InclusiveBetween(1, 999).WithMessage("El número debe estar entre 1 y 999");

        RuleFor(x => x.CourseName).ValidateName();

        RuleFor(x => x.Description).ValidateDescription();

        RuleFor(x => x.StartDateString)
            .NotEmpty().WithMessage("La fecha de inicio es obligatoria")
            .Must(BeAValidDate).WithMessage("La fecha de inicio debe ser válida")
            .Must(BeAValidDateNotInPast).WithMessage("La fecha de inicio no puede ser anterior a hoy");


        RuleFor(x => x.EndDateString)
            .NotEmpty().WithMessage("La fecha de finalización es obligatoria")
            .Must(BeAValidDate).WithMessage("La fecha de finalización debe ser válida");

        // Validación de fechas coherentes
        RuleFor(x => x)
            .Must(HaveValidDateRange)
            .WithMessage("La fecha de finalización debe ser posterior a la fecha de inicio")
            .When(x => BeAValidDate(x.StartDateString) && BeAValidDate(x.EndDateString));
    }

    private bool BeAValidDate(string dateString)
    {
        return DateTime.TryParse(dateString, out _);
    }

    private bool HaveValidDateRange(CreateCourseDto dto)
    {
        if (DateTime.TryParse(dto.StartDateString, out var startDate) &&
            DateTime.TryParse(dto.EndDateString, out var endDate))
        {
            return endDate > startDate;
        }
        return false;
    }

    private bool BeAValidDateNotInPast(string dateString)
    {
        if (DateTime.TryParse(dateString, out var date))
        {
            return date >= DateTime.Today;
        }
        return false;
    }
}