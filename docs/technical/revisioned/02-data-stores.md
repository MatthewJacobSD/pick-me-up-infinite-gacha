# 02 — Data stores

## Rule

**MySQL = definition + identity.**  
**MongoDB = player instance state that changes in play.**  
**Redis = ephemeral session and token state.**  
**Client disk = device and install configuration.**

“Fixed vs changing” is the informal version. Definitions can still be patched in a deploy. What matters is whether the row is a *catalog fact* or a *player instance*.

## Ownership table

| Data | Store | Syncs across devices | Notes |
|---|---|---|---|
| User account, email, roles | MySQL | Yes | ASP.NET Identity |
| Linked Google / Facebook ids | MySQL | Yes | External login keys |
| Username uniqueness | MySQL | Yes | Unique constraint |
| Language *catalog* (`en`, `ja`, …) | MySQL or shipped static | Yes | Validator list is temporary catalog |
| Hero / item / gacha *templates* | MySQL | Yes | Future |
| Account preferences document | MongoDB | Yes | All seven slices + `Version` |
| Friends, blocks, requests, party invites | MongoDB | Yes | Social graph |
| Roster instances, inventory stacks | MongoDB | Yes | Future gameplay |
| Refresh tokens, OAuth CSRF state, sessions | Redis | Session-scoped | TTL |
| Resolution, VSync, GPU, window mode | Client | No | Never on the account |
| Physical key mappings | Client | No | Logical actions may later live in prefs |

## One player id

```
MySQL ApplicationUser.Id  (Guid)
        │
        ▼
JWT sub + accountId  (same Guid string)
        │
        ├─► Mongo account_preferences.UserId
        └─► Mongo social documents keyed by the same string
```

Do not invent a second Mongo ObjectId as “the user.”

## Account folder ≠ MySQL

`Account/` contains both identity (SQL) and preferences (Mongo). Folder name is a domain, not a store.

## Transactions across stores

OAuth callback: commit MySQL user first, then upsert empty Mongo preferences. If Mongo fails, next authenticated GET creates the document. No distributed transaction in this milestone.

## Indexes (required before Unity)

Mongo:

- `account_preferences.userId` unique
- social: unique friendship pair, unique pending request pair, block pair

MySQL:

- Identity defaults
- unique normalized username when Profile is implemented

## Explicitly forbidden

- Device JSON on the account document
- Friend lists inside social *preference* documents
- Storing JWT refresh tokens in MySQL “because they belong to the account”
- Catalog tables in Mongo “because we might edit them often in admin”
