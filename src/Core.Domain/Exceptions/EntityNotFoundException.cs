// Core.Domain/Exceptions/EntityNotFoundException.cs
namespace Core.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) : base(message) { }
}