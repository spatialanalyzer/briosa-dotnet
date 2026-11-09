# Client 0.5.0: contract major 3

Client 0.5.0 adopts behavioral contract major 3, which Briosa Server 0.10.0
introduces. Both exact-target products, `Briosa.2024.1.0508.5` and
`Briosa.2026.1.0529.7`, change the same way. Several changes break source
compatibility on purpose; the compiler finds most of them.

The protocol is generated from the frozen major-3 checkpoint `0.10.0-dev.1`
(briosa commit `2960705e`). The server-side record of every change is
[contract major 3](https://github.com/spatialanalyzer/briosa/blob/main/docs/development/contract-major-3.md),
with the
[admission profile](https://github.com/spatialanalyzer/briosa/blob/main/docs/development/admission-profile-migration.md)
and
[argument defaults](https://github.com/spatialanalyzer/briosa/blob/main/docs/development/argument-defaults-migration.md)
migrations.

## Server selection

0.5.0 selects and accepts only servers of contract major 3. It rejects the
major-2 Server 0.9 releases before launch, with `server-installation-incompatible`
during discovery. Keep client 0.4.0 for applications that must run against 0.9.x.
The two clients and servers install side by side.

## Removed operations

The server no longer has these nine operations, so the facades and their value
types are gone:

| Removed | Notes |
| --- | --- |
| `MirrorObjectsAsync`, with `MirrorFramePlane` | SpatialAnalyzer rejects it on both targets. |
| `DeleteCloudPointsByXYZRangeAsync` | There is no X Y Z box delete. `DeleteCloudPointsByRadialDistanceFromPointsAsync` deletes by distance instead, has different semantics, and is not licensed-validated yet. |
| `ConstructCirclesLinesFromSurfacesAsync`, with `CircleLineMode` | SpatialAnalyzer rejects it on both targets. |
| `AddCollectionInstrumentsToRefListWildcardSelectionAsync` | SpatialAnalyzer rejects it on both targets. |
| `MakeCalloutViewRefListWildcardSelectionAsync`, both `SetCalloutViewPropertiesAsync` overloads | Select callout views with `MakeCollectionItemNameRefListWildcardSelectionAsync`. |
| `ConstructPolygonizedSurfaceFromPointCloudsAsync`, with `MeshOrientationType` | Nothing replaces it. |
| `MakeCollectionVectorGroupNameRefListRuntimeSelectAsync` | Nothing replaces it. |
| `MakeVectorNameRefListRuntimeSelectAsync` | SpatialAnalyzer never rejected it, but no operator could complete the selection on either target, so it is unverified. It can return in a 3.x revision once a licensed session verifies it. |

## Defaults that now wait

The server denies the old values of these defaults: without the interactive
opt-in for the measure and prompt options, and always for the non-waiting
values. A caller that relied on an old default passes it explicitly.

| Method | Parameter | 0.4.0 | 0.5.0 |
| --- | --- | --- | --- |
| `MeasureSinglePointHereAsync` | `measureImmediately` | `false` | `true` |
| `ConfigureAndMeasureAsync` | `measureImmediately` | `false` | `true` |
| `MeasureExistingSinglePointAsync` | `measureImmediately` | `false` | `true` |
| `MeasureExistingSinglePointAndCompareAsync` | `measureImmediately` | `false` | `true` |
| `MeasureExistingSinglePointManualGuideAsync` | `measureImmediately` | `false` | `true` |
| `AutoMeasurePointsAsync` | `autoStart` | `false` | `true` |
| `AutoMeasureSpecifiedGeometryAsync` | `waitForComplete` | `false` | `true` |
| `MoveRobotMachineToFrameAsync` | `acknowledgeArrival` | `false` | `true` |
| `MoveRobotMachineToNamedDestinationAsync` | `acknowledgeArrival` | `false` | `true` |
| `DirectCadAccessAsync` | `promptOnMissingComponents` | `true` | `false` |

## Arguments that became optional

These are optional with no default. When you leave one out, the request leaves
the field unset and SpatialAnalyzer runs without it. An empty value is still
rejected by the server.

- `CreateChartFromVectorGroupAsync`: `templateChartName`. A name that matches no
  chart can open a blocking SpatialAnalyzer dialog.
- `SetUserInterfaceProfileAsync`: `profileFileName`.
- `FitGeometryToPointGroupAsync`, `FitGeometryToPointsAsync` and
  `FitGeometryToPointGroupProjectedToPlaneAsync`: `startingConditionGeometry`.
  SpatialAnalyzer then fits without a starting geometry.

## Arguments that became required

No default fills in a value that could do damage. Each of these is now a
required parameter, so omitting it is a compile error:

| Method | Parameters |
| --- | --- |
| `MoveRobotMachineToJointPoseSixDofAsync` | `joint1` to `joint6` |
| `SetInstrumentWeatherSettingAsync` | `setAutomatically` |
| `ScaleObjectsAsync` | `scaleFactor` |
| `SetAbsoluteInstrumentScaleFactorAsync`, `SetMultiplyInstrumentScaleFactorAsync` | `scaleFactor` |
| `SetRobotMachineParameterAsync` | `parameterName`, `parameterValue` |
| `DeleteMeasurementObservationAsync`, `MoveMeasurementObservationAsync` | `observationIndex` |
| `DeleteIthVectorFromVectorGroupAsync` | `vectorIndex` |
| `DeleteFolderAsync` | `folderPath` |
| `DeleteVariableAsync` | `name` |
| `DeleteVariablesWildcardMatchAsync` | `variableWildcardCriteria` |
| `DeleteVectorByNameAsync` | `vectorName` |
| `DeleteRobotCalibrationAsync` | `calibrationName` |
| `RemoveReportTagAsync` | `tagName` |
| `MakeGroupToGroupRelationshipAsync`, `MakeGroupToNominalGroupRelationshipAsync`, `MakePointToPointRelationshipAsync`, `MakePointsToPointsRelationshipAsync` | `tolerance`, `constraint` |
| `LocateInstrumentGroupToSurfaceQuickFitAsync` | `otherObjectsToTransform` |
| `FeatureInspectionAutoFilterAsync` | `pointNames`, `groupNames`, `cloudNames`, `featureCheckNameList` |

- An explicit `0`, `false` or empty string is still sent as given. The server,
  not the client, rejects an empty name or pattern, so the call fails with a
  `Validation` failure and `NotStarted`.
- Some required parameters moved ahead of the optional ones, so positional
  callers must check their argument order: the relationship `tolerance` and
  `constraint`, `featureCheckNameList`, and `setAutomatically`.
- `ToleranceVectorOptions.NoLimits` disables all eight limits. Pass it where you
  want no relationship limits; it is never an implicit default.
- `SetInstrumentWeatherSettingAsync` takes `double?` temperature, pressure and
  relative humidity with no `0` default. Omitted values stay unset. The server
  requires them when `setAutomatically` is `false`, and SpatialAnalyzer ignores
  them when it is `true`.
- The `SetRobotMachineModelLinkParametersAsync` overload without a
  configuration is gone, because it sent every parameter as `0` or `false`.
  Every `RobotModelLinkConfiguration` member is now `required`.
- The quick fit's other objects and the four Feature Inspection Auto Filter
  lists are required because the server rejects them empty. A required list
  passed as `null` throws `ArgumentNullException` before any call.
- `LocateInstrumentsUsmnAsync` keeps `excludedGroups` optional. An omitted or
  empty list is sent empty, and the server then runs USMN without that setting,
  as licensed probe 33 showed SpatialAnalyzer does correctly.

## Partial success and the MP completion of a call

MP code 4 (PARTIAL SUCCESS) means the command did its work but not its full
intended effect. The server reports it as a success, so the call returns its
outputs normally. The client never throws for it and never retries it.

To see how a call's MP command completed, pass the new `onMpCompletion`
observer. Every MP method takes it as an optional parameter just before its
`CancellationToken`:

```csharp
MpCompletion? completion = null;
var directory = await briosa.GetWorkingDirectoryAsync(
    onMpCompletion: value => completion = value,
    cancellationToken: cancellationToken);
if (completion?.State == MpExecutionState.PartialSuccess)
{
    // The command ran, with MP code 4 in completion.MpResultCode.
}
```

- The client calls the observer once, synchronously, before the returned task
  completes, and only for a successful call. State is `Succeeded` or
  `PartialSuccess`, with the MP result code.
- A call that passed its token positionally must now name it:
  `GetWorkingDirectoryAsync(cancellationToken: token)`.
- Four operations report code 4 as an ordinary `MpFailure` with code 4, because
  their target documentation says the work was not done:
  `ConstructPointFromSurveyTargetCenterAsync`, `OpenSaFileAsync`,
  `OpenTemplateFileAsync` and `SetReportOptionsForObjectAsync`.
- A typed failure keeps its MP details in `BriosaOperationException.MpExecution`.
  A partial success whose output could not be read stays `PartialSuccess` there.
- Every success response must carry its MP execution details; a server that
  omits them is reported as a protocol error.

## Typed failures

- `OperationFailureKind.SdkCallFaulted` reports that an SDK call threw during the
  MP sequence. `ExecutionDisposition` carries the phase: `NotStarted` before the
  MP step, `StartedOutcomeUnknown` during it (reconcile before replaying), and
  `Completed` when an output getter threw after MP success.
  `MpExecution.OutputRetrievals` then tells the read outputs from the failed one.
- Both lifecycle failure kinds gain `CallerCancelled` and
  `CallerDeadlineExceeded`. A cancelled lifecycle call still surfaces as
  `OperationCanceledException`, never as a state conflict.
- A blocking SpatialAnalyzer dialog arrives as `WorkerWatchdogTimeout` (or a
  readiness failure) with diagnostic `sa-modal-dialog-open` and operator
  intervention guidance. The client never retries it. The server setting
  `Briosa:SpatialAnalyzer:InteractionMode` (`Unchanged` by default; `Silent`
  for unattended stations) controls whether SpatialAnalyzer shows such dialogs.
  SpatialAnalyzer keeps the mode across restarts and has no getter for it, and
  clients don't set it.

## Discovery and admission

- `BriosaServerSnapshot` reports the server's `AdmissionProfile` and its opaque
  `PolicyFingerprint`. Compare the fingerprint only for equality.
- Each `BriosaOperationCapability` reports its reviewed `RiskFlags`,
  `DurationClass`, `ValidationStatus` and `Admission`, exactly as the server
  sends them. A value from a later server revision maps to `Unknown`.
- `GetDeniedOperationsAsync` lists the operations the active policy denies,
  each with the precedence step that denied it. It is diagnostic only: denied
  operations stay out of the snapshot and stay denied when called.
- The server's default `standard` profile is narrower than before. The seven
  device-configuration setters (the two instrument scale factors,
  `SetRobotMachineModelLinkParametersAsync`, `SetActiveRobotCalibrationAsync`,
  `SetRobotCalibrationToolFrameAsync`,
  `SetRobotCalibrationMeasurementOffsetInToolFrameAsync` and
  `SetProbeOffsetFrameOfflineAsync`) need the `device` or `full` profile or an
  override. Operations that open or block on operator UI need the explicit
  interactive opt-in.

## Lifecycle

- Cancelling Start, Connect, Reconnect or Recover before the server accepts it
  prevents the work. After acceptance the server finishes the exchange under its
  own bounds, so a startup you abandoned may still produce a ready generation.
  Refresh SDK state (and the server snapshot before MP work) instead of
  assuming an outcome.
- Treat `sdk-lifecycle-transition-in-progress` as refresh and retry later, and
  stop a generation you own only after the accepted transition has ended.
- After a timeout while launching SpatialAnalyzer, refresh application state
  before launching again; treat `application-lifecycle-transition-in-progress`
  the same way.

## Result mapping fixes

These getters reported a protocol error for every real response in 0.4.0:

- `LocateInstrumentGroupToSurfaceQuickFitAsync` now returns its fit errors.
  The wire's maximum absolute error is `FitErrorResult.MaximumError`, as in the
  JavaScript client ([#45](https://github.com/spatialanalyzer/briosa-dotnet/issues/45)).
- `GetRobotMachineModelLinkParametersAsync` maps the flat wire result into its
  `Configuration` and `EncoderValue`.
- `GetRelationshipRefListVariableAsync` and
  `GetReportItemsReferenceListVariableAsync` return `CollectionItemName[]`, the
  item names the server sends, instead of `CollectionObjectName[]`.
