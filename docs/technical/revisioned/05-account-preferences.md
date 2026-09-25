# 05 — Account preferences

## Definition

Account preferences are **how this player wants the game to behave**. They persist on the backend, sync across devices, and are validated on write.

They are not:

- friends or blocks (Social)
- resolution or VSync (Device / client)
- the effective quality the GPU actually runs (Runtime, client)
- redeem, support, announcements

## Document (Mongo)

Collection: `account_preferences`
One document per `AccountId` (Guid, `[BsonId]`).

```
AccountId              # Guid, primary key (BsonId)
UserId                 # string, secondary identifier
Version                # optimistic concurrency, incremented on every write
Gameplay               # GameplaySettings record
Accessibility          # AccessibilitySettings record
Language               # LanguageSettings record
Notifications          # NotificationSettings record
SocialPreferences      # SocialSettings record (visibility rules only)
Audio                  # AudioSettings record
UiPreferences          # UiSettings record
```

Storing full defaults on first create is allowed early. Later, storing overrides only is optional.

## Slices

| Slice | Intent | Settings Type | Fields |
|---|---|---|---|
| Gameplay | Bars, combat text, camera assists, nameplates | `GameplaySettings` | 20 fields (action bars, combat UI, camera, interaction, nameplates, movement) |
| Accessibility | Colour, subtitles, motion, text size | `AccessibilitySettings` | 14 fields (colorblind, subtitles, audio assist, general) |
| Language | Preferred code | `LanguageSettings` | 1 field (PreferredLanguage) |
| Notifications | Want event types | `NotificationSettings` | 5 boolean toggles |
| Social preferences | Who may request / message / invite / see online | `SocialSettings` | 4 visibility enums |
| Audio | Volume + mute *intent* | `AudioSettings` | 9 fields (master/music/sfx/voice/ambient volume + mute) |
| UI | Scale and HUD layout *intent* | `UiSettings` | 13 fields (scale, text/icon size, HUD visibility, positions, layouts) |

All settings types are C# records (support `with` expressions for patching).

Field-of-view and camera invert are account *intent* here. Caps still belong on the client at apply time.

## HTTP

### GET all preferences

| Method | Path | Response |
|---|---|---|
| GET | `/account/preferences` | Full document with all slices + version |

### Per-slice endpoints

Each slice supports:

| Method | Path | Purpose |
|---|---|---|
| GET | `/account/preferences/{slice}` | Get current settings + version |
| PUT | `/account/preferences/{slice}` | Replace entire slice (requires `Version` in body) |
| PATCH | `/account/preferences/{slice}` | Partial update (nullable fields, requires `Version`) |
| POST | `/account/preferences/{slice}/reset` | Reset to defaults (requires `Version`) |
| GET | `/account/preferences/{slice}/defaults` | Get default values (no auth required) |

**Slices:** `gameplay`, `accessibility`, `language`, `notifications`, `social`, `audio`, `ui`

PUT/PATCH/RESET require current `Version` in the request body. Success returns the updated slice + new version. Stale version → 409 ProblemDetails.

### Response format

```json
{
  "settings": { ... },
  "version": 42
}
```

## Validation

FluentValidation runs in the pipeline. Examples:

- Audio volumes `0.0–1.0`, reject NaN
- `UiScale` `0.5–2.0`
- `Language` ∈ `{en, nl, fr, de, it, es, pt, pl, ru, ja, ko, zh}`
- `SocialVisibility` ∈ `Everyone | FriendsOnly | Nobody`
- `ColorBlindMode` ∈ `None | Protanopia | Deuteranopia | Tritanopia`
- `SubtitleSize` / `TextSize` ∈ `10–40`
- `VisibleActionBars` ∈ `1–6`
- `CameraSensitivity` ∈ `0.1–10`
- `FieldOfView` ∈ `60–120`

Unknown future JSON fields must not crash old documents. Typed BSON mapping needs an explicit ignore-extra policy.

## Repository

`IAccountPreferencesRepository` is the only persistence port. It provides:

- `GetOrCreateAsync(Guid)` — atomic upsert with defaults
- `FindAsync(Guid)` — nullable lookup
- Per-slice `Get*SettingsAsync(Guid)` — read a single slice
- Per-slice `Replace*SettingsAsync(Guid, settings, version)` — full replace with version check
- Per-slice `Patch*SettingsAsync(Guid, patchDto, version)` — partial update with version check

Implementation uses `FindOneAndUpdateAsync` with `Version` filter for optimistic concurrency. A shared `ReplaceSliceAsync<T>` helper avoids per-slice duplication.

## Server must never

Treat a client presentation flag as permission to loot, deal damage, or skip social checks.
