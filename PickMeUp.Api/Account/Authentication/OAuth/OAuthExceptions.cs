namespace PickMeUp.Api.Account.Authentication.OAuth
{
    /// <summary>
    /// Base exception for all OAuth errors. Carries the originating provider.
    /// </summary>
    public class OAuthException(OAuthProvider provider, string message) :
        Exception(message)
    {
        public OAuthProvider Provider { get; } = provider;
    }

    /// <summary>
    /// HTTP-level error from the provider's token or userinfo endpoint.
    /// </summary>
    public sealed class OAuthHttpException(OAuthProvider provider, int statusCode, string message) :
        OAuthException(provider, message)
    {
        public int StatusCode { get; } = statusCode;
    }

    /// <summary>
    /// Token exchange failed (e.g. invalid code, expired token).
    /// </summary>
    public sealed class OAuthTokenException(OAuthProvider provider, string message) :
        OAuthException(provider, message) { }

    /// <summary>
    /// Userinfo request returned null or unparseable data.
    /// </summary>
    public sealed class OAuthUserInfoException(OAuthProvider provider, string message) :
        OAuthException(provider, message) { }
}
