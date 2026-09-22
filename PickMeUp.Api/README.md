# PickMeUp.Api — Backend API

**Runtime:** .NET 10 · **Language:** C# · **Database:** MySQL + MongoDB + Redis

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
        Mongo["MongoDB<br>Social · Preferences"]
        Cache["Cache Layer<br>Redis"]
    end

    subgraph Providers["External Providers"]
        G[("Google")]
        F[("Facebook")]
    end

    subgraph Storage
        MySQL[("MySQL<br>Accounts · Identity")]
        MongoDB[("MongoDB<br>Social · Preferences")]
        Redis[("Redis<br>Sessions · Tokens · CSRF")]
    end

    U -->|REST| Auth
    UE -->|REST| Auth
    Auth --> API
    API --> Data
    API --> Mongo
    Auth --> Cache
    Data --> MySQL
    Mongo --> MongoDB
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
├── Program.cs                              Entry point, service wiring
├── .csproj                                 Project file
├── .slnx                                   Solution file
├── appsettings.json                        Configuration
│
├── Account/
│   ├── ApplicationDbContext.cs             EF Core Identity DbContext
│   ├── ApplicationUser.cs                  User entity (Identity)
│   │
│   ├── Authentication/
│   │   ├── Email.cs                        Email value object
│   │   ├── Password.cs                     Password value object (hashed)
│   │   │
│   │   ├── OAuth/
│   │   │   ├── Provider/
│   │   │   │   ├── GoogleProvider.cs       Google config value object
│   │   │   │   ├── FacebookProvider.cs     Facebook config value object
│   │   │   │   ├── Google/                 Google API DTOs
│   │   │   │   └── Facebook/               Facebook API DTOs
│   │   │   ├── UnifiedAuthController.cs    Main auth controller
│   │   │   ├── ExternalLoginService.cs     Redirect URL builder
│   │   │   ├── OAuthCallbackHandler.cs     Code exchange + userinfo
│   │   │   ├── AccountCreationService.cs   New account from OAuth
│   │   │   ├── AccountLinkingService.cs    Link OAuth to account
│   │   │   ├── OAuthStateValidator.cs      CSRF protection (Redis)
│   │   │   ├── OAuthConfigLoader.cs        Config → provider objects
│   │   │   ├── OAuthProviderRegistry.cs    Provider lookup
│   │   │   ├── OAuthErrorHandler.cs        HTTP + JSON error handling
│   │   │   ├── OAuthExceptions.cs          Exception hierarchy
│   │   │   ├── ExternalIdentity.cs         Normalised identity record
│   │   │   └── OAuthProviderExtentions.cs  Enum + extensions
│   │   │
│   │   └── Session/
│   │       ├── Jwt.cs                      JWT config root
│   │       ├── TokenConfig.cs              Base token config
│   │       ├── AccessTokenConfig.cs        Short-lived (minutes)
│   │       ├── RefreshTokenConfig.cs       Long-lived (days)
│   │       ├── TokenGeneratorService.cs    Token creation
│   │       ├── RefreshTokenService.cs      Token rotation
│   │       ├── SessionService.cs           Server-side sessions
│   │       ├── SessionConfig.cs            Session config
│   │       └── RefreshController.cs        Refresh endpoint
│   │
│   ├── Profile/
│   │   ├── Avatar.cs                       Avatar value object
│   │   ├── Username.cs                     Username value object
│   │   │
│   │   └── ProfileSettings/
│   │       ├── AccountPreferencesDocument.cs   MongoDB document
│   │       ├── AccountPreferenceRepository.cs  MongoDB repository
│   │       ├── IAccountPreferenceRepository.cs Repository interface
│   │       │
│   │       ├── Gameplay/
│   │       │   ├── GameplaySettings.cs         Domain model (20 settings)
│   │       │   ├── GameplaySettingsDto.cs      DTO
│   │       │   ├── GameplaySettingsController.cs  GET + PUT
│   │       │   └── GameplaySettingsValidator.cs   FluentValidation
│   │       │
│   │       ├── Accessibility/
│   │       │   ├── AccessibilitySettings.cs    Domain model (14 settings)
│   │       │   ├── AccessibilitySettingsDto.cs DTO
│   │       │   ├── AccessibilitySettingsController.cs  GET + PUT
│   │       │   └── AccessibilitySettingsValidator.cs   FluentValidation
│   │       │
│   │       ├── Language/
│   │       │   ├── LanguageSettings.cs         Domain model
│   │       │   ├── LanaguageSettingsDto.cs     DTO
│   │       │   ├── LanguageSettingsController.cs  GET + PUT
│   │       │   └── LanguageSettingsValidator.cs   FluentValidation
│   │       │
│   │       ├── Notifications/
│   │       │   ├── NotificationSettings.cs     Domain model (5 toggles)
│   │       │   ├── NotificationSettingsDto.cs  DTO
│   │       │   ├── NotificationSettingsController.cs  GET + PUT
│   │       │   └── NotificationSettingsValidator.cs   FluentValidation
│   │       │
│   │       └── Social/
│   │           ├── SocialState.cs              Domain models
│   │           ├── SocialDocument.cs           MongoDB document
│   │           ├── ISocialRepository.cs        Repository interface
│   │           ├── SocialRepository.cs         MongoDB implementation
│   │           ├── ISocialService.cs           Service interface
│   │           ├── SocialService.cs            Service implementation
│   │           ├── IFriendRequestLifecycleEngine.cs  Request lifecycle
│   │           ├── BlockEnforcementMiddleware.cs     Block check middleware
│   │           ├── SocialController.cs         REST endpoints
│   │           ├── FriendCommand.cs            Friend action command
│   │           ├── BlockCommand.cs             Block action command
│   │           └── PartyCommand.cs             Party invite command
│   │
│   └── UserCenter/                         (placeholder, not committed)
│
└── Properties/
```

---

## Stack

| Layer | Technology | Purpose |
|---|---|---|
| Runtime | .NET 10 | Host |
| Database | MySQL (Pomelo EF Core 9.0) | Account persistence |
| Database | MongoDB (Driver 3.12) | Social, preferences |
| Cache | Redis (StackExchange) | Sessions, tokens, CSRF |
| Identity | ASP.NET Identity (EF Core) | User management |
| Auth | Google OAuth, Facebook OAuth | External sign-in |
| Tokens | JWT (HMAC-SHA256) | API authentication |
| Validation | FluentValidation 11.11 | Input validation |
| Env | DotNetEnv 3.1 | Secret loading |
| API | OpenAPI | Documentation |

---

## API Endpoints

### Authentication

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/auth/login/{provider}` | Get OAuth redirect URL |
| GET | `/api/auth/callback/{provider}` | OAuth callback |
| POST | `/api/auth/consume-login-code` | Exchange loginCode for tokens |
| POST | `/api/auth/refresh` | Refresh access token |
| POST | `/api/auth/logout` | Revoke session |

### Account Preferences

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/account/preferences/gameplay` | Get gameplay settings |
| PUT | `/account/preferences/gameplay` | Update gameplay settings |
| GET | `/account/preferences/accessibility` | Get accessibility settings |
| PUT | `/account/preferences/accessibility` | Update accessibility settings |
| GET | `/account/preferences/language` | Get language settings |
| PUT | `/account/preferences/language` | Update language settings |
| GET | `/account/preferences/notifications` | Get notification settings |
| PUT | `/account/preferences/notifications` | Update notification settings |

### Social

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/account/social/friends` | List friends |
| POST | `/account/social/friends/requests` | Send friend request |
| DELETE | `/account/social/friends/{id}` | Remove friend |
| GET | `/account/social/blocks` | List blocks |
| POST | `/account/social/blocks` | Block user |
| DELETE | `/account/social/blocks/{id}` | Unblock user |
| POST | `/account/social/party` | Party invite |

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
