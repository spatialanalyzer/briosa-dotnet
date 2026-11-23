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

The NuGet package and assembly are both named `Briosa.2026.1.0529.7`. Install the package with:

```powershell
dotnet add package Briosa.2026.1.0529.7 --version 0.5.1
```

To try a local build instead, give it a version of its own. NuGet installs a
package from its global packages folder whenever that folder already holds the
same ID and version, so a local build numbered `0.5.1` would install the cached
copy. Pack with a local suffix from `targets/2026.1.0529.7/`:

```powershell
dotnet pack src/Briosa.Client/Briosa.Client.csproj -c Release -o artifacts/local-package --version-suffix local.1
```

Then add the package folder to your project's package sources, next to the feeds
that supply its dependencies. Add a `briosa-local` entry to the `nuget.config`
in your project's folder, or create the file with this content. Without a
`<clear />`, it keeps the feeds of your other NuGet configuration files, such as
nuget.org:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="briosa-local" value="C:\path\to\briosa-dotnet\targets\2026.1.0529.7\artifacts\local-package" />
  </packageSources>
</configuration>
```

Finally, add that version from your project. Don't pass the folder with
`--source` instead: that replaces your feeds for the restore, so any package
that isn't already cached fails with `NU1101`.

```powershell
dotnet add package Briosa.2026.1.0529.7 --version 0.5.1-local.1
```

Use a new suffix, such as `local.2`, for each rebuild.

When you're done, first set the reference back to the version you used before,
such as `0.5.1`:

```powershell
dotnet add package Briosa.2026.1.0529.7 --version 0.5.1
```

If you added the package only for the trial, remove it instead:

```powershell
dotnet remove package Briosa.2026.1.0529.7
```

Then remove the `briosa-local` entry, or the file if you created it, and run
`dotnet restore`. Removing only the entry would leave the project on the local
version: your global packages folder keeps restoring it, and a machine without
that copy restores `0.5.1` instead, with warning `NU1603`.

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

`BriosaStartOptions.StartupTimeout` bounds the whole `StartAsync()`. A value you
set is always the budget, including an explicit two minutes. Left unset, the
budget is two minutes, which covers the server's default worst case of about
100 seconds for a start that launches SpatialAnalyzer, with a 20-second margin.
With Server 0.10.1 and later, an unset budget then follows the server's own
configuration: at the first discovery of the server, the client takes the
startup allowance that the server publishes for the selected phases and makes
the budget the longer of two minutes and that allowance plus 20 seconds, counted
from the start of the call. It only ever grows, and a start whose two minutes
have already run out is not extended
([shared rule](https://github.com/spatialanalyzer/briosa/blob/main/docs/architecture/client-library-behavioral-contract.md#startup-budget-from-published-lifecycle-bounds)).
Once the server has published that allowance, the client also checks the
budget, set or not, against the clock before each startup request, so no step
starts after the deadline; a step that fails after it reports `startup-timeout`
with that failure as `InnerException`.
With Server 0.10.0, which publishes no allowance, the budget stays two minutes:
if you raise that server's `Briosa:Worker:StartupTimeout`,
`Briosa:Worker:ReadinessProbeTimeout`, or
`Briosa:SpatialAnalyzer:ApplicationStartupTimeout`, raise the client budget too;
the [public API contract](docs/design/v1-public-api-contract.md#startup-and-lifecycle-time-budgets)
shows how to size it. Lifecycle methods add no deadline of their own. If the
budget expires or you cancel while the server is starting or connecting the
SDK, read the SDK state before retrying: the server finishes a step it has
already accepted, so the SDK can still become ready
([spatialanalyzer/briosa#315](https://github.com/spatialanalyzer/briosa/pull/315)).
A second lifecycle call meanwhile fails with
`sdk-lifecycle-transition-in-progress`: refresh state and retry later. A start
that fails before the client has attached the server it launched stops that
server; after that, `StopAsync()` or `DisposeAsync()` stops it.

The client retains lifecycle generations and supplies RPC guards automatically.
Typed lifecycle failures, compatibility failures, ambiguous MP completion, and
replay guidance remain distinct. The client never automatically replays an MP
operation. `BriosaOperationException.MpExecution` carries the MP state, result
code, and output retrievals of a failure that relates to the MP command, and
`OperationFailureKind.SdkCallFaulted` reports an SDK call that threw, with its
phase in `ExecutionDisposition`.

`GetServerSnapshotAsync()` reports the server's admission profile and policy
fingerprint, and each admitted operation's risk flags, duration class,
validation status, and admission. `GetDeniedOperationsAsync()` returns the
operations the policy denies, with the reason, and the admission profile and
policy fingerprint the listing was taken under; it never makes them callable.

With Server 0.10.1 and later, discovery and policy denials say more. Against
Server 0.10.0, which sends none of this, each value below is `null` or
`Unknown`, and everything else behaves as in client 0.5.0:

- `BriosaServerSnapshot.LifecycleBounds`: the server's lifecycle wait
  allowances, each an optional `TimeSpan` (`null` when the server sent none or
  zero, never unbounded), and the host's graceful shutdown bound as
  `Unavailable`, `Finite`, or `Unbounded`.
- `BriosaServerSnapshot.ConfiguredInteractionMode`: the SpatialAnalyzer
  interaction mode the server is configured to set after each Connect. It is the
  server's configuration, not the mode SpatialAnalyzer is using now.
- `BriosaOperationCapability.RequestOptions`: each operation's reviewed request
  options, the request fields that turn on operator UI or leave work running,
  with what the policy decides for each one alone. `null` means the server did
  not describe them; an empty list means none apply. The client never checks a
  request against them; the server decides each call.
- `PolicyDenial` on `BriosaOperationException`, on a denied
  `BriosaOperationCapability`, and on an `OperationRequestOption`: which policy
  step denied (`PolicyDenialRule`), the denied risk flag, the admission profile,
  or the request field the server attributed the denial to. It never changes the
  failure's `Kind` or the capability's `Admission`, and the exception carries it
  only when its `Kind` is `PolicyDenied`. A value this client does not
  recognize, including a flag name, maps to an `Unknown` member and never fails
  discovery.

The new members take no part in the equality or hash code of
`BriosaServerSnapshot` and `BriosaOperationCapability`, and `ToString()` shows
them only when the server sent them, so both records behave as in client 0.5.0.

See the [Briosa documentation](https://spatialanalyzer.github.io/briosa-docs/api/dotnet/)
for the client API overview. The target-specific source and locked protocol define
this package's exact API.

### Inputs the server requires

Every input the server requires is a required parameter, or a `required` member
of its input value, so omitting one is a compile error. No default, overload or
empty value fills one in. Where you want the identity transform, pass
`Transform.Identity`. The
[Client 0.5.0 migration guide](https://github.com/spatialanalyzer/briosa-dotnet/blob/main/docs/client-0.5-migration.md) lists every
input that became required in 0.5.0.

Leaving an optional list argument out sends an empty list. The server rejects
these lists when they are empty or absent, so they are required:

| Method | Required lists |
| --- | --- |
| `LocateInstrumentGroupToSurfaceQuickFitAsync` | `otherObjectsToTransform` |
| `FeatureInspectionAutoFilterAsync` | `pointNames`, `groupNames`, `cloudNames`, and `featureCheckNameList` |
| `AutoFilterPointsGroupsCloudsToSurfaceFacesAsync` | `points`, `groups`, and `clouds` |
| `CreatePointsToObjectsMapAsync` | `points` and `groups` |
| `SetPointsToPointsRelationshipAssociatedDataAsync` | `nominalPoints` and `actualPoints` |
| `SetRelationshipAssociatedDataAsync` | `individualPoints`, `pointGroups`, `pointClouds`, and `objects` |
| `ExtractGeometryFromPointCloudsAsync` | `boundingPoints` |

An empty required list is still sent, and the server rejects it with a
`BriosaOperationException` whose `Kind` is `Validation` and whose
`ExecutionDisposition` is `NotStarted`, so nothing runs in SpatialAnalyzer. A
`null` list is never sent as an empty one: the client either throws
`ArgumentNullException` before any call or leaves the list out, which the server
rejects in the same way.

`LocateInstrumentsUsmnAsync` keeps both of its optional inputs. An omitted or
empty `excludedGroups` list is sent empty, and the server then runs USMN without
that setting. An omitted `nominalsGroup` is left out, and an empty name is sent
blank, which SpatialAnalyzer documents as no nominals group.

### Partial success

For most methods, MP code 4 (PARTIAL SUCCESS) means the command did its work but
not its full intended effect. The method never retries it, and it returns its
outputs when all of them are retrieved. For four methods, listed below, the
SA 2026 documentation says code 4 leaves their work undone, so they report it as
a failure. Pass the optional `onMpCompletion` observer to see how a call
completed:

```csharp
MpCompletion? completion = null;
string directory = await briosa.GetWorkingDirectoryAsync(
    onMpCompletion: value => completion = value);
bool partial = completion?.State == MpExecutionState.PartialSuccess;
```

If an output can't be retrieved after code 4, the method throws
`BriosaOperationException`, and the observer doesn't run. The exception's `Kind`
depends on what went wrong:

- `OutputRetrievalFailure` when an output's SDK getter returned no usable value,
  or when the worker couldn't deliver the result, for example because an output
  was too large;
- `SdkCallFaulted` when an output's SDK getter threw.

With either kind, the exception's `MpExecution` still has `State`
`PartialSuccess` and `MpResultCode` 4. Its `OutputRetrievals` are the per-output
states the server reports. They don't always identify the getter that failed:
when the worker can't deliver a result, every output can be marked failed.

For these four methods, code 4 throws `BriosaOperationException` whose `Kind` is
`MpFailure`, with code 4 in its `MpExecution`, and the observer doesn't run:
`ConstructPointFromSurveyTargetCenterAsync`, `OpenSaFileAsync`,
`OpenTemplateFileAsync`, and `SetReportOptionsForObjectAsync`.

The observer comes just before the `CancellationToken`, so pass the token by
name when you leave the observer out. See the
[Client 0.5.0 migration guide](https://github.com/spatialanalyzer/briosa-dotnet/blob/main/docs/client-0.5-migration.md) for the
other 0.5.0 changes.

## Server selection

Applications select a server independently of their protocol generation pin.
On Windows x64, discovery reads Registry64 installation hints, committed
canonical stores, explicit search roots, and supported local layouts. It validates
receipts and manifests, filters for the exact SA target and a compatible server
release, and selects the highest compatible stable release. No internet access is
needed.

Client 0.5.x works with Briosa Server 0.10.x. Client 0.5.0 was tested with
Server 0.10.0. Client 0.5.1 is tested with the released Server 0.10.0 and with
Server 0.10.1 built from the source it pins, and it keeps working with Server
0.10.0 unchanged. Before launch, discovery checks that the installation matches
the exact SA target and declares support for this client: Server 0.9.x and
earlier releases (including Server 0.9.2) and installations with missing
compatibility information (including Server 0.6.1) are rejected before launch,
and clients 0.4.x and earlier do not work with Server 0.10.x. Use client 0.4.0
with Server 0.9.x releases. For server releases after 0.10.0, a patch update
keeps the same clients working; a minor update may require a client upgrade.
Protocol and source pins remain exact build inputs; startup verifies the running
server against its selected installation.

The three robot methods Get Robot Machine Parameter, Start Robot Machine
Interface, and Stop Robot Machine Interface now take a collection/instrument
identity for the MP argument named Machine ID. Supply an instrument ID rather
than a machine ID; other robot methods retain their existing types.

A full server queue returns an overload error with NotStarted, no recovery
action, and MayReplay guidance. The client preserves these separate facts and
never retries automatically. The 64 KiB inbound limit may also produce a
transport size error without typed detail. See the
[authoritative migration guide](https://github.com/spatialanalyzer/briosa/blob/main/docs/development/runtime-redesign-migration.md)
and the [Server 0.10.0 change record](https://github.com/spatialanalyzer/briosa/blob/main/docs/development/contract-major-3.md).

A missing or incompatible explicit choice fails without selecting another
installation. The choice is fixed for the session, including worker recovery.
Prereleases require explicit opt-in. Conflicting manifests for the same release
precedence produce an ambiguity error; identical copies prefer machine, user,
then portable scope and a stable path order. Elevated automatic discovery admits
only protected machine installations.

The optional selection settings include an executable path or installation ID,
an exact version, minimum version (inclusive), maximum version (exclusive),
excluded versions, search roots, allowed scopes, and an SA executable path.
Search roots name package stores with a committed `products` directory.
Read-only discovery returns candidates, the selected identity, and rejection codes
without starting Briosa, the SDK, or SA. Detailed paths belong to explicitly
requested diagnostics; installer **Verify/Repair** provides full payload checks.

`BRIOSA_SERVER_PATH` is ignored by default in client 0.2. Use a direct selection
option in new applications. Existing scripts can opt in to the legacy environment
override; an invalid override fails without fallback. Direct selectors take
precedence. Published 0.1.0/0.1.1 packages retain their original behavior.

Custom stores can be registered through the Installer's **Register existing
installations** action or `packages register` command. Alternatively provide a
search root or executable path per application. No machine-wide active server is
selected. Installing several SA releases does not establish concurrent execution:
the server still verifies the activated SDK and connected SA independently.

The [shared selection contract](https://github.com/spatialanalyzer/briosa/blob/main/docs/architecture/installation-selection-and-compatibility.md)
owns these rules and the [compatibility matrix](https://github.com/spatialanalyzer/briosa/blob/main/compatibility/matrix.json)
distinguishes tested pairs from declared forward compatibility.

```csharp
var selection = new BriosaServerSelection { Version = "0.10.1-dev.1", AllowPrerelease = true };
var report = BriosaInstallations.Discover(selection); // no process launch
await briosa.StartAsync(new BriosaStartOptions { ServerSelection = selection });

```

Use `ExecutablePath`, `InstallationId`, `MinimumVersion`, `MaximumVersionExclusive`,
`ExcludedVersions`, `SearchRoots`, `AllowedScopes`, `AllowPrerelease`,
`SpatialAnalyzerExecutablePath`, and `UseLegacyEnvironmentOverride` as needed.

## Build and test

Run these commands from `targets/2026.1.0529.7/`; each target builds and
packages independently.

```powershell
dotnet restore Briosa.DotNet.slnx --locked-mode
dotnet build Briosa.DotNet.slnx -c Release --no-restore
dotnet test Briosa.DotNet.slnx -c Release --no-build --no-restore
./eng/Test-Conformance.ps1 -ArtifactPath C:\path\to\briosa-client-conformance-0.9.2-sa-2026.1.0529.7-win-x64.zip -ExpectIncompatible
./eng/Test-Conformance.ps1 -ArtifactPath C:\path\to\briosa-client-conformance-0.10.0-sa-2026.1.0529.7-win-x64.zip -LockPath released-conformance.lock.json
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
  -ArtifactPath C:\path\to\briosa-protocol-0.10.1-dev.1-sa-2026.1.0529.7.zip `
  -Update `
  -SourceChannel source_commit_bootstrap

./eng/Import-ProtocolArtifact.ps1 `
  -ArtifactPath C:\path\to\briosa-protocol-0.10.1-dev.1-sa-2026.1.0529.7.zip
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

## Compatibility and validation

This package pins its generation artifact and tests compatibility against
packaged servers. Client 0.5.x works with Server 0.10.x; the retained Server
0.9.2 baseline (from the incompatible 0.9.x line) must be rejected, and the
released Server 0.10.0 must keep working unchanged.
Exact SA target, runtime identity, capabilities, and readiness still gate MP calls.

Portable conformance covers lifecycle, identity mismatch, denied capabilities
with the admission profile and the denied listing, typed MP and output failure,
SDK call faults, partial success, deadlines, cancellation, a lifecycle Connect
whose caller stopped waiting, watchdog recovery, SDK loss, and owned-process
cleanup: all 15 fake-SDK scenarios tested with Client 0.5.0 and Server 0.10.0.
Client 0.5.1 runs them against the released Server 0.10.0 and against Server
0.10.1 built from its pinned source, where it also runs the optional
`large-result` scenario: a result of 1,046,528 characters, the server's 1 MiB
worker response budget, received whole. The harness disables Control Center auto-launch and
restores the prior setting afterward. These checks use a fake SDK, without a
SpatialAnalyzer installation or license.

The public relationship-reference inputs use `CollectionItemName`, including
the item name and optional item type; they are not geometric object references.
This corrects earlier pre-publication facade annotations that disagreed with the
protocol. Existing callers of those methods must pass the item-name value.

Broader licensed runtime coverage and protected runtime CI remain outstanding.
Enterprise Artifactory integration is also unverified. This package remains v0.x;
portable results do not imply v1.0 readiness or validation on physical instruments.
