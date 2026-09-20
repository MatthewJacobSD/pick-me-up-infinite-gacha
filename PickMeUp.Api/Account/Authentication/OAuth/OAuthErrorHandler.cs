namespace PickMeUp.Api.Account.Authentication.OAuth
{
    // Handles HTTP errors and null responses from OAuth provider APIs.
    // Used by OAuthCallbackHandler after token exchange and userinfo requests.

    public sealed class OAuthErrorHandler
    {
        // Deserialises the response or throws on HTTP error / empty body.
        public async Task<T> EnsureSuccess<T>(
            OAuthProvider provider,
            HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new OAuthHttpException(
                    provider,
                    (int)response.StatusCode,
                    $"OAuth HTTP error ({provider}): {body}"
                );
            }

            try
            {
                var result =
                    await response.Content.ReadFromJsonAsync<T>() ??
                    throw new OAuthUserInfoException(provider, "Empty JSON response");

                return result;
            }
            catch (Exception ex)
            {
                throw new OAuthUserInfoException(provider, $"Invalid JSON response: {ex.Message}");
            }
        }

        // Throws if the value is null.
        public void EnsureNotNull<T>(OAuthProvider provider, T? value, string message)
        {
            if (value is null)
                throw new OAuthException(provider, message);
        }
    }
}
