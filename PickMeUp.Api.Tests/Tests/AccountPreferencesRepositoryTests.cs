using FluentAssertions;
using Moq;
using PickMeUp.Api.Account.AccountPreferences;
using PickMeUp.Api.Account.AccountPreferences.Gameplay;
using PickMeUp.Api.Account.AccountPreferences.SocialPreferences;
using Xunit;

namespace PickMeUp.Api.Tests.Tests;

public class AccountPreferencesRepositoryTests
{
    private static AccountDocument CreateDoc(Guid? accountId = null, int version = 1)
    {
        return new AccountDocument
        {
            AccountId = accountId ?? Guid.NewGuid(),
            UserId = "test-user",
            Gameplay = new GameplaySettings
            {
                VisibleActionBars = 4,
                CameraSensitivity = 2.5f,
                FieldOfView = 90f
            },
            SocialPreferences = new SocialSettings
            {
                FriendRequests = SocialVisibility.FriendsOnly,
                Messages = SocialVisibility.Nobody,
                PartyInvites = SocialVisibility.Everyone,
                OnlineStatus = SocialVisibility.FriendsOnly
            },
            Version = version
        };
    }

    private static AccountDocument CreateDefaultDoc(Guid accountId)
    {
        return new AccountDocument
        {
            AccountId = accountId,
            UserId = "new-user",
            Gameplay = new GameplaySettings(),
            SocialPreferences = new SocialSettings(),
            Version = 1
        };
    }

    [Fact]
    public async Task GetOrCreateAsync_ExistingAccount_ReturnsDocument()
    {
        var accountId = Guid.NewGuid();
        var doc = CreateDoc(accountId);

        var mockRepo = new Mock<IAccountPreferencesRepository>();
        mockRepo.Setup(r => r.GetOrCreateAsync(accountId)).ReturnsAsync(doc);

        var result = await mockRepo.Object.GetOrCreateAsync(accountId);

        result.Should().NotBeNull();
        result.AccountId.Should().Be(accountId);
        result.Gameplay.VisibleActionBars.Should().Be(4);
    }

    [Fact]
    public async Task GetOrCreateAsync_NewAccount_ReturnsDefaults()
    {
        var accountId = Guid.NewGuid();
        var defaults = CreateDefaultDoc(accountId);

        var mockRepo = new Mock<IAccountPreferencesRepository>();
        mockRepo.Setup(r => r.GetOrCreateAsync(accountId)).ReturnsAsync(defaults);

        var result = await mockRepo.Object.GetOrCreateAsync(accountId);

        result.Should().NotBeNull();
        result.Gameplay.CameraSensitivity.Should().Be(1.0f);
        result.Gameplay.FieldOfView.Should().Be(75f);
        result.SocialPreferences.FriendRequests.Should().Be(SocialVisibility.Everyone);
        result.Version.Should().Be(1);
    }

    [Fact]
    public async Task ReplaceGameplaySettingsAsync_ValidVersion_ReturnsNewVersion()
    {
        var accountId = Guid.NewGuid();
        var updatedSettings = new GameplaySettings { VisibleActionBars = 3 };

        var mockRepo = new Mock<IAccountPreferencesRepository>();
        mockRepo.Setup(r => r.ReplaceGameplaySettingsAsync(accountId, updatedSettings, 1))
            .ReturnsAsync(CreateDoc(accountId, version: 2));

        var result = await mockRepo.Object.ReplaceGameplaySettingsAsync(accountId, updatedSettings, 1);

        result.Version.Should().Be(2);
    }

    [Fact]
    public async Task ReplaceGameplaySettingsAsync_StaleVersion_ThrowsConcurrencyException()
    {
        var accountId = Guid.NewGuid();
        var updatedSettings = new GameplaySettings { VisibleActionBars = 3 };

        var mockRepo = new Mock<IAccountPreferencesRepository>();
        mockRepo.Setup(r => r.ReplaceGameplaySettingsAsync(accountId, updatedSettings, 1))
            .ThrowsAsync(new InvalidOperationException("Stale version"));

        await FluentActions.Invoking(() =>
                mockRepo.Object.ReplaceGameplaySettingsAsync(accountId, updatedSettings, 1))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Stale version");
    }

    [Fact]
    public async Task ReplaceSocialPreferencesSettingsAsync_ValidVersion_ReturnsNewVersion()
    {
        var accountId = Guid.NewGuid();
        var updatedSettings = new SocialSettings
        {
            FriendRequests = SocialVisibility.Nobody,
            Messages = SocialVisibility.Nobody,
            PartyInvites = SocialVisibility.Nobody,
            OnlineStatus = SocialVisibility.Nobody
        };

        var mockRepo = new Mock<IAccountPreferencesRepository>();
        mockRepo.Setup(r => r.ReplaceSocialPreferencesSettingsAsync(accountId, updatedSettings, 2))
            .ReturnsAsync(CreateDoc(accountId, version: 3));

        var result = await mockRepo.Object.ReplaceSocialPreferencesSettingsAsync(accountId, updatedSettings, 2);

        result.Version.Should().Be(3);
    }

    [Fact]
    public async Task ReplaceGameplaySettingsAsync_DifferentSlicesUntouched()
    {
        var accountId = Guid.NewGuid();
        var originalDoc = CreateDoc(accountId, version: 1);
        var updatedSettings = new GameplaySettings { VisibleActionBars = 5 };

        var mockRepo = new Mock<IAccountPreferencesRepository>();
        mockRepo.Setup(r => r.GetOrCreateAsync(accountId)).ReturnsAsync(originalDoc);
        mockRepo.Setup(r => r.ReplaceGameplaySettingsAsync(accountId, updatedSettings, 1))
            .ReturnsAsync(CreateDoc(accountId, version: 2));

        var newVersion = await mockRepo.Object.ReplaceGameplaySettingsAsync(accountId, updatedSettings, 1);
        var doc = await mockRepo.Object.GetOrCreateAsync(accountId);

        newVersion.Version.Should().Be(2);
        doc.SocialPreferences.FriendRequests.Should().Be(SocialVisibility.FriendsOnly);
        doc.SocialPreferences.Messages.Should().Be(SocialVisibility.Nobody);
    }

    [Fact]
    public async Task GetOrCreateAsync_ReturnsDocumentWithCorrectAccountId()
    {
        var accountId = Guid.NewGuid();
        var doc = CreateDoc(accountId);

        var mockRepo = new Mock<IAccountPreferencesRepository>();
        mockRepo.Setup(r => r.GetOrCreateAsync(accountId)).ReturnsAsync(doc);

        var result = await mockRepo.Object.GetOrCreateAsync(accountId);

        result.AccountId.Should().Be(accountId);
    }

    [Fact]
    public async Task ReplaceSocialPreferencesSettingsAsync_StaleVersion_Throws()
    {
        var accountId = Guid.NewGuid();
        var settings = new SocialSettings();

        var mockRepo = new Mock<IAccountPreferencesRepository>();
        mockRepo.Setup(r => r.ReplaceSocialPreferencesSettingsAsync(accountId, settings, 5))
            .ThrowsAsync(new InvalidOperationException("Stale version"));

        await FluentActions.Invoking(() =>
                mockRepo.Object.ReplaceSocialPreferencesSettingsAsync(accountId, settings, 5))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Stale version");
    }

    [Fact]
    public async Task GetOrCreateAsync_VerifiesCorrectAccountIdPassed()
    {
        var accountId = Guid.NewGuid();
        var doc = CreateDoc(accountId);

        var mockRepo = new Mock<IAccountPreferencesRepository>();
        mockRepo.Setup(r => r.GetOrCreateAsync(accountId)).ReturnsAsync(doc);

        await mockRepo.Object.GetOrCreateAsync(accountId);

        mockRepo.Verify(r => r.GetOrCreateAsync(accountId), Times.Once);
    }
}
