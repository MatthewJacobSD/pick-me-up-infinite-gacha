namespace PickMeUp.Api.Common.Errors;

/// <summary>
/// Thrown when a required entity does not exist.
/// </summary>
public sealed class NotFoundException : DomainException
{
    public NotFoundException(string message = "Resource not found.")
        : base(message) { }
}
