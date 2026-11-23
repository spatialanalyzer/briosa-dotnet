# Client 0.5.0 migration

Client 0.5.x works with Briosa Server 0.10.x, starting with Server 0.10.0. Both
exact-target products, `Briosa.2024.1.0508.5` and `Briosa.2026.1.0529.7`, change
the same way. Several changes break source compatibility on purpose; the
compiler finds most of them.

The protocol is generated from the frozen Server 0.10.0 protocol checkpoint
`0.10.0-dev.1` (briosa commit `2960705e`). The server-side record of every
change is the
[Server 0.10.0 change record](https://github.com/spatialanalyzer/briosa/blob/main/docs/development/contract-major-3.md),
with the
[admission profile](https://github.com/spatialanalyzer/briosa/blob/main/docs/development/admission-profile-migration.md)
and
[argument defaults](https://github.com/spatialanalyzer/briosa/blob/main/docs/development/argument-defaults-migration.md)
migrations.

## Server selection

Client 0.5.x selects and accepts only Briosa Server 0.10.x installations. It
rejects Server 0.9.x and earlier releases before launch, with
`server-installation-incompatible` during discovery. Clients 0.4.x and earlier
do not work with Server 0.10.x; keep client 0.4.0 for applications that must
run against Server 0.9.x. A server patch release stays compatible with the same
clients; a minor release may break compatibility. The two clients and servers
install side by side.

## Removed operations

The server no longer has these nine operations, so the facades and their value
types are gone:

| Removed | Notes |
| --- | --- |
| `MirrorObjectsAsync`, with `MirrorFramePlane` | SpatialAnalyzer rejects it on both targets. |
| `DeleteCloudPointsByXYZRangeAsync` | There is no X Y Z box delete. `DeleteCloudPointsByRadialDistanceFromPointsAsync` deletes by distance instead and has different semantics; a licensed session validated it on both targets for one cloud and one point. |
| `ConstructCirclesLinesFromSurfacesAsync`, with `CircleLineMode` | SpatialAnalyzer rejects it on both targets. |
| `AddCollectionInstrumentsToRefListWildcardSelectionAsync` | SpatialAnalyzer reported success (MP code 2) on both targets, but reading the input list back returned nothing, and the MP documents no return argument. A caller that needs an instrument list can build it directly. |
| `MakeCalloutViewRefListWildcardSelectionAsync`, both `SetCalloutViewPropertiesAsync` overloads | Select callout views with `MakeCollectionItemNameRefListWildcardSelectionAsync` and the callout-view item type. It uses a different MP step, and that item type has not been observed on a licensed run. Nothing replaces setting callout view properties on a list. |
| `ConstructPolygonizedSurfaceFromPointCloudsAsync`, with `MeshOrientationType` | Nothing replaces it. |
| `MakeCollectionVectorGroupNameRefListRuntimeSelectAsync` | Nothing replaces it. |
| `MakeVectorNameRefListRuntimeSelectAsync` | SpatialAnalyzer never rejected it, but no operator could complete the selection on either target, so it is unverified. It can return in a later Server 0.10.x release once a licensed session verifies it. |

## Request defaults that changed

Ten request defaults changed. The five `measureImmediately` defaults and
`autoStart` are now `true`, so measurement begins without waiting for the
operator. `waitForComplete` and the two `acknowledgeArrival` defaults are now
`true`, so those calls wait for completion or arrival. `promptOnMissingComponents`
is now `false`, so CAD import skips its missing-component prompt.

The server denies the old values of these defaults: without the interactive
opt-in for the measure, start, and prompt options, and always for the
non-waiting values. A caller that relied on an old default passes it explicitly.

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

### Every other input the server requires

The client never fills in an input the server requires: no identity transform,
no default choice or settings value, and no empty list. An audit of every
optional parameter and input member against the server's request checks found
these, which are now required (both targets unless noted):

| Kind | Method | Parameters |
| --- | --- | --- |
| Lists | `AutoFilterPointsGroupsCloudsToSurfaceFacesAsync` | `points`, `groups`, `clouds` |
| | `CreatePointsToObjectsMapAsync` | `points`, `groups` |
| | `SetPointsToPointsRelationshipAssociatedDataAsync` | `nominalPoints`, `actualPoints` |
| | `SetRelationshipAssociatedDataAsync` | `individualPoints`, `pointGroups`, `pointClouds`, `objects` |
| | `ExtractGeometryFromPointCloudsAsync` (2026 only) | `boundingPoints` |
| Choices that had a default | `AutoFilterPointsGroupsCloudsToSurfaceFacesAsync`, `FeatureInspectionAutoFilterAsync` | `offsetDirection` |
| | `MakeDynamicCircleRelationshipAsync`, `…EllipseRelationshipAsync`, `…LineRelationshipAsync`, `…PlaneRelationshipAsync`, `…PointRelationshipAsync` | `constructionMode` |
| | `MakeCollectionObjectNameRefListByTypeAsync`, `…ByTypeAndColorAsync`, `…RuntimeSelectAsync`, `…WildcardSelectionAsync`, `MakeCollectionObjectNameRuntimeSelectAsync` | `objectType` |
| | `MakeCollectionItemNameRefListWildcardSelectionAsync` | `itemType` |
| | `ExtractGeometryFromPointCloudsAsync` (2026 only) | `geometryType` |
| | `SetGdtOptionsAsync` | `distanceBetweenMode`, `evaluationMethod` |
| | `CollimationAsync` | `tiltMode`, `baselineMethod` |
| | `LocateInstrumentsUsmnAsync` | `showUsmnDialog` |
| | `SetTargetComputationOptionsAsync` | `computationMethod` |
| | `CreatePointCalloutAsync` | `desiredCoordinateSystem` |
| | `ConstructPointsFromSurfacesOnUvGridAsync` | `edgePointMode` |
| Settings | `AutoFilterCloudsToNominalGeometry2DAsync`, `AutoFilterCloudsToNominalGeometry3DAsync`, `AutoFilterPointsToNominalGeometry3DAsync` | `filterProximitySettings2D` or `filterProximitySettings3D` |
| | `DoRelationshipFitAsync`, `MoveCollectionsByMinimizingRelationshipsAsync` | `motionToAllow` |
| | `MakeGroupsToObjectsRelationshipAsync`, `MakePointCloudsToObjectsRelationshipAsync`, `MakePointsToObjectsRelationshipAsync` | `projectionOptions` |
| | `MakeFrameToFrameRelationshipAsync` | `orientationTolerance`, `positionTolerance` |
| | `SetObjectToObjectDirectionRelationshipTolerancesAsync` | `angleBetweenVectorsTolerances`, `mutualPerpendicularLengthTolerances` |
| | `BuildTargetAsync` | `tolerance` |
| | `RelationshipWatchWindowTemplateAsync` | `udpNetworkTransmitSettings`, new: it was a member of the options |
| Names | `MakeDynamicPointRelationshipAsync` | `thirdReferenceGeometry` |
| | `ConstructCrossSectionCloudAsync` | `referenceObject` |
| | `ConstructPointFromCloudPointRuntimeSelectAsync` | `constructedPointName` |
| | `ConstructPointsSubsetWithGreatestSpacingAsync` | `groupForSubset` |
| | `EnableDisableDatumAlignmentForFeatureCheckAsync` | `alignment` |
| | `AutoCorrespondWithProximityTriggerAsync` | `deviationVectorGroupName` |
| | `CollimationAsync` | `scalePoint1`, `scalePoint2`, `notMeasuredByMovingInstrument`, `asMeasuredByMovingInstrument` |
| | `SetObservationCollimationShotOptionsAsync` | `targetedInstrument` |
| | `RelationshipWatchWindowTemplateAsync` | `watchWindowTemplateName`, no longer nullable |
| Transforms and vectors | `ConstructEllipsoidAsync` | `transformInWorkingCoordinates` |
| | `CalculateTcpFixtureUncertaintiesAsync` | `tcpInWorking` |
| | `ConstructPointsShiftedInWorkingFrameAsync` | `shiftVector` |
| | `GetGradientAtProjectedPointOnSurfaceEdgeAsync` | `edgeOffsetDirection` |

- Most of these moved ahead of the optional parameters. Check the argument
  order of positional calls, or name the arguments.
- These overloads are gone, because each one filled in a required input:
  - the overloads without a transform of `ConstructFrameAsync`,
    `ConstructFrameFromTransformInWorldAsync`,
    `SetCalibrationApplianceNodeMeasurementOffsetTransformAsync`,
    `SetRobotCalibrationMeasurementOffsetInToolFrameAsync`,
    `SetRobotCalibrationToolFrameAsync` and `SetRobotMachineBaseTransformAsync`,
    which sent the identity;
  - `RelationshipWatchWindowTemplateAsync()` with no arguments, which sent no
    template name and default UDP settings.
- `Transform.Values` is `required`, so `new Transform()` no longer compiles. Pass
  `Transform.Identity` where you mean the identity. Each read returns a fresh
  value.
- These input members are `required` too:
  - every member of `FilterProximitySettings`: the three proximity modes, the
    six distances (`SurfaceInclusionProximity`, `EdgeExclusionProximity`,
    `PlanarInclusionProximity`, `PlanarExclusionProximity`,
    `RadialInclusionProximity` and `GeometryExtractionTolerance`) and the two
    Booleans (`ProjectToPlane` and `AssertPlaneBoundaries`). The server sends
    each one to SpatialAnalyzer as given, and the distances are in the job's
    units. 0.4.0 filled in 0.1, 0.1, 0.5, 0.1, 0.1, 0.01, `true` and `false`.
    Treat those values as an example only, and choose your own for the job;
  - `ReportOutputOptions.OutputType`;
  - `Objects`, `GeometryRelationships`, `AuxiliaryObject` and
    `AuxiliaryGeometryRelationship` of both `MakeGdtDatumAnnotationOptions` and
    `MakeGdtFeatureCheckAnnotationOptions`.
- `RelationshipWatchWindowTemplateOptions` no longer has
  `UdpNetworkTransmitSettings`. Pass the settings to
  `RelationshipWatchWindowTemplateAsync`, where `new
  RelationshipWatchWindowUdpSettings()` sends the server's defaults.
- `ReportOutputOptions` must name exactly one destination, `ExternalPath` or
  `EmbeddedFile`. Naming neither now fails before any call, as naming both
  already did, with `ArgumentException`: `report-output-destination-missing` or
  `report-output-destination-conflict`.
- An input the server checks only when you pass it stays optional, as do the D2
  omissions above.

## Partial success and the MP completion of a call

MP code 4 (PARTIAL SUCCESS) means the command did its work but not its full
intended effect, except for the four operations below. The server reports it as
a success, so the call returns its outputs normally when all of them are
retrieved. The client never retries it.

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
  completes, and only for a successful call. State is `Succeeded` with MP code 2
  or `PartialSuccess` with MP code 4, and `MpCompletion.MpResultCode` is an
  `int`.
- A call that passed its token positionally must now name it:
  `GetWorkingDirectoryAsync(cancellationToken: token)`.
- Four operations report code 4 as an ordinary `MpFailure` with code 4 on both
  targets:
  - `ConstructPointFromSurveyTargetCenterAsync` and
    `SetReportOptionsForObjectAsync`, because the documentation of both targets
    says code 4 leaves their work undone: no target point was found, or report
    options are not available for the object;
  - `OpenSaFileAsync` and `OpenTemplateFileAsync`. The SA 2026 documentation
    says code 4 means the file was found but could not be opened. The SA 2024
    documentation doesn't list code 4 for them. They are on the SA 2024 list by
    maintainer decision
    ([spatialanalyzer/briosa#351](https://github.com/spatialanalyzer/briosa/issues/351)),
    because a false success that discards the open job would be worse than a
    false failure.
- A typed failure keeps its MP details in `BriosaOperationException.MpExecution`.
  If an output can't be retrieved after code 4, the call fails and the observer
  doesn't run. The kind is `SdkCallFaulted` when an output getter threw, and
  `OutputRetrievalFailure` when a getter returned no usable value or the worker
  couldn't deliver the result (diagnostic `worker-output-encoding-rejected`).
  With either kind, `MpExecution` stays `PartialSuccess` with code 4.
- The client checks every success, whether or not you observe it. A success is
  a protocol error, `BriosaProtocolException`, in three cases:
  - it has no MP execution details (`mp-execution-missing`);
  - its state is neither of the two above (`mp-execution-state`);
  - its MP code is missing or doesn't match its state
    (`mp-execution-result-code`).

  The observer runs after that check and before the outputs are mapped, so it
  still reports the completion when output mapping then fails. Adding or removing
  an observer never changes a call's outcome.

## Typed failures

- `OperationFailureKind.SdkCallFaulted` reports that an SDK call threw during the
  MP sequence. `ExecutionDisposition` carries the phase: `NotStarted` before the
  MP step, `StartedOutcomeUnknown` during it (reconcile before replaying), and
  `Completed` when an output getter threw after MP code 2 or 4. `MpExecution`
  then keeps `Succeeded` with 2 or `PartialSuccess` with 4, and its
  `OutputRetrievals` are the per-output states the server reports. They don't
  always identify the getter that threw: if the worker had to withhold that
  evidence, every output is reported failed.
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
- A transport failure without a typed trailer is still a
  `BriosaTransportException`. Its `DiagnosticCode` is now `transport-` plus the
  gRPC status name in lower kebab case, the format of the JavaScript and Python
  clients. Every such code is renamed:

  | 0.4.0 | 0.5.0 |
  | --- | --- |
  | `grpc-OK` | `transport-ok` |
  | `grpc-Unknown` | `transport-unknown` |
  | `grpc-InvalidArgument` | `transport-invalid-argument` |
  | `grpc-DeadlineExceeded` | `transport-deadline-exceeded` |
  | `grpc-NotFound` | `transport-not-found` |
  | `grpc-AlreadyExists` | `transport-already-exists` |
  | `grpc-PermissionDenied` | `transport-permission-denied` |
  | `grpc-ResourceExhausted` | `transport-resource-exhausted` |
  | `grpc-FailedPrecondition` | `transport-failed-precondition` |
  | `grpc-Aborted` | `transport-aborted` |
  | `grpc-OutOfRange` | `transport-out-of-range` |
  | `grpc-Unimplemented` | `transport-unimplemented` |
  | `grpc-Internal` | `transport-internal` |
  | `grpc-Unavailable` | `transport-unavailable` |
  | `grpc-DataLoss` | `transport-data-loss` |
  | `grpc-Unauthenticated` | `transport-unauthenticated` |
  | `grpc-` plus a number, for a status gRPC doesn't define | `transport-failure` |

  A cancelled call is still `OperationCanceledException`.

## Discovery and admission

- `BriosaServerSnapshot` reports the server's `AdmissionProfile` and its opaque
  `PolicyFingerprint`. Compare the fingerprint only for equality.
- Each `BriosaOperationCapability` reports its reviewed `RiskFlags`,
  `DurationClass`, `ValidationStatus` and `Admission`, exactly as the server
  sends them. A value from a later server release maps to `Unknown`.
- `GetDeniedOperationsAsync` returns a `BriosaDeniedOperationListing`: the
  admission profile and the policy fingerprint the listing was taken under, and
  the operations the active policy denies, each with the precedence step that
  denied it. It is diagnostic only: denied operations stay out of the snapshot
  and stay denied when called.
- The server's default `standard` profile is narrower than before. The seven
  device-configuration setters (the two instrument scale factors,
  `SetRobotMachineModelLinkParametersAsync`, `SetActiveRobotCalibrationAsync`,
  `SetRobotCalibrationToolFrameAsync`,
  `SetRobotCalibrationMeasurementOffsetInToolFrameAsync` and
  `SetProbeOffsetFrameOfflineAsync`) need the `device` or `full` profile,
  `Flags:device_config=allow`, or a per-operation `allow` override. Operations
  that open or block on operator UI need the explicit interactive opt-in.
- The facades of denied operations stay. The multi-call device, trapping,
  watch, robot-interface and appliance session workflows are exclusive, and
  every profile, flag and override denies them for now: a call fails with
  `PolicyDenied` and `NotStarted`.

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
- An external application can briefly report diagnostic
  `external-spatial-analyzer-unconfirmed`: the server could not confirm it was
  still running, so it stays `Running` with the same generation. It needs no
  client change.

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
