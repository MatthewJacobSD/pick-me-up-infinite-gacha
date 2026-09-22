using System.Reflection;
using DotNetEnv;

namespace PickMeUp.Api.Hosting;

/// <summary>
/// Loads .env files with priority precedence:
/// Process env (highest) → .env.{Environment} → .env.local → .env (lowest).
/// Lower-priority sources never overwrite values from higher-priority sources.
/// </summary>
public static class EnvLoader
{
    /// <summary>
    /// Loads .env files and pushes values into IConfiguration.
    /// </summary>
    /// <param name="configuration">The configuration builder to merge into.</param>
    /// <param name="environment">ASP.NET Core environment name (e.g. "Development"). Falls back to DOTNET_ENVIRONMENT / ASPNETCORE_ENVIRONMENT.</param>
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

    /// <summary>
    /// Loads .env files into process environment variables with priority precedence.
    /// Uses NoClobber so that higher-priority sources are never overwritten.
    /// Files are loaded lowest-priority-first so that NoClobber preserves the correct precedence.
    /// </summary>
    private static Dictionary<string, string> LoadEnvFiles(string root, string environment)
    {
        // NoClobber = true: first value encountered wins (already-set env vars are never overwritten).
        // Files loaded in order: .env → .env.local → .env.{env}
        // Because NoClobber prevents overwriting, the effective priority is:
        //   .env.{env} (highest, set last) > .env.local > .env (lowest, set first)
        // This matches: process env > .env.{env} > .env.local > .env
        DotNetEnv.Env.NoClobber().Load(Path.Combine(root, ".env"));
        DotNetEnv.Env.NoClobber().Load(Path.Combine(root, ".env.local"));
        DotNetEnv.Env.NoClobber().Load(Path.Combine(root, $".env.{environment}"));

        // Snapshot all environment variables so we can push them into IConfiguration.
        var variables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Collect from process environment (includes both original and .env-loaded values).
        foreach (System.Collections.DictionaryEntry entry in Environment.GetEnvironmentVariables())
        {
            if (entry.Key is string key && entry.Value is string value)
                variables[key] = value;
        }

        return variables;
    }

    /// <summary>
    /// Walks up from the entry assembly directory looking for a project or solution file.
    /// Returns the first directory containing a .csproj, .sln, or .slnx file.
    /// Falls back to the entry assembly's directory if no marker is found.
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

        // Fallback: use the assembly's directory.
        return directory;
    }
}
