using System.Text;
using System.Threading.RateLimiting;
using FluentValidation;
using HealthChecks.MongoDb;
using HealthChecks.MySql;
using HealthChecks.Redis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using PickMeUp.Api.Account.AccountPreferences;
using PickMeUp.Api.Account.Profile;
using PickMeUp.Api.DoNotTouchFolder;
using PickMeUp.Api.Social;
using StackExchange.Redis;

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
        services.AddControllers();

        services.AddAccountPreferences();
        services.AddProfile();
        services.AddSocial();

        services.AddHealthChecksServices(configuration);
        services.AddCorsPolicy();
        services.AddRateLimitingServices(configuration);

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

    // ── Health Checks ──────────────────────────────────────────

    private static void AddHealthChecksServices(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnectionString = configuration[$"{MongoOptions.SectionName}:ConnectionString"]
            ?? throw new InvalidOperationException("MongoDB connection string is not configured.");

        var mySqlConnectionString = configuration.GetConnectionString("MySql")
            ?? configuration[$"{MySqlOptions.SectionName}:ConnectionString"]
            ?? throw new InvalidOperationException("MySQL connection string is not configured.");

        var redisConnection = configuration[$"{RedisOptions.SectionName}:Connection"]
            ?? throw new InvalidOperationException("Redis connection string is not configured.");

        services.AddHealthChecks();
    }

    // ── CORS ────────────────────────────────────────────────────

    private static void AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("dev", policy =>
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }

    // ── Rate Limiting ───────────────────────────────────────────

    private static void AddRateLimitingServices(this IServiceCollection services, IConfiguration configuration)
    {
        var rateLimitOptions = configuration
            .GetSection(RateLimitOptions.SectionName)
            .Get<RateLimitOptions>() ?? new RateLimitOptions();

        services.Configure<RateLimitOptions>(
            configuration.GetSection(RateLimitOptions.SectionName));

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddFixedWindowLimiter("fixed-window", limiterOptions =>
            {
                limiterOptions.PermitLimit = rateLimitOptions.PermitLimit;
                limiterOptions.Window = TimeSpan.FromSeconds(rateLimitOptions.WindowSeconds);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = rateLimitOptions.QueueLimit;
            });
        });
    }
}
