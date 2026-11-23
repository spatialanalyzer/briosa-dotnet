#pragma warning disable CA2000 // Test doubles are disposed through the client session under test.

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
}
