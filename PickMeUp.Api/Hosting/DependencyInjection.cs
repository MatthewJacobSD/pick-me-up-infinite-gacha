using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using PickMeUp.Api.Account.AccountPreferences;
using PickMeUp.Api.DoNotTouchFolder;
using PickMeUp.Api.Social;

namespace PickMeUp.Api.Hosting;

public static class DependencyInjection
{
    public static IServiceCollection AddPickMeUpApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPickMeUpOptions(configuration);

        services.AddMySql(configuration);
        services.AddMongoDb(configuration);
        services.AddRedis(configuration);
        services.AddJwtAuthentication(configuration);
        services.AddOAuthProviders(configuration);
        services.AddFluentValidation();
        services.AddProblemDetails();

        services.AddAccountPreferences();
        services.AddSocial();

        return services;
    }

    // ── MySQL + Identity ────────────────────────────────────────

    private static void AddMySql(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MySql")
            ?? configuration["MySql:ConnectionString"]
            ?? throw new InvalidOperationException("MySQL connection string is not configured.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(connectionString)));

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }

    // ── MongoDB ─────────────────────────────────────────────────

    private static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoOptions = configuration
            .GetSection(MongoOptions.SectionName)
            .Get<MongoOptions>()
            ?? throw new InvalidOperationException("Mongo configuration section is missing.");

        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoOptions.ConnectionString));
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoOptions.Database);
        });
    }

    // ── Redis ───────────────────────────────────────────────────

    private static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration[$"{RedisOptions.SectionName}:Connection"];
        if (string.IsNullOrWhiteSpace(connection))
            throw new InvalidOperationException("Redis connection string is not configured.");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connection;
            options.InstanceName = "PickMeUp_";
        });
    }

    // ── JWT Bearer Authentication ───────────────────────────────

    private static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection(JwtOptions.SectionName);
        var jwtOptions = jwtSection.Get<JwtOptions>()
            ?? throw new InvalidOperationException("JWT configuration section is missing.");

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                    ClockSkew = TimeSpan.FromMinutes(1),
                };
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
            });

        services.AddAuthorization();
    }

    // ── OAuth Providers (conditional) ───────────────────────────

    private static void AddOAuthProviders(this IServiceCollection services, IConfiguration configuration)
    {
        var googleEnabled = configuration["OAuth:Google:Enabled"];
        if (string.Equals(googleEnabled, "true", StringComparison.OrdinalIgnoreCase))
        {
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = configuration["OAuth:Google:ClientId"]
                        ?? throw new InvalidOperationException("OAuth:Google:ClientId is not configured.");
                    options.ClientSecret = configuration["OAuth:Google:ClientSecret"]
                        ?? throw new InvalidOperationException("OAuth:Google:ClientSecret is not configured.");
                    options.CallbackPath = "/api/auth/callback/google";
                });
        }

        var facebookEnabled = configuration["OAuth:Facebook:Enabled"];
        if (string.Equals(facebookEnabled, "true", StringComparison.OrdinalIgnoreCase))
        {
            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = configuration["OAuth:Facebook:ClientId"]
                        ?? throw new InvalidOperationException("OAuth:Facebook:ClientId is not configured.");
                    options.AppSecret = configuration["OAuth:Facebook:ClientSecret"]
                        ?? throw new InvalidOperationException("OAuth:Facebook:ClientSecret is not configured.");
                    options.CallbackPath = "/api/auth/callback/facebook";
                });
        }
    }

    // ── FluentValidation ────────────────────────────────────────

    private static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AccountPreferencesRepository>();
    }
}
