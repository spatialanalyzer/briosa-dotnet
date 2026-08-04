# Briosa .NET v1 public API contract

- Status: Accepted design direction
- Last reviewed: 2026-08-04
- Scope: The target-specific Briosa .NET client packages
- Implementation status: Not yet implemented

## Purpose

This document records the foundational public-API decisions accepted for Briosa
.NET v1. It is the design target for the handwritten .NET façade, lifecycle
surface, packaging work, tests, and documentation.

The current repository is an earlier bootstrap and does not yet conform to this
contract. In particular, its README, generated-type visibility, request-shaped
wrapper, package identity, and GitHub issue #3 must be reviewed against this
document before implementation begins.

Shared protocol and MP-command semantics remain authoritative in
[spatialanalyzer/briosa](https://github.com/spatialanalyzer/briosa). This
document owns the idiomatic .NET consumer experience. It must not silently
invent shared behavior that belongs in the server or protocol project.

The cross-language applicability of these principles is being reviewed in
[SpatialAnalyzer Discussion #6](https://github.com/orgs/spatialanalyzer/discussions/6).

## Design summary

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

The same BriosaClient singleton exists for the lifetime of the application. It
is dormant until StartAsync establishes a verified runtime generation. MP
commands are handwritten, flat, asynchronous, strongly typed, and recognizable
to experienced MP developers.

## Rule 1: Use one flat, handwritten BriosaClient façade

All MP-command methods are declared on one public partial BriosaClient class:

~~~csharp
await briosa.GetWorkingDirectoryAsync(cancellationToken);
await briosa.GetWorkingFramePropertiesAsync(cancellationToken);
~~~

Source files may be grouped by MP category for maintainability, but categories
do not appear in the normal call path:

~~~text
BriosaClient.Analysis.cs
BriosaClient.Construction.cs
BriosaClient.File.cs
BriosaClient.Utility.cs
~~~

The v1 façade is handwritten. Briosa-specific source generation must not create
the client class, its MP methods, domain types, mappings, result types, or
documentation. Standard protobuf and gRPC code generation remains required.
Façade generation may be reconsidered only after the handwritten v1 contract is
stable.

## Rule 2: Do not expose request envelopes

An MP command method exposes one ordinary C# parameter for each top-level MP
input. Generated request messages do not appear in the consumer API.

Do:

~~~csharp
await briosa.SetPointNotesAsync(
    pointName,
    notes,
    append: true,
    cancellationToken);
~~~

Do not:

~~~csharp
await briosa.SetPointNotesAsync(
    new SetPointNotesRequest
    {
        // Generated transport fields.
    },
    cancellationToken);
~~~

A complex MP-native value remains one semantic parameter represented by a
handwritten domain type. Protocol-only controls stay outside the MP-shaped
parameter list. Each handwritten method maps its parameters into the generated
request internally.

## Rule 3: Preserve MP parameter identity

C# parameter names preserve the MP Editor input labels and abbreviations as
closely as the language permits. Preserve input order.

Examples include names such as:

~~~csharp
double rmsTol
double maxAbsTolerance
bool genEvent
~~~

Do not expand familiar MP abbreviations merely to make a name more verbose.
Duplicate MP input labels are not expected. If an actual technical collision is
found, use the smallest necessary C# clarification and retain the exact MP label
in XML documentation.

## Rule 4: Use handwritten public domain types and enums

Use a built-in .NET type only when it represents the MP type exactly and
losslessly. Otherwise, expose a handwritten public type named after the MP
Editor concept:

~~~csharp
Vector
CollectionObjectName
CollectionItemName
~~~

Do not invent replacement concepts that do not correspond to SA-native types.
For example, a parameter that accepts Collection Object Name or Collection Item
Name must use those types rather than a convenience type such as
WorkingFrameSelection.

V1 uses concrete handwritten types and concrete handwritten enums. Mapping into
generated protobuf types is explicit. Exact MP sentinel values must be
preserved. System.Numerics.Vector3 is not the canonical Vector because its
single-precision representation may lose MP data.

## Rule 5: Keep transport controls outside MP semantics

Every asynchronous MP method has one optional final CancellationToken:

~~~csharp
public Task<string> GetWorkingDirectoryAsync(
    CancellationToken cancellationToken = default);
~~~

The primary v1 MP methods do not expose per-call gRPC Metadata, CallOptions,
generated call objects, or custom transport option bags.

BriosaClientOptions may expose a client-wide optional command timeout. Its
default is null, meaning the client imposes no additional deadline. A caller
that needs a one-off time limit uses CancellationTokenSource.

Cancellation only stops the caller from waiting. It does not prove that an
in-flight SpatialAnalyzer operation was rolled back or did not execute.

Startup has a separate timeout and cancellation policy because launching an
application and establishing readiness is not an MP command.

## Rule 6: Keep generated APIs private to the package implementation

Generated protobuf and gRPC types are implementation details of the idiomatic
Briosa .NET package. They are not its supported public NuGet API.

Consumers that want to use raw gRPC generate their own client from the exact
target-specific protocol artifact published by spatialanalyzer/briosa. The
language-specific package does not need to serve both the idiomatic façade and
the raw protocol audience through one supported API surface.

## Rule 7: Match return cardinality to MP outputs

Return shapes follow the top-level MP outputs:

- No output: Task.
- One output: Task&lt;T&gt;.
- Multiple outputs: Task&lt;TNamedResult&gt;.

Do not use tuples, ref parameters, or out parameters.

~~~csharp
public Task SetPointNotesAsync(...);

public Task<string> GetWorkingDirectoryAsync(...);

public Task<WorkingFrameProperties>
    GetWorkingFramePropertiesAsync(...);
~~~

Ordinary success values do not contain transport execution metadata.

## Rule 8: Use named immutable records for multiple outputs

A multiple-output result is a handwritten immutable sealed record class with
one property per top-level MP output. Preserve MP output order and terminology.

~~~csharp
public sealed record WorkingFrameProperties
{
    public required CollectionObjectName FrameName { get; init; }

    public required Transform Transform { get; init; }
}
~~~

Prefer a domain-concept name such as WorkingFrameProperties. Use
CommandNameResult only when no better domain name exists. Avoid Response
because the public result is not a transport response. Result types have public
constructors and contain no gRPC status, trailers, or generated messages.

## Rule 9: Derive nullability from the semantic output contract

Public nullability follows the semantic success contract declared by Briosa:

- A value required on success is non-nullable.
- A value explicitly optional on success uses a nullable or domain-specific
  representation.
- A required value missing from a successful wire response is detected and
  rejected by the client.

Do not infer semantic presence from protobuf defaults alone. If the server
contract is ambiguous, clarify or fix the shared contract instead of guessing
in .NET.

## Rule 10: Accept collection inputs as IEnumerable&lt;T&gt;

Collection inputs use IEnumerable&lt;T&gt; with the correct handwritten element type:

~~~csharp
public Task DeleteObjectsAsync(
    IEnumerable<CollectionObjectName> objectNames,
    CancellationToken cancellationToken = default);
~~~

The client:

- Rejects a null required collection.
- Enumerates the input exactly once.
- Materializes it before starting the RPC.
- Rejects null elements when the element contract does not permit null.
- Defers empty-collection validity to the command contract.

The primary v1 API does not use List&lt;T&gt;, params arrays, or IAsyncEnumerable&lt;T&gt;
for ordinary MP collection inputs.

## Rule 11: Return collection outputs as arrays

Collection outputs are fully mapped and detached into T[] before the task
completes:

~~~csharp
CollectionObjectName[] names =
    await briosa.GetCollectionNamesAsync(cancellationToken);
~~~

A required empty result is Array.Empty&lt;T&gt;(), never null. Use a nullable array
only if the semantic contract distinguishes absent from present-but-empty.
Generated repeated-field collections and IReadOnlyList&lt;T&gt; are not the canonical
v1 result shape.

## Rule 12: Expose reviewed compile-time defaults as optional parameters

When the locked Briosa command contract declares a reviewed, fixed,
compile-time-representable MP default, expose it as an ordinary C# optional
parameter:

~~~csharp
public Task SetPointNotesAsync(
    CollectionObjectName pointName,
    string notes,
    bool append = true,
    CancellationToken cancellationToken = default);
~~~

Inputs without a reviewed fixed default remain required. The client sends the
effective value explicitly so behavior is visible and stable. ObjectiveSA is
secondary evidence; the exact Briosa target contract is authoritative.

A default change is a behavioral public-API change. The matching raw server may
retain the same fallback for direct gRPC callers, but the canonical default is
versioned in the shared command contract and presented idiomatically by each
client.

## Rule 13: Represent fixed non-constant defaults with named domain values

If a real MP input has a reviewed fixed default that cannot be expressed as a C#
optional-parameter constant, represent it as a named immutable value of the
same MP-native type:

~~~csharp
await briosa.SomeCommandAsync(
    options: ProjectionOptions.Default,
    cancellationToken);
~~~

Option-like values may support immutable editing:

~~~csharp
ProjectionOptions options =
    ProjectionOptions.Default with
    {
        // Deliberate changes.
    };
~~~

A domain-wide default belongs on the domain type. A command-specific default
uses a command-specific name. Do not disguise the default as null, default(T),
or a replacement type unrelated to the MP input.

## Rule 14: Do not add MP convenience overloads in v1

Expose exactly one public method per MP command. V1 does not reproduce the
large convenience-overload surface maintained by ObjectiveSA.

Modern collection expressions keep singleton collection calls concise:

~~~csharp
await briosa.SomeCommandAsync(
    vectorGroups: [vectorGroup],
    cancellationToken);
~~~

Overloads may be reconsidered after v1 only when real usage demonstrates enough
value to justify the maintenance cost. This rule applies to MP command methods;
small infrastructure APIs may be shaped according to their actual lifecycle
needs.

## Rule 15: Derive MP method names mechanically

An MP method name is mechanically derived from the exact MP command name:

- Preserve every word in its original order, including articles,
  conjunctions, and prepositions.
- Remove punctuation according to one documented normalization algorithm.
- Apply deterministic C# identifier casing.
- Retain MP abbreviations rather than expanding them.
- Append Async.
- Never substitute synonyms, reorder words, or apply discretionary grammatical
  cleanup.

Examples:

~~~text
Construct a Point in Working Coordinates
    -> ConstructAPointInWorkingCoordinatesAsync

Set (or construct) default collection
    -> SetOrConstructDefaultCollectionAsync

Get i-th Collection Name
    -> GetIthCollectionNameAsync
~~~

Use normal .NET acronym casing, such as Rms, Html, Sdk, and Gdt, while retaining
the abbreviation itself. XML documentation records the exact MP command name.

A genuine normalized-name collision uses a documented deterministic
disambiguation based on the smallest MP-recognizable qualifier. V1 provides no
aliases. Infrastructure methods such as StartAsync and StopAsync are not MP
wrappers and are outside this mechanical MP-name rule.

Before implementing the full surface, the project must specify and unit-test
the exact normalization algorithm so future checksum or completeness tooling
can reproduce names without human judgment.

## Rule 16: Limit local validation to representation integrity

The client validates only:

- The integrity of its .NET arguments.
- The invariants of handwritten domain values.
- Its ability to construct the protocol request safely.

The Briosa server remains authoritative for MP semantics and executability,
including current SA state, object existence, runtime object type, licensing,
geometry, cross-argument rules, and command-specific conditions.

Detailed validation decisions, such as whether a particular native SA name may
be empty, are made case by case while implementing the relevant domain type or
command. Do not turn this rule into an invented universal validation catalog.

## Rule 17: Expose asynchronous MP methods only

V1 provides Task-based MP methods and no synchronous wrappers:

~~~csharp
public Task<string> GetWorkingDirectoryAsync(
    CancellationToken cancellationToken = default);
~~~

Do not add:

~~~csharp
public string GetWorkingDirectory();
~~~

Do not block internally through GetAwaiter().GetResult(). Use Task and Task&lt;T&gt;,
not ValueTask, for remote MP work. This rule applies to MP commands; ordinary
domain construction and configuration remain synchronous where appropriate.

## Rule 18: Provide a stable handwritten exception boundary

A failed command faults its returned Task. The public API does not use a
catch-all Result&lt;T&gt; wrapper for ordinary MP calls.

The high-level failure model distinguishes:

- BriosaOperationException for a typed Briosa operation failure.
- BriosaTransportException for a failure without a valid typed operation error.
- Conventional argument exceptions for invalid local inputs.
- OperationCanceledException semantics for caller cancellation.
- Lifecycle/startup failures that occur before MP submission.

Exact lifecycle exception names and properties are deferred to architecture.

The client decodes the value-free briosa-operation-error-bin trailer and maps it
into handwritten public .NET types. It never parses gRPC status text and does
not require consumers to catch RpcException or understand generated error
messages. A low-level exception may be preserved internally as an inner
exception without becoming the supported contract.

## Rule 19: Never replay an ambiguously completed command automatically

A connection failure, deadline, cancellation, worker crash, or lost response
may occur after SpatialAnalyzer executed a command. The client never silently
submits that command again when completion is ambiguous.

Preserve these dimensions separately:

- Execution disposition: what is known about the original attempt.
- Recovery guidance: what should happen before the system proceeds.
- Replay guidance: whether another attempt is recommended.
- Replay safety: the risk of duplicate execution.

Do not collapse them into one Retryable or CanRetry Boolean. Cancellation and
timeout stop waiting; they do not prove rollback. Applications remain
responsible for state reconciliation and any later replay.

Low-level recovery that conclusively occurs before the server observes a
command is not prohibited. The boundary is that once execution may have
occurred, the MP command is not automatically replayed.

## Rule 20: Make one singleton safe across runtime generations

BriosaClient is a long-lived application singleton. It is safe for concurrent
use and may survive multiple start/stop generations:

~~~csharp
await briosa.StartAsync(cancellationToken);
await briosa.GetWorkingDirectoryAsync(cancellationToken);
await briosa.StopAsync(cancellationToken);

await briosa.StartAsync(cancellationToken);
await briosa.GetWorkingDirectoryAsync(cancellationToken);
~~~

The same object remains registered throughout.

The implementation stores no mutable per-command state on the shared façade.
Configuration is effectively immutable after construction. Lifecycle state is
managed explicitly and atomically.

Concurrent MP calls must not corrupt the client, but concurrency does not
promise SpatialAnalyzer parallelism or execution order. Code that depends on
order awaits calls sequentially.

Lifecycle operations are also concurrency-safe:

- Competing StartAsync calls cannot create competing generations.
- A command cannot enter a partially initialized generation.
- StopAsync closes admission before dismantling a generation.
- A failed or replaced server invalidates its generation.

## Rule 21: Construct a dormant, transport-neutral client

The primary constructor accepts a handwritten BriosaClientOptions contract.
Construction:

- Captures and validates immutable configuration.
- Creates local lifecycle coordination objects.
- Performs no RPC.
- Launches no process.
- Does not check SA or server readiness.
- Does not expose generated or gRPC-specific constructor parameters.

The final endpoint and active channel may not exist until StartAsync launches a
server generation. Options therefore describe deferred runtime launch and
connection policy in addition to client-wide command settings.

Prefer one primary options-based constructor over many overlapping convenience
constructors. Whether a standard local address is required or selected during
startup remains an architecture and deployment decision.

## Rule 22: Use explicit asynchronous runtime cleanup

Because the singleton may launch and supervise a Briosa server generation,
shutdown is not purely synchronous local disposal.

BriosaClient exposes:

~~~csharp
public Task StopAsync(
    CancellationToken cancellationToken = default);
~~~

and supports asynchronous disposal. StopAsync is the controlled path when an
application needs shutdown diagnostics. Async disposal supplies final cleanup
for client-owned Briosa infrastructure.

For a client-owned generation, shutdown:

- Stops admitting new MP commands.
- Unpublishes the active generation.
- Performs bounded graceful server shutdown.
- Allows the server to clean up its worker and SDK connection.
- Disposes the generation channel and local resources.
- Never claims that shutdown rolled back an in-flight MP command.

The client never terminates an externally owned Briosa server. The policy for a
graceful SpatialAnalyzer close is separate because SA may contain unsaved
interactive work. Ordinary cleanup never forcefully terminates SpatialAnalyzer.

## Rule 23: Require explicit lifecycle startup

BriosaClient is dormant after construction and resolution. StartAsync
explicitly establishes a runtime generation; StopAsync ends it.

Registration, construction, DI resolution, and the first MP call never trigger
hidden startup. An MP command invoked without a ready generation fails
immediately rather than launching processes or waiting indefinitely.

The common UI flow is:

~~~csharp
private async void StartButton_Click(
    object sender,
    RoutedEventArgs e)
{
    await _briosa.StartAsync(_shutdownToken);
    SpatialAnalyzerFeatures.IsEnabled = true;
}
~~~

The singleton uses an internal lifecycle controller and verified connection
slot. Consumers do not coordinate unrelated public singleton services.

## Rule 24: Make StartAsync the readiness and compatibility boundary

StartAsync returns only after the new generation is genuinely ready for MP work
and compatible with the exact client package.

The conceptual sequence is:

~~~text
Launch or identify SpatialAnalyzer
    -> launch matching Briosa server
    -> establish gRPC liveness
    -> establish Briosa MP readiness
    -> read server information
    -> verify exact target and protocol identity
    -> read admitted capabilities
    -> atomically publish the generation
    -> complete StartAsync
~~~

Liveness alone is insufficient. Readiness alone is also insufficient if the
server belongs to another exact target.

Verification uses only identity information actually exposed by the server. Do
not claim a runtime schema-fingerprint check when no such field exists.

ListCapabilities may contain only the operations admitted by runtime policy.
Startup does not require every method compiled into the NuGet package to be
enabled.

The provisional startup channel remains unavailable to ordinary commands until
all gates succeed. Every new server generation repeats verification, even when
it appears at the same address. Startup timeout policy is separate from the
default MP-command timeout.

## Rule 25: Model the three runtime entities without sharing COM

The client design recognizes three distinct lifecycle entities:

- The SpatialAnalyzer application.
- The SpatialAnalyzer SDK engine.
- The Briosa gRPC server.

Public abstractions for these entities are desirable so applications can
support custom startup procedures, monitoring, and gradual adoption. Provisional
names include SpatialAnalyzerApplication, SpatialAnalyzerSdkEngine, and
BriosaServer. Their exact class/interface hierarchy, nesting, and method
signatures are deferred to architecture design.

The ordinary BriosaClient.StartAsync remains the convenience orchestration path
for an application starting from a clean runtime state. Advanced startup may
compose application-supplied lifecycle components.

The SDK ownership rule is strict:

~~~text
Existing application owns SDK connection
    -> disconnect and release
No SDK client owns execution
    -> Briosa worker creates its own connection
Briosa worker owns SDK execution
~~~

This is an exclusive ownership handoff, not a COM-object handoff. A live
ISpatialAnalyzerSDK object never crosses a process or language boundary. An
existing direct-SDK application must disconnect and release its COM interface
before the Briosa worker creates its own connection.

Simultaneous direct-SDK execution and a separate Briosa worker connection are
not supported by v1. Reusing the exact application-owned COM object would
require a separate external-worker hosting architecture and different recovery
guarantees.

Process existence is never treated as MP readiness. The client does not create
a competing SDK connection for interrogation. Only owned Briosa processes may
be terminated automatically, and ordinary lifecycle APIs never forcefully
terminate SpatialAnalyzer.

Whether a direct COM wrapper belongs in the primary package or an optional
target-specific integration package is deferred.

## Rule 26: Use one shared language-neutral Briosa test host

The deterministic fake environment is implemented once in the central Briosa
server project, not independently in every language client.

~~~text
briosa-dotnet --\
briosa-py -------+--> shared target-specific Briosa test host
briosa-js ------/
~~~

The shared test host:

- Implements the real public gRPC contract.
- Uses deterministic fake worker behavior.
- Exercises lifecycle, readiness, compatibility, typed failures, deadlines,
  cancellation, crashes, and unknown completion.
- Requires no SpatialAnalyzer installation, license, proprietary SDK binary, or
  vendor documentation.
- Is clearly identified as test-only software.
- Is not a SpatialAnalyzer emulator and makes no claim about real SA geometry,
  analysis, licensing, COM activation, port ownership, or performance.

Each language repository may provide a thin idiomatic fixture that locates,
launches, configures, and cleans up the shared host. Those fixtures do not
reimplement MP behavior or failure semantics.

Scenario configuration uses one versioned language-neutral mechanism owned by
spatialanalyzer/briosa. A test-control service or scenario artifact must never be
mapped accidentally by the production server.

The exact test artifact name, control protocol, package placement, and consumer
fixture API are deferred to test-host architecture.

## Explicit non-goals for v1

- No source-generated handwritten façade or domain model.
- No generated request envelopes in the idiomatic API.
- No synchronous MP wrappers.
- No MP convenience-overload matrix.
- No public generated protobuf/gRPC contract in the idiomatic NuGet package.
- No invented contextual defaults or hidden preliminary MP calls.
- No hidden startup on construction, DI resolution, or first command.
- No automatic replay of ambiguously completed work.
- No shared live COM interface between an existing application and Briosa.
- No independently maintained fake MP implementation in each language client.
- No claim that a fake test host emulates SpatialAnalyzer.

## Deferred architecture decisions

The following questions are intentionally not answered by the foundational
rules:

- Exact hierarchy and public interfaces for BriosaClient, BriosaServer,
  SpatialAnalyzerApplication, and SpatialAnalyzerSdkEngine.
- Exact default and advanced StartAsync signatures.
- Server artifact installation, discovery, verification, and executable
  location.
- SpatialAnalyzer executable discovery and launch strategy.
- Graceful SpatialAnalyzer close behavior and unsaved-work handling.
- Detailed client, application, SDK, server, and recovery state types.
- State-change notification and UI-binding mechanisms.
- Exact lifecycle and startup exception hierarchy and properties.
- Whether direct COM integration belongs in the main package or an optional
  package.
- Runtime endpoint selection and multiple desktop-application-instance policy.
- Test-host artifact format, scenario control contract, and thin language
  fixture APIs.
- Whether the concrete client also needs a public consumer interface; this must
  not be decided merely for mocking convenience.

These decisions must be resolved through focused architecture and implementation
issues using the rules above as constraints.

## Required cross-project follow-up

Several accepted decisions require coordinated work outside this repository:

- The central Briosa lifecycle documentation currently requires SpatialAnalyzer
  to be started separately. Supporting a default client-orchestrated SA launch
  must be accepted and documented in spatialanalyzer/briosa.
- The shared target-specific test host and its language-neutral scenario
  mechanism belong in spatialanalyzer/briosa.
- Exact defaults, presence, error, compatibility, and capability semantics
  remain shared server/protocol contracts.
- Other first-party clients should offer equivalent behavior without copying
  .NET-specific type shapes or reimplementing the fake backend.

Do not implement these shared behaviors solely in briosa-dotnet and then treat
the result as the cross-language contract.

## Next planning steps

1. Complete the cross-language review in SpatialAnalyzer Discussion #6.
2. Revise briosa-dotnet issue #3, whose current generated-public-client scope
   conflicts with this design.
3. Create focused architecture issues for lifecycle composition, shared server
   launching, process ownership, compatibility gating, and the shared test host.
4. Create implementation issues only after the relevant architecture decisions
   are accepted.
