# Context

> Story/world context and current-state snapshot, distilled from `docs/` so future sessions can resume without re-reading everything.

---

## Game Identity

- **Name:** Infinite Gacha TM ("Pick Me Up: Infinite Gacha")
- **Engine:** Unity (chosen so far); Unreal considered for future scope
- **Organisation:** Moebius Order DO
- **Platform:** PC, Mobile, Console (cross-platform), fully online
- **Mode:** Fully online; 24/7 server connectivity; client never trusted

## The Story (as of Ch. 17)

- **Protagonist:** Han Seojin, real player from Seoul (Gangseo-Gu, South Earth). Summoned into his own game as a **1★ hero "Islat Han"** by a Level-999 bugged entity (the God of Death).
- **Big reveal:** Han **is Loki** (Account #46631913, Rank 5, "Master of Masters"). Heroes are real humans from across ~1M worlds.
- **The Master:** **ANYTNG** — a reckless office worker playing the gacha on the other side; 65,000-won packages; Han is being managed by a "semi-newbie" who approved a lethal duel without understanding it.
- **Iselle:** Pixie lounge manager; Loki fangirl; administrates the Sanctuary; openly calls Han "Loki" and seeks his praise after her Ch. 17 self-defence kills.
- **Jenna Cirai:** First ally, calls Han "Oppa", growing bond; archery Lv.5.
- **Aaron:** Loyal disciple, calls Han "Hyung-nim"; spear skill stuck at Lv.2.
- **Niflheimr:** The evil guild/order that killed Loki; their **Elite 5** (6★ Lv.99) are investigating Loki's return — Sirris dispatched to find Han, Yurneth investigating the master.

## Current State Snapshot (through Ch. 17)

- **Alive (12):** Han, Jenna, Aaron, Enok, Chloe, Dika, Louis, Joffrey, Owen + **Edis Callen** (defected from Wolves mid-duel — stayed only for her father's request) + **Sorial & Daniel** (saved: Jenna returned them to the accommodations before the duel).
- **Pulverizing Wolves destroyed (Ch. 17):** Avant consumed by Han via duel synthesis condition; Jaken/Wave/Beignin died by system sudden death ("suicide due to stress") when Iselle's self-defence punished their interference; party disbanded when Edis quit.
- **Han:** **Lv.10 — MAX for 1★**, promotion pending (materials acquired Ch. 14). Won the Open Duel vs Avant in Berserk mode without unsheathing his sword; 'Basic Sword-Shield Technique' + 'Pain Tolerance' levelled up; total stats close to a 3★. Berserk modifiers: +5 STR/HP/AGI over cap, −10 INT; headache side effect — avoid outside real fights.
- **Ch.16 crisis (resolved):** ANYTNG synthesized new pulls without examination (Diman & Lexigel consumed); Han exposed the Wolves as a thief troupe and staked himself in an Open Duel.
- **Duel system mechanics:** master Yes/No approval gate; conditions bind (surrender ≠ escape from consumption); no cancellation once allowed; interference forbidden — enforced lethally by Manager Self-Defence; winner gains level + skill-ups.
- **Promotion rules expanded:** promotion stone OR same-star hero at certain level; heroes promotable up to 7 stars; max-level prompt at Lv.10 for 1★.
- **Tower progress:** Floor 6 cleared (MVP Han); large goblin habitat stream chain pending; Plants of Life gathering pending; daily dungeons running.
- **New mechanics (cumulative):** Streams; affinity link; summon-rank knowledge gap; Open Duel; hostility notifications + tracked relationships; TIP-via-Iselle channel; synthesis grants immediate level-up; party disbanding on member exit; Sudden Death cause-of-death logging.
- **Master spending:** 'newbie potent package' — 50,000 won → 2,500 gems + 50,000 gold.
- **Jenna:** 'Hunter of the Forest' awakened; dreams of non-combat life; first moral rebellion vs synthesis (Ch. 16); trusted with saving the seized heroes (Ch. 17).
- **Standing facts:** Facilities include Lodging, Restaurant, Training Center, Equipment Workshop, Square, Weapons Storage + Blacksmith's Forge. Synthesis used: Toby → Han, Yelson → Jenna, Shay (4★) by ANYTNG, Dolf + 2 inactive, Diman + Lexigel (unexamined), Avant (duel condition). Awakening triggers: limit-tests (Han) or achievement (Jenna). ANYTNG logs in 3x/day; waiting room 3x dilation; battle stages real-time.
- **Deaths:** 15 combat + 7 synthesis (incl. Avant) + 3 sudden death (Wolves trio) + 1 thinning. Total introduced: 42.
- **Data note:** Han's Lv.9 exp normalized to 54/70 (Ch. 17 panel showed 11/70 — user confirmed manhwa error).

## Pending Story Beats

- ANYTNG's reaction to losing all five Wolves + Han's taunt ("Try synthesizing me if you dare") (Ch. 18).
- Han's promotion to 2★ (Lv.10 max, materials ready — imminent).
- Edis integration / Party 5 rebuild ("We happen to need one person").
- Large goblin habitat / next stream chain (Ch. 17+).
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
