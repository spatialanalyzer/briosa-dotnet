namespace Briosa;

/// <summary>Configures one reusable target-specific Briosa client.</summary>
public sealed record BriosaClientOptions
{
    /// <summary>
    /// Gets the optional client-imposed deadline for MP commands. Lifecycle calls
    /// use their own documented bounds.
    /// </summary>
    public TimeSpan? CommandTimeout { get; init; }

    internal void Validate()
    {
        if (CommandTimeout is { } timeout && timeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(CommandTimeout),
                "The command timeout must be positive when supplied.");
        }
    }
}

/// <summary>Controls the approved SpatialAnalyzer launch inputs.</summary>
public sealed record SpatialAnalyzerLaunchOptions
{
    /// <summary>Gets an optional absolute local SA job path.</summary>
    public string? JobFilePath { get; init; }

    /// <summary>Gets an optional quick-start instrument name.</summary>
    public string? QuickStartInstrumentName { get; init; }

    /// <summary>Gets whether SpatialAnalyzer should start minimized.</summary>
    public bool StartMinimized { get; init; }

    internal bool IsDefault =>
        JobFilePath is null &&
        QuickStartInstrumentName is null &&
        !StartMinimized;

    internal void Validate()
    {
        if (JobFilePath is not null && QuickStartInstrumentName is not null)
        {
            throw new ArgumentException(
                "JobFilePath and QuickStartInstrumentName are mutually exclusive.");
        }

        if (JobFilePath is not null &&
            (string.IsNullOrWhiteSpace(JobFilePath) ||
                !Path.IsPathFullyQualified(JobFilePath)))
        {
            throw new ArgumentException(
                "JobFilePath must be an absolute non-empty path when supplied.",
                nameof(JobFilePath));
        }

        if (QuickStartInstrumentName is not null &&
            (string.IsNullOrWhiteSpace(QuickStartInstrumentName) ||
                QuickStartInstrumentName.Length > 256 ||
                QuickStartInstrumentName.Any(char.IsControl)))
        {
            throw new ArgumentException(
                "QuickStartInstrumentName must be a safe non-empty value of at most 256 characters.",
                nameof(QuickStartInstrumentName));
        }
    }
}

/// <summary>Selects the phases performed by one client-owned server startup.</summary>
public sealed record BriosaStartOptions
{
    /// <summary>Gets per-startup installation selection and SA location constraints.</summary>
    public BriosaServerSelection ServerSelection { get; init; } = new();
    /// <summary>Gets startup-only server logging overrides; null preserves server configuration.</summary>
    public BriosaLoggingOptions? Logging { get; init; }
    /// <summary>Gets whether startup creates a disconnected SDK generation.</summary>
    public bool StartSpatialAnalyzerSdk { get; init; } = true;

    /// <summary>Gets whether startup launches a fresh SpatialAnalyzer application.</summary>
    public bool LaunchSpatialAnalyzer { get; init; } = true;

    /// <summary>Gets whether startup connects the new SDK and proves MP readiness.</summary>
    public bool ConnectToSpatialAnalyzer { get; init; } = true;

    /// <summary>Gets controlled SpatialAnalyzer launch inputs.</summary>
    public SpatialAnalyzerLaunchOptions LaunchOptions { get; init; } = new();

    /// <summary>
    /// Gets the overall client startup budget: one bound for the whole
    /// <see cref="BriosaClient.StartAsync(BriosaStartOptions, CancellationToken)"/>,
    /// whichever phases are selected. Expiry raises <see cref="BriosaStartupException"/>
    /// with diagnostic <c>startup-timeout</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A value you set is the budget, always, including an explicit two minutes.
    /// When you leave it unset, the budget starts at two minutes, the value this
    /// property returns. Against Server 0.10.1 and later, which publish their
    /// lifecycle bounds, the client then sizes it once from the server's own
    /// startup allowance for the selected phases, at the first validated discovery
    /// of the server: the budget becomes the longer of two minutes and that
    /// allowance plus 20 seconds, counted from the start of the call. It only ever
    /// grows, it never revives a startup whose two minutes have already run out,
    /// and it stays two minutes when the server publishes no usable allowance.
    /// Against Server 0.10.0, which publishes none, the budget is two minutes, as
    /// in client 0.5.0.
    /// </para>
    /// <para>
    /// The default of two minutes covers the server's default worst case of 100 seconds
    /// (SDK start 10 s, SpatialAnalyzer launch 30 s, Connect with readiness probe and
    /// application association 60 s) plus 20 seconds for server host startup, process
    /// observation, and round trips. With Server 0.10.0, raise it when the server's
    /// lifecycle bounds are raised. Expiry during an in-flight RPC is reported the same way; cancelling the
    /// caller's token only stops that caller from waiting. A server that includes
    /// spatialanalyzer/briosa#315 finishes an SDK start or connection it has accepted
    /// after the client stops waiting; older servers, including released 0.9 builds,
    /// cancel it, which can leave the SDK stopped or faulted.
    /// </para>
    /// <para>
    /// Equality of two <see cref="BriosaStartOptions"/> values compares this budget
    /// only, as in client 0.5.0: an unset budget equals an explicit two minutes.
    /// </para>
    /// </remarks>
    public TimeSpan StartupTimeout
    {
        get => _startupTimeout.Value;
        init => _startupTimeout = new StartupTimeoutSetting(value, IsExplicit: true);
    }

    private readonly StartupTimeoutSetting _startupTimeout = new(DefaultStartupTimeout, IsExplicit: false);

    /// <summary>Gets the ordinary ready-for-MP startup procedure.</summary>
    public static BriosaStartOptions Default { get; } = new();

    // The budget when StartupTimeout is left unset, and the floor of an adopted
    // budget. Expression-bodied so that Default, initialized above, sees it.
    internal static TimeSpan DefaultStartupTimeout => TimeSpan.FromMinutes(2);

    // Whether the caller set StartupTimeout. An explicit value always wins over
    // the server's published allowance, including an explicit two minutes.
    internal bool StartupTimeoutIsExplicit => _startupTimeout.IsExplicit;

    // Equality and hash use the value only, so record equality of the options is
    // what it was in 0.5.0, when no explicit flag existed.
    private sealed record StartupTimeoutSetting(TimeSpan Value, bool IsExplicit)
    {
        public bool Equals(StartupTimeoutSetting? other) => other is not null && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();
    }

    internal void Validate()
    {
        _ = Logging?.ToArguments();
        ArgumentNullException.ThrowIfNull(ServerSelection);
        ServerSelection.Validate();
        if (StartupTimeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(StartupTimeout),
                "The startup timeout must be positive.");
        }

        if (ConnectToSpatialAnalyzer && !StartSpatialAnalyzerSdk)
        {
            throw new ArgumentException(
                "ConnectToSpatialAnalyzer requires StartSpatialAnalyzerSdk in the same startup call.");
        }

        ArgumentNullException.ThrowIfNull(LaunchOptions);
        LaunchOptions.Validate();
        if (!LaunchSpatialAnalyzer && !LaunchOptions.IsDefault)
        {
            throw new ArgumentException(
                "LaunchOptions must remain at their defaults when LaunchSpatialAnalyzer is false.");
        }
    }
}
