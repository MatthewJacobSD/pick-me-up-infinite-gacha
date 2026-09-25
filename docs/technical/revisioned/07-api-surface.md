# 07 — API surface (Unity contract)

Base: HTTPS. Auth: `Authorization: Bearer <access>`.  
JSON. ProblemDetails on errors.

## Identity

See [04](./04-identity-and-authentication.md).

## Preferences

All preference responses include `version`.

`GET /account/preferences` — full document for startup hydration.

Slice GET/PUT:

- `/account/preferences/gameplay`
- `/account/preferences/accessibility`
- `/account/preferences/language`
- `/account/preferences/notifications`
- `/account/preferences/social`
- `/account/preferences/audio`
- `/account/preferences/ui`

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
4. `GET /account/preferences`
5. Load social lists as needed
6. Validate + defaults
7. Resolve hybrid locally
8. Build effective runtime config
9. Start game systems

The server does steps 3–5. The client does 1–2 and 6–9.
