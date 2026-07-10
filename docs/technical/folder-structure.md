# Folder Structure — Project Scaffolding

This is how the C# Solution should be to maintain sanity. At least my sanity, though if I am the one to be setting up the architecture, I may or may not stray away from this structure.

```
/Solution-Niflheim
|__ /Niflheim.Server (ASP.NET Core)
|   |__ /Controllers       (REST Endpoints: Auth, Store, Inventory)
|   |__ /Hubs              (SignalR: GameHub, ChatHub)
|   |__ /Services          (Business Logic)
|   |   |__ CombatService.cs
|   |   |__ GachaService.cs
|   |   |__ SanctuaryService.cs
|   |__ /Models            (DTOs - Data Transfer Objects)
|
|__ /Niflheim.Core (Shared Library - DLL to Unity)
|   |__ /Enums             (ElementTypes, ClassTypes)
|   |__ /Formulas          (DamageMath, ExpCurves)
|   |__ /Packets           (C# Classes defining network messages)
|
|__ /Niflheim.Data (Persistence)
    |__ /Interfaces        (IRepository)
    |__ /Implementations   (SqlRepository, MockRepository)
```

## Why Niflheim.Core?

We share the exact same C# code between the Server and the Unity Client. This ensures that the client can predict the damage (for UI responsiveness) but the server validates it using the exact same math.
