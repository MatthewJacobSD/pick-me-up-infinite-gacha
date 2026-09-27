namespace PickMeUp.Api.Account.Authentication.OAuth
{
    /// <summary>
    /// Normalised identity returned by any OAuth provider after token exchange.
    /// </summary>
    public sealed record ExternalIdentity(
        OAuthProvider Provider,
        string Email,
        string Name,
        string ExternalId);
}
