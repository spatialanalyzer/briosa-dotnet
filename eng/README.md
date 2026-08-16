# Engineering scripts

`Import-ProtocolArtifact.ps1` verifies one schema-2 Briosa protocol ZIP and its
adjacent checksum. `-Update` regenerates the private C# transport, relocates its
namespace beneath `Briosa.Client.Transport`, and records exact artifact, source,
schema, descriptor, package, generation-contract, and SA-target identities.
Verification mode regenerates into a temporary directory and fails on any drift.

The current released artifact is:

```powershell
./eng/Import-ProtocolArtifact.ps1 `
  -ArtifactPath C:\path\to\briosa-protocol-0.2.1-sa-2026.1.0529.7.zip `
  -Update `
  -SourceChannel github_actions_artifact
```

`Test-Conformance.ps1` verifies the immutable package named by
`conformance.lock.json`, then runs the shared Briosa scenario runner against the
public-API-only `Briosa.Client.Conformance` fixture. The package supplies the
real Briosa server plus a portable fake SDK/application host, so lifecycle,
compatibility, capability, failure, interruption, worker-loss, recovery, and
cleanup behavior can run in ordinary Windows CI without SpatialAnalyzer or a
license.
