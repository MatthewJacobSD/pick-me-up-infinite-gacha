using Microsoft.Extensions.DependencyInjection.Extensions;
using PickMeUp.Api.Account.AccountPreferences;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Social;

/// <summary>
/// Registers all social-domain services into the DI container.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddSocial(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAddScoped<ICurrentUser, CurrentUser>();

        // Social registers the dependencies it needs.
        services.TryAddSingleton<IAccountPreferencesRepository, AccountPreferencesRepository>();

        services.AddSingleton<ISocialVisibilityQuery, PreferencesSocialVisibilityQuery>();
        services.AddSingleton<ISocialRepository, SocialRepository>();
        services.AddSingleton<IFriendRequestLifecycleEngine, FriendRequestLifecycleEngine>();
        services.AddSingleton<SocialPolicy>();
        services.AddSingleton<ISocialService, SocialService>();
        services.AddHostedService<SocialIndexHostedService>();
        services.AddExceptionHandler<SocialExceptionHandler>();
        return services;
    }
}
