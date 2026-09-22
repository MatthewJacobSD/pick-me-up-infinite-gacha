namespace PickMeUp.Api.Hosting;

// ── Typed Options ──────────────────────────────────────────────

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Secret { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; } = 15;
    public int RefreshExpiryDays { get; init; } = 7;
}

public sealed class MongoOptions
{
    public const string SectionName = "Mongo";

    public string ConnectionString { get; init; } = string.Empty;
    public string Database { get; init; } = string.Empty;
}

public sealed class MySqlOptions
{
    public const string SectionName = "MySql";

    public string ConnectionString { get; init; } = string.Empty;
}

public sealed class RedisOptions
{
    public const string SectionName = "Redis";

    public string Connection { get; init; } = string.Empty;
}

public sealed class OAuthOptions
{
    public const string SectionName = "OAuth";

    public GoogleOptions Google { get; init; } = new();
    public FacebookOptions Facebook { get; init; } = new();
}

public sealed class GoogleOptions
{
    public bool Enabled { get; init; }
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
}

public sealed class FacebookOptions
{
    public bool Enabled { get; init; }
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
}

// ── Configuration Extensions ───────────────────────────────────

public static class ConfigurationExtensions
{
    /// <summary>
    /// Loads .env files via DotNetEnv and merges values into the configuration.
    /// Priority: process env > .env.{Environment} > .env.local > .env.
    /// </summary>
    public static IConfigurationBuilder AddDotNetEnv(this IConfigurationBuilder configuration, string? environment = null)
    {
        EnvLoader.Load(configuration, environment);
        return configuration;
    }

    /// <summary>
    /// Binds typed options from configuration and validates required keys at startup.
    /// </summary>
    public static IServiceCollection AddPickMeUpOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<MongoOptions>(configuration.GetSection(MongoOptions.SectionName));
        services.Configure<MySqlOptions>(configuration.GetSection(MySqlOptions.SectionName));
        services.Configure<RedisOptions>(configuration.GetSection(RedisOptions.SectionName));
        services.Configure<OAuthOptions>(configuration.GetSection(OAuthOptions.SectionName));

        ValidateRequiredConfiguration(configuration);

        return services;
    }

    /// <summary>
    /// Validates that all required configuration keys are present.
    /// OAuth providers are only validated when Enabled=true.
    /// Never logs or exposes secret values.
    /// </summary>
    private static void ValidateRequiredConfiguration(IConfiguration configuration)
    {
        var missingKeys = new List<string>();

        ValidateSection(configuration, JwtOptions.SectionName, new[]
        {
            "Secret", "RefreshToken", "Issuer", "Audience"
        }, missingKeys);

        ValidateSection(configuration, MongoOptions.SectionName, new[]
        {
            "ConnectionString", "Database"
        }, missingKeys);

        ValidateSection(configuration, MySqlOptions.SectionName, new[]
        {
            "ConnectionString"
        }, missingKeys);

        ValidateSection(configuration, RedisOptions.SectionName, new[]
        {
            "Connection"
        }, missingKeys);

        ValidateOAuthProvider(configuration, "Google", missingKeys);
        ValidateOAuthProvider(configuration, "Facebook", missingKeys);

        if (missingKeys.Count > 0)
        {
            throw new InvalidOperationException(
                $"Missing required configuration keys: {string.Join(", ", missingKeys)}. " +
                "Set them via environment variables or a .env file. See .env.example for reference.");
        }
    }

    private static void ValidateSection(
        IConfiguration configuration,
        string section,
        string[] keys,
        List<string> missingKeys)
    {
        foreach (var key in keys)
        {
            var value = configuration[$"{section}:{key}"];
            if (string.IsNullOrWhiteSpace(value))
                missingKeys.Add($"{section}:{key}");
        }
    }

    private static void ValidateOAuthProvider(
        IConfiguration configuration,
        string provider,
        List<string> missingKeys)
    {
        var enabled = configuration[$"OAuth:{provider}:Enabled"];
        if (string.Equals(enabled, "true", StringComparison.OrdinalIgnoreCase))
        {
            ValidateSection(configuration, $"OAuth:{provider}", new[]
            {
                "ClientId", "ClientSecret"
            }, missingKeys);
        }
    }
}
