# AGENTS.md

> This file is auto-loaded into OpenCode's context at session start. It routes every future session to the persistent memory system.

## First Action — Load Project Memory

Before doing any substantial work in this repository, read:

```
agents/opencode/OPENCODE.md
```

That directory (`agents/opencode/`) is OpenCode's persistent development memory: rules, architecture understanding, workflow, decisions, and story context. Start at `OPENCODE.md` (the index) and read the files relevant to your task.

## Critical Rules (Quick Reference)

1. **The project is the source of truth.** `agents/opencode/` is memory, not authority. Verify notes against the actual repo before acting.
2. **Documentation-only project.** No game code or API contracts until the user explicitly announces development. The `interfaces/` folder is a self-contained UI prototyping experiment — isolated from the project structure; do not mix it with project docs.
3. **One version bump = one commit** on `master` (see `CONTRIBUTING.md`). Commit only when asked.
4. **Story format** follows `docs/technical/script-format.md`. Standing conventions live in `docs/decisions.md`.
5. **After substantial changes**, update `agents/opencode/` so memory reflects reality.

## Open Questions / Awaiting User Input

- **Chapter 12**: not yet provided. Floor 5 survival outcome pending (Ch.11 cliffhanger: Gide and Hansen dead; Han, Jenna, Aaron cornered at Game time 00:32).
- **Task definitions**: backend/frontend/design tasks are all "Pending" with no definition of done; code development has not been announced.
- **`docs/licenses/`**: empty placeholder folder.

## Repository Map

| Path | Content |
|---|---|
| `docs/` | Engine-agnostic project documentation (story, characters, systems, technical, tasks) |
| `interfaces/` | Self-contained React UI prototype (isolated experiment) |
| `v1/` | Unity C# specifics |
| `v2/` | Unreal C++ specifics |
| `agents/opencode/` | OpenCode persistent memory (secondary scope) |
