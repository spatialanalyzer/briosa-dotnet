namespace Briosa.Client.Tests;

/// <summary>
/// A monotonic test clock. <see cref="Advance"/> moves it and runs every timer
/// that falls due; <see cref="AdvanceWithoutFiringTimers"/> moves it while a due
/// timer's callback has not run yet.
/// </summary>
internal sealed class ManualTimeProvider : TimeProvider
{
    private readonly object _sync = new();
    private readonly List<ManualTimer> _timers = [];
    private long _now;
    private int _readsUntilJump = -1;
    private long _jump;

    public override long TimestampFrequency => TimeSpan.TicksPerSecond;

    public TimeSpan Elapsed
    {
        get
        {
            lock (_sync)
            {
                return TimeSpan.FromTicks(_now);
            }
        }
    }

    public override long GetTimestamp()
    {
        lock (_sync)
        {
            var reading = _now;
            if (_readsUntilJump > 0 && --_readsUntilJump == 0)
            {
                // Time passes between this reading and the next, without
                // running timers: a thread descheduled mid-decision.
                _now += _jump;
                _readsUntilJump = -1;
            }

            return reading;
        }
    }

    // After the next <paramref name="reads"/> clock readings, moves the clock by
    // <paramref name="by"/> without running timers.
    public void AdvanceAfterReads(int reads, TimeSpan by)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(reads, 1);
        lock (_sync)
        {
            _readsUntilJump = reads;
            _jump = by.Ticks;
        }
    }

    // Runs every active timer's callback now, whatever its due time: a timer
    // callback that was already in flight when a later deadline replaced it.
    public void FireActiveTimersEarly()
    {
        ManualTimer[] active;
        lock (_sync)
        {
            active = _timers.Where(timer => timer.DueTicks is not null).ToArray();
            foreach (var timer in active)
            {
                timer.DueTicks = null;
            }
        }

        foreach (var timer in active)
        {
            timer.Fire();
        }
    }

    public override ITimer CreateTimer(
        TimerCallback callback,
        object? state,
        TimeSpan dueTime,
        TimeSpan period)
    {
        var timer = new ManualTimer(this, callback, state);
        timer.Change(dueTime, period);
        lock (_sync)
        {
            _timers.Add(timer);
        }

        FireDueTimers();
        return timer;
    }

    public void Advance(TimeSpan by)
    {
        AdvanceWithoutFiringTimers(by);
        FireDueTimers();
    }

    public void AdvanceWithoutFiringTimers(TimeSpan by)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(by, TimeSpan.Zero);
        lock (_sync)
        {
            _now += by.Ticks;
        }
    }

    // The due time of the one active timer, measured from the clock's origin.
    public TimeSpan? NextDueTime()
    {
        lock (_sync)
        {
            var due = _timers.Where(timer => timer.DueTicks is not null)
                .Select(timer => timer.DueTicks!.Value)
                .DefaultIfEmpty(-1)
                .Min();
            return due < 0 ? null : TimeSpan.FromTicks(due);
        }
    }

    private void FireDueTimers()
    {
        while (true)
        {
            ManualTimer? due;
            lock (_sync)
            {
                due = _timers.FirstOrDefault(timer => timer.DueTicks is { } ticks && ticks <= _now);
                if (due is not null)
                {
                    due.DueTicks = null;
                }
            }

            if (due is null)
            {
                return;
            }

            due.Fire();
        }
    }

    private sealed class ManualTimer(ManualTimeProvider owner, TimerCallback callback, object? state) : ITimer
    {
        public long? DueTicks { get; set; }

        public bool Change(TimeSpan dueTime, TimeSpan period)
        {
            if (period != Timeout.InfiniteTimeSpan && period != TimeSpan.Zero)
            {
                throw new NotSupportedException("Periodic timers are not used by the client.");
            }

            lock (owner._sync)
            {
                DueTicks = dueTime == Timeout.InfiniteTimeSpan ? null : owner._now + dueTime.Ticks;
            }

            return true;
        }

        public void Fire() => callback(state);

        public void Dispose()
        {
            lock (owner._sync)
            {
                DueTicks = null;
                owner._timers.Remove(this);
            }
        }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
