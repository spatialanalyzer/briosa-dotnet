# Briosa .NET clients

[Documentation](https://briosa.dev/) | [API Reference](https://briosa.dev/api/dotnet) | [First MP Command](https://briosa.dev/docs/getting-started/first-request)

Independent exact-target clients for the [Briosa SpatialAnalyzer bridge](https://github.com/spatialanalyzer/briosa).

| SpatialAnalyzer target | Product and development guide | Client version |
| --- | --- | --- |
| 2024.1.0508.5 | [SA 2024](targets/2024.1.0508.5/README.md) | 0.5.1 |
| 2026.1.0529.7 | [SA 2026](targets/2026.1.0529.7/README.md) | 0.5.1 |

Choose the package for your exact SA installation. Client 0.5.x works with
Briosa Server 0.10.x and does not work with Server 0.9.x or earlier (clients
0.4.x and earlier do not work with Server 0.10.x). For server releases after
0.10.0, a patch update keeps the same clients working; a minor update may
require a client upgrade. Each product pins its own generation artifact,
retains Server 0.9.2 as a tested rejection baseline, and tests the released
Server 0.10.0 as a baseline that must keep working unchanged. Client package
versions are independent of the Briosa server version. See
[Releasing](RELEASING.md) for registry publishing and validation.

Package names include the SA target; application namespaces/imports remain stable.
See each product guide for installation and examples. Run development commands
from the chosen `targets/<exact-sa-release>/` directory. CI validates both products.

SpatialAnalyzer, its SDK, and a license are required separately for real work.
Portable checks do not establish licensed runtime validation. Broader licensed
runtime CI and enterprise Artifactory verification remain outstanding before v1.0
promotion.

The 0.3.0 API naming changes are described in the [MP argument migration guide](docs/mp-argument-name-migration.md).

## 0.5.1 (for Server 0.10.x)

Client 0.5.1 adds to 0.5.0 without breaking it: an application written for
0.5.0 compiles and behaves the same, except in the three failure cases below,
each a fix. It uses what Server 0.10.1 adds and works with Server 0.10.0
unchanged, where each addition below reports that the server sent nothing:

- with Server 0.10.1 and later, a startup whose
  `BriosaStartOptions.StartupTimeout` is left unset sizes its budget from the
  server's published startup allowance: the longer of two minutes and that
  allowance plus 20 seconds. A timeout you set is always used as is, including
  an explicit two minutes. With Server 0.10.0 the budget stays two minutes;
- with Server 0.10.1 and later, the client also checks the startup budget
  against the clock before each startup request, so no step starts after the
  deadline, and a step that fails after it reports `startup-timeout` with that
  failure as its `InnerException`. A start that failed keeps MP commands
  refused, whatever state the client reads afterwards; stop the client and start
  it again. With Server 0.10.0 only the budget's timer ends a start, and
  readiness alone decides MP admission, as in 0.5.0;
- with Server 0.10.1 and later, the server snapshot reports the server's
  lifecycle wait allowances (`LifecycleBounds`) and the SpatialAnalyzer
  interaction mode it is configured to set (`ConfiguredInteractionMode`), which
  is configuration, never SpatialAnalyzer's live mode;
- with Server 0.10.1 and later, a policy denial carries typed detail
  (`BriosaOperationException.PolicyDenial`): which policy step denied, the
  denied risk flag, the admission profile, or the request field. Denied
  capabilities carry the same detail, and every capability lists its reviewed
  request options (`RequestOptions`, `null` when the server does not describe
  them). None of it changes a failure's kind or a capability's admission;
- the client accepts both Server 0.10.0 and Server 0.10.1, and still checks that
  the running server matches the installation it selected;
- the conformance fixture runs the server's optional `large-result` scenario,
  which shows a 1 MiB result received whole, and conformance runs keep the
  server logs of a failed scenario as a CI artifact.

With either server, it differs from 0.5.0 in exactly three failure cases, each
a fix:

- a start that fails before the client owns the server it launched stops that
  server itself, whether it ended through its budget, another failure, or
  disposal of the client meanwhile. Client 0.5.0 left that process running when
  the start ended after the server answered discovery but before the client took
  ownership, and in the disposal case `DisposeAsync` also threw
  `ObjectDisposedException`. Once the client owns the server, a failed start
  leaves it to a later `StopAsync` or `DisposeAsync`, which stops it, as before;
- after a start rejected for an out-of-range `StartupTimeout`, a later
  `StartAsync` with valid options works and `DisposeAsync` no longer rethrows.
  The rejection itself is unchanged: 4,294,967,295 whole milliseconds or more
  (about 49.7 days) throws `ArgumentOutOfRangeException` for parameter `delay`.
  A fraction of a millisecond is ignored, so 4,294,967,294.9999 ms is accepted;
- resource cleanup is attempted in full. When one close throws, the other
  resources are still released, a failed start keeps its own error, and
  `StopAsync` or `DisposeAsync` reports the close failure once every close has
  been attempted. A pending start that faulted, with any exception, no longer
  keeps `StopAsync` or `DisposeAsync` from closing its session.

The product guides describe each addition:
[SA 2024](targets/2024.1.0508.5/README.md#usage),
[SA 2026](targets/2026.1.0529.7/README.md#usage).

## 0.5.0 (for Server 0.10.x)

Client 0.5.x works with Server 0.10.x. Before launch, it checks that the
installation matches the exact SA target and declares support for this client.
It rejects Server 0.9.x and earlier. Client 0.5.0 breaks source compatibility on
purpose in both products; see the
[Client 0.5.0 migration guide](docs/client-0.5-migration.md) for each change:

- nine operations are removed: eight failed on both exact targets in licensed
  probe sessions, and no operator could complete the ninth;
- ten request defaults changed: six start measurement without waiting for the
  operator (for example `measureImmediately` defaults to `true`), three wait
  for completion or arrival, and one skips a prompt;
- no default or overload fills in an input the server requires: each one is a
  required parameter or a `required` member, with `Transform.Identity` for an
  explicit identity;
- every MP method takes an optional `onMpCompletion` observer before its
  `CancellationToken`, which reports a partial success (MP code 4) without
  changing the return type, except for four methods that report code 4 as an
  `MpFailure`;
- typed failures add `SdkCallFaulted`, MP execution details, and the lifecycle
  caller kinds, and a transport failure without a typed trailer reports
  `transport-<status>` (such as `transport-deadline-exceeded`) instead of
  `grpc-<Status>`;
- discovery reports the admission profile, each operation's classification,
  and a listing of denied operations with the profile and policy fingerprint it
  was taken under;
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
  [SA 2024](targets/2024.1.0508.5/README.md#inputs-the-server-requires),
  [SA 2026](targets/2026.1.0529.7/README.md#inputs-the-server-requires).
