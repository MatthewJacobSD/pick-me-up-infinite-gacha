# v1 — Unity C# Implementation

**Engine:** Unity
**Language:** C#
**Target Platforms:** PC, Mobile, Console

---

## Project Structure

```
v1/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/              ← Shared logic (formulas, enums, packets)
│   │   ├── Server/            ← API/WebSocket client logic
│   │   └── Data/              ← Local data models, save systems
│   ├── Prefabs/               ← Reusable game objects
│   ├── Scenes/                ← Game screens (Title, Login, Menu, Battle)
│   ├── Resources/             ← Runtime-loadable assets
│   └── Editor/                ← Custom Unity editor tools
├── Packages/                  ← Unity Package Manager manifest
└── ProjectSettings/           ← Unity project configuration
```

---

## Stack

| Component | Technology |
|---|---|
| Engine | Unity 2022 LTS (or latest LTS) |
| Language | C# |
| Networking | Unity WebGL / .NET WebSocket client |
| Real-Time | SignalR client (via Niflheim.Core DLL) |
| Data Storage | PlayerPrefs / JSON local cache |
| Build | Unity Build Pipeline |

---

## Shared Code (Niflheim.Core)

The `Niflheim.Core` DLL is shared between the Unity client and the C# server. This ensures:
- Client can predict damage (UI responsiveness)
- Server validates using the exact same math
- Enums, formulas, and packets stay in sync

### Key Shared Files

| File | Purpose |
|---|---|
| `Enums/ElementTypes.cs` | Fire, Water, Earth, etc. |
| `Enums/ClassTypes.cs` | Warrior, Healer, Rogue, etc. |
| `Formulas/DamageMath.cs` | Damage calculation curves |
| `Formulas/ExpCurves.cs` | Experience leveling formulas |
| `Packets/CombatStatePacket.cs` | Server→Client battle state |
| `Packets/InterventionRequest.cs` | Client→Server skill intervention |

---

## Unity-Specific Considerations

- **MonoBehaviour Lifecycle:** Game managers must handle `Awake`, `Start`, `OnDestroy` properly for connection cleanup
- **Threading:** Unity runs on main thread; use `async/await` with UniTask or Unity's `Awaitable` for non-blocking calls
- **Addressables:** Use Unity Addressables for asset loading instead of Resources for better memory management
- **Input System:** New Input System for cross-platform input handling
- **UI:** Unity UI Toolkit or uGUI for interface implementation

---

## Scene Flow

```
Title Screen → Login Screen → Main Menu → Game Screens
     ↓              ↓             ↓
  Auto-Auth    Email/Google    Play/Options/Exit
     ↓              ↓             ↓
  Main Menu    Store Token     Navigate
```

---

## Dependencies

- Niflheim.Core (shared DLL)
- Unity Input System
- Unity Addressables (recommended)
- UniTask or Unity Awaitable (async support)
