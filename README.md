# Briosa .NET client

`Briosa.2026.1.0529.7` is the asynchronous .NET client for the open-source
[Briosa](https://github.com/spatialanalyzer/briosa) SpatialAnalyzer bridge.
It provides an idiomatic lifecycle API and handwritten MP methods while keeping
generated gRPC types private.

The package does not include SpatialAnalyzer, the SA SDK, or a license. The
current package targets SpatialAnalyzer `2026.1.0529.7` exactly and .NET 10 on
Windows x64. Its full protocol identity is pinned in
[`protocol.lock.json`](protocol.lock.json).

## Package Identity

The NuGet package and assembly are both named `Briosa.2026.1.0529.7`. The
package has not been published to NuGet yet. After publication, the intended
installation command is:

```powershell
dotnet add package Briosa.2026.1.0529.7 --version 0.1.0
```

Exact SpatialAnalyzer targets use separate package and assembly identities,
while application code continues to use the stable `Briosa` namespace. A
future application that must reference several target packages can isolate
their otherwise matching public names with .NET assembly aliases; there is no
universal runtime target selector. Only the `2026.1.0529.7` target package is
implemented today.

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
./eng/Test-Conformance.ps1 -ArtifactPath C:\path\to\briosa-client-conformance-0.3.0-sa-2026.1.0529.7-win-x64.zip
dotnet pack src/Briosa.Client/Briosa.Client.csproj -c Release --no-build --no-restore
```

Unit tests use a fake private transport. The shared conformance suite runs the
real client and server against a portable fake SDK/application host. Neither
path requires SpatialAnalyzer or a license.

## Protocol regeneration

After locked restore, import the exact protocol artifact through the repository
script:

```powershell
./eng/Import-ProtocolArtifact.ps1 `
  -ArtifactPath C:\path\to\briosa-protocol-0.3.0-sa-2026.1.0529.7.zip `
  -Update `
  -SourceChannel github_release

./eng/Import-ProtocolArtifact.ps1 `
  -ArtifactPath C:\path\to\briosa-protocol-0.3.0-sa-2026.1.0529.7.zip
```

Never edit `src/Briosa.Client/Generated` or `protocol.lock.json` by hand.
## Server Logging

Use the optional `BriosaStartOptions.Logging` property to override startup logging:

```csharp
await briosa.StartAsync(new BriosaStartOptions
{
    Logging = new BriosaLoggingOptions
    {
        MinimumLevel = BriosaLogLevel.Debug,
        ConsoleEnabled = false,
        MaxFileSizeMiB = 20,
        RetainedFileCount = 10
    }
});
```

`CategoryLevels`, `FileEnabled`, `FileDirectory`, `MaxAgeDays`, and
`MaxTotalSizeMiB` provide the remaining typed controls. Omitted settings preserve
server configuration. Values are validated before launch; custom directories
must be absolute. See the [shared startup contract](https://github.com/spatialanalyzer/briosa/blob/main/docs/architecture/client-library-behavioral-contract.md#server-logging-startup-controls)
and [server observability guide](https://github.com/spatialanalyzer/briosa/blob/main/targets/2026.1.0529.7/docs/operations/server-observability.md).
