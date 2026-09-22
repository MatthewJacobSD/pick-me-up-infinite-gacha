namespace PickMeUp.Api.Common.Errors;

public sealed class NotFoundException : DomainException
{
    public NotFoundException(string message = "Resource not found.")
        : base(message) { }
}
