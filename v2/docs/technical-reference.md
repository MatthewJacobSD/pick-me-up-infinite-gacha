# v2 — Unreal Engine C++ Technical Reference

## Networking Layer

### WebSocket Connection

```cpp
// Conceptual structure — Source/PickMeUp/Server/WebSocketClient.h
#pragma once

#include "CoreMinimal.h"
#include "WebSocketsModule.h"
#include "IWebSocket.h"

UCLASS()
class PICKMEUP_API UWebSocketClient : public UObject
{
    GENERATED_BODY()

public:
    void Connect(const FString& Token);
    void Send(const FString& JsonPacket);
    void Disconnect();

    DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FOnMessageReceived, const FString&, Json);
    UPROPERTY(BlueprintAssignable)
    FOnMessageReceived OnMessageReceived;

private:
    TSharedPtr<IWebSocket> WebSocket;
    FString ServerUrl;

    void OnConnected();
    void OnConnectionError(const FString& Error);
    void OnMessage(const FString& Message);
    void OnClosed(int32 StatusCode, const FString& Reason, bool bWasClean);
};
```

```cpp
// WebSocketClient.cpp
void UWebSocketClient::Connect(const FString& Token)
{
    IWebSocketModule& Module = FModuleManager::Get().LoadModuleChecked<IWebSocketModule>("WebSockets");
    WebSocket = Module.CreateWebSocket(ServerUrl);

    WebSocket->OnConnected().AddUObject(this, &UWebSocketClient::OnConnected);
    WebSocket->OnConnectionError().AddUObject(this, &UWebSocketClient::OnConnectionError);
    WebSocket->OnMessage().AddUObject(this, &UWebSocketClient::OnMessage);
    WebSocket->OnClosed().AddUObject(this, &UWebSocketClient::OnClosed);

    WebSocket->AddHeader("Authorization", FString::Printf(TEXT("Bearer %s"), *Token));
    WebSocket->Connect();
}

void UWebSocketClient::OnMessage(const FString& Message)
{
    // Parse JSON and broadcast to listeners
    OnMessageReceived.Broadcast(Message);
}
```

### HTTP Client

```cpp
// Conceptual structure — Source/PickMeUp/Server/ApiClient.h
#pragma once

#include "CoreMinimal.h"
#include "Http.h"

UCLASS()
class PICKMEUP_API UApiClient : public UObject
{
    GENERATED_BODY()

public:
    void Post(const FString& Endpoint, const FString& JsonBody,
              TFunction<void(const FString& Response)> OnSuccess,
              TFunction<void(int32 Code, const FString& Error)> OnError);

private:
    FString BaseUrl;
    FString Token;

    void HandleResponse(FHttpRequestPtr Request, FHttpResponsePtr Response, bool bConnected);
};
```

```cpp
// ApiClient.cpp
void UApiClient::Post(const FString& Endpoint, const FString& JsonBody,
                      TFunction<void(const FString&)> OnSuccess,
                      TFunction<void(int32, const FString&)> OnError)
{
    TSharedRef<IHttpRequest, ESPMode::ThreadSafe> Request = FHttpModule::Get().CreateRequest();
    Request->SetURL(BaseUrl + Endpoint);
    Request->SetVerb("POST");
    Request->SetHeader("Content-Type", "application/json");
    Request->SetHeader("Authorization", FString::Printf(TEXT("Bearer %s"), *Token));
    Request->SetContentAsString(JsonBody);

    Request->OnProcessRequestComplete().BindLambda(
        [OnSuccess, OnError](FHttpRequestPtr Req, FHttpResponsePtr Resp, bool bOk)
        {
            if (bOk && Resp.IsValid())
            {
                OnSuccess(Resp->GetContentAsString());
            }
            else
            {
                OnError(Resp.IsValid() ? Resp->GetResponseCode() : -1,
                        Resp.IsValid() ? Resp->GetContentAsString() : "Connection failed");
            }
        });

    Request->ProcessRequest();
}
```

## Data Persistence

### Token Storage (SaveGame)

```cpp
// Source/PickMeUp/Data/TokenSaveGame.h
#pragma once

#include "CoreMinimal.h"
#include "GameFramework/SaveGame.h"
#include "TokenSaveGame.generated.h"

UCLASS()
class PICKMEUP_API UTokenSaveGame : public USaveGame
{
    GENERATED_BODY()

public:
    UPROPERTY()
    FString AuthToken;

    UPROPERTY()
    FDateTime SavedAt;

    static const FString SlotName;
};
```

```cpp
// TokenSaveGame.cpp
const FString UTokenSaveGame::SlotName = "AuthTokenSlot";

void SaveToken(const FString& Token)
{
    UTokenSaveGame* SaveGameInstance = Cast<UTokenSaveGame>(
        UGameplayStatics::CreateSaveGameObject(UTokenSaveGame::StaticClass()));

    SaveGameInstance->AuthToken = Token;
    SaveGameInstance->SavedAt = FDateTime::Now();

    UGameplayStatics::SaveGameToSlot(SaveGameInstance, UTokenSaveGame::SlotName, 0);
}

FString LoadToken()
{
    if (UGameplayStatics::DoesSaveGameExist(UTokenSaveGame::SlotName, 0))
    {
        UTokenSaveGame* SaveGameInstance = Cast<UTokenSaveGame>(
            UGameplayStatics::LoadGameFromSlot(UTokenSaveGame::SlotName, 0));
        return SaveGameInstance->AuthToken;
    }
    return "";
}
```

## Level Management

### Level Names (Constants)

```cpp
// Source/PickMeUp/Core/LevelNames.h
#pragma once

#include "CoreMinimal.h"

namespace LevelNames
{
    const FString Title = TEXT("/Game/Levels/TitleScreen");
    const FString Login = TEXT("/Game/Levels/LoginScreen");
    const FString MainMenu = TEXT("/Game/Levels/MainMenu");
    const FString Battle = TEXT("/Game/Levels/BattleScene");
}
```

## Platform-Specific Notes

| Platform | Build Target | Notes |
|---|---|---|
| PC | Windows/Linux | Full input, largest screen, primary dev target |
| Console | PS5/Xbox/Switch | Gamepad navigation, certification, platform SDKs |
| Mobile | Android/iOS | Touch input, smaller UI, deferred to Phase 2 |

## Build Configuration

### Required UE5 Plugins

| Plugin | Purpose |
|---|---|
| WebSockets | WebSocket client for real-time communication |
| HTTP | REST API calls |
| Json / JsonUtilities | JSON parsing and serialization |
| OnlineSubsystem | Platform authentication (Google, Facebook) |
| EnhancedInput | Modern input handling |

### Build.cs Dependencies

```csharp
// Source/PickMeUp/PickMeUp.Build.cs
PublicDependencyModuleNames.AddRange(new string[]
{
    "Core",
    "CoreUObject",
    "Engine",
    "UMG",
    "InputCore",
    "WebSockets",
    "HTTP",
    "Json",
    "JsonUtilities",
    "OnlineSubsystem",
    "EnhancedInput"
});
```
