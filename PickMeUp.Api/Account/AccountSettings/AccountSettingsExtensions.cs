namespace PickMeUp.Api.Account.AccountSettings;

// ── AccountSettings DI Registration ───────────────────────
// Registers account-settings services (validators, etc.)
// FluentValidation auto-discovers validators from this assembly,
// so this stub is a hook for any future non-validator registrations.

public static class AccountSettingsExtensions
{
    public static IServiceCollection AddAccountSettings(this IServiceCollection services)
    {
        // Validators are auto-registered via AddValidatorsFromAssemblyContaining
        // in DependencyInjection.AddFluentValidation(). Nothing else needed yet.
        return services;
    }
}
