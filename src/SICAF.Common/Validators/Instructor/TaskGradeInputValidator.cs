using FluentValidation;

using SICAF.Common.Constants;
using SICAF.Common.DTOs.Instructor;

using static SICAF.Common.Constants.GradeConstants;

namespace SICAF.Common.Validators.Instructor;

/// <summary>
/// Validador para calificaciones de tareas regulares
/// </summary>
public class TaskGradeInputValidator : AbstractValidator<TaskGradeInputDto>
{
    public TaskGradeInputValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithMessage("El ID de la tarea es requerido");

        RuleFor(x => x.Grade)
            .NotEmpty()
            .WithMessage("La calificación es requerida")
            .Must(BeValidGrade);

        // Validar cada elemento de la lista NRedReasons
        RuleForEach(x => x.NRedReasons)
            .SetValidator(new NRedReasonValidator())
            .When(x => x.Grade.Equals(GradeTypes.NR) || x.Grade.Equals(GradeTypes.N));
    }

    private static bool BeValidGrade(string grade)
    {
        var validGrades = GradeTypes.All;
        return validGrades.Contains(grade?.ToUpper());
    }
}