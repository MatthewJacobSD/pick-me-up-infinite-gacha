namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // Base exception for all OAuth errors.

    public class OAuthException(OAuthProvider provider, string message) :
        Exception(message)
    {
        public OAuthProvider Provider { get; } = provider;
    }

    // HTTP-level error from the provider's token or userinfo endpoint.

    public sealed class OAuthHttpException(OAuthProvider provider, int statusCode, string message) :
        OAuthException(provider, message)
    {
        public int StatusCode { get; } = statusCode;
    }

    // Token exchange failed (e.g. invalid code, expired token).

    public sealed class OAuthTokenException(OAuthProvider provider, string message) :
        OAuthException(provider, message) { }

    // Userinfo request returned null or unparseable data.

    public sealed class OAuthUserInfoException(OAuthProvider provider, string message) :
        OAuthException(provider, message) { }
}
