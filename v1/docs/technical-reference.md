# v1 — Unity C# Technical Reference

## Networking Layer

### WebSocket Connection

```csharp
// Conceptual structure — Assets/Scripts/Server/WebSocketClient.cs
public class WebSocketClient : MonoBehaviour
{
    private ClientWebSocket _socket;
    private string _serverUrl;
    private string _token;

    public async Task Connect(string token)
    {
        _token = token;
        _socket = new ClientWebSocket();
        _socket.Options.SetRequestHeader("Authorization", $"Bearer {_token}");
        await _socket.ConnectAsync(new Uri(_serverUrl), CancellationToken.None);
    }

    public async Task Send<T>(T packet)
    {
        var json = JsonUtility.ToJson(packet);
        var bytes = Encoding.UTF8.GetBytes(json);
        await _socket.SendAsync(new ArraySegment<byte>(bytes),
            WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async Task<T> Receive<T>()
    {
        var buffer = new byte[4096];
        var result = await _socket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);
        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
        return JsonUtility.FromJson<T>(json);
    }
}
```

### HTTP Client

```csharp
// Conceptual structure — Assets/Scripts/Server/HttpClient.cs
public class ApiClient : MonoBehaviour
{
    private string _baseUrl;
    private string _token;

    public async Task<T> Post<T>(string endpoint, object body)
    {
        var json = JsonUtility.ToJson(body);
        var request = new UnityWebRequest($"{_baseUrl}{endpoint}", "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", $"Bearer {_token}");
        request.SetRequestHeader("Content-Type", "application/json");

        await request.SendWebRequest();
        return JsonUtility.FromJson<T>(request.downloadHandler.text);
    }
}
```

## Data Persistence

### Token Storage

```csharp
// Secure token storage concept
public static class TokenStore
{
    private const string TokenKey = "auth_token";

    public static void Save(string token)
    {
        PlayerPrefs.SetString(TokenKey, token);
        PlayerPrefs.Save();
    }

    public static string Load()
    {
        return PlayerPrefs.GetString(TokenKey, "");
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteKey(TokenKey);
    }

    public static bool HasValidToken()
    {
        return !string.IsNullOrEmpty(Load());
    }
}
```

## Scene Management

### Scene Names (Constants)

```csharp
// Assets/Scripts/Core/SceneNames.cs
public static class SceneNames
{
    public const string Title = "TitleScreen";
    public const string Login = "LoginScreen";
    public const string MainMenu = "MainMenu";
    public const string Battle = "BattleScene";
}
```

## Platform-Specific Notes

| Platform | Build Target | Notes |
|---|---|---|
| PC | Standalone (Windows/Mac/Linux) | Full input, largest screen |
| Mobile | Android/iOS | Touch input, smaller UI, battery considerations |
| Console | Switch/PS5/Xbox | Gamepad navigation, certification requirements |

## Unity Package Dependencies

```json
{
  "dependencies": {
    "com.unity.inputsystem": "1.7.0",
    "com.unity.addressables": "1.21.0",
    "com.unity.textmeshpro": "3.0.6",
    "com.unity.nuget.newtonsoft-json": "3.2.1"
  }
}
```
