# 11 — Target folder structure (`PickMeUp.Api`)

Vertical slices. One `DependencyInjection.cs` per module instead of barrel files.

```
PickMeUp.Api/
  Program.cs                            # thin composition root
  Hosting/
    EnvLoader.cs                        # .env loading with priority precedence
    ConfigurationExtensions.cs          # typed options + AddDotNetEnv
    DependencyInjection.cs              # AddPickMeUpApi composition root
  Common/
    Authentication/
      ICurrentUser.cs                   # account identity interface
      CurrentUser.cs                    # JWT claim reader
    Errors/
      DomainException.cs                # base domain exception
      ValidationException.cs            # validation error collection
      VersionConflictException.cs       # 409 stale version
      NotFoundException.cs              # 404 not found
      ProblemDetailsExtensions.cs       # exception → ProblemDetails middleware
  Account/
    Authentication/
      Email.cs                          # email value object
      Password.cs                       # password value object
      OAuth/                            # OAuth flow (Google, Facebook)
        AccountCreationService.cs
        AccountLinkingService.cs
        ExternalIdentity.cs
        ExternalLoginService.cs
        OAuthCallbackHandler.cs
        OAuthConfigLoader.cs
        OAuthErrorHandler.cs
        OAuthExceptions.cs
        OAuthProviderExtentions.cs
        OAuthProviderRegistry.cs
        OAuthStateValidator.cs
        UnifiedAuthController.cs
        Provider/
          GoogleProvider.cs
          FacebookProvider.cs
          Google/                       # GoogleTokenResponse, GoogleUserInfo
          Facebook/                     # FacebookTokenResponse, FacebookUserInfo
      Session/                          # JWT + refresh token management
        AccessTokenConfig.cs
        RefreshTokenConfig.cs
        RefreshController.cs
        RefreshTokenService.cs
        SessionConfig.cs
        SessionService.cs
        TokenConfig.cs
        TokenGeneratorService.cs
        Jwt.cs
    AccountPreferences/
      IAccountRepository.cs             # preferences repository interface
      AccountRepository.cs              # MongoDB implementation
      AccountDocument.cs                # MongoDB document
      DependencyInjection.cs            # module DI stub
      VersionDto.cs                     # shared version DTO
      VersionValidatorDto.cs            # validator for VersionDto
      Gameplay/
        GameplaySettings.cs             # settings record (20 fields)
        GameplayDto.cs                  # PUT DTO + GameplayPatchDto
        GameplayController.cs           # GET, PUT, PATCH, reset, defaults
        GameplayValidator.cs
      Accessibility/
        AccessibilitySettings.cs        # settings record (14 fields)
        AccessibilityDto.cs             # PUT DTO + AccessibilityPatchDto
        AccessibilityController.cs      # GET, PUT, PATCH, reset, defaults
        AccessibilityValidator.cs
      Language/
        LanguageSettings.cs             # settings record
        LanaguageDto.cs                 # PUT DTO (note: filename typo preserved)
        LanguagePatchDto.cs             # PATCH DTO
        LanguageController.cs           # GET, PUT, PATCH, reset, defaults
        LanguageValidator.cs
      Notifications/
        NotificationSettings.cs         # settings record (5 booleans)
        NotificationDto.cs              # PUT DTO
        NotificationPatchDto.cs         # PATCH DTO
        NotificationController.cs       # GET, PUT, PATCH, reset, defaults
        NotificationValidator.cs
      SocialPreferences/
        SocialSettings.cs               # settings record (4 visibility enums)
        SocialDto.cs                    # PUT DTO
        SocialPatchDto.cs               # PATCH DTO
        SocialController.cs             # GET, PUT, PATCH, reset, defaults
        SocialValidator.cs
      Audio/
        AudioSettings.cs                # settings record (9 fields)
        AudioDto.cs                     # PUT DTO + AudioPatchDto
        AudioController.cs              # GET, PUT, PATCH, reset, defaults
        AudioValidator.cs
      UiPreferences/
        UiSettings.cs                   # settings record (13 fields)
        UiPreferencesDto.cs             # PUT DTO
        UiPatchDto.cs                   # PATCH DTO
        UiController.cs                 # GET, PUT, PATCH, reset, defaults
        UiValidator.cs
    Profile/
      Avatar.cs                         # avatar value object
      Username.cs                       # username value object
  Social/
    DependencyInjection.cs
    ISocialRepository.cs
    SocialRepository.cs
    ISocialService.cs
    SocialService.cs
    SocialController.cs
    SocialPolicy.cs
    SocialDocument.cs
    FriendRequestDocument.cs
    PartyInviteDocument.cs
    BlockEnforcementMiddleware.cs
    FriendCommand.cs
    BlockCommand.cs
    PartyCommand.cs
    SocialExceptions.cs
    SocialExceptionHandler.cs
    SocialIndexDefinitions.cs
    SocialIndexHostedService.cs
    ISocialVisibilityQuery.cs
    PreferencesSocialVisibilityQuery.cs
  DoNotTouchFolder/
    ApplicationDbContext.cs
    ApplicationUser.cs
    SharedSettings/UserCenter/          # Agreement, BindAccount, OTP, TestCenter
```

## Naming conventions

- Settings types: `*Settings` (records)
- HTTP DTOs: `*Dto` (PUT), `*PatchDto` (PATCH)
- Controllers: `*Controller` (no "Settings" suffix)
- Validators: `*Validator` (FluentValidation)
- Repositories: `I*Repository` / `*Repository`
- DI: `DependencyInjection.cs` with `Add*()` extension method

## Solution

Keep a single `PickMeUp.Api.slnx` until contracts are extracted. Do not split microservices.

## Empty folders

Do not add `DeviceSettings/` under the API.
`AccountSettings/` stays empty or is removed until there is a real "account security / permissions" resource distinct from Identity and Preferences.
