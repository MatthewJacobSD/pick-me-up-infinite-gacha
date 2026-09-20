namespace PickMeUp.Api.Account.Authentication.OAuth.Provider.Google
{
    // Deserialisation target for Google's /oauth2/v2/userinfo response.
    public sealed class GoogleUserInfo
    {
        public string Id { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Picture { get; init; } = string.Empty;
    }
}
