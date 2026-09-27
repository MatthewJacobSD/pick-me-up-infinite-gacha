namespace PickMeUp.Api.Account.Authentication.OAuth.Provider.Facebook
{
    /// <summary>
    /// Deserialisation target for Facebook's /me endpoint response.
    /// </summary>
    public sealed class FacebookUserInfo
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}
