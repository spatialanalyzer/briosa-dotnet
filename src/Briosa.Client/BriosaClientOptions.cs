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
    /// <summary>Gets whether startup creates a disconnected SDK generation.</summary>
    public bool StartSpatialAnalyzerSdk { get; init; } = true;

    /// <summary>Gets whether startup launches a fresh SpatialAnalyzer application.</summary>
    public bool LaunchSpatialAnalyzer { get; init; } = true;

    /// <summary>Gets whether startup connects the new SDK and proves MP readiness.</summary>
    public bool ConnectToSpatialAnalyzer { get; init; } = true;

    /// <summary>Gets controlled SpatialAnalyzer launch inputs.</summary>
    public SpatialAnalyzerLaunchOptions LaunchOptions { get; init; } = new();

    /// <summary>Gets the overall client startup timeout.</summary>
    public TimeSpan StartupTimeout { get; init; } = TimeSpan.FromSeconds(30);

    /// <summary>Gets the ordinary ready-for-MP startup procedure.</summary>
    public static BriosaStartOptions Default { get; } = new();

    internal void Validate()
    {
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
