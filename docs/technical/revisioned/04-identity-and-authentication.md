# 04 — Identity and authentication

## Domain

Authentication answers **who the player is**. It is not a setting. QR login, session refresh, and logout stay out of the preferences API.

## Stores

| Concern | Store |
|---|---|
| Account (Id, PublicCode, Email, Username) | MongoDB |
| External provider links (Google, Facebook) | MongoDB (`oauth_identities`) |
| Access token | Stateless JWT |
| Refresh token, session, OAuth state | Redis |

## Account Model

Two IDs per account:
- **`Id`** (Guid) — internal, server-only, never exposed to clients
- **`PublicCode`** (MOE-XXXXXXXXXX) — user-facing, Crockford Base32, used for support/invites

Multiple OAuth providers can link to the same account (matched by email).

## Token contract

Access token claims:

- `sub` — Account `Id` as Guid string
- `accountId` — same value (explicit for clients)

API code uses `ICurrentUser.AccountId` from `sub` / `NameIdentifier` / `accountId`.

## HTTP endpoints

| Method | Path | Notes |
|---|---|---|
| GET | `/api/auth/login/{provider}` | 302 redirect to Google/Facebook |
| GET | `/api/auth/login-url/{provider}` | JSON redirect URL (for programmatic clients) |
| GET | `/api/auth/callback/{provider}` | CSRF state validated, code exchanged, account created/linked |
| POST | `/api/auth/consume-login-code` | Exchange one-time loginCode for tokens |
| POST | `/api/auth/refresh` | Rotate refresh token |
| POST | `/api/auth/logout` | Revoke session / refresh |

## OAuth providers (verified working in local development)

| Provider | Version | Callback URI | Redirect URI in Console |
|---|---|---|---|
| Google | default | `http://localhost:5137/api/auth/callback/google` | HTTP accepted for localhost |
| Facebook | v26.0 | `https://localhost:7111/api/auth/callback/facebook` | HTTPS required |

**Multi-provider support:** Same email links to same account. Both Google and Facebook can be linked to one account simultaneously.

## Post-login side effect

After OAuth login:

1. Find or create `Account` in MongoDB (matched by email)
2. Link `ExternalAccountLink` in `oauth_identities` (provider + externalId)
3. Create session in Redis
4. Generate short-lived `loginCode` (2 minutes)
5. In Development: return JSON with `publicCode`, `sessionId`, `identity`
6. In Production: redirect to `CLIENT_COMPLETE_URL` with `loginCode`

## Out of this milestone

- Multiple simultaneous device session policy beyond "refresh in Redis"
- Account linking UX polish (unlink provider)
- Email/password as a primary game login (Identity is present; OAuth is the intended path)
