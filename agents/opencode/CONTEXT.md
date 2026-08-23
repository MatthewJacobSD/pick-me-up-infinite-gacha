# Context

> Story/world context and current-state snapshot, distilled from `docs/` so future sessions can resume without re-reading everything.

---

## Game Identity

- **Name:** Infinite Gacha TM ("Pick Me Up: Infinite Gacha")
- **Engine:** Unity (chosen so far); Unreal considered for future scope
- **Organisation:** Moebius Order DO
- **Platform:** PC, Mobile, Console (cross-platform), fully online
- **Mode:** Fully online; 24/7 server connectivity; client never trusted

## The Story (as of Ch. 16)

- **Protagonist:** Han Seojin, real player from Seoul (Gangseo-Gu, South Earth). Summoned into his own game as a **1★ hero "Islat Han"** by a Level-999 bugged entity (the God of Death).
- **Big reveal:** Han **is Loki** (Account #46631913, Rank 5, "Master of Masters"). Heroes are real humans from across ~1M worlds.
- **The Master:** **ANYTNG** — a reckless office worker playing the gacha on the other side; 65,000-won packages; Han is being managed by a "semi-newbie".
- **Iselle:** Pixie lounge manager; Loki fangirl; administrates the Sanctuary.
- **Jenna Cirai:** First ally, calls Han "Oppa", growing bond; archery Lv.5.
- **Aaron:** Loyal disciple, calls Han "Hyung-nim"; spear skill stuck at Lv.2.
- **Niflheimr:** The evil guild/order that killed Loki; their **Elite 5** (6★ Lv.99) are investigating Loki's return — Sirris dispatched to find Han, Yurneth investigating the master.

## Current State Snapshot (through Ch. 16)

- **Alive (14):** Han, Jenna, Aaron, Enok, Chloe, Dika, Louis, Joffrey, Owen + **Pulverizing Wolves (3★, Ch.15): Avant Dejacques (Party 5 leader), Edis Callen (Thief), Beignin, Jaken, Wave.** Plus Sorial & Daniel (1★, Ch.16) seized for synthesis — fate unresolved at the cliffhanger.
- **Tower progress:** **Floor 6 Explore cleared** — MVP Han, 5,000G. Stream hints point to a large goblin habitat; streams appearing earlier than Han expected. Daily dungeons running; Plants of Life gathering pending; Han's promotion materials acquired.
- **Ch.16 crisis:** ANYTNG ran another 10 consecutive summons and **synthesized immediately without examination** — Diman & Lexigel consumed (Jaken levelled up); Sorial & Daniel seized next. Han sent advice to the master through Iselle as a TIP notification; Jaken attacked him (blocked by Jenna's deliberate miss); Han exposed the Wolves as a **thief troupe** → hostility formed with the whole team; Iselle admitted hero-vs-hero fighting isn't strictly forbidden.
- **Cliffhanger:** **Open duel — Han vs Avant**, condition 'synthesizing' (winner consumes the loser); Avant accepted.
- **New mechanics:** Streams (linked quest chains from exploration hints, master-competence-gated); affinity link via consecutive summons (stronger in same party); summon-rank knowledge gap (3★ know synthesis innately; summoning memories suppressed); 3★ arrive with classes/skill kits; Open Duel system with agreed conditions; hostility notifications + tracked hero relationships; heroes can reach the master via TIP through Iselle; synthesis grants the consumer an immediate level-up.
- **Master spending:** 'newbie potent package' — 50,000 won → 2,500 gems + 50,000 gold.
- **Han:** Lv.9, near 1★ cap; Composure+Berserker bug insight stands. Dismissed Party 1 before the newcomers arrived; rebuffed slacker NPCs begging protection.
- **Jenna:** 'Hunter of the Forest' awakened; dreams of non-combat life; social bridge to Edis; first moral rebellion against synthesis (Ch.16).
- **Edis Callen:** ex-Pulverizing Wolves mercenary (Halsia); dagger-type Thief; skilled even among mercenaries — clear gap above Molmont (2★). Showed conscience during synthesis ("Avant, let's stop here for today") — fracture line inside the Wolves.
- **Standing facts:** Facilities include Lodging, Restaurant, Training Center, Equipment Workshop, Square, Weapons Storage + Blacksmith's Forge. Synthesis used: Toby → Han, Yelson → Jenna, Shay (4★) by ANYTNG, Dolf + 2 inactive, Diman + Lexigel (Ch.16, unexamined). Equipment production: blacksmith + tanner + Enok mass-produce E-rank gear. Awakening triggers: limit-tests (Han) or achievement (Jenna). ANYTNG logs in 3x/day; waiting room 3x dilation; battle stages real-time.
- **Deaths:** ~15 combat + 6 synthesis (incl. Diman, Lexigel) + 1 thinning. Total introduced: 42.

## Pending Story Beats

- Han vs Avant duel outcome — synthesis stake (Ch. 17).
- Sorial & Daniel's fate (Ch. 17).
- Large goblin habitat / next stream chain (Ch. 17+).
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
