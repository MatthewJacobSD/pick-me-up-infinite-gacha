# v1 — Unity C# Tasks

## Sprint 1 — Frontend Tasks

### Title Screen with Login Verification
- **Status:** Pending
- **Engine:** Unity (C#)
- **Scene:** `Assets/Scenes/TitleScreen.unity`

**Scope:**
- Display title screen UI canvas
- Check for stored token on `Start()`
- Valid token → tap → load Main Menu scene
- No token → reveal login buttons
- Use `SceneManager.LoadSceneAsync()` for transitions

**Unity Implementation Notes:**
- Use `PlayerPrefs` or JSON file for token persistence
- Implement `IAwaitable` for async scene loading
- CanvasScaler for responsive layout across platforms

---

### Main Game Menu
- **Status:** Pending
- **Engine:** Unity (C#)
- **Scene:** `Assets/Scenes/MainMenu.unity`

**Scope:**
- Button GameObjects: Play, Options, Exit
- `OnClick()` listeners for navigation
- Options panel (future expansion)
- Exit via `Application.Quit()`

**Unity Implementation Notes:**
- Use Unity UI Toolkit or uGUI Button components
- EventSystem for input handling
- ScriptableObject for button configuration

---

### Login Screen
- **Status:** Pending
- **Engine:** Unity (C#)
- **Scene:** `Assets/Scenes/LoginScreen.unity`

**Scope:**
- TMP_InputField for email/password
- Google login button (Unity Authentication SDK)
- Facebook-ready button (disabled, placeholder)
- POST to backend `/auth/login`
- Store token via `SecureStorage` or encrypted PlayerPrefs
- Redirect to Main Menu

**Unity Implementation Notes:**
- Use `UnityWebRequest` for HTTP calls
- JSON deserialization with `JsonUtility` or Newtonsoft
- Error handling with UI feedback (loading spinner, error toast)

---

### Front-End WebSocket Integration
- **Status:** Pending
- **Engine:** Unity (C#)
- **Script:** `Assets/Scripts/Server/WebSocketClient.cs`

**Scope:**
- Connect to `ws://server/game` after auth
- Send token in connection header or first message
- Listen for `CombatStatePacket` messages
- Send `InterventionRequest` on skill activation
- Handle disconnect → reconnect with exponential backoff

**Unity Implementation Notes:**
- Use `ClientWebSocket` from `System.Net.WebSockets`
- Run on background thread, marshal to main thread
- ScriptableObject for server URL configuration
- Events/Actions for message routing
