# Context

> Story/world context and current-state snapshot, distilled from `docs/` so future sessions can resume without re-reading everything.

---

## Game Identity

- **Name:** Infinite Gacha TM ("Pick Me Up: Infinite Gacha")
- **Engine:** Unity (chosen so far); Unreal considered for future scope
- **Organisation:** Moebius Order DO
- **Platform:** PC, Mobile, Console (cross-platform), fully online
- **Mode:** Fully online; 24/7 server connectivity; client never trusted

## The Story (as of Ch. 14)

- **Protagonist:** Han Seojin, real player from Seoul (Gangseo-Gu, South Earth). Summoned into his own game as a **1★ hero "Islat Han"** by a Level-999 bugged entity (the God of Death).
- **Big reveal:** Han **is Loki** (Account #46631913, Rank 5, "Master of Masters"). Heroes are real humans from across ~1M worlds.
- **The Master:** **ANYTNG** — a reckless office worker playing the gacha on the other side; 65,000-won packages; Han is being managed by a "semi-newbie".
- **Iselle:** Pixie lounge manager; Loki fangirl; administrates the Sanctuary.
- **Jenna Cirai:** First ally, calls Han "Oppa", growing bond; archery Lv.5.
- **Aaron:** Loyal disciple, calls Han "Hyung-nim"; spear skill stuck at Lv.2.
- **Niflheimr:** The evil guild/order that killed Loki; their **Elite 5** (6★ Lv.99) are investigating Loki's return — Sirris dispatched to find Han, Yurneth investigating the master.

## Current State Snapshot (through Ch. 14)

- **Alive (9):** Islat Han, Jenna Cirai, Aaron, Enok (carpenter), Chloe (cook), Dika (trainee) + Louis, Joffrey, Owen (Party 4 gatherers).
- **Parties:** Party 1 = Han, Jenna, Aaron + **Dika joined for Floor 6**; Party 4 (Ch. 13) = Han, Jenna, Louis, Joffrey, Owen — daily dungeon gatherers.
- **Han:** **Lv.9 (exp 54/70)** — near the 1★ cap of 10; promotion materials now acquired (attribute stone Ch.13 + wind/water elemental stones D- Ch.14). Holds Composure + Berserker together — a bug tied to being half master, half hero.
- **Jenna:** awakened **'Hunter of the Forest'** dressing the Forest Queen (Ch.14) — awakening can trigger on achievement, not only mortal danger. Dreams of a non-combat paradise without a master; Han hinted at making a spot for her.
- **Tower progress:** Floor 5 cleared; daily dungeons running (wind stone from Forest Queen, water stone from plateau wolf hunt); **Plants of Life** spotted (healing potion ingredient). **Floor 6 in progress: Explore mission** ("Investigate the Unfamiliar location!") — goblin scouts (Lv.8 x3) with blowing horn revealed a ~100-strong village trap; ambush set, cliffhanger mid-engagement.
- **Master pattern:** ANYTNG logs in three times a day (morning/afternoon/night); waiting room dilation 3x; battle stages real-time.
- **Standing facts:** Facilities include Lodging, Restaurant, Training Center, Equipment Workshop, Square, Weapons Storage + Blacksmith's Forge. Synthesis used: Toby → Han, Yelson → Jenna, Shay (4★) by ANYTNG, Dolf + 2 inactive (Ch.9). Equipment production: blacksmith + tanner + Enok mass-produce E-rank gear; main party gets better. Awakening can trigger on limit-tests (Han, Ch.11-12) or achievement (Jenna, Ch.14).
- **Deaths (unchanged):** ~15 combat + 4 synthesis + 1 thinning. Total introduced: 33.

## Pending Story Beats

- Floor 6 Explore outcome — goblin village ambush in progress (Ch. 15 expected).
- Han's promotion to 2★ (materials acquired, imminent).
- Plants of Life gathering run.
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
