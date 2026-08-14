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

## Progression Tracking (Two Viewpoints)

- `docs/characters/han.md` records Han's development **up to the latest chapter** (current-state snapshot).
- `docs/story/player-progression.md` tracks the player's/master's growth **per chapter** (chapter-by-chapter log).
