# Context

> Story/world context and current-state snapshot, distilled from `docs/` so future sessions can resume without re-reading everything.

---

## Game Identity

- **Name:** Infinite Gacha TM ("Pick Me Up: Infinite Gacha")
- **Engine:** Unity (chosen so far); Unreal considered for future scope
- **Organisation:** Moebius Order DO
- **Platform:** PC, Mobile, Console (cross-platform), fully online
- **Mode:** Fully online; 24/7 server connectivity; client never trusted

## The Story (as of Ch. 10)

- **Protagonist:** Han Seojin, real player from Seoul (Gangseo-Gu, South Earth). Summoned into his own game as a **1★ hero "Islat Han"** by a Level-999 bugged entity (the God of Death).
- **Big reveal:** Han **is Loki** (Account #46631913, Rank 5, "Master of Masters"). Heroes are real humans from across ~1M worlds.
- **The Master:** **ANYTNG** — a reckless office worker playing the gacha on the other side; 65,000-won packages; Han is being managed by a "semi-newbie".
- **Iselle:** Pixie lounge manager; Loki fangirl; administrates the Sanctuary.
- **Jenna Cirai:** First ally, calls Han "Oppa", growing bond; archery Lv.5.
- **Aaron:** Loyal disciple, calls Han "Hyung-nim"; spear skill stuck at Lv.2.
- **Niflheimr:** The evil guild/order that killed Loki; their **Elite 5** (6★ Lv.99) are investigating Loki's return — Sirris dispatched to find Han, Yurneth investigating the master.

## Current State Snapshot (through Ch. 9)

- **Alive (8):** Islat Han, Jenna Cirai, Aaron, Enok (carpenter), Chloe (cook), Gide, Hansen, Dika (trainees).
- **Combat team:** 6 members; **Support team:** 2 members.
- **Facilities:** Lodging, Restaurant, Training Center, Equipment Workshop, Square, Weapons Storage + Blacksmith's Forge annex, etc.
- **Han:** Lv.7 (up from Lv.6 after first synthesis, Ch.5). Skills: Basic Sword-Shield Techniques (Lv.3), Pain Tolerance, Composure.
- **Synthesis used:** Toby → Han (Composure); Yelson/Elson → Jenna (Eagle's Eye); Shay (4★) sacrificed by ANYTNG; three inactive heroes incl. Dolf (Ch. 9).
- **Equipment production (Ch. 9):** second ten pull summoned a blacksmith + tanner; with carpenter Enok they mass-produce E-rank swords/shields for new heroes; higher quality for the main party.
- **Tower progress:** Floor 4 cleared (replayed in Ch. 9 as training: Gide & Hansen levelled up, MVP Jenna). **Floor 5 = survival battle ongoing through Ch. 10**: ~1,846 goblins, survive 10 minutes, historical survival rate 9%; Party 1 = Jenna, Islat Han, Aaron, Gide, Hansen in a funnel defence. Ch.10: rotation fighting, 5+ min survived; Gide bleeding + fear (-30%) + hand injured → **on the brink of death (cliffhanger)**; Hansen's sword went blunt (rotated out to repair); equipment limits exposed (normal iron swords blunt mid-battle).
- **Master behaviour change (Ch. 9):** logoff time between sessions has increased a lot.
- **Deaths:** ~13 combat + 4 synthesis sacrifices (Shay, Toby, Yelson, Dolf) + 1 unnamed hero lost in thinning. Total introduced: 30.

## Pending Story Beats

- Floor 5 survival mission outcome (Ch. 11 expected) — Gide's fate unknown.
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
