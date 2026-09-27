using Microsoft.Extensions.DependencyInjection.Extensions;
using PickMeUp.Api.Common.Authentication;

namespace PickMeUp.Api.Account.AccountPreferences;

/// <summary>
/// Registers account-preferences-domain services into the DI container.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddAccountPreferences(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAddScoped<ICurrentUser, CurrentUser>();

        services.TryAddSingleton<IAccountPreferencesRepository, AccountPreferencesRepository>();
        services.AddHostedService<AccountPreferencesIndexHostedService>();

        return services;
    }
}
