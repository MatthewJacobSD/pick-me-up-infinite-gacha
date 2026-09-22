namespace PickMeUp.Api.Social;

public sealed class SocialValidationException(string message) : Exception(message);

public sealed class SocialUnauthenticatedException(string message = "Authentication is required.")
    : Exception(message);

public sealed class SocialPolicyDeniedException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}

public sealed class SocialNotFoundException(string message) : Exception(message);

public sealed class SocialConflictException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}
