namespace PickMeUp.Api.Account.Authentication.OAuth.Provider.Google
{
    /// <summary>
    /// Deserialisation target for Google's /oauth2/v2/userinfo endpoint response.
    /// </summary>
    public sealed class GoogleUserInfo
    {
        public string Id { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Picture { get; init; } = string.Empty;
    }
}
