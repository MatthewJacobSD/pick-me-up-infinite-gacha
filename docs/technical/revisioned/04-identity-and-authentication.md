# 04 — Identity and authentication

## Domain

Authentication answers **who the player is**. It is not a setting. QR login, session refresh, and logout stay out of the preferences API.

## Stores

| Concern | Store |
|---|---|
| User row, password/lockout, roles | MySQL Identity |
| External provider key | MySQL |
| Access token | Stateless JWT |
| Refresh token, session, OAuth state | Redis |

## Token contract (required)

Access token claims:

- `sub` — `ApplicationUser.Id` as Guid string
- `accountId` — same value (explicit for clients)

Do **not** use `User.Identity.Name` unless a `Name` claim is also issued. Controllers today do that; it is a defect.

API code uses `ICurrentUser.AccountId` from `sub` / `NameIdentifier` / `accountId`.

## HTTP (existing shape, keep)

| Method | Path | Notes |
|---|---|---|
| GET | `/api/auth/login/{provider}` | Google, Facebook |
| GET | `/api/auth/callback/{provider}` | CSRF state one-time in Redis |
| POST | `/api/auth/consume-login-code` | Exchange one-time code |
| POST | `/api/auth/refresh` | Rotate refresh token |
| POST | `/api/auth/logout` | Revoke session / refresh |

## Post-login side effect

After a MySQL user exists:

1. Upsert Mongo `account_preferences` with defaults if missing.
2. Do not create social documents until the first social action (or create an empty graph doc — pick one and document it in Social). Preferred: create empty social doc lazily.

## Out of this milestone

- Multiple simultaneous device session policy beyond “refresh in Redis”
- Account linking UX polish
- Email/password as a primary game login (Identity is present; OAuth is the intended path)
