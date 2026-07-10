# v2 — Unreal Engine C++ Implementation

**Engine:** Unreal Engine 5
**Language:** C++
**Target Platforms:** PC, Console (initial), Mobile (future)

---

## Project Structure

```
v2/
├── Source/
│   └── PickMeUp/
│       ├── Core/              ← Shared logic (formulas, enums, packets)
│       ├── Server/            ← API/WebSocket client, HTTP modules
│       └── Data/              ← Data models, save game systems
├── Config/                    ← DefaultEngine.ini, DefaultGame.ini, etc.
├── Content/                   ← Blueprints, materials, maps, assets
└── Plugins/                   ← Custom and marketplace plugins
```

---

## Stack

| Component | Technology |
|---|---|
| Engine | Unreal Engine 5.3+ |
| Language | C++ (with Blueprints for visual scripting) |
| Networking | WebSocket via `FWebSocketsModule` or `IWebSocket` |
| Real-Time | SignalR C++ client (custom or library) |
| Data Storage | UE5 SaveGame system / JSON serialization |
| Build | Unreal Build Tool (UBT) |

---

## Shared Code (Niflheim.Core Equivalent)

Unreal does not share the C# DLL. Instead, we port the shared logic to C++ and ensure parity with the server's C# implementation.

### Key Shared Files (C++ Port)

| File | Purpose |
|---|---|
| `Core/ElementTypes.h` | UENUM — Fire, Water, Earth, etc. |
| `Core/ClassTypes.h` | UENUM — Warrior, Healer, Rogue, etc. |
| `Core/DamageMath.h` | Damage calculation functions |
| `Core/ExpCurves.h` | Experience leveling curves |
| `Core/CombatStatePacket.h` | Struct for server→client battle state |
| `Core/InterventionRequest.h` | Struct for client→server skill intervention |

---

## Unreal-Specific Considerations

- **GameInstance:** Use `UGameInstance` for persistent data (auth token, connection state)
- **GameMode/GameState:** Handle server-authoritative state with `AGameModeBase` and `AGameStateBase`
- **Widgets:** UMG (Unreal Motion Graphics) for UI — `UUserWidget` subclasses
- **Input:** Enhanced Input System (`UInputAction`, `UInputMappingContext`)
- **Networking:** UE5 has built-in replication; for WebSocket we use `IWebSocket` from `WebSockets` module
- **Async:** Use `FAsyncTask`, `FRunnable`, or coroutines (`UE::Tasks`) for non-blocking operations
- **Memory:** UE5 handles memory via garbage collection; avoid raw `new`/`delete`

---

## Scene (Level) Flow

```
Title Level → Login Level → Main Menu Level → Game Levels
     ↓             ↓              ↓
  Auto-Auth   Email/Google   Play/Options/Exit
     ↓             ↓              ↓
  Main Menu   Store Token    Navigate
```

---

## Dependencies

- UE5 Core (Engine, CoreUObject, UMG, InputSystem)
- WebSockets plugin (`WebSockets`)
- HTTP module (`HTTP`)
- Json module (`Json`, `JsonUtilities`)
- Niflheim.Core (C++ port — maintained in sync with server C#)
