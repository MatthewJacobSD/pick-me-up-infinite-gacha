namespace PickMeUp.Api.Account.Profile;

/**--------[Repository Contract]--------**/

/// <summary>
/// Persistence contract for player profile data.
/// All writes use optimistic concurrency via <c>expectedVersion</c>.
/// </summary>
public interface IProfileRepository
{
    /// <summary>
    /// Returns the existing profile or atomically inserts one with default values.
    /// </summary>
    Task<ProfileDocument> GetOrCreateAsync(Guid accountId);

    /// <summary>
    /// Returns the profile if it exists, otherwise <c>null</c>.
    /// </summary>
    Task<ProfileDocument?> FindAsync(Guid accountId);

    /// <summary>
    /// Replaces the username field. Throws if <paramref name="expectedVersion"/> is stale.
    /// </summary>
    Task<ProfileDocument> UpdateUsernameAsync(Guid accountId, Username username, int expectedVersion);

    /// <summary>
    /// Replaces the avatar field. Throws if <paramref name="expectedVersion"/> is stale.
    /// </summary>
    Task<ProfileDocument> UpdateAvatarAsync(Guid accountId, Avatar avatar, int expectedVersion);
}
