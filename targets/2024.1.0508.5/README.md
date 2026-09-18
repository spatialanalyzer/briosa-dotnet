# Briosa .NET client

`Briosa.2024.1.0508.5` is the asynchronous .NET client for the open-source
[Briosa](https://github.com/spatialanalyzer/briosa) SpatialAnalyzer bridge.
It provides an idiomatic lifecycle API and handwritten MP methods while keeping
generated gRPC types private.

The package does not include SpatialAnalyzer, the SA SDK, or a license. The
current package targets SpatialAnalyzer `2024.1.0508.5` exactly and .NET 10 on
Windows x64. Its full protocol identity is pinned in
[`protocol.lock.json`](protocol.lock.json).

## Package Identity

The NuGet package and assembly are both named `Briosa.2024.1.0508.5`. Install the package with:

```powershell
dotnet add package Briosa.2024.1.0508.5 --version 0.1.1
```

Exact SpatialAnalyzer targets use separate package and assembly identities,
while application code continues to use the stable `Briosa` namespace. An
application that must reference several target packages can isolate
their otherwise matching public names with .NET assembly aliases; there is no
universal runtime target selector. Both reviewed SA targets are maintained
independently in this repository.

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
for the client API overview. The target-specific source and locked protocol define
this package's exact API.

## Server distribution lookup

Install **Briosa Server 0.6.1 for SA 2024.1.0508.5** with the Briosa Installer.
Default startup searches these locations in order:

1. `BRIOSA_SERVER_PATH`, pointing to `Briosa.Server.exe`.
2. An application-local `briosa-server/Briosa.Server.exe`.
3. `%LOCALAPPDATA%/Briosa/Packages/products/<package-id>/payload/Briosa.Server.exe`.
4. `%PROGRAMDATA%/Briosa/Packages/products/<package-id>/payload/Briosa.Server.exe`.
5. The legacy `%LOCALAPPDATA%/Briosa/servers/<briosa-version>/sa-<sa-target>/Briosa.Server.exe`.

For this client, `<package-id>` is `briosa-0.6.1-sa-2024.1.0508.5-win-x64`.
Managed installations must have a matching committed receipt, manifest, and required
entry points. Missing or invalid candidates are skipped; discovery never selects a
different server version or SA target. Runtime compatibility checks still apply.
Installer verification/repair checks package integrity separately.

For a custom Installer store, set `BRIOSA_SERVER_PATH` to the desired product's
`payload/Briosa.Server.exe`. Custom stores are not searched automatically.
The [shared discovery contract](https://github.com/spatialanalyzer/briosa/blob/main/docs/architecture/installed-package-store.md#client-server-discovery)
defines precedence, eligibility, root handling, and the installation/runtime boundary.

## Build and test

Run these commands from `targets/2024.1.0508.5/`; each target builds and
packages independently.

```powershell
dotnet restore Briosa.DotNet.slnx --locked-mode
dotnet build Briosa.DotNet.slnx -c Release --no-restore
dotnet test Briosa.DotNet.slnx -c Release --no-build --no-restore
./eng/Test-Conformance.ps1 -ArtifactPath C:\path\to\briosa-client-conformance-0.6.1-sa-2024.1.0508.5-win-x64.zip
dotnet pack src/Briosa.Client/Briosa.Client.csproj -c Release --no-build --no-restore -o artifacts/package
./eng/Test-PackageIdentity.ps1
./eng/Test-PackageConsumer.ps1
```

Unit tests use a fake private transport. The shared conformance suite runs the
real client and server against a portable fake SDK/application host. Neither
path requires SpatialAnalyzer or a license.

## Protocol regeneration

After locked restore, import the exact protocol artifact through the repository
script:

```powershell
./eng/Import-ProtocolArtifact.ps1 `
  -ArtifactPath C:\path\to\briosa-protocol-0.6.1-sa-2024.1.0508.5.zip `
  -Update `
  -SourceChannel github_release

./eng/Import-ProtocolArtifact.ps1 `
  -ArtifactPath C:\path\to\briosa-protocol-0.6.1-sa-2024.1.0508.5.zip
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
and [server observability guide](https://github.com/spatialanalyzer/briosa/blob/main/targets/2024.1.0508.5/docs/operations/server-observability.md).

## Compatibility and validation

This package pins the matching Briosa v0.6.1 protocol and conformance bundles.
Startup checks the server version, source revision, protocol package, and exact
SA target before admitting MP calls. The other SA target is not interchangeable.

Portable conformance covers lifecycle, identity mismatch, denied capabilities,
typed MP and output failure, deadlines, cancellation, watchdog recovery, SDK loss,
and owned-process cleanup. Its harness disables Control Center auto-launch and
restores the prior setting afterward. These checks use a fake SDK, without a
SpatialAnalyzer installation or license.

The public relationship-reference inputs use `CollectionItemName`, including
the item name and optional item type; they are not geometric object references.
This corrects earlier pre-publication facade annotations that disagreed with the
protocol. Existing callers of those methods must pass the item-name value.

Broader licensed runtime coverage and protected runtime CI remain outstanding.
Enterprise Artifactory integration is also unverified. This package remains v0.x;
portable results do not imply v1.0 readiness or validation on physical instruments.

### SA 2024 differences

The facade implements the reviewed 996-operation SA 2024 surface. It omits later
commands and the three scan operations whose SDK bindings are unavailable in
this release. See the authoritative [compatibility record](https://github.com/spatialanalyzer/briosa/blob/v0.6.1/targets/2024.1.0508.5/docs/development/sa2024-compatibility.md)
for the complete command and field differences.

- Run Crib Sheet, Project Objects, and Stop Projection are available. Their
  instrument-dependent runtime validation remains outstanding.
- Surface-face construction takes seven explicit Boolean selectors (planes,
  cylinders, spheres, cones, lines, points, circles).
- Direct CAD Access requires an explicit surface compatibility mode; both QDAS
  operations require the caller's date/time stamp.
- General relationship statistics return Max Deviation without an inferred
  absolute-value guarantee. Cone properties omit cut length; points-to-objects
  statistics omit average deviation; feature-check reporting omits the later
  failed-vectors-only option.
- Enhanced Cloud and the three later system-string choices are absent. Reserved
  wire values stay reserved; remaining choices retain their original numbers.
- Instrument names retain the exact value supplied by the caller. The server
  validates against the 185 reviewed 2024 literals, including `PMT Arm 4m 7 dof`.
