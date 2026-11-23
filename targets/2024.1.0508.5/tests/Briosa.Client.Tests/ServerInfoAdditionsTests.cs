using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

/// <summary>
/// Lifecycle bounds and the configured interaction mode from Server 0.10.1
/// (spatialanalyzer/briosa#400), and the minimum revision that keeps Server
/// 0.10.0 compatible (briosa#397 and #400, decision Q3/Q4).
/// </summary>
public sealed class ServerInfoAdditionsTests
{
    [Fact]
    public void PublishedLifecycleBoundsAreOptionalDurations()
    {
        var server = ServerInfo(revision: 1);
        server.LifecycleBounds = new Transport.LifecycleBounds
        {
            StartSdkMilliseconds = 45_000,
            LaunchSpatialAnalyzerMilliseconds = 30_000,
            ConnectSpatialAnalyzerMilliseconds = 60_000,
            RecoverSdkMilliseconds = 55_000,
            StopSdkMilliseconds = 15_000,
            CloseOwnedSpatialAnalyzerMilliseconds = 30_001,
            StartupWithLaunchMilliseconds = 100_000,
            StartupWithoutLaunchMilliseconds = 70_000,
            HostGracefulShutdown = new Transport.GracefulShutdownBound { FiniteMilliseconds = 105_000 },
        };
        server.ConfiguredSpatialAnalyzerInteractionMode = Transport.ConfiguredSpatialAnalyzerInteractionMode.Silent;

        var snapshot = ProtocolMapping.MapSnapshot(server, Capabilities());

        var bounds = Assert.IsType<LifecycleBounds>(snapshot.LifecycleBounds);
        Assert.Equal(TimeSpan.FromSeconds(45), bounds.StartSdk);
        Assert.Equal(TimeSpan.FromSeconds(30), bounds.LaunchSpatialAnalyzer);
        Assert.Equal(TimeSpan.FromSeconds(60), bounds.ConnectToSpatialAnalyzer);
        Assert.Equal(TimeSpan.FromSeconds(55), bounds.RecoverSdk);
        Assert.Equal(TimeSpan.FromSeconds(15), bounds.StopSdk);
        Assert.Equal(TimeSpan.FromMilliseconds(30_001), bounds.CloseOwnedSpatialAnalyzer);
        Assert.Equal(TimeSpan.FromSeconds(100), bounds.StartupWithLaunch);
        Assert.Equal(TimeSpan.FromSeconds(70), bounds.StartupWithoutLaunch);
        Assert.Equal(GracefulShutdownBoundKind.Finite, bounds.HostGracefulShutdown.Kind);
        Assert.Equal(TimeSpan.FromSeconds(105), bounds.HostGracefulShutdown.Limit);
        Assert.Equal(ConfiguredSpatialAnalyzerInteractionMode.Silent, snapshot.ConfiguredInteractionMode);
    }

    // Server 0.10.0: no bounds and an UNSPECIFIED mode.
    [Fact]
    public void WithoutPublishedBoundsTheSnapshotSaysSo()
    {
        var snapshot = ProtocolMapping.MapSnapshot(ServerInfo(revision: 0), Capabilities());

        Assert.Null(snapshot.LifecycleBounds);
        Assert.Equal(ConfiguredSpatialAnalyzerInteractionMode.Unknown, snapshot.ConfiguredInteractionMode);
    }

    // An absent or zero allowance is unavailable, never unbounded, and leaves
    // every other allowance usable.
    [Fact]
    public void AnAbsentOrZeroAllowanceIsUnavailableOnItsOwn()
    {
        var server = ServerInfo(revision: 1);
        server.LifecycleBounds = new Transport.LifecycleBounds
        {
            StartSdkMilliseconds = 0,
            StartupWithLaunchMilliseconds = 100_000,
        };

        var bounds = ProtocolMapping.MapSnapshot(server, Capabilities()).LifecycleBounds!;

        Assert.Null(bounds.StartSdk);
        Assert.Null(bounds.LaunchSpatialAnalyzer);
        Assert.Null(bounds.ConnectToSpatialAnalyzer);
        Assert.Null(bounds.RecoverSdk);
        Assert.Null(bounds.StopSdk);
        Assert.Null(bounds.CloseOwnedSpatialAnalyzer);
        Assert.Null(bounds.StartupWithoutLaunch);
        Assert.Equal(TimeSpan.FromSeconds(100), bounds.StartupWithLaunch);
        Assert.Equal(GracefulShutdownBoundKind.Unavailable, bounds.HostGracefulShutdown.Kind);
        Assert.Null(bounds.HostGracefulShutdown.Limit);
    }

    [Fact]
    public void TheLargestAllowanceTheProtocolCarriesIsPreserved()
    {
        var server = ServerInfo(revision: 1);
        server.LifecycleBounds = new Transport.LifecycleBounds { StartupWithLaunchMilliseconds = uint.MaxValue };

        var bounds = ProtocolMapping.MapSnapshot(server, Capabilities()).LifecycleBounds!;

        Assert.Equal(TimeSpan.FromMilliseconds(uint.MaxValue), bounds.StartupWithLaunch);
    }

    // Read from the limit case: absent, unset, zero, or too large to represent
    // is unavailable; only the explicit case is unbounded.
    [Theory]
    [InlineData("absent", GracefulShutdownBoundKind.Unavailable, null)]
    [InlineData("unset", GracefulShutdownBoundKind.Unavailable, null)]
    [InlineData("zero", GracefulShutdownBoundKind.Unavailable, null)]
    [InlineData("finite", GracefulShutdownBoundKind.Finite, 4_294_967_294L)]
    [InlineData("huge", GracefulShutdownBoundKind.Unavailable, null)]
    [InlineData("unbounded", GracefulShutdownBoundKind.Unbounded, null)]
    public void TheHostShutdownBoundIsReadFromItsLimitCase(
        string shape,
        GracefulShutdownBoundKind expectedKind,
        long? expectedMilliseconds)
    {
        var server = ServerInfo(revision: 1);
        server.LifecycleBounds = new Transport.LifecycleBounds { StartupWithLaunchMilliseconds = 100_000 };
        server.LifecycleBounds.HostGracefulShutdown = shape switch
        {
            "absent" => null,
            "unset" => new Transport.GracefulShutdownBound(),
            "zero" => new Transport.GracefulShutdownBound { FiniteMilliseconds = 0 },
            "finite" => new Transport.GracefulShutdownBound { FiniteMilliseconds = 4_294_967_294 },
            "huge" => new Transport.GracefulShutdownBound { FiniteMilliseconds = ulong.MaxValue },
            _ => new Transport.GracefulShutdownBound { Unbounded = new Transport.UnboundedShutdownWait() },
        };

        var bounds = ProtocolMapping.MapSnapshot(server, Capabilities()).LifecycleBounds!;

        Assert.Equal(expectedKind, bounds.HostGracefulShutdown.Kind);
        Assert.Equal(
            expectedMilliseconds is { } milliseconds ? TimeSpan.FromMilliseconds(milliseconds) : null,
            bounds.HostGracefulShutdown.Limit);
        // Fall back per purpose: the startup allowance stays usable.
        Assert.Equal(TimeSpan.FromSeconds(100), bounds.StartupWithLaunch);
    }

    // Configuration only, mapped by name. UNSPECIFIED and unknown numbers are Unknown.
    [Theory]
    [InlineData(Transport.ConfiguredSpatialAnalyzerInteractionMode.Unspecified, ConfiguredSpatialAnalyzerInteractionMode.Unknown)]
    [InlineData(Transport.ConfiguredSpatialAnalyzerInteractionMode.Unchanged, ConfiguredSpatialAnalyzerInteractionMode.Unchanged)]
    [InlineData(Transport.ConfiguredSpatialAnalyzerInteractionMode.Silent, ConfiguredSpatialAnalyzerInteractionMode.Silent)]
    [InlineData(Transport.ConfiguredSpatialAnalyzerInteractionMode.Automatic, ConfiguredSpatialAnalyzerInteractionMode.Automatic)]
    [InlineData(Transport.ConfiguredSpatialAnalyzerInteractionMode.Manual, ConfiguredSpatialAnalyzerInteractionMode.Manual)]
    [InlineData((Transport.ConfiguredSpatialAnalyzerInteractionMode)42, ConfiguredSpatialAnalyzerInteractionMode.Unknown)]
    [InlineData((Transport.ConfiguredSpatialAnalyzerInteractionMode)(-1), ConfiguredSpatialAnalyzerInteractionMode.Unknown)]
    public void TheConfiguredInteractionModeHasAnUnknownMember(
        Transport.ConfiguredSpatialAnalyzerInteractionMode wire,
        ConfiguredSpatialAnalyzerInteractionMode expected)
    {
        var server = ServerInfo(revision: 1);
        server.ConfiguredSpatialAnalyzerInteractionMode = wire;

        var snapshot = ProtocolMapping.MapSnapshot(server, Capabilities());

        Assert.Equal(expected, snapshot.ConfiguredInteractionMode);
        // An unknown mode never makes the bounds unavailable.
        Assert.Null(snapshot.LifecycleBounds);
    }

    [Fact]
    public void TheClientRequiresRevisionZeroWhileTheArtifactDeclaresOne()
    {
        Assert.Equal(1U, Transport.BriosaProtocolIdentity.CompatibilityRevision);
        Assert.Equal(0U, ServerSelectionPolicy.MinimumCompatibilityRevision);
    }

    // Live discovery: Server 0.10.0 declares 3.0 and Server 0.10.1 declares 3.1.
    [Theory]
    [InlineData(0U)]
    [InlineData(1U)]
    [InlineData(2U)]
    public void LiveDiscoveryAcceptsEveryRevisionOfMajorThree(uint revision)
    {
        var snapshot = ProtocolMapping.MapSnapshot(ServerInfo(revision), Capabilities());

        Assert.Equal("0.10.1", snapshot.BriosaVersion);
    }

    // Installation selection: a 3.0 and a 3.1 installation are both eligible,
    // and the newest compatible release wins.
    [Fact]
    public void SelectionAcceptsInstallationsDeclaringRevisionZeroOrOne()
    {
        var released = Installation("released", "0.10.0", revision: 0);
        var patch = Installation("patch", "0.10.1", revision: 1);

        Assert.Equal("released", ServerSelectionPolicy.Select([released], new BriosaServerSelection()).Selected?.InstallationId);
        Assert.Equal("patch", ServerSelectionPolicy.Select([patch], new BriosaServerSelection()).Selected?.InstallationId);
        Assert.Equal("patch", ServerSelectionPolicy.Select([released, patch], new BriosaServerSelection()).Selected?.InstallationId);
        Assert.Equal(
            "released",
            ServerSelectionPolicy.Select(
                [released, patch],
                new BriosaServerSelection { MaximumVersionExclusive = "0.10.1" }).Selected?.InstallationId);
        Assert.True(ServerSelectionPolicy.Compatible(3, 0, "0.10.0", new string('a', 40)));
        Assert.True(ServerSelectionPolicy.Compatible(3, 1, "0.10.1", new string('a', 40)));
    }

    // Live coordinates must still match the selected installation's manifest.
    [Theory]
    [InlineData(0U, 0U, true)]
    [InlineData(1U, 1U, true)]
    [InlineData(1U, 0U, false)]
    [InlineData(0U, 1U, false)]
    public void LiveCoordinatesMustMatchTheSelectedInstallation(uint live, uint manifest, bool matches)
    {
        var server = ServerInfo(live);
        var installation = Installation("selected", "0.10.1", manifest);

        if (matches)
        {
            BriosaProtocolCompatibility.ValidateInstallation(server, installation);
        }
        else
        {
            var failure = Assert.Throws<BriosaCompatibilityException>(
                () => BriosaProtocolCompatibility.ValidateInstallation(server, installation));
            Assert.Equal("server-installation-contract-mismatch", failure.DiagnosticCode);
        }
    }

    private static BriosaInstallation Installation(string id, string version, uint revision) =>
        new(id, $@"C:\Briosa\{id}\Briosa.Server.exe", version, new string('a', 40),
            Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget, "win-x64", 3, revision,
            new string(id[0], 64), "user");

    private static Transport.GetServerInfoResponse ServerInfo(uint revision) => new()
    {
        Version = new Transport.VersionCoordinates
        {
            BriosaVersion = "0.10.1",
            SourceRevision = new string('a', 40),
            ProtocolPackage = Transport.BriosaProtocolIdentity.ProtocolPackage,
            SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
        },
        Compatibility = new Transport.CompatibilityContract { Major = 3, Revision = revision },
        TargetIsolationMode = Transport.TargetIsolationMode.SingleTenant,
    };

    private static Transport.ListCapabilitiesResponse Capabilities() => new()
    {
        ProtocolPackage = Transport.BriosaProtocolIdentity.ProtocolPackage,
        SpatialAnalyzerTarget = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
        AdmissionProfile = "standard",
        PolicyFingerprint = "sha256:" + new string('b', 64),
    };
}
