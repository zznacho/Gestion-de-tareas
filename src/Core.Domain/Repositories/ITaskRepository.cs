// Core.Domain/Repositories/ITaskRepository.cs
using Core.Domain.Entities;
using Core.Domain.Enums;
using TaskStatus = Core.Domain.Enums.TaskStatus;

namespace Core.Domain.Repositories;

public interface ITaskRepository
{
    Task<TodoTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TodoTask>> ListAsync(TaskStatus? status, TaskPriority? priority, CancellationToken cancellationToken = default);
    Task AddAsync(TodoTask task, CancellationToken cancellationToken = default);
    Task UpdateAsync(TodoTask task, CancellationToken cancellationToken = default);
}