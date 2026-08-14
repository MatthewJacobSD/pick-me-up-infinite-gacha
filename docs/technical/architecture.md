# Technical Design Document: Project Niflheim — Server Architecture

**Target Platform:** Unity Client / Custom C# Server
**Framework:** ASP.NET Core (Hybrid REST + SignalR)
**Version:** 1.0

---

## High-Level Architecture

24/7 connectivity, we will utilize a Modular Monolith architecture. This keeps development speed high and deployment cost low, but structures the code so modules (Combat, Gacha, Town) can be ripped out and turned into microservices later if the game scales massively.

### The Stack

- **Runtime:** .NET 8 (or latest LTS)
- **API Layer:** ASP.NET Core Web API (for stateless requests like Gacha/Inventory)
- **Real-Time Layer:** ASP.NET Core SignalR (for Battle Intervention, Chat Matchmaking)
- **Persistence:** Abstracted (Repository Pattern)
- **Caching:** In-Memory (Redis recommended but optional for MVP)

The Unity client will maintain a persistent SignalR (WebSocket) connection to the server. This serves as the "heartbeat". If this connection drops, the player is booted to the title screen, preventing client-side state manipulation.

---

## Database Strategy — The Abstracted Codex

The database stack is confirmed as **MySQL** (account data) + **MongoDB** (gameplay data). We still design the Data Access Layer (DAL) using interfaces, so swapping or extending engines later does not require rewriting game logic.

### Core Entities (Schema Concepts)

- **Account:** `Id`, `Username`, `MasterLevel`, `Currencies`
- **Roster:** `Id`, `AccountId`, `UnitTemplateId`, `Level`, `Exp`, `Equipment`, `Status` (Active/Dead/SoftDeleted)
- **Sanctuary:** `AccountId`, `BuildingList` (Json), `Timers` (Json)
- **BattleHistory:** `Id`, `Result`, `ReplayData` (Compressed)

Soft Delete Implementation: Never run `DELETE FROM Heroes WHERE Id = 1`. Instead, run `UPDATE Heroes SET IsDeleted = 1, DeathReason = "Goblin Ambush" WHERE Id = 1`.

This allows you to show a "Hall of Fallen Heroes" feature.

---

## Matchmaking — Multiplayer

For PVP or Co-op, we need a way to group players.

### The "Bucket" Matchmaker (Simple & Effective)

Since we are using ASP.NET Core Memory or a Redis Cache:

1. Player clicks "Find Match"
2. Server adds Player to `MatchmakingQueue` list in memory
3. A background `HostedService` runs every 2 seconds:
   - It looks at the Queue
   - It sorts players by `MasterLevel` or `EloRating`
   - It pairs two players with similar scores
4. Server creates a `MatchID` and sends it to both players via SignalR
5. Both players load into a Combat Instance

---

## Security & Anti-Cheat

- **Sanity Check:** If a Level 1 Hero deals 99,999 damage, the server logic (Combat Engine) will catch it because the server calculates the damage, not the client
- **Speed Hacks:** Since combat is paced by the server sending "Chunks" of data, a player speeding up their Unity client will just finish the animation early and sit waiting for the server to send the next chunk
- **The "Disconnect" Cheat:** If a player is losing a battle and Alt+F4s to save their unit from Permadeath:
  1. The server detects the socket disconnect
  2. **The Grim Reaper Logic:** The battle continues on the Server for 30 seconds (or until end). The AI takes over the player's team (usually playing poorly)
  3. If the unit dies while the player is disconnected, it stays dead
