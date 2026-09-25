# 11 — Target folder structure (`PickMeUp.Api`)

Vertical slices. One `DependencyInjection.cs` per module instead of barrel files.

```
PickMeUp.Api/
  Program.cs
  Hosting/
    EnvLoader.cs
    ConfigurationExtensions.cs
    DependencyInjection.cs          # AddPickMeUpApi
  Common/
    CurrentUser.cs
    HttpProblemExtensions.cs
  Account/
    Authentication/
      OAuth/
      Session/
      DependencyInjection.cs
    Preferences/
      AccountPreferencesDocument.cs
      IAccountPreferencesRepository.cs
      AccountPreferencesRepository.cs
      AccountPreferencesController.cs
      DependencyInjection.cs
      Gameplay/
      Accessibility/
      Language/
      Notifications/
      Social/                       # visibility only
      Audio/
      Ui/
    Profile/                        # stubs until a later milestone
  Social/
    DependencyInjection.cs
    SocialController.cs
    SocialService.cs
    SocialPolicy.cs
    FriendRequests/
    Blocks/
    Parties/
    Persistence/
  Infrastructure/
    Mongo/
    Identity/                       # migrate DoNotTouchFolder here when touched
```

## Naming

- Rename `LanaguageSettingsDto.cs`
- Rename interface file to match `IAccountPreferencesRepository`
- Settings types stay `*Settings`; HTTP bodies stay `*Dto`

## Solution

Keep a single `PickMeUp.Api.slnx` until contracts are extracted. Do not split microservices.

## Empty folders

Do not add `DeviceSettings/` under the API.  
`AccountSettings/` stays empty or is removed until there is a real “account security / permissions” resource distinct from Identity and Preferences.
