# Briosa .NET clients

[Documentation](https://briosa.dev/) | [API Reference](https://briosa.dev/api/dotnet) | [First MP Command](https://briosa.dev/docs/getting-started/first-request)

Independent exact-target clients for the [Briosa SpatialAnalyzer bridge](https://github.com/spatialanalyzer/briosa).

| SpatialAnalyzer target | Product and development guide | Client version |
| --- | --- | --- |
| 2024.1.0508.5 | [SA 2024](targets/2024.1.0508.5/README.md) | 0.5.0 |
| 2026.1.0529.7 | [SA 2026](targets/2026.1.0529.7/README.md) | 0.5.0 |

Choose the package for your exact SA installation. Each product pins its own
generation artifact and retains Server 0.9.2, the previous contract major, as a
tested rejection baseline. The 0.5 client line selects side-by-side servers
using behavioral contract 3.0. Client package versions
are independent of the Briosa server version. See [Releasing](RELEASING.md) for
registry publishing and validation.

Package names include the SA target; application namespaces/imports remain stable.
See each product guide for installation and examples. Run development commands
from the chosen `targets/<exact-sa-release>/` directory. CI validates both products.

SpatialAnalyzer, its SDK, and a license are required separately for real work.
Portable checks do not establish licensed runtime validation. Broader licensed
runtime CI and enterprise Artifactory verification remain outstanding before v1.0
promotion.

The 0.3.0 API naming changes are described in the [MP argument migration guide](docs/mp-argument-name-migration.md).

## 0.5.0 (unreleased): contract major 3

Client 0.5.0 adopts behavioral contract major 3 (Briosa Server 0.10.0) in both
products, and selects only major-3 servers. It breaks source compatibility on
purpose; see the [contract major 3 migration](docs/contract-major-3-migration.md)
for each change:

- nine operations that SpatialAnalyzer rejects or no operator could complete are
  removed;
- ten request defaults now wait, for example `measureImmediately` defaults to
  `true`;
- no default fills in a risky value, and the arguments the server requires are
  required parameters;
- every MP method takes an optional `onMpCompletion` observer before its
  `CancellationToken`, which reports a partial success (MP code 4) without
  changing the return type;
- typed failures add `SdkCallFaulted`, MP execution details, and the lifecycle
  caller kinds;
- discovery reports the admission profile, each operation's classification,
  and a listing of denied operations;
- four result mappings that failed on every real response are fixed, including
  the quick fit ([#45](https://github.com/spatialanalyzer/briosa-dotnet/issues/45)).

It also includes two changes made since 0.4.0:

- The default `BriosaStartOptions.StartupTimeout` is two minutes instead of 30
  seconds. It is sized from the server's default lifecycle bounds; see
  [startup and lifecycle time budgets](docs/design/v1-public-api-contract.md#startup-and-lifecycle-time-budgets).
  A budget that expires during an RPC raises `BriosaStartupException`
  (`startup-timeout`) instead of `OperationCanceledException`.
- Optional list arguments left at their `null` default are sent as empty lists
  instead of throwing `ArgumentNullException`; see
  [collections](docs/design/v1-public-api-contract.md#collections). The lists the
  server rejects when empty are required parameters in 0.5.0; each product guide
  lists them:
  [SA 2024](targets/2024.1.0508.5/README.md#lists-the-server-requires),
  [SA 2026](targets/2026.1.0529.7/README.md#lists-the-server-requires).
