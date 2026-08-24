# Context

> Story/world context and current-state snapshot, distilled from `docs/` so future sessions can resume without re-reading everything.

---

## Game Identity

- **Name:** Infinite Gacha TM ("Pick Me Up: Infinite Gacha")
- **Engine:** Unity (chosen so far); Unreal considered for future scope
- **Organisation:** Moebius Order DO
- **Platform:** PC, Mobile, Console (cross-platform), fully online
- **Mode:** Fully online; 24/7 server connectivity; client never trusted

## The Story (as of Ch. 24)

- **Protagonist:** Han Seojin, real player from Seoul (Gangseo-Gu, South Earth). Summoned into his own game as a **1★ hero "Islat Han"** by a Level-999 bugged entity (the God of Death). As past master/Loki he confesses the appeal: "There's no way it wouldn't be fun to be able to control their lives." **Arc shift (Ch. 23):** "I think I'm slowly approaching the time where I'll no longer be at the center" — acknowledging his role is evolving from primary damage to leadership/strategy as Eolka's firepower outpaces his. **Skills now include:** <Fire Resistance> (Ch. 24, obtained via brazier training).
- **Big reveal:** Han **is Loki** (Account #46631913, Rank 5, "Master of Masters"). Heroes are real humans from across ~1M worlds.
- **The Master:** **ANYTNG** — reckless office worker; built Magic Hall on login (Ch. 24); watching the fire-resistance training as entertainment ("it is fun to watch even if you're not doing anything").
- **Iselle:** Pixie lounge manager; Loki fangirl; praises Han's potion crafting ("As expected of Loki!").
- **Jenna Cirai:** First ally, calls Han "Oppa"; archery Lv.5; assigned Switching skill (dagger/arrow quick-switch); fire resistance training pending; "high maintenance" comment re: Eolka.
- **Aaron:** Loyal disciple, calls Han "Hyung-nim"; spear stuck at Lv.2; levelled up Floor 7 + Floor 8; assigned Pain Resistance training; "If you're tired, I'll protect you" (Ch. 22).
- **Edis Callen (3★):** Ex-Wolves defector; + Trap Disarm; Party 2 leader.
- **Usher Roderick (1★→Lv.3):** Porter; Roderick = family name; Party 2.
- **Roderick (3★):** Veteran city guard; crack test with Party 2 (Ch. 20).
- **Eolka Rivel Strashur (3★):** **MVP Floor 8**; levelled up; spell sequence "Ignite! Burn! Transcend!"; "tank without armor"; custom magic clothing (family heirloom) increases fire magic strength; stamina training (3 laps daily); collapsing after full spell from mana depletion. Triangle formation: Eolka at centre.
- **Magic Hall (Ch. 24):** magic research lab + alchemy lab + library + training annex; Isralta Mine opened (all weekday dungeons accessible); potion crafting via rhythm-game mini-game (6 health + 2 magic potions crafted).
- **Floor 10 cooperation mission:** at least 2 parties required; mission types: subjugation, survival, escort, breakthrough, assassination, protection, siege, pursuit, escape; chain quest hint: "1~?? Parties Enter"; Party 2 likely to Floor 9 first.

## Current State Snapshot (through Ch. 24)

- **Roster: Party 1 (Red):** Islat Han, Jenna Cirai, Aaron Delcut, Eolka Rivel Strashur (3★ MVP). **Party 2 (Blue):** Edis Callen, Usher, Dika, Roderick (3★). Support: Patrick, Enok, Chloe, Amarin, Alter. (Sorial & Daniel off-screen, kept Alive per user ruling.)
- **Magic Hall built (Ch. 24):** research skills unlocked; alchemy lab for item synthesis; library for mages; Isralta Mine opened (all weekday dungeons accessible); potion crafting via rhythm-game mini-game (6 health + 2 magic potions crafted).
- **Fire resistance training (Ch. 24):** Han walked through Eolka's fire10 times → obtained <Fire Resistance> skill; Aaron assigned Pain Resistance; Jenna assigned Switching (dagger/arrow quick-switch); new triangular formation with Eolka at centre.
- **Floor 10 cooperation mission confirmed (Ch. 24):** at least 2 parties; mission types: subjugation, survival, escort, breakthrough, assassination, protection, siege, pursuit, escape; chain quest hint: "1~?? Parties Enter"; Party 2 likely to Floor 9 first.
- **Han:** **Lv.10 — MAX for 1★**, promotion STILL pending. Skills now include <Fire Resistance>; arc shifting toward leadership.
- **Standing facts:** ANYTNG logs in 3x/day; waiting room 3x dilation; battle stages real-time; master controls crack destination. Synthesis death count: 18+.
- **Data note:** Han's Lv.9 exp normalized to 54/70 (Ch. 17 panel showed 11/70 — user confirmed manhwa error).

## Pending Story Beats

- Han's stamina recovery after double Berserk (Ch. 24)
- Han's promotion to 2★ (Lv.10 max, materials ready)
- Forest / linked-quest thread (dam-collapse; was "blocked off") (Ch. 24+)
- Whatever Han kept Aaron behind for (Ch. 19)
- Large goblin habitat / next stream chain (Ch. 17+)
- Plants of Life gathering (Ch. 15+)
- Sirris arrives (Ch. 10+)
- Yurneth finds master (Ch. 10+)
- Han vs Niflheimr (Ch. 15+)

## UI Prototype State (interfaces/)

**Current flow (22 screens):**
```
Device Select → Initial → Connecting → Creating Account
→ Login Select → Authenticating → Login Confirmation
→ Name Input → Name Confirmation → Welcome → Tutorial Choice
→ Story 1-5 → Tutorial Quest → Combat Info → Summoning Prompt
→ Main Menu (with sub-panels)
```

**Prototype features implemented:**
- Device selection (PC/Console/Mobile) — stored in state, affects UI
- Mock auth flow (Google/Facebook/Apple/Guest) — randomly generated username + character ID
- Gothic SVG frame system with animated electricity corners
- 3D depth effects (perspective, multi-layer shadows, bevels)
- Data-driven story screens using Chapter 2 source text
- Main menu with Settings, News, FAQ, Support, Quit panels
- Responsive layout (desktop/tablet/mobile)
- Keyboard navigation (Enter/Escape/Space)

**Prototype features NOT implemented (future ideas):**
- Real backend integration
- Actual gameplay screens (combat, gacha, inventory)
- Sound effects / music
- Save/load state (currently session-only)
- More story chapters beyond the 5 panels
- Particle effects beyond basic floating
- Tooltips / help system
- Accessibility features (screen reader, high contrast)

## Technical Reference Summary

- Lore ↔ tech mapping lives in `docs/technical/glossary.md` (Master=User, Smartphone=Client Interface, Sanctuary=Game State/Town Service, Tower=Combat Instance, Dimensional Store=Gacha Service, Intervention=Interrupt Request, Space Rift=Latency).
- Combat pacing: "Tactical Frame" chunks over SignalR; client animates, server validates.
- Gacha: atomic REST transaction.
- Permadeath is permanent; soft-delete keeps the "Hall of Fallen Heroes".

## Communication Notes

- A reply appearing only at the top of the chat does not mean the session is broken — it is a TUI viewport issue; scroll down. The chat remains functional.
- The user transcribes the manhwa as it comes; author name inconsistencies between chapters are to be expected and preserved, not "fixed".
- Transcription typo handling (established Ch. 18–20): obvious fast-typing errors are cleaned in chapter docs (e.g. "string"→"strong", "misurderstandng"→"misunderstanding", "Audio"→"Andio", "mux"→mix); intentional stylized speech is preserved verbatim (e.g. Jenna's pinched-cheek slur "Whut ar eww swing?!"). The user writes quickly and misses typos — fix them proactively and report each normalization so it can be vetoed.
