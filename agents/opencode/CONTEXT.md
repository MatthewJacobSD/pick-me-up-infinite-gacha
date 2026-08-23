# Context

> Story/world context and current-state snapshot, distilled from `docs/` so future sessions can resume without re-reading everything.

---

## Game Identity

- **Name:** Infinite Gacha TM ("Pick Me Up: Infinite Gacha")
- **Engine:** Unity (chosen so far); Unreal considered for future scope
- **Organisation:** Moebius Order DO
- **Platform:** PC, Mobile, Console (cross-platform), fully online
- **Mode:** Fully online; 24/7 server connectivity; client never trusted

## The Story (as of Ch. 23)

- **Protagonist:** Han Seojin, real player from Seoul (Gangseo-Gu, South Earth). Summoned into his own game as a **1★ hero "Islat Han"** by a Level-999 bugged entity (the God of Death). As past master/Loki he confesses the appeal: "There's no way it wouldn't be fun to be able to control their lives." **Arc shift (Ch. 23):** "I think I'm slowly approaching the time where I'll no longer be at the center" — acknowledging his role is evolving from primary damage to leadership/strategy as Eolka's firepower outpaces his.
- **Big reveal:** Han **is Loki** (Account #46631913, Rank 5, "Master of Masters"). Heroes are real humans from across ~1M worlds.
- **The Master:** **ANYTNG** — reckless office worker; whale-psychology read; retaliated by sending Party 1 to Floor 8 instead of Floor 4 (Ch. 21).
- **Iselle:** Pixie lounge manager; Loki fangirl; invokes the crack for test/deployment stages.
- **Jenna Cirai:** First ally, calls Han "Oppa"; archery Lv.5; cross-training daggers with Edis; callled Eolka "Unnie" (Ch. 22-23) — growing familiarity.
- **Aaron:** Loyal disciple, calls Han "Hyung-nim"; spear stuck at Lv.2; levelled up on Floor 7 and again on Floor 8 (Ch. 23); "If you're tired, I'll protect you" — growing confidence (Ch. 22).
- **Edis Callen (3★):** Ex-Wolves defector; + Trap Disarm; Party 2 leader.
- **Usher Roderick (1★→Lv.3):** Porter; Roderick = family name (distinct from Roderick 3★); Party 2.
- **Roderick (3★):** Veteran city guard; entered crack test with Party 2 (Ch. 20); performed better than Edis expected.
- **Eolka Rivel Strashur (3★):** **MVP of Floor 8 clear (Ch. 23)**; levelled up; full spell sequence **"Ignite! Burn! Transcend!"** — destroyed 20+ Goblin Raiders; collapsed from total mana depletion. All stats below 1★ (STR/HP/AGI all 7/7) — **"tank without armor"** (Han's analogy: firepower for defense/mobility; made for war if protected). ~1-minute stationary cast; party members must protect her during cast. No support magic; no skill window; "Rivel's Witch" / "witch of Count Rivel's Spirit".
- **Floor 8 CLEARED (Ch. 23):** Goblin Raider Lv.9 ×27; rewards 20,000G, Iron Ore (B) ×3, Wolf Leather ×5. Forest zone ("connecting field between forest and plains") was newly accessible — may connect to dam-collapse thread.

## Current State Snapshot (through Ch. 23)

- **Roster per Ch. 20: Party 1 (Red):** Islat Han, Jenna Cirai, Aaron Delcut (levelled up, Ch.23), Eolka Rivel Strashur (3★, MVP, levelled up, Ch.23). **Party 2 (Blue):** Edis Callen, Usher, Dika, Roderick (3★). Support: Patrick, Enok, Chloe, Amarin, Alter. (Sorial & Daniel off-screen, kept Alive per user ruling.)
- **Floor 8 CLEARED (Ch. 23):** Eolka's "Ignite! Burn! Transcend!" destroyed 20+ Goblin Raiders; MVP Eolka; rewards 20,000G, Iron Ore (B) ×3, Wolf Leather ×5. Eolka collapsed from total mana depletion. Han's arc shift: "slowly approaching the time where I'll no longer be at the center."
- **Berkerk stamina cap confirmed**: two uses in one day exhausts Han completely (Ch. 22); headache + forced out of berserk state.
- **Han:** **Lv.10 — MAX for 1★**, promotion STILL pending. Double Berserk fatigue on Floor 8; arc shifting toward leadership role.
- **Standing facts:** ANYTNG logs in 3x/day; waiting room 3x dilation; battle stages real-time; master controls crack destination (retaliation precedent). Forest zone on Floor 8 may link to dam-collapse thread. Synthesis death count: 18+.
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
