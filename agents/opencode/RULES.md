# Rules

> Current rules and constraints governing how to operate in this repository. Project state is authoritative; these notes are the remembered operating rules.

---

## 1. Project Nature

- **Documentation repository with UI prototype.** The repo contains documentation (`docs/`, `v1/`, `v2/`) and a React UI prototype (`interfaces/`).
- **`interfaces/` is a prototype only** — not production game logic. It simulates the startup/login/tutorial flow for UI testing.
- **Do not modify `v1/`, `v2/`, `docs/`** from `interfaces/` work — these are documentation-only.
- **Do not create API contracts** until the user announces it — they are deferred along with backend code.

## 2. Commit / Versioning

- One version bump = one commit on `master`.
- Version bump goes to the next `0.0.x` for documentation work.
- Write concise commit messages in the style of the existing history, e.g. `v0.0.22: Add standing decisions doc, record confirmed conventions`.
- Commit only when the user asks. Stage only intended files.

## 3. Story / Script Format (from `docs/technical/script-format.md`)

- **Dialogue:** `Name: text` — one sentence per line; repeat `Name:` for consecutive lines.
- **Narration:** `Name - narrating: text`.
- **Internal thought:** `*in his mind*`.
- **System messages:** `Notification:` and `TIPS:`.
- **Sounds** (attacks, etc.) are written in CAPS.
- Preserve the user's line arrangement. Do not merge lines.
- Chapters live in `docs/story/chapters/chapter-NN.md`.

### Consistent Spelling (per script-format.md + decisions.md)

| Correct | Not |
|---|---|
| `Islat Han` | Islan Han |
| `Iselle` | Islette |
| `Niflheimr` | Nilflheim / Niflheirm |
| `ANYTNG` | Anytng |
| `Molmont` | Mormont |
| `Lyle` | Lyla |

- `Lyle` is correct; `Lyla` was a wording error and is not used.
- Be aware the manhwa source itself contains author inconsistencies between chapters (e.g. name changes). The user transcribes the manhwa as-is; when a change appears it may simply reflect the source. Do not "fix" the source into uniformity unless the user asks.

## 4. NPC Numbering (from `docs/decisions.md`)

- NPC ids **start from 1 each chapter** and **reset each chapter** (no global counter → never `NPC672920`).
- Unnamed NPCs share numeric ids (`NPC1`, `NPC11`, `NPC20`, …).
- Once an NPC is given a name, refer to them **by name only**, disregarding any previous numeric id.

## 5. Timeline Conventions

- The timeline is **not** 1 chapter = 1 day. Time skips between chapters are normal.
- Do not assume a chapter maps to a fixed amount of in-world time.

## 6. Repeated Content

- Repeated panels/lines across chapters are **intentional** (connections between chapters, e.g. the well-forged longsword). Do not treat them as errors to remove.

## 7. Architecture "Must" Rules (from `docs/technical/`)

- **Gacha must be an atomic REST transaction** (`server-modules.md`).
- **Combat must be server-paced/simulated** using the "Tactical Frame" pattern with `CombatStatePacket` + `InterventionRequest` via SignalR.
- **Soft-delete only** for heroes (`UPDATE ... SET IsDeleted = 1`, never `DELETE`).
- **Auth:** JWT, duplicate-login check (disconnect old socket), heartbeat boot on connection drop.
- **No background timers** in the Sanctuary — use timestamp math `(CurrentTime - LastCollectionTime) * ProductionRate`.
- **Shared `Niflheim.Core`** DLL so server and client use the exact same math.
- **Credentials via environment variables**, never committed.

## 8. interfaces/ Prototype Rules

- `interfaces/` is a standalone Vite project — no monorepo, no shared configs.
- Use `export default function` pattern (not `React.FC`).
- All styling is inline styles — no CSS modules, no Tailwind.
- Gothic dark fantasy aesthetic: Cinzel fonts, SVG frames, purple accent palette.
- No real OAuth, no real payments, no real backend — all mock/prototype.
- State managed via custom `useFlowState` hook — no Redux/Zustand.
- `lockRef` guard on transitions prevents duplicate screen changes.
- Build must pass `tsc --noEmit` and `vite build` with 0 errors before committing.

## 9. General Working Behavior

- Follow existing conventions (file layout, markdown style, naming).
- No comments in code unless asked.
- Keep responses concise.
- Never commit secrets.
- Only commit when explicitly asked.
