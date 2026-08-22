# Context

> Story/world context and current-state snapshot, distilled from `docs/` so future sessions can resume without re-reading everything.

---

## Game Identity

- **Name:** Infinite Gacha TM ("Pick Me Up: Infinite Gacha")
- **Engine:** Unity (chosen so far); Unreal considered for future scope
- **Organisation:** Moebius Order DO
- **Platform:** PC, Mobile, Console (cross-platform), fully online
- **Mode:** Fully online; 24/7 server connectivity; client never trusted

## The Story (as of Ch. 13)

- **Protagonist:** Han Seojin, real player from Seoul (Gangseo-Gu, South Earth). Summoned into his own game as a **1★ hero "Islat Han"** by a Level-999 bugged entity (the God of Death).
- **Big reveal:** Han **is Loki** (Account #46631913, Rank 5, "Master of Masters"). Heroes are real humans from across ~1M worlds.
- **The Master:** **ANYTNG** — a reckless office worker playing the gacha on the other side; 65,000-won packages; Han is being managed by a "semi-newbie".
- **Iselle:** Pixie lounge manager; Loki fangirl; administrates the Sanctuary.
- **Jenna Cirai:** First ally, calls Han "Oppa", growing bond; archery Lv.5.
- **Aaron:** Loyal disciple, calls Han "Hyung-nim"; spear skill stuck at Lv.2.
- **Niflheimr:** The evil guild/order that killed Loki; their **Elite 5** (6★ Lv.99) are investigating Loki's return — Sirris dispatched to find Han, Yurneth investigating the master.

## Current State Snapshot (through Ch. 13)

- **Alive (9):** Islat Han, Jenna Cirai, Aaron, Enok (carpenter), Chloe (cook), Dika (trainee) + newly named in Ch. 13: Louis, Joffrey, Owen.
- **Combat team:** 4 members (Jenna, Han, Aaron, Dika); **Support team:** 2 members; **Party 4** (Ch. 13): Han, Jenna, Louis, Joffrey, Owen — daily dungeon gatherers.
- **Facilities:** Lodging, Restaurant, Training Center, Equipment Workshop, Square, Weapons Storage + Blacksmith's Forge annex, etc.
- **Han:** **Lv.9 (exp 54/70)** — near the 1★ cap of 10, promotion to 2★ imminent. Stats: STR 23/23, INT 11/11, HP 21/21, AGI 21/21. Skills: Basic Swordsmanship (Lv.5), Pain Tolerance (Lv.2), Composure (Lv.3), Berserker (Lv.1); Quick Movements absent from the Ch.13 stat block. Holds Composure + Berserker together — a bug tied to being half master, half hero. Growth cost fixed at 5 = normal 3★ rate.
- **Jenna:** gained an unnamed skill from the Floor 5 battle (Ch. 13); butchers hunted game (her father was a hunter).
- **Aaron:** no new skills despite hard training (Ch. 13).
- **Synthesis used:** Toby → Han (Composure); Yelson/Elson → Jenna (Eagle's Eye); Shay (4★) sacrificed by ANYTNG; three inactive heroes incl. Dolf (Ch. 9).
- **Equipment production (Ch. 9):** second ten pull summoned a blacksmith + tanner; with carpenter Enok they mass-produce E-rank swords/shields for new heroes; higher quality for the main party.
- **Tower progress:** **Floor 5 survival mission CLEARED (Ch. 9-12)** — ~1,846 goblins, 10 minutes survived. Gide and Hansen died. **Daily dungeon unlocked and first-run (Ch. 13):** weekly rotation (Mon-Tue Isralta Mine / Wed-Thu Kendert Forest / Fri-Sat Sinmiel Plateau / Sun all open); ~11-hour runs with auto-return; gathering mechanics (Trash(F) discard, Branch x100 → Lumber via carpenter's shop, dimension window storage). Rare monster **Queen of the Forest** drops a low-rank attribute stone (promotion material) — fight pending at chapter end.
- **Master pattern deduced (Ch. 13):** ANYTNG logs in three times a day (morning/afternoon/night); waiting room dilation is 3x while battle stages run real-time.
- **Awakening mechanic revealed (Ch.11-12):** limit-testing situations make heroes' souls awaken — skills jump levels mid-battle; at the edge of death Han awakened Berserker.
- **Deaths:** ~15 combat (incl. Gide, Hansen) + 4 synthesis sacrifices (Shay, Toby, Yelson, Dolf) + 1 unnamed hero lost in thinning. Total introduced: 33.

## Pending Story Beats

- Queen of the Forest fight outcome + first promotion material (Ch. 14 expected).
- Han's promotion to 2★ (imminent — needs attribute stone).
- Sirris arrives (Ch. 10+).
- Yurneth finds the master (Ch. 10+).
- Han vs Niflheimr (Ch. 15+).

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
