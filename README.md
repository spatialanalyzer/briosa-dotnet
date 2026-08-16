# Briosa .NET client

`Briosa.Client` is the asynchronous .NET client for the open-source
[Briosa](https://github.com/spatialanalyzer/briosa) SpatialAnalyzer bridge.
It provides an idiomatic lifecycle API and handwritten MP methods while keeping
generated gRPC types private.

The package does not include SpatialAnalyzer, the SA SDK, or a license. The
current package targets SpatialAnalyzer `2026.1.0529.7` exactly and .NET 10 on
Windows x64. Its full protocol identity is pinned in
[`protocol.lock.json`](protocol.lock.json).

## Usage

```csharp
using Briosa;

await using var briosa = new BriosaClient();
await briosa.StartAsync();

string workingDirectory = await briosa.GetWorkingDirectoryAsync();
```

Construction is dormant. By default, `StartAsync()`:

1. Locates and launches the matching local Briosa server on an owned loopback
   endpoint.
2. Starts a disconnected SA SDK generation.
3. Launches a fresh SpatialAnalyzer application.
4. Connects the SDK and verifies exact identity and MP readiness.

`BriosaStartOptions` can select a control-plane-only startup or connect to an
eligible application that is already running. The application and SDK also have
distinct state, launch, connect, stop, and recovery methods. `StopAsync()` and
`DisposeAsync()` stop the owned server and SDK but never close SpatialAnalyzer.

The client retains lifecycle generations and supplies RPC guards automatically.
Typed lifecycle failures, compatibility failures, ambiguous MP completion, and
replay guidance remain distinct. The client never automatically replays an MP
operation.

See the [Briosa documentation](https://spatialanalyzer.github.io/briosa-docs/api/dotnet/)
for the complete Next API contract.

## Server distribution lookup

The client resolves the matching server distribution in this order:

1. `BRIOSA_SERVER_PATH`
2. A package-local `briosa-server/Briosa.Server.exe`
3. `%LOCALAPPDATA%/Briosa/servers/<briosa-version>/sa-<sa-target>/Briosa.Server.exe`

This locator is intentionally isolated from the public lifecycle API so the
eventual installer/package layout can change without adding server paths to MP
or startup option types.

## Build and test

```powershell
dotnet restore Briosa.DotNet.slnx --locked-mode
dotnet build Briosa.DotNet.slnx -c Release --no-restore
dotnet test Briosa.DotNet.slnx -c Release --no-build --no-restore
./eng/Test-Conformance.ps1 -ArtifactPath C:\path\to\briosa-client-conformance-0.2.1-sa-2026.1.0529.7-win-x64.zip
dotnet pack src/Briosa.Client/Briosa.Client.csproj -c Release --no-build --no-restore
```

Unit tests use a fake private transport. The shared conformance suite runs the
real client and server against a portable fake SDK/application host. Neither
path requires SpatialAnalyzer or a license.

## Protocol regeneration

After locked restore, import the exact lifecycle artifact through the repository
script:

```powershell
./eng/Import-ProtocolArtifact.ps1 `
  -ArtifactPath C:\path\to\briosa-protocol-0.2.1-sa-2026.1.0529.7.zip `
  -Update `
  -SourceChannel github_release

./eng/Import-ProtocolArtifact.ps1 `
  -ArtifactPath C:\path\to\briosa-protocol-0.2.1-sa-2026.1.0529.7.zip
```

Never edit `src/Briosa.Client/Generated` or `protocol.lock.json` by hand.
