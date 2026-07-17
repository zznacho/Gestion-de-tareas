// Infrastructure.Persistence/Repositories/TaskRepository.cs
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Domain.Repositories;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Core.Domain.Enums.TaskStatus;

namespace Infrastructure.Persistence.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<TodoTask>> ListAsync(TaskStatus? status, TaskPriority? priority, CancellationToken cancellationToken = default)
    {
        var query = _context.Tasks.AsNoTracking();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TodoTask task, CancellationToken cancellationToken = default)
    {
        await _context.Tasks.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TodoTask task, CancellationToken cancellationToken = default)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync(cancellationToken);
    }
}