using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PickMeUp.Api.Account;
using PickMeUp.Api.Account.Authentication.Session;
using PickMeUp.Api.Account.Profile.ProfileSettings.Social;
using System.Text;

// ── Load .env ────────────────────────────────────────────────────
Env.Load(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".env"));

var builder = WebApplication.CreateBuilder(args);

// ── Configuration (from .env) ────────────────────────────────────
var config = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        // OAuth
        ["Authentication:Google:ClientId"] = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID"),
        ["Authentication:Google:ClientSecret"] = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET"),
        ["Authentication:Facebook:ClientId"] = Environment.GetEnvironmentVariable("FACEBOOK_APP_ID"),
        ["Authentication:Facebook:ClientSecret"] = Environment.GetEnvironmentVariable("FACEBOOK_APP_SECRET"),

        // MySQL
        ["ConnectionStrings:DefaultConnection"] =
            $"Server={Environment.GetEnvironmentVariable("MYSQL_HOST")};" +
            $"Port={Environment.GetEnvironmentVariable("MYSQL_PORT")};" +
            $"Database={Environment.GetEnvironmentVariable("MYSQL_DATABASE")};" +
            $"User={Environment.GetEnvironmentVariable("MYSQL_USER")};" +
            $"Password={Environment.GetEnvironmentVariable("MYSQL_PASSWORD")};",

        // JWT
        ["Jwt:Secret"] = Environment.GetEnvironmentVariable("JWT_SECRET"),
        ["Jwt:RefreshToken"] = Environment.GetEnvironmentVariable("JWT_REFRESH_TOKEN"),
        ["Jwt:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ["Jwt:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        ["Jwt:ExpiryMinutes"] = Environment.GetEnvironmentVariable("JWT_EXPIRY_MINUTES"),
        ["Jwt:RefreshExpiryDays"] = Environment.GetEnvironmentVariable("JWT_REFRESH_EXPIRY_DAYS"),

        // Session
        ["Session:ExpireHours"] = Environment.GetEnvironmentVariable("SESSION_EXPIRE_HOURS"),

        // Redis
        ["Redis:Connection"] = Environment.GetEnvironmentVariable("REDIS_CONNECTION"),
    })
    .Build();

builder.Configuration.AddConfiguration(config);

// ── OpenAPI ──────────────────────────────────────────────────────
builder.Services.AddOpenApi();

// ── Redis Cache (required for refresh tokens + sessions) ─────────
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = config["Redis:Connection"];
});

// ── Bind JWT configuration ───────────────────────────────────────
builder.Services.AddSingleton(sp =>
{
    var cfg = builder.Configuration;

    var accessToken = AccessTokenConfig.Create(
        key: cfg["Jwt:Secret"]!,
        expireInMinutes: int.Parse(cfg["Jwt:ExpiryMinutes"]!)
    );

    var refreshToken = RefreshTokenConfig.Create(
        key: cfg["Jwt:RefreshToken"]!,
        expireInDays: int.Parse(cfg["Jwt:RefreshExpiryDays"]!)
    );

    var sessionConfig = SessionConfig.Create(
        expireInHours: int.Parse(cfg["Session:ExpireHours"]!)
    );

    return Jwt.Create(
        issuer: cfg["Jwt:Issuer"]!,
        audience: cfg["Jwt:Audience"]!,
        accessToken: accessToken,
        refreshToken: refreshToken,
        sessionConfig: sessionConfig
    );
});

// ── Register Token + Session Services ─────────────────────────────
builder.Services.AddSingleton<TokenGeneratorService>();
builder.Services.AddSingleton<RefreshTokenService>();
builder.Services.AddSingleton<SessionService>();

// ── Authentication (Google + Facebook) ───────────────────────────
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = config["Authentication:Google:ClientId"]!;
        options.ClientSecret = config["Authentication:Google:ClientSecret"]!;
    })
    .AddFacebook(options =>
    {
        options.ClientId = config["Authentication:Facebook:ClientId"]!;
        options.ClientSecret = config["Authentication:Facebook:ClientSecret"]!;
    });

// ── JWT Authentication ───────────────────────────────────────────
var jwt = builder.Services.BuildServiceProvider().GetRequiredService<Jwt>();

builder.Services.AddAuthentication(options =>
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

        ValidIssuer = jwt.Issuer,
        ValidAudience = jwt.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt.AccessToken.Key)
        ),

        ClockSkew = TimeSpan.Zero
    };
});

// ── Social State (MongoDB) ──────────────────────────────────────
builder.Services.AddSingleton<ISocialRepository, SocialRepository>();
builder.Services.AddSingleton<IFriendRequestLifecycleEngine, FriendRequestLifecycleEngine>();
builder.Services.AddScoped<ISocialService, SocialService>();


// ── Identity + EF Core (MySQL) ───────────────────────────────────
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequiredLength = 12;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        config["ConnectionStrings:DefaultConnection"],
        ServerVersion.AutoDetect(config["ConnectionStrings:DefaultConnection"])
    ));

builder.Services.AddControllers();
builder.Services.AddAuthorization();

// ── Build App ─────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<BlockEnforcementMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
