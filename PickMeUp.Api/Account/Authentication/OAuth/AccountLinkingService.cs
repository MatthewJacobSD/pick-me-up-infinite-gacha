namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // Repository interface for external account links (provider → account mapping).
    public interface IExternalAccountRepository
    {
        ExternalAccountLink? FindByExternalId(OAuthProvider provider, string externalId);
        ExternalAccountLink? FindByEmail(string email);
        void Add(ExternalAccountLink link);
    }

    // Maps an external provider identity to an internal account ID.
    public sealed record ExternalAccountLink(
        Guid AccountId,
        OAuthProvider Provider,
        string ExternalId,
        string Email);

    // Links an OAuth identity to an existing account, or returns an existing link.
    //
    // Priority:
    //   1. If a link with this (provider, externalId) exists → return it.
    //   2. If a link with this email exists → return it (different provider, same email).
    //   3. Otherwise, create a new link.

    public sealed class AccountLinkingService(IExternalAccountRepository externalAccounts)
    {
        private readonly IExternalAccountRepository _externalAccounts = externalAccounts;

        public ExternalAccountLink LinkOrGetExisting(Guid accountId, ExternalIdentity identity)
        {
            var existingByExternal = _externalAccounts.FindByExternalId(identity.Provider, identity.ExternalId);
            if (existingByExternal is not null)
                return existingByExternal;

            var existingByEmail = _externalAccounts.FindByEmail(identity.Email);
            if (existingByEmail is not null)
                return existingByEmail;

            var link = new ExternalAccountLink(
                AccountId: accountId,
                Provider: identity.Provider,
                ExternalId: identity.ExternalId,
                Email: identity.Email
            );

            _externalAccounts.Add(link);
            return link;
        }
    }
}
