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
One document per `UserId`.

```
UserId
Version              # optimistic concurrency, not schema migration
SettingsVersion      # schema version; migrate on read when needed
Gameplay
Accessibility
Language
Notifications
SocialPreferences    # visibility rules only
Audio
UiPreferences
```

Storing full defaults on first create is allowed early. Later, storing overrides only is optional.

## Slices

| Slice | Intent | Hybrid later? |
|---|---|---|
| Gameplay | Bars, combat text, camera assists, nameplates | Input/camera vs device |
| Accessibility | Colour, subtitles, motion, text size | DPI / fonts |
| Language | Preferred code | Installed packs on client |
| Notifications | Want event types | Delivery system separate |
| Social preferences | Who may request / message / invite / see online | Enforced by Social policy |
| Audio | Volume + mute *intent* | Output device is local |
| UI | Scale and HUD layout *intent* | Safe area / resolution local |

Field-of-view and camera invert are account *intent* here. Caps still belong on the client at apply time.

## HTTP

| Method | Path |
|---|---|
| GET | `/account/preferences` |
| PUT | `/account/preferences/{slice}` as today |
| GET | `/account/preferences/{slice}` |

PUT requires current `Version` (body or `If-Match`). Success returns the slice + new version. Stale version → 409.

No single immortal Settings controller that also lists friends.

## Validation

FluentValidation must run in the pipeline. Examples:

- volumes `0.0–1.0`, reject NaN
- `UiScale` `0.5–2.0`
- language ∈ catalog
- visibility ∈ `Everyone | FriendsOnly | Nobody`

Unknown future JSON fields must not crash old documents. Typed BSON mapping needs an explicit ignore-extra policy.

## Repository

`IAccountPreferencesRepository` is the only persistence port. The implementation must match the interface (Social + UI methods are missing today). Updates filter `UserId + Version`. Unique index on `UserId`.

## Server must never

Treat a client presentation flag as permission to loot, deal damage, or skip social checks.
