# Database Schema — MoebiusOrder

> Copy-paste ready. Based on current project state.

---

## MySQL — `MoebiusOrder` database

### Tables

```sql
-- ============================================================
-- ACCOUNTS (ASP.NET Identity)
-- ============================================================
CREATE TABLE IF NOT EXISTS accounts (
    Id                      CHAR(36)        NOT NULL PRIMARY KEY,
    Username                VARCHAR(100)    NOT NULL,
    Email                   VARCHAR(255)    NOT NULL,
    PasswordHashed          LONGTEXT        NULL,
    PhoneNumber             VARCHAR(30)     NULL,
    Status                  LONGTEXT        NOT NULL,
    Role                    LONGTEXT        NOT NULL,
    Coordinates_Longitude   DOUBLE          NOT NULL DEFAULT 0,
    Coordinates_Latitude    DOUBLE          NOT NULL DEFAULT 0,
    Coordinates_Altitude    DOUBLE          NOT NULL DEFAULT 0,
    Settings_Language       VARCHAR(30)     NOT NULL DEFAULT 'en',
    Settings_Theme          VARCHAR(30)     NOT NULL DEFAULT 'default',
    Settings_ReceiveNotifications TINYINT(1) NOT NULL DEFAULT 1,
    Settings_AllowFriendRequests  TINYINT(1) NOT NULL DEFAULT 1,
    Settings_ShowOnlineStatus     TINYINT(1) NOT NULL DEFAULT 1,
    Settings_TwoFactorEnabled     TINYINT(1) NOT NULL DEFAULT 0,
    LastLoginAt            DATETIME(6)     NULL,
    UpdatedAt              DATETIME(6)     NULL,
    CreatedAt              DATETIME(6)     NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
);

-- ============================================================
-- OAUTH IDENTITIES (linked providers)
-- ============================================================
CREATE TABLE IF NOT EXISTS oauthidentities (
    Id              CHAR(36)        NOT NULL PRIMARY KEY,
    UserId          CHAR(36)        NOT NULL,
    Provider        VARCHAR(50)     NOT NULL,
    ProviderKey     VARCHAR(255)    NOT NULL,
    CreatedAt       DATETIME(6)     NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
);

-- ============================================================
-- REFRESH TOKENS
-- ============================================================
CREATE TABLE IF NOT EXISTS refreshtokens (
    Id              CHAR(36)        NOT NULL PRIMARY KEY,
    UserId          CHAR(36)        NOT NULL,
    Token           VARCHAR(500)    NOT NULL,
    Expires         DATETIME(6)     NOT NULL,
    Created         DATETIME(6)     NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    CreatedByIp     VARCHAR(45)     NULL,
    Revoked         DATETIME(6)     NULL,
    RevokedByIp     VARCHAR(45)     NULL,
    ReplacedByToken VARCHAR(500)    NULL
);

-- ============================================================
-- BUILDINGS (future game feature)
-- ============================================================
CREATE TABLE IF NOT EXISTS buildings (
    Id              CHAR(36)        NOT NULL PRIMARY KEY,
    UserId          CHAR(36)        NOT NULL,
    BuildingType    VARCHAR(100)    NOT NULL,
    Level           INT             NOT NULL DEFAULT 1,
    CreatedAt       DATETIME(6)     NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
);

-- ============================================================
-- SECURITY KEYS (ASP.NET Data Protection)
-- ============================================================
CREATE TABLE IF NOT EXISTS securitykeys (
    Id              VARCHAR(255)    NOT NULL PRIMARY KEY,
    FriendlyName    VARCHAR(255)    NULL,
    Xml             LONGTEXT        NOT NULL
);
```

---

## MongoDB — `MoebiusOrder` database

### Collections

```javascript
// ============================================================
// accounts — Player accounts (primary: _id = Guid)
// ============================================================
// Collection: accounts
// Indexes: _id (unique, Guid Standard representation)
{
  "_id": UUID("..."),                    // Guid, internal (server-only)
  "PublicCode": "MOE-H5P35875",         // User-facing ID (Crockford Base32)
  "Email": "player@example.com",        // Normalized email
  "Username": "usr_dc0b2c20ebbcb640",    // Crypto-random (usr_ + 16 hex)
  "AvatarUrl": "/avatars/default.png"    // Avatar path
}

// ============================================================
// oauth_identities — Linked OAuth providers
// ============================================================
// Collection: oauth_identities
// Indexes: _id (unique, Guid Standard)
{
  "_id": UUID("..."),
  "AccountId": UUID("..."),              // FK → accounts._id
  "Provider": 1,                          // 1 = Google, 2 = Facebook
  "ExternalId": "102645547297300160067",  // Provider's user ID
  "Email": "player@example.com"          // Email from provider
}

// ============================================================
// account_preferences — Player preference slices
// ============================================================
// Collection: account_preferences
// Indexes: _id = AccountId (unique, Guid Standard)
{
  "_id": UUID("..."),                     // AccountId (Guid)
  "UserId": "guid-string",               // String representation
  "Version": 1,                           // Optimistic concurrency counter
  "Gameplay": {
    "VisibleActionBars": 1,
    "ShowCooldownNumbers": true,
    "ShowKeybindLabels": true,
    "LockActionBars": false,
    "ShowDamageNumbers": true,
    "ShowHealingNumbers": true,
    "ShowCriticalEffects": true,
    "ShowFloatingCombatText": true,
    "InvertYAxis": false,
    "InvertXAxis": false,
    "CameraSensitivity": 1.0,
    "FieldOfView": 75.0,
    "AutoLoot": true,
    "HighlightInteractables": true,
    "ShowNameplates": true,
    "ShowEnemyNameplates": true,
    "ShowFriendlyNameplates": true,
    "ShowPingIndicators": true,
    "AutoRunToggle": false,
    "ToggleSprint": false
  },
  "Accessibility": {
    "ColorBlindMode": 0,                 // 0=None, 1=Protanopia, 2=Deuteranopia, 3=Tritanopia
    "HighContrastMode": false,
    "SubtitlesEnabled": true,
    "SubtitleSize": 16,
    "SubtitleOpacity": 0.5,
    "SubtitleSpeakerNames": true,
    "SubtitleSoundEffects": true,
    "VisualAudioIndicators": false,
    "FootstepVisualization": false,
    "GunshotVisualization": false,
    "ReducedMotion": false,
    "DisableFlashingEffects": false,
    "SimplifiedUI": false,
    "TextSize": 16
  },
  "Language": {
    "PreferredLanguage": "en"
  },
  "Notifications": {
    "EventNotifications": true,
    "FriendRequestNotifications": true,
    "PartyInviteNotifications": true,
    "SystemAnnouncements": true,
    "RewardNotifications": true
  },
  "SocialPreferences": {
    "FriendRequests": 0,                 // 0=Everyone, 1=FriendsOnly, 2=Nobody
    "Messages": 1,
    "PartyInvites": 1,
    "OnlineStatus": 1
  },
  "Audio": {
    "MasterVolume": 1.0,
    "MusicVolume": 0.8,
    "MusicMuted": false,
    "SfxVolume": 0.8,
    "SfxMuted": false,
    "VoiceVolume": 0.8,
    "VoiceMuted": false,
    "AmbientVolume": 0.7,
    "AmbientMuted": false
  },
  "UiPreferences": {
    "UiScale": 1.0,
    "TextSize": 1,                        // 0=Small, 1=Medium, 2=Large
    "IconSize": 1,
    "ShowMinimap": true,
    "ShowChatWindow": true,
    "ShowQuestTracker": true,
    "ShowActionBars": true,
    "MinimapPosition": 1,                 // 0=TopLeft, 1=TopRight, etc.
    "ChatWindowPosition": 3,              // 3=BottomLeft
    "QuestTrackerPosition": 5,            // 5=RightSide
    "ActionBarLayout": 0,                 // 0=Horizontal, 1=Vertical
    "InventoryLayout": 0,                 // 0=Grid, 1=List
    "ChatLayout": 1                      // 0=Compact, 1=Expanded
  }
}

// ============================================================
// oauth_state — CSRF tokens (ephemeral, 5 min TTL)
// ============================================================
// Collection: oauth_state (managed by Redis, not MongoDB)
// Stored in Redis with key: oauth_state:{hex}
// TTL: 5 minutes
// Value: "valid"

// ============================================================
// sessions — User sessions (ephemeral, configurable TTL)
// ============================================================
// Collection: sessions (managed by Redis, not MongoDB)
// Stored in Redis with key: session:{sessionId}
// Value: AccountId (string)
// TTL: configurable (default 24 hours)

// ============================================================
// refresh_tokens — JWT refresh tokens (ephemeral, configurable TTL)
// ============================================================
// Collection: refresh_tokens (managed by Redis, not MongoDB)
// Stored in Redis with key: refresh:{token}
// Value: AccountId (string)
// TTL: configurable (default 7 days)

// ============================================================
// login_codes — One-time OAuth login codes (ephemeral, 2 min TTL)
// ============================================================
// Collection: login_codes (managed by Redis, not MongoDB)
// Stored in Redis with key: login_code:{code}
// Value: sessionId
// TTL: 2 minutes

// ============================================================
// friend_requests — Pending friend requests
// ============================================================
// Collection: friend_requests (managed by MongoDB)
// Status: Pending, Accepted, Declined, Cancelled, Expired

// ============================================================
// party_invites — Pending party invites
// ============================================================
// Collection: party_invites (managed by MongoDB)
// Status: Pending, Accepted, Declined, Cancelled, Expired
```

---

## Data Flow Summary

```
┌─────────────────────────────────────────────────────────────┐
│                    MySQL (Identity)                          │
│  accounts, oauthidentities, refreshtokens,                   │
│  buildings, securitykeys                                     │
└─────────────────────────────────────────────────────────────┘
                            │
                            │ JWT sub = Account.Id
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                 MongoDB (Game Data)                          │
│  accounts (MOE- code, email, username)                      │
│  oauth_identities (provider links)                          │
│  account_preferences (7 slices + version)                   │
│  friend_requests, party_invites                             │
└─────────────────────────────────────────────────────────────┘
                            │
                            │ Session/loginCode/refreshToken
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                   Redis (Ephemeral)                          │
│  oauth_state (CSRF, 5 min)                                  │
│  session:{id} (session data, configurable TTL)              │
│  refresh:{token} (refresh token, 7 days)                    │
│  login_code:{code} (one-time bridge, 2 min)                 │
└─────────────────────────────────────────────────────────────┘
```
