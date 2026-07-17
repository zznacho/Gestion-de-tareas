// Core.Domain/Entities/TodoTask.cs
using Core.Domain.Enums;
using Core.Domain.Exceptions;
using TaskStatus = Core.Domain.Enums.TaskStatus;

namespace Core.Domain.Entities;

public class TodoTask
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public TaskPriority Priority { get; private set; }
    public TaskStatus Status { get; private set; }
    public DateTime? DueDate { get; private set; }
    public bool IsDeleted { get; private set; } // Flag para Eliminación Lógica
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Requerido por EF Core
    private TodoTask() { }

    public TodoTask(string title, string description, TaskPriority priority, DateTime? dueDate)
    {
        UpdateDetails(title, description, priority, dueDate);
        Id = Guid.NewGuid();
        Status = TaskStatus.Pendiente;
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string title, string description, TaskPriority priority, DateTime? dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("El título no puede estar vacío.", nameof(title));

        if (dueDate.HasValue && dueDate.Value < DateTime.UtcNow.Date)
            throw new InvalidOperationException("La fecha de vencimiento no puede ser menor a la fecha actual.");

        Title = title.Trim();
        Description = description.Trim();
        Priority = priority;
        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(TaskStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        if (IsDeleted) return;
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}