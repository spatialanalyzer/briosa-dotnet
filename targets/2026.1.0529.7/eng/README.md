# Engineering scripts

`Import-ProtocolArtifact.ps1` verifies one schema-3 Briosa protocol ZIP and its
adjacent checksum. `-Update` regenerates the private C# transport, relocates its
namespace beneath `Briosa.Client.Transport`, and records exact artifact, source,
schema, descriptor, package, generation-contract, and SA-target identities.
Verification mode regenerates into a temporary directory and fails on any drift.

The pinned generation artifact is the checkpoint `0.10.1-dev.1`, which the
server's `targets/2026.1.0529.7/eng/New-ProtocolArtifact.ps1` builds from the
Server 0.10.1 source revision recorded in `protocol.lock.json`:

```powershell
# From a briosa checkout at the pinned source revision:
./targets/2026.1.0529.7/eng/New-ProtocolArtifact.ps1 `
  -Version 0.10.1-dev.1 `
  -SourceRevision d405779027085b3dc26dc09c62a669820db5c164 `
  -OutputDirectory C:\path\to\protocol

# From this directory:
./eng/Import-ProtocolArtifact.ps1 `
  -ArtifactPath C:\path\to\protocol\briosa-protocol-0.10.1-dev.1-sa-2026.1.0529.7.zip `
  -Update `
  -SourceChannel source_commit_bootstrap
```

The artifact's manifest declares revision 1, and the generated identity
records it. The client still requires revision 0, in installation selection and
in live discovery (`ServerSelectionPolicy.MinimumCompatibilityRevision`), so it
accepts Server 0.10.0 too.

`Test-Conformance.ps1` verifies the immutable package named by
`conformance.lock.json`, then runs the shared Briosa scenario runner against the
public-API-only `Briosa.Client.Conformance` fixture. The package supplies the
real Briosa server plus a portable fake SDK/application host, so lifecycle,
compatibility, capability, failure, interruption, worker-loss, recovery, and
cleanup behavior can run in ordinary Windows CI without SpatialAnalyzer or a
license.


Run all commands from this exact-target directory. `Test-Conformance.ps1` runs
headlessly with `Briosa__Desktop__Mode=Disabled` for its child processes and restores
the caller's prior value. The package consumer check installs the actual local
package and validates the stable public namespace/import without launching SA.

The importer also copies the server-owned installation selection fixtures.
Runtime compatibility is independent of the generation artifact build.
`Test-Conformance.ps1 -LockPath <lock> -EvidencePath <new-report.json>` runs
another exact, hash-verified server fixture and retains portable evidence.
The default lock retains Server 0.9.2, the previous contract major, for
rejection coverage. Use -ExpectIncompatible with that lock; a contract-major mismatch must fail before launch.
`released-conformance.lock.json` pins the published Server 0.10.0 package, which
this client must still accept with unchanged behavior; run it with
`-LockPath released-conformance.lock.json` and without `-ExpectIncompatible`.

When the packaged runner offers optional scenarios (Server 0.10.1 and later),
`Test-Conformance.ps1` also requests `large-result` and records it in the
evidence under `scenarios` and `optionalScenarios`. It also gives the fixture
240 seconds (`-FixtureTimeoutSeconds`, recorded as `fixtureTimeoutSeconds`):
with Server 0.10.1 and the runner's 30-second fake-worker startup bound, the
client's startup budget is 180 seconds, so a hang ends in the client's typed
startup timeout before the runner stops the fixture. The evidence's
`client.version` is the supplied package's version for a package run and this
source's version otherwise; `client.fixtureVersion` is always this source's.
The fixture computes the
UTF-8 length and SHA-256 of the directory this client returned and reports them,
never the value. The rejection path requests no optional scenario. The fixture
decides whether a server publishes the Server 0.10.1 additions from the
`ContractRevision` of the installation that discovery selected (1 or later),
never from the server's version label, which is `0.2.0-ci` in the server's own
CI.

Set `BRIOSA_CONFORMANCE_DIAGNOSTICS_DIRECTORY` to keep a failing scenario's
failure message, server log digest, fixture output, and server log files; CI
sets one folder per step and uploads them when a job fails. The Server 0.9.2
runner does not support it, and its rejection legs start no server.

`Test-CurrentServerConformance.ps1` builds the exact generation source and writes
its own conformance lock; the legacy lock remains independently pinned to 0.9.2.
`Test-PackageCompatibility.ps1` exercises the actual packed client against the
rejected Server 0.9.2, the released Server 0.10.0, and the server built from the
pinned source, each with its own diagnostics folder. `Test-PublishedClientConformance.ps1` is also used by the
server's retained published-client gate: it verifies the package digest and
identity, creates an isolated consumer, and runs the public-API fixture without
referencing client runtime source. The fixture compiles against every published
0.5.x package: it asserts the client 0.5.1 additions only when the package is
0.5.1 or later, through the `BRIOSA_CLIENT_0_5_1` compilation symbol. JSON evidence identifies local versus public
package origin and records fake-SDK validation separately from licensed SA work.
