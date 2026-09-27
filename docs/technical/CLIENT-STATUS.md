# Client Functionality — Status & Blockers

> Last updated: September 2026
> **Client development is on hold until story documentation is finished.**

---

## What exists today

| Layer | Status | Notes |
|---|---|---|
| Backend API | ✅ Built | Auth (Google + Facebook), Preferences (7 slices), Social, Profile |
| Unity (`v1/`) | ⬜ Empty | Only README and docs |
| Unreal (`v2/`) | ⬜ Empty | Only README and docs |
| Interface prototype | ⬜ Web only | Vite + React onboarding flow |

---

## Verified Backend Features

### OAuth (Google + Facebook) — ✅ Working in local development

| Provider | Status | Redirect URI | Notes |
|---|---|---|---|
| Google | ✅ Working | `http://localhost:5137/api/auth/callback/google` | HTTP accepted by Google for localhost |
| Facebook | ✅ Working | `https://localhost:7111/api/auth/callback/facebook` | HTTPS required by Facebook |

**Tested flow:**
1. `GET /api/auth/login/{provider}` → 302 to provider
2. User authenticates → provider redirects to callback
3. Backend exchanges code → creates/links account → returns `loginCode`
4. In Development mode: returns JSON with `publicCode`, `sessionId`, `identity`

**Multi-provider:** Same email links to same account. Both Google and Facebook can be linked to one account.

**Public Code format:** `MOE-XXXXXXXXXX` (Crockford Base32, user-facing ID).

**Account entity:** Two IDs — `Id` (Guid, server-only) + `PublicCode` (MOE-, user-facing).

### Preferences — ✅ Working

7 slices: Gameplay, Accessibility, Language, Notifications, Social, Audio, UI.
All support GET, PUT, PATCH, reset, and defaults endpoints.

### Social — ✅ Working

Friends, friend requests, blocks, party invites. Server-authoritative.

### Profile — ✅ Working

Username + Avatar persistence via MongoDB.

---

## What clients need before starting

### Must-have (blocked on story docs)

| Item | Why |
|---|---|
| **Story documentation complete** | Game mechanics, UI flow, and screen layout must be defined before implementing client screens |
| **Character/system docs finalized** | Heroes, gacha, tower, combat systems need stable definitions |
| **Screen flow definition** | Title → Login → Menu → Battle flow must be locked |
| **Asset pipeline decision** | How art/audio assets are delivered and managed |

### Must-have (backend ready, client not started)

| Item | Status | What's needed |
|---|---|---|
| Auth client | ✅ Backend ready | OAuth flow (open browser, store refresh token securely) |
| Preferences client | ✅ Backend ready | GET all at login, PUT/PATCH slices, handle 409 |
| Social client | ✅ Backend ready | Friends, blocks, party invites |
| Profile client | ✅ Backend ready | GET/PUT username, avatar |
| Device settings manager | ⬜ Not started | `graphics.json`, `input.json`, `device.json` or engine equivalent |
| Hybrid resolver | ⬜ Not started | Account intent + device caps → effective config |

---

## Missing for Unity (`v1/`)

| Category | What's missing |
|---|---|
| **Project setup** | Unity project structure, assembly definitions, package manifest |
| **Networking** | HTTP client wrapper for backend API calls |
| **Auth flow** | OAuth browser redirect, token storage, refresh logic |
| **Settings UI** | Settings screen with all preference slices |
| **Social UI** | Friends list, request management, block list, party invites |
| **Profile UI** | Username display/edit, avatar selection |
| **Device settings** | Graphics, input, audio output configuration |
| **Hybrid resolver** | Merge account prefs with device caps |
| **Asset pipeline** | Art/audio loading, addressables or equivalent |

## Missing for Unreal (`v2/`)

| Category | What's missing |
|---|---|
| **Project setup** | UE5 project structure, C++ modules, Blueprints |
| **Networking** | HTTP client for backend API calls |
| **Auth flow** | OAuth browser redirect, token storage, refresh logic |
| **Settings UI** | UMG widgets for all preference slices |
| **Social UI** | Friends, blocks, party invite widgets |
| **Profile UI** | Username/avatar display and edit |
| **Device settings** | `GameUserSettings`, SaveGame, ini files |
| **Hybrid resolver** | Account intent + device capabilities |
| **Asset pipeline** | Content delivery, packaging |

---

## Story documentation gaps (blocking client work)

| Gap | Impact |
|---|---|
| No finalized screen flow | Can't implement navigation |
| No character/system docs | Can't implement game screens |
| No asset specifications | Can't build UI or load assets |
| No gameplay mechanics defined | Can't implement battle, gacha, tower |
| No UI/UX mockups | Can't implement settings screens |

**Until story docs are complete, client folders (`v1/`, `v2/`) remain documentation-only.**
