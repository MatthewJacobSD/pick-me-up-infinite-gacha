# OpenCode — Persistent Development Memory

> **Purpose:** This directory is OpenCode's working memory for the repository. It records rules, architecture understanding, workflow, decisions, and context so future sessions can continue safely without reconstructing everything from scratch.
>
> **The project is the source of truth. This directory is the memory that helps you understand and safely work on that source of truth.** The actual project state always takes priority over anything written here.

---

## How to Use This Directory

1. **Before substantial work:** Inspect `agents/opencode/` as contextual memory.
2. **Always verify** important information against the actual repository before acting on it — the project may have evolved past these notes.
3. **After substantial changes:** Update the relevant file(s) here so this memory continues to represent reality.
4. **Never treat this directory as authority to override the project.** If these notes conflict with the actual project, the project wins, and these notes are outdated and must be fixed.

## Scope Boundary

- **Primary scope (source of truth):** everything outside `agents/` — actual project docs (`docs/`, `v1/`, `v2/`), `CONTRIBUTING.md`, git history.
- **Secondary scope (this memory):** `agents/opencode/` — OpenCode's own operating knowledge only. Do not place generated project documentation here merely for convenience.

## File Map

| File | Content |
|---|---|
| [`OPENCODE.md`](OPENCODE.md) | This index; how to use the memory system |
| [`RULES.md`](RULES.md) | Every rule/constraint governing work in this repo |
| [`ARCHITECTURE.md`](ARCHITECTURE.md) | Understanding of the project architecture |
| [`WORKFLOW.md`](WORKFLOW.md) | How work actually gets done (versioning, commits, format) |
| [`DECISIONS.md`](DECISIONS.md) | Decisions made during development and their rationale |
| [`CONTEXT.md`](CONTEXT.md) | Story/world context and current-state snapshot |

## Maintenance Policy

- Update when a significant architectural, workflow, or decision change occurs.
- Do **not** record every minor code/format change.
- If any entry becomes outdated, fix it here rather than letting the project be forced to conform to stale notes.
