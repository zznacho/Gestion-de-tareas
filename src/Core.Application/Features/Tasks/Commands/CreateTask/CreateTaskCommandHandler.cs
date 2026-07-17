// Core.Application/Features/Tasks/Commands/CreateTask/CreateTaskCommandHandler.cs
using Core.Application.Abstractions;
using Core.Application.Dtos;
using Core.Domain.Entities;
using Core.Domain.Repositories;
using FluentValidation;

namespace Core.Application.Features.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _repository;
    private readonly IValidator<CreateTaskCommand> _validator;

    public CreateTaskCommandHandler(ITaskRepository repository, IValidator<CreateTaskCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<TaskDto> HandleAsync(CreateTaskCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var task = new TodoTask(
            command.Title,
            command.Description,
            command.Priority,
            command.DueDate
        );

        await _repository.AddAsync(task, cancellationToken);

        // Mapeo manual ultra veloz y libre de dependencias "mágicas"
        return new TaskDto(
            task.Id,
            task.Title,
            task.Description,
            task.Priority,
            task.Status,
            task.DueDate,
            task.CreatedAt
        );
    }
}