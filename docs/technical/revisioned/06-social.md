# 06 — Social

## Definition

Social is **relationships and pending actions between two players**. It is not a settings document.

```
Preferences (Account)     State (Social)
who may contact me    +   who I actually know / blocked
        \                   /
         SocialPolicy.Can*(actor, target)
```

## State machine (friend request)

`Pending → Accepted | Declined | Cancelled | Expired`

Accept writes a bidirectional friendship and closes the request.  
Block removes an existing friendship and forbids further requests, invites, and messages.

## Persistence (Mongo)

Own collections or one `social` document per user. Either is acceptable if:

- friendship is bidirectional in one write path
- pending request between a pair is unique
- block is directional and policy checks **both** directions

## Policy

`SocialPolicy` reads:

1. Block either way → deny
2. Target’s `SocialPreferences` for that action
3. `FriendsOnly` → `AreFriends`
4. `Nobody` → deny
5. `Everyone` → allow if not blocked

Used by send-request, party invite, and future messages. Not only by middleware.

## HTTP (target)

| Method | Path | Purpose |
|---|---|---|
| GET | `/account/social/friends` | List |
| DELETE | `/account/social/friends/{targetUserId}` | Remove |
| GET | `/account/social/friends/requests` | Pending in/out |
| POST | `/account/social/friends/requests` | Send |
| POST | `/account/social/friends/requests/{senderId}/accept` | Accept |
| POST | `/account/social/friends/requests/{senderId}/decline` | Decline |
| DELETE | `/account/social/friends/requests/{targetUserId}` | Cancel outbound |
| GET | `/account/social/blocks` | List |
| POST | `/account/social/blocks` | Block |
| DELETE | `/account/social/blocks/{targetUserId}` | Unblock |
| POST | `/account/social/party` | Invite |
| POST | `/account/social/party/{senderId}/accept` | Accept invite |
| POST | `/account/social/party/{senderId}/decline` | Decline |

Lifecycle engine already has accept/decline/cancel/expire. The controller must expose them. Expire via hosted service or expire-on-read.

## Errors

Domain denials are **403** or **409**, never unhandled 500. ProblemDetails code: `social.blocked`, `social.visibility`, `social.already_friends`, `social.no_pending`.

## Current gaps

- Controller missing accept/decline/cancel/list requests
- Policy ignores preference enums
- Middleware ignores JSON body targets
- Party invite is write-only
