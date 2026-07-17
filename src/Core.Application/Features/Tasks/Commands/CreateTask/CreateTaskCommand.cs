// Core.Application/Features/Tasks/Commands/CreateTask/CreateTaskCommand.cs
using Core.Domain.Enums;

namespace Core.Application.Features.Tasks.Commands.CreateTask;

public record CreateTaskCommand(
    string Title,
    string Description,
    TaskPriority Priority,
    DateTime? DueDate
);