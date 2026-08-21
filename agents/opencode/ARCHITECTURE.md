# Architecture — Understanding of the Project

> Technical design target and current implementation state.

---

## Repo Layout (Actual)

```
/  (root)
├── CONTRIBUTING.md          versioning + branching + folder conventions
├── .gitignore
├── docs/                    engine-agnostic general knowledge
│   ├── game-overview.md     game facts, core features
│   ├── roadmap.md           build stages 1-4
│   ├── decisions.md         standing decisions & conventions (authoritative)
│   ├── quantum-ai-player.md + dev-notes/quantum-ai-dev-note.md
│   ├── trello-board.md      board design, labels, sprint 1
│   ├── story/               chapters 1-8, character-tracker, timeline, player-progression
│   ├── characters/          han, jenna, aaron, iselle, anytng, shay, niflheimr-elite
│   ├── psychology/          emotional_rules, han_behaviour
│   ├── systems/             tower, heroes, economy
│   ├── technical/           architecture, server-modules, script-format, glossary, folder-structure
│   └── tasks/               backend, frontend, design (9 Discord tasks, all Pending)
├── v1/                      Unity C# specifics (README, docs/tasks, docs/technical-reference)
├── v2/                      Unreal C++ specifics (README, docs/tasks, docs/technical-reference)
├── interfaces/              React+Vite+TypeScript UI prototype (NEW)
│   ├── src/
│   │   ├── App.tsx          main flow router (22 screens)
│   │   ├── types.ts         ScreenId, DeviceType, AuthState, etc.
│   │   ├── state/           useFlowState hook (device, auth, screen machine)
│   │   ├── components/      GothicFrame, GothicButton, MissionBanner, AmbientBackground, etc.
│   │   ├── screens/         16 screen components
│   │   ├── data/            storyScreens.ts (Chapter 2 text)
│   │   └── styles/          global.css (CSS vars, animations, responsive)
│   └── package.json         React 19, Vite 6, TypeScript 5
└── agents/opencode/         THIS memory system
```

## Current Implementation: interfaces/ Prototype

**Stack:** React 19 + Vite 6 + TypeScript 5 (strict mode)

**Design system:**
- Gothic dark fantasy aesthetic — Cinzel / Cinzel Decorative fonts
- SVG ornamental frame system (GothicFrame) with animated electricity corners
- 3D depth: perspective transforms, multi-layer box-shadows, bevel highlights
- Dark panel body (#121020 -> #06050c), purple accent palette
- Responsive: ≤768px hides SVG ornaments, ≤480px shrinks buttons

**Screen flow (22 screens):**
```
Device Select → Initial → Connecting → Creating Account
→ Login Select → Authenticating → Login Confirmation
→ Name Input → Name Confirmation → Welcome → Tutorial Choice
→ Story 1-5 → Tutorial Quest → Combat Info → Summoning Prompt
→ Main Menu (with sub-panels)
```

**State management:**
- `useFlowState` custom hook (no external state library)
- `device`: PC/Console/Mobile
- `auth`: { provider, username, characterId } — randomly generated
- `currentScreen`: ScreenId union type
- `mainMenuPanel`: tracks open sub-panel
- `lockRef`: debounce guard prevents duplicate transitions

**Key components:**
| Component | Purpose |
|---|---|
| GothicFrame | Reusable SVG ornate frame with electricity corners, 3D depth |
| GothicButton | Primary/secondary variants with hover glow |
| MissionBanner | Floor/mission/goal display with SVG frame |
| AmbientBackground | Floating particles, gradients, vignette |
| ScreenTransition | Fade+scale wrapper |
| NameInput | Letter-only filtered input with validation |

**Sub-panels (from Main Menu):**
- Settings: Music/SFX volume sliders, graphics quality selector
- News: 3 mock game news items
- FAQ: 4 expandable questions
- Support: Mock donation placeholder
- Quit: Confirmation → "GAME EXITED — PROTOTYPE"

**Device-specific behavior:**
- Apple login: shown for mobile+PC, hidden for console
- Quit button: shown for PC/Console, hidden for mobile

**Build stats:** 55 modules, ~248 KB JS, ~3 KB CSS

## Intended Tech Stack (Target, for full game)

- **Runtime:** .NET 8 (or latest LTS)
- **API:** ASP.NET Core Web API (stateless: Gacha/Inventory)
- **Real-time:** ASP.NET Core SignalR (battle intervention, chat, matchmaking)
- **Persistence:** MySQL (accounts) + MongoDB (gameplay), abstracted behind repository interfaces
- **Caching:** In-memory (Redis recommended but optional for MVP)
- **Architecture style:** Modular Monolith (modules Combat/Gacha/Town rip-out-able to microservices)
- **Engine (chosen so far):** Unity. Unreal is a separate future scope (`2.0.0`).

## Key Design Decisions (from architecture.md / server-modules.md)

1. **Client is never trusted.** Server is the "Dungeon Master"; sanity checks catch impossible damage.
2. **Persistent SignalR connection = heartbeat.** Drop ⇒ player booted to title screen.
3. **Gacha:** pure REST atomic transaction (deduct currency → RNG → generate → save → commit; rollback on crash).
4. **Combat "Tactical Frame":** server simulates in chunks (≈10s or 1 round), sends `CombatStatePacket`; client animates; master may send `InterventionRequest` (validated server-side); invalid ⇒ `ActionFailed`.
5. **Permadeath:** on death, `Status = "Deceased"`, `DeceasedDate = Now`; unit stays in "Graveyard" (Hall of Fallen Heroes).
6. **Disconnect cheat counter:** on socket drop mid-battle, the battle continues server-side ~30s with AI taking over; deaths stick.
7. **Sanctuary timers:** timestamp math, no background per-player threads.
8. **Soft delete:** `UPDATE ... SET IsDeleted = 1, DeathReason = ...`, never `DELETE`.

## Build Roadmap (roadmap.md)

1. Foundation (ASP.NET Core project, login/auth, SignalR ping/pong)
2. Sanctuary (data storage mock/JSON, resource gen, gacha logic)
3. Tower (server combat simulator, auto-battle first, then intervention)
4. Crowd (matchmaking + leaderboards)
