# 00 — Overview

## What this repository is

Pick Me Up Infinite Gacha is a manhwa-inspired game with **two engine clients** and **one shared backend**.

| Layer | Location | Role |
|---|---|---|
| Shared backend | `PickMeUp.Api/` | Identity, account preferences, social graph, future gameplay services |
| Unity client | `v1/` | Engine implementation (not started beyond docs) |
| Unreal client | `v2/` | Engine implementation (not started beyond docs) |
| Interface prototype | `interfaces/` | Web onboarding / presentation prototype |
| Design docs | `docs/` | Story, systems, technical, tasks |
| Agent notes | `agents/`, `AGENTS.md` | Contributor / agent conventions |

There is no playable client hooked to the API. That is intentional until the backend can identify a player and persist their data.

## Architectural principles (locked)

1. **Domain ownership over UI tabs.** Something in a Settings screen is not automatically a setting.
2. **One player id.** MySQL Identity `Guid` is the `sub` / `accountId` on JWTs and the key on every Mongo document.
3. **Definition data vs instance data.** MySQL holds identity and catalogs. MongoDB holds changing player state. Redis holds sessions. The client holds device configuration.
4. **Server-authoritative social rules.** Visibility preferences and blocks are enforced on the server, not trusted from the client.
5. **Preferences follow the player. Hardware stays on the machine.**
6. **Vertical slices in the API**, not a return to Controllers / Services / Models piles.
7. **No TypeScript-style barrel files.** C# modules expose DI extension methods and namespaces.
8. **Backend milestone before engine wiring.** Unity and Unreal consume a finished Account + Social contract.

## Current honesty

The settings architecture status table that marks Account slices “complete” is **not accurate**. On disk there are DTOs, controllers, and validators. They are not a running module: Mongo is unregistered, validators are unwired, the preferences repository does not implement its interface, and controllers read `User.Identity.Name` while tokens emit `sub`.

This revisioned set describes the **target design** and the **gap**. Implementation follows [10-implementation-plan.md](./10-implementation-plan.md).

## Engines

`v1/` and `v2/` remain engine folders only. Shared contracts that both engines need later may become a small contracts package. That package is not created in the current backend pass.
