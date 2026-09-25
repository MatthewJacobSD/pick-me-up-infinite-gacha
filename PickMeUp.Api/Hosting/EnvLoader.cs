using System.Reflection;
using DotNetEnv;

namespace PickMeUp.Api.Hosting;

/// <summary>
/// Loads .env files with priority precedence:
/// Process env (highest) → .env.{Environment} → .env.local → .env (lowest).
/// Lower-priority sources never overwrite values from higher-priority sources.
/// Flat env var names (JWT_SECRET) are mapped to nested config keys (Jwt:Secret).
/// </summary>
public static class EnvLoader
{
    /// <summary>
    /// Loads .env files and pushes values into IConfiguration.
    /// </summary>
    public static void Load(IConfigurationBuilder configuration, string? environment = null)
    {
        var env = environment
            ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? "Production";

        var root = FindContentRoot();
        var variables = LoadEnvFiles(root, env);
        configuration.AddInMemoryCollection(variables);
    }

    private static Dictionary<string, string> LoadEnvFiles(string root, string environment)
    {
        DotNetEnv.Env.NoClobber().Load(Path.Combine(root, ".env"));
        DotNetEnv.Env.NoClobber().Load(Path.Combine(root, ".env.local"));
        DotNetEnv.Env.NoClobber().Load(Path.Combine(root, $".env.{environment}"));

        var variables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (System.Collections.DictionaryEntry entry in Environment.GetEnvironmentVariables())
        {
            if (entry.Key is string key && entry.Value is string value)
                variables[key] = value;
        }

        // Map flat env var names → nested config keys.
        var envToConfig = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["JWT_SECRET"] = "Jwt:Secret",
            ["JWT_REFRESH_TOKEN"] = "Jwt:RefreshToken",
            ["JWT_ISSUER"] = "Jwt:Issuer",
            ["JWT_AUDIENCE"] = "Jwt:Audience",
            ["JWT_EXPIRY_MINUTES"] = "Jwt:ExpiryMinutes",
            ["JWT_REFRESH_EXPIRY_DAYS"] = "Jwt:RefreshExpiryDays",
            ["SESSION_EXPIRE_HOURS"] = "Session:ExpireHours",
            ["MONGODB_CONNECTION_STRING"] = "Mongo:ConnectionString",
            ["MONGODB_DATABASE"] = "Mongo:Database",
            ["REDIS_CONNECTION"] = "Redis:Connection",
            ["MYSQL_HOST"] = "MySql:Host",
            ["MYSQL_PORT"] = "MySql:Port",
            ["MYSQL_DATABASE"] = "MySql:Database",
            ["MYSQL_USER"] = "MySql:User",
            ["MYSQL_PASSWORD"] = "MySql:Password",
            ["GOOGLE_CLIENT_ID"] = "OAuth:Google:ClientId",
            ["GOOGLE_CLIENT_SECRET"] = "OAuth:Google:ClientSecret",
            ["GOOGLE_ENABLED"] = "OAuth:Google:Enabled",
            ["FACEBOOK_APP_ID"] = "OAuth:Facebook:ClientId",
            ["FACEBOOK_APP_SECRET"] = "OAuth:Facebook:ClientSecret",
            ["FACEBOOK_ENABLED"] = "OAuth:Facebook:Enabled",
        };

        // Build MySql connection string from individual parts.
        if (variables.TryGetValue("MYSQL_HOST", out var host) &&
            variables.TryGetValue("MYSQL_PORT", out var port) &&
            variables.TryGetValue("MYSQL_DATABASE", out var db) &&
            variables.TryGetValue("MYSQL_USER", out var user) &&
            variables.TryGetValue("MYSQL_PASSWORD", out var pass))
        {
            variables["MySql:ConnectionString"] = $"Server={host};Port={port};Database={db};User={user};Password={pass};";
        }

        foreach (var (envKey, configKey) in envToConfig)
        {
            if (variables.TryGetValue(envKey, out var envValue))
                variables[configKey] = envValue;
        }

        return variables;
    }

    /// <summary>
    /// Walks up from the entry assembly directory looking for a project or solution file.
    /// </summary>
    private static string FindContentRoot()
    {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var directory = Path.GetDirectoryName(assembly.Location)
            ?? AppContext.BaseDirectory;

        var current = new DirectoryInfo(directory);
        while (current is not null)
        {
            if (current.EnumerateFiles("*.*proj").Any()
                || current.EnumerateFiles("*.sln").Any()
                || current.EnumerateFiles("*.slnx").Any())
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        return directory;
    }
}
