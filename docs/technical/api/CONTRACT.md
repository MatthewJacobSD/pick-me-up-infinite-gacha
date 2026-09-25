# Pick Me Up — API Contract

> For Unity (`v1/`) and Unreal (`v2/`) engine developers.
> Both engines connect to the **same backend**. Routes are engine-agnostic.

---

## Base URL

```
http://localhost:5137
```

---

## Health

| Method | Endpoint | Auth | Response |
|---|---|---|---|
| GET | `/health` | None | 200 if process is up |
| GET | `/ready` | None | 200 if Mongo + MySQL + Redis are reachable |

---

## Authentication

### Flow

```
1. Engine calls GET  /api/auth/login/{provider}
   → receives { redirectUrl }

2. Engine opens redirectUrl in browser
   → user authenticates with Google/Facebook
   → browser redirected to /api/auth/callback/{provider}

3. Engine polls POST /api/auth/consume-login-code
   → sends { loginCode } (from redirect URL)
   → receives { sessionId, accessToken, refreshToken }

4. Engine uses accessToken in Authorization header for all subsequent requests

5. When accessToken expires, POST /api/auth/refresh
   → sends { refreshToken }
   → receives { accessToken, refreshToken }
```

### JWT Token

```
Authorization: Bearer <accessToken>
```

- Access token expires per `Jwt:ExpiryMinutes` config (default 10 min)
- Refresh token expires per `Jwt:RefreshExpiryDays` config (default 7 days)
- Both are HMAC-SHA256 signed

---

## Error Format

All errors use RFC 9457 `application/problem+json`:

```json
{
  "type": "https://pickmeup/errors/validation",
  "title": "Bad Request",
  "status": 400,
  "detail": "Validation failed.",
  "code": "validation.failed"
}
```

### Error Types

| Type URI | HTTP | When |
|---|---|---|
| `https://pickmeup/errors/validation` | 400 | Invalid input |
| `https://pickmeup/errors/unauthorized` | 401 | Missing/invalid JWT |
| `https://pickmeup/errors/policy-denied` | 403 | Block or visibility rule |
| `https://pickmeup/errors/not-found` | 404 | Resource doesn't exist |
| `https://pickmeup/errors/conflict` | 409 | Duplicate request (social) |
| `https://pickmeup/errors/version-conflict` | 409 | Stale version (preferences) |

**Engine rule:** Branch on `type`, never on `detail` string.

---

## Rate Limiting

All authenticated endpoints are rate-limited to **100 requests per minute per user**.
Exceeding the limit returns `429 Too Many Requests`.

---

## Profile

| Method | Endpoint | Auth | Body | Response |
|---|---|---|---|---|
| GET | `/account/profile` | Yes | — | `{ username, avatar, version }` |
| PUT | `/account/profile/username` | Yes | `{ username, version }` | `{ username, version }` |
| PUT | `/account/profile/avatar` | Yes | `{ avatarUrl, avatarType, version }` | `{ avatar, version }` |

### Avatar Types

```
Default  — system placeholder (no upload)
Static   — uploaded image at fixed URL
Custom   — user-customisable (animated, layered, etc.)
```

---

## Account Settings

| Method | Endpoint | Auth | Body | Response |
|---|---|---|---|---|
| POST | `/account/settings/password` | Yes | `{ currentPassword, newPassword }` | 200 OK |

- `newPassword` must be ≥12 chars, mixed case, digit, symbol
- Returns 400 if current password is wrong or new password fails validation

---

## Preferences

Every domain follows the same pattern:

```
GET    /account/preferences/{slice}              →  { settings: {...}, version: N }
PUT    /account/preferences/{slice}              →  { settings: {...}, version: N+1 }
PATCH  /account/preferences/{slice}              →  { settings: {...}, version: N+1 }
POST   /account/preferences/{slice}/reset        →  { settings: {...}, version: N+1 }
GET    /account/preferences/{slice}/defaults     →  default settings (no auth)
```

PUT replaces the entire slice. PATCH updates only provided fields (nullable).
Both require `version` in the body. Stale version → 409.

### GET /account/preferences

Returns all slices + single version number.

```json
{
  "version": 42,
  "gameplay": { ... },
  "accessibility": { ... },
  "language": { ... },
  "notifications": { ... },
  "socialPreferences": { ... },
  "audio": { ... },
  "ui": { ... }
}
```

### PUT /account/preferences/{slice}

Request body = full domain DTO + `version` field.

### PATCH /account/preferences/{slice}

Request body = partial DTO (nullable fields) + `version` field. Only provided fields are updated.

### POST /account/preferences/{slice}/reset

Request body = `{ "version": N }`. Resets to default values.

### GET /account/preferences/{slice}/defaults

Returns the default settings for that slice. No authentication required.

### Slices

| Slice | Route | Key Fields |
|---|---|---|
| **Gameplay** | `/account/preferences/gameplay` | visibleActionBars (1-6), cameraSensitivity (0.1-10), fieldOfView (60-120), 16 booleans |
| **Accessibility** | `/account/preferences/accessibility` | colorblindMode (enum), subtitleSize (10-40), subtitleOpacity (0-1), textSize (10-40) |
| **Language** | `/account/preferences/language` | preferredLanguage (en/nl/fr/de/it/es/pt/pl/ru/ja/ko/zh) |
| **Notifications** | `/account/preferences/notifications` | 5 boolean toggles |
| **Social Preferences** | `/account/preferences/social` | friendRequests/messages/partyInvites/onlineStatus (Everyone/FriendsOnly/Nobody) |
| **Audio** | `/account/preferences/audio` | 5 volume sliders (0-1), 4 mute booleans |
| **UI** | `/account/preferences/ui` | uiScale (0.5-2), textSize/iconSize (Small/Medium/Large), 4 visibility booleans, 4 position enums, 2 layout enums |

---

## Social

All social endpoints require authentication.

### Friends

| Method | Route | Description |
|---|---|---|
| `GET` | `/account/social/friends` | List friend account IDs |
| `DELETE` | `/account/social/friends/{targetUserId}` | Remove friend |

### Friend Requests

| Method | Route | Description |
|---|---|---|
| `GET` | `/account/social/friends/requests` | List pending requests |
| `POST` | `/account/social/friends/requests` | Send request (body: `{ targetUserId }`) |
| `POST` | `/account/social/friends/requests/{senderId}/accept` | Accept request |
| `POST` | `/account/social/friends/requests/{senderId}/decline` | Decline request |
| `DELETE` | `/account/social/friends/requests/{receiverId}` | Cancel outgoing request |

### Blocks

| Method | Route | Description |
|---|---|---|
| `GET` | `/account/social/blocks` | List blocked account IDs |
| `POST` | `/account/social/blocks` | Block player (body: `{ targetUserId }`) — removes friendship, voids pending requests/invites |
| `DELETE` | `/account/social/blocks/{targetUserId}` | Unblock player |

### Party Invites

| Method | Route | Description |
|---|---|---|
| `GET` | `/account/social/party/invites` | List pending invites |
| `POST` | `/account/social/party/invites` | Send invite (body: `{ targetUserId }`) |
| `POST` | `/account/social/party/invites/{senderId}/accept` | Accept invite |
| `POST` | `/account/social/party/invites/{senderId}/decline` | Decline invite |
| `DELETE` | `/account/social/party/invites/{receiverId}` | Cancel outgoing invite |

### Social Rules

- **Block check:** Bidirectional. If A blocked B or B blocked A, social actions are rejected (403).
- **Visibility:** Checked via SocialPreferences. `Nobody` = no one can interact. `FriendsOnly` = only friends. `Everyone` = anyone.
- **Block removes friendship:** When you block someone, the friendship is deleted and all pending requests/invites between you are voided.
- **Pending requests expire after 7 days** (checked on read).

---

## Enums (string-serialized)

All enums serialize as strings, never integers.

```
SocialVisibility:  Everyone | FriendsOnly | Nobody
FriendRequestStatus: Pending | Accepted | Declined | Cancelled | Expired
ColorblindMode:    None | Protanopia | Deuteranopia | Tritanopia
UiTextSize:        Small | Medium | Large
UiIconSize:        Small | Medium | Large
HudPosition:       TopLeft | TopRight | BottomLeft | BottomRight | LeftSide | RightSide | Center
ActionBarLayout:   Horizontal | Vertical
InventoryLayout:   Grid | List
ChatLayout:        Compact | Expanded
AvatarType:        Default | Static | Custom
```

---

## Dates

All dates are ISO-8601 UTC:
```
2026-09-22T14:30:00Z
```

---

## Version Conflict (Optimistic Concurrency)

Every preferences PUT/PATCH returns a `version`. The next PUT/PATCH must include that same `version`.

```
Client A: GET → version 42
Client B: GET → version 42
Client A: PUT version=42 → success, version=43
Client B: PUT version=42 → 409 (stale)
```

Client B must re-GET to get the latest version, merge changes if needed, and retry.

---

## Quick Reference

```bash
# Health
curl http://localhost:5137/health
curl http://localhost:5137/ready

# Login
curl http://localhost:5137/api/auth/login/google

# Get all preferences
curl -H "Authorization: Bearer $TOKEN" http://localhost:5137/account/preferences

# Update audio (PUT)
curl -X PUT -H "Authorization: Bearer $TOKEN" -H "Content-Type: application/json" \
  -d '{"masterVolume":0.8,"version":42}' \
  http://localhost:5137/account/preferences/audio

# Partial update audio (PATCH)
curl -X PATCH -H "Authorization: Bearer $TOKEN" -H "Content-Type: application/json" \
  -d '{"musicVolume":0.5,"version":42}' \
  http://localhost:5137/account/preferences/audio

# Reset audio to defaults
curl -X POST -H "Authorization: Bearer $TOKEN" -H "Content-Type: application/json" \
  -d '{"version":42}' \
  http://localhost:5137/account/preferences/audio/reset

# Get audio defaults (no auth)
curl http://localhost:5137/account/preferences/audio/defaults

# Get profile
curl -H "Authorization: Bearer $TOKEN" http://localhost:5137/account/profile

# Change password
curl -X POST -H "Authorization: Bearer $TOKEN" -H "Content-Type: application/json" \
  -d '{"currentPassword":"old","newPassword":"NewP@ssw0rd!12"}' \
  http://localhost:5137/account/settings/password

# Send friend request
curl -X POST -H "Authorization: Bearer $TOKEN" -H "Content-Type: application/json" \
  -d '{"targetUserId":"uuid-here"}' \
  http://localhost:5137/account/social/friends/requests

# Block player
curl -X POST -H "Authorization: Bearer $TOKEN" -H "Content-Type: application/json" \
  -d '{"targetUserId":"uuid-here"}' \
  http://localhost:5137/account/social/blocks
```
