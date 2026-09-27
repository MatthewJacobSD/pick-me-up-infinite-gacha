# Current Implementation Status

> Last updated: September 2026
> This file records the current state of the codebase implementation, separate from story docs and technical design docs.

---

## Backend (PickMeUp.Api) — Implementation Status

### ✅ Completed

| Module | What's done |
|---|---|
| **Account Preferences** | 7 domains (Gameplay, Accessibility, Language, Notifications, Social Preferences, Audio, UI) — all with GET/PUT/PATCH/reset/defaults, FluentValidation, MongoDB persistence with versioned writes |
| **Social** | Friends, blocks, party invites — full lifecycle with policy, enforcement, exception handling |
| **Authentication** | Google (v26.0) + Facebook (v26.0), OAuth callback, JWT tokens, multi-provider linking, MOE- public codes, email+password registration, account recovery (30-day soft delete) |
| **Profile** | Username + Avatar persistence via MongoDB |
| **Infrastructure** | Program.cs (thin composition root), .env loading, MySQL Identity, MongoDB, Redis, health checks, CORS, rate limiting |

### ⚠️ Partial / Needs Polish

| What | Status |
|---|---|
| Profile tests | Mock-based tests exist but need integration tests with real MongoDB |
| Integration tests | None yet — full HTTP pipeline tests |
| MongoDB indexes | Index definitions + hosted service exist, need verification |
| Settings versioning | SettingsVersion field added, migration logic is placeholder |

### Tests

| Count | Status |
|---|---|
| 204 | Passing |

---

## Client (interfaces/) — Reference Viewer Only

**This is NOT connected to the backend.** It's a Vite + React gothic onboarding prototype used for UI/UX design inspiration only.

| Screen | Status |
|---|---|
| Device Select | ✅ |
| Initial / Welcome | ✅ |
| Login Select (Google/Facebook/Email) | ✅ (updated with real API calls, not connected) |
| Authenticating | ✅ (mock flow) |
| Name Input/Confirmation | ✅ |
| Tutorial screens | ✅ |
| Story panels (5) | ✅ |
| Main Menu | ✅ |
| Settings screens | ⬜ Not built (reference only) |
| Social screens | ⬜ Not built (reference only) |
| Profile screens | ⬜ Not built (reference only) |

**Purpose:** Visual reference for how to design the Unity/Unreal client UI. Not a production app.

---

## Unity (v1/) — Status

⬜ Empty. Only `README.md` and task/technical-reference stubs.

**Blocked by:** Story documentation (game mechanics, screen flow, assets not finalized)

## Unreal (v2/) — Status

⬜ Empty. Only `README.md` and task/technical-reference stubs.

**Blocked by:** Story documentation (same as Unity)

---

## Story Documentation — Status

| Chapter | Status |
|---|---|
| Chapters 1–28 | ✅ Transcribed |
| Chapter 29+ | ⬜ Not yet provided |

**Systems documented:** Economy, Heroes, Tower
**Systems NOT documented yet:** Combat details, Gacha mechanics, Crafting, Base Building, Equipment, Matchmaking

---

## What's Blocking Client Development

1. **Story docs incomplete** — Game mechanics, screen flow, assets not finalized
2. **No character/system docs** — Heroes, gacha, tower, combat need stable definitions
3. **No screen flow definition** — Title → Login → Menu → Battle not locked
4. **No asset pipeline** — Art/audio delivery not decided

**Client folders (`v1/`, `v2/`) stay documentation-only until story docs are complete.**
