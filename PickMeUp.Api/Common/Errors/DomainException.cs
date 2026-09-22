namespace PickMeUp.Api.Common.Errors;

public abstract class DomainException(string message) : Exception(message);
