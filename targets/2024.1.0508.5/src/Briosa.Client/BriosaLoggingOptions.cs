using System.Globalization;

namespace Briosa;

/// <summary>Startup-only server log filtering and bounded file retention.</summary>
public sealed record BriosaLoggingOptions
{
    /// <summary>Gets the optional global minimum severity.</summary>
    public BriosaLogLevel? MinimumLevel { get; init; }
    /// <summary>Gets optional category minimum severities.</summary>
    public IReadOnlyDictionary<string, BriosaLogLevel> CategoryLevels { get; init; } =
        new Dictionary<string, BriosaLogLevel>();
    /// <summary>Gets whether console output is enabled.</summary>
    public bool? ConsoleEnabled { get; init; }
    /// <summary>Gets whether persistent JSONL output is enabled.</summary>
    public bool? FileEnabled { get; init; }
    /// <summary>Gets an optional absolute log directory.</summary>
    public string? FileDirectory { get; init; }
    /// <summary>Gets the maximum file size, from 1 through 1024 MiB.</summary>
    public int? MaxFileSizeMiB { get; init; }
    /// <summary>Gets the retained file count, from 1 through 1000.</summary>
    public int? RetainedFileCount { get; init; }
    /// <summary>Gets the retained age, from 1 through 365 days.</summary>
    public int? MaxAgeDays { get; init; }
    /// <summary>Gets the directory storage limit, from 1 through 10240 MiB.</summary>
    public int? MaxTotalSizeMiB { get; init; }

    internal string[] ToArguments()
    {
        if (MinimumLevel.HasValue && !Enum.IsDefined(MinimumLevel.Value) ||
            CategoryLevels is null || CategoryLevels.Any(pair =>
                string.IsNullOrWhiteSpace(pair.Key) || pair.Key.Length > 256 ||
                pair.Key.Any(character => !char.IsAsciiLetterOrDigit(character) && character is not ('.' or '_')) ||
                !Enum.IsDefined(pair.Value)) ||
            FileDirectory is not null && (string.IsNullOrWhiteSpace(FileDirectory) ||
                !Path.IsPathFullyQualified(FileDirectory) || FileDirectory.Any(char.IsControl)) ||
            MaxFileSizeMiB is < 1 or > 1024 || RetainedFileCount is < 1 or > 1000 ||
            MaxAgeDays is < 1 or > 365 || MaxTotalSizeMiB is < 1 or > 10240 ||
            MaxTotalSizeMiB.HasValue && MaxFileSizeMiB.HasValue && MaxTotalSizeMiB < MaxFileSizeMiB)
            throw new ArgumentException("Invalid server logging options.");

        var arguments = new List<string>();
        Add("Logging:LogLevel:Default", MinimumLevel?.ToString());
        foreach (var (category, level) in CategoryLevels)
            Add($"Logging:LogLevel:{category}", level.ToString());
        Add("Briosa:Logging:ConsoleEnabled", ConsoleEnabled?.ToString());
        Add("Briosa:Logging:File:Enabled", FileEnabled?.ToString());
        Add("Briosa:Logging:File:Directory", FileDirectory);
        Add("Briosa:Logging:File:MaxFileSizeMiB", MaxFileSizeMiB?.ToString(CultureInfo.InvariantCulture));
        Add("Briosa:Logging:File:RetainedFileCount", RetainedFileCount?.ToString(CultureInfo.InvariantCulture));
        Add("Briosa:Logging:File:MaxAgeDays", MaxAgeDays?.ToString(CultureInfo.InvariantCulture));
        Add("Briosa:Logging:File:MaxTotalSizeMiB", MaxTotalSizeMiB?.ToString(CultureInfo.InvariantCulture));
        return [.. arguments];

        void Add(string key, string? value)
        {
            if (value is not null) arguments.Add($"--{key}={value}");
        }
    }
}

/// <summary>Server log severities, in increasing order.</summary>
public enum BriosaLogLevel
{
    /// <summary>Detailed internal events.</summary>
    Trace,
    /// <summary>Diagnostic events.</summary>
    Debug,
    /// <summary>Normal operational events.</summary>
    Information,
    /// <summary>Rejected work and degraded operation.</summary>
    Warning,
    /// <summary>Failures requiring investigation.</summary>
    Error,
    /// <summary>Unrecoverable host failures.</summary>
    Critical,
    /// <summary>Disable the selected category.</summary>
    None
}
