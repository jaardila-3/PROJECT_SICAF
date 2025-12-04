using FluentValidation;

using SICAF.Common.DTOs.Instructor;

namespace SICAF.Common.Validators.Instructor;

/// <summary>
/// Validador para el DTO de guardar evaluación de misión
/// </summary>
public class SaveMissionEvaluationValidator : AbstractValidator<SaveMissionEvaluationDto>
{
    public SaveMissionEvaluationValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty()
            .WithMessage("El ID del estudiante es requerido");

        RuleFor(x => x.InstructorId)
            .NotEmpty()
            .WithMessage("El ID del instructor es requerido");

        RuleFor(x => x.MissionId)
            .NotEmpty()
            .WithMessage("El ID de la misión es requerido");

        RuleFor(x => x.PhaseId)
            .NotEmpty()
            .WithMessage("El ID de la fase es requerido");

        RuleFor(x => x.CourseId)
            .NotEmpty()
            .WithMessage("El ID del programa es requerido");

        RuleFor(x => x.AircraftId)
            .NotEmpty()
            .WithMessage("El ID de la aeronave es requerido");

        RuleFor(x => x.EvaluationDate)
            .NotEmpty()
            .WithMessage("La fecha de evaluación es requerida")
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("La fecha de evaluación no puede ser futura")
            .GreaterThan(DateTime.Now.AddDays(-30))
            .WithMessage("La fecha de evaluación no puede ser mayor a 30 días en el pasado");

        RuleFor(x => x.GeneralObservations)
            .MaximumLength(2000)
            .WithMessage("Las observaciones no pueden exceder 2000 caracteres");

        // Validar horas de vuelo máquina
        RuleFor(x => x.MachineFlightHours)
            .InclusiveBetween(0.1, 10.0)
            .WithMessage("Las horas de vuelo máquina deben estar entre 0.1 y 10.0");

        // Validar que haya al menos una calificación de tarea
        RuleFor(x => x.TaskGrades)
            .NotNull()
            .WithMessage("Las calificaciones de tareas son requeridas")
            .Must(x => x != null && x.Count > 0)
            .WithMessage("Debe calificar todas las tareas de la misión");

        // Validar cada calificación de tarea
        RuleForEach(x => x.TaskGrades).SetValidator(new TaskGradeInputValidator());
    }
}
