// Core.Application/DependencyInjection.cs
using Core.Application.Abstractions;
using Core.Application.Dtos;
using Core.Application.Features.Tasks.Commands.CreateTask;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registro de Validadores
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Registro de Handlers (CQRS)
        services.AddScoped<ICommandHandler<CreateTaskCommand, TaskDto>, CreateTaskCommandHandler>();
        
        return services;
    }
}