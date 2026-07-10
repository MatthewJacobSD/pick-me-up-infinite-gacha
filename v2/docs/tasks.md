# v2 — Unreal Engine C++ Tasks

## Sprint 1 — Frontend Tasks

### Title Screen with Login Verification
- **Status:** Pending
- **Engine:** Unreal Engine 5 (C++)
- **Level:** `Content/Levels/TitleScreen.umap`
- **Widget:** `WBP_TitleScreen`

**Scope:**
- Display title screen UMG widget
- Check for stored token in `UGameInstance` on `BeginPlay()`
- Valid token → tap/click → load Main Menu level
- No token → reveal login buttons with animation
- Use `UGameplayStatics::OpenLevel()` for transitions

**Unreal Implementation Notes:**
- Use `USaveGame` subclass for token persistence
- `FStreamableManager` for async asset loading
- `UCanvasPanel` with `USizeBox` for responsive layout

---

### Main Game Menu
- **Status:** Pending
- **Engine:** Unreal Engine 5 (C++)
- **Level:** `Content/Levels/MainMenu.umap`
- **Widget:** `WBP_MainMenu`

**Scope:**
- UMG Button widgets: Play, Options, Exit
- `OnClicked` delegates for navigation
- Options panel (future expansion)
- Exit via `UKismetSystemLibrary::QuitGame()`

**Unreal Implementation Notes:**
- Use `UButton` components with `FOnClicked` delegates
- `APlayerController` for input mode switching
- Data Asset for button configuration

---

### Login Screen
- **Status:** Pending
- **Engine:** Unreal Engine 5 (C++)
- **Level:** `Content/Levels/LoginScreen.umap`
- **Widget:** `WBP_Login`

**Scope:**
- `UEditableTextBox` for email/password
- Google login button (Online Subsystem)
- Facebook-ready button (disabled, placeholder)
- POST to backend `/auth/login` via `FHttpModule`
- Store token via `USaveGame` subclass
- Redirect to Main Menu

**Unreal Implementation Notes:**
- Use `IHttpRequest` for HTTP calls
- `FJsonObjectConverter` for JSON serialization
- Error handling with UI feedback (progress bar, error dialog)

---

### Front-End WebSocket Integration
- **Status:** Pending
- **Engine:** Unreal Engine 5 (C++)
- **Class:** `Source/PickMeUp/Server/WebSocketClient.h`

**Scope:**
- Connect to `ws://server/game` after auth
- Send token in connection header or first message
- Listen for `FCombatStatePacket` messages
- Send `FInterventionRequest` on skill activation
- Handle disconnect → reconnect with exponential backoff

**Unreal Implementation Notes:**
- Use `IWebSocket` from `WebSockets` module
- Run on game thread via `FFunctionGraphTickedDelegate`
- `UCurveFloat` for backoff timing
- `FOnWebSocketConnectionComplete` for connection events
