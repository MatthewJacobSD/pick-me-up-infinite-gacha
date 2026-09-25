# 10 — Implementation plan (backend milestone)

Focus: `PickMeUp.Api` only.

## Sequence

1. ~~Env loader + options + thin `Program.cs`~~ ✓
2. ~~Register Mongo, MySQL, Redis; fail if any missing~~ ✓
3. ~~`ICurrentUser` from JWT `sub` / `accountId`~~ ✓
4. ~~Preferences repository complete + unique index + versioned writes~~ ✓
5. ~~`GET /account/preferences` + FluentValidation wired~~ ✓
6. Social policy + missing HTTP lifecycle + ProblemDetails — **in progress**
7. Tests for validators, version conflict, policy — **partially done**
8. `/health` `/ready` + Development CORS — **not started**

## Current status

### Completed

- `Hosting/` — EnvLoader, ConfigurationExtensions, DependencyInjection (AddPickMeUpApi)
- `Common/` — ICurrentUser, CurrentUser, DomainException hierarchy, ProblemDetails middleware
- Account Preferences: all 7 slices with Settings records, DTOs, PatchDts, Controllers (GET/PUT/PATCH/reset/defaults), Validators, Repository (upsert, versioned writes, shared ReplaceSliceAsync)
- FluentValidation wired via assembly scan
- Build succeeds with 0 errors

### In progress

- Social module — graph structure, policy, controller exist but lifecycle endpoints being completed
- Social DI registration

### Not started

- `/health` and `/ready` endpoints
- Development CORS configuration
- MongoDB index creation hosted service (Social indexes exist, preferences index needed)
- Integration tests (full HTTP pipeline)
- OpenAPI/Swagger generation from real controllers

## Definition of done

- `dotnet build` succeeds ✓
- Process starts from `.env` + three backing services ✓
- Authenticated `GET /account/preferences` returns defaults for the token's account id ✓
- Invalid audio volume → 400 ✓
- Stale preference version → 409 ✓
- Target `FriendRequests = Nobody` → 403 on send — Social in progress
- Block prevents invite — Social in progress
- Accept request produces friendship both ways — Social in progress
- No `User.Identity.Name` in Account/Social controllers ✓
- `Program.cs` has no store construction beyond `AddPickMeUpApi` ✓

## After done

Unity (`v1`) auth + preferences client. DeviceSettings on the client only.
