# 01 — Repository map

## Top level

```
pick-me-up-infinite-gacha/
  AGENTS.md
  CONTRIBUTING.md
  PickMeUp.Api/          shared ASP.NET backend (.slnx lives here today)
  PickMeUp.Api.Tests/    xUnit test project
  agents/                agent-facing notes
  docs/                  design, story, systems, technical
  interfaces/            Vite + React prototype (onboarding flow)
  v1/                    Unity
  v2/                    Unreal
```

## `docs/`

| Path | Content | Engineering weight |
|---|---|---|
| `docs/technical/` | Original technical set | Historical + still cited |
| `docs/technical/revisioned/` | This design | **Source of truth for backend/store/API** |
| `docs/technical/api/` | OpenAPI spec + engine contract | Source of truth for HTTP shapes |
| `docs/systems/` | Economy, heroes, tower | Game rules; not implemented in API yet |
| `docs/story/` | Chapters, timeline, tracker | Fiction |
| `docs/characters/`, `docs/psychology/` | Character work | Fiction |
| `docs/tasks/` | Backend / frontend / design task lists | Planning |
| `docs/game-overview.md`, `roadmap.md`, `decisions.md` | Product planning | Planning |

Do not put implementation status only in `settings-architecture.md`. Status for engineering lives in revisioned docs and git.

## `PickMeUp.Api/` current structure

```
PickMeUp.Api/
  Program.cs                            # thin composition root
  appsettings.json
  appsettings.Development.json
  PickMeUp.Api.csproj
  PickMeUp.Api.slnx
  Hosting/
    EnvLoader.cs                        # .env loading with priority precedence
    ConfigurationExtensions.cs          # typed options (Jwt, Mongo, MySql, Redis, OAuth)
    DependencyInjection.cs              # AddPickMeUpApi composition root
  Common/
    Authentication/
      ICurrentUser.cs                   # account identity abstraction
      CurrentUser.cs                    # reads JWT sub/accountId claims
    Errors/
      DomainException.cs                # base domain exception
      ValidationException.cs            # validation errors
      VersionConflictException.cs       # 409 stale version
      NotFoundException.cs              # 404 not found
      ProblemDetailsExtensions.cs       # exception → ProblemDetails middleware
  Account/
    Authentication/
      Email.cs                          # email value object
      Password.cs                       # password value object
      OAuth/                            # OAuth flow (Google, Facebook)
      Session/                          # JWT + refresh token management
    AccountPreferences/
      IAccountRepository.cs             # preferences repository interface
      AccountRepository.cs              # MongoDB implementation (upsert, versioned writes, PATCH)
      AccountDocument.cs                # MongoDB document (all 7 slices + version)
      DependencyInjection.cs            # empty stub (filled by hosting DI)
      VersionDto.cs                     # shared version DTO for reset endpoints
      VersionValidatorDto.cs            # validator for VersionDto
      Gameplay/                         # 20 settings (action bars, combat, camera, etc.)
        GameplaySettings.cs             # settings record
        GameplayDto.cs                  # PUT DTO + GameplayPatchDto
        GameplayController.cs           # GET, PUT, PATCH, reset, defaults
        GameplayValidator.cs            # FluentValidation
      Accessibility/                    # 14 settings (colors, subtitles, audio assist, etc.)
        AccessibilitySettings.cs        # settings record
        AccessibilityDto.cs             # PUT DTO + AccessibilityPatchDto
        AccessibilityController.cs      # GET, PUT, PATCH, reset, defaults
        AccessibilityValidator.cs       # FluentValidation
      Language/                         # preferred language (ISO code)
        LanguageSettings.cs             # settings record
        LanaguageDto.cs                 # PUT DTO (note: filename has typo)
        LanguagePatchDto.cs             # PATCH DTO
        LanguageController.cs           # GET, PUT, PATCH, reset, defaults
        LanguageValidator.cs            # FluentValidation
      Notifications/                    # 5 boolean toggles
        NotificationSettings.cs         # settings record
        NotificationDto.cs              # PUT DTO
        NotificationPatchDto.cs         # PATCH DTO
        NotificationController.cs       # GET, PUT, PATCH, reset, defaults
        NotificationValidator.cs        # FluentValidation
      SocialPreferences/                # visibility rules (Everyone/FriendsOnly/Nobody)
        SocialSettings.cs               # settings record
        SocialDto.cs                    # PUT DTO
        SocialPatchDto.cs               # PATCH DTO
        SocialController.cs             # GET, PUT, PATCH, reset, defaults
        SocialValidator.cs              # FluentValidation
      Audio/                            # 9 settings (volumes + mutes)
        AudioSettings.cs                # settings record
        AudioDto.cs                     # PUT DTO + AudioPatchDto
        AudioController.cs              # GET, PUT, PATCH, reset, defaults
        AudioValidator.cs               # FluentValidation
      UiPreferences/                    # 13 settings (layout, positions, scales)
        UiSettings.cs                   # settings record
        UiPreferencesDto.cs             # PUT DTO
        UiPatchDto.cs                   # PATCH DTO
        UiController.cs                 # GET, PUT, PATCH, reset, defaults
        UiValidator.cs                  # FluentValidation
    Profile/
      Avatar.cs                         # avatar value object (Default/Static/Custom)
      Username.cs                       # username value object (3-20 chars)
  Social/                               # relationships and invites
    ISocialRepository.cs
    SocialRepository.cs
    ISocialService.cs
    SocialService.cs
    SocialController.cs
    SocialPolicy.cs
    SocialDocument.cs
    FriendRequestDocument.cs
    PartyInviteDocument.cs
    BlockEnforcementMiddleware.cs
    FriendCommand.cs / BlockCommand.cs / PartyCommand.cs
    SocialExceptions.cs / SocialExceptionHandler.cs
    SocialIndexDefinitions.cs / SocialIndexHostedService.cs
    ISocialVisibilityQuery.cs / PreferencesSocialVisibilityQuery.cs
    DependencyInjection.cs
  DoNotTouchFolder/                     # Identity / UserCenter placeholders
    ApplicationDbContext.cs
    ApplicationUser.cs
    SharedSettings/UserCenter/          # Agreement, BindAccount, OTP, TestCenter
```

## What each folder is allowed to own

| Folder | May own | Must not own |
|---|---|---|
| `Account/Authentication` | Login, tokens, sessions, external identities | Volumes, friends, resolution |
| `Account/AccountPreferences` | Synced player intent | Friend lists, GPU, window mode |
| `Account/Profile` | Display identity (later) | Social graph |
| `Social/` | Relationships and invites | Preference documents |
| `DoNotTouchFolder/` | Legacy Identity scaffolding until moved | New features |
| `v1/`, `v2/` | Engine, local device config, rendering | Authoritative social or economy |
| `interfaces/` | Prototype screens | Production persistence |

## Versioning scheme (from CONTRIBUTING)

| Version | Meaning |
|---|---|
| 0.0.x | Documentation / planning |
| 1.0.0 | Unity implementation begins |
| 2.0.0 | Unreal implementation begins |

Backend work now is still documentation-plus-API scaffold. Do not bump to 1.0.0 because folders exist.
