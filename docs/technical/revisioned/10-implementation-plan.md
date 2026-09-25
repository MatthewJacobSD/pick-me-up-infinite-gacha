# 10 — Implementation plan (backend milestone)

Focus: `PickMeUp.Api` only.

## Sequence

1. Env loader + options + thin `Program.cs`
2. Register Mongo, MySQL, Redis; fail if any missing
3. `ICurrentUser` from JWT `sub` / `accountId`
4. Preferences repository complete + unique index + versioned writes
5. `GET /account/preferences` + FluentValidation wired
6. Social policy + missing HTTP lifecycle + ProblemDetails
7. Tests for validators, version conflict, policy
8. `/health` `/ready` + Development CORS

## Definition of done

- `dotnet build` succeeds
- Process starts from `.env` + three backing services
- Authenticated `GET /account/preferences` returns defaults for the token’s account id
- Invalid audio volume → 400
- Stale preference version → 409
- Target `FriendRequests = Nobody` → 403 on send
- Block prevents invite
- Accept request produces friendship both ways
- No `User.Identity.Name` in Account/Social controllers
- `Program.cs` has no store construction beyond `AddPickMeUpApi`

## Not done when

Folders exist, validators exist but are unregistered, or the architecture status table says complete.

## After done

Unity (`v1`) auth + preferences client. DeviceSettings on the client only.
