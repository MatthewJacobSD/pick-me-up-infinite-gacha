namespace PickMeUp.Api.Common.Errors;

public sealed class ValidationException : DomainException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("Validation failed.")
    {
        Errors = new Dictionary<string, string[]>(errors);
    }
}
