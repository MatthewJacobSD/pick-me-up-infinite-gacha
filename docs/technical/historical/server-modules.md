# Server Modules — The Logic Layers

---

## The Gatekeeper (Auth & Session)

**Role:** Handles login, version checking, and the "Heartbeat"

**Architecture:**
- Standard JWT (JSON Web Token) authentication
- Duplicate Login Check: When a User connects, check the `ConnectionManager`. If the `UserID` is already active on another socket, disconnect the old one (preventing account sharing/cloning)

---

## The Sanctuary Service (Town Management)

**Logic:** Server-Authoritative Timers

**Implementation:**
- Passive Resource Gen: Do not run a background thread ticking up gold every second for every player. That kills CPUs.
- **The Timestamp Math:** When a player requests to "Collect Gold", the server calculates:

  ```
  (CurrentTime - LastCollectionTime) * ProductionRate
  ```

- Construction: When building a facility, store `CompletionTime` in the DB. When the client asks "Is it done?", compare `CurrentTime >= CompletionTime`.

---

## The Dimensional Store (Gacha System)

**Logic:** Secure Random Number Generation (RNG)

**Implementation:**
- Must be pure REST API (Atomic Transaction)
- Process:
  1. Start Transaction
  2. Deduct Currency (Crystals)
  3. Run RNG Logic (Weighted Tablets for 1* to 5*)
  4. Generate Unit Data (Stats, Unique ID)
  5. Save to Player Inventory
  6. Commit Transaction
  7. Return Result to Client
- **Note:** If the server crashes at step 4, the transaction rolls back. The player loses no currency.

---

## The Tower Engine (Combat & Tactics)

> "Master plans, Server simulates, Master intervenes"

### The Simulation Approach

We cannot trust the client to calculate damage. The server is the "Dungeon Master".

### Phase 1: Preparation (REST/SignalR)

Player sends `CombatStartRequest` with a list of Hero IDs and initial Formation/Tactics. Server validates energy/stamina and locks the unit loadout.

### Phase 2: The Simulation Loop (Server-Side)

The Server runs a Turn-Based/Tick-Based Simulation in memory.

**Crucial Design Choice:** Since the player can "Intervene", the server cannot just calculate the whole battle in one millisecond and send the result. It must pace itself or use breakpoints.

**The "Tactical Frame" Pattern:**
- The server calculates the battle in "chunks" (e.g., 10 seconds of combat or 1 full round)
- The server sends a `CombatStatePacket` to the client:
  - Contains: "Hero A moves to X, Y", "Hero B attacks Enemy C for 50 dmg"
- The Unity Client plays out these animations

### Phase 3: Intervention (SignalR)

While the client is playing the animation, the Master presses a skill button (Intervention):

1. Client sends `InterventionRequest` (`SkillID`, `TargetID`)
2. Server validates: "Does the Master have mana? Is the cooldown up?"
3. If Valid: The server injects this action into the next simulation chunk
4. If Invalid: Server sends `ActionFailed`

### Phase 4: Result & Permadeath

- Battle ends: Server calculates drops
- **Permadeath Logic:** If `Hero.Health <= 0`, update the database:
  - `Status = "Deceased"`
  - `DeceasedDate = Now`
- These units are no longer selectable in Loadouts but remain in the "Graveyard" (History)
