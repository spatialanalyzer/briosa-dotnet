#pragma warning disable CA2000 // Test doubles are disposed through the client session under test.

using Grpc.Core;

namespace Briosa.Client.Tests;

/// <summary>
/// Cleanup is attempted in full. A close that throws never keeps the server from
/// being stopped, never replaces a failed startup's own error, and never leaves
/// the client starting or stopping; a close failure that is the only failure is
/// reported once every close has been attempted. A startup that faults with any
/// exception after the client took ownership leaves its session to the stop or
/// disposal that is waiting for it.
/// </summary>
public sealed partial class StartupBudgetTests
{
    // Before ownership: the 10 ms budget ends during discovery and one close
    // throws. The startup still reports its timeout, both closes are attempted,
    // and the client is no longer starting.
    [Theory]
    [InlineData("transport", true)]
    [InlineData("transport", false)]
    [InlineData("server", true)]
    [InlineData("server", false)]
    public async Task AThrowingCloseBeforeOwnershipStillStopsTheServer(string failing, bool published)
    {
        var clock = new ManualTimeProvider();
        var launcher = new RecordingLauncher();
        var transport = new RecordingTransport
        {
            Clock = clock,
            SnapshotAt = [TimeSpan.FromMilliseconds(55)],
            Bounds = published ? [ServerBounds(startupWithLaunch: 300_000)] : [],
        };
        if (failing == "transport")
        {
            transport.DisposeFailure = new IOException("transport close failed");
        }
        else
        {
            launcher.Server.DisposeFailure = new IOException("server close failed");
        }

        await using var client = CreateClient(new BriosaClientOptions(), transport, clock, launcher);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(
            () => client.StartAsync(new BriosaStartOptions { StartupTimeout = TimeSpan.FromMilliseconds(10) }));

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.True(transport.Disposed);
        Assert.True(launcher.Server.Disposed);
        var notStarted = await Assert.ThrowsAsync<BriosaLifecycleException>(() => client.GetServerSnapshotAsync());
        Assert.Equal("client-not-started", notStarted.DiagnosticCode);
    }

    // After ownership: StopAsync or DisposeAsync attempts both closes when one of
    // them throws, then reports that failure.
    [Theory]
    [InlineData("transport", "stop")]
    [InlineData("transport", "dispose")]
    [InlineData("server", "stop")]
    [InlineData("server", "dispose")]
    public async Task AThrowingCloseAfterOwnershipStillClosesTheRest(string failing, string how)
    {
        var launcher = new RecordingLauncher();
        var transport = new RecordingTransport();
        var client = CreateClient(new BriosaClientOptions(), transport, launcher: launcher);
        await client.StartAsync();
        var failure = new IOException($"{failing} close failed");
        if (failing == "transport")
        {
            transport.DisposeFailure = failure;
        }
        else
        {
            launcher.Server.DisposeFailure = failure;
        }

        var reported = await Assert.ThrowsAsync<IOException>(
            () => how == "stop" ? client.StopAsync() : client.DisposeAsync().AsTask());

        Assert.Same(failure, reported);
        Assert.True(transport.Disposed);
        Assert.True(launcher.Server.Disposed);
        Assert.Contains("stop-sdk", transport.Calls.Select(call => call.Name));
        if (how == "stop")
        {
            // The stop has ended: the client is neither stopping nor started.
            var notStarted = await Assert.ThrowsAsync<BriosaLifecycleException>(() => client.GetServerSnapshotAsync());
            Assert.Equal("client-not-started", notStarted.DiagnosticCode);
        }

        await client.DisposeAsync();
    }

    // An owned startup is held in Launch while the client is disposed or stopped,
    // and Launch then answers without its state: the startup fails with the
    // ArgumentNullException that 0.5.0 reported, and the waiting disposal or stop
    // still closes the transport and the server.
    [Theory]
    [InlineData("dispose")]
    [InlineData("stop")]
    public async Task AnOwnedStartupThatFaultsLeavesItsSessionToTheWaitingStop(string how)
    {
        var reached = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var launcher = new RecordingLauncher();
        var transport = new RecordingTransport
        {
            MissingStateFor = ["launch-sa"],
            DuringCall = async (name, occurrence) =>
            {
                if (name == "launch-sa")
                {
                    reached.TrySetResult();
                    await release.Task.ConfigureAwait(false);
                }
            },
        };
        var client = CreateClient(new BriosaClientOptions(), transport, launcher: launcher);

        var start = client.StartAsync();
        await reached.Task.ConfigureAwait(true);
        var stop = how == "dispose" ? client.DisposeAsync().AsTask() : client.StopAsync();
        release.SetResult();

        await Assert.ThrowsAsync<ArgumentNullException>(() => start);
        await stop.ConfigureAwait(true);
        Assert.True(transport.Disposed);
        Assert.True(launcher.Server.Disposed);
        if (how == "stop")
        {
            await client.DisposeAsync();
        }
    }

    // After a control-plane start, the SDK-state read during shutdown answers
    // without its State, or fails with UNAVAILABLE. Both closes still happen. The
    // read's ArgumentNullException stays the one reported, as in 0.5.0, and
    // UNAVAILABLE stays the best-effort failure that 0.5.0 ignored.
    [Theory]
    [InlineData("missing-state", "stop")]
    [InlineData("missing-state", "dispose")]
    [InlineData("unavailable", "stop")]
    [InlineData("unavailable", "dispose")]
    public async Task AFailedShutdownReadStillClosesTheTransportAndTheServer(string failure, string how)
    {
        var launcher = new RecordingLauncher();
        var transport = new RecordingTransport
        {
            MissingStateFor = failure == "missing-state" ? ["get-sdk-state"] : [],
            DuringCall = (name, occurrence) => failure == "unavailable" && name == "get-sdk-state"
                ? throw new RpcException(new Status(StatusCode.Unavailable, "transport down"))
                : Task.CompletedTask,
        };
        var client = CreateClient(new BriosaClientOptions(), transport, launcher: launcher);
        await client.StartAsync(new BriosaStartOptions
        {
            StartSpatialAnalyzerSdk = false,
            LaunchSpatialAnalyzer = false,
            ConnectToSpatialAnalyzer = false,
        });

        Func<Task> close = how == "stop" ? () => client.StopAsync() : () => client.DisposeAsync().AsTask();
        if (failure == "missing-state")
        {
            await Assert.ThrowsAsync<ArgumentNullException>(close);
        }
        else
        {
            await close();
        }

        Assert.Contains("get-sdk-state", transport.Calls.Select(call => call.Name));
        Assert.True(transport.Disposed);
        Assert.True(launcher.Server.Disposed);
        if (how == "stop")
        {
            await client.DisposeAsync();
        }
    }

    // The budget's timer throws when it is disposed after a timeout before
    // ownership. The startup still reports its timeout, its server is stopped,
    // and the start is no longer pending.
    [Fact]
    public async Task ATimerThatThrowsOnDisposalKeepsTheStartupTimeout()
    {
        var clock = new ManualTimeProvider();
        var launcher = new RecordingLauncher();
        var transport = new RecordingTransport
        {
            Clock = clock,
            SnapshotAt = [TimeSpan.FromMilliseconds(55)],
            Bounds = [ServerBounds(startupWithLaunch: 300_000)],
        };
        await using var client = CreateClient(
            new BriosaClientOptions(),
            transport,
            new TimersThatThrowOnDisposal(clock),
            launcher);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(
            () => client.StartAsync(new BriosaStartOptions { StartupTimeout = TimeSpan.FromMilliseconds(10) }));

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.True(launcher.Server.Disposed);
        var notStarted = await Assert.ThrowsAsync<BriosaLifecycleException>(() => client.GetServerSnapshotAsync());
        Assert.Equal("client-not-started", notStarted.DiagnosticCode);
    }

    [Fact]
    public async Task WithBoundsATimerDisposalFailureLeavesStartupUnpublished()
    {
        var clock = new ManualTimeProvider();
        var throwingClock = new TimersThatThrowOnDisposal(clock);
        var notReady = new RecordingTransport
        {
            Bounds = [ServerBounds(startupWithLaunch: 300_000)],
            ShapeServerInfo = (index, info) =>
            {
                if (index == 1)
                {
                    info.ReadyForMp = false;
                }
            },
        };
        await using (var rejectedClient = CreateClient(new BriosaClientOptions(), notReady, throwingClock))
        {
            var readinessFailure = await Assert.ThrowsAsync<BriosaProtocolException>(() => rejectedClient.StartAsync());
            Assert.Equal("startup-readiness-not-established", readinessFailure.DiagnosticCode);
        }

        var ready = new RecordingTransport { Bounds = [ServerBounds(startupWithLaunch: 300_000)] };
        await using var client = CreateClient(new BriosaClientOptions(), ready, throwingClock);

        var timerFailure = await Assert.ThrowsAsync<IOException>(() => client.StartAsync());
        Assert.Equal("timer disposal failed", timerFailure.Message);

        var commandFailure = await Record.ExceptionAsync(() => client.GetWorkingDirectoryAsync());
        var retryFailure = await Record.ExceptionAsync(() => client.StartAsync());
        Assert.Equal(0, ready.MpCalls);
        Assert.Equal("mp-command-admission-closed", Assert.IsType<BriosaLifecycleException>(commandFailure).DiagnosticCode);
        Assert.Equal("startup-partially-completed", Assert.IsType<BriosaLifecycleException>(retryFailure).DiagnosticCode);
    }

    // The manual clock, except that each timer throws IOException from Dispose
    // after it has disposed itself.
    private sealed class TimersThatThrowOnDisposal(ManualTimeProvider clock) : TimeProvider
    {
        public override long TimestampFrequency => clock.TimestampFrequency;

        public override long GetTimestamp() => clock.GetTimestamp();

        public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period) =>
            new ThrowingTimer(clock.CreateTimer(callback, state, dueTime, period));

        private sealed class ThrowingTimer(ITimer timer) : ITimer
        {
            public bool Change(TimeSpan dueTime, TimeSpan period) => timer.Change(dueTime, period);

#pragma warning disable CA1065 // The test injects exactly this failure.
            public void Dispose()
            {
                timer.Dispose();
                throw new IOException("timer disposal failed");
            }
#pragma warning restore CA1065

            public ValueTask DisposeAsync()
            {
                Dispose();
                return ValueTask.CompletedTask;
            }
        }
    }
}
