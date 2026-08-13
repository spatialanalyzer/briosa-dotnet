# Engineering scripts

`Import-ProtocolArtifact.ps1` verifies one schema-2 Briosa protocol ZIP and its
adjacent checksum. `-Update` regenerates the private C# transport, relocates its
namespace beneath `Briosa.Client.Transport`, and records exact artifact, source,
schema, descriptor, package, generation-contract, and SA-target identities.
Verification mode regenerates into a temporary directory and fails on any drift.

The current bootstrap artifact is:

```powershell
./eng/Import-ProtocolArtifact.ps1 `
  -ArtifactPath C:\path\to\briosa-protocol-0.2.0-lifecycle-sa-2026.1.0529.7.zip `
  -Update `
  -SourceChannel source_commit_bootstrap
```

`Briosa.Client.Conformance` emits the normalized lifecycle contract implemented
by this client. Behavioral coverage lives in `Briosa.Client.Tests` and uses fake
server/transport boundaries; no SpatialAnalyzer installation or license is
required.
