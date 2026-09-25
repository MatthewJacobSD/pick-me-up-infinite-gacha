# 07 — API surface (Unity contract)

Base: HTTPS. Auth: `Authorization: Bearer <access>`.
JSON. ProblemDetails on errors.

## Identity

See [04](./04-identity-and-authentication.md).

## Preferences

### GET /account/preferences

Returns the full document with all slices + version. Used at startup hydration.

### Per-slice endpoints

All responses include `version`.

| Method | Path | Body | Response |
|---|---|---|---|
| GET | `/account/preferences/{slice}` | — | `{ settings, version }` |
| PUT | `/account/preferences/{slice}` | Full DTO + `version` | `{ settings, version }` |
| PATCH | `/account/preferences/{slice}` | Partial DTO (nullable fields) + `version` | `{ settings, version }` |
| POST | `/account/preferences/{slice}/reset` | `{ version }` | `{ settings, version }` |
| GET | `/account/preferences/{slice}/defaults` | — | Default settings (no auth) |

**Slices:** `gameplay`, `accessibility`, `language`, `notifications`, `social`, `audio`, `ui`

### Version conflict

If the `version` in the request doesn't match the server's current version, the server returns **409** with ProblemDetails. The client must re-GET, merge changes, and retry.

### Error responses

All errors use `application/problem+json`:

| Status | `type` URI | When |
|---|---|---|
| 400 | `https://pickmeup/errors/validation` | Invalid input (FluentValidation) |
| 401 | `https://pickmeup/errors/unauthorized` | Missing/invalid JWT |
| 409 | `https://pickmeup/errors/version-conflict` | Stale version on PUT/PATCH/RESET |

## Social

See [06](./06-social.md).

## Not in this API

| Path idea | Why not |
|---|---|
| `/account/device` | Device is local |
| `/account/graphics` | Effective graphics are runtime |
| `/account/settings` | Forbidden mega-resource |
| `/account/redeem` | Other domain, later |

## Client startup sequence (when engines exist)

1. Load local device config
2. Detect hardware
3. Authenticate
4. `GET /account/preferences` — full hydration
5. Load social lists as needed
6. Validate + defaults
7. Resolve hybrid locally
8. Build effective runtime config
9. Start game systems

The server does steps 3–5. The client does 1–2 and 6–9.
