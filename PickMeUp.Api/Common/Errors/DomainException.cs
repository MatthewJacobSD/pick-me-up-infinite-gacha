namespace PickMeUp.Api.Common.Errors;

/// <summary>
/// Base exception for domain-layer errors that should be translated to Problem Details responses.
/// </summary>
public abstract class DomainException(string message) : Exception(message);
