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

- **Chapters 18-25**: transcribed (`docs/story/chapters/`). Ch.17's cliffhanger resolved: ANYTNG vented via mass synthesis (six slackers → Edis, Ch.18); two-party restructure by lots (Ch.19); Floor 7 cleared; Eolka & Roderick summoned (Ch.20); master's revenge Floor 8 (Ch.21-23, MVP Eolka); Magic Hall + fire resistance training (Ch.24); Floor 10 entered — pre-ruined city from Floor 5 (Ch.25). **Chapter 26**: not yet provided.
- **Standing rulings (user)**: roster screens show active heroes only; Sorial & Daniel remain Alive until source says otherwise; Usher Roderick ≠ Roderick Sajan (family name vs given name); game title always "Pick Me Up" (capitalised); "3-runa" = caster type, "3-circle" = mage rank (preserve verbatim); skill-name variants normalize to dominant form — `Basic Sword-Shield Technique` (not "Beginner"); Ch.24 brazier count = 10 seconds. See `docs/decisions.md`.
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
