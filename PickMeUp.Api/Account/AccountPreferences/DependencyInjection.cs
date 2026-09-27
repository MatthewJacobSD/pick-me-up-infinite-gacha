namespace PickMeUp.Api.Account.AccountPreferences;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountPreferences(this IServiceCollection services)
    {
        // All preferences are registered via Hosting/DependencyInjection.cs
        // (MongoDB client/database, repository, validators via assembly scan).
        // This module has no additional services to register.
        return services;
    }
}
