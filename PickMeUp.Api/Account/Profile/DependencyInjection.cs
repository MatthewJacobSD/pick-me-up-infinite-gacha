using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PickMeUp.Api.Account.Profile;

/**--------[DI Registration]--------**/

/// <summary>
/// Registers Profile services into the IoC container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the profile repository as a singleton (backed by a single MongoDB collection).
    /// </summary>
    public static IServiceCollection AddProfile(this IServiceCollection services)
    {
        // TryAddSingleton — safe to call multiple times without overwriting a prior registration.
        services.TryAddSingleton<IProfileRepository, ProfileRepository>();
        return services;
    }
}
