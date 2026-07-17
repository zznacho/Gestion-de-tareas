// Core.Application/Dtos/TaskDto.cs
using Core.Domain.Enums;
using TaskStatus = Core.Domain.Enums.TaskStatus;

namespace Core.Application.Dtos;

public record TaskDto(
    Guid Id,
    string Title,
    string Description,
    TaskPriority Priority,
    TaskStatus Status,
    DateTime? DueDate,
    DateTime CreatedAt
);