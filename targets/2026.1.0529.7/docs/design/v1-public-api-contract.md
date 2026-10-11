# Briosa .NET v1 public API contract

- Status: Accepted .NET design direction
- Last reviewed: 2026-08-12
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
        return _briosa.GetWorkingFramePropertiesAsync(cancellationToken);
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
    cancellationToken);
~~~

Every MP method has one optional final `CancellationToken`. It exposes no
per-call gRPC `Metadata`, `CallOptions`, generated call object, or custom
transport option bag.

~~~csharp
public Task<string> GetWorkingDirectoryAsync(
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

### Exceptions

The handwritten .NET exception boundary uses:

- `BriosaOperationException` for a valid typed Briosa operation failure;
- `BriosaTransportException` for a transport failure without a valid typed
  operation error;
- conventional `ArgumentException` subclasses for invalid local input;
- `OperationCanceledException` semantics for caller cancellation; and
- dedicated lifecycle, startup, and compatibility exceptions for failures
  before MP submission.

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

Released 0.9 servers and the pinned protocol build `89b59aa` have no larger
defaults: their startup bound is a fixed 10 s, and their readiness probe uses the
quick execution budget (30 s by default), so the default covers them too.

When the server's lifecycle bounds are raised, size the client budget to at
least three times `StartupTimeout`, plus `ReadinessProbeTimeout`, plus
`ApplicationStartupTimeout` when launching, plus 10 s, plus a margin. For
example, a server `StartupTimeout` of 30 s and `ReadinessProbeTimeout` of 60 s
with the default 30 s application startup need at least 90 + 60 + 30 + 10 =
190 s, or 210 s (3.5 minutes) with the 20-second margin.

Lifecycle methods take only a `CancellationToken`. The client adds no per-call
lifecycle deadline, and `CommandTimeout` applies only to MP commands.

What happens on the server when a caller stops waiting during an SDK start,
connection, or recovery depends on the server build:

- Builds that contain
  [spatialanalyzer/briosa#315](https://github.com/spatialanalyzer/briosa/pull/315)
  (briosa main from `0c7ff7a`, planned for the contract-major-3 release) finish
  an exchange they have accepted under their own bounds, whether the caller
  stopped waiting through the startup budget or a cancellation.
- Older supported builds, including every released 0.9 server and the pinned
  protocol build `89b59aa`, cancel the exchange with the caller. A cancelled SDK
  start, or the replacement start inside a recovery, terminates the new worker
  and leaves the SDK stopped (`worker-startup-cancelled`). A cancelled ConnectEx
  retires the generation, which then needs recovery (`connect-ex-cancelled`). A
  cancelled readiness probe quarantines the generation and requires operator
  recovery (`execution-readiness-probe-cancelled`).

Either way, refresh SDK state before deciding what to do next, and keep any
per-call deadline at or above the server's worst case for that call. Server
worst cases at the default bounds: start the SDK 10 s; connect or reconnect 55 s;
recover 20 s; launch SpatialAnalyzer 30 s; close it 30 s; stop the SDK waits for
any in-flight MP command's execution budget, then up to 10 s.

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
