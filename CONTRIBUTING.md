# Contributing Guide

## Versioning Scheme

| Version | Meaning |
|---|---|
| `0.0.x` | Documentation / story chapters / planning |
| `0.1.x` | Unity (`v1/`) development — alpha |
| `0.2.x` | Unreal (`v2/`) development — alpha |
| `1.0.0` | Unity implementation milestone (stable) |
| `2.0.0` | Unreal implementation milestone (stable) |

Backend code (`PickMeUp.Api/`) uses conventional commits without version numbers:
`feat:`, `fix:`, `refactor:`, `docs:`, `chore:`.

## Branching Strategy

| Branch | Purpose |
|---|---|
| `main` | Shared backend + documentation |
| `dev/unity` | Unity client development |
| `dev/unity-test` | Unity testing / experiments |
| `dev/unreal` | Unreal client development |
| `dev/unreal-test` | Unreal testing / experiments |

Unity and Unreal branches are **independent** — no cross-merging.

## Folder Convention

| Folder | Content |
|---|---|
| `docs/` | Engine-agnostic documentation (story, systems, technical, tasks) |
| `PickMeUp.Api/` | Shared ASP.NET backend (engine-agnostic) |
| `v1/` | Unity client specifics |
| `v2/` | Unreal client specifics |
| `interfaces/` | Web UI prototype (isolated experiment) |

## Documentation Structure

| Path | Purpose |
|---|---|
| `docs/technical/revisioned/` | **Authoritative** technical design |
| `docs/technical/historical/` | Superseded docs (reference only) |
| `docs/technical/api/` | API contracts (OpenAPI + engine contract) |
| `docs/systems/` | Game design rules (economy, heroes, tower) |
| `docs/story/` | Story chapters (versioned v0.0.XX) |
| `docs/characters/` | Character profiles |
| `docs/psychology/` | Character behavior rules |

If a revisioned doc contradicts a historical doc, the **revisioned set wins**.
