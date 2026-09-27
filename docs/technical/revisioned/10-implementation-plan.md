# 10 — Implementation plan (backend milestone)

Focus: `PickMeUp.Api` only.

## Sequence

1. ~~Env loader + options + thin `Program.cs`~~ ✓
2. ~~Register Mongo, MySQL, Redis; fail if any missing~~ ✓
3. ~~`ICurrentUser` from JWT `sub` / `accountId`~~ ✓
4. ~~Preferences repository complete + unique index + versioned writes~~ ✓
5. ~~`GET /account/preferences` + FluentValidation wired~~ ✓
6. ~~Social policy + missing HTTP lifecycle + ProblemDetails~~ ✓
7. ~~Tests for validators, version conflict, policy~~ ✓
8. ~~`/health` `/ready` + Development CORS~~ ✓

## Current status

### Completed

- `Hosting/` — EnvLoader, ConfigurationExtensions, DependencyInjection (AddPickMeUpApi), DistributedFixedWindowRateLimiter, RateLimitOptions
- `Common/` — ICurrentUser, CurrentUser, DomainException hierarchy, ProblemDetails middleware
- Account Preferences: all 7 slices with Settings records, DTOs, PatchDts, Controllers (GET/PUT/PATCH/reset/defaults), Validators, Repository (upsert, versioned writes, shared ReplaceSliceAsync)
- FluentValidation wired via assembly scan
- Social module: controller, service, repository, policy, exception handler, index definitions, hosted service, block enforcement middleware, friend request lifecycle engine
- Health checks: `/health` and `/ready` endpoints wired in `Program.cs`
- Development CORS (`"dev"`) enabled in `Program.cs` under `IsDevelopment()`
- Profile module: controller, repository, DI, validator, document model
- Account Settings: controller, validators for password change and account deletion
- Build succeeds with 0 errors

### Remaining

- Profile controller tests (no tests exist yet for Profile)
- Integration tests (full HTTP pipeline) for any module
- OpenAPI/Swagger generation from real controllers

## Definition of done

- `dotnet build` succeeds ✓
- Process starts from `.env` + three backing services ✓
- Authenticated `GET /account/preferences` returns defaults for the token's account id ✓
- Invalid audio volume → 400 ✓
- Stale preference version → 409 ✓
- Target `FriendRequests = Nobody` → 403 on send ✓
- Block prevents invite ✓
- Accept request produces friendship both ways ✓
- No `User.Identity.Name` in Account/Social controllers ✓
- `Program.cs` has no store construction beyond `AddPickMeUpApi` ✓
- `/health` returns 200 when process is up ✓
- `/ready` verifies backing service connectivity ✓
- Profile GET/PUT/PATCH endpoints functional ✓

## After done

Unity (`v1`) auth + preferences client. DeviceSettings on the client only.
