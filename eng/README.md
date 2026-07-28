# Engineering scripts

## Protocol artifact

`Import-ProtocolArtifact.ps1` accepts one Briosa protocol ZIP. With `-Update`, it verifies the archive and regenerates the committed C# transport plus `protocol.lock.json`. Without `-Update`, it generates into a temporary directory and fails on any identity, checksum, file-list, or generated-code drift.

The script uses the `protoc` and C# gRPC plugin from the centrally pinned `Grpc.Tools` package. Run locked restore before it.

## Shared conformance

`Test-Conformance.ps1` requires the pinned protocol ZIP and an exact Briosa source checkout:

```powershell
./eng/Test-Conformance.ps1 `
  -ProtocolArtifactPath C:\path\to\briosa-protocol-0.2.0-dev.2-sa-2026.1.0529.7-catalog-5.zip `
  -BriosaRepository C:\path\to\briosa
```

It validates every language-neutral typed-error fixture, builds a deterministic Briosa Windows package, substitutes Briosa's fake worker, and runs every live scenario through `Briosa.Client`. It never prints the returned working-directory value.
