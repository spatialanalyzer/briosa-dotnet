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
//
// The budget keeps one absolute deadline, measured from t0, under one lock.
// Adoption and the timer callback both decide against it. Until a deadline is
// adopted, the callback trusts its timer, as the CancellationTokenSource timer
// of client 0.5.0 did. Once one is adopted, the callback cancels only when the
// clock has reached the active deadline and re-arms for the rest otherwise, so
// a stale or superseded timer never ends a startup early.
internal sealed class StartupBudget : IDisposable
{
    // Server host startup, process observation, installation discovery, and
    // round trips, which the server's allowances exclude.
    internal static readonly TimeSpan ClientMargin = TimeSpan.FromSeconds(20);

    // The longest delay a TimeProvider timer accepts.
    internal static readonly TimeSpan MaximumTimerDelay = TimeSpan.FromMilliseconds(uint.MaxValue - 1L);

    private readonly object _sync = new();
    private readonly TimeProvider _timeProvider;
    private readonly long _start;
    private readonly bool _adoptable;
    private readonly ITimer _timer;
    private TimeSpan _deadline;
    private bool _adopted;
    private bool _expired;
    private bool _disposed;
    private bool _adoptionConsidered;
    private bool _enforced;

    public StartupBudget(BriosaStartOptions options, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(timeProvider);
        // Client 0.5.0 armed new CancellationTokenSource(StartupTimeout), which
        // counts whole milliseconds: it rejects more than uint.MaxValue - 1 of them
        // with this exception, drops a fraction of a millisecond, and starts out
        // cancelled when no whole millisecond remains.
        var wholeMilliseconds = (long)options.StartupTimeout.TotalMilliseconds;
        if (wholeMilliseconds > uint.MaxValue - 1L)
        {
#pragma warning disable CA2208 // The 0.5.0 exception named the CancellationTokenSource parameter.
            throw new ArgumentOutOfRangeException("delay");
#pragma warning restore CA2208
        }

        _timeProvider = timeProvider;
        _start = timeProvider.GetTimestamp();
        _deadline = TimeSpan.FromTicks(wholeMilliseconds * TimeSpan.TicksPerMillisecond);
        _adoptable = !options.StartupTimeoutIsExplicit;
        Source = new CancellationTokenSource();
        if (wholeMilliseconds == 0)
        {
            // Expired before the server is launched, as in client 0.5.0.
            _expired = true;
            Source.Cancel();
        }

        // Like CancellationTokenSource's own timer, the callback does not carry
        // the caller's execution context.
        using (ExecutionContext.SuppressFlow())
        {
            _timer = timeProvider.CreateTimer(
                static state => ((StartupBudget)state!).OnTimer(),
                this,
                _expired ? Timeout.InfiniteTimeSpan : _deadline,
                Timeout.InfiniteTimeSpan);
        }
    }

    public CancellationTokenSource Source { get; }

    public CancellationToken Token => Source.Token;

    // The active deadline, measured from t0.
    internal TimeSpan Deadline
    {
        get
        {
            lock (_sync)
            {
                return _deadline;
            }
        }
    }

    // Whether the first validated snapshot published lifecycle bounds, so that
    // EnsureWithinBudget checks the deadline against the clock. False for Server
    // 0.10.0, which publishes none.
    internal bool Enforced => _enforced;

    // Called with the first validated startup snapshot: server information and
    // capabilities whose live identity matches the selected installation. Later
    // snapshots, such as the refresh after Connect, never restart the budget.
    public void Adopt(LifecycleBounds? bounds, BriosaStartOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (_adoptionConsidered)
        {
            return;
        }

        _adoptionConsidered = true;
        if (bounds is null)
        {
            // No lifecycle bounds, as from Server 0.10.0: the bootstrap deadline
            // and its timer stay exactly as armed, and nothing else is checked.
            return;
        }

        // Published bounds: from here on the startup is decided against the
        // clock, whether or not the timeout can grow, so a timer callback that
        // has not run yet never lets a phase start after the deadline.
        _enforced = true;
        bool expire;
        lock (_sync)
        {
            expire = _expired || ElapsedNow() >= _deadline || (_adoptable && AdoptLocked(bounds, options));
            if (expire)
            {
                _expired = true;
            }
        }

        if (expire)
        {
            // The deadline has passed, even if its timer callback has not run
            // yet. Never extend it, and submit no further phase.
            Expire();
        }
    }

    // With published bounds, ends the startup when its budget was cancelled or
    // the clock has reached the active deadline, whether or not the timer callback
    // has run. Called before each lifecycle submission and before the startup
    // accepts readiness and is published. Without published bounds it does
    // nothing, so a Server 0.10.0 startup runs as in client 0.5.0.
    public void EnsureWithinBudget()
    {
        if (!_enforced)
        {
            return;
        }

        bool expire;
        lock (_sync)
        {
            expire = _expired || Source.IsCancellationRequested || ElapsedNow() >= _deadline;
            if (expire)
            {
                _expired = true;
            }
        }

        if (expire)
        {
            Expire();
        }
    }

    // With published bounds, whether the budget was cancelled or the clock has
    // reached the active deadline. Without them, always false. It changes nothing,
    // so an exception filter can ask.
    public bool HasExpired()
    {
        if (!_enforced)
        {
            return false;
        }

        lock (_sync)
        {
            return _expired || Source.IsCancellationRequested || ElapsedNow() >= _deadline;
        }
    }

    private void Expire()
    {
        Source.Cancel();
        Source.Token.ThrowIfCancellationRequested();
    }

    public void Dispose()
    {
        lock (_sync)
        {
            _disposed = true;
        }

        _timer.Dispose();
        Source.Dispose();
    }

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

    // Returns true when the startup must expire now. Called under the lock, with
    // the clock before the active deadline.
    private bool AdoptLocked(LifecycleBounds bounds, BriosaStartOptions options)
    {
        if (SelectAllowance(bounds, options) is not { } allowance)
        {
            return false;
        }

        var deadline = Max(BriosaStartOptions.DefaultStartupTimeout, allowance + ClientMargin);
        if (deadline <= _deadline)
        {
            return false;
        }

        // Read the clock again at the moment of arming, so time spent since the
        // decision counts against the remaining delay.
        var remaining = deadline - ElapsedNow();
        if (remaining <= TimeSpan.Zero)
        {
            return true;
        }

        if (remaining > MaximumTimerDelay)
        {
            // Cannot be scheduled: keep the bootstrap deadline rather than clamp.
            return false;
        }

        _deadline = deadline;
        _adopted = true;
        _timer.Change(remaining, Timeout.InfiniteTimeSpan);
        return false;
    }

    private void OnTimer()
    {
        lock (_sync)
        {
            if (_expired || _disposed)
            {
                return;
            }

            var now = ElapsedNow();
            if (_adopted && now < _deadline)
            {
                // The bootstrap timer that the adopted deadline superseded, or a
                // timer that fired early: keep the active deadline.
                _timer.Change(_deadline - now, Timeout.InfiniteTimeSpan);
                return;
            }

            _expired = true;
        }

        // Outside the lock: cancellation runs the token's callbacks inline.
        try
        {
            Source.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // The startup finished and disposed the budget as the timer fired.
        }
    }

    private TimeSpan ElapsedNow() => _timeProvider.GetElapsedTime(_start);

    private static TimeSpan Max(TimeSpan left, TimeSpan right) => left >= right ? left : right;
}
