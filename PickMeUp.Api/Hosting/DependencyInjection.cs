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
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using PickMeUp.Api.Account.AccountPreferences;
using PickMeUp.Api.Account.Authentication.OAuth;
using PickMeUp.Api.Account.Authentication.OAuth.Provider;
using PickMeUp.Api.Account.Authentication.Session;
using PickMeUp.Api.Account.Profile;
using PickMeUp.Api.DoNotTouchFolder;
using PickMeUp.Api.Social;
using StackExchange.Redis;

namespace PickMeUp.Api.Hosting;

/// <summary>
/// Composes all application services: infrastructure, auth, features, health checks, CORS, and rate limiting.
/// </summary>
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

        /**--------[OAuth]--------**/

        services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var googleClientId = config["OAuth:Google:ClientId"] ?? "";
            var googleSecret = config["OAuth:Google:ClientSecret"] ?? "";
            var facebookAppId = config["OAuth:Facebook:ClientId"] ?? "";
            var facebookSecret = config["OAuth:Facebook:ClientSecret"] ?? "";

            var google = !string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleSecret)
                ? PickMeUp.Api.Account.Authentication.OAuth.Provider.GoogleProvider.Create(googleClientId, googleSecret)
                : PickMeUp.Api.Account.Authentication.OAuth.Provider.GoogleProvider.Create("disabled", "disabled-placeholder-value");

            var facebook = !string.IsNullOrWhiteSpace(facebookAppId) && !string.IsNullOrWhiteSpace(facebookSecret)
                ? PickMeUp.Api.Account.Authentication.OAuth.Provider.FacebookProvider.Create(facebookAppId, facebookSecret)
                : PickMeUp.Api.Account.Authentication.OAuth.Provider.FacebookProvider.Create("disabled", "disabled-placeholder-value");

            return new OauthConfig { Google = google, Facebook = facebook };
        });
        services.AddSingleton<OAuthProviderRegistry>();
        services.AddSingleton<OAuthConfigLoader>();
        services.AddSingleton<ExternalLoginService>();
        services.AddSingleton<OAuthCallbackHandler>();
        services.AddSingleton<OAuthStateValidator>();
        services.AddSingleton<IAccountRepository, MongoAccountRepository>();
        services.AddSingleton<IExternalAccountRepository, MongoExternalAccountRepository>();
        services.AddSingleton<AccountCreationService>();
        services.AddSingleton<AccountLinkingService>();

        /**--------[Session]--------**/

        services.AddSingleton(sp =>
        {
            var jwtSection = configuration.GetSection(JwtOptions.SectionName);
            var jwtOptions = jwtSection.Get<JwtOptions>()
                ?? throw new InvalidOperationException("JWT configuration section is missing.");

            var accessToken = AccessTokenConfig.Create(jwtOptions.Secret, jwtOptions.ExpiryMinutes);
            var refreshToken = RefreshTokenConfig.Create(jwtOptions.RefreshToken, jwtOptions.RefreshExpiryDays);
            var sessionConfig = SessionConfig.Create(24); // default 24 hours

            return Jwt.Create(jwtOptions.Issuer, jwtOptions.Audience, accessToken, refreshToken, sessionConfig);
        });
        services.AddSingleton(sp => sp.GetRequiredService<Jwt>().AccessToken);
        services.AddSingleton(sp => sp.GetRequiredService<Jwt>().RefreshToken);
        services.AddSingleton(sp => sp.GetRequiredService<Jwt>().SessionConfig);
        services.AddHttpClient();
        services.AddSingleton<SessionService>();
        services.AddSingleton<TokenGeneratorService>();
        services.AddSingleton<RefreshTokenService>();

        /**--------[Features]--------**/

        services.AddAccountPreferences();
        services.AddProfile();
        services.AddSocial();

        services.AddHealthChecksServices(configuration);
        services.AddCorsPolicy();
        services.AddRateLimitingServices(configuration);

        return services;
    }

    /**--------[MySQL + Identity]--------**/

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

    /**--------[MongoDB]--------**/

    private static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Guid serializer globally — required for MongoDB Driver 3.x
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

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

    /**--------[Redis]--------**/

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

    /**--------[JWT Bearer Authentication]--------**/

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

    /**--------[OAuth Providers]--------**/

    // Custom AuthController handles the entire OAuth flow.
    // Do NOT register built-in AddGoogle()/AddFacebook() — they intercept
    // the callback and conflict with the custom OAuthStateValidator.
    private static void AddOAuthProviders(this IServiceCollection services, IConfiguration configuration)
    {
        // No-op: OAuth is handled by the custom AuthController + OAuthCallbackHandler.
    }

    /**--------[FluentValidation]--------**/

    private static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AccountPreferencesRepository>();
    }

    /**--------[Health Checks]--------**/

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

    /**--------[CORS]--------**/

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

    /**--------[Rate Limiting]--------**/

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
