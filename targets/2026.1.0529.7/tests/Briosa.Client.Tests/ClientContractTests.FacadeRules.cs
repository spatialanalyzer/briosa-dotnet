#pragma warning disable CA2000 // Test doubles are disposed through the client session under test.

using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

// The facade defaults, omissions and requiredness rules of contract major 3
// (briosa admission-profile and argument-defaults migrations, #297 and #325),
// the six lists the server requires (briosa-dotnet#39 follow-up), and the
// result mapping of every operation (#45). The shared conformance scenarios
// don't exercise these mappings, so each rule is checked on the built request.
public sealed partial class ClientContractTests
{
    // The ten request defaults that now wait: the client sends the new value,
    // and an explicit caller value unchanged.
    [Theory]
    [InlineData(nameof(BriosaInstrumentOperations.MeasureSinglePointHereAsync), "measureImmediately", true)]
    [InlineData(nameof(BriosaInstrumentOperations.ConfigureAndMeasureAsync), "measureImmediately", true)]
    [InlineData(nameof(BriosaInstrumentOperations.MeasureExistingSinglePointAsync), "measureImmediately", true)]
    [InlineData(nameof(BriosaInstrumentOperations.MeasureExistingSinglePointAndCompareAsync), "measureImmediately", true)]
    [InlineData(nameof(BriosaInstrumentOperations.MeasureExistingSinglePointManualGuideAsync), "measureImmediately", true)]
    [InlineData(nameof(BriosaInstrumentOperations.AutoMeasurePointsAsync), "autoStart", true)]
    [InlineData(nameof(BriosaInstrumentOperations.AutoMeasureSpecifiedGeometryAsync), "waitForComplete", true)]
    [InlineData(nameof(BriosaRobotOperations.MoveRobotMachineToFrameAsync), "acknowledgeArrival", true)]
    [InlineData(nameof(BriosaRobotOperations.MoveRobotMachineToNamedDestinationAsync), "acknowledgeArrival", true)]
    [InlineData(nameof(BriosaClient.DirectCadAccessAsync), "promptOnMissingComponents", false)]
    public async Task FlippedDefaultsAreSentAndExplicitValuesKept(
        string methodName,
        string parameterName,
        bool expectedDefault)
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var (target, method) = Facade(client, methodName);
        var parameter = Assert.Single(method.GetParameters(), item => item.Name == parameterName);
        Assert.Equal(expectedDefault, parameter.DefaultValue);

        var sent = await InvokeAsync(transport, target, method);
        Assert.Equal(expectedDefault, FieldValue(sent, parameterName));
        Assert.True(HasField(sent, parameterName));

        sent = await InvokeAsync(transport, target, method, (parameterName, !expectedDefault));
        Assert.Equal(!expectedDefault, FieldValue(sent, parameterName));
    }

    // Optional with no default: an omitted argument leaves the field unset, and a
    // supplied one is sent.
    [Theory]
    [InlineData(nameof(BriosaClient.CreateChartFromVectorGroupAsync), "templateChartName")]
    [InlineData(nameof(BriosaClient.SetUserInterfaceProfileAsync), "profileFileName")]
    [InlineData(nameof(BriosaClient.FitGeometryToPointGroupAsync), "startingConditionGeometry")]
    [InlineData(nameof(BriosaClient.FitGeometryToPointsAsync), "startingConditionGeometry")]
    [InlineData(nameof(BriosaClient.FitGeometryToPointGroupProjectedToPlaneAsync), "startingConditionGeometry")]
    public async Task OmittedOptionalArgumentsLeaveTheFieldUnset(string methodName, string parameterName)
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var (target, method) = Facade(client, methodName);
        var parameter = Assert.Single(method.GetParameters(), item => item.Name == parameterName);
        Assert.True(parameter.HasDefaultValue);
        Assert.Null(parameter.DefaultValue);

        var sent = await InvokeAsync(transport, target, method);
        Assert.False(HasField(sent, parameterName));

        sent = await InvokeAsync(
            transport,
            target,
            method,
            (parameterName, SampleArgument(parameter.ParameterType)));
        Assert.True(HasField(sent, parameterName));
    }

    // Required with no default, so omitting one is a compile error. The
    // relationship limits have no implicit default; the six lists the server
    // rejects when empty are required except the USMN excluded groups.
    [Theory]
    [InlineData(nameof(BriosaClient.MakeGroupToGroupRelationshipAsync), "tolerance")]
    [InlineData(nameof(BriosaClient.MakeGroupToGroupRelationshipAsync), "constraint")]
    [InlineData(nameof(BriosaClient.MakeGroupToNominalGroupRelationshipAsync), "tolerance")]
    [InlineData(nameof(BriosaClient.MakeGroupToNominalGroupRelationshipAsync), "constraint")]
    [InlineData(nameof(BriosaClient.MakePointToPointRelationshipAsync), "tolerance")]
    [InlineData(nameof(BriosaClient.MakePointToPointRelationshipAsync), "constraint")]
    [InlineData(nameof(BriosaClient.MakePointsToPointsRelationshipAsync), "tolerance")]
    [InlineData(nameof(BriosaClient.MakePointsToPointsRelationshipAsync), "constraint")]
    [InlineData(nameof(BriosaRobotOperations.MoveRobotMachineToJointPoseSixDofAsync), "joint1")]
    [InlineData(nameof(BriosaRobotOperations.MoveRobotMachineToJointPoseSixDofAsync), "joint2")]
    [InlineData(nameof(BriosaRobotOperations.MoveRobotMachineToJointPoseSixDofAsync), "joint3")]
    [InlineData(nameof(BriosaRobotOperations.MoveRobotMachineToJointPoseSixDofAsync), "joint4")]
    [InlineData(nameof(BriosaRobotOperations.MoveRobotMachineToJointPoseSixDofAsync), "joint5")]
    [InlineData(nameof(BriosaRobotOperations.MoveRobotMachineToJointPoseSixDofAsync), "joint6")]
    [InlineData(nameof(BriosaInstrumentOperations.SetInstrumentWeatherSettingAsync), "setAutomatically")]
    [InlineData(nameof(BriosaClient.ScaleObjectsAsync), "scaleFactor")]
    [InlineData(nameof(BriosaInstrumentOperations.SetAbsoluteInstrumentScaleFactorAsync), "scaleFactor")]
    [InlineData(nameof(BriosaInstrumentOperations.SetMultiplyInstrumentScaleFactorAsync), "scaleFactor")]
    [InlineData(nameof(BriosaRobotOperations.SetRobotMachineParameterAsync), "parameterValue")]
    [InlineData(nameof(BriosaInstrumentOperations.DeleteMeasurementObservationAsync), "observationIndex")]
    [InlineData(nameof(BriosaInstrumentOperations.MoveMeasurementObservationAsync), "observationIndex")]
    [InlineData(nameof(BriosaClient.DeleteIthVectorFromVectorGroupAsync), "vectorIndex")]
    [InlineData(nameof(BriosaClient.DeleteFolderAsync), "folderPath")]
    [InlineData(nameof(BriosaClient.DeleteVariableAsync), "name")]
    [InlineData(nameof(BriosaClient.DeleteVariablesWildcardMatchAsync), "variableWildcardCriteria")]
    [InlineData(nameof(BriosaClient.DeleteVectorByNameAsync), "vectorName")]
    [InlineData(nameof(BriosaConstructionOperations.DeleteCollectionsByWildcardAsync), "searchString")]
    [InlineData(nameof(BriosaConstructionOperations.DeleteFoldersByWildcardAsync), "searchString")]
    [InlineData(nameof(BriosaRobotOperations.DeleteRobotCalibrationAsync), "calibrationName")]
    [InlineData(nameof(BriosaClient.RemoveReportTagAsync), "tagName")]
    [InlineData(nameof(BriosaGdtOperations.FeatureInspectionAutoFilterAsync), "pointNames")]
    [InlineData(nameof(BriosaGdtOperations.FeatureInspectionAutoFilterAsync), "groupNames")]
    [InlineData(nameof(BriosaGdtOperations.FeatureInspectionAutoFilterAsync), "cloudNames")]
    [InlineData(nameof(BriosaGdtOperations.FeatureInspectionAutoFilterAsync), "featureCheckNameList")]
    [InlineData(nameof(BriosaInstrumentOperations.LocateInstrumentGroupToSurfaceQuickFitAsync), "otherObjectsToTransform")]
    // C3 (spatialanalyzer/briosa#363): every input the server requires.
    [InlineData(nameof(BriosaInstrumentOperations.AutoCorrespondWithProximityTriggerAsync), "deviationVectorGroupName")]
    [InlineData(nameof(BriosaClient.AutoFilterCloudsToNominalGeometry2DAsync), "filterProximitySettings2D")]
    [InlineData(nameof(BriosaClient.AutoFilterCloudsToNominalGeometry3DAsync), "filterProximitySettings3D")]
    [InlineData(nameof(BriosaClient.AutoFilterPointsGroupsCloudsToSurfaceFacesAsync), "clouds")]
    [InlineData(nameof(BriosaClient.AutoFilterPointsGroupsCloudsToSurfaceFacesAsync), "groups")]
    [InlineData(nameof(BriosaClient.AutoFilterPointsGroupsCloudsToSurfaceFacesAsync), "offsetDirection")]
    [InlineData(nameof(BriosaClient.AutoFilterPointsGroupsCloudsToSurfaceFacesAsync), "points")]
    [InlineData(nameof(BriosaClient.AutoFilterPointsToNominalGeometry3DAsync), "filterProximitySettings3D")]
    [InlineData(nameof(BriosaInstrumentOperations.BuildTargetAsync), "tolerance")]
    [InlineData(nameof(BriosaInstrumentOperations.CalculateTcpFixtureUncertaintiesAsync), "tcpInWorking")]
    [InlineData(nameof(BriosaInstrumentOperations.CollimationAsync), "asMeasuredByMovingInstrument")]
    [InlineData(nameof(BriosaInstrumentOperations.CollimationAsync), "baselineMethod")]
    [InlineData(nameof(BriosaInstrumentOperations.CollimationAsync), "notMeasuredByMovingInstrument")]
    [InlineData(nameof(BriosaInstrumentOperations.CollimationAsync), "scalePoint1")]
    [InlineData(nameof(BriosaInstrumentOperations.CollimationAsync), "scalePoint2")]
    [InlineData(nameof(BriosaInstrumentOperations.CollimationAsync), "tiltMode")]
    [InlineData(nameof(BriosaConstructionOperations.ConstructCrossSectionCloudAsync), "referenceObject")]
    [InlineData(nameof(BriosaConstructionOperations.ConstructEllipsoidAsync), "transformInWorkingCoordinates")]
    [InlineData(nameof(BriosaConstructionOperations.ConstructPointFromCloudPointRuntimeSelectAsync), "constructedPointName")]
    [InlineData(nameof(BriosaConstructionOperations.ConstructPointsFromSurfacesOnUvGridAsync), "edgePointMode")]
    [InlineData(nameof(BriosaConstructionOperations.ConstructPointsShiftedInWorkingFrameAsync), "shiftVector")]
    [InlineData(nameof(BriosaConstructionOperations.ConstructPointsSubsetWithGreatestSpacingAsync), "groupForSubset")]
    [InlineData(nameof(BriosaConstructionOperations.CreatePointCalloutAsync), "desiredCoordinateSystem")]
    [InlineData(nameof(BriosaClient.CreatePointsToObjectsMapAsync), "groups")]
    [InlineData(nameof(BriosaClient.CreatePointsToObjectsMapAsync), "points")]
    [InlineData(nameof(BriosaClient.DoRelationshipFitAsync), "motionToAllow")]
    [InlineData(nameof(BriosaGdtOperations.EnableDisableDatumAlignmentForFeatureCheckAsync), "alignment")]
    [InlineData(nameof(BriosaGdtOperations.FeatureInspectionAutoFilterAsync), "offsetDirection")]
    [InlineData(nameof(BriosaConstructionOperations.GetGradientAtProjectedPointOnSurfaceEdgeAsync), "edgeOffsetDirection")]
    [InlineData(nameof(BriosaInstrumentOperations.LocateInstrumentsUsmnAsync), "showUsmnDialog")]
    [InlineData(nameof(BriosaConstructionOperations.MakeCollectionItemNameRefListWildcardSelectionAsync), "itemType")]
    [InlineData(nameof(BriosaConstructionOperations.MakeCollectionObjectNameRefListByTypeAndColorAsync), "objectType")]
    [InlineData(nameof(BriosaConstructionOperations.MakeCollectionObjectNameRefListByTypeAsync), "objectType")]
    [InlineData(nameof(BriosaConstructionOperations.MakeCollectionObjectNameRefListRuntimeSelectAsync), "objectType")]
    [InlineData(nameof(BriosaConstructionOperations.MakeCollectionObjectNameRefListWildcardSelectionAsync), "objectType")]
    [InlineData(nameof(BriosaConstructionOperations.MakeCollectionObjectNameRuntimeSelectAsync), "objectType")]
    [InlineData(nameof(BriosaClient.MakeDynamicCircleRelationshipAsync), "constructionMode")]
    [InlineData(nameof(BriosaClient.MakeDynamicEllipseRelationshipAsync), "constructionMode")]
    [InlineData(nameof(BriosaClient.MakeDynamicLineRelationshipAsync), "constructionMode")]
    [InlineData(nameof(BriosaClient.MakeDynamicPlaneRelationshipAsync), "constructionMode")]
    [InlineData(nameof(BriosaClient.MakeDynamicPointRelationshipAsync), "constructionMode")]
    [InlineData(nameof(BriosaClient.MakeDynamicPointRelationshipAsync), "thirdReferenceGeometry")]
    [InlineData(nameof(BriosaClient.MakeFrameToFrameRelationshipAsync), "orientationTolerance")]
    [InlineData(nameof(BriosaClient.MakeFrameToFrameRelationshipAsync), "positionTolerance")]
    [InlineData(nameof(BriosaClient.MakeGroupsToObjectsRelationshipAsync), "projectionOptions")]
    [InlineData(nameof(BriosaClient.MakePointCloudsToObjectsRelationshipAsync), "projectionOptions")]
    [InlineData(nameof(BriosaClient.MakePointsToObjectsRelationshipAsync), "projectionOptions")]
    [InlineData(nameof(BriosaClient.MoveCollectionsByMinimizingRelationshipsAsync), "motionToAllow")]
    [InlineData(nameof(BriosaClient.RelationshipWatchWindowTemplateAsync), "udpNetworkTransmitSettings")]
    [InlineData(nameof(BriosaClient.RelationshipWatchWindowTemplateAsync), "watchWindowTemplateName")]
    [InlineData(nameof(BriosaGdtOperations.SetGdtOptionsAsync), "distanceBetweenMode")]
    [InlineData(nameof(BriosaGdtOperations.SetGdtOptionsAsync), "evaluationMethod")]
    [InlineData(nameof(BriosaClient.SetObjectToObjectDirectionRelationshipTolerancesAsync), "angleBetweenVectorsTolerances")]
    [InlineData(nameof(BriosaClient.SetObjectToObjectDirectionRelationshipTolerancesAsync), "mutualPerpendicularLengthTolerances")]
    [InlineData(nameof(BriosaInstrumentOperations.SetObservationCollimationShotOptionsAsync), "targetedInstrument")]
    [InlineData(nameof(BriosaClient.SetPointsToPointsRelationshipAssociatedDataAsync), "actualPoints")]
    [InlineData(nameof(BriosaClient.SetPointsToPointsRelationshipAssociatedDataAsync), "nominalPoints")]
    [InlineData(nameof(BriosaClient.SetRelationshipAssociatedDataAsync), "individualPoints")]
    [InlineData(nameof(BriosaClient.SetRelationshipAssociatedDataAsync), "objects")]
    [InlineData(nameof(BriosaClient.SetRelationshipAssociatedDataAsync), "pointClouds")]
    [InlineData(nameof(BriosaClient.SetRelationshipAssociatedDataAsync), "pointGroups")]
    [InlineData(nameof(BriosaInstrumentOperations.SetTargetComputationOptionsAsync), "computationMethod")]
    public async Task RequiredArgumentsHaveNoDefaultAndAreSentAsGiven(string methodName, string parameterName)
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var (target, method) = Facade(client, methodName);

        await AssertRequiredAndSentAsync(transport, target, method, parameterName);
    }

    // C3 rows for an operation that only one target's SpatialAnalyzer offers.
    [Theory]
    [InlineData("2026.1.0529.7", "ExtractGeometryFromPointCloudsAsync", "geometryType")]
    [InlineData("2026.1.0529.7", "ExtractGeometryFromPointCloudsAsync", "boundingPoints")]
    public async Task TargetOnlyRequiredArgumentsHaveNoDefaultAndAreSentAsGiven(
        string spatialAnalyzerTarget,
        string methodName,
        string parameterName)
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var facades = FacadeTargets(client).Where(item => item.Method.Name == methodName).ToArray();
        if (Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget != spatialAnalyzerTarget)
        {
            Assert.Empty(facades);
            return;
        }

        var (target, method) = Assert.Single(facades);
        await AssertRequiredAndSentAsync(transport, target, method, parameterName);
    }

    // C3: no overload fills in an input the server requires, such as the
    // identity transform or a watch window without its template.
    [Theory]
    [InlineData(nameof(BriosaConstructionOperations.ConstructFrameAsync), "transformInWorkingCoordinates")]
    [InlineData(nameof(BriosaConstructionOperations.ConstructFrameFromTransformInWorldAsync), "transformInWorldCoordinates")]
    [InlineData(nameof(BriosaRobotCalibrationApplianceNodeOperations.SetCalibrationApplianceNodeMeasurementOffsetTransformAsync), "measurementOffsetTransform")]
    [InlineData(nameof(BriosaRobotOperations.SetRobotCalibrationMeasurementOffsetInToolFrameAsync), "measurementFrame")]
    [InlineData(nameof(BriosaRobotOperations.SetRobotCalibrationToolFrameAsync), "toolFrame")]
    [InlineData(nameof(BriosaRobotOperations.SetRobotMachineBaseTransformAsync), "destinationTransform")]
    [InlineData(nameof(BriosaClient.RelationshipWatchWindowTemplateAsync), "watchWindowTemplateName")]
    public void NoOverloadFillsInAServerRequiredInput(string methodName, string parameterName)
    {
        var overloads = FacadeTypes()
            .SelectMany(type => type.GetMethods(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .Where(method => method.Name == methodName)
            .ToArray();

        var method = Assert.Single(overloads);
        Assert.Contains(method.GetParameters(), item => item.Name == parameterName && !item.HasDefaultValue);
    }

    // C3: members of input values that the server requires have no default.
    [Theory]
    [InlineData(typeof(FilterProximitySettings), nameof(FilterProximitySettings.SurfaceProximityMode))]
    [InlineData(typeof(FilterProximitySettings), nameof(FilterProximitySettings.PlanarProximityMode))]
    [InlineData(typeof(FilterProximitySettings), nameof(FilterProximitySettings.RadialProximityMode))]
    [InlineData(typeof(ReportOutputOptions), nameof(ReportOutputOptions.OutputType))]
    [InlineData(typeof(Transform), nameof(Transform.Values))]
    [InlineData(typeof(MakeGdtDatumAnnotationOptions), nameof(MakeGdtDatumAnnotationOptions.Objects))]
    [InlineData(typeof(MakeGdtDatumAnnotationOptions), nameof(MakeGdtDatumAnnotationOptions.GeometryRelationships))]
    [InlineData(typeof(MakeGdtDatumAnnotationOptions), nameof(MakeGdtDatumAnnotationOptions.AuxiliaryObject))]
    [InlineData(typeof(MakeGdtDatumAnnotationOptions), nameof(MakeGdtDatumAnnotationOptions.AuxiliaryGeometryRelationship))]
    [InlineData(typeof(MakeGdtFeatureCheckAnnotationOptions), nameof(MakeGdtFeatureCheckAnnotationOptions.Objects))]
    [InlineData(typeof(MakeGdtFeatureCheckAnnotationOptions), nameof(MakeGdtFeatureCheckAnnotationOptions.GeometryRelationships))]
    [InlineData(typeof(MakeGdtFeatureCheckAnnotationOptions), nameof(MakeGdtFeatureCheckAnnotationOptions.AuxiliaryObject))]
    [InlineData(typeof(MakeGdtFeatureCheckAnnotationOptions), nameof(MakeGdtFeatureCheckAnnotationOptions.AuxiliaryGeometryRelationship))]
    public void ServerRequiredMembersAreRequired(Type type, string memberName)
    {
        ArgumentNullException.ThrowIfNull(type);
        var property = type.GetProperty(memberName)!;

        Assert.True(
            property.IsDefined(typeof(RequiredMemberAttribute)),
            $"{type.Name}.{memberName} is not required.");
        Assert.Equal(
            NullabilityState.NotNull,
            new NullabilityInfoContext().Create(property).WriteState);
    }

    // C3: the watch window's UDP settings are a required facade argument, so the
    // options value no longer fills them in.
    [Fact]
    public void WatchWindowTemplateOptionsDoNotFillInTheUdpSettings() =>
        Assert.Null(typeof(RelationshipWatchWindowTemplateOptions).GetProperty("UdpNetworkTransmitSettings"));

    // C3: report output options name exactly one destination, as the server
    // requires whenever they are given. The client rejects neither, as it already
    // rejects both, before any call.
    [Theory]
    [InlineData(false, false, "report-output-destination-missing")]
    [InlineData(true, true, "report-output-destination-conflict")]
    public async Task ReportOutputOptionsWithoutExactlyOneDestinationFailBeforeAnyCall(
        bool externalPath,
        bool embeddedFile,
        string diagnosticCode)
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var (target, method) = Facade(client, nameof(BriosaClient.GenerateQuickReportFromTabOrderAsync));
        var calls = transport.Calls.Count;

        var failure = await Record.ExceptionAsync(() => InvokeAsync(
            transport,
            target,
            method,
            ("reportOutputOptions", ReportOptions(externalPath, embeddedFile))));

        var argument = Assert.IsType<ArgumentException>(failure);
        Assert.StartsWith(diagnosticCode, argument.Message, StringComparison.Ordinal);
        Assert.Equal(calls, transport.Calls.Count);
    }

    [Theory]
    [InlineData(true, Transport.ReportOutputOptions.DestinationOneofCase.ExternalPath)]
    [InlineData(false, Transport.ReportOutputOptions.DestinationOneofCase.EmbeddedFile)]
    public async Task ReportOutputOptionsWithOneDestinationAreSentAsGiven(
        bool externalPath,
        Transport.ReportOutputOptions.DestinationOneofCase expected)
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var (target, method) = Facade(client, nameof(BriosaClient.GenerateQuickReportFromTabOrderAsync));

        var sent = await InvokeAsync(
            transport,
            target,
            method,
            ("reportOutputOptions", ReportOptions(externalPath, !externalPath)));

        var options = Assert.IsType<Transport.GenerateQuickReportFromTabOrderRequest>(sent).ReportOutputOptions;
        Assert.Equal(expected, options.DestinationCase);
        Assert.Equal(Transport.ReportOutputType.SaReport, options.OutputType);
    }

    private static ReportOutputOptions ReportOptions(bool externalPath, bool embeddedFile) => new()
    {
        OutputType = ReportOutputType.SaReport,
        ExternalPath = externalPath ? "C:\\Reports\\Inspection.pdf" : null,
        EmbeddedFile = embeddedFile
            ? new EmbeddedReportFile { CollectionName = "Inspection", FileName = "Report" }
            : null,
    };

    // C3: an identity transform is sent only when the caller passes one. Each read
    // returns a fresh value, so no caller can change another's identity.
    [Fact]
    public void TransformIdentityIsAnExplicitFreshValue()
    {
        var property = typeof(Transform).GetProperty("Identity", BindingFlags.Public | BindingFlags.Static);

        Assert.NotNull(property);
        var first = Assert.IsType<Transform>(property.GetValue(null));
        var second = Assert.IsType<Transform>(property.GetValue(null));
        Assert.Equal([1d, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1], first.Values);
        Assert.NotSame(first.Values, second.Values);
    }

    private static async Task AssertRequiredAndSentAsync(
        FakeTransport transport,
        object target,
        MethodInfo method,
        string parameterName)
    {
        var parameter = Assert.Single(method.GetParameters(), item => item.Name == parameterName);
        Assert.False(parameter.HasDefaultValue, $"{method.Name}.{parameterName} has a default.");
        Assert.Equal(
            NullabilityState.NotNull,
            new NullabilityInfoContext().Create(parameter).WriteState);

        var sent = await InvokeAsync(transport, target, method).ConfigureAwait(false);
        Assert.True(HasField(sent, parameterName));
    }

    // A required list given as null fails on the client before any call; an
    // empty one is sent empty for the server to reject.
    [Fact]
    public async Task RequiredListsFailOnNullBeforeAnyCall()
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var (target, method) = Facade(client, nameof(BriosaInstrumentOperations.LocateInstrumentGroupToSurfaceQuickFitAsync));
        var calls = transport.Calls.Count;

        var failure = await Record.ExceptionAsync(
            () => InvokeAsync(transport, target, method, ("otherObjectsToTransform", null)));

        Assert.IsType<ArgumentNullException>(failure);
        Assert.Equal(calls, transport.Calls.Count);
        var sent = await InvokeAsync(
            transport,
            target,
            method,
            ("otherObjectsToTransform", Array.Empty<CollectionObjectName>()));
        Assert.Empty(Assert.IsType<Transport.LocateInstrumentGroupToSurfaceQuickFitRequest>(sent)
            .OtherObjectsToTransform);
    }

    // Probe 33: USMN runs with the excluded-groups setter skipped, so the list
    // stays optional and an omitted one is sent empty.
    [Fact]
    public void UsmnExcludedGroupsStayOptional()
    {
        var parameter = typeof(BriosaInstrumentOperations)
            .GetMethod(nameof(BriosaInstrumentOperations.LocateInstrumentsUsmnAsync))!
            .GetParameters()
            .Single(item => item.Name == "excludedGroups");

        Assert.True(parameter.HasDefaultValue);
        Assert.Null(parameter.DefaultValue);
    }

    // A caller that wants no relationship limits passes them explicitly.
    [Fact]
    public async Task NoLimitsIsSentOnlyWhenPassedAndDisablesEveryLimit()
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var relationship = new CollectionItemName
        {
            CollectionName = "Inspection",
            ItemName = "P2P",
            ItemType = ItemType.Relationship,
        };

        await client.MakePointToPointRelationshipAsync(
            relationship,
            SamplePoint(),
            SamplePoint() with { TargetName = "P2" },
            tolerance: ToleranceVectorOptions.NoLimits,
            constraint: ToleranceVectorOptions.NoLimits);

        var request = Assert.IsType<Transport.MakePointToPointRelationshipRequest>(
            transport.LastOperationRequest);
        foreach (var options in new[] { request.Tolerance, request.Constraint })
        {
            var limits = new[]
            {
                options.HighX, options.HighY, options.HighZ, options.HighMagnitude,
                options.LowX, options.LowY, options.LowZ, options.LowMagnitude,
            };
            Assert.All(limits, limit =>
            {
                Assert.True(limit.HasEnabled);
                Assert.False(limit.Enabled);
            });
        }

        Assert.Equal(new ToleranceVectorOptions(), ToleranceVectorOptions.NoLimits);
    }

    // R2: set_automatically is required. The three values stay optional with no
    // 0 default; omitted ones stay unset whatever set_automatically is.
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task WeatherValuesStayUnsetWhenOmitted(bool setAutomatically)
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var (target, method) = Facade(client, nameof(BriosaInstrumentOperations.SetInstrumentWeatherSettingAsync));
        foreach (var name in new[] { "temperature", "pressure", "relativeHumidity" })
        {
            var parameter = Assert.Single(method.GetParameters(), item => item.Name == name);
            Assert.True(parameter.HasDefaultValue);
            Assert.Null(parameter.DefaultValue);
        }

        var sent = Assert.IsType<Transport.SetInstrumentWeatherSettingRequest>(
            await InvokeAsync(transport, target, method, ("setAutomatically", setAutomatically)));
        Assert.True(sent.HasSetAutomatically);
        Assert.Equal(setAutomatically, sent.SetAutomatically);
        Assert.False(sent.HasTemperature || sent.HasPressure || sent.HasRelativeHumidity);

        sent = Assert.IsType<Transport.SetInstrumentWeatherSettingRequest>(await InvokeAsync(
            transport,
            target,
            method,
            ("setAutomatically", setAutomatically),
            ("temperature", 68.0),
            ("pressure", 760.0),
            ("relativeHumidity", 45.0)));
        Assert.Equal((68.0, 760.0, 45.0), (sent.Temperature, sent.Pressure, sent.RelativeHumidity));
    }

    // R5: no fill-in sends every model-link parameter as 0 or false.
    [Fact]
    public void ModelLinkParametersNeedAFullConfiguration()
    {
        var overload = Assert.Single(
            typeof(BriosaRobotOperations).GetMethods(),
            method => method.Name == nameof(BriosaRobotOperations.SetRobotMachineModelLinkParametersAsync));
        Assert.Contains(
            overload.GetParameters(),
            parameter => parameter.ParameterType == typeof(RobotModelLinkConfiguration));
        Assert.All(
            typeof(RobotModelLinkConfiguration).GetProperties(),
            property => Assert.True(
                property.IsDefined(typeof(RequiredMemberAttribute)),
                $"{property.Name} has a default."));
        Assert.Equal(25, typeof(RobotModelLinkConfiguration).GetProperties().Length);
    }

    // #45: a successful quick fit reports its errors instead of a protocol error.
    [Fact]
    public async Task QuickFitReportsItsFitErrors()
    {
        var transport = new FakeTransport
        {
            OperationResponse = new Transport.LocateInstrumentGroupToSurfaceQuickFitResult
            {
                RmsError = 0.01,
                MaximumAbsoluteError = 0.03,
            },
        };
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var instrument = new CollectionInstrumentId { CollectionName = "Inspection", InstrumentId = 1 };
        CollectionObjectName Group(string name) => new()
        {
            CollectionName = "Inspection",
            ObjectName = name,
            ObjectType = ObjectType.PointGroup,
        };

        var fit = await client.InstrumentOperations.LocateInstrumentGroupToSurfaceQuickFitAsync(
            instrument,
            Group("Measured"),
            Group("Surface Points"),
            Group("Surface") with { ObjectType = ObjectType.Surface },
            otherObjectsToTransform: [Group("Fixture")]);

        Assert.Equal(0.01, fit.RmsError);
        Assert.Equal(0.03, fit.MaximumError);
    }

    // #45 item 3: a fully populated response for every operation maps through its
    // facade without error.
    [Fact]
    public async Task EveryOperationMapsAFullyPopulatedResponse()
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var targets = FacadeTargets(client);
        var failures = new List<string>();
        var mapped = 0;

        foreach (var rpc in OperationServices().SelectMany(service => service.Methods))
        {
            foreach (var (target, method) in targets.Where(item => item.Method.Name == rpc.Name + "Async"))
            {
                transport.OperationResponse = PopulatedMessage(rpc.OutputType, depth: 0);
                var calls = transport.Calls.Count;
                var (_, failure) = await CallAsync(transport, target, method);
                if (failure is not null)
                {
                    failures.Add(transport.Calls.Count > calls
                        ? $"{method.Name}: {failure.GetType().Name} {failure.Message}"
                        : $"{method.Name}: not called ({failure.GetType().Name} {failure.Message})");
                    continue;
                }

                mapped++;
            }
        }

        Assert.True(mapped > 900, $"Only {mapped} operations were mapped.");
        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
    }

    private static (object Target, MethodInfo Method) Facade(BriosaClient client, string methodName) =>
        Assert.Single(FacadeTargets(client), item => item.Method.Name == methodName);

    private static (object Target, MethodInfo Method)[] FacadeTargets(BriosaClient client)
    {
        object[] targets =
        [
            client,
            client.ConstructionOperations,
            client.GdtOperations,
            client.InstrumentOperations,
            client.RobotCalibrationApplianceNodeOperations,
            client.RobotOperations,
        ];
        return targets
            .SelectMany(target => target.GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(method => method.Name.EndsWith("Async", StringComparison.Ordinal) &&
                    method.GetParameters() is [.., { Name: "onMpCompletion" }, _])
                .Select(method => (target, method)))
            .ToArray();
    }

    // Calls a facade with sample values for its required arguments, the named
    // overrides, and the defaults of everything else; returns the request sent.
    // A failure after dispatch comes from mapping the fake's response, which only
    // the populated-response test checks.
    private static async Task<IMessage> InvokeAsync(
        FakeTransport transport,
        object target,
        MethodInfo method,
        params (string Name, object? Value)[] overrides)
    {
        var (request, failure) = await CallAsync(transport, target, method, overrides)
            .ConfigureAwait(false);
        return request ?? throw failure ??
            new InvalidOperationException($"{method.Name} sent no request.");
    }

    private static async Task<(IMessage? Request, Exception? Failure)> CallAsync(
        FakeTransport transport,
        object target,
        MethodInfo method,
        params (string Name, object? Value)[] overrides)
    {
        var arguments = method.GetParameters()
            .Select(parameter => overrides.Any(item => item.Name == parameter.Name)
                ? overrides.Single(item => item.Name == parameter.Name).Value
                : parameter.HasDefaultValue
                    ? Type.Missing
                    : SampleArgument(parameter.ParameterType))
            .ToArray();
        var calls = transport.Calls.Count;
        var failure = await Record.ExceptionAsync(
            () => (Task)method.Invoke(target, arguments)!).ConfigureAwait(false);
        failure = (failure as TargetInvocationException)?.InnerException ?? failure;
        return (transport.Calls.Count > calls ? transport.LastOperationRequest : null, failure);
    }

    private static FieldDescriptor RequestField(IMessage request, string argument) =>
        request.Descriptor.Fields.InDeclarationOrder().Single(field =>
            string.Equals(field.JsonName, argument, StringComparison.OrdinalIgnoreCase));

    private static object? FieldValue(IMessage request, string argument) =>
        RequestField(request, argument).Accessor.GetValue(request);

    private static bool HasField(IMessage request, string argument)
    {
        var field = RequestField(request, argument);
        return field.IsRepeated
            ? ((IList)field.Accessor.GetValue(request)).Count > 0
            : field.HasPresence
                ? field.Accessor.HasValue(request)
                : field.Accessor.GetValue(request) is not null;
    }

    // A response with every field set to a valid sample: one item per repeated
    // field, the first field of each oneof, and a plain MP success.
    private static IMessage PopulatedMessage(MessageDescriptor descriptor, int depth)
    {
        var message = (IMessage)Activator.CreateInstance(descriptor.ClrType)!;
        if (descriptor.Name == "Transform")
        {
            var values = (IList)descriptor.FindFieldByName("values").Accessor.GetValue(message);
            foreach (var value in Enumerable.Range(0, 16).Select(index => index % 5 == 0 ? 1.0 : 0.0))
            {
                values.Add(value);
            }

            return message;
        }

        foreach (var field in descriptor.Fields.InFieldNumberOrder())
        {
            if (field.Name == "execution" && field.FieldType == FieldType.Message)
            {
                field.Accessor.SetValue(message, FakeTransport.SucceededExecution());
                continue;
            }

            if (field.RealContainingOneof is { } oneof && oneof.Fields[0] != field)
            {
                continue;
            }

            if (field.FieldType == FieldType.Message && depth > 8)
            {
                continue;
            }

            var sample = SampleWireValue(field, depth);
            if (field.IsRepeated)
            {
                ((IList)field.Accessor.GetValue(message)).Add(sample);
            }
            else
            {
                field.Accessor.SetValue(message, sample);
            }
        }

        return message;
    }

    private static object SampleWireValue(FieldDescriptor field, int depth) => field.FieldType switch
    {
        FieldType.Message => PopulatedMessage(field.MessageType, depth + 1),
        FieldType.Enum => Enum.ToObject(
            field.EnumType.ClrType,
            field.EnumType.Values.First(value => value.Number != 0).Number),
        FieldType.String => "Sample",
        FieldType.Bool => true,
        FieldType.Double => 1.5,
        FieldType.Float => 1.5f,
        FieldType.Int32 or FieldType.SInt32 or FieldType.SFixed32 => 1,
        FieldType.Int64 or FieldType.SInt64 or FieldType.SFixed64 => 1L,
        FieldType.UInt32 or FieldType.Fixed32 => 1U,
        FieldType.UInt64 or FieldType.Fixed64 => 1UL,
        FieldType.Bytes => ByteString.CopyFromUtf8("Sample"),
        _ => throw new InvalidOperationException($"No sample for {field.FullName}."),
    };
}
