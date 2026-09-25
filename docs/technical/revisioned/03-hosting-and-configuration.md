# 03 — Hosting and configuration

## Composition root

`Program.cs` is only:

1. Load environment files into `IConfiguration`
2. `WebApplication.CreateBuilder`
3. `builder.Services.AddPickMeUpApi(builder.Configuration)`
4. HTTP pipeline
5. `Run`

No inline Mongo construction, no `BuildServiceProvider()`, no duplicated `AddAuthentication`.

## Env loader

Correct for **local development**. Incorrect as the production secret store.

### Behaviour

- Library: `DotNetEnv`, loaded before `WebApplication.CreateBuilder`
- File order (first existing wins per key only if process env is empty): process environment → `.env.{Environment}` → `.env.local` → `.env`
- Search: content root, current directory, project directory in Development.
- **Do not overwrite** variables already set in the process (containers win).
- **Do not** load from `AppContext.BaseDirectory/../../../.env` only.

### Mapping

One class maps names:

| Env | Config key |
|---|---|
| `GOOGLE_CLIENT_ID` / `SECRET` | `Authentication:Google:*` |
| `FACEBOOK_APP_ID` / `SECRET` | `Authentication:Facebook:*` |
| `MYSQL_HOST` `PORT` `DATABASE` `USER` `PASSWORD` | `ConnectionStrings:MySql` |
| `MONGODB_CONNECTION` `MONGODB_DATABASE` | `Mongo:*` |
| `REDIS_CONNECTION` | `Redis:Connection` |
| `JWT_SECRET` `JWT_REFRESH_TOKEN` `JWT_ISSUER` `JWT_AUDIENCE` | `Jwt:*` |
| `JWT_EXPIRY_MINUTES` `JWT_REFRESH_EXPIRY_DAYS` `SESSION_EXPIRE_HOURS` | `Jwt:*` / `Session:*` |

Missing required keys fail startup with a single exception that lists names.

### Files

- `.env.example` committed, empty values
- `.env` gitignored
- `appsettings.json` non-secret defaults only (log level, database name, lifetimes)

## Pipeline

```
ExceptionHandler (ProblemDetails)
HTTPS
CORS (Development origins for interfaces + later editors)
Authentication
Authorization
MapControllers
```

Block *policy* belongs in Social services. A global block middleware that only reads route `targetUserId` is insufficient for POST bodies.

## DI modules (the C# equivalent of an index)

```
AddPickMeUpApi()
  AddPickMeUpOptions()          # bind JwtOptions, MongoOptions, MySqlOptions, RedisOptions
  AddMySql()                    # DbContext + Identity (ApplicationUser, IdentityRole<Guid>)
  AddMongoDb()                  # IMongoClient (singleton) + IMongoDatabase (singleton)
  AddRedis()                    # StackExchange Redis cache
  AddJwtAuthentication()        # JWT Bearer + Cookie auth schemes
  AddOAuthProviders()           # Google/Facebook (conditional on config)
  AddFluentValidation()         # assembly scan from AccountPreferencesRepository
  AddProblemDetails()
  AddAccountPreferences()       # currently empty stub
  AddSocial()                   # full Social module registration
```

Each module lives next to its domain as `DependencyInjection.cs`. That is the wrapper file. Not a class that "calls every feature."

## Packages

Present: JwtBearer, Google, Facebook, Identity EF, Pomelo MySQL, Mongo driver, Redis cache, FluentValidation core + ASP.NET integration, DotNetEnv, OpenAPI.

Missing for full design: explicit CORS configuration, health checks.
