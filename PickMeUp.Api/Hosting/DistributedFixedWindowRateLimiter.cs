using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace PickMeUp.Api.Hosting;

/// <summary>
/// A Redis-backed fixed-window rate limiter that is instantiated per partition (user).
/// Each instance tracks one user's rate limit window using Redis sorted sets.
/// The partition key is embedded at construction time so the <see cref="RateLimiter"/> API
/// (which has no context parameter) works cleanly.
/// </summary>
public sealed class RedisBackedFixedWindowLimiter : RateLimiter
{
    private readonly IDatabase _db;
    private readonly DistributedFixedWindowRateLimiterOptions _options;
    private readonly string _partitionKey;
    private long _totalFailedLeases;
    private long _totalSuccessfulLeases;

    public RedisBackedFixedWindowLimiter(
        IConnectionMultiplexer redis,
        DistributedFixedWindowRateLimiterOptions options,
        string partitionKey)
        : base()
    {
        _db = (redis ?? throw new ArgumentNullException(nameof(redis))).GetDatabase();
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _partitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey));
    }

    public override TimeSpan? IdleDuration => null;

    public override RateLimiterStatistics? GetStatistics()
    {
        return new RateLimiterStatistics
        {
            TotalFailedLeases = Interlocked.Read(ref _totalFailedLeases),
            TotalSuccessfulLeases = Interlocked.Read(ref _totalSuccessfulLeases),
        };
    }

    protected override RateLimitLease AttemptAcquireCore(int permitCount)
    {
        var key = $"ratelimit:{_options.KeyPrefix}:{_partitionKey}";
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var windowStartMs = now - (long)_options.Window.TotalMilliseconds;

        // Remove expired entries and count current requests in the window
        _db.SortedSetRemoveRangeByScore(key, 0, windowStartMs);
        var currentCount = _db.SortedSetLength(key);

        if (currentCount + permitCount > _options.PermitLimit)
        {
            Interlocked.Increment(ref _totalFailedLeases);
            return new FixedWindowLease(isAcquired: false);
        }

        // Atomically add entries for this request
        var batch = _db.CreateBatch();
        for (var i = 0; i < permitCount; i++)
        {
            var member = $"{now}:{i}";
            batch.SortedSetAddAsync(key, member, now);
        }
        batch.KeyExpireAsync(key, _options.Window);
        batch.Execute();

        Interlocked.Increment(ref _totalSuccessfulLeases);
        return new FixedWindowLease(isAcquired: true);
    }

    protected override ValueTask<RateLimitLease> AcquireAsyncCore(
        int permitCount,
        CancellationToken cancellationToken)
    {
        return new ValueTask<RateLimitLease>(AttemptAcquireCore(permitCount));
    }
}

/// <summary>
/// Simple lease implementation for the fixed-window rate limiter.
/// </summary>
internal sealed class FixedWindowLease : RateLimitLease
{
    private static readonly IReadOnlyList<string> s_metadataNames = [];

    public FixedWindowLease(bool isAcquired)
    {
        IsAcquired = isAcquired;
    }

    public override bool IsAcquired { get; }

    public override IReadOnlyList<string> MetadataNames => s_metadataNames;

    public override bool TryGetMetadata(string metadataName, out object? metadata)
    {
        metadata = null;
        return false;
    }
}

/// <summary>
/// Configuration for the distributed fixed-window rate limiter.
/// </summary>
public sealed class DistributedFixedWindowRateLimiterOptions
{
    public const string SectionName = "RateLimiting";

    /// <summary>Key prefix for Redis sorted-set keys to avoid collisions.</summary>
    public string KeyPrefix { get; init; } = "api";

    /// <summary>Maximum number of requests allowed per window.</summary>
    public int PermitLimit { get; init; } = 100;

    /// <summary>Duration of the fixed window.</summary>
    public TimeSpan Window { get; init; } = TimeSpan.FromMinutes(1);
}
