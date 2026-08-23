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

## NPC Numbering

- NPC ids **start from 1 each chapter** and **reset each chapter** (no global counter, so no `NPC672920`).
- Unnamed NPCs share numeric ids (`NPC1`, `NPC11`, `NPC20`, ...).
- Once an NPC is given a name, they are referred to **by name only**, disregarding any previous numeric id.

---

## Story Conventions

- Repeated panels/lines across chapters are **intentional** (connections between chapters, e.g. the well-forged longsword). Do not treat them as errors to fix.
- The timeline is **not** 1 chapter = 1 day. Time skips between chapters are normal.

---

## Skill Name Capitalization

- Skill names always capitalize every word, including hyphenated segments: `Basic Swordsmanship`, `Low-Rank Archery`, `Basic Sword-Shield Technique`.
- Applies to transcripts and all documentation. Generic prose uses of words like "basic" are untouched.
- Retroactively applied to Ch. 3, 4, 17 stat panels/notifications (v0.0.33).

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
