# Technical design — revisioned

**Status:** design revision, September 2026  
**Scope:** entire repository, with implementation priority on `PickMeUp.Api`  
**Supersedes for engineering decisions:** older lines in `docs/technical/architecture.md` and `docs/technical/folder-structure.md` where they conflict  
**Does not replace:** story, systems, and character docs under `docs/`

This folder is the revised technical design. It records what the repo *is*, what the architecture *requires*, and what is *out of scope* until Account and Social actually run.

| Doc | Subject |
|---|---|
| [00-overview.md](./00-overview.md) | Product shape, engines, backend-first rule |
| [01-repository-map.md](./01-repository-map.md) | Every top-level folder and who owns it |
| [02-data-stores.md](./02-data-stores.md) | MySQL vs MongoDB vs Redis vs client-local |
| [03-hosting-and-configuration.md](./03-hosting-and-configuration.md) | `Program.cs`, env loader, composition root |
| [04-identity-and-authentication.md](./04-identity-and-authentication.md) | Account identity, JWT, OAuth |
| [05-account-preferences.md](./05-account-preferences.md) | Synced settings (not device, not social graph) |
| [06-social.md](./06-social.md) | Friends, blocks, requests, party, policy |
| [07-api-surface.md](./07-api-surface.md) | HTTP contracts Unity will call |
| [08-clients.md](./08-clients.md) | `v1/` Unity, `v2/` Unreal, `interfaces/` |
| [09-future-and-exclusions.md](./09-future-and-exclusions.md) | Device, hybrid runtime, combat, gacha |
| [10-implementation-plan.md](./10-implementation-plan.md) | A–Z backend milestone |
| [11-target-folder-structure.md](./11-target-folder-structure.md) | How `PickMeUp.Api` should be laid out |

## How to use this folder

- New backend work is checked against **02, 05, 06, 07**.
- New files are placed using **11**.
- “Are we allowed to start Unity?” is answered by **10** definition of done.
- DeviceSettings and engine settings managers are **09** and **08**, not Account.

## Conflict policy

If an older technical doc disagrees with this folder:

1. This folder wins for store ownership, settings vs social, and hosting.
2. Story and game-system docs still win for fiction and gameplay rules.
3. Update the older file or add a one-line pointer here. Do not leave two opposing “account = MySQL” sentences live.
