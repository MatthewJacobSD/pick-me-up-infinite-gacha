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

Document types:
- `SocialDocument` — per-user friends list, blocks list
- `FriendRequestDocument` — pending/accepted/declined/cancelled/expired requests
- `PartyInviteDocument` — pending/accepted/declined/cancelled/expired invites

## Policy

`SocialPolicy` reads:

1. Block either way → deny
2. Target's `SocialPreferences` for that action
3. `FriendsOnly` → `AreFriends`
4. `Nobody` → deny
5. `Everyone` → allow if not blocked

Used by send-request, party invite, and future messages. Not only by middleware.

## HTTP

| Method | Path | Purpose |
|---|---|---|
| GET | `/account/social/friends` | List friend account IDs |
| DELETE | `/account/social/friends/{targetUserId}` | Remove friend |
| GET | `/account/social/friends/requests` | Pending in/out requests |
| POST | `/account/social/friends/requests` | Send friend request |
| POST | `/account/social/friends/requests/{senderId}/accept` | Accept request |
| POST | `/account/social/friends/requests/{senderId}/decline` | Decline request |
| DELETE | `/account/social/friends/requests/{receiverId}` | Cancel outbound request |
| GET | `/account/social/blocks` | List blocked account IDs |
| POST | `/account/social/blocks` | Block player |
| DELETE | `/account/social/blocks/{targetUserId}` | Unblock player |
| POST | `/account/social/party/invites` | Send party invite |
| POST | `/account/social/party/invites/{senderId}/accept` | Accept invite |
| POST | `/account/social/party/invites/{senderId}/decline` | Decline invite |
| DELETE | `/account/social/party/invites/{receiverId}` | Cancel outbound invite |
| GET | `/account/social/party/invites` | List pending party invites |

All endpoints use `ICurrentUser.AccountId` for identity (no `User.Identity.Name`).

## Errors

Domain denials are **403** or **409**, never unhandled 500. ProblemDetails code: `social.blocked`, `social.visibility`, `social.already_friends`, `social.no_pending`.

## Implementation status

The controller is complete with all endpoints (friends, requests, blocks, party invites). Supporting infrastructure:

- `SocialPolicy` — reads block state + target's `SocialPreferences` to gate actions
- `SocialExceptionHandler` — translates domain exceptions into ProblemDetails responses
- `SocialIndexDefinitions` — defines MongoDB indexes for social collections (friend requests, party invites, blocks)
- `SocialIndexHostedService` — ensures indexes are created at startup
- `BlockEnforcementMiddleware` — checks block state before social actions
- `IFriendRequestLifecycleEngine` — state machine for friend request and party invite transitions
- `SocialVisibilityQuery` interface with `PreferencesSocialVisibilityQuery` — bridges Social to Account preferences

## Current gaps

- Party invite expiry-on-read (7-day TTL) may need a hosted service for background cleanup
- No integration tests for the full HTTP pipeline yet
- No Profile controller tests
