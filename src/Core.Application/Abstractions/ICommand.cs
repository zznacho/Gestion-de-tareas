// Core.Application/Abstractions/ICommand.cs
namespace Core.Application.Abstractions;

public interface ICommandHandler<in TCommand, TResponse>
{
    Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}