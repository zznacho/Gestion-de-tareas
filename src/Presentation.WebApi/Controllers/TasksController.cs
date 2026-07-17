// Presentation.WebApi/Controllers/TasksController.cs
using Core.Application.Abstractions;
using Core.Application.Dtos;
using Core.Application.Features.Tasks.Commands.CreateTask;
using Core.Domain.Enums;
using Core.Domain.Exceptions;
using Core.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using TaskStatus = Core.Domain.Enums.TaskStatus;

namespace Presentation.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository; // Para consultas rápidas o de lectura

    public TasksController(ITaskRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create(
        [FromServices] ICommandHandler<CreateTaskCommand, TaskDto> handler,
        [FromBody] CreateTaskCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TaskDto>>> GetTasks(
        [FromQuery] TaskStatus? status,
        [FromQuery] TaskPriority? priority,
        CancellationToken cancellationToken)
    {
        var tasks = await _repository.ListAsync(status, priority, cancellationToken);
        
        // Mapeo manual a DTO
        var dtos = tasks.Select(t => new TaskDto(
            t.Id, t.Title, t.Description, t.Priority, t.Status, t.DueDate, t.CreatedAt
        )).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(id, cancellationToken);
        if (task == null)
            throw new EntityNotFoundException($"La tarea con ID {id} no existe.");

        return Ok(new TaskDto(
            task.Id, task.Title, task.Description, task.Priority, task.Status, task.DueDate, task.CreatedAt
        ));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(id, cancellationToken);
        if (task == null)
            throw new EntityNotFoundException($"La tarea con ID {id} no existe.");

        // Ejecutar comportamiento a través del Dominio
        task.UpdateDetails(request.Title, request.Description, request.Priority, request.DueDate);
        
        await _repository.UpdateAsync(task, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(id, cancellationToken);
        if (task == null)
            throw new EntityNotFoundException($"La tarea con ID {id} no existe.");

        task.UpdateStatus(request.Status);
        
        await _repository.UpdateAsync(task, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(id, cancellationToken);
        if (task == null)
            throw new EntityNotFoundException($"La tarea con ID {id} no existe.");

        // Ejecutamos eliminación lógica respetando las reglas tácticas del dominio
        task.SoftDelete();
        
        await _repository.UpdateAsync(task, cancellationToken);
        return NoContent();
    }
}

// Modelos locales de Request (Records)
public record UpdateTaskRequest(string Title, string Description, TaskPriority Priority, DateTime? DueDate);
public record UpdateStatusRequest(TaskStatus Status);