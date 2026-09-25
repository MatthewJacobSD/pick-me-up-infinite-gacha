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

- Library: existing `DotNetEnv`, wrapped as `IConfigurationSource` or loaded into configuration before bind.
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

## Pipeline (target)

```
Exception handler / ProblemDetails
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
  AddPickMeUpEnv-bound options
  AddPickMeUpAuth()
  AddPickMeUpPreferences()
  AddPickMeUpSocial()
  AddFluentValidation()
  AddMongo()
  AddIdentityMySql()
  AddRedis()
```

Each module lives next to its domain as `DependencyInjection.cs`. That is the wrapper file. Not a class that “calls every feature.”

## Packages to add vs what exists

Present: JwtBearer, Google, Facebook, Identity EF, Pomelo MySQL, Mongo driver, Redis cache, FluentValidation core, DotNetEnv, OpenAPI.

Missing for the design: FluentValidation ASP.NET integration (or a custom filter), explicit CORS, health checks.

## Health

`GET /health` — process up  
`GET /ready` — Mongo ping + MySQL `CanConnect` + Redis ping  

Required before a client team is told the API is up.
