# Workflow

> How work actually gets done in this repository, derived from `CONTRIBUTING.md` and observed history.

---

## Versioning Scheme (CONTRIBUTING.md)

| Version | Meaning |
|---|---|
| `0.0.x` | Documentation / early planning / UI prototype (current) |
| `1.0.0` | Unity C# implementation begins |
| `2.0.0` | Unreal C++ implementation begins |
| `x.1.0` | Major feature added (general, Unity, or Unreal) |

- The `1.0.0` / `2.0.0` split is **intentional** (two engine environments for future scope discussion). Not a conflict.

## Branching

| Branch | Purpose |
|---|---|
| `master` | General documentation, design docs, shared knowledge, UI prototype |
| `v1-unity` | Unity C# implementation |
| `v2-unreal` | Unreal C++ implementation |

All current work happens on `master`.

## Folder Conventions

| Folder | Content |
|---|---|
| `docs/` | General knowledge (engine-agnostic) |
| `v1/` | Unity C# specifics |
| `v2/` | Unreal C++ specifics |
| `interfaces/` | React+Vite+TypeScript UI prototype |
| `agents/opencode/` | OpenCode's own memory (secondary scope) |

## Standard Task Flow

1. **Verify current state:** `git status`, `git log --oneline -5`. Read relevant docs.
2. **Consult memory:** check `agents/opencode/` (this directory) for applicable rules/decisions.
3. **Check rules** in `RULES.md` and authoritative project files (`docs/decisions.md`, `CONTRIBUTING.md`).
4. **Make edits** following existing conventions. Use Edit for targeted changes, Write for new files.
5. **Verify** with grep/read as needed; ensure no contradictions with actual project.
6. **Commit only when asked.** One version bump = one commit, staged files only, message like `v0.0.NN: <summary>`.
7. **Update this memory** after substantial changes so it reflects reality.

## Content Update Pattern (for new chapters)

When a new manhwa chapter arrives (e.g. Chapter 9):

1. Add `docs/story/chapters/chapter-09.md` in `script-format.md` format.
2. **Run the cross-document audit** (RULES.md §6b / `docs/decisions.md`): search Character/, Psychology/, Systems/, Story docs for genuine dependencies; classify game/manhwa/shared canon; **report affected documents to the user and wait for approval before modifying them**.
3. After approval, apply approved updates:
   - `docs/story/character-tracker.md` (statuses, deaths)
   - `docs/story/timeline.md` (remember: time skips are normal)
   - `docs/story/player-progression.md` (per-chapter player/master growth)
   - `docs/characters/*.md` latest-state snapshots (named characters only — RULES.md §6c)
   - systems/psychology docs if mechanics/emotional beats change
4. Bump version and commit once.
5. **Final consistency check:** grep for stale "through Chapter N" / "Based on Chapters 1-N" ranges, old counts, and outdated statuses.

## interfaces/ Development Pattern

When working on the React UI prototype:

1. **Type check:** `npx tsc --noEmit` (must pass with 0 errors)
2. **Build:** `npx vite build` (must pass with 0 errors)
3. **File conventions:**
   - `export default function` for components (not `React.FC`)
   - Inline styles only (no CSS modules)
   - Props interfaces defined in the same file
   - Screen components in `screens/`, reusable components in `components/`
4. **After adding screens:** Update `ScreenId` in `types.ts`, update `SCREEN_ORDER` in `useFlowState.ts`, add switch case in `App.tsx`
5. **After adding state:** Update `useFlowState.ts` hook (state + returned functions)
6. **Before committing:** Verify `tsc --noEmit` and `vite build` both pass

## Formatting Conventions

- Markdown tables for structured data (stats, events, statuses).
- Blockquotes (`>`) for file-intro notes.
- `---` horizontal rules as section separators.
- Chapters use the strict script format (see `RULES.md` §3).

## Post-Change Verification

- Re-check file against the source material the user provides.
- Flag (don't silently "fix") author inconsistencies from the source.
- Confirm counts (alive/deceased) stay consistent in `character-tracker.md`.
- For `interfaces/`: run `tsc --noEmit` and `vite build` to verify no regressions.
