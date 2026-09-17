namespace Briosa.Client.Tests;

public sealed class LoggingOptionsTests
{
    [Fact]
    public void StartupOptionsProduceOnlyValidatedServerArguments()
    {
        var options = new BriosaLoggingOptions
        {
            MinimumLevel = BriosaLogLevel.Debug,
            CategoryLevels = new Dictionary<string, BriosaLogLevel> { ["Microsoft"] = BriosaLogLevel.Error },
            ConsoleEnabled = false,
            FileEnabled = true,
            FileDirectory = Path.GetFullPath("logs with spaces"),
            MaxFileSizeMiB = 4,
            RetainedFileCount = 3,
            MaxAgeDays = 2,
            MaxTotalSizeMiB = 12
        };
        var args = options.ToArguments();
        Assert.Contains("--Logging:LogLevel:Default=Debug", args);
        Assert.Contains("--Logging:LogLevel:Microsoft=Error", args);
        Assert.Contains("--Briosa:Logging:ConsoleEnabled=False", args);
        Assert.Contains("--Briosa:Logging:File:Directory=" + options.FileDirectory, args);
        Assert.Contains("--Briosa:Logging:File:MaxTotalSizeMiB=12", args);
        Assert.Empty(new BriosaLoggingOptions().ToArguments());
        Assert.Single(new BriosaLoggingOptions { MaxFileSizeMiB = 512 }.ToArguments());
        Assert.Throws<ArgumentException>(() => (options with { MaxTotalSizeMiB = 1 }).ToArguments());
        Assert.Throws<ArgumentException>(() => (options with { FileDirectory = "relative" }).ToArguments());
        Assert.Throws<ArgumentException>(() => (options with { RetainedFileCount = 0 }).ToArguments());
        Assert.Throws<ArgumentException>(() => (options with
        {
            CategoryLevels = new Dictionary<string, BriosaLogLevel> { ["Default:Injected"] = BriosaLogLevel.Trace }
        }).ToArguments());
    }
}
