# Briosa .NET v1 public API contract

- Status: Accepted .NET design direction
- Last reviewed: 2026-11-09
- Scope: The target-specific Briosa .NET client packages
- Implementation status: Lifecycle foundation conforming; MP surface expanding

## Purpose and authority

This document records the idiomatic .NET public-API decisions for Briosa v1. It
is the normative design target for the handwritten .NET facade, public domain
types, lifecycle surface, packaging, tests, and documentation.

All first-party clients also implement the authoritative
[client-library behavioral contract](https://github.com/spatialanalyzer/briosa/blob/main/docs/architecture/client-library-behavioral-contract.md).
That central contract owns language-neutral behavior, including MP identity,
presence and fixed defaults, failure semantics, completion ambiguity, replay,
lifecycle safety, runtime ownership, compatibility, capabilities, and shared
conformance. This document owns only their .NET expression and does not repeat
or redefine them.

[SpatialAnalyzer Discussion #6](https://github.com/orgs/spatialanalyzer/discussions/6)
records the completed cross-language review that established this boundary.
The public protobuf contract, handwritten server implementation, runtime
capability registration, and exact target remain authoritative for MP-command
semantics.

## Consumer shape

The ordinary consumer experience should eventually resemble:

~~~csharp
services.AddBriosa(options =>
{
    // Exact configuration remains an architecture decision.
});

public sealed class MainViewModel
{
    private readonly BriosaClient _briosa;

    public MainViewModel(BriosaClient briosa)
    {
        _briosa = briosa;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _briosa.StartAsync(cancellationToken);
    }

    public Task<WorkingFrameProperties> GetWorkingFramePropertiesAsync(
        CancellationToken cancellationToken)
    {
        return _briosa.GetWorkingFramePropertiesAsync(
            cancellationToken: cancellationToken);
    }
}
~~~

`BriosaClient` is registered as a long-lived singleton. It is dormant until
`StartAsync` establishes a verified runtime generation. MP methods are flat,
asynchronous, strongly typed, and recognizable to experienced MP developers.

## Accepted .NET API decisions

### Package and namespace

The first target package is `Briosa.2026.1.0529.7`. Target packages use unique
assembly identities and the stable public namespace `Briosa`.

Generated protobuf and gRPC code is compiled as private transport
implementation. Generated messages, enums, service clients, call objects, and
protocol-identity helpers are not part of the supported NuGet API. A consumer
that needs raw gRPC generates a separate client from the matching published
protocol artifact.

### Facade and command signatures

All MP methods are declared on one handwritten public partial `BriosaClient`
class. Source files may be grouped by MP category, but categories do not appear
in the ordinary call path:

~~~text
BriosaClient.Analysis.cs
BriosaClient.Construction.cs
BriosaClient.File.cs
BriosaClient.Utility.cs
~~~

Each command accepts one ordinary C# parameter for each top-level MP input.
Generated request envelopes never appear in the public signature. A complex
MP-native value remains one parameter represented by a handwritten domain type.

~~~csharp
await briosa.SetPointNotesAsync(
    pointName,
    notes,
    append: true,
    cancellationToken: cancellationToken);
~~~

Every MP method has one optional final `CancellationToken`, preceded by one
optional `onMpCompletion` observer (see [MP completion](#mp-completion-and-partial-success)).
Both are call controls, not MP inputs. A method exposes no per-call gRPC
`Metadata`, `CallOptions`, generated call object, or custom transport option
bag. Because the observer comes first, pass the token by name when you leave
the observer out.

~~~csharp
public Task<string> GetWorkingDirectoryAsync(
    Action<MpCompletion>? onMpCompletion = null,
    CancellationToken cancellationToken = default);
~~~

Remote MP work uses `Task` or `Task<T>`, not `ValueTask`, and has no synchronous
wrapper. There is exactly one public method per MP command.

### C# naming

The central contract defines mechanical command identity. Its C# expression is:

- Convert the exact MP command words to PascalCase without substituting or
  reordering words.
- Retain MP abbreviations but use normal .NET casing, such as `Rms`, `Html`,
  `Sdk`, and `Gdt`.
- Append `Async`.
- Use camelCase parameter names that preserve MP input order, labels, and
  familiar abbreviations as closely as C# permits.
- Record the exact MP command and input labels in XML documentation.

Examples:

~~~text
Construct a Point in Working Coordinates
    -> ConstructAPointInWorkingCoordinatesAsync

Set (or construct) default collection
    -> SetOrConstructDefaultCollectionAsync

Get i-th Collection Name
    -> GetIthCollectionNameAsync
~~~

The exact punctuation, tokenization, reserved-word, and collision algorithm
must be specified and unit-tested before the full command surface is
implemented. V1 provides no aliases.

### Domain values and enums

Public MP-native values and enums are handwritten C# types. Concrete names
follow the MP Editor concept, such as:

~~~csharp
Vector
CollectionObjectName
CollectionItemName
~~~

Immutable record classes are preferred when value semantics fit the MP
concept. Exact MP sentinel values with reviewed domain meaning are represented
by the handwritten type or enum; wire-only sentinels stay private.
`System.Numerics.Vector3` is not the canonical `Vector` because its
single-precision representation may lose MP data.

### Results and nullability

Top-level MP output cardinality maps to C# as follows:

- No output: `Task`.
- One output: `Task<T>`.
- Multiple outputs: `Task<TNamedResult>`.

A multiple-output result is a handwritten immutable sealed record class with a
public constructor and one property per top-level MP output. Properties preserve
MP output order and terminology. Prefer a domain name such as
`WorkingFrameProperties`; use `CommandNameResult` only when no clearer domain
name exists. Do not use tuples, `ref` parameters, `out` parameters, or a
transport-shaped `Response` type.

~~~csharp
public sealed record WorkingFrameProperties
{
    public required CollectionObjectName FrameName { get; init; }

    public required Transform Transform { get; init; }
}
~~~

Nullable reference type annotations express the shared semantic presence
contract. Required-on-success values are non-nullable and required immutable
properties use `required`. An explicitly optional success value uses the
appropriate nullable or domain-specific representation.

### MP completion and partial success

From contract major 3, an MP command that returns code 4 (PARTIAL SUCCESS) did
its work but not its full intended effect. The server reports it as a success,
so the method returns its outputs, never throws for it, and never retries it.
A caller that needs to know passes the optional `onMpCompletion` observer. The
client calls it once, synchronously, before the returned task completes, and
only for a successful call. Its `MpCompletion` value has the state,
`Succeeded` or `PartialSuccess`, and the MP result code. The observer belongs
to that one call, so concurrent calls stay distinguishable.

~~~csharp
MpCompletion? completion = null;
await briosa.SetPointNotesAsync(
    pointName,
    notes,
    onMpCompletion: value => completion = value,
    cancellationToken: cancellationToken);
~~~

Four operations report code 4 as an ordinary `MpFailure`, because their target
documentation says the work was not done: Construct Point From Survey Target
Center, Open SA File, Open Template File, and Set Report Options For Object. A
success response without MP execution details is a protocol error.

### Collections

Ordinary collection inputs use `IEnumerable<T>` with a handwritten element
type:

~~~csharp
public Task DeleteObjectsAsync(
    IEnumerable<CollectionObjectName> objectNames,
    CancellationToken cancellationToken = default);
~~~

The implementation materializes the enumerable once before starting the RPC.
The canonical input shape is not `List<T>`, a `params` array, or
`IAsyncEnumerable<T>`.

An optional collection input is `IEnumerable<T>?` and defaults to `null`. If
its request field is a list wrapper message, `null` leaves the field unset. If
it is a plain repeated field, which has no absent form other than an empty
list, `null` is sent as an empty list. Either way the server, not the client,
decides whether the input may be omitted.

Collection outputs use fresh detached `T[]` values. A required empty output is
`Array.Empty<T>()`; nullable arrays are used only when the semantic contract
distinguishes absence from present-but-empty. Generated repeated fields and
`IReadOnlyList<T>` are not canonical v1 result shapes.

### Fixed defaults

A reviewed fixed value that is representable as a C# compile-time constant is
an ordinary optional parameter. The client sends the effective value
explicitly.

~~~csharp
public Task SetPointNotesAsync(
    CollectionObjectName pointName,
    string notes,
    bool append = true,
    CancellationToken cancellationToken = default);
~~~

A reviewed non-constant fixed value is exposed as a named immutable value of
the same MP-native type, such as `ProjectionOptions.Default`. Option-like
values may support nondestructive mutation with C# `with` expressions.

No default fills in a value the server requires. Such a parameter is required
and has no default, so omitting it is a compile error, and the client never
sends an empty value, `0`, or `false` for an argument the caller left out. A
named value can still make an explicit choice easy: `ToleranceVectorOptions.NoLimits`
disables every limit when a caller passes it as a required relationship
tolerance or constraint.

### Exceptions

The handwritten .NET exception boundary uses:

- `BriosaOperationException` for a valid typed Briosa operation failure;
- `BriosaTransportException` for a transport failure without a valid typed
  operation error;
- conventional `ArgumentException` subclasses for invalid local input;
- `OperationCanceledException` semantics for caller cancellation; and
- dedicated lifecycle, startup, and compatibility exceptions for failures
  before MP submission.

`BriosaOperationException` preserves every server failure dimension:
`Kind`, `ExecutionDisposition`, `RecoveryGuidance`, `ReplayGuidance`,
`ReplaySafety`, and, when the failure relates to the MP command, `MpExecution`
with the MP state, the result code, and each output's retrieval. Every
`OperationFailureKind` is a distinct member: `SdkCallFaulted` reports an SDK call
that threw, with its phase in `ExecutionDisposition`. The lifecycle exceptions'
kinds include `CallerCancelled` and `CallerDeadlineExceeded`, but a cancelled
call surfaces as `OperationCanceledException`, never as a state conflict.

Generated error messages and gRPC exceptions remain private implementation
details. An underlying diagnostic may be retained as `InnerException` without
making the transport type part of the supported contract. Exact exception
properties and lifecycle exception names remain deferred.

### Construction, lifetime, and cleanup

The primary constructor accepts a handwritten `BriosaClientOptions` value and
performs no external I/O. Configuration is effectively immutable after
construction.

The same `BriosaClient` instance may be used concurrently and across multiple
start/stop generations. The .NET lifecycle surface includes:

~~~csharp
public Task StartAsync(
    CancellationToken cancellationToken = default);

public Task StopAsync(
    CancellationToken cancellationToken = default);
~~~

The client implements `IAsyncDisposable` for final cleanup. `StopAsync` remains
the reusable, diagnostic shutdown path; asynchronous disposal delegates to the
same ownership rules. The exact DI registration API and supported runtime modes
remain deferred.

`BriosaClientOptions` may expose a nullable client-wide command timeout. `null`
means that the client adds no deadline. A caller that needs a one-off time limit
uses `CancellationTokenSource`; startup has separate timeout and cancellation
settings.

### Startup and lifecycle time budgets

`BriosaStartOptions.StartupTimeout` is one overall budget for the whole
`StartAsync`, whichever phases are selected. It defaults to two minutes. When it
expires, `StartAsync` throws `BriosaStartupException` with diagnostic
`startup-timeout`, whichever phase it interrupts, including an RPC in flight.
Cancelling the `CancellationToken` passed to `StartAsync` only stops that caller
from waiting and surfaces as `OperationCanceledException`; the start continues
until it completes or its budget expires.

The default follows the server's default lifecycle bounds
([spatialanalyzer/briosa#298](https://github.com/spatialanalyzer/briosa/pull/298),
[#315](https://github.com/spatialanalyzer/briosa/pull/315)) for the ordinary
start:

| Startup phase | Server bound (default) | Worst case until the server answers |
| --- | --- | --- |
| Launch the owned server and wait for discovery | none; server host startup | a few seconds |
| Start the SDK | `Briosa:Worker:StartupTimeout` (10 s), then 5 s cleanup on failure | 10 s (15 s on failure) |
| Launch SpatialAnalyzer | `Briosa:SpatialAnalyzer:ApplicationStartupTimeout` (30 s) | 30 s |
| Connect and prove readiness | ConnectEx and the application association, each under `Briosa:Worker:StartupTimeout` (10 s); the readiness probe under `Briosa:Worker:ReadinessProbeTimeout` (30 s); a 5 s liveness check; 5 s cleanup on failure | 55 s (60 s on failure) |
| Total | | 95 s to readiness, 100 s to a typed failure |

Two minutes leaves 20 seconds for server host startup, process observation,
installation discovery, and round trips. Attaching to a running SpatialAnalyzer
skips the 30-second launch. A heavily loaded machine can slow process
observation further until
[spatialanalyzer/briosa#317](https://github.com/spatialanalyzer/briosa/issues/317)
lands.

When the server's lifecycle bounds are raised, size the client budget to at
least three times `StartupTimeout`, plus `ReadinessProbeTimeout`, plus
`ApplicationStartupTimeout` when launching, plus 10 s, plus a margin. For
example, a server `StartupTimeout` of 30 s and `ReadinessProbeTimeout` of 60 s
with the default 30 s application startup need at least 90 + 60 + 30 + 10 =
190 s, or 210 s (3.5 minutes) with the 20-second margin.

Lifecycle methods take only a `CancellationToken`. The client adds no per-call
lifecycle deadline, and `CommandTimeout` applies only to MP commands.

When a caller stops waiting during an SDK start, connection, or recovery, a
contract-major-3 server
([spatialanalyzer/briosa#315](https://github.com/spatialanalyzer/briosa/pull/315))
finishes an exchange it has already accepted under its own bounds, whether the
caller stopped waiting through the startup budget or a cancellation. A request
cancelled before acceptance changes nothing. A second call meanwhile fails with
`sdk-lifecycle-transition-in-progress`; refresh state and retry later, and stop
a generation you own only after the accepted transition has ended. After a
SpatialAnalyzer launch times out, refresh application state before launching
again. The shared `lifecycle-cancellation` conformance scenario checks this for
Connect.

Refresh SDK state before deciding what to do next. Before MP work, refresh the
server snapshot too: MP admission needs both the snapshot and the SDK to report
readiness.

A per-call deadline shorter than the server's worst case for that call can stop
the caller waiting before the server's typed failure, lifecycle state, and
recovery guidance arrive. Under the default server configuration, use at least
the deadlines below. Each is the server's worst case to a typed answer, failure
cleanup included, plus 5 s for scheduling, transport, and an idle process
observation. Allow more on a heavily loaded machine.

| Call | Server worst case to a typed answer | Minimum per-call deadline |
| --- | --- | --- |
| Start the SDK | 15 s: the 10 s startup bound, then up to 5 s of worker cleanup on failure | 20 s |
| Connect or reconnect | 60 s: ConnectEx 10 s, readiness probe 30 s, application association 10 s, and liveness check 5 s, then up to 5 s of worker cleanup on failure; a process observation comes first | 65 s |
| Recover the SDK | 25 s: heartbeat drain 5 s, cleanup of the faulted worker 5 s, and replacement start 10 s, then up to 5 s of worker cleanup on failure | 30 s |
| Launch SpatialAnalyzer | 30 s: the application startup bound, after a process observation | 35 s |
| Close the owned SpatialAnalyzer | 30 s: the application shutdown bound, with up to two process observations | 35 s |
| Stop the SDK | the execution budget of any in-flight MP command, then 15 s: heartbeat drain 5 s, stop exchange 5 s, and forced cleanup 5 s | that budget + 20 s |

When the server's bounds are raised, add the increase: `StartupTimeout` counts
once for Start and Recover and twice for Connect, `ReadinessProbeTimeout` once
for Connect, and the application startup and shutdown bounds once for Launch and
Close.

### Discovery and admission

`BriosaServerSnapshot` reports the server's admission profile and opaque policy
fingerprint, which callers compare only for equality. Each
`BriosaOperationCapability` carries its reviewed risk flags (an ordered list,
empty when no risk applies), duration class, validation status, and admission,
exactly as the server reports them. A value from a later revision maps to the
enum's `Unknown` member instead of failing discovery. The client never
recomputes admission and never pre-validates request options.
`GetDeniedOperationsAsync` is a diagnostic listing of the denied operations
and their denial reasons; it never adds them to the snapshot.

### Public API verification

The accepted public surface is protected by API-surface tests. Mapping and
validation tests cover handwritten domain values, request construction, result
detachment, nullable presence, exceptions, and the C# naming algorithm.
Language-neutral behavior is exercised through the shared target-specific
conformance host; this repository owns only its thin .NET fixture.

## Implemented Lifecycle Foundation

The package now uses dormant construction, explicit reusable asynchronous
lifecycle, asynchronous disposal, private generated transport, handwritten
public states and exceptions, cancellation tokens, exact-target compatibility
checks, and detached MP results required by this contract. Subsequent vertical
slices add handwritten MP methods without changing this lifecycle foundation.

## Deferred .NET decisions

The following choices require focused architecture or implementation review:

- The exact hierarchy and public interfaces around `BriosaClient` and advanced
  lifecycle components.
- The exact default and advanced `StartAsync` signatures.
- The final `BriosaClientOptions` properties and validation rules.
- The DI registration and factory API.
- Detailed lifecycle, startup, compatibility, and transport exception names and
  properties.
- State-change notification and UI-binding mechanisms.
- Whether the concrete client also needs a public consumer interface; mocking
  convenience alone is not sufficient justification.
- The exact command-name normalization algorithm for punctuation, tokenization,
  reserved words, and collisions.
- Multi-target `extern alias` guidance, deferred until a second real target
  package exists.

The central lifecycle modes and ownership state machine are tracked by
[spatialanalyzer/briosa#147](https://github.com/spatialanalyzer/briosa/issues/147).
The shared conformance host and scenario contract are tracked by
[spatialanalyzer/briosa#148](https://github.com/spatialanalyzer/briosa/issues/148).
Their outcomes must be expressed idiomatically here without duplicating their
language-neutral policy.

## .NET v1 non-goals

- No source-generated idiomatic facade, public domain model, or documentation.
- No generated request envelopes or generated transport types in the supported
  NuGet API.
- No synchronous MP wrappers or internal sync-over-async.
- No `ValueTask` for remote MP commands.
- No callback alternative, aliases, or MP convenience-overload matrix.
- No tuples, `ref`, or `out` for multiple MP outputs.
- No `params` or `IAsyncEnumerable<T>` canonical collection-input surface.
- No `System.Numerics.Vector3` substitution for the MP-native `Vector`.
- No public API that requires consumers to catch `RpcException` or inspect gRPC
  details.

Language-neutral v1 non-goals are defined only in the
[shared behavioral contract](https://github.com/spatialanalyzer/briosa/blob/main/docs/architecture/client-library-behavioral-contract.md).

## Planning boundary

[Issue #3](https://github.com/spatialanalyzer/briosa-dotnet/issues/3) is the v1
client epic and is consistent with this contract. Implementation work must
resolve the relevant deferred decisions and replace bootstrap behavior through
focused, reviewable issues. A local implementation must not silently redefine
the central behavioral contract.

## Implemented targets

Independent SA `2024.1.0508.5` and `2026.1.0529.7` products live under
`targets/`. See the [repository guide](../../README.md) for package selection;
the target README records signature differences and validation limits.
