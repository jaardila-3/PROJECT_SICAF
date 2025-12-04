using FluentValidation;

using SICAF.Common.DTOs.Academic;

namespace SICAF.Common.Validators.Academic;

public class UpdateTasksDtoValidator : AbstractValidator<UpdateTasksDto>
{
    public UpdateTasksDtoValidator()
    {
        RuleFor(x => x.PhaseId)
            .NotEmpty()
            .WithMessage("El ID de la fase es requerido.");

        RuleFor(x => x.Tasks)
            .NotEmpty()
            .WithMessage("Debe incluir al menos una tarea.");

        RuleForEach(x => x.Tasks)
            .SetValidator(new TaskUpdateDtoValidator());
    }
}

public class TaskUpdateDtoValidator : AbstractValidator<TaskUpdateDto>
{
    public TaskUpdateDtoValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithMessage("El ID de la tarea es requerido.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("El nombre de la tarea es requerido.")
            .Length(3, 300)
            .WithMessage("El nombre debe tener entre 3 y 300 caracteres.");

        RuleFor(x => x.DisplayOrder)
            .InclusiveBetween(1, 100)
            .WithMessage("El orden de visualización debe estar entre 1 y 100.");

        // Validación condicional: Si IsP3InPhase es true, P3StartingFromMission debe tener valor
        RuleFor(x => x.P3StartingFromMission)
            .NotNull()
            .When(x => x.IsP3InPhase)
            .WithMessage("Debe especificar desde qué misión es P3 cuando marca la tarea como P3.");

        RuleFor(x => x.P3StartingFromMission)
            .InclusiveBetween(1, 100)
            .When(x => x.P3StartingFromMission.HasValue)
            .WithMessage("El número de misión debe estar entre 1 y 100.");

        // Validación adicional: Si P3StartingFromMission tiene valor, IsP3InPhase debe ser true
        RuleFor(x => x.IsP3InPhase)
            .Equal(true)
            .When(x => x.P3StartingFromMission.HasValue)
            .WithMessage("Si especifica una misión de inicio P3, debe marcar la tarea como P3.");
    }
}
