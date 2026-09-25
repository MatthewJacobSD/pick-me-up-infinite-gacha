using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PickMeUp.Api.Account.Profile;

public static class DependencyInjection
{
    public static IServiceCollection AddProfile(this IServiceCollection services)
    {
        services.TryAddSingleton<IProfileRepository, ProfileRepository>();
        return services;
    }
}
