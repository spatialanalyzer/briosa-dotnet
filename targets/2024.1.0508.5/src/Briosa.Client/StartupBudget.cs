namespace Briosa;

// The overall startup budget of one StartAsync call, with the adoption rule of
// the shared client contract ("Startup budget from published lifecycle bounds",
// spatialanalyzer/briosa docs/architecture/client-library-behavioral-contract.md):
//
// 1. t0 is captured once, when startup begins, before host launch or discovery.
// 2. An explicit StartupTimeout always wins, including an explicit two minutes.
// 3. An omitted one arms the bootstrap deadline at t0 + 120 s. Against a server
//    that publishes no lifecycle bounds, such as Server 0.10.0, that deadline
//    stays untouched.
// 4. Adoption happens once, at the first validated startup snapshot, with the
//    allowance for the selected phases. A missing, zero, or unusable allowance
//    keeps the bootstrap deadline; a partial budget is never summed.
// 5. deadline = t0 + max(120 s, allowance + 20 s). An expired startup is never
//    revived, and a delay that cannot be scheduled keeps the existing deadline.
// 6. No individual lifecycle RPC gets a default deadline of its own.
internal sealed class StartupBudget : IDisposable
{
    // Server host startup, process observation, installation discovery, and
    // round trips, which the server's allowances exclude.
    internal static readonly TimeSpan ClientMargin = TimeSpan.FromSeconds(20);

    // The longest delay a CancellationTokenSource timer accepts.
    internal static readonly TimeSpan MaximumTimerDelay = TimeSpan.FromMilliseconds(uint.MaxValue - 1L);

    private readonly TimeProvider _timeProvider;
    private readonly long _start;
    private readonly bool _adoptable;
    private TimeSpan _deadline;
    private bool _adoptionConsidered;

    public StartupBudget(BriosaStartOptions options, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(timeProvider);
        _timeProvider = timeProvider;
        _start = timeProvider.GetTimestamp();
        _deadline = options.StartupTimeout;
        _adoptable = !options.StartupTimeoutIsExplicit;
        Source = new CancellationTokenSource(_deadline, timeProvider);
    }

    public CancellationTokenSource Source { get; }

    public CancellationToken Token => Source.Token;

    // The current deadline, measured from t0.
    internal TimeSpan Deadline => _deadline;

    // Called with the first validated startup snapshot: server information and
    // capabilities whose live identity matches the selected installation. Later
    // snapshots, such as the refresh after Connect, never restart the budget.
    public void Adopt(LifecycleBounds? bounds, BriosaStartOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (!_adoptable || _adoptionConsidered)
        {
            return;
        }

        _adoptionConsidered = true;
        if (SelectAllowance(bounds, options) is not { } allowance)
        {
            // Nothing to adopt: the bootstrap deadline stays exactly as armed.
            return;
        }

        var now = _timeProvider.GetElapsedTime(_start);
        if (Source.IsCancellationRequested || now >= _deadline)
        {
            // The bootstrap deadline has passed, even if its timer callback has
            // not run yet. Never extend it, and submit no further phase.
            Expire();
        }

        var deadline = Max(BriosaStartOptions.DefaultStartupTimeout, allowance + ClientMargin);
        if (deadline <= now)
        {
            Expire();
        }

        if (deadline <= _deadline)
        {
            return;
        }

        var delay = deadline - now;
        if (delay > MaximumTimerDelay)
        {
            // Cannot be scheduled: keep the bootstrap deadline rather than clamp.
            return;
        }

        Source.CancelAfter(delay);
        _deadline = deadline;
    }

    public void Dispose() => Source.Dispose();

    // The server allowance for the selected phases, or null when the selection
    // has no server phase or an allowance it needs is unavailable.
    internal static TimeSpan? SelectAllowance(LifecycleBounds? bounds, BriosaStartOptions options)
    {
        if (bounds is null)
        {
            return null;
        }

        return (options.StartSpatialAnalyzerSdk, options.LaunchSpatialAnalyzer, options.ConnectToSpatialAnalyzer) switch
        {
            (true, true, true) => bounds.StartupWithLaunch,
            (true, false, true) => bounds.StartupWithoutLaunch,
            (true, true, false) => bounds is { StartSdk: { } start, LaunchSpatialAnalyzer: { } launch }
                ? start + launch
                : null,
            (true, false, false) => bounds.StartSdk,
            (false, true, false) => bounds.LaunchSpatialAnalyzer,
            // No server phase. Connect without Start SDK is rejected before startup.
            _ => null,
        };
    }

    private void Expire()
    {
        Source.Cancel();
        Source.Token.ThrowIfCancellationRequested();
    }

    private static TimeSpan Max(TimeSpan left, TimeSpan right) => left >= right ? left : right;
}
