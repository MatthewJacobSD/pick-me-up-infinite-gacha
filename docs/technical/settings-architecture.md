# Pick Me Up — Settings & Configuration Architecture

> Reference document · Domain ownership, not UI layout · September 2026
>
> This is an architecture reference, not a C# implementation. It answers:
> what data exists, who owns it, where it lives, whether it syncs,
> what the server must enforce, and what must never be called a setting
> just because it appears in the Settings menu.

---

## 1. Mental Model

When classifying any piece of data, walk these questions in order:

```mermaid
flowchart TD
    Q1{"Is this about WHO the player<br>is or what they prefer?"}
    Q2{"Is this about the player's<br>relationship with another player?"}
    Q3{"Is this about THIS<br>computer / console / phone?"}
    Q4{"Does this require both the<br>player's preference AND the<br>device's capability?"}
    Q5{"Is this the final value<br>actually used by the game?"}
    Q6{"Is this an action / service<br>rather than a persistent preference?"}

    Q1 -->|"YES"| Account["Account"]
    Q1 -->|"NO"| Q2
    Q2 -->|"YES"| Social["Social"]
    Q2 -->|"NO"| Q3
    Q3 -->|"YES"| Device["Device"]
    Q3 -->|"NO"| Q4
    Q4 -->|"YES"| Hybrid["Hybrid"]
    Q4 -->|"NO"| Q5
    Q5 -->|"YES"| Runtime["Runtime"]
    Q5 -->|"NO"| Q6
    Q6 -->|"YES"| OwnDomain["Own Domain"]
    Q6 -->|"NO"| Review["Review further"]

    style Account fill:#2d6a4f,color:#fff
    style Social fill:#e76f51,color:#fff
    style Device fill:#457b9d,color:#fff
    style Hybrid fill:#9b5de5,color:#fff
    style Runtime fill:#f4a261,color:#000
    style OwnDomain fill:#264653,color:#fff
```

---

## 2. High-Level Map

Player Account owns preferences and identity. Social system owns relationships.
Device owns hardware. Runtime owns the resolved result the client actually applies.

```mermaid
graph TB
    subgraph Account["Player Account"]
        AP["Account Preferences"]
    end

    subgraph SocialDomain["Social System"]
        FR["Friends"]
        BL["Blocks"]
        RE["Requests"]
    end

    subgraph DeviceDomain["Device"]
        DC["Device Configuration"]
    end

    subgraph RuntimeDomain["Runtime"]
        EC["Effective Configuration"]
    end

    subgraph UI["Settings UI"]
        direction LR
        U1["Gameplay"]
        U2["Accessibility"]
        U3["Controls"]
        U4["Audio"]
        U5["Graphics"]
        U6["Social"]
        U7["Notifications"]
        U8["Language"]
    end

    AP --> EC
    DC --> EC
    EC --> Game["Game Client"]

    UI -->|reads| AP
    UI -->|reads| DC
    UI -->|writes| AP
    UI -->|writes| DC

    style Account fill:#2d6a4f,color:#fff
    style SocialDomain fill:#e76f51,color:#fff
    style DeviceDomain fill:#457b9d,color:#fff
    style RuntimeDomain fill:#f4a261,color:#000
    style UI fill:#264653,color:#fff
```

---

## 3. Four Data Categories

### A — Account Preferences

> "How does this player want the game to behave?"

```mermaid
flowchart LR
    subgraph AccountPreferences["Account Preferences"]
        direction TB
        GP["Gameplay"]
        AC["Accessibility"]
        SP["Social Preferences"]
        NT["Notifications"]
        LG["Language"]
        UI2["UI Preferences"]
        AU["Audio Preferences"]
    end

    GP -->|"action bars,<br>combat text,<br>camera"| Pres["Presentation"]
    AC -->|"colours,<br>subtitles,<br>reduced motion"| Access["Access"]
    SP -->|"who can message,<br>who can invite"| Rules["Rules"]
    NT -->|"I want event<br>notifications"| Pref["Preference"]
    LG -->|"English,<br>Italian, etc."| Lang["Language"]
    UI2 -->|"HUD layout,<br>widget positions"| Layout["Layout"]
    AU -->|"volume levels,<br>mute states"| Vol["Volume"]

    style AccountPreferences fill:#2d6a4f,color:#fff
```

**Owned by:** Account · **Persists:** Backend · **Syncs:** Yes · **Validated:** Yes

### B — Social State

> These are NOT settings, even if the Social tab lives in the Settings screen.

```mermaid
stateDiagram-v2
    [*] --> Pending: Request sent
    Pending --> Accepted: Request accepted
    Pending --> Rejected: Request rejected
    Pending --> Cancelled: Sender cancels
    Pending --> Expired: Time limit reached

    Accepted --> [*]: Friendship active

    state "Block" as Blocked {
        note right of Blocked
            Backend enforces:
            - Cannot message
            Cannot send request
            Cannot invite
            Cannot match
        end note
    }
```

**Owned by:** Social system · **Persists:** Backend · **Syncs:** Yes · **Authoritative:** Server

### C — Device Configuration

> What THIS machine can do and how this install is configured.

```mermaid
flowchart TB
    subgraph DeviceConfig["Device Configuration"]
        direction LR
        subgraph GFX["Graphics"]
            Res["Resolution"]
            RR["Refresh Rate"]
            VS["VSync"]
            AA["Anti-Aliasing"]
            FPS["FPS Limit"]
        end
        subgraph DSP["Display"]
            WM["Window Mode"]
            HDR["HDR"]
            SA["Safe Area"]
        end
        subgraph AUD["Audio Output"]
            OD["Output Device"]
            ID["Input Device"]
            CH["Channel Config"]
        end
        subgraph INP["Physical Input"]
            KB["Keyboard"]
            CTL["Controller"]
            TCH["Touch"]
        end
    end

    style DeviceConfig fill:#457b9d,color:#fff
```

**Owned by:** Device · **Persists:** Local · **Syncs:** No

### D — Effective Runtime Configuration

> Calculated, not a database entity.

```mermaid
flowchart TD
    A["Account Preferences<br>PreferredQuality = High"] --> Resolver["Configuration Resolver"]
    B["Device Configuration<br>GPU = weak"] --> Resolver
    C["Hardware Capabilities<br>MaxTextureQuality = Medium"] --> Resolver
    D["Game Defaults<br>HighQualityProfile"] --> Resolver
    E["Platform Constraints<br>Mobile = limited"] --> Resolver

    Resolver --> Result["Effective Runtime Config<br>TextureQuality = Medium<br>ShadowQuality = Low<br>MusicVolume = 0.50"]

    style Resolver fill:#f4a261,color:#000
    style Result fill:#2d6a4f,color:#fff
```

---

## 4. Account Preferences — Breakdown

### 4.1 Gameplay

Client presentation and interaction preferences.

> The server must never treat a client flag as permission to perform an action.

```mermaid
flowchart TB
    subgraph Gameplay["Gameplay Preferences"]
        AB["Action Bars<br>visible count, auto-hide,<br>lock, cooldowns, keybind labels"]
        CP["Combat Presentation<br>target selection UX,<br>combat text, damage numbers,<br>camera assists"]
        PN["Ping / Nameplates<br>visibility, distance, sounds"]
        EN["Enhancements<br>auto-loot UX,<br>interaction highlights,<br>visual indicators"]
    end

    style Gameplay fill:#2d6a4f,color:#fff
```

### 4.2 Accessibility

Highly personal. Normally follows the account. May be hybrid because DPI, fonts, and platform capabilities differ.

```mermaid
flowchart TB
    subgraph Access["Accessibility Preferences"]
        CO["Colours<br>colourblind filters,<br>custom presets,<br>team/enemy visibility"]
        SU["Subtitles<br>enabled, size, speaker names,<br>background opacity,<br>sound effect captions"]
        AA["Audio Assistance<br>visual audio indicators,<br>directional sound,<br>enhanced notifications"]
        GI["General<br>reduced motion, simplified UI,<br>hold vs toggle, text size,<br>high contrast"]
    end

    style Access fill:#2d6a4f,color:#fff
```

### 4.3 Social Preferences vs Social State

```mermaid
flowchart LR
    subgraph Preferences["Social Preferences"]
        FR["FriendRequests = Everyone"]
        MG["Messages = FriendsOnly"]
        IV["Invites = FriendsOnly"]
        OS["OnlineStatus = FriendsOnly"]
    end

    subgraph State["Social State"]
        FL["Friend List"]
        BL2["Block List"]
        RL["Request List"]
    end

    Preferences -->|"rules the player defines"| Enforce["Backend Enforces<br>Can A message B?"]
    State -->|"relationships the<br>backend establishes"| Store["Backend Stores"]

    style Preferences fill:#2d6a4f,color:#fff
    style State fill:#e76f51,color:#fff
```

### 4.4 Notification Preferences

> Preference is "I want event notifications."
> Delivery (push, email, in-game) is a separate system.

### 4.5 Language, UI, Audio

```mermaid
flowchart TD
    subgraph Account["Account Level"]
        Lang["PreferredLanguage = English"]
        HUD["HUD Layout Intent"]
        Vol["MusicVolume = 0.50"]
    end

    subgraph Device["Device Level"]
        Assets["Installed language assets"]
        DPI["DPI, safe area"]
        Output["Output device = Headset"]
    end

    subgraph Runtime["Runtime"]
        RL["Language = English"]
        RU["HUD scaled to device"]
        RA["Music at 0.40 to Headset"]
    end

    Account --> Runtime
    Device --> Runtime

    style Account fill:#2d6a4f,color:#fff
    style Device fill:#457b9d,color:#fff
    style Runtime fill:#f4a261,color:#000
```

---

## 5. Things That Are NOT Settings

```mermaid
flowchart LR
    subgraph NotSettings["NOT Settings"]
        QR["QR Login"]
        Auth["Authentication"]
        RC["Redeem Code"]
        FR2["Friend Relationships"]
        BL3["Block Relationships"]
        AN["Announcements"]
        MT["Maintenance Events"]
        SP["Support Tickets"]
        EC2["Account Security"]
    end

    QR -->|belongs to| AuthD["Authentication Domain"]
    RC -->|belongs to| RedD["Redemption Domain"]
    FR2 -->|belongs to| SocD["Social Domain"]
    AN -->|belongs to| ConD["Content Domain"]
    SP -->|belongs to| SupD["Support Domain"]

    style NotSettings fill:#e76f51,color:#fff
```

---

## 6. Device Configuration

> One device config object with subdomains. Do not invent a second
> "Performance Settings" owner that repeats the same fields.

### Graphics Preference vs Graphics Configuration

```mermaid
flowchart TD
    subgraph Account2["Account"]
        PG["PreferredGraphicsQuality = High"]
    end

    subgraph Device2["Device"]
        GPU["GPU capabilities"]
        VRAM["VRAM"]
        RES["Supported resolution"]
        FEAT["Supported rendering features"]
    end

    subgraph Runtime2["Runtime"]
        TQ["TextureQuality = Medium"]
        SQ["ShadowQuality = Low"]
        EQ["EffectsQuality = Medium"]
    end

    Account2 -->|"stores the INTENT"| Runtime2
    Device2 -->|"stores the CAPABILITY"| Runtime2

    style Account2 fill:#2d6a4f,color:#fff
    style Device2 fill:#457b9d,color:#fff
    style Runtime2 fill:#f4a261,color:#000
```

---

## 7. Hybrid Settings

A hybrid setting is one where the PLAYER has a preference but the DEVICE
determines how that preference can actually be realised.

```mermaid
flowchart TD
    AP2["Account Preference"] --> Validate["Validation / Resolution"]
    DC2["Device Capabilities"] --> Validate
    Validate --> ERS["Effective Runtime Setting"]

    style AP2 fill:#2d6a4f,color:#fff
    style DC2 fill:#457b9d,color:#fff
    style Validate fill:#9b5de5,color:#fff
    style ERS fill:#f4a261,color:#000
```

### Hybrid Examples

| Setting | Account Part | Device Part | Runtime Result |
|---|---|---|---|
| **Keybindings** | Logical actions (Jump, Attack) | Physical mappings (Space, A, Touch) | Active mapping for current device |
| **UI** | HUD layout intent | DPI, safe area, resolution | Scaled layout |
| **Audio** | Volume levels | Output device, channel config | Routed audio |
| **Graphics** | Preferred quality tier | GPU capabilities | Actual quality settings |
| **Accessibility** | InterfaceScale preference | DPI, font availability | Rendered scale |
| **Language** | Preferred language | Installed assets | Active language |

---

## 8. Ownership and Authority

```mermaid
flowchart TB
    subgraph Server["Server Owns / Validates"]
        S1["Account preferences"]
        S2["Social relationships"]
        S3["Privacy enforcement"]
        S4["Blocks"]
        S5["Requests"]
        S6["Preferred language record"]
        S7["Redeem validation"]
        S8["Account permissions"]
        S9["Anything security-sensitive"]
    end

    subgraph Client["Client Owns"]
        C1["Resolution"]
        C2["Refresh rate"]
        C3["VSync"]
        C4["GPU settings"]
        C5["Output device"]
        C6["Window mode"]
        C7["Physical mappings"]
    end

    subgraph Both["Both Participate"]
        B1["Keybinds"]
        B2["HUD"]
        B3["Audio volumes + device"]
        B4["Accessibility"]
        B5["Graphics preference"]
    end

    style Server fill:#2d6a4f,color:#fff
    style Client fill:#457b9d,color:#fff
    style Both fill:#9b5de5,color:#fff
```

---

## 9. API Shape

> Do not expose one giant Settings endpoint just because the UI is one screen.

### Account Preferences

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/account/preferences` | All preferences |
| PUT | `/account/preferences` | Update all |
| GET | `/account/preferences/gameplay` | Gameplay prefs |
| PUT | `/account/preferences/gameplay` | Update gameplay |
| GET | `/account/preferences/accessibility` | Accessibility prefs |
| PUT | `/account/preferences/accessibility` | Update accessibility |
| GET | `/account/preferences/social` | Social prefs |
| PUT | `/account/preferences/social` | Update social |
| GET | `/account/preferences/notifications` | Notification prefs |
| PUT | `/account/preferences/notifications` | Update notifications |

### Social State

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/account/social/friends` | List friends |
| POST | `/account/social/friends/requests` | Send request |
| DELETE | `/account/social/friends/{playerId}` | Remove friend |
| GET | `/account/social/blocks` | List blocks |
| POST | `/account/social/blocks` | Block player |
| DELETE | `/account/social/blocks/{playerId}` | Unblock player |

### Other

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/account/redeem` | Redeem code |
| * | Auth endpoints | QR / session approval |

> Device settings stay client-local.

---

## 10. Persistence

### Recommended for Pick Me Up

```mermaid
flowchart TB
    subgraph Options["Storage Options"]
        A["Option A: One large table<br>Simple initially, hard to version"]
        B["Option B: Separate tables<br>Clear ownership, more migrations"]
        C["Option C: JSON document<br>Flexible, weaker relational integrity"]
    end

    Recommend["Recommendation:<br>Start with versioned account-preferences<br>document + separate social tables.<br>Do not put device JSON on the account."] --> B

    style Recommend fill:#2d6a4f,color:#fff
```

### Defaults

> Prefer storing overrides only. If the player has never changed a setting,
> the database has no entry and the runtime uses the default.
> Explicitly storing every default is acceptable early if it reduces confusion.

### Versioning

```mermaid
flowchart LR
    SV["Stored Version"] --> Migrate["Migration"] --> CV["Current Version"] --> RT["Runtime"]

    style Migrate fill:#f4a261,color:#000
```

> Add `SettingsVersion` and migrate on read.

---

## 11. Client Storage

| File | Contents |
|---|---|
| `graphics.json` | Resolution, VSync, quality, FPS |
| `input.json` | Keybindings, controller mappings |
| `device.json` | Hardware info, display properties |
| `platform` | Platform preference store |

> Exact storage mechanism decided by the client/engine layer.

---

## 12. Flows

### Startup

```mermaid
flowchart TD
    S1["1. Load local device config"] --> S2["2. Detect hardware capabilities"]
    S2 --> S3["3. Authenticate player"]
    S3 --> S4["4. Load account preferences"]
    S4 --> S5["5. Load social/account state"]
    S5 --> S6["6. Validate received data"]
    S6 --> S7["7. Apply defaults"]
    S7 --> S8["8. Resolve hybrid settings"]
    S8 --> S9["9. Build effective runtime config"]
    S9 --> S10["10. Start game systems"]

    style S9 fill:#f4a261,color:#000
```

### Update (e.g. Music Volume)

```mermaid
flowchart TD
    U1["Player changes<br>MusicVolume = 40%"] --> U2["UI"]
    U2 --> U3["Client settings manager"]
    U3 --> U4["Validate locally"]
    U4 --> U5["Update local runtime"]
    U5 --> U6["Send to API"]
    U6 --> U7["API validates"]
    U7 --> U8["Database"]
    U8 --> U9["Response"]
    U9 --> U10["Local state confirmed"]

    style U5 fill:#2d6a4f,color:#fff
    style U7 fill:#e76f51,color:#fff
```

> Backend remains authoritative for persisted account state.

---

## 13. Validation

> Every value entering the backend is untrusted.

| Setting | Valid Range | Reject |
|---|---|---|
| `MusicVolume` | 0.0 — 1.0 | Negatives, huge numbers, NaN |
| `InterfaceScale` | 0.5 — 2.0 | Out of documented range |
| `Language` | Known enum values | Unknown keys |
| `FriendRequests` | `Everyone`, `FriendsOnly`, `Nobody` | Unknown enums |

> Unknown keys on an old client should not crash a new schema.

---

## 14. Settings UI as Orchestrator

The screen can still look like one Settings tree:

```
Settings
├── Gameplay
├── Accessibility
├── Controls
├── Audio
├── Graphics
├── Display
├── Social
├── Notifications
├── Language
├── Account
└── Support
```

But internally the UI calls separate services:

```mermaid
flowchart TD
    UI3["Settings UI"] --> APS["AccountPreferencesService"]
    UI3 --> SS["SocialService"]
    UI3 --> DSS["DeviceSettingsService"]
    UI3 --> IS["InputService"]
    UI3 --> AS2["AudioService"]
    UI3 --> GS["GraphicsService"]
    UI3 --> AuthS["AuthenticationService"]
    UI3 --> SupS["SupportService"]
    UI3 --> RS["RedemptionService"]
    UI3 --> CS["ContentService"]

    style UI3 fill:#264653,color:#fff
```

> The UI is a presentation layer, not a domain owner.

---

## 15. Implementation Order

> Do not start with 100 classes. Draw the boundaries first.
> Implement only what the current game needs.

```mermaid
flowchart LR
    P1["Phase 1<br>Conceptual ownership"] --> P2["Phase 2<br>DTOs / Contracts"]
    P2 --> P3["Phase 3<br>Persistence"]
    P3 --> P4["Phase 4<br>API operations"]
    P4 --> P5["Phase 5<br>Client settings manager"]
    P5 --> P6["Phase 6<br>Hybrid resolution"]
    P6 --> P7["Phase 7<br>Versioning & migrations"]

    style P1 fill:#2d6a4f,color:#fff
    style P4 fill:#9b5de5,color:#fff
    style P7 fill:#f4a261,color:#000
```

---

## 16. Final Classification

| Domain | Contains |
|---|---|
| **Account** | Gameplay, Accessibility, Social Preferences, Notifications, Language, UI Preferences, Audio Preferences |
| **Social** | Friends, Friend Requests, Blocks, Relationships |
| **Device** | Graphics, Display, Audio Output, Performance, Physical Input, Network Diagnostics |
| **Hybrid** | Keybindings, HUD, Audio, Graphics Preference, Interface Scaling, Accessibility |
| **Runtime** | Effective Graphics, Effective Audio, Effective Input, Effective UI |
| **Other** | Authentication, Redemption, Support, Announcements, Events, Account Security |

---

## 17. Advantages and Costs

### Why Bother

- Clear ownership — hardware does not pollute the account
- Preferences follow the player across devices
- Security stays outside cosmetic flags
- Cross-platform input and UI can coexist
- APIs stay domain-sized instead of one immortal Settings class

### What It Costs

- More boundaries, sync/conflict policy, schema versioning
- Hybrid resolution logic
- Larger test matrix (account only, device only, both, missing data, invalid data, old data)

### Philosophy

> Design the boundaries now.
> Implement only what is currently required.
> That avoids both a blob class and speculative over-engineering.

---

## 18. Final Rule

Never ask whether it appears in the Settings menu.

Ask:

- Who owns this data?
- Where does it need to persist?
- Should it synchronise?
- Is it hardware-specific?
- Is it server-authoritative?
- Does it describe a relationship?
- Is it a preference or an action?
- Is it a raw value or an effective runtime value?

If those answers exist first, the C# structure is easy.

---

## 19. Implementation Status

> Last updated: September 2026

### Resolved — Backend (PickMeUp.Api)

| Domain | Status | Files | Notes |
|---|---|---|---|
| **Gameplay** | ✅ Complete | Settings, DTO, Controller, Validator | 20 settings, FluentValidation |
| **Accessibility** | ✅ Complete | Settings, DTO, Controller, Validator | 14 settings, FluentValidation |
| **Language** | ✅ Complete | Settings, DTO, Controller, Validator | 12 supported codes |
| **Notifications** | ✅ Complete | Settings, DTO, Controller, Validator | 5 boolean toggles |
| **Social Preferences** | ✅ Complete | Settings, DTO, Controller, Validator | 4 visibility rules (enum) |
| **Audio** | ✅ Complete | Settings, DTO, Controller, Validator | 9 settings (volumes + mutes) |
| **UI Preferences** | ✅ Complete | Settings, DTO, Controller, Validator | 13 settings (layout, positions) |
| **Social (relationships)** | ✅ Complete | Repository, Service, Controller, Middleware | Friends, blocks, party invites (MongoDB) |
| **Authentication** | ✅ Complete | OAuth + Session + JWT | Google, Facebook, token rotation, Redis |
| **Persistence** | ✅ Complete | IAccountPreferencesRepository + MongoDB | Per-domain get/update, versioned |

### Resolved — Infrastructure

| Component | Status | Notes |
|---|---|---|
| **Program.cs** | ✅ Wired | All services registered, middleware pipeline |
| **.csproj** | ✅ Complete | 11 packages (MongoDB, FluentValidation, Redis, etc.) |
| **.env loading** | ✅ Working | DotNetEnv, all secrets mapped to config |
| **JWT** | ✅ Working | Access + refresh tokens, Redis-backed sessions |
| **OAuth** | ✅ Working | Google + Facebook, CSRF state, callback flow |

### Still Missing / Needs Work

| Domain | Status | What's needed |
|---|---|---|
| **Account Settings** | ⬜ Empty folder | `AccountSettings/` — reserved, no files yet |
| **Profile (Avatar)** | ⚠️ Placeholder | Value object exists, no controller or persistence |
| **Profile (Username)** | ⚠️ Placeholder | Value object exists, no controller or persistence |
| **Social Preferences** | ⚠️ Partial | Controller exists, but no block enforcement on preference updates |
| **Device Configuration** | ⬜ Not started | Resolution, refresh rate, VSync, GPU — client-side only |
| **Input Configuration** | ⬜ Not started | Logical actions ↔ physical mappings — client-side only |
| **Hybrid Resolution** | ⬜ Not started | Account preference + device capability → effective runtime |
| **Effective Runtime Config** | ⬜ Not started | Calculated object, not a database entity |
| **Settings Versioning** | ⚠️ Partial | Version field exists, no migration logic |
| **Conflict Resolution** | ⬜ Not started | Last-write-wins vs version checks |
| **Client Settings Manager** | ⬜ Not started | Client-side settings coordinator |
| **Tests** | ⬜ Not started | No unit or integration tests yet |
| **MongoDB Index Setup** | ⬜ Not started | Index creation on startup |
| **Health Checks** | ⬜ Not started | `/health`, `/ready` endpoints |
| **CORS** | ⬜ Not started | Required for client communication |
| **Rate Limiting** | ⬜ Not started | User-based request throttling |

### Folder Ownership Summary

| Folder | Owner | Purpose |
|---|---|---|
| `Account/Authentication/` | Shared backend | OAuth, JWT, sessions |
| `Account/AccountPreferences/` | Shared backend | Per-user preference storage |
| `Account/Profile/` | Shared backend | Value objects (Avatar, Username) |
| `Social/` | Shared backend | Friend/block/party relationships |
| `DoNotTouchFolder/` | Reserved | Identity entities, UserCenter placeholders |
| `v1/` | Unity client | Engine-specific implementation |
| `v2/` | Unreal client | Engine-specific implementation |
