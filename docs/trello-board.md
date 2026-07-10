# Trello Board Design — Pick Me Up: Infinite Gacha

## Board: Infinite Gacha — Development

---

## Labels

| Label | Color | Description |
|---|---|---|
| `Backend` | Blue | C# Server / API tasks |
| `Frontend` | Green | Unity Client tasks |
| `Design` | Purple | UI/UX & Visual tasks |
| `Priority: High` | Red | Critical path items |
| `Priority: Medium` | Yellow | Important but not blocking |
| `Release Notes` | Orange | Project Quantum AI & releases |

---

## Lists

### 📋 Backlog

> Tasks identified but not yet scheduled for current sprint.

| Card | Labels | Description |
|---|---|---|
| **Facebook Login Integration** | `Backend` `Frontend` | Add Facebook login support (architecture prepped in auth task) |
| **Matchmaking System** | `Backend` | Bucket matchmaker — PVP & Co-op pairing |
| **Leaderboards** | `Backend` `Frontend` | Competitive ranking display |
| **Database Migration Strategy** | `Backend` | Plan for schema versioning & rollbacks |
| **Cross-Platform Optimization** | `Frontend` `Design` | PC / Mobile / Console layout adaptation |
| **Chat System** | `Backend` `Frontend` | Alliance & global chat via WebSocket |
| **Audio Integration** | `Design` `Frontend` | Music, SFX, voice systems |
| **VFX & Animation Systems** | `Design` `Frontend` | 2D/3D art pipeline, character animation |

---

### 📌 To Do (Sprint 1)

> Current sprint — committed deliverables.

| Card | Labels | Checklist |
|---|---|---|
| **Database Modeling** | `Backend` `Priority: High` | ☐ MySQL schema (id, username, email, phone, island coords, creation date, settings) |
| | | ☐ MongoDB schema (xp, gold, crystal, hp, mp, character level) |
| | | ☐ Document separation rationale |
| | | ☐ Registration creates records in both DBs |
| | | ☐ Unit tests: schemas, fields, separation, default data |
| **Backend Connection to MySQL and MongoDB** | `Backend` `Priority: High` | ☐ MySQL connection |
| | | ☐ MongoDB connection |
| | | ☐ Environment variables for credentials |
| | | ☐ Data access layer (separated by responsibility) |
| | | ☐ Connection check on server startup |
| | | ☐ Unit tests: mocked connections, failure handling, read/write, env vars |
| **Token-Based Authentication** | `Backend` `Priority: High` | ☐ Login with email/password |
| | | ☐ Login with Google |
| | | ☐ Token generation after login |
| | | ☐ Token validation on protected routes |
| | | ☐ Architecture ready for Facebook |
| | | ☐ Unit tests: valid/invalid creds, token gen, validation, expiration, access denied |
| **Authenticated WebSocket Integration** | `Backend` `Priority: High` | ☐ Open WebSocket connection |
| | | ☐ Validate token on connection |
| | | ☐ Block unauthenticated users |
| | | ☐ Channel for game data send/receive |
| | | ☐ Base structure for future messages |
| | | ☐ Unit tests: valid/invalid token, rejection, message send/receive |
| **Title Screen with Login Verification** | `Frontend` `Priority: High` | ☐ Display title screen |
| | | ☐ Auto-check login on load |
| | | ☐ Valid token → tap → main menu |
| | | ☐ No token → show login buttons |
| | | ☐ Unit tests: rendering, token detection, redirection, button visibility |
| **Main Game Menu** | `Frontend` `Priority: High` | ☐ Display Play, Options, Exit buttons |
| | | ☐ Navigation between screens |
| | | ☐ Base for future options |
| | | ☐ Unit tests: rendering, clicks, navigation, exit |
| **Login Screen** | `Frontend` `Priority: High` | ☐ Email/password login |
| | | ☐ Google login |
| | | ☐ Facebook-ready architecture |
| | | ☐ Receive & store token |
| | | ☐ Redirect to main menu |
| | | ☐ Unit tests: rendering, submission, token storage, error handling, Google flow |
| **Front-End WebSocket Integration** | `Frontend` `Priority: High` | ☐ Connect after auth |
| | | ☐ Send token on connect |
| | | ☐ Receive backend responses |
| | | ☐ Real-time data exchange prep |
| | | ☐ Disconnect/reconnect handling |
| | | ☐ Unit tests: connection, failure, message reception, disconnect |
| **Visual Layout for Main Screens** | `Design` `Priority: High` | ☐ Title screen layout |
| | | ☐ Login screen layout |
| | | ☐ Main menu layout |
| | | ☐ Button/color/hierarchy guide |
| | | ☐ Visual reference for front-end |
| | | ☐ Validation: navigable prototype, reproducibility check, consistency checklist |

---

### 🔨 In Progress

> Currently being worked on.

*No tasks currently in progress.*

---

### 👀 Review

> Tasks awaiting code review, QA, or stakeholder sign-off.

*No tasks currently in review.*

---

### ✅ Done

> Completed and merged.

*No tasks completed yet.*

---

### 🚀 Releases

> Milestone cards tracking version releases and notes.

| Card | Labels | Details |
|---|---|---|
| **v0.0.1 — Documentation Foundation** | `Priority: Medium` | ✅ Initial project documentation structure (game-overview, architecture, glossary, server modules, folder structure, roadmap, Quantum AI notes) |
| **v0.0.2 — Task Documentation** | `Priority: Medium` | ✅ Discord tasks documented (9 tasks across backend, frontend, design) |
| **v0.0.3 — Trello Board Design** | `Priority: Medium` | 🔄 This document |
| **v1.0.0 — Project Quantum AI Launch** | `Release Notes` `Priority: High` | ⬜ Player-facing Quantum AI release — adaptive character AI, unique personalities, dynamic gameplay |
| **v1.1.0 — Backend MVP** | `Release Notes` | ⬜ Database modeling, connections, auth, WebSocket all operational |
| **v1.2.0 — Frontend MVP** | `Release Notes` | ⬜ Title screen, login, main menu, WebSocket integration complete |
| **v2.0.0 — Core Gameplay** | `Release Notes` | ⬜ Combat system (Tower Engine), Gacha (Dimensional Store), Sanctuary (Town) live |

---

## Project Quantum AI — Release Notes (v1.0.0)

### What's New
- **Intelligent & Responsive Characters** — Heroes and NPCs now think and react dynamically to game events
- **Unique Personality Profiles** — Each character has distinct behavioural tendencies (cautious healer, bold warrior, clever rogue)
- **Adaptive Gameplay** — AI adapts to player playstyle, ensuring no two sessions feel identical

### Key Features
- Characters make decisions based on personality + environment
- Dynamic responses replace fixed scripts
- Enhanced immersion through authentic character behaviour
- Endless replayability via AI-driven unique scenarios

### Player Impact
- Deeper emotional connection to characters
- More engaging and unpredictable battles
- Living, breathing game world

### Technical Foundation
- Powered by **Project Quantum AI** advanced local intelligence
- Designed for performance across all platforms (PC, Mobile, Console)

---

## Board Workflow Rules

1. **New tasks** go to `Backlog` or `To Do` (Sprint) based on priority
2. **Sprint planning** moves items from `Backlog` → `To Do`
3. **Starting work** moves item `To Do` → `In Progress`
4. **Completion** moves item `In Progress` → `Review`
5. **After review/QA** moves item `Review` → `Done`
6. **Releases** get a card in the `Releases` list with notes

## Card Movement Rules

- A card must have all checklist items checked before moving to `Review`
- `Release Notes` cards are created retroactively after `Done`
- Blocked cards stay in their current list with a `BLOCKED` comment
