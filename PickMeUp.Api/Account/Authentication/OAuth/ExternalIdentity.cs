namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // Normalised identity returned by any OAuth provider after token exchange.
    // Used downstream by AccountCreationService and AccountLinkingService.

    public sealed record ExternalIdentity(
        OAuthProvider Provider,
        string Email,
        string Name,
        string ExternalId);
}
