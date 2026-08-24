# Context

> Story/world context and current-state snapshot, distilled from `docs/` so future sessions can resume without re-reading everything.

---

## Game Identity

- **Name:** Infinite Gacha TM ("Pick Me Up: Infinite Gacha")
- **Engine:** Unity (chosen so far); Unreal considered for future scope
- **Organisation:** Moebius Order DO
- **Platform:** PC, Mobile, Console (cross-platform), fully online
- **Mode:** Fully online; 24/7 server connectivity; client never trusted

## The Story (as of Ch. 25)

- **Protagonist:** Han Seojin, real player from Seoul (Gangseo-Gu, South Earth). Summoned into his own game as a **1★ hero "Islat Han"** by a Level-999 bugged entity (the God of Death). As past master/Loki he confesses the appeal: "There's no way it wouldn't be fun to be able to control their lives." **Arc shift (Ch. 23):** "I think I'm slowly approaching the time where I'll no longer be at the center" — acknowledging his role is evolving from primary damage to leadership/strategy as Eolka's firepower outpaces his. **Master of Masters**: highest-ranking user in Pick Me Up; took 2 years to reach Floor 88 with dozens of party annihilations — "A Pick Me Up master grows through their failures."
- **Skills now include:** <Fire Resistance> (Ch. 24), <Projectile Defense> (Ch. 25), <Beginner Sword-Shield Technique> Lv.6 (Ch. 25).
- **Big reveal:** Han **is Loki** (Account #46631913, Rank 5, "Master of Masters"). Heroes are real humans from across ~1M worlds.
- **The Master:** **ANYTNG** — built Magic Hall on login (Ch. 24); challenged Floor 10 without waiting for Han's preferred week's preparation; watching fire-resistance training as entertainment.
- **Iselle:** Pixie lounge manager; Loki fangirl; reassured by Han: "I'll make sure you climb the tower."
- **Jenna Cirai:** First ally, calls Han "Oppa"; archery Lv.5; learning Switching at abnormal rate; assigned fire resistance training (pending); cheeks pinched again (Ch. 25).
- **Aaron:** Loyal disciple, calls Han "Hyung-nim"; basic spearmanship Lv.3; pain resistance training holed up in room; assigned fire resistance training (pending).
- **Edis Callen (3★):** Party 2 leader; cleared Floor 9 (muddy ground report — linked-quest clue); tasked with regrouping at centre if separated on Floor 10.
- **Usher Roderick (1★→Lv.3):** Porter; Roderick = family name; Party 2.
- **Roderick Sajan (3★):** Surname confirmed (Ch. 25); veteran city guard; Party 2.
- **Shurn (1★):** NEW (Ch. 25); joined Party 1 as fifth member; Lv.1, basic swordsmanship Lv.3; too inexperienced — Han told him to hide during combat.
- **Mehkin (1★):** NEW (Ch. 25); joined Party 2 as fifth member.
- **Eolka Rivel Strashur (3★):** MVP Floor 8; spell sequence "Ignite! Burn! Transcend!"; magic classes: Class 1 = ignition, Class 2 = burning/explosive, Class 3 = directing explosion upward; higher class = more mana. Han's directive: decrease strength, increase speed, reduce mana consumption — use class 1 for most situations. Custom magic clothing (family heirloom) increases fire magic strength. Training at magic hall.
- **Floor 10 (Ch. 25):** pre-ruined city from Floor 5 — "it hasn't been destroyed yet"; NPC present who can't see the heroes; tactical station mechanic (complicated for ANYTNG); 6 health potions (3 per party); mana potions for Eolka only; two parties of five each.

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
