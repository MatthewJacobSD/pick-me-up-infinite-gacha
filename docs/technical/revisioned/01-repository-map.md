# 01 — Repository map

## Top level

```
pick-me-up-infinite-gacha/
  AGENTS.md
  CONTRIBUTING.md
  PickMeUp.Api/          shared ASP.NET backend (.slnx lives here today)
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
| `docs/systems/` | Economy, heroes, tower | Game rules; not implemented in API yet |
| `docs/story/` | Chapters, timeline, tracker | Fiction |
| `docs/characters/`, `docs/psychology/` | Character work | Fiction |
| `docs/tasks/` | Backend / frontend / design task lists | Planning |
| `docs/game-overview.md`, `roadmap.md`, `decisions.md` | Product planning | Planning |

Do not put implementation status only in `settings-architecture.md`. Status for engineering lives in revisioned docs and git.

## `PickMeUp.Api/` today

```
PickMeUp.Api/
  Program.cs
  appsettings.json
  appsettings.Development.json
  PickMeUp.Api.csproj
  PickMeUp.Api.slnx
  settings-architecture.md          # duplicate; retire
  Account/
    Authentication/                 # OAuth + JWT + session
    AccountPreferences/             # seven slices + incomplete repository
    Profile/                        # Avatar, Username value objects
    AccountSettings/                # empty folder in csproj
  Social/                           # graph + partial HTTP
  DoNotTouchFolder/                 # Identity / UserCenter placeholders
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
