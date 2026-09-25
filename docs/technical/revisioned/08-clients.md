# 08 — Clients (`v1`, `v2`, `interfaces`)

## Rule

Engines implement **device + runtime + presentation**.  
They do not own friends or account preference truth.

## `v1/` Unity

Today: `README.md`, task and technical-reference stubs.

When backend definition of done is met:

- Auth client (open browser / in-app OAuth, store refresh securely)
- Preferences client (GET all at login, PUT slices on change, honor 409)
- Social client
- Device settings manager (`graphics.json`, `input.json`, `device.json` or `PlayerPrefs` / Addressables equivalent)
- Hybrid resolver (account intent + device caps)

Do not duplicate Mongo access from Unity.

## `v2/` Unreal

Same contract, different storage (`GameUserSettings`, SaveGame, ini). Same HTTP.

## `interfaces/`

Vite + React gothic onboarding prototype. Not the production settings UI. May later point at `/api/auth` for flow testing. Not a third engine.

## Shared contracts (future)

If C# sharing with Unity is required, extract DTOs into `PickMeUp.Contracts` consumed by API and Unity. Unreal will still speak JSON. Do not extract that package before the HTTP shapes stabilize in this backend pass.

## Local files (spec)

| File | Contents |
|---|---|
| `graphics.json` | Resolution, VSync, quality applied, FPS |
| `input.json` | Physical mappings |
| `device.json` | Caps snapshot |
| platform store | OS-level prefs |

Never uploaded to Account.
