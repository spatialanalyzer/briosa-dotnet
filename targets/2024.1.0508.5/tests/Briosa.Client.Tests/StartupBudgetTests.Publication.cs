#pragma warning disable CA2000 // Test doubles are disposed through the client session under test.

using System.Diagnostics;
using System.Reflection;

namespace Briosa.Client.Tests;

/// <summary>
/// With published lifecycle bounds, a startup that was rejected never admits MP
/// commands, however its session's state is refreshed afterwards, and the
/// post-Connect refresh is mapped and validated before the last budget check, so
/// the time mapping takes counts and a late mapping error is the timeout's cause.
/// A reply whose state is missing is part of that translation. Without published
/// bounds, each of these stays as in client 0.5.0.
/// </summary>
public sealed partial class StartupBudgetTests
{
    // Connect answers with success at 321 s against an adopted 320 s deadline:
    // both callers see the timeout, and no state refresh afterwards, alone or in
    // order, reopens MP admission. Diagnostics stay available.
    [Theory]
    [InlineData("snapshot")]
    [InlineData("sdk-state")]
    [InlineData("sdk-state,snapshot")]
    [InlineData("snapshot,sdk-state")]
    [InlineData("connect,snapshot")]
    public async Task WithBoundsAStateRefreshAfterARejectedStartKeepsMpAdmissionClosed(string reads)
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = [ServerBounds(startupWithLaunch: 300_000)],
            DuringCall = MoveClockDuring(clock, "connect-sdk", 1, TimeSpan.FromSeconds(321), fireTimers: true),
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var first = client.StartAsync();
        var second = client.StartAsync();

        Assert.Equal("startup-timeout", (await Assert.ThrowsAsync<BriosaStartupException>(() => first)).DiagnosticCode);
        Assert.Equal("startup-timeout", (await Assert.ThrowsAsync<BriosaStartupException>(() => second)).DiagnosticCode);
        await AssertMpAdmissionClosedAsync(client, transport);
        ArgumentNullException.ThrowIfNull(reads);
        foreach (var read in reads.Split(','))
        {
            await RefreshAsync(client, read);
        }

        await AssertMpAdmissionClosedAsync(client, transport);
    }

    // The Server 0.10.0 counterpart, as in client 0.5.0: a start that failed after
    // the client took ownership leaves a session whose state refreshes decide
    // admission, so once both are ready MP commands are admitted again.
    [Fact]
    public async Task WithoutBoundsAStateRefreshAfterAFailedStartReopensMpAdmissionAsInZeroFiveZero()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            HangAt = "snapshot",
            HangAtOccurrence = 2,
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var start = client.StartAsync();
        await transport.Reached.Task.ConfigureAwait(true);
        clock.Advance(TimeSpan.FromSeconds(120));

        Assert.Equal("startup-timeout", (await Assert.ThrowsAsync<BriosaStartupException>(() => start)).DiagnosticCode);
        await AssertMpAdmissionClosedAsync(client, transport);
        await RefreshAsync(client, "snapshot");
        await AssertMpAdmittedAsync(client, transport);
    }

    // On the real clock: the refresh answers just before a 1.5 s deadline, and
    // mapping its 80,000 capabilities carries the startup past it. The check after
    // mapping rejects the startup; nothing is published or admitted.
    [Fact]
    public async Task WithBoundsMappingTheRefreshPastTheDeadlineRejectsTheStartup()
    {
        var started = Stopwatch.GetTimestamp();
        var transport = new RecordingTransport
        {
            Bounds = [ServerBounds(startupWithLaunch: 160_000)],
            ConnectedCapabilityCount = 80_000,
            BeforeAnswer = index =>
            {
                if (index == 1)
                {
                    var answerAt = TimeSpan.FromMilliseconds(1_499);
                    var wait = answerAt - Stopwatch.GetElapsedTime(started) - TimeSpan.FromMilliseconds(30);
                    if (wait > TimeSpan.Zero)
                    {
                        Thread.Sleep(wait);
                    }

                    while (Stopwatch.GetElapsedTime(started) < answerAt)
                    {
                        Thread.SpinWait(64);
                    }
                }
            },
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(
            () => client.StartAsync(new BriosaStartOptions { StartupTimeout = TimeSpan.FromMilliseconds(1_500) }));

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        await AssertMpAdmissionClosedAsync(client, transport);
    }

    // The controllable-clock counterpart: the refresh's reply arrives at 321 s,
    // after its submission checks passed, and the timer callback has not run.
    [Fact]
    public async Task WithBoundsARefreshAnsweredAfterTheDeadlineIsNeverPublished()
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = [ServerBounds(startupWithLaunch: 300_000)],
            BeforeAnswer = index =>
            {
                if (index == 1)
                {
                    clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(321) - clock.Elapsed);
                }
            },
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var exception = await Assert.ThrowsAsync<BriosaStartupException>(() => client.StartAsync());

        Assert.Equal("startup-timeout", exception.DiagnosticCode);
        Assert.Equal(FullStartup, transport.Calls.Select(call => call.Name));
        await AssertMpAdmissionClosedAsync(client, transport);
    }

    // A refresh without server version information that answers after the
    // deadline: its mapping error is the timeout's cause. Before the deadline, and
    // without bounds, the error is the one 0.5.0 reported.
    [Theory]
    [InlineData(321, true, true)]
    [InlineData(300, true, false)]
    [InlineData(321, false, false)]
    public async Task ALateMalformedRefreshKeepsItsMappingErrorAsTheCause(int seconds, bool published, bool timeout)
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = published ? [ServerBounds(startupWithLaunch: 300_000)] : [],
            ShapeServerInfo = (index, info) =>
            {
                if (index == 1)
                {
                    info.Version = null;
                }
            },
            BeforeAnswer = index =>
            {
                if (index == 1)
                {
                    clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(seconds) - clock.Elapsed);
                }
            },
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var exception = await Assert.ThrowsAnyAsync<BriosaException>(() => client.StartAsync());

        var mapping = timeout
            ? Assert.IsType<BriosaCompatibilityException>(
                Assert.IsType<BriosaStartupException>(exception).InnerException)
            : Assert.IsType<BriosaCompatibilityException>(exception);
        Assert.Equal("server-version-missing", mapping.DiagnosticCode);
        if (timeout)
        {
            Assert.Equal("startup-timeout", ((BriosaStartupException)exception).DiagnosticCode);
        }
    }

    // A reply that arrives without its State submessage once the deadline has
    // passed, before the timer callback runs, is the timeout, with the mapping's
    // ArgumentNullException as its cause. Before the deadline it stays the
    // ArgumentNullException that 0.5.0 reported.
    [Theory]
    [InlineData("start-sdk", 321, true)]
    [InlineData("launch-sa", 321, true)]
    [InlineData("connect-sdk", 321, true)]
    [InlineData("start-sdk", 300, false)]
    [InlineData("launch-sa", 300, false)]
    [InlineData("connect-sdk", 300, false)]
    public async Task WithBoundsALateReplyWithoutStateIsTheStartupTimeout(string phase, int seconds, bool pastDeadline)
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = [ServerBounds(startupWithLaunch: 300_000)],
            MissingStateFor = [phase],
            DuringCall = MoveClockDuring(clock, phase, 1, TimeSpan.FromSeconds(seconds), fireTimers: false),
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        var exception = await Assert.ThrowsAnyAsync<Exception>(() => client.StartAsync());

        if (pastDeadline)
        {
            var timeout = Assert.IsType<BriosaStartupException>(exception);
            Assert.Equal("startup-timeout", timeout.DiagnosticCode);
            Assert.IsType<ArgumentNullException>(timeout.InnerException);
            await AssertMpAdmissionClosedAsync(client, transport);
        }
        else
        {
            Assert.IsType<ArgumentNullException>(exception);
        }
    }

    // The Server 0.10.0 counterpart: the same late reply is the
    // ArgumentNullException that 0.5.0 reported.
    [Theory]
    [InlineData("start-sdk")]
    [InlineData("launch-sa")]
    [InlineData("connect-sdk")]
    public async Task WithoutBoundsALateReplyWithoutStateKeepsItsArgumentNullException(string phase)
    {
        var clock = new ManualTimeProvider();
        var transport = new RecordingTransport
        {
            Clock = clock,
            MissingStateFor = [phase],
            DuringCall = MoveClockDuring(clock, phase, 1, TimeSpan.FromSeconds(121), fireTimers: false),
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);

        await Assert.ThrowsAsync<ArgumentNullException>(() => client.StartAsync());
    }

    // With bounds, the client's gate is held across the deadline: the refresh is
    // mapped at 319 s against a 320 s deadline and the startup then waits for the
    // gate, behind which an MP call is queued. The startup ends in its timeout, and
    // the queued MP call never reaches the transport.
    [Fact]
    public async Task WithBoundsAGateHeldAcrossTheDeadlineNeverLetsTheStartupPublish()
    {
        var clock = new ManualTimeProvider();
        SemaphoreSlim? gate = null;
        var holding = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var transport = new RecordingTransport
        {
            Clock = clock,
            Bounds = [ServerBounds(startupWithLaunch: 300_000)],
            BeforeAnswer = index =>
            {
                if (index == 1)
                {
                    clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(319) - clock.Elapsed);
                    gate!.Wait();
                    holding.TrySetResult();
                }
            },
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);
        gate = Gate(client);

        var start = client.StartAsync();
        await holding.Task.ConfigureAwait(true);
        await Task.Delay(100).ConfigureAwait(true);
        var queued = client.GetWorkingDirectoryAsync();
        clock.Advance(TimeSpan.FromSeconds(321) - clock.Elapsed);
        gate.Release();

        Assert.Equal("startup-timeout", (await Assert.ThrowsAsync<BriosaStartupException>(() => start)).DiagnosticCode);
        await Assert.ThrowsAsync<BriosaLifecycleException>(() => queued);
        Assert.Equal(0, transport.MpCalls);
        await AssertMpAdmissionClosedAsync(client, transport);
    }

    // The Server 0.10.0 counterpart, as in client 0.5.0: the startup publishes
    // after its refresh at 119 s and then waits for the held gate; once the gate
    // is released at 121 s it succeeds and MP commands are admitted.
    [Fact]
    public async Task WithoutBoundsAGateHeldAcrossTheDeadlineLetsThePublishedStartupFinishAsInZeroFiveZero()
    {
        var clock = new ManualTimeProvider();
        SemaphoreSlim? gate = null;
        var holding = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var transport = new RecordingTransport
        {
            Clock = clock,
            BeforeAnswer = index =>
            {
                if (index == 1)
                {
                    clock.AdvanceWithoutFiringTimers(TimeSpan.FromSeconds(119) - clock.Elapsed);
                    gate!.Wait();
                    holding.TrySetResult();
                }
            },
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);
        gate = Gate(client);

        var start = client.StartAsync();
        await holding.Task.ConfigureAwait(true);
        await Task.Delay(100).ConfigureAwait(true);
        clock.Advance(TimeSpan.FromSeconds(121) - clock.Elapsed);
        gate.Release();

        await start.ConfigureAwait(true);
        await AssertMpAdmittedAsync(client, transport);
    }

    [Fact]
    public async Task WithoutBoundsTheStartupTokenCancelsWhileTheFinalGateIsHeld()
    {
        var clock = new ManualTimeProvider();
        var refreshing = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var reply = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var transport = new RecordingTransport
        {
            DuringCall = (name, occurrence) =>
            {
                if (name != "snapshot" || occurrence != 2)
                {
                    return Task.CompletedTask;
                }

                refreshing.SetResult();
                return reply.Task;
            },
        };
        await using var client = CreateClient(new BriosaClientOptions(), transport, clock);
        var timeout = TimeSpan.FromSeconds(2);
        var start = client.StartAsync(new BriosaStartOptions { StartupTimeout = timeout });
        await refreshing.Task.WaitAsync(TimeSpan.FromSeconds(10));
        var startupToken = transport.Calls[^1].Token;
        var gate = Gate(client);
        await gate.WaitAsync();
        Task queued;
        try
        {
            reply.SetResult();
            // No other call is queued yet. Wait for the startup's final gate
            // wait so the deadline falls after the ready reply was processed.
            var waitQueue = typeof(SemaphoreSlim).GetField("m_asyncHead", BindingFlags.NonPublic | BindingFlags.Instance)!;
            Assert.True(await Task.Run(() => SpinWait.SpinUntil(
                () => waitQueue.GetValue(gate) is not null, TimeSpan.FromSeconds(10))));
            queued = client.GetWorkingDirectoryAsync();
            clock.Advance(timeout + TimeSpan.FromMilliseconds(1));
        }
        finally
        {
            gate.Release();
        }

        await start.WaitAsync(TimeSpan.FromSeconds(10));
        await Assert.ThrowsAsync<NotSupportedException>(() => queued);
        Assert.Equal(1, transport.MpCalls);
        Assert.True(startupToken.IsCancellationRequested);
    }

    private static async Task RefreshAsync(BriosaClient client, string read)
    {
        switch (read)
        {
            case "snapshot":
                Assert.True((await client.GetServerSnapshotAsync().ConfigureAwait(false)).ReadyForMp);
                break;
            case "sdk-state":
                Assert.True((await client.GetSpatialAnalyzerSdkStateAsync().ConfigureAwait(false)).ReadyForMp);
                break;
            case "connect":
                Assert.True((await client.ConnectToSpatialAnalyzerAsync().ConfigureAwait(false)).ReadyForMp);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(read));
        }
    }
}
