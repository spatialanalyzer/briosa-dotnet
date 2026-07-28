# Briosa .NET client agent guide

This repository contains a thin generated client. Public protocol and command semantics belong to `spatialanalyzer/briosa`, not here.

- Consume only an exact, verified Briosa protocol artifact recorded in `protocol.lock.json`.
- Never hand-edit `src/Briosa.Client/Generated`; update through `eng/Import-ProtocolArtifact.ps1`.
- Keep hand-written code limited to idiomatic .NET transport adapters, packaging, deadlines/cancellation, presence handling, and typed errors.
- Do not add duplicate synchronous wrappers or hand-maintained per-command transports.
- Do not parse gRPC status text. Decode the value-free `briosa-operation-error-bin` trailer.
- Never automatically replay an ambiguously completed operation. Preserve execution disposition, recovery guidance, replay guidance, and replay safety separately.
- Ordinary builds and tests must not require SpatialAnalyzer, a license, proprietary SDK binaries, or vendor documentation.
- Run locked restore, build, tests, formatting, package creation, protocol drift verification, and shared conformance in proportion to the change.

GitHub issues and the organization roadmap are the planning source of truth. Keep changes scoped to the active client issue and link a PR with `Closes #<number>` only when all acceptance criteria are met.
