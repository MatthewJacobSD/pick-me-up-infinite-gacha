using MongoDB.Bson.Serialization.Attributes;

namespace PickMeUp.Api.Account.Authentication.OAuth
{
    /**--------[Repository Interface]--------**/

    public interface IExternalAccountRepository
    {
        ExternalAccountLink? FindByExternalId(OAuthProvider provider, string externalId);
        ExternalAccountLink? FindByEmail(string email);
        void Add(ExternalAccountLink link);
    }

    /**--------[Account Link Entity]--------**/

    /// <summary>
    /// Maps an external provider identity to an internal account ID.
    /// </summary>
    public sealed class ExternalAccountLink
    {
        [BsonId]
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid AccountId { get; init; }
        public OAuthProvider Provider { get; init; }
        public string ExternalId { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }

    /**--------[Account Linking Service]--------**/

    /// <summary>
    /// Links an OAuth identity to an existing account, or returns an existing link.
    /// Priority: (provider + externalId) match > email match > create new.
    /// </summary>
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

            var link = new ExternalAccountLink
            {
                AccountId = accountId,
                Provider = identity.Provider,
                ExternalId = identity.ExternalId,
                Email = identity.Email
            };

            _externalAccounts.Add(link);
            return link;
        }
    }
}
