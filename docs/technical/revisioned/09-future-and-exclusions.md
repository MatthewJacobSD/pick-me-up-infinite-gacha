# 09 — Future work and exclusions

## Explicitly after Account + Social + hosting

| Topic | Owner | Store |
|---|---|---|
| Device configuration | Client | Local |
| Hybrid resolver | Client | Calculated |
| Effective runtime config | Client | Memory |
| Profile avatar / username HTTP | Account/Profile | MySQL username + optional Mongo cosmetics |
| Redeem | Own domain | MySQL catalog + Mongo redemptions |
| Support / tickets | Own domain | later |
| Combat, gacha, sanctuary, tower | Gameplay services | Mongo instances + MySQL templates |
| SignalR heartbeat / matchmaking | Hosting + game | Redis queues |
| DeviceSettings folder in the API | **Do not create** | — |

## Older docs to treat as historical

- `docs/technical/folder-structure.md` Niflheim Controllers/Services/Models — do not reorganize toward that.
- `docs/technical/architecture.md` “MySQL = account data” — superseded by [02](./02-data-stores.md).
- Duplicate `PickMeUp.Api/settings-architecture.md` — pointer or delete.

Story, economy, heroes, tower docs remain valid as **game design**, not as folder layout.
