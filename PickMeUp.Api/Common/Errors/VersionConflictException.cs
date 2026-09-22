namespace PickMeUp.Api.Common.Errors;

public sealed class VersionConflictException : DomainException
{
    public VersionConflictException(string message = "The resource has been modified by another request.")
        : base(message) { }
}
