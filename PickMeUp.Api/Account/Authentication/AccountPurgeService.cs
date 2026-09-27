using PickMeUp.Api.Account.Authentication.OAuth;

namespace PickMeUp.Api.Account.Authentication;

/// <summary>
/// Background service that periodically purges soft-deleted accounts
/// whose 30-day recovery window has expired.
/// Runs hourly.
/// </summary>
public sealed class AccountPurgeService(
    IAccountRepository accountRepository,
    ILogger<AccountPurgeService> logger) : BackgroundService
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly ILogger<AccountPurgeService> _logger = logger;

    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("[AccountPurge] Running purge sweep…");
                _accountRepository.PurgeExpiredAsync();
                _logger.LogInformation("[AccountPurge] Purge sweep complete.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AccountPurge] Error during purge sweep.");
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }
}
