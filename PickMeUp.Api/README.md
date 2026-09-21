# PickMeUp.Api — Backend API

**Runtime:** .NET 10 · **Language:** C# · **Database:** MySQL + Redis

---

## System Architecture

```mermaid
graph TB
    subgraph Clients
        U[("Unity Client — v1/")]
        UE[("Unreal Client — v2/")]
    end

    subgraph Backend["Backend — PickMeUp.Api"]
        direction TB
        Auth["Authentication Layer<br>OAuth · JWT · Sessions"]
        API["API Contracts<br>REST Endpoints"]
        Data["Data Layer<br>EF Core · MySQL"]
        Cache["Cache Layer<br>Redis"]
    end

    subgraph Providers["External Providers"]
        G[("Google")]
        F[("Facebook")]
    end

    subgraph Storage
        MySQL[("MySQL<br>Accounts · Identity")]
        Redis[("Redis<br>Sessions · Tokens · CSRF")]
    end

    U -->|REST| Auth
    UE -->|REST| Auth
    Auth --> API
    API --> Data
    Auth --> Cache
    Data --> MySQL
    Cache --> Redis
    Auth -->|OAuth| G
    Auth -->|OAuth| F

    style Backend fill:#1a1a2e,stroke:#e94560,color:#fff
    style Clients fill:#0f3460,stroke:#16213e,color:#fff
    style Providers fill:#533483,stroke:#e94560,color:#fff
    style Storage fill:#16213e,stroke:#0f3460,color:#fff
```

---

## Project Structure

```
PickMeUp.Api/
├── Program.cs                         Entry point, service wiring
│
├── Account/
│   ├── ApplicationDbContext.cs        EF Core Identity DbContext
│   ├── ApplicationUser.cs             User entity (Identity)
│   ├── Settings.cs                    Game settings (placeholder)
│   │
│   ├── Authentication/
│   │   ├── Email.cs                   Email value object
│   │   ├── Password.cs                Password value object (hashed)
│   │   │
│   │   ├── OAuth/                     ── OAuth Flow ──
│   │   │   ├── Provider/
│   │   │   │   ├── GoogleProvider.cs       Google config value object
│   │   │   │   ├── FacebookProvider.cs     Facebook config value object
│   │   │   │   ├── Google/                 Google API DTOs
│   │   │   │   └── Facebook/               Facebook API DTOs
│   │   │   │
│   │   │   ├── UnifiedAuthController.cs    Main auth controller
│   │   │   ├── ExternalLoginService.cs     Redirect URL builder
│   │   │   ├── OAuthCallbackHandler.cs     Code exchange + userinfo
│   │   │   ├── AccountCreationService.cs   New account from OAuth
│   │   │   ├── AccountLinkingService.cs    Link OAuth to account
│   │   │   ├── OAuthStateValidator.cs      CSRF protection (Redis)
│   │   │   ├── OAuthConfigLoader.cs        Reads config → provider objects
│   │   │   ├── OAuthProviderRegistry.cs    Provider lookup
│   │   │   ├── OAuthErrorHandler.cs        HTTP + JSON error handling
│   │   │   ├── OAuthExceptions.cs          Exception hierarchy
│   │   │   ├── ExternalIdentity.cs         Normalised identity record
│   │   │   └── OAuthProviderExtentions.cs  Enum + extension methods
│   │   │
│   │   └── Session/                   ── JWT + Sessions ──
│   │       ├── Jwt.cs                     JWT config root
│   │       ├── TokenConfig.cs             Base token config
│   │       ├── AccessTokenConfig.cs       Short-lived (minutes)
│   │       ├── RefreshTokenConfig.cs      Long-lived (days)
│   │       ├── TokenGeneratorService.cs   Token creation
│   │       ├── RefreshTokenService.cs     Token rotation
│   │       ├── SessionService.cs          Server-side sessions
│   │       ├── SessionConfig.cs           Session config
│   │       └── RefreshController.cs       Refresh endpoint
│   │
│   ├── Profile/
│   │   ├── Avatar.cs                  Avatar value object
│   │   └── Username.cs                Username value object
│   │
│   └── UserCenter/                    User center (placeholder)
│       ├── Agreement.cs
│       ├── BindAccount.cs
│       ├── OTP.cs
│       └── TestCenter/
│
└── Properties/
```

---

## Stack

| Layer | Technology | Purpose |
|---|---|---|
| Runtime | .NET 10 | Host |
| Database | MySQL (Pomelo EF Core 9.0) | Account persistence |
| Cache | Redis (StackExchange) | Sessions, refresh tokens, CSRF state |
| Identity | ASP.NET Identity (EF Core) | User management, password hashing |
| Auth | Google OAuth, Facebook OAuth | External sign-in |
| Tokens | JWT (HMAC-SHA256) | API authentication |
| Env | DotNetEnv 3.1 | Secret loading from `.env` |
| API | OpenAPI | Documentation |

---

## Authentication Flow

### OAuth Login

```mermaid
sequenceDiagram
    autonumber
    participant U as Unity Client
    participant B as Backend API
    participant P as OAuth Provider
    participant R as Redis

    U->>B: GET /api/auth/login/google
    B->>R: Generate CSRF state (256-bit)
    R-->>B: state stored (5 min TTL)
    B-->>U: { redirectUrl }

    Note over U,P: Browser opens provider URL

    U->>P: User authenticates
    P-->>B: GET /api/auth/callback/google?code=...&state=...

    B->>R: Validate state
    R-->>B: valid (single-use, delete)

    B->>P: POST /token (exchange code)
    P-->>B: { access_token }

    B->>P: GET /userinfo
    P-->>B: { id, email, name }

    B->>B: Create/link account
    B->>R: Store session (account_id, TTL)
    B->>R: Store loginCode (2 min TTL)

    B-->>U: Redirect to /auth/complete?loginCode=...

    U->>B: POST /consume-login-code
    B->>R: Validate loginCode
    R-->>B: session_id

    B->>B: Generate JWT access + refresh tokens
    B->>R: Store refresh token (TTL)
    B-->>U: { sessionId, accessToken, refreshToken }
```

### Token Lifecycle

```mermaid
stateDiagram-v2
    [*] --> Authenticated: Login / OAuth callback

    Authenticated --> Active: Got access + refresh tokens

    Active --> Active: API call with valid access token

    Active --> AccessExpired: Access token expired (10 min)

    AccessExpired --> Active: POST /refresh, new tokens
    AccessExpired --> Expired: Refresh token invalid or expired (7 days)

    Active --> LoggedOut: POST /logout
    AccessExpired --> LoggedOut: POST /logout

    Expired --> [*]
    LoggedOut --> [*]
```

---

## Entity Relationships

```mermaid
erDiagram
    ApplicationUser {
        Guid Id PK
        string Username
        bool IsActive
        bool IsLocked
        DateTime CreatedAtUtc
    }

    Email {
        string Address
        EmailProvider Provider
    }

    Password {
        string Hash
    }

    Username {
        string Value
    }

    Avatar {
        Guid Id PK
        AvatarTypeStatus AvatarType
        string Value
        string AvatarUrlPath
        bool IsDefault
    }

    ExternalAccountLink {
        Guid AccountId FK
        OAuthProvider Provider
        string ExternalId
        string Email
    }

    ApplicationUser ||--|| Email : "has"
    ApplicationUser ||--o| Password : "has, null for OAuth"
    ApplicationUser ||--|| Username : "has"
    ApplicationUser ||--|| Avatar : "has"
    ApplicationUser ||--o{ ExternalAccountLink : "linked to"
```

---

## Middleware Pipeline

```mermaid
graph LR
    Request["Incoming Request"] --> HTTPS["HTTPS Redirection"]
    HTTPS --> AuthN["Authentication<br>JWT Bearer"]
    AuthN --> AuthZ["Authorization"]
    AuthZ --> Controller["Controller"]
    Controller --> Response["Response"]

    style Request fill:#533483,color:#fff
    style Response fill:#533483,color:#fff
    style AuthN fill:#e94560,color:#fff
    style AuthZ fill:#e94560,color:#fff
```

---

## Folder Dependency Map

```mermaid
graph TD
    Program["Program.cs"] --> JWT["Jwt.cs"]
    Program --> Redis["Redis Cache"]
    Program --> MySQL["MySQL / EF Core"]
    Program --> Identity["ASP.NET Identity"]

    JWT --> AccessTokenConfig
    JWT --> RefreshTokenConfig
    JWT --> SessionConfig

    AccessTokenConfig --> TokenConfig
    RefreshTokenConfig --> TokenConfig

    TokenGeneratorService --> JWT
    RefreshTokenService --> TokenGeneratorService
    RefreshTokenService --> Redis
    SessionService --> Redis

    UnifiedAuthController --> ExternalLoginService
    UnifiedAuthController --> OAuthCallbackHandler
    UnifiedAuthController --> AccountCreationService
    UnifiedAuthController --> AccountLinkingService
    UnifiedAuthController --> SessionService
    UnifiedAuthController --> TokenGeneratorService
    UnifiedAuthController --> RefreshTokenService

    ExternalLoginService --> OAuthProviderRegistry
    ExternalLoginService --> OAuthStateValidator
    OAuthCallbackHandler --> OAuthProviderRegistry
    OAuthCallbackHandler --> OAuthStateValidator
    OAuthCallbackHandler --> HttpClient

    OAuthProviderRegistry --> OauthConfig
    OAuthConfigLoader --> OauthConfig

    AccountCreationService --> IAccountRepository
    AccountLinkingService --> IExternalAccountRepository

    ApplicationUser --> Email
    ApplicationUser --> Password
    ApplicationUser --> Username
    ApplicationUser --> Avatar

    style Program fill:#0f3460,color:#fff
    style UnifiedAuthController fill:#e94560,color:#fff
    style TokenGeneratorService fill:#533483,color:#fff
    style SessionService fill:#533483,color:#fff
```

---

## Configuration

All secrets are loaded from `.env` via DotNetEnv.

| Variable | Purpose |
|---|---|
| `GOOGLE_CLIENT_ID` / `GOOGLE_CLIENT_SECRET` | Google OAuth |
| `FACEBOOK_APP_ID` / `FACEBOOK_APP_SECRET` | Facebook OAuth |
| `MYSQL_HOST` / `MYSQL_PORT` / `MYSQL_DATABASE` / `MYSQL_USER` / `MYSQL_PASSWORD` | MySQL connection |
| `REDIS_CONNECTION` | Redis connection string |
| `JWT_SECRET` | Access token signing key |
| `JWT_REFRESH_TOKEN` | Refresh token signing key |
| `JWT_ISSUER` / `JWT_AUDIENCE` | JWT claims |
| `JWT_EXPIRY_MINUTES` | Access token lifetime |
| `JWT_REFRESH_EXPIRY_DAYS` | Refresh token lifetime |
| `SESSION_EXPIRE_HOURS` | Server-side session lifetime |

---

## Running

```bash
dotnet restore
dotnet run
```

Starts on `http://localhost:5137` · OpenAPI at `/openapi` in development.
