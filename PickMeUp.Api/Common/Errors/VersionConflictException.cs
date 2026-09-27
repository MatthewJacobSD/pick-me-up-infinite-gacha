namespace PickMeUp.Api.Common.Errors;

/// <summary>
/// Thrown when a resource has been modified concurrently and the current write would overwrite it.
/// </summary>
public sealed class VersionConflictException : DomainException
{
    public VersionConflictException(string message = "The resource has been modified by another request.")
        : base(message) { }
}
