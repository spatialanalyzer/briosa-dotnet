namespace Briosa.Client.Tests;

/// <summary>
/// Pins each branch of the startup-budget adoption rule of the shared client
/// contract ("Startup budget from published lifecycle bounds",
/// spatialanalyzer/briosa#400) with a controllable monotonic clock. The clock's
/// origin is the start instant t0.
/// </summary>
public sealed class StartupBudgetAdoptionTests
{
    private static readonly TimeSpan Bootstrap = TimeSpan.FromMinutes(2);

    // Distinct values, so that a wrong selection yields a different deadline.
    private static readonly LifecycleBounds Published = new()
    {
        StartSdk = TimeSpan.FromSeconds(150),
        LaunchSpatialAnalyzer = TimeSpan.FromSeconds(160),
        ConnectToSpatialAnalyzer = TimeSpan.FromSeconds(180),
        StartupWithLaunch = TimeSpan.FromSeconds(300),
        StartupWithoutLaunch = TimeSpan.FromSeconds(200),
    };

    [Fact]
    public void StartOptionsTellAnOmittedBudgetFromAnExplicitOne()
    {
        Assert.False(BriosaStartOptions.Default.StartupTimeoutIsExplicit);
        Assert.False(new BriosaStartOptions().StartupTimeoutIsExplicit);
        Assert.False((BriosaStartOptions.Default with { LaunchSpatialAnalyzer = false }).StartupTimeoutIsExplicit);
        var explicitDefault = new BriosaStartOptions { StartupTimeout = Bootstrap };
        Assert.True(explicitDefault.StartupTimeoutIsExplicit);
        Assert.True((explicitDefault with { LaunchSpatialAnalyzer = false }).StartupTimeoutIsExplicit);
        Assert.Equal(Bootstrap, explicitDefault.StartupTimeout);
        Assert.Equal(Bootstrap, BriosaStartOptions.Default.StartupTimeout);
    }

    // Record equality is what it was in 0.5.0, which had no explicit flag.
    [Fact]
    public void StartOptionsEqualityComparesTheBudgetOnly()
    {
        var explicitDefault = BriosaStartOptions.Default with { StartupTimeout = Bootstrap };

        Assert.True(explicitDefault.StartupTimeoutIsExplicit);
        Assert.Equal(BriosaStartOptions.Default, explicitDefault);
        Assert.Equal(BriosaStartOptions.Default.GetHashCode(), explicitDefault.GetHashCode());
        Assert.NotEqual(
            BriosaStartOptions.Default,
            BriosaStartOptions.Default with { StartupTimeout = TimeSpan.FromMinutes(3) });
    }

    // Server 0.10.0 publishes no lifecycle bounds: the bootstrap deadline and its
    // timer stay exactly as armed, even when discovery arrives late.
    [Fact]
    public void WithoutPublishedBoundsTheBootstrapDeadlineIsUntouched()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(121));

        budget.Adopt(null, BriosaStartOptions.Default);

        Assert.Equal(Bootstrap, budget.Deadline);
        Assert.Equal(Bootstrap, clock.NextDueTime());
        Assert.False(budget.Token.IsCancellationRequested);
        clock.Advance(TimeSpan.Zero);
        Assert.True(budget.Token.IsCancellationRequested);
    }

    [Theory]
    [InlineData(true, true, true, 320)]
    [InlineData(true, false, true, 220)]
    [InlineData(true, true, false, 330)]
    [InlineData(true, false, false, 170)]
    [InlineData(false, true, false, 180)]
    [InlineData(false, false, false, 120)]
    public void TheSelectedPhasesChooseTheAllowance(
        bool startSdk,
        bool launch,
        bool connect,
        int expectedSeconds)
    {
        var options = new BriosaStartOptions
        {
            StartSpatialAnalyzerSdk = startSdk,
            LaunchSpatialAnalyzer = launch,
            ConnectToSpatialAnalyzer = connect,
        };
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(options, clock);
        clock.Advance(TimeSpan.FromSeconds(5));

        budget.Adopt(Published, options);

        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), budget.Deadline);
        Assert.Equal(TimeSpan.FromSeconds(expectedSeconds), clock.NextDueTime());
    }

    // An explicit budget counts whole milliseconds, as the
    // CancellationTokenSource(TimeSpan) of client 0.5.0 did: its timer is armed
    // for the whole milliseconds, so a budget under one millisecond falls due at
    // once, on the timer's thread, and nothing is cancelled while it is created.
    [Theory]
    [InlineData(1L, 0)]
    [InlineData(9_999L, 0)]
    [InlineData(10_000L, 1)]
    [InlineData(15_000L, 1)]
    [InlineData(600_000_009_999L, 60_000_000)]
    public void AnExplicitBudgetArmsItsTimerForWholeMilliseconds(long ticks, long milliseconds)
    {
        var clock = new TimerRecordingTimeProvider();
        using var budget = new StartupBudget(
            new BriosaStartOptions { StartupTimeout = TimeSpan.FromTicks(ticks) },
            clock);

        Assert.Equal(TimeSpan.FromMilliseconds(milliseconds), clock.DueTime);
        Assert.Equal(TimeSpan.FromMilliseconds(milliseconds), budget.Deadline);
        Assert.False(budget.Token.IsCancellationRequested);
    }

    // Records the due time a timer is armed with; its timers never fire.
    private sealed class TimerRecordingTimeProvider : TimeProvider
    {
        public TimeSpan? DueTime { get; private set; }

        public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
        {
            DueTime = dueTime;
            return new IdleTimer();
        }

        private sealed class IdleTimer : ITimer
        {
            public bool Change(TimeSpan dueTime, TimeSpan period) => true;

            public void Dispose()
            {
            }

            public ValueTask DisposeAsync() => ValueTask.CompletedTask;
        }
    }

    // The deadline is measured from t0, so time already spent counts.
    [Fact]
    public void TheAdoptedDeadlineCountsFromTheStartInstant()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        clock.Advance(TimeSpan.FromSeconds(100));

        budget.Adopt(Published, BriosaStartOptions.Default);

        Assert.Equal(TimeSpan.FromSeconds(320), budget.Deadline);
        clock.Advance(TimeSpan.FromSeconds(220) - TimeSpan.FromTicks(1));
        Assert.False(budget.Token.IsCancellationRequested);
        clock.Advance(TimeSpan.FromTicks(1));
        Assert.True(budget.Token.IsCancellationRequested);
    }

    // The 120 s default is a floor: adoption only ever lengthens the budget, and
    // an allowance that does not lengthen it leaves the timer alone.
    [Fact]
    public void AShortAllowanceKeepsTheTwoMinuteFloor()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        clock.Advance(TimeSpan.FromSeconds(1));

        budget.Adopt(Published with { StartupWithLaunch = TimeSpan.FromSeconds(70) }, BriosaStartOptions.Default);

        Assert.Equal(Bootstrap, budget.Deadline);
        Assert.Equal(Bootstrap, clock.NextDueTime());
    }

    // An explicit timeout always wins, including an explicit two minutes.
    [Theory]
    [InlineData(120)]
    [InlineData(60)]
    [InlineData(600)]
    public void AnExplicitBudgetIsNeverAdopted(int seconds)
    {
        var options = new BriosaStartOptions { StartupTimeout = TimeSpan.FromSeconds(seconds) };
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(options, clock);
        clock.Advance(TimeSpan.FromSeconds(1));

        budget.Adopt(Published, options);

        Assert.Equal(TimeSpan.FromSeconds(seconds), budget.Deadline);
        Assert.Equal(TimeSpan.FromSeconds(seconds), clock.NextDueTime());
    }

    // A needed allowance that is absent or zero (both null in the public value)
    // keeps the bootstrap deadline. A partial budget is never summed.
    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    public void AMissingAllowanceKeepsTheBootstrapDeadline(bool startSdk, bool launch, bool connect)
    {
        var options = new BriosaStartOptions
        {
            StartSpatialAnalyzerSdk = startSdk,
            LaunchSpatialAnalyzer = launch,
            ConnectToSpatialAnalyzer = connect,
        };
        var missing = (startSdk, launch, connect) switch
        {
            (true, true, true) => Published with { StartupWithLaunch = null },
            (true, false, true) => Published with { StartupWithoutLaunch = null },
            (true, true, false) => Published with { LaunchSpatialAnalyzer = null },
            (true, false, false) => Published with { StartSdk = null },
            _ => Published with { LaunchSpatialAnalyzer = null },
        };
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(options, clock);

        budget.Adopt(missing, options);

        Assert.Equal(Bootstrap, budget.Deadline);
        Assert.Equal(Bootstrap, clock.NextDueTime());
    }

    [Fact]
    public void TheFullStartupNeverSumsItsPhaseAllowances()
    {
        var bounds = Published with { StartupWithLaunch = null };

        Assert.Null(StartupBudget.SelectAllowance(bounds, BriosaStartOptions.Default));
        Assert.Null(StartupBudget.SelectAllowance(
            Published with { StartSdk = null },
            new BriosaStartOptions { ConnectToSpatialAnalyzer = false }));
    }

    // A snapshot observed at 121 s must not extend a 300 s allowance just because
    // the bootstrap timer callback has not run yet: the startup expires now.
    [Theory]
    [InlineData(120)]
    [InlineData(121)]
    public void LateDiscoveryNeverRevivesAnExpiredStartup(int seconds)
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(seconds));

        Assert.Throws<OperationCanceledException>(
            () => budget.Adopt(Published, BriosaStartOptions.Default));

        Assert.True(budget.Token.IsCancellationRequested);
        Assert.Equal(Bootstrap, budget.Deadline);
    }

    // With lifecycle bounds present, a late discovery expires the startup even
    // when the allowance its phases need is missing: a missing allowance keeps
    // the bootstrap deadline, and that deadline has passed.
    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    [InlineData(false, false, false)]
    public void LateDiscoveryWithAMissingAllowanceStillExpires(bool startSdk, bool launch, bool connect)
    {
        var options = new BriosaStartOptions
        {
            StartSpatialAnalyzerSdk = startSdk,
            LaunchSpatialAnalyzer = launch,
            ConnectToSpatialAnalyzer = connect,
        };
        var partial = Published with
        {
            StartupWithLaunch = null,
            StartupWithoutLaunch = null,
            LaunchSpatialAnalyzer = null,
        };
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(options, clock);
        clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(121));

        Assert.Throws<OperationCanceledException>(() => budget.Adopt(partial, options));

        Assert.True(budget.Token.IsCancellationRequested);
        Assert.Equal(Bootstrap, budget.Deadline);
    }

    // Before the deadline, a missing allowance leaves everything as armed.
    [Fact]
    public void AMissingAllowanceBeforeTheDeadlineChangesNothing()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(119));

        budget.Adopt(Published with { StartupWithLaunch = null }, BriosaStartOptions.Default);

        Assert.False(budget.Token.IsCancellationRequested);
        Assert.Equal(Bootstrap, budget.Deadline);
        Assert.Equal(Bootstrap, clock.NextDueTime());
    }

    // The decision reads the clock at 119 s and the timer is armed at 121 s:
    // the remaining delay is measured when arming, so the timer falls due at the
    // adopted absolute deadline (320 s), not two seconds after it.
    [Fact]
    public void TheTimerIsArmedFromTheClockAtTheMomentOfArming()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        clock.Advance(TimeSpan.FromSeconds(119));
        clock.AdvanceAfterReads(1, TimeSpan.FromSeconds(2));

        budget.Adopt(Published, BriosaStartOptions.Default);

        Assert.Equal(TimeSpan.FromSeconds(121), clock.Elapsed);
        Assert.Equal(TimeSpan.FromSeconds(320), budget.Deadline);
        Assert.Equal(TimeSpan.FromSeconds(320), clock.NextDueTime());
        clock.Advance(TimeSpan.FromSeconds(199) - TimeSpan.FromTicks(1));
        Assert.False(budget.Token.IsCancellationRequested);
        clock.Advance(TimeSpan.FromTicks(1));
        Assert.True(budget.Token.IsCancellationRequested);
    }

    // If arming finds the adopted deadline already passed, the startup expires.
    [Fact]
    public void ArmingPastTheAdoptedDeadlineExpires()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        clock.Advance(TimeSpan.FromSeconds(119));
        clock.AdvanceAfterReads(1, TimeSpan.FromSeconds(300));

        Assert.Throws<OperationCanceledException>(
            () => budget.Adopt(Published, BriosaStartOptions.Default));

        Assert.True(budget.Token.IsCancellationRequested);
    }

    // The bootstrap timer callback runs after the adopted deadline replaced it:
    // it checks the active deadline and re-arms instead of cancelling.
    [Fact]
    public void ASupersededTimerCallbackKeepsTheAdoptedDeadline()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        clock.Advance(TimeSpan.FromSeconds(10));
        budget.Adopt(Published, BriosaStartOptions.Default);
        clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(110));

        clock.FireActiveTimersEarly();

        Assert.False(budget.Token.IsCancellationRequested);
        Assert.Equal(TimeSpan.FromSeconds(320), clock.NextDueTime());
        clock.Advance(TimeSpan.FromSeconds(200));
        Assert.True(budget.Token.IsCancellationRequested);
    }

    // Without adoption the callback trusts its timer, as the 0.5.0 budget did.
    [Fact]
    public void WithoutAdoptionTheTimerCallbackEndsTheBudget()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        budget.Adopt(null, BriosaStartOptions.Default);

        clock.Advance(Bootstrap);

        Assert.True(budget.Token.IsCancellationRequested);
    }

    [Fact]
    public void ATimerAfterDisposalDoesNothing()
    {
        var clock = new ManualTimeProvider();
        var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        budget.Dispose();

        clock.Advance(TimeSpan.FromMinutes(5));

        Assert.Null(clock.NextDueTime());
    }

    // With published bounds, an explicit budget never grows but is checked against
    // the clock before each phase, whether or not its timer callback has run.
    [Fact]
    public void AnExplicitBudgetWithPublishedBoundsIsCheckedAgainstTheClock()
    {
        var options = new BriosaStartOptions { StartupTimeout = TimeSpan.FromSeconds(60) };
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(options, clock);
        budget.Adopt(Published, options);
        budget.EnsureWithinBudget();
        clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(60));

        Assert.True(budget.Enforced);
        Assert.Equal(TimeSpan.FromSeconds(60), budget.Deadline);
        Assert.Throws<OperationCanceledException>(budget.EnsureWithinBudget);
        Assert.True(budget.Token.IsCancellationRequested);
    }

    // Without published bounds, nothing but the timer ends the startup, as in
    // client 0.5.0.
    [Fact]
    public void WithoutPublishedBoundsNothingButTheTimerEndsTheStartup()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        budget.Adopt(null, BriosaStartOptions.Default);
        clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(500));

        budget.EnsureWithinBudget();

        Assert.False(budget.Enforced);
        Assert.False(budget.Token.IsCancellationRequested);
        clock.Advance(TimeSpan.Zero);
        Assert.True(budget.Token.IsCancellationRequested);
    }

    [Fact]
    public void AnAlreadyExpiredStartupStaysExpired()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        clock.Advance(TimeSpan.FromSeconds(125));
        Assert.True(budget.Token.IsCancellationRequested);

        Assert.Throws<OperationCanceledException>(
            () => budget.Adopt(Published, BriosaStartOptions.Default));

        Assert.Equal(Bootstrap, budget.Deadline);
    }

    // The largest allowance the protocol can carry plus the margin cannot be
    // scheduled. The bootstrap deadline stays; no clamped delay is scheduled.
    [Fact]
    public void AnUnschedulableDeadlineKeepsTheBootstrapDeadline()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);

        budget.Adopt(
            Published with { StartupWithLaunch = TimeSpan.FromMilliseconds(uint.MaxValue) },
            BriosaStartOptions.Default);

        Assert.Equal(Bootstrap, budget.Deadline);
        Assert.Equal(Bootstrap, clock.NextDueTime());
    }

    [Fact]
    public void TheLongestSchedulableDeadlineIsAdopted()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        var allowance = StartupBudget.MaximumTimerDelay - StartupBudget.ClientMargin;

        budget.Adopt(Published with { StartupWithLaunch = allowance }, BriosaStartOptions.Default);

        Assert.Equal(StartupBudget.MaximumTimerDelay, budget.Deadline);
        Assert.Equal(StartupBudget.MaximumTimerDelay, clock.NextDueTime());
    }

    // Adoption happens once, at the first validated snapshot.
    [Fact]
    public void ALaterSnapshotNeverRestartsTheBudget()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        budget.Adopt(Published, BriosaStartOptions.Default);
        clock.Advance(TimeSpan.FromSeconds(100));

        budget.Adopt(Published with { StartupWithLaunch = TimeSpan.FromSeconds(900) }, BriosaStartOptions.Default);

        Assert.Equal(TimeSpan.FromSeconds(320), budget.Deadline);
        Assert.Equal(TimeSpan.FromSeconds(320), clock.NextDueTime());
    }

    // A first snapshot without bounds is still the first: a later one with bounds
    // does not adopt.
    [Fact]
    public void TheFirstSnapshotDecidesEvenWithoutBounds()
    {
        var clock = new ManualTimeProvider();
        using var budget = new StartupBudget(BriosaStartOptions.Default, clock);
        budget.Adopt(null, BriosaStartOptions.Default);

        budget.Adopt(Published, BriosaStartOptions.Default);

        Assert.Equal(Bootstrap, budget.Deadline);
    }

    // Fall back per purpose: an unknown interaction mode or a missing host bound
    // does not make a usable startup allowance unavailable.
    [Fact]
    public void AMissingHostBoundDoesNotAffectTheStartupAllowance()
    {
        Assert.Equal(GracefulShutdownBoundKind.Unavailable, Published.HostGracefulShutdown.Kind);
        Assert.Equal(
            TimeSpan.FromSeconds(300),
            StartupBudget.SelectAllowance(Published, BriosaStartOptions.Default));
    }
}
