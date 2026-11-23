#pragma warning disable CA2000 // Test doubles are disposed through the client session under test.

using System.Reflection;
using Grpc.Core;
using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

/// <summary>
/// With published lifecycle bounds, a startup is decided against the clock before
/// each lifecycle submission and before readiness is accepted and published, so a
/// late timer callback or a reply that arrives after cancellation never lets it
/// succeed past its deadline. Without published bounds (Server 0.10.0), the
/// startup runs exactly as in client 0.5.0. A startup that ends before its session
/// is published stops the server it started.
/// </summary>
public sealed partial class StartupBudgetTests
{
    private static readonly string[] FullStartup = ["snapshot", "start-sdk", "launch-sa", "connect-sdk", "snapshot"];

    // (a) A 10 ms explicit budget, discovery at 55 ms, and a timer callback that
    // has not run: the startup ends before any phase and stops its server.
    [Fact]
    public async Task WithBoundsAnExplicitBudgetEndsAtTheClockBeforeItsTimerRuns()
    {
        var clock = new ManualTimeProvider();
        var launcher = new RecordingLauncher();
        var transport = new RecordingTransport
        {
            Clock = clock,
            SnapshotAt = [TimeSpan.FromMilliseconds(55)],
            FireTimersAtSnapshot = false,
            Bounds = [ServerBounds(startupWithLaunch: 300_000)],
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock, launcher);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(
            () => client.StartAsync(new BriosaStartOptions { StartupTimeout = TimeSpan.FromMilliseconds(10) }));

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.Equal(["snapshot"], transport.Calls.Select(call => call.Name));
        Assert.True(launcher.Server.Disposed);
        Assert.True(transport.Disposed);
    }

    // The Server 0.10.0 counterpart: as in client 0.5.0, only the timer ends the
    // startup, so a timer callback that has not run changes nothing.
    [Fact]
    public async Task WithoutBoundsALateTimerCallbackChangesNothingAsInZeroFiveZero()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            SnapshotAt = [TimeSpan.FromMilliseconds(55)],
            FireTimersAtSnapshot = false,
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        await client.StartAsync(new BriosaStartOptions { StartupTimeout = TimeSpan.FromMilliseconds(10) });

        Assert.Equal(FullStartup, transport.Calls.Select(call => call.Name));
    }

    // (b) Partial bounds keep the 120 s bootstrap deadline; (c) full bounds adopt
    // 320 s. The clock passes the deadline during one phase while the timer
    // callback has not run: no later phase starts, and MP admission stays closed.
    [Theory]
    [InlineData("start-sdk", 1, false)]
    [InlineData("launch-sa", 1, false)]
    [InlineData("connect-sdk", 1, false)]
    [InlineData("snapshot", 2, false)]
    [InlineData("start-sdk", 1, true)]
    [InlineData("launch-sa", 1, true)]
    [InlineData("connect-sdk", 1, true)]
    [InlineData("snapshot", 2, true)]
    public async Task WithBoundsTheDeadlinePassingDuringAPhaseEndsTheStartup(string phase, int occurrence, bool adopted)
    {
        var clock = new ManualTimeProvider();
        var bounds = ServerBounds(startupWithLaunch: 300_000);
        if (!adopted)
        {
            bounds.ClearStartupWithLaunchMilliseconds();
        }

        var pastDeadline = TimeSpan.FromSeconds(adopted ? 321 : 121);
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = [bounds],
            DuringCall = MoveClockDuring(clock, phase, occurrence, pastDeadline, fireTimers: false),
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(() => client.StartAsync());

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.Equal(FullStartup[..(PhaseIndex(phase, occurrence) + 1)], transport.Calls.Select(call => call.Name));
        Assert.Equal(1, transport.CapabilitySubmissions);
        await AssertMpAdmissionClosedAsync(client, transport);
    }

    // The Server 0.10.0 counterpart: the same clock movement changes nothing.
    [Theory]
    [InlineData("start-sdk", 1)]
    [InlineData("launch-sa", 1)]
    [InlineData("connect-sdk", 1)]
    [InlineData("snapshot", 2)]
    public async Task WithoutBoundsTheClockAloneNeverEndsTheStartup(string phase, int occurrence)
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            DuringCall = MoveClockDuring(clock, phase, occurrence, TimeSpan.FromSeconds(121), fireTimers: false),
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        await client.StartAsync();

        Assert.Equal(FullStartup, transport.Calls.Select(call => call.Name));
    }

    // (d) The budget is cancelled while Connect is in flight, and the transport
    // still answers with success: readiness is never accepted or published.
    [Fact]
    public async Task WithBoundsACancellationDuringConnectKeepsReadinessUnpublished()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = [ServerBounds(startupWithLaunch: 300_000)],
            DuringCall = MoveClockDuring(clock, "connect-sdk", 1, TimeSpan.FromSeconds(320), fireTimers: true),
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(() => client.StartAsync());

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.Equal(FullStartup[..4], transport.Calls.Select(call => call.Name));
        Assert.True(transport.Calls[^1].Token.IsCancellationRequested);
        await AssertMpAdmissionClosedAsync(client, transport);
    }

    // The Server 0.10.0 counterpart: as in client 0.5.0, the cancelled token is
    // the only signal, so the next call carries it and the transport decides.
    [Fact]
    public async Task WithoutBoundsACancellationDuringConnectIsLeftToTheTransport()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            DuringCall = MoveClockDuring(clock, "connect-sdk", 1, TimeSpan.FromSeconds(120), fireTimers: true),
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        await client.StartAsync();

        Assert.Equal(FullStartup, transport.Calls.Select(call => call.Name));
        Assert.True(transport.Calls[^1].Token.IsCancellationRequested);
    }

    // A timeout while the startup waits to publish its session (here the client's
    // gate is held) stops the server it started, with or without bounds. Client
    // 0.5.0 left that process running, even after DisposeAsync.
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ATimeoutBeforeTheSessionIsPublishedStopsTheServer(bool published)
    {
        var clock = new ManualTimeProvider();
        var launcher = new RecordingLauncher();
        SemaphoreSlim? gate = null;
        var holding = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = published ? [ServerBounds(startupWithLaunch: 300_000)] : [],
            DuringCall = async (name, occurrence) =>
            {
                if (name == "snapshot" && occurrence == 1)
                {
                    await gate!.WaitAsync().ConfigureAwait(false);
                    holding.TrySetResult();
                }
            },
        };
        var client = CreateClient(new BriosaClientOptions(), transport, clock, launcher);
        gate = Gate(client);

        var start = client.StartAsync();
        await holding.Task.ConfigureAwait(true);
        await Task.Delay(100);
        clock.Advance(TimeSpan.FromSeconds(published ? 321 : 121));
        // The startup stops its server before it needs the gate again; only then
        // is the gate released, so the cancelled wait cannot still be granted.
        await WaitUntilAsync(() => launcher.Server.Disposed);
        gate.Release();

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(() => start);
        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.Equal(["snapshot"], transport.Calls.Select(call => call.Name));
        Assert.True(launcher.Server.Disposed);
        Assert.True(transport.Disposed);
        await client.DisposeAsync();
    }

    // The 10 ms budget whose timer fires normally during discovery: the startup
    // ends there and stops its server, with or without bounds.
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ATimerThatFiresDuringDiscoveryStopsTheServer(bool published)
    {
        var clock = new ManualTimeProvider();
        var launcher = new RecordingLauncher();
        var transport = new RecordingTransport
        {
            Clock = clock,
            SnapshotAt = [TimeSpan.FromMilliseconds(55)],
            Bounds = published ? [ServerBounds(startupWithLaunch: 300_000)] : [],
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock, launcher);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(
            () => client.StartAsync(new BriosaStartOptions { StartupTimeout = TimeSpan.FromMilliseconds(10) }));

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.Equal(["snapshot"], transport.Calls.Select(call => call.Name));
        Assert.True(launcher.Server.Disposed);
        Assert.True(transport.Disposed);
    }

    // Disposing the client while the startup runs: the startup ends, its server
    // is stopped, and DisposeAsync completes.
    [Fact]
    public async Task DisposingDuringStartupStopsTheServer()
    {
        var launcher = new RecordingLauncher();
        var reached = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var transport = new RecordingTransport
        {
            DuringCall = async (name, occurrence) =>
            {
                if (name == "snapshot" && occurrence == 1)
                {
                    reached.TrySetResult();
                    await release.Task.ConfigureAwait(false);
                }
            },
        };
        var client = CreateClient(new BriosaClientOptions(), transport, launcher: launcher);

        var start = client.StartAsync();
        await reached.Task.ConfigureAwait(true);
        var dispose = client.DisposeAsync().AsTask();
        release.TrySetResult();

        await Assert.ThrowsAsync<ObjectDisposedException>(() => start);
        await dispose.ConfigureAwait(true);
        Assert.True(launcher.Server.Disposed);
        Assert.True(transport.Disposed);
    }

    // A startup that fails after its session is published keeps that session, as
    // in client 0.5.0: MP admission stays closed, and stopping or disposing the
    // client stops the server.
    [Fact]
    public async Task ATimeoutAfterTheSessionIsPublishedLeavesTheServerToStop()
    {
        var clock = new ManualTimeProvider();
        var launcher = new RecordingLauncher();
        var bounds = ServerBounds(startupWithLaunch: 300_000);
        bounds.ClearStartupWithLaunchMilliseconds();
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = [bounds],
            DuringCall = MoveClockDuring(clock, "start-sdk", 1, TimeSpan.FromSeconds(121), fireTimers: false),
        };
        var client = CreateClient(new BriosaClientOptions(), transport, clock, launcher);

        await Assert.ThrowsAsync<BriosaStartupException>(() => client.StartAsync());
        Assert.False(launcher.Server.Disposed);

        await client.DisposeAsync();
        Assert.True(launcher.Server.Disposed);
        Assert.True(transport.Disposed);
    }

    // Client 0.5.0 rejected an explicit budget over uint.MaxValue - 1 whole
    // milliseconds with ArgumentOutOfRangeException("delay") before launch. That
    // stays; the client no longer keeps that failed start, so it can start again
    // and dispose.
    [Theory]
    [InlineData(42_949_672_950_000L)]
    [InlineData(long.MaxValue)]
    public async Task AnOversizedExplicitBudgetFailsAsInZeroFiveZero(long ticks)
    {
        var transport = new RecordingTransport();
        var launcher = new RecordingLauncher();
        var client = CreateClient(new BriosaClientOptions(), transport, launcher: launcher);

        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => client.StartAsync(new BriosaStartOptions { StartupTimeout = TimeSpan.FromTicks(ticks) }));

        Assert.Equal("delay", exception.ParamName);
        Assert.Equal("Specified argument was out of the range of valid values. (Parameter 'delay')", exception.Message);
        Assert.Empty(transport.Calls);
        await client.StartAsync();
        await client.DisposeAsync();
        Assert.True(launcher.Server.Disposed);
    }

    // As in client 0.5.0, validation rejects a budget that is not positive.
    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    [InlineData(-10_000L)]
    [InlineData(long.MinValue)]
    public async Task ANonPositiveExplicitBudgetFailsValidationAsInZeroFiveZero(long ticks)
    {
        var transport = new RecordingTransport();
        await using var client = CreateClient(new BriosaClientOptions(), transport);

        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => client.StartAsync(new BriosaStartOptions { StartupTimeout = TimeSpan.FromTicks(ticks) }));

        Assert.Equal("StartupTimeout", exception.ParamName);
        Assert.Empty(transport.Calls);
    }

    // The largest budget that 0.5.0 accepted, including a fraction of a
    // millisecond over it, is accepted.
    [Theory]
    [InlineData(42_949_672_940_000L)]
    [InlineData(42_949_672_949_000L)]
    public async Task TheLargestAcceptedExplicitBudgetStillStarts(long ticks)
    {
        var transport = new RecordingTransport();
        await using var client = CreateClient(new BriosaClientOptions(), transport);

        await client.StartAsync(new BriosaStartOptions { StartupTimeout = TimeSpan.FromTicks(ticks) });

        Assert.Equal(FullStartup, transport.Calls.Select(call => call.Name));
    }

    // (5) A phase that fails after the deadline, before the timer callback runs,
    // reports the startup timeout with the failure as its cause; before the
    // deadline the failure keeps its own error.
    [Theory]
    [InlineData(181, true)]
    [InlineData(170, false)]
    public async Task WithBoundsALateLaunchFailureIsTheStartupTimeout(int seconds, bool pastDeadline)
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = [ServerBounds(startupWithLaunch: 160_000)],
            DuringCall = (name, occurrence) =>
            {
                if (name == "launch-sa")
                {
                    clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(seconds) - clock.Elapsed);
                    throw new RpcException(new Status(StatusCode.Unavailable, "transport down"));
                }

                return Task.CompletedTask;
            },
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var exception = await Assert.ThrowsAnyAsync<BriosaException>(() => client.StartAsync());

        if (pastDeadline)
        {
            var timeout = Assert.IsType<BriosaStartupException>(exception);
            Assert.Equal("startup-timeout", timeout.DiagnosticCode);
            Assert.Equal(StatusCode.Unavailable, Assert.IsType<RpcException>(timeout.InnerException).StatusCode);
        }
        else
        {
            Assert.Equal("transport-unavailable", Assert.IsType<BriosaTransportException>(exception).DiagnosticCode);
        }
    }

    // The same for a malformed reply: an application state the client cannot read.
    [Theory]
    [InlineData(181, true)]
    [InlineData(170, false)]
    public async Task WithBoundsALateMalformedReplyIsTheStartupTimeout(int seconds, bool pastDeadline)
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = [ServerBounds(startupWithLaunch: 160_000)],
            LaunchReply = new Transport.SpatialAnalyzerLifecycleState
            {
                ApplicationState = (Transport.SpatialAnalyzerApplicationState)99,
            },
            DuringCall = MoveClockDuring(clock, "launch-sa", 1, TimeSpan.FromSeconds(seconds), fireTimers: false),
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var exception = await Assert.ThrowsAnyAsync<BriosaException>(() => client.StartAsync());

        if (pastDeadline)
        {
            var timeout = Assert.IsType<BriosaStartupException>(exception);
            Assert.Equal("startup-timeout", timeout.DiagnosticCode);
            Assert.Equal("application-state", Assert.IsType<BriosaProtocolException>(timeout.InnerException).DiagnosticCode);
        }
        else
        {
            Assert.Equal("application-state", Assert.IsType<BriosaProtocolException>(exception).DiagnosticCode);
        }
    }

    // The Server 0.10.0 counterpart: a late failure keeps its own error, as in 0.5.0.
    [Fact]
    public async Task WithoutBoundsALateLaunchFailureKeepsItsOwnError()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            DuringCall = (name, occurrence) =>
            {
                if (name == "launch-sa")
                {
                    clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(121));
                    throw new RpcException(new Status(StatusCode.Unavailable, "transport down"));
                }

                return Task.CompletedTask;
            },
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var exception = await Assert.ThrowsAsync<BriosaTransportException>(() => client.StartAsync());

        Assert.Equal("transport-unavailable", exception.DiagnosticCode);
    }

    // (6) The check runs at the submission itself, after the request is built: a
    // request that took until past the deadline to build is never submitted.
    [Fact]
    public async Task WithBoundsASubmissionAfterTheDeadlineIsNeverSent()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = [ServerBounds(startupWithLaunch: 160_000)],
            WhileBuildingRequest = (name, occurrence) =>
            {
                if (name == "launch-sa")
                {
                    clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(181) - clock.Elapsed);
                }
            },
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(() => client.StartAsync());

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.Equal(FullStartup[..2], transport.Calls.Select(call => call.Name));
        await AssertMpAdmissionClosedAsync(client, transport);
    }

    // (6) Between the refresh's two discovery RPCs: server information that answers
    // at 181 s submits no capability listing.
    [Fact]
    public async Task WithBoundsTheRefreshSubmitsNoCapabilityListingAfterTheDeadline()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = [ServerBounds(startupWithLaunch: 160_000)],
            DuringCall = MoveClockDuring(clock, "snapshot", 2, TimeSpan.FromSeconds(181), fireTimers: false),
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(() => client.StartAsync());

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.Equal(FullStartup, transport.Calls.Select(call => call.Name));
        Assert.Equal(1, transport.CapabilitySubmissions);
        await AssertMpAdmissionClosedAsync(client, transport);
    }

    // The Server 0.10.0 counterpart: nothing checks submissions, as in 0.5.0.
    [Fact]
    public async Task WithoutBoundsSubmissionsAreNotChecked()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            WhileBuildingRequest = (name, occurrence) =>
            {
                if (name == "launch-sa")
                {
                    clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(121));
                }
            },
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        await client.StartAsync();

        Assert.Equal(FullStartup, transport.Calls.Select(call => call.Name));
        Assert.Equal(2, transport.CapabilitySubmissions);
    }

    // An MP command issued at once after a startup ended by its timeout fails in
    // the client and never reaches the transport.
    private static async Task AssertMpAdmissionClosedAsync(BriosaClient client, RecordingTransport transport)
    {
        var closed = await Assert.ThrowsAsync<BriosaLifecycleException>(() => client.GetWorkingDirectoryAsync())
            .ConfigureAwait(false);
        Assert.Equal("mp-command-admission-closed", closed.DiagnosticCode);
        var operation = await Assert.ThrowsAsync<BriosaLifecycleException>(() => client.GetActiveCollectionNameAsync())
            .ConfigureAwait(false);
        Assert.Equal("mp-command-admission-closed", operation.DiagnosticCode);
        Assert.Equal(0, transport.MpCalls);
    }

    private static async Task WaitUntilAsync(Func<bool> condition)
    {
        using var limit = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        while (!condition())
        {
            await Task.Delay(10, limit.Token).ConfigureAwait(false);
        }
    }

    private static int PhaseIndex(string phase, int occurrence) =>
        phase == "snapshot" ? (occurrence == 1 ? 0 : 4) : Array.IndexOf(FullStartup, phase);

    private static Func<string, int, Task> MoveClockDuring(
        ManualTimeProvider clock,
        string phase,
        int occurrence,
        TimeSpan to,
        bool fireTimers) =>
        (name, count) =>
        {
            if (name == phase && count == occurrence && to > clock.Elapsed)
            {
                if (fireTimers)
                {
                    clock.Advance(to - clock.Elapsed);
                }
                else
                {
                    clock.AdvanceWithoutFiringTimers(to - clock.Elapsed);
                }
            }

            return Task.CompletedTask;
        };

    private static SemaphoreSlim Gate(BriosaClient client) =>
        (SemaphoreSlim)typeof(BriosaClient)
            .GetField("_gate", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(client)!;
}
