# Standing Decisions & Conventions

> Authoritative record of confirmed decisions. When in doubt, defer to this file.

---

## Project Phase

- **Documentation-only.** No game code and no API contracts will be developed until a further announcement.
- Current versioning is strictly documentation-based (`0.0.x`).

---

## Versioning Intent (Not a Conflict)

- `1.0.0` = Unity path.
- `2.0.0` = Unreal path.
- The dual numbering is **intentional**. It keeps two separate engine environments for future scope discussion.
- Unity is the chosen engine so far; nothing has been implemented.

---

## Database (Confirmed)

- **MySQL** (account data) + **MongoDB** (gameplay data).
- These two are the current, confirmed choices. Other discussions may be added for future updates.

---

## Character Naming

- `Lyle` is correct. `Lyla` was a wording error and is not used.
- See `docs/technical/script-format.md` for the full spelling rule list.

---

## Game Title Capitalization

- The game's name is always written as **`Pick Me Up`** — three words, each capitalised. Never "pick me up", "Pick me up", or any other variation. Retroactively applied to Ch. 2-7 (v0.0.36).

---

## NPC Numbering

- NPC ids **start from 1 each chapter** and **reset each chapter** (no global counter, so no `NPC672920`).
- Unnamed NPCs share numeric ids (`NPC1`, `NPC11`, `NPC20`, ...).
- Once an NPC is given a name, they are referred to **by name only**, disregarding any previous numeric id.

---

## Story Conventions

- Repeated panels/lines across chapters are **intentional** (connections between chapters, e.g. the well-forged longsword). Do not treat them as errors to fix.
- The timeline is **not** 1 chapter = 1 day. Time skips between chapters are normal.
- **Roster screens show the active roster only** (confirmed Ch. 19 end screen). Absence from a roster screen does NOT mean death or departure — heroes stay at their last known status until the source explicitly states otherwise (user ruling after Ch. 19: Sorial & Daniel remain Alive; if a later chapter synthesizes or mentions them, the user will flag it).
- **Usher Roderick ≠ Roderick (3★)**: two distinct people (Ch. 20 source name collision). Usher Roderick (1★ porter, Ch. 19) uses "Roderick" as his **family name**; the Ch. 20 veteran city guard uses Roderick as his **given name**. User ruling — preserved as separate characters, never merged.

---

## Skill Name Capitalization

- Skill names always capitalize every word, including hyphenated segments: `Basic Swordsmanship`, `Low-Rank Archery`, `Basic Sword-Shield Technique`.
- Applies to transcripts and all documentation. Generic prose uses of words like "basic" are untouched.
- Retroactively applied to Ch. 3, 4, 17 stat panels/notifications (v0.0.33).

### Name Variants (v0.0.37)

- The source alternates between `Basic` (Ch. 4, 11, 17) and `Beginner` (Ch. 25, alongside the typo "sword-shied") for Han's shield skill. Per Stat Panel Discrepancies, normalized to **Basic Sword-Shield Technique** everywhere — transcripts included — as the dominant form across four chapters.
- Same ruling fixed the Ch. 24 fire-resistance count: it is a **10-second endurance count** at the brazier (escalating punctuation in source), not ten separate walks through fire.

---

## Mage Terminology (World-Building, Ch. 21)

- **3-runa** = a magic caster type designation (how mage classes are designated in-world; "support magic is magic that a 3-runa would learn"). Preserve verbatim in all documentation; do not normalize.
- **3-circle** = a mage rank measure (the scale of magic a mage can cast; Eolka's max is "third-circle fire magic"). Keep as transcribed.
- Both terms are intentional world-building elements from the manhwa source; future chapters may expand the classification further.

---

## Stat Panel Discrepancies

- When a later panel contradicts an earlier one with no in-story explanation, treat it as a source error and normalize to the consistent value.
- Example: Han's exp at Lv.9 was 54/70 (Ch. 13); the Ch. 17 pre-duel panel showed 11/70 — user confirmed it as a manhwa error; kept at 54/70.

---

## Progression Tracking (Two Viewpoints)

- `docs/characters/han.md` records Han's development **up to the latest chapter** (current-state snapshot).
- `docs/story/player-progression.md` tracks the player's/master's growth **per chapter** (chapter-by-chapter log).

---

## Cross-Document Audit Workflow (Standing Convention)

Story chapter updates require a dependency audit **before** propagating changes into other documents. The chapter document itself may be updated per the original request, but secondary documents are never modified silently.

Process:

1. **Story chapter update** follows `docs/technical/script-format.md`.
2. **Cross-document dependency search** across `docs/characters/`, `docs/psychology/`, `docs/systems/`, and Story docs (`timeline.md`, `player-progression.md`, `character-tracker.md`), plus any other doc referencing changed facts.
3. **Only genuine dependencies are flagged** — sharing a topic with the change is not enough; there must be a real consistency or dependency issue.
4. **Game vs Manhwa classification**: identify whether each affected item is game-specific, manhwa-specific, shared canon, or a deliberate adaptation/difference. Never overwrite one version using information from the other unless documentation establishes shared canon.
5. **Report before modifying**: for each affected document provide (a) file, (b) section, (c) story change creating the dependency, (d) why existing info is now inconsistent, (e) proposed update. Then **stop and wait for approval**.
6. **Apply approved secondary updates only.**
7. **Final consistency check** after approved updates: search for stale chapter ranges ("through Chapter N", "Based on Chapters 1-N"), outdated counts, names, and statuses across all affected files.
8. **Preserve established characterization and canon distinctions**; verify interpretation of ambiguous lines against surrounding context rather than assuming from isolated lines.

---

## Game vs Manhwa Documentation (Monitoring Rule)

- Continuously classify information as **game-specific**, **manhwa-specific**, **shared canon**, or an **adaptation/difference** when documenting.
- Keep the current folder structure while it remains small and manageable — no restructuring purely for the sake of separation.
- As the project grows and separation becomes difficult to manage, **proactively recommend and implement** a clearer Game/Manhwa split — do not wait until the structure is chaotic.
- When an update introduces a distinction between Game and Manhwa versions, reflect that distinction explicitly in the relevant documents.

---

## Psychology vs Characters Documentation Methodology

- **`docs/psychology/` = Character + Behaviour records.** Where a character has meaningful behavioural, emotional, psychological, or developmental evidence, maintain a dedicated record tracking that development **across chapters** (`han_behaviour.md` is the model). Purpose: behavioural progression and psychological continuity — not restating the character sheet.
- **`docs/characters/` = named characters only.** Dedicated character records are created/maintained exclusively for characters **explicitly named in the story**. Do not create permanent records for generic/unnamed NPCs or background/temporary NPCs merely because they appear in a chapter.
- **Promotion rule:** if an unnamed NPC later gains a name, becomes established/recurring, or is otherwise promoted into a defined character, create their record at that point and **backfill** relevant information from earlier appearances where useful.
- Keep these two concepts separate during audits so the project does not accumulate dozens of unnecessary NPC documents.
