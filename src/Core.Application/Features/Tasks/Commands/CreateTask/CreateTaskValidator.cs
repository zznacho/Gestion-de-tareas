// Core.Application/Features/Tasks/Commands/CreateTask/CreateTaskValidator.cs
using FluentValidation;

namespace Core.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio.")
            .MaximumLength(100).WithMessage("El título no puede exceder los 100 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("La prioridad seleccionada no es válida.");

        RuleFor(x => x.DueDate)
            .Must(date => !date.HasValue || date.Value.Date >= DateTime.UtcNow.Date)
            .WithMessage("La fecha de vencimiento no puede estar en el pasado.");
    }
}