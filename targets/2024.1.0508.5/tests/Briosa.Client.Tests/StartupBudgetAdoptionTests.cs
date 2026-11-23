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
