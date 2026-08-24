# Decisions

> Decisions made during development and their rationale. `docs/decisions.md` in the project is the authoritative record; this file is the memory mirror and history.

---

## Current Standing Decisions (mirrors docs/decisions.md)

| Topic | Decision | Reason |
|---|---|---|
| Project phase | UI prototype exists in `interfaces/`; backend code deferred | User's direction to prototype UI first |
| Versioning | `1.0.0` = Unity, `2.0.0` = Unreal, intentional dual numbering | Keeps two engine environments for future scope discussion; Unity chosen so far |
| Database | MySQL (accounts) + MongoDB (gameplay), behind repository interfaces | Confirmed by user; keep DAL abstracted for future swaps |
| Lyle naming | `Lyle` correct; `Lyla` was a wording error | User clarification |
| NPC numbering | Start from 1, reset each chapter; unnamed share numeric ids; named → name only | Avoids runaway global ids like `NPC672920`; clean organisation |
| Repeated panels | Intentional across chapters (e.g. well-forged longsword) | Manhwa links chapters together |
| Timeline | Not 1 chapter = 1 day; time skips normal | User clarification |
| Progression tracking | Two viewpoints: `han.md` = latest-state snapshot; `player-progression.md` = per-chapter growth | Clarity of two viewpoints |
| Han level | Lv.7 as of Ch.5 (not stale Lv.1) | Verified from chapter text |
| Lyle/Rail | Same character — "Rail" is his in-game death-log alias (Ch.2 log) | Ch.2 "Rail (Lyle)" dedup |
| UI prototype scope | `interfaces/` is standalone Vite project, not monorepo | No other Node projects exist; simpler setup |
| Prototype vs production | `interfaces/` is UI prototype only — not game logic | Clean separation from future Unity/Unreal code |
| Frame style | Angular/spiky white corners, dark body (#121020), 3D depth | Matches reference images from game |
| Story text source | Chapter 2 manhwa dialogue used for story screens | Source of truth for tutorial flow |
| Tutorial skip | "No" on tutorial choice skips to main menu | User preference — some players skip tutorials |
| Auth flow | Mock only — no real OAuth, no passwords, no backend | Prototype requirement |
| Memory routing | Root `AGENTS.md` auto-loads and points every session to `agents/opencode/OPENCODE.md` | Guarantees memory is read without the user reminding |
| Interfaces isolation | `interfaces/` is a self-contained UI prototyping experiment, isolated from project structure; kept to preview the game before implementation | User clarification (v0.0.23+) |
| Cross-document audit | Story chapter updates require a dependency audit → report affected docs → wait for approval → apply → final consistency check; never propagate silently | Standing convention (v0.0.29); full workflow in `docs/decisions.md` |
| Game vs Manhwa split | Classify info as game-specific / manhwa-specific / shared canon / adaptation; keep current structure while manageable; proactively recommend separation before it becomes chaotic | User direction (v0.0.29) |
| Psychology scope | `docs/psychology/` = Character + Behaviour records accumulating chapter evidence where meaningful (`han_behaviour.md` model) — progression/continuity, not character-sheet restatement | User direction (v0.0.29) |
| Characters scope | Dedicated character records for explicitly named characters only; unnamed/background NPCs get no permanent record unless later named/recurring, then backfill | User direction (v0.0.29) |

## Historical Decision Log (by version)

- **v0.0.1–0.0.5:** Initial docs structure, .gitignore, Chapter 1, CONTRIBUTING guide with versioning/branching.
- **v0.0.6–0.0.8:** Chapters 1–2, character tracker, NPC numbering convention.
- **v0.0.9–0.0.14:** Tracker, Chapters 3–5, chapter corrections, Shay's line reverted to source, Chapter 6.
- **v0.0.14.1–0.0.16:** Corrections, Chapter 7 (Forging Ahead), Chapter 8 (Thinning and Roles).
- **v0.0.17:** Restructured docs into story/characters/psychology/systems; added emotional analysis.
- **v0.0.18–0.0.19:** Script format reference; chapters re-formatted to user's format.
- **v0.0.20–0.0.21:** Dialogue line formatting; chapter fixes, character files, game mechanics docs (economy/heroes/tower).
- **v0.0.22:** Added `docs/decisions.md`, recorded confirmed conventions, deduped Lyle/Rail, refreshed Han state.
- **v0.0.23:** React UI prototype (22 screens), opencode memory system, .gitignore for interfaces artifacts (user commit).
- **v0.0.24:** Chapter 9 (Survival Mission) documented; trackers/timeline/progression/systems updated; root `AGENTS.md` added.
- **v0.0.25:** Chapter 10 (survival battle) documented; new mechanics recorded (bleeding status, mid-battle fear trigger, weapon durability); Gide on brink of death cliffhanger tracked.
- **v0.0.26:** Chapter 11 documented — Gide and Hansen confirmed dead; Awakening mechanic added to heroes.md; roster/trackers updated to 6 alive.
- **v0.0.27:** Chapter 12 documented — Floor 5 cleared at 00:00, Han awakens Berserker, MVP; daily dungeon unlocked + waiting room concept added.
- **v0.0.28:** Chapter 13 documented — daily dungeon Kendert Forest run; promotion system (1★ cap Lv.10 → 2★), growth cost fixed at 5, Composure/Berserker conflict bug, master login pattern (3x/day), gathering mechanics (Branch x100 → Lumber), Queen of the Forest + attribute stone; Louis/Joffrey/Owen named (alive 9, total 33).
- **v0.0.29:** Approved Ch.13 audit propagation — timeline anchored to ~1 month in-game / ~10 days real; psychology docs extended to Ch.13 with corrected characterization (Han praises Gide/Hansen, contempt aimed at synthesis-dodgers, "business secret" = identity concealment); jenna.md/aaron.md updated; standing conventions recorded (cross-document audit workflow, Game/Manhwa monitoring, Psychology/Characters documentation methodology).
- **v0.0.30:** Chapter 14 documented via audit workflow (first full run) — Queen defeated + wind stone D-, Jenna awakens 'Hunter of the Forest' (achievement-triggered awakening broadens the mechanic), Sinmiel Plateau water stone, Plants of Life, Floor 6 Explore mission + Dika joins Party 1, goblin village trap cliffhanger. Also repaired Tracking Notes header accidentally removed in v0.0.28.
- **v0.0.31:** Chapter 15 documented via audit workflow — Floor 6 clear (MVP Han), streams concept + glossary terms, affinity-link consecutive summons, summon-rank knowledge gap, 'newbie potent package' economy, Pulverizing Wolves 3★ x5 added to trackers (alive 14, total 38); Jenna quote/key moment additions approved by user.
- **v0.0.32:** Chapter 16 documented via audit workflow — unexamined post-pull synthesis (Diman & Lexigel consumed, Jaken level-up), Sorial & Daniel seized (cliffhanger), Han's TIP-via-Iselle channel to the master, thief-troupe exposure → team-wide hostility notifications, Open Duel system (Han vs Avant, synthesis stake); heroes.md Duel/Hostility sections added; glossary 'Open Duel' term; counts updated (synthesis deaths 6, total 42). Speaker-tag fix: the "S-save me please!!" line belongs to Sorial, not Lexigel.
- **v0.0.33:** Chapter 17 documented via audit workflow — duel executed: ANYTNG approves via Yes/No unknowingly; Han Berserk-dominates Avant (sword never unsheathed); Edis recruited mid-duel (stayed for her father's request) → party disbanded; Iselle's self-defence kills interfering Jaken/Wave/Beignin ([Sudden Death] suicide due to stress); Avant consumed per condition; Han Lv.10 MAX + skill-ups + promotion prompt (stone or same-star hero, cap 7★); Berserker modifiers/headache documented; Manager Self-Defence + Party Disbanding sections added; counts: alive 12, synthesis 7, sudden death 3. User clarifications applied: ch15 NPC2 = Joffrey (backfilled); skill names always capitalized (retro-fixed ch3/4/17); Han exp normalized to 54/70 (ch17 panel 11/70 confirmed manhwa error).
- **v0.0.34–0.0.35:** Chapters 18–23 + 24 documented (separate session) — mass synthesis of six slackers into Edis (+Trap Disarm); Usher Roderick hand-picked, evening weeding → Lv.3; two-party lot split (Red: Han/Jenna/Aaron, Blue: Edis/Usher/Dika) via party suggestions; Floor 7 cleared (MVP Han, dam-collapse suspicion); double package purchase + Rare Eolka/Roderick summons; Yon/Zenin object-lesson synthesis; crack of time/space tests; master's revenge Floor 8 (Goblin Raider ×27, La Gran Sedus, double Berserk stamina cap); Ignite/Burn/Transcend clear (MVP Eolka, "tank without armor"); Magic Hall + potion rhythm game + fire/pain resistance training + triangular formation. Fix commit clarified brazier count as seconds.
- **v0.0.36:** Chapter 25 documented (separate session) — training montage (<Projectile Defense>, Sword-Shield Lv.6, Switching abnormal rate), level snapshot Han 10/Jenna 8/Aaron 6/Eolka 4, D-rank gear + support daggers, Shurn & Mehkin fill parties, Floor 9 report (muddy ground clue), tactical station + consumables system, ANYTNG challenges Floor 10, pre-ruined city from Floor 5 revealed; 'Pick Me Up' capitalization normalized repo-wide.
- **v0.0.37:** Post-Ch.25 verification pass (this session) — verified other session's commits against user's re-pasted source text; fixed residual drift: progression log chapter order (25→24 swap), three "10 times" stragglers → 10-second count, `Beginner`/`Basic` skill-name wobble unified to **Basic Sword-Shield Technique** per Stat Panel Discrepancies convention (recorded in docs/decisions.md), two truncated "get fuck." quotes restored to transcript wording, ch23 `*in my mind*` tag slip; caught up five stale character files (jenna/aaron/eolka/roderick/anytng/iselle Ch.24-25 arcs, aaron skill table un-stuck from Lv.2), glossary +12 terms (Magic Hall → Tank Without Armor), emotional_rules extended with Two-Party Era section + matrix rows; han.md stats header → Ch.25.

## Open Questions / Pending

- **Chapter 26**: not yet provided; Floor 10 battle in progress (two parties, pre-ruined city, NPC who can't see heroes).
- Game/Manhwa folder separation deferred until the structure becomes hard to manage (monitor actively).
- Task statuses (backend/frontend/design) remain Pending — code development has not been announced.
- `docs/licenses/` directory exists but is empty (placeholder).
- Whether the prototype should be wired to real backend APIs or remain mock-only.
- Whether to add more story screens beyond the current 5 + 3 tutorial screens.
- Mobile-specific UI refinements beyond layout adaptation.
