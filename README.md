# Briosa .NET client

`Briosa.Client` is the thin asynchronous .NET client for the open-source [Briosa](https://github.com/spatialanalyzer/briosa) gRPC bridge.

The client does not contain SpatialAnalyzer, the SpatialAnalyzer SDK, or a license. A compatible Briosa server and a separately installed, running, licensed SpatialAnalyzer instance are required for useful work.

## Current compatibility

This bootstrap targets:

| Coordinate | Pinned value |
| --- | --- |
| SpatialAnalyzer | `2026.1.0529.7` exactly |
| Core protocol | `briosa.core.v1alpha1` |
| Target protocol | `briosa.sa.v2026_1_0529_7.v1alpha1` |
| Catalog | `briosa.sa.2026.1.0529.7`, revision `5` |
| .NET | `net10.0` |

The complete generation identity is committed in [`protocol.lock.json`](protocol.lock.json). Client package versions, Briosa server versions, protocol packages, catalog revisions, and SpatialAnalyzer releases are independent coordinates. This client does not infer compatibility with another SpatialAnalyzer release.

Until Briosa publishes its first v0.2 release asset, the lock uses the reversible `source_commit_bootstrap` channel: CI rebuilds `0.2.0-dev.1` from the immutable Briosa merge commit and verifies its ZIP hash. A release update will replace that channel with the published release asset without changing client semantics.

## Usage

```csharp
using Briosa.Client;

using var client = new BriosaClient(new BriosaClientOptions
{
    Address = new Uri("http://127.0.0.1:50051"),
    DefaultTimeout = TimeSpan.FromSeconds(30)
});

var snapshot = await client.GetServerSnapshotAsync(cancellationToken: cancellationToken);
if (!snapshot.ReadyForMp)
{
    return;
}

var result = await client.GetWorkingDirectoryAsync(
    cancellationToken: cancellationToken);

if (result.HasDirectory)
{
    var workingDirectory = result.Directory;
}
```

`GetServerSnapshotAsync` verifies the exact target protocol and catalog identity before returning discovery data. Generated protobuf messages preserve field presence, including `GetWorkingDirectoryResult.HasDirectory`.

Failed calls throw `BriosaCallException`. Its `StatusCode` is independent from its optional typed `OperationError`; the client decodes `briosa-operation-error-bin` and never parses status text. `CompletionUnknown` and `ReconciliationRequired` remain distinct from worker recovery. The client performs no automatic operation replay.

Only asynchronous convenience methods are hand-written. Generated messages and transport clients are public for callers that need lower-level gRPC control; this repository does not reproduce ObjectiveSA-style parallel synchronous/asynchronous command implementations.

## Build and test

```powershell
dotnet restore Briosa.DotNet.slnx --locked-mode
dotnet build Briosa.DotNet.slnx -c Release --no-restore
dotnet test Briosa.DotNet.slnx -c Release --no-build --no-restore
dotnet pack src/Briosa.Client/Briosa.Client.csproj -c Release --no-build --no-restore
```

Ordinary builds and unit tests require neither SpatialAnalyzer nor the Briosa server.

## Protocol regeneration

Restore first so the pinned `Grpc.Tools` compiler is available, then import an exact Briosa protocol ZIP:

```powershell
./eng/Import-ProtocolArtifact.ps1 -ArtifactPath C:\path\to\briosa-protocol-....zip -Update
./eng/Import-ProtocolArtifact.ps1 -ArtifactPath C:\path\to\briosa-protocol-....zip
```

`-Update` is an intentional dependency update. It requires and verifies the adjacent `.zip.sha256`, regenerates transport sources, and atomically records the artifact, schema, descriptor, catalog, target, and conformance identities. Its default source channel is `github_release`; use `-SourceChannel source_commit_bootstrap` only for the current unreleased, immutable-commit bootstrap. Verification mode fails on ZIP, manifest, coordinate, generated-file, or file-list drift. Never edit `src/Briosa.Client/Generated` or `protocol.lock.json` by hand.

`./eng/Test-Conformance.ps1` rebuilds the pinned Briosa server package and runs the shared live and typed-error fixture sets with its fake worker. The test requires 64-bit Windows but not SpatialAnalyzer or a license.
