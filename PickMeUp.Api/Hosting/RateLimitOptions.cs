using System.Threading.RateLimiting;
using Microsoft.Extensions.Options;

namespace PickMeUp.Api.Hosting;

/// <summary>
/// Configuration for the fixed-window rate limiter.
/// </summary>
public sealed class RateLimitOptions
{
    public const string SectionName = "RateLimiting";
    public int PermitLimit { get; init; } = 100;
    public int WindowSeconds { get; init; } = 60;
    public int QueueLimit { get; init; } = 0;
}
