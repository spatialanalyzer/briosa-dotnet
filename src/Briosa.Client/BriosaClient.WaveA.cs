// Drafted mechanically from the approved Briosa documentation contract.
#pragma warning disable CA1720 // Exact MP argument name is "Object".
#pragma warning disable CS1591 // Full API reference is maintained in briosa-docs.
using Transport = Briosa.Client.Transport;

namespace Briosa;

public sealed partial class BriosaClient
{
    public Task<double> AngleBetweenLineAndPlaneAsync(
        CollectionObjectName selectedLine,
        CollectionObjectName selectedPlane,
        double nominalAngle = 0.000000,
        double angleTolerance00ForNone = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AngleBetweenLineAndPlaneRequest(),
            new Dictionary<string, object?>
            {
                ["selected_line"] = selectedLine,
                ["selected_plane"] = selectedPlane,
                ["nominal_angle"] = nominalAngle,
                ["angle_tolerance_0_0_for_none"] = angleTolerance00ForNone,
            });
        return InvokeOperationAsync<double>(
            "briosa.AnalysisOperations",
            "AngleBetweenLineAndPlane",
            request,
            Transport.AngleBetweenLineAndPlaneResult.Parser,
            cancellationToken);
    }

    public Task<double> AngleBetweenTwoLinesAsync(
        CollectionObjectName line1,
        CollectionObjectName line2,
        double nominalAngle = 0.000000,
        double angleTolerance00ForNone = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AngleBetweenTwoLinesRequest(),
            new Dictionary<string, object?>
            {
                ["line_1"] = line1,
                ["line_2"] = line2,
                ["nominal_angle"] = nominalAngle,
                ["angle_tolerance_0_0_for_none"] = angleTolerance00ForNone,
            });
        return InvokeOperationAsync<double>(
            "briosa.AnalysisOperations",
            "AngleBetweenTwoLines",
            request,
            Transport.AngleBetweenTwoLinesResult.Parser,
            cancellationToken);
    }

    public Task<double> AngleBetweenTwoPlanesNormalsAsync(
        CollectionObjectName planeA,
        CollectionObjectName planeB,
        double nominalAngle = 0.000000,
        double angleTolerance00ForNone = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AngleBetweenTwoPlanesNormalsRequest(),
            new Dictionary<string, object?>
            {
                ["plane_a"] = planeA,
                ["plane_b"] = planeB,
                ["nominal_angle"] = nominalAngle,
                ["angle_tolerance_0_0_for_none"] = angleTolerance00ForNone,
            });
        return InvokeOperationAsync<double>(
            "briosa.AnalysisOperations",
            "AngleBetweenTwoPlanesNormals",
            request,
            Transport.AngleBetweenTwoPlanesNormalsResult.Parser,
            cancellationToken);
    }

    public Task<BestFitTransformationGroupToGroupResult> BestFitTransformationGroupToGroupAsync(
        CollectionObjectName referenceGroup,
        CollectionObjectName correspondingGroup,
        bool showInterface,
        double rmsTolerance00ForNone,
        double maximumAbsoluteTolerance00ForNone,
        bool allowScale,
        bool allowX,
        bool allowY,
        bool allowZ,
        bool allowRx,
        bool allowRy,
        bool allowRz,
        bool lockDegreesOfFreedom,
        bool generateEvent,
        FileReference filePathForCsvTextReportRequiresShowInterfaceTrue,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.BestFitTransformationGroupToGroupRequest(),
            new Dictionary<string, object?>
            {
                ["reference_group"] = referenceGroup,
                ["corresponding_group"] = correspondingGroup,
                ["show_interface"] = showInterface,
                ["rms_tolerance_0_0_for_none"] = rmsTolerance00ForNone,
                ["maximum_absolute_tolerance_0_0_for_none"] = maximumAbsoluteTolerance00ForNone,
                ["allow_scale"] = allowScale,
                ["allow_x"] = allowX,
                ["allow_y"] = allowY,
                ["allow_z"] = allowZ,
                ["allow_rx"] = allowRx,
                ["allow_ry"] = allowRy,
                ["allow_rz"] = allowRz,
                ["lock_degrees_of_freedom"] = lockDegreesOfFreedom,
                ["generate_event"] = generateEvent,
                ["file_path_for_csv_text_report_requires_show_interface_true"] = filePathForCsvTextReportRequiresShowInterfaceTrue,
            });
        return InvokeOperationAsync<BestFitTransformationGroupToGroupResult>(
            "briosa.AnalysisOperations",
            "BestFitTransformationGroupToGroup",
            request,
            Transport.BestFitTransformationGroupToGroupResult.Parser,
            cancellationToken);
    }

    public Task<ComputeGroupToGroupOrientationRxRyRzResult> ComputeGroupToGroupOrientationRxRyRzAsync(
        CollectionObjectName referenceGroup,
        CollectionObjectName correspondingGroup,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ComputeGroupToGroupOrientationRxRyRzRequest(),
            new Dictionary<string, object?>
            {
                ["reference_group"] = referenceGroup,
                ["corresponding_group"] = correspondingGroup,
            });
        return InvokeOperationAsync<ComputeGroupToGroupOrientationRxRyRzResult>(
            "briosa.AnalysisOperations",
            "ComputeGroupToGroupOrientationRxRyRz",
            request,
            Transport.ComputeGroupToGroupOrientationRxRyRzResult.Parser,
            cancellationToken);
    }

    public Task<CreatePointUncertaintyCloudPointSetsResult> CreatePointUncertaintyCloudPointSetsAsync(
        IEnumerable<PointName> pointNameList,
        int numberOfSamples = 1000,
        string uncertaintyReferenceFrameMode = "With respect to WORLD",
        string groupingMode = "Group per point",
        string pointSetMode = "Point clouds",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreatePointUncertaintyCloudPointSetsRequest(),
            new Dictionary<string, object?>
            {
                ["point_name_list"] = pointNameList,
                ["number_of_samples"] = numberOfSamples,
                ["uncertainty_reference_frame_mode"] = uncertaintyReferenceFrameMode,
                ["grouping_mode"] = groupingMode,
                ["point_set_mode"] = pointSetMode,
            });
        return InvokeOperationAsync<CreatePointUncertaintyCloudPointSetsResult>(
            "briosa.AnalysisOperations",
            "CreatePointUncertaintyCloudPointSets",
            request,
            Transport.CreatePointUncertaintyCloudPointSetsResult.Parser,
            cancellationToken);
    }

    public Task CreatePointUncertaintyFieldsAsync(
        IEnumerable<PointName> pointNameList,
        int numberOfSamples = 1000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreatePointUncertaintyFieldsRequest(),
            new Dictionary<string, object?>
            {
                ["point_name_list"] = pointNameList,
                ["number_of_samples"] = numberOfSamples,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "CreatePointUncertaintyFields",
            request,
            Transport.CreatePointUncertaintyFieldsResult.Parser,
            cancellationToken);
    }

    public Task FitGeometryToPointGroupAsync(
        GeometryType geometryType,
        CollectionObjectName groupToFit,
        CollectionObjectName resultingObjectName,
        string fitProfileName,
        bool reportDeviations,
        double fitInterfaceTolerance10UseProfile,
        bool ignoreOutOfTolerancePoints,
        CollectionObjectName startingConditionGeometryOptional,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FitGeometryToPointGroupRequest(),
            new Dictionary<string, object?>
            {
                ["geometry_type"] = geometryType,
                ["group_to_fit"] = groupToFit,
                ["resulting_object_name"] = resultingObjectName,
                ["fit_profile_name"] = fitProfileName,
                ["report_deviations"] = reportDeviations,
                ["fit_interface_tolerance_1_0_use_profile"] = fitInterfaceTolerance10UseProfile,
                ["ignore_out_of_tolerance_points"] = ignoreOutOfTolerancePoints,
                ["starting_condition_geometry_optional"] = startingConditionGeometryOptional,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "FitGeometryToPointGroup",
            request,
            Transport.FitGeometryToPointGroupResult.Parser,
            cancellationToken);
    }

    public Task FitGeometryToPointGroupProjectedToPlaneAsync(
        GeometryType geometryType,
        CollectionObjectName groupToFit,
        CollectionObjectName planeName,
        CollectionObjectName resultingObjectName,
        string fitProfileName,
        bool reportDeviations,
        double fitInterfaceTolerance10UseProfile,
        bool ignoreOutOfTolerancePoints,
        CollectionObjectName startingConditionGeometryOptional,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FitGeometryToPointGroupProjectedToPlaneRequest(),
            new Dictionary<string, object?>
            {
                ["geometry_type"] = geometryType,
                ["group_to_fit"] = groupToFit,
                ["plane_name"] = planeName,
                ["resulting_object_name"] = resultingObjectName,
                ["fit_profile_name"] = fitProfileName,
                ["report_deviations"] = reportDeviations,
                ["fit_interface_tolerance_1_0_use_profile"] = fitInterfaceTolerance10UseProfile,
                ["ignore_out_of_tolerance_points"] = ignoreOutOfTolerancePoints,
                ["starting_condition_geometry_optional"] = startingConditionGeometryOptional,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "FitGeometryToPointGroupProjectedToPlane",
            request,
            Transport.FitGeometryToPointGroupProjectedToPlaneResult.Parser,
            cancellationToken);
    }

    public Task FitGeometryToPointsAsync(
        GeometryType geometryType,
        IEnumerable<PointName> pointsToFit,
        CollectionObjectName resultingObjectName,
        string fitProfileName,
        bool reportDeviations,
        double fitInterfaceTolerance10UseProfile,
        bool ignoreOutOfTolerancePoints,
        CollectionObjectName startingConditionGeometryOptional,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FitGeometryToPointsRequest(),
            new Dictionary<string, object?>
            {
                ["geometry_type"] = geometryType,
                ["points_to_fit"] = pointsToFit,
                ["resulting_object_name"] = resultingObjectName,
                ["fit_profile_name"] = fitProfileName,
                ["report_deviations"] = reportDeviations,
                ["fit_interface_tolerance_1_0_use_profile"] = fitInterfaceTolerance10UseProfile,
                ["ignore_out_of_tolerance_points"] = ignoreOutOfTolerancePoints,
                ["starting_condition_geometry_optional"] = startingConditionGeometryOptional,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "FitGeometryToPoints",
            request,
            Transport.FitGeometryToPointsResult.Parser,
            cancellationToken);
    }

    public Task<GetBSplinePropertiesResult> GetBSplinePropertiesAsync(
        CollectionObjectName bSplineName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetBSplinePropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["b_spline_name"] = bSplineName,
            });
        return InvokeOperationAsync<GetBSplinePropertiesResult>(
            "briosa.AnalysisOperations",
            "GetBSplineProperties",
            request,
            Transport.GetBSplinePropertiesResult.Parser,
            cancellationToken);
    }

    public Task<GetCirclePropertiesResult> GetCirclePropertiesAsync(
        CollectionObjectName circleName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCirclePropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["circle_name"] = circleName,
            });
        return InvokeOperationAsync<GetCirclePropertiesResult>(
            "briosa.AnalysisOperations",
            "GetCircleProperties",
            request,
            Transport.GetCirclePropertiesResult.Parser,
            cancellationToken);
    }

    public Task<GetConePropertiesResult> GetConePropertiesAsync(
        CollectionObjectName coneName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetConePropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["cone_name"] = coneName,
            });
        return InvokeOperationAsync<GetConePropertiesResult>(
            "briosa.AnalysisOperations",
            "GetConeProperties",
            request,
            Transport.GetConePropertiesResult.Parser,
            cancellationToken);
    }

    public Task<GetCoordinateForIthPointInPointSetResult> GetCoordinateForIthPointInPointSetAsync(
        CollectionObjectName pointSet,
        int pointSetIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCoordinateForIthPointInPointSetRequest(),
            new Dictionary<string, object?>
            {
                ["point_set"] = pointSet,
                ["point_set_index"] = pointSetIndex,
            });
        return InvokeOperationAsync<GetCoordinateForIthPointInPointSetResult>(
            "briosa.AnalysisOperations",
            "GetCoordinateForIthPointInPointSet",
            request,
            Transport.GetCoordinateForIthPointInPointSetResult.Parser,
            cancellationToken);
    }

    public Task<GetCylinderPropertiesResult> GetCylinderPropertiesAsync(
        CollectionObjectName cylinderName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCylinderPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["cylinder_name"] = cylinderName,
            });
        return InvokeOperationAsync<GetCylinderPropertiesResult>(
            "briosa.AnalysisOperations",
            "GetCylinderProperties",
            request,
            Transport.GetCylinderPropertiesResult.Parser,
            cancellationToken);
    }

    public Task<GetEllipsePropertiesResult> GetEllipsePropertiesAsync(
        CollectionObjectName ellipseName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetEllipsePropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["ellipse_name"] = ellipseName,
            });
        return InvokeOperationAsync<GetEllipsePropertiesResult>(
            "briosa.AnalysisOperations",
            "GetEllipseProperties",
            request,
            Transport.GetEllipsePropertiesResult.Parser,
            cancellationToken);
    }

    public Task<GetEulerParametersForFrameResult> GetEulerParametersForFrameAsync(
        CollectionObjectName frame,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetEulerParametersForFrameRequest(),
            new Dictionary<string, object?>
            {
                ["frame"] = frame,
            });
        return InvokeOperationAsync<GetEulerParametersForFrameResult>(
            "briosa.AnalysisOperations",
            "GetEulerParametersForFrame",
            request,
            Transport.GetEulerParametersForFrameResult.Parser,
            cancellationToken);
    }

    public Task<GetEulerParametersForIthFrameInFrameSetResult> GetEulerParametersForIthFrameInFrameSetAsync(
        CollectionObjectName frameSet,
        int frameSetIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetEulerParametersForIthFrameInFrameSetRequest(),
            new Dictionary<string, object?>
            {
                ["frame_set"] = frameSet,
                ["frame_set_index"] = frameSetIndex,
            });
        return InvokeOperationAsync<GetEulerParametersForIthFrameInFrameSetResult>(
            "briosa.AnalysisOperations",
            "GetEulerParametersForIthFrameInFrameSet",
            request,
            Transport.GetEulerParametersForIthFrameInFrameSetResult.Parser,
            cancellationToken);
    }

    public Task<string> GetIthCollectionNameAsync(
        int collectionIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetIthCollectionNameRequest(),
            new Dictionary<string, object?>
            {
                ["collection_index"] = collectionIndex,
            });
        return InvokeOperationAsync<string>(
            "briosa.AnalysisOperations",
            "GetIthCollectionName",
            request,
            Transport.GetIthCollectionNameResult.Parser,
            cancellationToken);
    }

    public Task<GetIthPointFromGroupResult> GetIthPointFromGroupAsync(
        CollectionObjectName groupName,
        int pointIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetIthPointFromGroupRequest(),
            new Dictionary<string, object?>
            {
                ["group_name"] = groupName,
                ["point_index"] = pointIndex,
            });
        return InvokeOperationAsync<GetIthPointFromGroupResult>(
            "briosa.AnalysisOperations",
            "GetIthPointFromGroup",
            request,
            Transport.GetIthPointFromGroupResult.Parser,
            cancellationToken);
    }

    public Task<GetLinePropertiesResult> GetLinePropertiesAsync(
        CollectionObjectName lineName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetLinePropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["line_name"] = lineName,
            });
        return InvokeOperationAsync<GetLinePropertiesResult>(
            "briosa.AnalysisOperations",
            "GetLineProperties",
            request,
            Transport.GetLinePropertiesResult.Parser,
            cancellationToken);
    }

    public Task<GetMeasurementAuxiliaryDataResult> GetMeasurementAuxiliaryDataAsync(
        PointName pointName,
        string auxiliaryName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetMeasurementAuxiliaryDataRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
                ["auxiliary_name"] = auxiliaryName,
            });
        return InvokeOperationAsync<GetMeasurementAuxiliaryDataResult>(
            "briosa.AnalysisOperations",
            "GetMeasurementAuxiliaryData",
            request,
            Transport.GetMeasurementAuxiliaryDataResult.Parser,
            cancellationToken);
    }

    public Task<string> GetMeasurementInfoDataAsync(
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetMeasurementInfoDataRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
            });
        return InvokeOperationAsync<string>(
            "briosa.AnalysisOperations",
            "GetMeasurementInfoData",
            request,
            Transport.GetMeasurementInfoDataResult.Parser,
            cancellationToken);
    }

    public Task<GetMeasurementWeatherDataResult> GetMeasurementWeatherDataAsync(
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetMeasurementWeatherDataRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
            });
        return InvokeOperationAsync<GetMeasurementWeatherDataResult>(
            "briosa.AnalysisOperations",
            "GetMeasurementWeatherData",
            request,
            Transport.GetMeasurementWeatherDataResult.Parser,
            cancellationToken);
    }

    public Task<int> GetNumberOfCollectionsAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetNumberOfCollectionsRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync<int>(
            "briosa.AnalysisOperations",
            "GetNumberOfCollections",
            request,
            Transport.GetNumberOfCollectionsResult.Parser,
            cancellationToken);
    }

    public Task<int> GetNumberOfFramesInFrameSetAsync(
        CollectionObjectName frameSetContainer,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetNumberOfFramesInFrameSetRequest(),
            new Dictionary<string, object?>
            {
                ["frame_set_container"] = frameSetContainer,
            });
        return InvokeOperationAsync<int>(
            "briosa.AnalysisOperations",
            "GetNumberOfFramesInFrameSet",
            request,
            Transport.GetNumberOfFramesInFrameSetResult.Parser,
            cancellationToken);
    }

    public Task<int> GetNumberOfPointsInGroupAsync(
        CollectionObjectName groupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetNumberOfPointsInGroupRequest(),
            new Dictionary<string, object?>
            {
                ["group_name"] = groupName,
            });
        return InvokeOperationAsync<int>(
            "briosa.AnalysisOperations",
            "GetNumberOfPointsInGroup",
            request,
            Transport.GetNumberOfPointsInGroupResult.Parser,
            cancellationToken);
    }

    public Task<int> GetNumberOfPointsInPointSetAsync(
        CollectionObjectName pointSetContainer,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetNumberOfPointsInPointSetRequest(),
            new Dictionary<string, object?>
            {
                ["point_set_container"] = pointSetContainer,
            });
        return InvokeOperationAsync<int>(
            "briosa.AnalysisOperations",
            "GetNumberOfPointsInPointSet",
            request,
            Transport.GetNumberOfPointsInPointSetResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName> GetObjectReportingFrameAsync(
        CollectionObjectName objectName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetObjectReportingFrameRequest(),
            new Dictionary<string, object?>
            {
                ["object_name"] = objectName,
            });
        return InvokeOperationAsync<CollectionObjectName>(
            "briosa.AnalysisOperations",
            "GetObjectReportingFrame",
            request,
            Transport.GetObjectReportingFrameResult.Parser,
            cancellationToken);
    }

    public Task<GetPlanePropertiesResult> GetPlanePropertiesAsync(
        CollectionObjectName planeName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPlanePropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["plane_name"] = planeName,
            });
        return InvokeOperationAsync<GetPlanePropertiesResult>(
            "briosa.AnalysisOperations",
            "GetPlaneProperties",
            request,
            Transport.GetPlanePropertiesResult.Parser,
            cancellationToken);
    }

    public Task<GetPointCoordinateResult> GetPointCoordinateAsync(
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointCoordinateRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
            });
        return InvokeOperationAsync<GetPointCoordinateResult>(
            "briosa.AnalysisOperations",
            "GetPointCoordinate",
            request,
            Transport.GetPointCoordinateResult.Parser,
            cancellationToken);
    }

    public Task<GetPointCoordinateCylindricalResult> GetPointCoordinateCylindricalAsync(
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointCoordinateCylindricalRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
            });
        return InvokeOperationAsync<GetPointCoordinateCylindricalResult>(
            "briosa.AnalysisOperations",
            "GetPointCoordinateCylindrical",
            request,
            Transport.GetPointCoordinateCylindricalResult.Parser,
            cancellationToken);
    }

    public Task<GetPointCoordinatePolarResult> GetPointCoordinatePolarAsync(
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointCoordinatePolarRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
            });
        return InvokeOperationAsync<GetPointCoordinatePolarResult>(
            "briosa.AnalysisOperations",
            "GetPointCoordinatePolar",
            request,
            Transport.GetPointCoordinatePolarResult.Parser,
            cancellationToken);
    }

    public Task<GetPointPropertiesResult> GetPointPropertiesAsync(
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
            });
        return InvokeOperationAsync<GetPointPropertiesResult>(
            "briosa.AnalysisOperations",
            "GetPointProperties",
            request,
            Transport.GetPointPropertiesResult.Parser,
            cancellationToken);
    }

    public Task<GetPointToLineDistanceResult> GetPointToLineDistanceAsync(
        PointName point,
        CollectionObjectName line,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointToLineDistanceRequest(),
            new Dictionary<string, object?>
            {
                ["point"] = point,
                ["line"] = line,
            });
        return InvokeOperationAsync<GetPointToLineDistanceResult>(
            "briosa.AnalysisOperations",
            "GetPointToLineDistance",
            request,
            Transport.GetPointToLineDistanceResult.Parser,
            cancellationToken);
    }

    public Task<GetPointToPointDistanceResult> GetPointToPointDistanceAsync(
        PointName firstPoint,
        PointName secondPoint,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointToPointDistanceRequest(),
            new Dictionary<string, object?>
            {
                ["first_point"] = firstPoint,
                ["second_point"] = secondPoint,
            });
        return InvokeOperationAsync<GetPointToPointDistanceResult>(
            "briosa.AnalysisOperations",
            "GetPointToPointDistance",
            request,
            Transport.GetPointToPointDistanceResult.Parser,
            cancellationToken);
    }

    public Task<GetPointToleranceResult> GetPointToleranceAsync(
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointToleranceRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
            });
        return InvokeOperationAsync<GetPointToleranceResult>(
            "briosa.AnalysisOperations",
            "GetPointTolerance",
            request,
            Transport.GetPointToleranceResult.Parser,
            cancellationToken);
    }

    public Task<GetSlotPropertiesResult> GetSlotPropertiesAsync(
        CollectionObjectName slotName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetSlotPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["slot_name"] = slotName,
            });
        return InvokeOperationAsync<GetSlotPropertiesResult>(
            "briosa.AnalysisOperations",
            "GetSlotProperties",
            request,
            Transport.GetSlotPropertiesResult.Parser,
            cancellationToken);
    }

    public Task<GetSpherePropertiesResult> GetSpherePropertiesAsync(
        CollectionObjectName sphereName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetSpherePropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["sphere_name"] = sphereName,
            });
        return InvokeOperationAsync<GetSpherePropertiesResult>(
            "briosa.AnalysisOperations",
            "GetSphereProperties",
            request,
            Transport.GetSpherePropertiesResult.Parser,
            cancellationToken);
    }

    public Task<GetSurfacePhysicalStatsResult> GetSurfacePhysicalStatsAsync(
        CollectionObjectName surfaceName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetSurfacePhysicalStatsRequest(),
            new Dictionary<string, object?>
            {
                ["surface_name"] = surfaceName,
            });
        return InvokeOperationAsync<GetSurfacePhysicalStatsResult>(
            "briosa.AnalysisOperations",
            "GetSurfacePhysicalStats",
            request,
            Transport.GetSurfacePhysicalStatsResult.Parser,
            cancellationToken);
    }

    public Task<double> GetTimestampForIthFrameInFrameSetAsync(
        CollectionObjectName frameSet,
        int frameSetIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetTimestampForIthFrameInFrameSetRequest(),
            new Dictionary<string, object?>
            {
                ["frame_set"] = frameSet,
                ["frame_set_index"] = frameSetIndex,
            });
        return InvokeOperationAsync<double>(
            "briosa.AnalysisOperations",
            "GetTimestampForIthFrameInFrameSet",
            request,
            Transport.GetTimestampForIthFrameInFrameSetResult.Parser,
            cancellationToken);
    }

    public Task<double> GetTimestampForIthPointInPointSetAsync(
        CollectionObjectName pointSet,
        int pointSetIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetTimestampForIthPointInPointSetRequest(),
            new Dictionary<string, object?>
            {
                ["point_set"] = pointSet,
                ["point_set_index"] = pointSetIndex,
            });
        return InvokeOperationAsync<double>(
            "briosa.AnalysisOperations",
            "GetTimestampForIthPointInPointSet",
            request,
            Transport.GetTimestampForIthPointInPointSetResult.Parser,
            cancellationToken);
    }

    public Task<GetTorusPropertiesResult> GetTorusPropertiesAsync(
        CollectionObjectName torusName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetTorusPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["torus_name"] = torusName,
            });
        return InvokeOperationAsync<GetTorusPropertiesResult>(
            "briosa.AnalysisOperations",
            "GetTorusProperties",
            request,
            Transport.GetTorusPropertiesResult.Parser,
            cancellationToken);
    }

    public Task<Transform> GetTransformForIthFrameInFrameSetAsync(
        CollectionObjectName frameSet,
        int frameSetIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetTransformForIthFrameInFrameSetRequest(),
            new Dictionary<string, object?>
            {
                ["frame_set"] = frameSet,
                ["frame_set_index"] = frameSetIndex,
            });
        return InvokeOperationAsync<Transform>(
            "briosa.AnalysisOperations",
            "GetTransformForIthFrameInFrameSet",
            request,
            Transport.GetTransformForIthFrameInFrameSetResult.Parser,
            cancellationToken);
    }

    public Task<GroupToSurfaceFitResult> GroupToSurfaceFitAsync(
        CollectionObjectName groupToFit,
        CollectionObjectName surface,
        bool doConventionalFit = false,
        double rmsTolerance00ForNone = 0.000000,
        double maximumAbsoluteTolerance00ForNone = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GroupToSurfaceFitRequest(),
            new Dictionary<string, object?>
            {
                ["group_to_fit"] = groupToFit,
                ["surface"] = surface,
                ["do_conventional_fit"] = doConventionalFit,
                ["rms_tolerance_0_0_for_none"] = rmsTolerance00ForNone,
                ["maximum_absolute_tolerance_0_0_for_none"] = maximumAbsoluteTolerance00ForNone,
            });
        return InvokeOperationAsync<GroupToSurfaceFitResult>(
            "briosa.AnalysisOperations",
            "GroupToSurfaceFit",
            request,
            Transport.GroupToSurfaceFitResult.Parser,
            cancellationToken);
    }

    public Task ImportGeometryFitProfilesAsync(
        FileReference geometryFitProfilesFilePath,
        bool overwriteProfilesWithSameName = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportGeometryFitProfilesRequest(),
            new Dictionary<string, object?>
            {
                ["geometry_fit_profiles_file_path"] = geometryFitProfilesFilePath,
                ["overwrite_profiles_with_same_name"] = overwriteProfilesWithSameName,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "ImportGeometryFitProfiles",
            request,
            Transport.ImportGeometryFitProfilesResult.Parser,
            cancellationToken);
    }

    public Task<bool> IsObjectOfTypeAsync(
        CollectionObjectName objectName,
        ObjectType objectType,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.IsObjectOfTypeRequest(),
            new Dictionary<string, object?>
            {
                ["object_name"] = objectName,
                ["object_type"] = objectType,
            });
        return InvokeOperationAsync<bool>(
            "briosa.AnalysisOperations",
            "IsObjectOfType",
            request,
            Transport.IsObjectOfTypeResult.Parser,
            cancellationToken);
    }

    public Task MakeCircleFitProfileAsync(
        string fitProfileName,
        MeasuredSideForRadialOffset measuredSideForRadialOffset,
        double overrideRadialOffset10UseCurrent,
        MeasuredSideForPlanarOffset measuredSideForPlanarOffset,
        double overridePlanarOffset10UseCurrent,
        NormalDirection planarOffsetDirection,
        double lockRadius10DoNotLock,
        CompTechnique circleComputationTechnique,
        bool reverseNormalVectorAfterFit = false,
        bool makeCardinalPoints = true,
        bool cardinalPt1Center = true,
        bool cardinalPt2PointOnNormal = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCircleFitProfileRequest(),
            new Dictionary<string, object?>
            {
                ["fit_profile_name"] = fitProfileName,
                ["measured_side_for_radial_offset"] = measuredSideForRadialOffset,
                ["override_radial_offset_1_0_use_current"] = overrideRadialOffset10UseCurrent,
                ["measured_side_for_planar_offset"] = measuredSideForPlanarOffset,
                ["override_planar_offset_1_0_use_current"] = overridePlanarOffset10UseCurrent,
                ["planar_offset_direction"] = planarOffsetDirection,
                ["lock_radius_1_0_do_not_lock"] = lockRadius10DoNotLock,
                ["circle_computation_technique"] = circleComputationTechnique,
                ["reverse_normal_vector_after_fit"] = reverseNormalVectorAfterFit,
                ["make_cardinal_points"] = makeCardinalPoints,
                ["cardinal_pt_1_center"] = cardinalPt1Center,
                ["cardinal_pt_2_point_on_normal"] = cardinalPt2PointOnNormal,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "MakeCircleFitProfile",
            request,
            Transport.MakeCircleFitProfileResult.Parser,
            cancellationToken);
    }

    public Task MakeConeFitProfileAsync(
        string fitProfileName,
        MeasuredSideForRadialOffset measuredSideForRadialOffset,
        double overrideRadialOffset10UseCurrent = -1.000000,
        double lockAngleInDegrees10DoNotLock = -1.000000,
        bool useExhaustiveSearch = true,
        bool makeCardinalPoints = true,
        bool cardinalPt1Vertex = true,
        bool cardinalPt2PointOnAxis = true,
        bool cardinalPt3CutPointOnAxis = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeConeFitProfileRequest(),
            new Dictionary<string, object?>
            {
                ["fit_profile_name"] = fitProfileName,
                ["measured_side_for_radial_offset"] = measuredSideForRadialOffset,
                ["override_radial_offset_1_0_use_current"] = overrideRadialOffset10UseCurrent,
                ["lock_angle_in_degrees_1_0_do_not_lock"] = lockAngleInDegrees10DoNotLock,
                ["use_exhaustive_search"] = useExhaustiveSearch,
                ["make_cardinal_points"] = makeCardinalPoints,
                ["cardinal_pt_1_vertex"] = cardinalPt1Vertex,
                ["cardinal_pt_2_point_on_axis"] = cardinalPt2PointOnAxis,
                ["cardinal_pt_3_cut_point_on_axis"] = cardinalPt3CutPointOnAxis,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "MakeConeFitProfile",
            request,
            Transport.MakeConeFitProfileResult.Parser,
            cancellationToken);
    }

    public Task MakeCylinderFitProfileAsync(
        string fitProfileName,
        MeasuredSideForRadialOffset measuredSideForRadialOffset,
        double overrideRadialOffset10UseCurrent,
        double lockRadius10DoNotLock,
        FitMethod lockedRadiusFitMethod,
        bool constrainToNominalAxis,
        bool constrainToNominalOrientation,
        bool alignWithNominal,
        bool reverseAxis,
        bool setAxisFirstToLastPoint,
        CompTechnique cylinderComputationTechnique,
        bool useExhaustiveSearch = false,
        bool makeCardinalPoints = true,
        bool cardinalPt1BeginPt = true,
        bool cardinalPt2EndPt = true,
        bool cardinalPt3Center = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCylinderFitProfileRequest(),
            new Dictionary<string, object?>
            {
                ["fit_profile_name"] = fitProfileName,
                ["measured_side_for_radial_offset"] = measuredSideForRadialOffset,
                ["override_radial_offset_1_0_use_current"] = overrideRadialOffset10UseCurrent,
                ["lock_radius_1_0_do_not_lock"] = lockRadius10DoNotLock,
                ["locked_radius_fit_method"] = lockedRadiusFitMethod,
                ["constrain_to_nominal_axis"] = constrainToNominalAxis,
                ["constrain_to_nominal_orientation"] = constrainToNominalOrientation,
                ["align_with_nominal"] = alignWithNominal,
                ["reverse_axis"] = reverseAxis,
                ["set_axis_first_to_last_point"] = setAxisFirstToLastPoint,
                ["cylinder_computation_technique"] = cylinderComputationTechnique,
                ["use_exhaustive_search"] = useExhaustiveSearch,
                ["make_cardinal_points"] = makeCardinalPoints,
                ["cardinal_pt_1_begin_pt"] = cardinalPt1BeginPt,
                ["cardinal_pt_2_end_pt"] = cardinalPt2EndPt,
                ["cardinal_pt_3_center"] = cardinalPt3Center,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "MakeCylinderFitProfile",
            request,
            Transport.MakeCylinderFitProfileResult.Parser,
            cancellationToken);
    }

    public Task MakeEllipseFitProfileAsync(
        string fitProfileName,
        MeasuredSideForRadialOffset measuredSideForRadialOffset,
        double overrideRadialOffset10UseCurrent,
        MeasuredSideForPlanarOffset measuredSideForPlanarOffset,
        double overridePlanarOffset10UseCurrent,
        NormalDirection planarOffsetDirection,
        bool reverseNormalVectorAfterFit = false,
        bool makeCardinalPoints = true,
        bool cardinalPt1Center = true,
        bool cardinalPt2PointOnNormal = true,
        bool cardinalPt3FocalPt1 = true,
        bool cardinalPt4FocalPt2 = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeEllipseFitProfileRequest(),
            new Dictionary<string, object?>
            {
                ["fit_profile_name"] = fitProfileName,
                ["measured_side_for_radial_offset"] = measuredSideForRadialOffset,
                ["override_radial_offset_1_0_use_current"] = overrideRadialOffset10UseCurrent,
                ["measured_side_for_planar_offset"] = measuredSideForPlanarOffset,
                ["override_planar_offset_1_0_use_current"] = overridePlanarOffset10UseCurrent,
                ["planar_offset_direction"] = planarOffsetDirection,
                ["reverse_normal_vector_after_fit"] = reverseNormalVectorAfterFit,
                ["make_cardinal_points"] = makeCardinalPoints,
                ["cardinal_pt_1_center"] = cardinalPt1Center,
                ["cardinal_pt_2_point_on_normal"] = cardinalPt2PointOnNormal,
                ["cardinal_pt_3_focal_pt_1"] = cardinalPt3FocalPt1,
                ["cardinal_pt_4_focal_pt_2"] = cardinalPt4FocalPt2,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "MakeEllipseFitProfile",
            request,
            Transport.MakeEllipseFitProfileResult.Parser,
            cancellationToken);
    }

    public Task MakeLineFitProfileAsync(
        string fitProfileName = "",
        bool reverseNormalVectorAfterFit = false,
        bool makeCardinalPoints = true,
        bool cardinalPt1PointA = true,
        bool cardinalPt2PointB = true,
        bool cardinalPt3MidPoint = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeLineFitProfileRequest(),
            new Dictionary<string, object?>
            {
                ["fit_profile_name"] = fitProfileName,
                ["reverse_normal_vector_after_fit"] = reverseNormalVectorAfterFit,
                ["make_cardinal_points"] = makeCardinalPoints,
                ["cardinal_pt_1_point_a"] = cardinalPt1PointA,
                ["cardinal_pt_2_point_b"] = cardinalPt2PointB,
                ["cardinal_pt_3_mid_point"] = cardinalPt3MidPoint,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "MakeLineFitProfile",
            request,
            Transport.MakeLineFitProfileResult.Parser,
            cancellationToken);
    }

    public Task MakeParaboloidFitProfileAsync(
        string fitProfileName,
        MeasuredSideForRadialOffset measuredSideForRadialOffset,
        double overrideRadialOffset10UseCurrent,
        double lockFocalLength10DoNotLock,
        DegreeOfFreedom degreeOfFreedom,
        bool makeCardinalPoints = true,
        bool cardinalPt1Vertex = true,
        bool cardinalPt2FocalPoint = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeParaboloidFitProfileRequest(),
            new Dictionary<string, object?>
            {
                ["fit_profile_name"] = fitProfileName,
                ["measured_side_for_radial_offset"] = measuredSideForRadialOffset,
                ["override_radial_offset_1_0_use_current"] = overrideRadialOffset10UseCurrent,
                ["lock_focal_length_1_0_do_not_lock"] = lockFocalLength10DoNotLock,
                ["degree_of_freedom"] = degreeOfFreedom,
                ["make_cardinal_points"] = makeCardinalPoints,
                ["cardinal_pt_1_vertex"] = cardinalPt1Vertex,
                ["cardinal_pt_2_focal_point"] = cardinalPt2FocalPoint,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "MakeParaboloidFitProfile",
            request,
            Transport.MakeParaboloidFitProfileResult.Parser,
            cancellationToken);
    }

    public Task MakePlaneFitProfileAsync(
        string fitProfileName,
        MeasuredSideForPlanarOffset measuredSideForPlanarOffset,
        double overridePlanarOffset10UseCurrent,
        NormalDirection planarOffsetDirection,
        bool reverseNormalVectorAfterFit = false,
        bool makeCardinalPoints = true,
        bool cardinalPt1Centroid = true,
        bool cardinalPt2PointOnNormal = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePlaneFitProfileRequest(),
            new Dictionary<string, object?>
            {
                ["fit_profile_name"] = fitProfileName,
                ["measured_side_for_planar_offset"] = measuredSideForPlanarOffset,
                ["override_planar_offset_1_0_use_current"] = overridePlanarOffset10UseCurrent,
                ["planar_offset_direction"] = planarOffsetDirection,
                ["reverse_normal_vector_after_fit"] = reverseNormalVectorAfterFit,
                ["make_cardinal_points"] = makeCardinalPoints,
                ["cardinal_pt_1_centroid"] = cardinalPt1Centroid,
                ["cardinal_pt_2_point_on_normal"] = cardinalPt2PointOnNormal,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "MakePlaneFitProfile",
            request,
            Transport.MakePlaneFitProfileResult.Parser,
            cancellationToken);
    }

    public Task MakeSlotFitProfileAsync(
        string fitProfileName,
        MeasuredSideForRadialOffset measuredSideForRadialOffset,
        double overrideRadialOffset10UseCurrent,
        MeasuredSideForPlanarOffset measuredSideForPlanarOffset,
        double overridePlanarOffset10UseCurrent,
        NormalDirection planarOffsetDirection,
        SlotType slotType,
        CompTechnique slotComputationTechnique,
        bool reverseNormalVectorAfterFit = false,
        bool makeCardinalPoints = true,
        bool cardinalPt1Center = true,
        bool cardinalPt2PointOnNormal = true,
        bool cardinalPt3CenterlinePt1 = true,
        bool cardinalPt4CenterlinePt2 = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeSlotFitProfileRequest(),
            new Dictionary<string, object?>
            {
                ["fit_profile_name"] = fitProfileName,
                ["measured_side_for_radial_offset"] = measuredSideForRadialOffset,
                ["override_radial_offset_1_0_use_current"] = overrideRadialOffset10UseCurrent,
                ["measured_side_for_planar_offset"] = measuredSideForPlanarOffset,
                ["override_planar_offset_1_0_use_current"] = overridePlanarOffset10UseCurrent,
                ["planar_offset_direction"] = planarOffsetDirection,
                ["slot_type"] = slotType,
                ["slot_computation_technique"] = slotComputationTechnique,
                ["reverse_normal_vector_after_fit"] = reverseNormalVectorAfterFit,
                ["make_cardinal_points"] = makeCardinalPoints,
                ["cardinal_pt_1_center"] = cardinalPt1Center,
                ["cardinal_pt_2_point_on_normal"] = cardinalPt2PointOnNormal,
                ["cardinal_pt_3_centerline_pt_1"] = cardinalPt3CenterlinePt1,
                ["cardinal_pt_4_centerline_pt_2"] = cardinalPt4CenterlinePt2,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "MakeSlotFitProfile",
            request,
            Transport.MakeSlotFitProfileResult.Parser,
            cancellationToken);
    }

    public Task MakeSphereFitProfileAsync(
        string fitProfileName,
        MeasuredSideForRadialOffset measuredSideForRadialOffset,
        double overrideRadialOffset10UseCurrent,
        double lockRadius10DoNotLock,
        bool makeCardinalPoints,
        bool cardinalPt1Center,
        SphereFitComputationMode computationMethod,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeSphereFitProfileRequest(),
            new Dictionary<string, object?>
            {
                ["fit_profile_name"] = fitProfileName,
                ["measured_side_for_radial_offset"] = measuredSideForRadialOffset,
                ["override_radial_offset_1_0_use_current"] = overrideRadialOffset10UseCurrent,
                ["lock_radius_1_0_do_not_lock"] = lockRadius10DoNotLock,
                ["make_cardinal_points"] = makeCardinalPoints,
                ["cardinal_pt_1_center"] = cardinalPt1Center,
                ["computation_method"] = computationMethod,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "MakeSphereFitProfile",
            request,
            Transport.MakeSphereFitProfileResult.Parser,
            cancellationToken);
    }

    public Task<MushroomTargetHoleInspectionResult> MushroomTargetHoleInspectionAsync(
        string namePrefixForIntermediateConstructions,
        CollectionObjectName spherePointsGroupName,
        double sphereTargetRadius,
        CollectionObjectName targetContactPlane,
        PointName pointToCreateAtHole,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MushroomTargetHoleInspectionRequest(),
            new Dictionary<string, object?>
            {
                ["name_prefix_for_intermediate_constructions"] = namePrefixForIntermediateConstructions,
                ["sphere_points_group_name"] = spherePointsGroupName,
                ["sphere_target_radius"] = sphereTargetRadius,
                ["target_contact_plane"] = targetContactPlane,
                ["point_to_create_at_hole"] = pointToCreateAtHole,
            });
        return InvokeOperationAsync<MushroomTargetHoleInspectionResult>(
            "briosa.AnalysisOperations",
            "MushroomTargetHoleInspection",
            request,
            Transport.MushroomTargetHoleInspectionResult.Parser,
            cancellationToken);
    }

    public Task PatchNormalShiftHolePinAsync(
        CollectionObjectName planePointsGroupName,
        CollectionObjectName perimeterPointsGroupName,
        PointName resultingPointName,
        double additionalMaterialThickness = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.PatchNormalShiftHolePinRequest(),
            new Dictionary<string, object?>
            {
                ["plane_points_group_name"] = planePointsGroupName,
                ["perimeter_points_group_name"] = perimeterPointsGroupName,
                ["resulting_point_name"] = resultingPointName,
                ["additional_material_thickness"] = additionalMaterialThickness,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "PatchNormalShiftHolePin",
            request,
            Transport.PatchNormalShiftHolePinResult.Parser,
            cancellationToken);
    }

    public Task PatchNormalShiftPointAsync(
        CollectionObjectName planePointsGroupName,
        PointName pointToShift,
        PointName resultingPointName,
        double additionalMaterialThickness = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.PatchNormalShiftPointRequest(),
            new Dictionary<string, object?>
            {
                ["plane_points_group_name"] = planePointsGroupName,
                ["point_to_shift"] = pointToShift,
                ["resulting_point_name"] = resultingPointName,
                ["additional_material_thickness"] = additionalMaterialThickness,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "PatchNormalShiftPoint",
            request,
            Transport.PatchNormalShiftPointResult.Parser,
            cancellationToken);
    }

    public Task<QueryCloudsToObjectsResult> QueryCloudsToObjectsAsync(
        IEnumerable<CollectionObjectName> cloudNames,
        IEnumerable<CollectionObjectName> objectNames,
        CollectionObjectName resultingObjectName,
        ProjectionOptions projectionOptions,
        double proximity = 0.000000,
        int skipFactor = 0,
        double rmsTolerance00ForNone = 0.000000,
        double maximumAbsoluteTolerance00ForNone = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.QueryCloudsToObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_names"] = cloudNames,
                ["object_names"] = objectNames,
                ["resulting_object_name"] = resultingObjectName,
                ["projection_options"] = projectionOptions,
                ["proximity"] = proximity,
                ["skip_factor"] = skipFactor,
                ["rms_tolerance_0_0_for_none"] = rmsTolerance00ForNone,
                ["maximum_absolute_tolerance_0_0_for_none"] = maximumAbsoluteTolerance00ForNone,
            });
        return InvokeOperationAsync<QueryCloudsToObjectsResult>(
            "briosa.AnalysisOperations",
            "QueryCloudsToObjects",
            request,
            Transport.QueryCloudsToObjectsResult.Parser,
            cancellationToken);
    }

    public Task<QueryCloudsToSurfaceResult> QueryCloudsToSurfaceAsync(
        IEnumerable<CollectionObjectName> cloudNames,
        CollectionObjectName filterSurfaceName,
        CollectionObjectName resultingObjectName,
        ProjectionOptions projectionOptions,
        double proximity = 0.000000,
        int skipFactor = 0,
        double rmsTolerance00ForNone = 0.000000,
        double maximumAbsoluteTolerance00ForNone = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.QueryCloudsToSurfaceRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_names"] = cloudNames,
                ["filter_surface_name"] = filterSurfaceName,
                ["resulting_object_name"] = resultingObjectName,
                ["projection_options"] = projectionOptions,
                ["proximity"] = proximity,
                ["skip_factor"] = skipFactor,
                ["rms_tolerance_0_0_for_none"] = rmsTolerance00ForNone,
                ["maximum_absolute_tolerance_0_0_for_none"] = maximumAbsoluteTolerance00ForNone,
            });
        return InvokeOperationAsync<QueryCloudsToSurfaceResult>(
            "briosa.AnalysisOperations",
            "QueryCloudsToSurface",
            request,
            Transport.QueryCloudsToSurfaceResult.Parser,
            cancellationToken);
    }

    public Task<QueryFrameToFrameResult> QueryFrameToFrameAsync(
        CollectionObjectName referenceFrameName,
        CollectionObjectName correspondingFrameName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.QueryFrameToFrameRequest(),
            new Dictionary<string, object?>
            {
                ["reference_frame_name"] = referenceFrameName,
                ["corresponding_frame_name"] = correspondingFrameName,
            });
        return InvokeOperationAsync<QueryFrameToFrameResult>(
            "briosa.AnalysisOperations",
            "QueryFrameToFrame",
            request,
            Transport.QueryFrameToFrameResult.Parser,
            cancellationToken);
    }

    public Task<QueryGroupsToObjectsResult> QueryGroupsToObjectsAsync(
        IEnumerable<CollectionObjectName> groupNameListGroupsToProject,
        IEnumerable<CollectionObjectName> objectNameListObjectsToProjectTo,
        CollectionObjectName resultingObjectName,
        ProjectionOptions projectionOptions,
        double rmsTolerance00ForNone = 0.000000,
        double maximumAbsoluteTolerance00ForNone = 0.000000,
        bool showResultsDialog = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.QueryGroupsToObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["group_name_list_groups_to_project"] = groupNameListGroupsToProject,
                ["object_name_list_objects_to_project_to"] = objectNameListObjectsToProjectTo,
                ["resulting_object_name"] = resultingObjectName,
                ["projection_options"] = projectionOptions,
                ["rms_tolerance_0_0_for_none"] = rmsTolerance00ForNone,
                ["maximum_absolute_tolerance_0_0_for_none"] = maximumAbsoluteTolerance00ForNone,
                ["show_results_dialog"] = showResultsDialog,
            });
        return InvokeOperationAsync<QueryGroupsToObjectsResult>(
            "briosa.AnalysisOperations",
            "QueryGroupsToObjects",
            request,
            Transport.QueryGroupsToObjectsResult.Parser,
            cancellationToken);
    }

    public Task<QueryPointToObjectsResult> QueryPointToObjectsAsync(
        PointName pointName,
        IEnumerable<CollectionObjectName> objects,
        bool ignoreTargetOffset = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.QueryPointToObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
                ["objects"] = objects,
                ["ignore_target_offset"] = ignoreTargetOffset,
            });
        return InvokeOperationAsync<QueryPointToObjectsResult>(
            "briosa.AnalysisOperations",
            "QueryPointToObjects",
            request,
            Transport.QueryPointToObjectsResult.Parser,
            cancellationToken);
    }

    public Task<double> QueryPointToPointAlongCurveAsync(
        PointName value1stPoint,
        PointName value2ndPoint,
        CollectionObjectName curve,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.QueryPointToPointAlongCurveRequest(),
            new Dictionary<string, object?>
            {
                ["value_1st_point"] = value1stPoint,
                ["value_2nd_point"] = value2ndPoint,
                ["curve"] = curve,
            });
        return InvokeOperationAsync<double>(
            "briosa.AnalysisOperations",
            "QueryPointToPointAlongCurve",
            request,
            Transport.QueryPointToPointAlongCurveResult.Parser,
            cancellationToken);
    }

    public Task QueryPointsToCircleAsync(
        CollectionObjectName circleName,
        CollectionObjectName pointGroupName,
        bool isInsideMeasurement,
        int autoScaleVectorsToOfRadius,
        CollectionObjectName vectorGroupNameForRadial,
        CollectionObjectName vectorGroupNameForPlanar,
        CollectionObjectName vectorGroupNameForCombined,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.QueryPointsToCircleRequest(),
            new Dictionary<string, object?>
            {
                ["circle_name"] = circleName,
                ["point_group_name"] = pointGroupName,
                ["is_inside_measurement"] = isInsideMeasurement,
                ["auto_scale_vectors_to_of_radius"] = autoScaleVectorsToOfRadius,
                ["vector_group_name_for_radial"] = vectorGroupNameForRadial,
                ["vector_group_name_for_planar"] = vectorGroupNameForPlanar,
                ["vector_group_name_for_combined"] = vectorGroupNameForCombined,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "QueryPointsToCircle",
            request,
            Transport.QueryPointsToCircleResult.Parser,
            cancellationToken);
    }

    public Task<QueryPointsToObjectsResult> QueryPointsToObjectsAsync(
        IEnumerable<PointName> pointNames,
        IEnumerable<CollectionObjectName> objectNameListObjectsToProjectTo,
        CollectionObjectName resultingObjectName,
        ProjectionOptions projectionOptions,
        double rmsTolerance00ForNone = 0.000000,
        double maximumAbsoluteTolerance00ForNone = 0.000000,
        bool showResultsDialog = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.QueryPointsToObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["point_names"] = pointNames,
                ["object_name_list_objects_to_project_to"] = objectNameListObjectsToProjectTo,
                ["resulting_object_name"] = resultingObjectName,
                ["projection_options"] = projectionOptions,
                ["rms_tolerance_0_0_for_none"] = rmsTolerance00ForNone,
                ["maximum_absolute_tolerance_0_0_for_none"] = maximumAbsoluteTolerance00ForNone,
                ["show_results_dialog"] = showResultsDialog,
            });
        return InvokeOperationAsync<QueryPointsToObjectsResult>(
            "briosa.AnalysisOperations",
            "QueryPointsToObjects",
            request,
            Transport.QueryPointsToObjectsResult.Parser,
            cancellationToken);
    }

    public Task QueryPointsToSinglePointAsync(
        IEnumerable<PointName> pointNames,
        PointName singlePoint,
        bool showVectorProperties = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.QueryPointsToSinglePointRequest(),
            new Dictionary<string, object?>
            {
                ["point_names"] = pointNames,
                ["single_point"] = singlePoint,
                ["show_vector_properties"] = showVectorProperties,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "QueryPointsToSinglePoint",
            request,
            Transport.QueryPointsToSinglePointResult.Parser,
            cancellationToken);
    }

    public Task ReComputeCalculatedItemsAsync(
        bool targetsFromShots = false,
        bool hiddenPoints = false,
        bool relationships = false,
        bool refreshFilteredCloudData = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ReComputeCalculatedItemsRequest(),
            new Dictionary<string, object?>
            {
                ["targets_from_shots"] = targetsFromShots,
                ["hidden_points"] = hiddenPoints,
                ["relationships"] = relationships,
                ["refresh_filtered_cloud_data"] = refreshFilteredCloudData,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "ReComputeCalculatedItems",
            request,
            Transport.ReComputeCalculatedItemsResult.Parser,
            cancellationToken);
    }

    public Task RenamePointsBasedOnInterPointDistanceToReferencePointsAsync(
        CollectionObjectName referenceGroupName,
        CollectionObjectName groupToRenamePoints,
        double distanceThreshold = 0.000000,
        bool verifyResults = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RenamePointsBasedOnInterPointDistanceToReferencePointsRequest(),
            new Dictionary<string, object?>
            {
                ["reference_group_name"] = referenceGroupName,
                ["group_to_rename_points"] = groupToRenamePoints,
                ["distance_threshold"] = distanceThreshold,
                ["verify_results"] = verifyResults,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "RenamePointsBasedOnInterPointDistanceToReferencePoints",
            request,
            Transport.RenamePointsBasedOnInterPointDistanceToReferencePointsResult.Parser,
            cancellationToken);
    }

    public Task RenamePointsBasedOnProximityToReferencePointsAsync(
        CollectionObjectName referenceGroupName,
        CollectionObjectName groupToRenamePoints,
        double proximityThreshold = 0.000000,
        bool verifyResults = false,
        bool renameAllProximatePoints = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RenamePointsBasedOnProximityToReferencePointsRequest(),
            new Dictionary<string, object?>
            {
                ["reference_group_name"] = referenceGroupName,
                ["group_to_rename_points"] = groupToRenamePoints,
                ["proximity_threshold"] = proximityThreshold,
                ["verify_results"] = verifyResults,
                ["rename_all_proximate_points"] = renameAllProximatePoints,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "RenamePointsBasedOnProximityToReferencePoints",
            request,
            Transport.RenamePointsBasedOnProximityToReferencePointsResult.Parser,
            cancellationToken);
    }

    public Task ReverseBSplinesAsync(
        IEnumerable<CollectionObjectName> bSplineList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ReverseBSplinesRequest(),
            new Dictionary<string, object?>
            {
                ["b_spline_list"] = bSplineList,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "ReverseBSplines",
            request,
            Transport.ReverseBSplinesResult.Parser,
            cancellationToken);
    }

    public Task ReversePlaneNormalsAsync(
        IEnumerable<CollectionObjectName> planeList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ReversePlaneNormalsRequest(),
            new Dictionary<string, object?>
            {
                ["plane_list"] = planeList,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "ReversePlaneNormals",
            request,
            Transport.ReversePlaneNormalsResult.Parser,
            cancellationToken);
    }

    public Task ReverseSurfaceNormalsAsync(
        IEnumerable<CollectionObjectName> surfaceList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ReverseSurfaceNormalsRequest(),
            new Dictionary<string, object?>
            {
                ["surface_list"] = surfaceList,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "ReverseSurfaceNormals",
            request,
            Transport.ReverseSurfaceNormalsResult.Parser,
            cancellationToken);
    }

    public Task SetCirclePropertiesAsync(
        CollectionObjectName circleName,
        Vector centerCoordinate,
        Vector normalDirection,
        double radius = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCirclePropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["circle_name"] = circleName,
                ["center_coordinate"] = centerCoordinate,
                ["normal_direction"] = normalDirection,
                ["radius"] = radius,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "SetCircleProperties",
            request,
            Transport.SetCirclePropertiesResult.Parser,
            cancellationToken);
    }

    public Task SetConePropertiesAsync(
        CollectionObjectName coneName,
        Vector coneEndPointInWorkingCoordinates,
        Vector coneAxisInWorkingCoordinates,
        double coneLength = 0.000000,
        double coneThetaStart = 0.000000,
        double coneThetaSpan = 0.000000,
        double coneIncludedAngle = 0.000000,
        double cutLengthFromApex = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetConePropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["cone_name"] = coneName,
                ["cone_end_point_in_working_coordinates"] = coneEndPointInWorkingCoordinates,
                ["cone_axis_in_working_coordinates"] = coneAxisInWorkingCoordinates,
                ["cone_length"] = coneLength,
                ["cone_theta_start"] = coneThetaStart,
                ["cone_theta_span"] = coneThetaSpan,
                ["cone_included_angle"] = coneIncludedAngle,
                ["cut_length_from_apex"] = cutLengthFromApex,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "SetConeProperties",
            request,
            Transport.SetConePropertiesResult.Parser,
            cancellationToken);
    }

    public Task SetCylinderPropertiesAsync(
        CollectionObjectName cylinderName,
        Vector beginCoordinate,
        Vector axisDirection,
        double length = 0.000000,
        double diameter = 0.000000,
        bool nominalsPointInward = true,
        int facets = 32,
        bool enableThetaExtentDisplayMode = true,
        double thetaStartInDegrees = 0.000000,
        double thetaSpanInDegrees = 360.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCylinderPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["cylinder_name"] = cylinderName,
                ["begin_coordinate"] = beginCoordinate,
                ["axis_direction"] = axisDirection,
                ["length"] = length,
                ["diameter"] = diameter,
                ["nominals_point_inward"] = nominalsPointInward,
                ["facets"] = facets,
                ["enable_theta_extent_display_mode"] = enableThetaExtentDisplayMode,
                ["theta_start_in_degrees"] = thetaStartInDegrees,
                ["theta_span_in_degrees"] = thetaSpanInDegrees,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "SetCylinderProperties",
            request,
            Transport.SetCylinderPropertiesResult.Parser,
            cancellationToken);
    }

    public Task SetDefaultColorizationOptionsAsync(
        ColorizationOptions colorizationOptions,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetDefaultColorizationOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["colorization_options"] = colorizationOptions,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "SetDefaultColorizationOptions",
            request,
            Transport.SetDefaultColorizationOptionsResult.Parser,
            cancellationToken);
    }

    public Task SetEllipsePropertiesAsync(
        CollectionObjectName ellipseName,
        Vector centerCoordinate,
        Vector normalDirection,
        double majorAxisRadius = 0.000000,
        double minorAxisRadius = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetEllipsePropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["ellipse_name"] = ellipseName,
                ["center_coordinate"] = centerCoordinate,
                ["normal_direction"] = normalDirection,
                ["major_axis_radius"] = majorAxisRadius,
                ["minor_axis_radius"] = minorAxisRadius,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "SetEllipseProperties",
            request,
            Transport.SetEllipsePropertiesResult.Parser,
            cancellationToken);
    }

    public Task SetGeometryRelationshipFitProfileAsync(
        GeometryType geometryType,
        IEnumerable<CollectionItemName> relationshipRefList,
        string fitProfileName = "",
        bool applyCardinalPointSettings = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGeometryRelationshipFitProfileRequest(),
            new Dictionary<string, object?>
            {
                ["geometry_type"] = geometryType,
                ["relationship_ref_list"] = relationshipRefList,
                ["fit_profile_name"] = fitProfileName,
                ["apply_cardinal_point_settings"] = applyCardinalPointSettings,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "SetGeometryRelationshipFitProfile",
            request,
            Transport.SetGeometryRelationshipFitProfileResult.Parser,
            cancellationToken);
    }

    public Task SetLinePropertiesAsync(
        CollectionObjectName lineName,
        Vector beginCoordinate,
        Vector endCoordinate,
        double lengthOptional = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetLinePropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["line_name"] = lineName,
                ["begin_coordinate"] = beginCoordinate,
                ["end_coordinate"] = endCoordinate,
                ["length_optional"] = lengthOptional,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "SetLineProperties",
            request,
            Transport.SetLinePropertiesResult.Parser,
            cancellationToken);
    }

    public Task SetMeasurementAuxiliaryDataAsync(
        PointName pointName,
        string auxiliaryName = "",
        double value = 0.000000,
        string units = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetMeasurementAuxiliaryDataRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
                ["auxiliary_name"] = auxiliaryName,
                ["value"] = value,
                ["units"] = units,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "SetMeasurementAuxiliaryData",
            request,
            Transport.SetMeasurementAuxiliaryDataResult.Parser,
            cancellationToken);
    }

    public Task SetObjectReportingFrameAsync(
        CollectionObjectName objectName,
        CollectionObjectName reportingFrame,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetObjectReportingFrameRequest(),
            new Dictionary<string, object?>
            {
                ["object_name"] = objectName,
                ["reporting_frame"] = reportingFrame,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "SetObjectReportingFrame",
            request,
            Transport.SetObjectReportingFrameResult.Parser,
            cancellationToken);
    }

    public Task SetPointPropertiesAsync(
        IEnumerable<PointName> pointNameList,
        double planarOffset,
        double radialOffset,
        ToleranceVectorOptions positionTolerance,
        Vector componentWeights,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPointPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["point_name_list"] = pointNameList,
                ["planar_offset"] = planarOffset,
                ["radial_offset"] = radialOffset,
                ["position_tolerance"] = positionTolerance,
                ["component_weights"] = componentWeights,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "SetPointProperties",
            request,
            Transport.SetPointPropertiesResult.Parser,
            cancellationToken);
    }

    public Task<PointName[]> SetPointWeightsFromUncertaintiesAsync(
        IEnumerable<PointName> pointNameList,
        string uncertaintyReferenceFrameMode,
        CollectionObjectName reportingFrame,
        string weightNormalizationMode,
        double fixedWeightValue,
        CollectionObjectName outputWeightedPointGroup,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPointWeightsFromUncertaintiesRequest(),
            new Dictionary<string, object?>
            {
                ["point_name_list"] = pointNameList,
                ["uncertainty_reference_frame_mode"] = uncertaintyReferenceFrameMode,
                ["reporting_frame"] = reportingFrame,
                ["weight_normalization_mode"] = weightNormalizationMode,
                ["fixed_weight_value"] = fixedWeightValue,
                ["output_weighted_point_group"] = outputWeightedPointGroup,
            });
        return InvokeOperationAsync<PointName[]>(
            "briosa.AnalysisOperations",
            "SetPointWeightsFromUncertainties",
            request,
            Transport.SetPointWeightsFromUncertaintiesResult.Parser,
            cancellationToken);
    }

    public Task SetTransformForIthFrameInFrameSetAsync(
        CollectionObjectName frameSet,
        int frameSetIndex,
        Transform transformInWorking,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetTransformForIthFrameInFrameSetRequest(),
            new Dictionary<string, object?>
            {
                ["frame_set"] = frameSet,
                ["frame_set_index"] = frameSetIndex,
                ["transform_in_working"] = transformInWorking,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "SetTransformForIthFrameInFrameSet",
            request,
            Transport.SetTransformForIthFrameInFrameSetResult.Parser,
            cancellationToken);
    }

    public Task<SphereAxisCheckResult> SphereAxisCheckAsync(
        CollectionObjectName spherePointsGroupName,
        double sphereTargetRadius,
        PointName pointToCreateAtSphereCenter,
        CollectionObjectName lineDefiningTheAxis,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SphereAxisCheckRequest(),
            new Dictionary<string, object?>
            {
                ["sphere_points_group_name"] = spherePointsGroupName,
                ["sphere_target_radius"] = sphereTargetRadius,
                ["point_to_create_at_sphere_center"] = pointToCreateAtSphereCenter,
                ["line_defining_the_axis"] = lineDefiningTheAxis,
            });
        return InvokeOperationAsync<SphereAxisCheckResult>(
            "briosa.AnalysisOperations",
            "SphereAxisCheck",
            request,
            Transport.SphereAxisCheckResult.Parser,
            cancellationToken);
    }

    public Task TemperatureCompensateAGroupAsync(
        CollectionObjectName originalGroup,
        FrameName scalingOriginCoordinateFrame,
        double materialCte1DegF,
        double initialTemperatureF,
        double finalTemperatureF,
        CollectionObjectName scaledGroupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.TemperatureCompensateAGroupRequest(),
            new Dictionary<string, object?>
            {
                ["original_group"] = originalGroup,
                ["scaling_origin_coordinate_frame"] = scalingOriginCoordinateFrame,
                ["material_cte_1_deg_f"] = materialCte1DegF,
                ["initial_temperature_f"] = initialTemperatureF,
                ["final_temperature_f"] = finalTemperatureF,
                ["scaled_group_name"] = scaledGroupName,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "TemperatureCompensateAGroup",
            request,
            Transport.TemperatureCompensateAGroupResult.Parser,
            cancellationToken);
    }

    public Task TransformObjectsFrameToFrameAsync(
        IEnumerable<CollectionObjectName> objectNameList,
        CollectionObjectName initialFrameName,
        CollectionObjectName destinationFrameName,
        int numberOfSteps = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.TransformObjectsFrameToFrameRequest(),
            new Dictionary<string, object?>
            {
                ["object_name_list"] = objectNameList,
                ["initial_frame_name"] = initialFrameName,
                ["destination_frame_name"] = destinationFrameName,
                ["number_of_steps"] = numberOfSteps,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "TransformObjectsFrameToFrame",
            request,
            Transport.TransformObjectsFrameToFrameResult.Parser,
            cancellationToken);
    }

    public Task TransformObjectsByDeltaAboutWorkingFrameAsync(
        IEnumerable<CollectionObjectName> objectsToTransform,
        Transform deltaTransform,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.TransformObjectsByDeltaAboutWorkingFrameRequest(),
            new Dictionary<string, object?>
            {
                ["objects_to_transform"] = objectsToTransform,
                ["delta_transform"] = deltaTransform,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "TransformObjectsByDeltaAboutWorkingFrame",
            request,
            Transport.TransformObjectsByDeltaAboutWorkingFrameResult.Parser,
            cancellationToken);
    }

    public Task TransformObjectsByDeltaWorldTransformOperatorAsync(
        IEnumerable<CollectionObjectName> objectsToTransform,
        WorldTransform deltaTransform,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.TransformObjectsByDeltaWorldTransformOperatorRequest(),
            new Dictionary<string, object?>
            {
                ["objects_to_transform"] = objectsToTransform,
                ["delta_transform"] = deltaTransform,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "TransformObjectsByDeltaWorldTransformOperator",
            request,
            Transport.TransformObjectsByDeltaWorldTransformOperatorResult.Parser,
            cancellationToken);
    }

    public Task TranslateObjectsByDeltaAsync(
        IEnumerable<CollectionObjectName> objectsToTranslate,
        Vector deltaTranslation,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.TranslateObjectsByDeltaRequest(),
            new Dictionary<string, object?>
            {
                ["objects_to_translate"] = objectsToTranslate,
                ["delta_translation"] = deltaTranslation,
            });
        return InvokeOperationAsync(
            "briosa.AnalysisOperations",
            "TranslateObjectsByDelta",
            request,
            Transport.TranslateObjectsByDeltaResult.Parser,
            cancellationToken);
    }

    public Task DeleteDimensionAsync(
        CollectionObjectName dimensionName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteDimensionRequest(),
            new Dictionary<string, object?>
            {
                ["dimension_name"] = dimensionName,
            });
        return InvokeOperationAsync(
            "briosa.DimensionOperations",
            "DeleteDimension",
            request,
            Transport.DeleteDimensionResult.Parser,
            cancellationToken);
    }

    public Task<GetDimensionValueResult> GetDimensionValueAsync(
        CollectionObjectName dimensionName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetDimensionValueRequest(),
            new Dictionary<string, object?>
            {
                ["dimension_name"] = dimensionName,
            });
        return InvokeOperationAsync<GetDimensionValueResult>(
            "briosa.DimensionOperations",
            "GetDimensionValue",
            request,
            Transport.GetDimensionValueResult.Parser,
            cancellationToken);
    }

    public Task SetDimensionToleranceAsync(
        CollectionItemName dimensionName,
        bool enableNominal = false,
        bool enableHigh = false,
        bool enableLow = false,
        double nominal = 0.000000,
        double highTolerance = 0.000000,
        double lowTolerance = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetDimensionToleranceRequest(),
            new Dictionary<string, object?>
            {
                ["dimension_name"] = dimensionName,
                ["enable_nominal"] = enableNominal,
                ["enable_high"] = enableHigh,
                ["enable_low"] = enableLow,
                ["nominal"] = nominal,
                ["high_tolerance"] = highTolerance,
                ["low_tolerance"] = lowTolerance,
            });
        return InvokeOperationAsync(
            "briosa.DimensionOperations",
            "SetDimensionTolerance",
            request,
            Transport.SetDimensionToleranceResult.Parser,
            cancellationToken);
    }

    public Task DeleteEventAsync(
        CollectionObjectName eventName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteEventRequest(),
            new Dictionary<string, object?>
            {
                ["event_name"] = eventName,
            });
        return InvokeOperationAsync(
            "briosa.EventOperations",
            "DeleteEvent",
            request,
            Transport.DeleteEventResult.Parser,
            cancellationToken);
    }

    public Task ExportEventRefListAsync(
        IEnumerable<CollectionItemName> eventList,
        FileReference filePath,
        int decimalPrecision = 6,
        bool overwriteExistingFile = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportEventRefListRequest(),
            new Dictionary<string, object?>
            {
                ["event_list"] = eventList,
                ["file_path"] = filePath,
                ["decimal_precision"] = decimalPrecision,
                ["overwrite_existing_file"] = overwriteExistingFile,
            });
        return InvokeOperationAsync(
            "briosa.EventOperations",
            "ExportEventRefList",
            request,
            Transport.ExportEventRefListResult.Parser,
            cancellationToken);
    }

    public Task<CollectionItemName> GetIthEventFromEventRefListAsync(
        IEnumerable<CollectionItemName> eventList,
        int eventIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetIthEventFromEventRefListRequest(),
            new Dictionary<string, object?>
            {
                ["event_list"] = eventList,
                ["event_index"] = eventIndex,
            });
        return InvokeOperationAsync<CollectionItemName>(
            "briosa.EventOperations",
            "GetIthEventFromEventRefList",
            request,
            Transport.GetIthEventFromEventRefListResult.Parser,
            cancellationToken);
    }

    public Task<int> GetNumberOfEventsInEventRefListAsync(
        IEnumerable<CollectionItemName> eventList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetNumberOfEventsInEventRefListRequest(),
            new Dictionary<string, object?>
            {
                ["event_list"] = eventList,
            });
        return InvokeOperationAsync<int>(
            "briosa.EventOperations",
            "GetNumberOfEventsInEventRefList",
            request,
            Transport.GetNumberOfEventsInEventRefListResult.Parser,
            cancellationToken);
    }

    public Task RenameEventAsync(
        CollectionObjectName originalEventName,
        CollectionObjectName newEventName,
        bool overwriteIfExists = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RenameEventRequest(),
            new Dictionary<string, object?>
            {
                ["original_event_name"] = originalEventName,
                ["new_event_name"] = newEventName,
                ["overwrite_if_exists"] = overwriteIfExists,
            });
        return InvokeOperationAsync(
            "briosa.EventOperations",
            "RenameEvent",
            request,
            Transport.RenameEventResult.Parser,
            cancellationToken);
    }

    public Task BackupNowAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.BackupNowRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "BackupNow",
            request,
            Transport.BackupNowResult.Parser,
            cancellationToken);
    }

    public Task CopyGeneralFileAsync(
        FileReference sourceFileName,
        FileReference destinationFileName,
        bool overwrite = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CopyGeneralFileRequest(),
            new Dictionary<string, object?>
            {
                ["source_file_name"] = sourceFileName,
                ["destination_file_name"] = destinationFileName,
                ["overwrite"] = overwrite,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "CopyGeneralFile",
            request,
            Transport.CopyGeneralFileResult.Parser,
            cancellationToken);
    }

    public Task DeleteGeneralFileAsync(
        FileReference fileName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteGeneralFileRequest(),
            new Dictionary<string, object?>
            {
                ["file_name"] = fileName,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "DeleteGeneralFile",
            request,
            Transport.DeleteGeneralFileResult.Parser,
            cancellationToken);
    }

    public Task<DirectCadAccessResult> DirectCadAccessAsync(
        FileReference cadFileName,
        bool importSolids = true,
        bool importSurfaces = true,
        bool importPolygonizedSurfaces = true,
        bool importAnnotations = true,
        bool importVectors = true,
        bool importPoints = true,
        string pointGroupName = "CAD pts",
        bool importAttributesMetadata = true,
        bool importCooordinateFrames = true,
        bool importPlanes = true,
        bool import3DCurvesLines = true,
        bool import3DCurvesCircles = true,
        bool import3DCurvesGeneralCurves = true,
        bool importConstructionGeometry = false,
        bool importHiddenEntities = false,
        bool importAllSurfacesAsMeshGraphicalEntities = false,
        bool doNotImportFillets = false,
        bool doNotImportDittos = false,
        int dittoThreshold = 1,
        bool centerViewOnImportedObjects = true,
        bool importIntoFoldersMatchingCadFileHierarchy = false,
        bool removeEmptyFolders = true,
        int surfaceNormalsMode1Or2 = 1,
        bool promptOnMissingComponents = true,
        bool selectiveImport = false,
        bool surfaceCompatibilityMode = true,
        bool explodeSurfaces = false,
        string cadFileUnitsLeaveBlankToUseTheUnitsSpecifiedInTheFile = "",
        bool buildCalloutViews = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DirectCadAccessRequest(),
            new Dictionary<string, object?>
            {
                ["cad_file_name"] = cadFileName,
                ["import_solids"] = importSolids,
                ["import_surfaces"] = importSurfaces,
                ["import_polygonized_surfaces"] = importPolygonizedSurfaces,
                ["import_annotations"] = importAnnotations,
                ["import_vectors"] = importVectors,
                ["import_points"] = importPoints,
                ["point_group_name"] = pointGroupName,
                ["import_attributes_metadata"] = importAttributesMetadata,
                ["import_cooordinate_frames"] = importCooordinateFrames,
                ["import_planes"] = importPlanes,
                ["import_3d_curves_lines"] = import3DCurvesLines,
                ["import_3d_curves_circles"] = import3DCurvesCircles,
                ["import_3d_curves_general_curves"] = import3DCurvesGeneralCurves,
                ["import_construction_geometry"] = importConstructionGeometry,
                ["import_hidden_entities"] = importHiddenEntities,
                ["import_all_surfaces_as_mesh_graphical_entities"] = importAllSurfacesAsMeshGraphicalEntities,
                ["do_not_import_fillets"] = doNotImportFillets,
                ["do_not_import_dittos"] = doNotImportDittos,
                ["ditto_threshold"] = dittoThreshold,
                ["center_view_on_imported_objects"] = centerViewOnImportedObjects,
                ["import_into_folders_matching_cad_file_hierarchy"] = importIntoFoldersMatchingCadFileHierarchy,
                ["remove_empty_folders"] = removeEmptyFolders,
                ["surface_normals_mode_1_or_2"] = surfaceNormalsMode1Or2,
                ["prompt_on_missing_components"] = promptOnMissingComponents,
                ["selective_import"] = selectiveImport,
                ["surface_compatibility_mode"] = surfaceCompatibilityMode,
                ["explode_surfaces"] = explodeSurfaces,
                ["cad_file_units_leave_blank_to_use_the_units_specified_in_the_file"] = cadFileUnitsLeaveBlankToUseTheUnitsSpecifiedInTheFile,
                ["build_callout_views"] = buildCalloutViews,
            });
        return InvokeOperationAsync<DirectCadAccessResult>(
            "briosa.FileOperations",
            "DirectCadAccess",
            request,
            Transport.DirectCadAccessResult.Parser,
            cancellationToken);
    }

    public Task ExportAsciiFrameSetAsync(
        FileReference asciiFilePath,
        CollectionObjectName frameSetContainer,
        ExportDataDelimeterType dataDelimiter,
        AsciiFileFormat fileFormat,
        bool includeExportFormatInfo = false,
        int decimalPrecision = 6,
        bool append = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportAsciiFrameSetRequest(),
            new Dictionary<string, object?>
            {
                ["ascii_file_path"] = asciiFilePath,
                ["frame_set_container"] = frameSetContainer,
                ["data_delimiter"] = dataDelimiter,
                ["file_format"] = fileFormat,
                ["include_export_format_info"] = includeExportFormatInfo,
                ["decimal_precision"] = decimalPrecision,
                ["append"] = append,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportAsciiFrameSet",
            request,
            Transport.ExportAsciiFrameSetResult.Parser,
            cancellationToken);
    }

    public Task ExportAsciiFramesAsync(
        FileReference asciiFilePath,
        IEnumerable<CollectionObjectName> objectList,
        string exportFrameMode = "Fixed XYZ",
        bool overwriteExistingFile = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportAsciiFramesRequest(),
            new Dictionary<string, object?>
            {
                ["ascii_file_path"] = asciiFilePath,
                ["object_list"] = objectList,
                ["export_frame_mode"] = exportFrameMode,
                ["overwrite_existing_file"] = overwriteExistingFile,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportAsciiFrames",
            request,
            Transport.ExportAsciiFramesResult.Parser,
            cancellationToken);
    }

    public Task ExportAsciiPointCloudsAsync(
        FileReference asciiFilePath,
        IEnumerable<CollectionObjectName> pointCloudList,
        ExportDataDelimeterType dataDelimiter,
        bool overwriteExistingFile = false,
        bool showProgressDialog = false,
        bool includeCloudPointLabeling = false,
        bool includeScanDirectionVector = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportAsciiPointCloudsRequest(),
            new Dictionary<string, object?>
            {
                ["ascii_file_path"] = asciiFilePath,
                ["point_cloud_list"] = pointCloudList,
                ["data_delimiter"] = dataDelimiter,
                ["overwrite_existing_file"] = overwriteExistingFile,
                ["show_progress_dialog"] = showProgressDialog,
                ["include_cloud_point_labeling"] = includeCloudPointLabeling,
                ["include_scan_direction_vector"] = includeScanDirectionVector,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportAsciiPointClouds",
            request,
            Transport.ExportAsciiPointCloudsResult.Parser,
            cancellationToken);
    }

    public Task ExportAsciiPointSetAsync(
        FileReference asciiFilePath,
        CollectionObjectName pointSetContainer,
        ExportDataDelimeterType dataDelimiter,
        ExportTargetNameFormat targetNameFormat,
        CoordinateSystemType desiredCoordinateSystem,
        bool includeTargetOffsets = false,
        bool includeTimestamps = false,
        bool includeSaVersionAndFrameComments = false,
        bool includeAxisComments = false,
        bool includeExportFormatInfo = false,
        bool maximumPrecisionScientificNotation = false,
        int decimalPrecision = 6,
        bool append = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportAsciiPointSetRequest(),
            new Dictionary<string, object?>
            {
                ["ascii_file_path"] = asciiFilePath,
                ["point_set_container"] = pointSetContainer,
                ["data_delimiter"] = dataDelimiter,
                ["target_name_format"] = targetNameFormat,
                ["desired_coordinate_system"] = desiredCoordinateSystem,
                ["include_target_offsets"] = includeTargetOffsets,
                ["include_timestamps"] = includeTimestamps,
                ["include_sa_version_and_frame_comments"] = includeSaVersionAndFrameComments,
                ["include_axis_comments"] = includeAxisComments,
                ["include_export_format_info"] = includeExportFormatInfo,
                ["maximum_precision_scientific_notation"] = maximumPrecisionScientificNotation,
                ["decimal_precision"] = decimalPrecision,
                ["append"] = append,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportAsciiPointSet",
            request,
            Transport.ExportAsciiPointSetResult.Parser,
            cancellationToken);
    }

    public Task ExportAsciiPointsAsync(
        FileReference asciiFilePath,
        IEnumerable<CollectionGroupName> groupNamesToExport,
        ExportDataDelimeterType dataDelimiter,
        ExportTargetNameFormat targetNameFormat,
        CoordinateSystemType desiredCoordinateSystem,
        bool includeTargetOffsets = false,
        bool includeTargetComments = false,
        bool includeTimestamps = false,
        bool includeTolerances = false,
        bool includeCoordinateUncertainties = false,
        bool includeSaVersionAndFrameComments = false,
        bool includeAxisComments = false,
        bool includeExportFormatInfo = false,
        bool includeWeights = false,
        bool includeMeasurementDetails = false,
        bool maximumPrecisionScientificNotation = false,
        int decimalPrecision = 6,
        bool append = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportAsciiPointsRequest(),
            new Dictionary<string, object?>
            {
                ["ascii_file_path"] = asciiFilePath,
                ["group_names_to_export"] = groupNamesToExport,
                ["data_delimiter"] = dataDelimiter,
                ["target_name_format"] = targetNameFormat,
                ["desired_coordinate_system"] = desiredCoordinateSystem,
                ["include_target_offsets"] = includeTargetOffsets,
                ["include_target_comments"] = includeTargetComments,
                ["include_timestamps"] = includeTimestamps,
                ["include_tolerances"] = includeTolerances,
                ["include_coordinate_uncertainties"] = includeCoordinateUncertainties,
                ["include_sa_version_and_frame_comments"] = includeSaVersionAndFrameComments,
                ["include_axis_comments"] = includeAxisComments,
                ["include_export_format_info"] = includeExportFormatInfo,
                ["include_weights"] = includeWeights,
                ["include_measurement_details"] = includeMeasurementDetails,
                ["maximum_precision_scientific_notation"] = maximumPrecisionScientificNotation,
                ["decimal_precision"] = decimalPrecision,
                ["append"] = append,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportAsciiPoints",
            request,
            Transport.ExportAsciiPointsResult.Parser,
            cancellationToken);
    }

    public Task ExportDxfAsync(
        FileReference dxfFilePath,
        IEnumerable<PointName> pointNames,
        IEnumerable<CollectionObjectName> cloudNames,
        bool includePointLabels = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportDxfRequest(),
            new Dictionary<string, object?>
            {
                ["dxf_file_path"] = dxfFilePath,
                ["point_names"] = pointNames,
                ["cloud_names"] = cloudNames,
                ["include_point_labels"] = includePointLabels,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportDxf",
            request,
            Transport.ExportDxfResult.Parser,
            cancellationToken);
    }

    public Task ExportEmbeddedFileAsync(
        CollectionName embeddedFileCollectionName,
        string embeddedFileName,
        FileReference externalFileName,
        bool replaceExisting = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportEmbeddedFileRequest(),
            new Dictionary<string, object?>
            {
                ["embedded_file_collection_name"] = embeddedFileCollectionName,
                ["embedded_file_name"] = embeddedFileName,
                ["external_file_name"] = externalFileName,
                ["replace_existing"] = replaceExisting,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportEmbeddedFile",
            request,
            Transport.ExportEmbeddedFileResult.Parser,
            cancellationToken);
    }

    public Task ExportHiddenPointBarXmlFileAsync(
        FileReference xmlFilePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportHiddenPointBarXmlFileRequest(),
            new Dictionary<string, object?>
            {
                ["xml_file_path"] = xmlFilePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportHiddenPointBarXmlFile",
            request,
            Transport.ExportHiddenPointBarXmlFileResult.Parser,
            cancellationToken);
    }

    public Task ExportIgesFileEntireModelAsync(
        FileReference igesFilePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportIgesFileEntireModelRequest(),
            new Dictionary<string, object?>
            {
                ["iges_file_path"] = igesFilePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportIgesFileEntireModel",
            request,
            Transport.ExportIgesFileEntireModelResult.Parser,
            cancellationToken);
    }

    public Task ExportIgesFilePartialModelAsync(
        FileReference igesFilePath,
        IEnumerable<CollectionObjectName> objectNameList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportIgesFilePartialModelRequest(),
            new Dictionary<string, object?>
            {
                ["iges_file_path"] = igesFilePath,
                ["object_name_list"] = objectNameList,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportIgesFilePartialModel",
            request,
            Transport.ExportIgesFilePartialModelResult.Parser,
            cancellationToken);
    }

    public Task ExportPtxPointCloudsAsync(
        FileReference ptxFilePath,
        IEnumerable<CollectionObjectName> pointCloudList,
        bool overwriteExistingFile = false,
        bool showProgressDialog = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportPtxPointCloudsRequest(),
            new Dictionary<string, object?>
            {
                ["ptx_file_path"] = ptxFilePath,
                ["point_cloud_list"] = pointCloudList,
                ["overwrite_existing_file"] = overwriteExistingFile,
                ["show_progress_dialog"] = showProgressDialog,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportPtxPointClouds",
            request,
            Transport.ExportPtxPointCloudsResult.Parser,
            cancellationToken);
    }

    public Task ExportQdasCharacteristicsAsync(
        FileReference qdasExportFilePath,
        string k1001PartNumber,
        string k1002PartDescription,
        string k1071SupplierNumber,
        string k1072SupplierDescription,
        string k1203ReasonForTest,
        string k1303Plant,
        string k1900PartRemark,
        string k0006BatchNumber,
        string k0014PartId,
        string k0053OrderNumber,
        string k0004DateTimeStamp,
        int k0008OperatorIdentifier,
        int k0010MachineIdentifier,
        int k0012GageIdentifier,
        IEnumerable<CollectionItemName> relationshipList,
        IEnumerable<CollectionItemName> featureCheckList,
        IEnumerable<CollectionObjectName> vectorGroupList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportQdasCharacteristicsRequest(),
            new Dictionary<string, object?>
            {
                ["qdas_export_file_path"] = qdasExportFilePath,
                ["k1001_part_number"] = k1001PartNumber,
                ["k1002_part_description"] = k1002PartDescription,
                ["k1071_supplier_number"] = k1071SupplierNumber,
                ["k1072_supplier_description"] = k1072SupplierDescription,
                ["k1203_reason_for_test"] = k1203ReasonForTest,
                ["k1303_plant"] = k1303Plant,
                ["k1900_part_remark"] = k1900PartRemark,
                ["k0006_batch_number"] = k0006BatchNumber,
                ["k0014_part_id"] = k0014PartId,
                ["k0053_order_number"] = k0053OrderNumber,
                ["k0004_date_time_stamp"] = k0004DateTimeStamp,
                ["k0008_operator_identifier"] = k0008OperatorIdentifier,
                ["k0010_machine_identifier"] = k0010MachineIdentifier,
                ["k0012_gage_identifier"] = k0012GageIdentifier,
                ["relationship_list"] = relationshipList,
                ["feature_check_list"] = featureCheckList,
                ["vector_group_list"] = vectorGroupList,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportQdasCharacteristics",
            request,
            Transport.ExportQdasCharacteristicsResult.Parser,
            cancellationToken);
    }

    public Task ExportQdasDataListAsync(
        FileReference qdasExportFilePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportQdasDataListRequest(),
            new Dictionary<string, object?>
            {
                ["qdas_export_file_path"] = qdasExportFilePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportQdasDataList",
            request,
            Transport.ExportQdasDataListResult.Parser,
            cancellationToken);
    }

    public Task ExportScanStripeMeshToStlFileAsync(
        FileReference stlFilePath,
        CollectionObjectName mesh,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportScanStripeMeshToStlFileRequest(),
            new Dictionary<string, object?>
            {
                ["stl_file_path"] = stlFilePath,
                ["mesh"] = mesh,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportScanStripeMeshToStlFile",
            request,
            Transport.ExportScanStripeMeshToStlFileResult.Parser,
            cancellationToken);
    }

    public Task ExportStepFileEntireModelAsync(
        FileReference stepFilePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportStepFileEntireModelRequest(),
            new Dictionary<string, object?>
            {
                ["step_file_path"] = stepFilePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportStepFileEntireModel",
            request,
            Transport.ExportStepFileEntireModelResult.Parser,
            cancellationToken);
    }

    public Task ExportStepFilePartialModelAsync(
        FileReference stepFilePath,
        IEnumerable<CollectionObjectName> objectNameList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportStepFilePartialModelRequest(),
            new Dictionary<string, object?>
            {
                ["step_file_path"] = stepFilePath,
                ["object_name_list"] = objectNameList,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportStepFilePartialModel",
            request,
            Transport.ExportStepFilePartialModelResult.Parser,
            cancellationToken);
    }

    public Task ExportVdaFsFileEntireModelAsync(
        FileReference vdaFsFilePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportVdaFsFileEntireModelRequest(),
            new Dictionary<string, object?>
            {
                ["vda_fs_file_path"] = vdaFsFilePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportVdaFsFileEntireModel",
            request,
            Transport.ExportVdaFsFileEntireModelResult.Parser,
            cancellationToken);
    }

    public Task ExportVdaFsFilePartialModelAsync(
        FileReference vdaFsFilePath,
        IEnumerable<CollectionObjectName> objectNameList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportVdaFsFilePartialModelRequest(),
            new Dictionary<string, object?>
            {
                ["vda_fs_file_path"] = vdaFsFilePath,
                ["object_name_list"] = objectNameList,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportVdaFsFilePartialModel",
            request,
            Transport.ExportVdaFsFilePartialModelResult.Parser,
            cancellationToken);
    }

    public Task ExportVectorContainerToAsciiFileAsync(
        FileReference asciiFilePath,
        IEnumerable<CollectionVectorGroupName> vectorGroupsToExport,
        bool overwriteExistingFileFalseAppend,
        bool useFullPrecisionScientificNotation,
        ExportVectorNameFormat vectorNameFormat,
        bool includeVectorLength = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportVectorContainerToAsciiFileRequest(),
            new Dictionary<string, object?>
            {
                ["ascii_file_path"] = asciiFilePath,
                ["vector_groups_to_export"] = vectorGroupsToExport,
                ["overwrite_existing_file_false_append"] = overwriteExistingFileFalseAppend,
                ["use_full_precision_scientific_notation"] = useFullPrecisionScientificNotation,
                ["vector_name_format"] = vectorNameFormat,
                ["include_vector_length"] = includeVectorLength,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ExportVectorContainerToAsciiFile",
            request,
            Transport.ExportVectorContainerToAsciiFileResult.Parser,
            cancellationToken);
    }

    public Task<string[]> FindFilesInDirectoryAsync(
        string directory = "",
        string fileNamePattern = "*.*",
        bool recursive = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FindFilesInDirectoryRequest(),
            new Dictionary<string, object?>
            {
                ["directory"] = directory,
                ["file_name_pattern"] = fileNamePattern,
                ["recursive"] = recursive,
            });
        return InvokeOperationAsync<string[]>(
            "briosa.FileOperations",
            "FindFilesInDirectory",
            request,
            Transport.FindFilesInDirectoryResult.Parser,
            cancellationToken);
    }

    public Task<string[]> FindSubDirectoriesInDirectoryAsync(
        string directory = "",
        bool recursive = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FindSubDirectoriesInDirectoryRequest(),
            new Dictionary<string, object?>
            {
                ["directory"] = directory,
                ["recursive"] = recursive,
            });
        return InvokeOperationAsync<string[]>(
            "briosa.FileOperations",
            "FindSubDirectoriesInDirectory",
            request,
            Transport.FindSubDirectoriesInDirectoryResult.Parser,
            cancellationToken);
    }

    public Task<bool> GetBooleanFromDataShareFileAsync(
        FileReference dataShareFilePath,
        string booleanName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetBooleanFromDataShareFileRequest(),
            new Dictionary<string, object?>
            {
                ["data_share_file_path"] = dataShareFilePath,
                ["boolean_name"] = booleanName,
            });
        return InvokeOperationAsync<bool>(
            "briosa.FileOperations",
            "GetBooleanFromDataShareFile",
            request,
            Transport.GetBooleanFromDataShareFileResult.Parser,
            cancellationToken);
    }

    public Task<double> GetDoubleFromDataShareFileAsync(
        FileReference dataShareFilePath,
        string doubleName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetDoubleFromDataShareFileRequest(),
            new Dictionary<string, object?>
            {
                ["data_share_file_path"] = dataShareFilePath,
                ["double_name"] = doubleName,
            });
        return InvokeOperationAsync<double>(
            "briosa.FileOperations",
            "GetDoubleFromDataShareFile",
            request,
            Transport.GetDoubleFromDataShareFileResult.Parser,
            cancellationToken);
    }

    public Task<int> GetIntegerFromDataShareFileAsync(
        FileReference dataShareFilePath,
        string integerName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetIntegerFromDataShareFileRequest(),
            new Dictionary<string, object?>
            {
                ["data_share_file_path"] = dataShareFilePath,
                ["integer_name"] = integerName,
            });
        return InvokeOperationAsync<int>(
            "briosa.FileOperations",
            "GetIntegerFromDataShareFile",
            request,
            Transport.GetIntegerFromDataShareFileResult.Parser,
            cancellationToken);
    }

    public Task<string[]> GetQdasCatalogEntriesAsync(
        string kFieldTarget = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetQdasCatalogEntriesRequest(),
            new Dictionary<string, object?>
            {
                ["k_field_target"] = kFieldTarget,
            });
        return InvokeOperationAsync<string[]>(
            "briosa.FileOperations",
            "GetQdasCatalogEntries",
            request,
            Transport.GetQdasCatalogEntriesResult.Parser,
            cancellationToken);
    }

    public Task<string> GetStringFromDataShareFileAsync(
        FileReference dataShareFilePath,
        string stringName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetStringFromDataShareFileRequest(),
            new Dictionary<string, object?>
            {
                ["data_share_file_path"] = dataShareFilePath,
                ["string_name"] = stringName,
            });
        return InvokeOperationAsync<string>(
            "briosa.FileOperations",
            "GetStringFromDataShareFile",
            request,
            Transport.GetStringFromDataShareFileResult.Parser,
            cancellationToken);
    }

    public Task<Transform> GetTransformFromDataShareFileAsync(
        FileReference dataShareFilePath,
        string transformName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetTransformFromDataShareFileRequest(),
            new Dictionary<string, object?>
            {
                ["data_share_file_path"] = dataShareFilePath,
                ["transform_name"] = transformName,
            });
        return InvokeOperationAsync<Transform>(
            "briosa.FileOperations",
            "GetTransformFromDataShareFile",
            request,
            Transport.GetTransformFromDataShareFileResult.Parser,
            cancellationToken);
    }

    public Task<Vector> GetVectorFromDataShareFileAsync(
        FileReference dataShareFilePath,
        string vectorName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetVectorFromDataShareFileRequest(),
            new Dictionary<string, object?>
            {
                ["data_share_file_path"] = dataShareFilePath,
                ["vector_name"] = vectorName,
            });
        return InvokeOperationAsync<Vector>(
            "briosa.FileOperations",
            "GetVectorFromDataShareFile",
            request,
            Transport.GetVectorFromDataShareFileResult.Parser,
            cancellationToken);
    }

    public Task ImportAsciiPredefinedFormatsAsync(
        FileReference asciiFilePath,
        AsciiFileFormat fileFormat,
        DistanceUnits units,
        AngularUnits angularUnits,
        CollectionObjectName groupName,
        bool importAsCloud = false,
        bool ensureNewPointGroup = true,
        bool ensureUniqueNames = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportAsciiPredefinedFormatsRequest(),
            new Dictionary<string, object?>
            {
                ["ascii_file_path"] = asciiFilePath,
                ["file_format"] = fileFormat,
                ["units"] = units,
                ["angular_units"] = angularUnits,
                ["group_name"] = groupName,
                ["import_as_cloud"] = importAsCloud,
                ["ensure_new_point_group"] = ensureNewPointGroup,
                ["ensure_unique_names"] = ensureUniqueNames,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportAsciiPredefinedFormats",
            request,
            Transport.ImportAsciiPredefinedFormatsResult.Parser,
            cancellationToken);
    }

    public Task ImportAsciiPredefinedFrameSetFormatsAsync(
        FileReference asciiFilePath,
        AsciiFileFormat fileFormat,
        DistanceUnits units,
        AngularUnits angularUnits,
        CollectionObjectName frameSetContainerName,
        bool ensureUniqueName = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportAsciiPredefinedFrameSetFormatsRequest(),
            new Dictionary<string, object?>
            {
                ["ascii_file_path"] = asciiFilePath,
                ["file_format"] = fileFormat,
                ["units"] = units,
                ["angular_units"] = angularUnits,
                ["frame_set_container_name"] = frameSetContainerName,
                ["ensure_unique_name"] = ensureUniqueName,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportAsciiPredefinedFrameSetFormats",
            request,
            Transport.ImportAsciiPredefinedFrameSetFormatsResult.Parser,
            cancellationToken);
    }

    public Task ImportE57FileAsync(
        FileReference e57FilePath,
        bool saveConvertedFile,
        bool useSquareRootOfIntensity,
        bool automaticallyCloseConverter,
        bool prioritizeColorOverIntensity,
        bool importScanBlocksAsSeparateClouds,
        DistanceUnits units,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportE57FileRequest(),
            new Dictionary<string, object?>
            {
                ["e57_file_path"] = e57FilePath,
                ["save_converted_file"] = saveConvertedFile,
                ["use_square_root_of_intensity"] = useSquareRootOfIntensity,
                ["automatically_close_converter"] = automaticallyCloseConverter,
                ["prioritize_color_over_intensity"] = prioritizeColorOverIntensity,
                ["import_scan_blocks_as_separate_clouds"] = importScanBlocksAsSeparateClouds,
                ["units"] = units,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportE57File",
            request,
            Transport.ImportE57FileResult.Parser,
            cancellationToken);
    }

    public Task ImportFileAsEmbeddedFileAsync(
        FileReference externalFileName,
        bool replaceExisting = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportFileAsEmbeddedFileRequest(),
            new Dictionary<string, object?>
            {
                ["external_file_name"] = externalFileName,
                ["replace_existing"] = replaceExisting,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportFileAsEmbeddedFile",
            request,
            Transport.ImportFileAsEmbeddedFileResult.Parser,
            cancellationToken);
    }

    public Task ImportFileAsPictureAsync(
        FileReference externalFileName,
        bool replaceExisting = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportFileAsPictureRequest(),
            new Dictionary<string, object?>
            {
                ["external_file_name"] = externalFileName,
                ["replace_existing"] = replaceExisting,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportFileAsPicture",
            request,
            Transport.ImportFileAsPictureResult.Parser,
            cancellationToken);
    }

    public Task ImportHiddenPointBarXmlFileAsync(
        FileReference xmlFilePath,
        bool replaceExistingEntries = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportHiddenPointBarXmlFileRequest(),
            new Dictionary<string, object?>
            {
                ["xml_file_path"] = xmlFilePath,
                ["replace_existing_entries"] = replaceExistingEntries,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportHiddenPointBarXmlFile",
            request,
            Transport.ImportHiddenPointBarXmlFileResult.Parser,
            cancellationToken);
    }

    public Task ImportIgesFileAsync(
        FileReference igesFilePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportIgesFileRequest(),
            new Dictionary<string, object?>
            {
                ["iges_file_path"] = igesFilePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportIgesFile",
            request,
            Transport.ImportIgesFileResult.Parser,
            cancellationToken);
    }

    public Task ImportLeicaGsiFileAsync(
        CollectionInstrumentId instrumentId,
        CollectionObjectName groupName,
        FileReference filePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportLeicaGsiFileRequest(),
            new Dictionary<string, object?>
            {
                ["instrument_id"] = instrumentId,
                ["group_name"] = groupName,
                ["file_path"] = filePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportLeicaGsiFile",
            request,
            Transport.ImportLeicaGsiFileResult.Parser,
            cancellationToken);
    }

    public Task ImportLeicaSdbFileAsync(
        CollectionInstrumentId instrumentId,
        CollectionObjectName scanCloudName,
        FileReference filePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportLeicaSdbFileRequest(),
            new Dictionary<string, object?>
            {
                ["instrument_id"] = instrumentId,
                ["scan_cloud_name"] = scanCloudName,
                ["file_path"] = filePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportLeicaSdbFile",
            request,
            Transport.ImportLeicaSdbFileResult.Parser,
            cancellationToken);
    }

    public Task ImportMpFileAsEmbeddedMpAsync(
        FileReference externalMpFileName,
        bool replaceExisting = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportMpFileAsEmbeddedMpRequest(),
            new Dictionary<string, object?>
            {
                ["external_mp_file_name"] = externalMpFileName,
                ["replace_existing"] = replaceExisting,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportMpFileAsEmbeddedMp",
            request,
            Transport.ImportMpFileAsEmbeddedMpResult.Parser,
            cancellationToken);
    }

    public Task ImportNominalsFromXmlFileAsync(
        FileReference filePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportNominalsFromXmlFileRequest(),
            new Dictionary<string, object?>
            {
                ["file_path"] = filePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportNominalsFromXmlFile",
            request,
            Transport.ImportNominalsFromXmlFileResult.Parser,
            cancellationToken);
    }

    public Task ImportPolyworksFileAsync(
        CollectionObjectName cloudName,
        FileReference filePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportPolyworksFileRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_name"] = cloudName,
                ["file_path"] = filePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportPolyworksFile",
            request,
            Transport.ImportPolyworksFileResult.Parser,
            cancellationToken);
    }

    public Task ImportQdasCatalogFileAsync(
        FileReference qdasDfdFilePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportQdasCatalogFileRequest(),
            new Dictionary<string, object?>
            {
                ["qdas_dfd_file_path"] = qdasDfdFilePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportQdasCatalogFile",
            request,
            Transport.ImportQdasCatalogFileResult.Parser,
            cancellationToken);
    }

    public Task ImportSaFileAsync(
        FileReference saFileName,
        bool allowOperatorSelections,
        IEnumerable<string> selectedCollectionsOptional,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportSaFileRequest(),
            new Dictionary<string, object?>
            {
                ["sa_file_name"] = saFileName,
                ["allow_operator_selections"] = allowOperatorSelections,
                ["selected_collections_optional"] = selectedCollectionsOptional,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportSaFile",
            request,
            Transport.ImportSaFileResult.Parser,
            cancellationToken);
    }

    public Task ImportSaWindowsPlacementAsync(
        FileReference filePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportSaWindowsPlacementRequest(),
            new Dictionary<string, object?>
            {
                ["file_path"] = filePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportSaWindowsPlacement",
            request,
            Transport.ImportSaWindowsPlacementResult.Parser,
            cancellationToken);
    }

    public Task ImportSatFileAsync(
        FileReference satFilePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportSatFileRequest(),
            new Dictionary<string, object?>
            {
                ["sat_file_path"] = satFilePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportSatFile",
            request,
            Transport.ImportSatFileResult.Parser,
            cancellationToken);
    }

    public Task ImportStepFileAsync(
        FileReference stepFilePath,
        bool displayEntityFilters = false,
        bool displayResiduals = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportStepFileRequest(),
            new Dictionary<string, object?>
            {
                ["step_file_path"] = stepFilePath,
                ["display_entity_filters"] = displayEntityFilters,
                ["display_residuals"] = displayResiduals,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportStepFile",
            request,
            Transport.ImportStepFileResult.Parser,
            cancellationToken);
    }

    public Task ImportStlFileAsync(
        FileReference stlFilePath,
        DistanceUnits units,
        bool importMesh = true,
        bool importPointCloud = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportStlFileRequest(),
            new Dictionary<string, object?>
            {
                ["stl_file_path"] = stlFilePath,
                ["units"] = units,
                ["import_mesh"] = importMesh,
                ["import_point_cloud"] = importPointCloud,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportStlFile",
            request,
            Transport.ImportStlFileResult.Parser,
            cancellationToken);
    }

    public Task ImportVdaFsFileAsync(
        FileReference vdaFsFilePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportVdaFsFileRequest(),
            new Dictionary<string, object?>
            {
                ["vda_fs_file_path"] = vdaFsFilePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportVdaFsFile",
            request,
            Transport.ImportVdaFsFileResult.Parser,
            cancellationToken);
    }

    public Task ImportVstarsXyzFileAsync(
        FileReference filePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportVstarsXyzFileRequest(),
            new Dictionary<string, object?>
            {
                ["file_path"] = filePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportVstarsXyzFile",
            request,
            Transport.ImportVstarsXyzFileResult.Parser,
            cancellationToken);
    }

    public Task ImportVstarsCamerasAsync(
        FileReference filePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportVstarsCamerasRequest(),
            new Dictionary<string, object?>
            {
                ["file_path"] = filePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "ImportVstarsCameras",
            request,
            Transport.ImportVstarsCamerasResult.Parser,
            cancellationToken);
    }

    public Task LoadHtmlFormAsync(
        FileReference inputHtmlFormPath,
        int windowWidth,
        int windowHeight,
        FileReference inputDataShareFilePath,
        FileReference outputDataShareFilePath,
        bool saveInBinaryFormat = false,
        string saveButtonText = "Save",
        string cancelButtonText = "Cancel",
        bool hideSaveAndCancelButtons = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LoadHtmlFormRequest(),
            new Dictionary<string, object?>
            {
                ["input_html_form_path"] = inputHtmlFormPath,
                ["window_width"] = windowWidth,
                ["window_height"] = windowHeight,
                ["input_data_share_file_path"] = inputDataShareFilePath,
                ["output_data_share_file_path"] = outputDataShareFilePath,
                ["save_in_binary_format"] = saveInBinaryFormat,
                ["save_button_text"] = saveButtonText,
                ["cancel_button_text"] = cancelButtonText,
                ["hide_save_and_cancel_buttons"] = hideSaveAndCancelButtons,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "LoadHtmlForm",
            request,
            Transport.LoadHtmlFormResult.Parser,
            cancellationToken);
    }

    public Task LoadHtmlFormInEdgeBrowserAsync(
        FileReference inputHtmlFormPath,
        int windowWidth,
        int windowHeight,
        FileReference inputDataShareFilePath,
        FileReference outputDataShareFilePath,
        bool saveInBinaryFormat = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LoadHtmlFormInEdgeBrowserRequest(),
            new Dictionary<string, object?>
            {
                ["input_html_form_path"] = inputHtmlFormPath,
                ["window_width"] = windowWidth,
                ["window_height"] = windowHeight,
                ["input_data_share_file_path"] = inputDataShareFilePath,
                ["output_data_share_file_path"] = outputDataShareFilePath,
                ["save_in_binary_format"] = saveInBinaryFormat,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "LoadHtmlFormInEdgeBrowser",
            request,
            Transport.LoadHtmlFormInEdgeBrowserResult.Parser,
            cancellationToken);
    }

    public Task<string[]> MakeEmbeddedFileNameListAsync(
        string collectionWildcardCriteria = "*",
        string fileNamePattern = "*.*",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeEmbeddedFileNameListRequest(),
            new Dictionary<string, object?>
            {
                ["collection_wildcard_criteria"] = collectionWildcardCriteria,
                ["file_name_pattern"] = fileNamePattern,
            });
        return InvokeOperationAsync<string[]>(
            "briosa.FileOperations",
            "MakeEmbeddedFileNameList",
            request,
            Transport.MakeEmbeddedFileNameListResult.Parser,
            cancellationToken);
    }

    public Task MergeMeasurementsIntoXmlFileAsync(
        FileReference filePath,
        CollectionObjectName groupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MergeMeasurementsIntoXmlFileRequest(),
            new Dictionary<string, object?>
            {
                ["file_path"] = filePath,
                ["group_name"] = groupName,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "MergeMeasurementsIntoXmlFile",
            request,
            Transport.MergeMeasurementsIntoXmlFileResult.Parser,
            cancellationToken);
    }

    public Task NewSaFileAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.NewSaFileRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "NewSaFile",
            request,
            Transport.NewSaFileResult.Parser,
            cancellationToken);
    }

    public Task OpenSaFileAsync(
        FileReference saFileName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.OpenSaFileRequest(),
            new Dictionary<string, object?>
            {
                ["sa_file_name"] = saFileName,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "OpenSaFile",
            request,
            Transport.OpenSaFileResult.Parser,
            cancellationToken);
    }

    public Task OpenTemplateFileAsync(
        FileReference templateFileName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.OpenTemplateFileRequest(),
            new Dictionary<string, object?>
            {
                ["template_file_name"] = templateFileName,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "OpenTemplateFile",
            request,
            Transport.OpenTemplateFileResult.Parser,
            cancellationToken);
    }

    public Task PopPolyBayAnalysisWindowAsync(
        string materialsFilePath = "",
        string bayFilePath = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.PopPolyBayAnalysisWindowRequest(),
            new Dictionary<string, object?>
            {
                ["materials_file_path"] = materialsFilePath,
                ["bay_file_path"] = bayFilePath,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "PopPolyBayAnalysisWindow",
            request,
            Transport.PopPolyBayAnalysisWindowResult.Parser,
            cancellationToken);
    }

    public Task PrepareQdasDataListAsync(
        string k1001PartNumber,
        string k1002PartDescription,
        string k1071SupplierNumber,
        string k1072SupplierDescription,
        string k1203ReasonForTest,
        string k1303Plant,
        string k1900PartRemark,
        string k0006BatchNumber,
        string k0014PartId,
        string k0053OrderNumber,
        string k0004DateTimeStamp,
        int k0008OperatorIdentifier,
        int k0010MachineIdentifier,
        int k0012GageIdentifier,
        IEnumerable<CollectionItemName> relationshipList,
        IEnumerable<CollectionItemName> featureCheckList,
        IEnumerable<CollectionObjectName> vectorGroupList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.PrepareQdasDataListRequest(),
            new Dictionary<string, object?>
            {
                ["k1001_part_number"] = k1001PartNumber,
                ["k1002_part_description"] = k1002PartDescription,
                ["k1071_supplier_number"] = k1071SupplierNumber,
                ["k1072_supplier_description"] = k1072SupplierDescription,
                ["k1203_reason_for_test"] = k1203ReasonForTest,
                ["k1303_plant"] = k1303Plant,
                ["k1900_part_remark"] = k1900PartRemark,
                ["k0006_batch_number"] = k0006BatchNumber,
                ["k0014_part_id"] = k0014PartId,
                ["k0053_order_number"] = k0053OrderNumber,
                ["k0004_date_time_stamp"] = k0004DateTimeStamp,
                ["k0008_operator_identifier"] = k0008OperatorIdentifier,
                ["k0010_machine_identifier"] = k0010MachineIdentifier,
                ["k0012_gage_identifier"] = k0012GageIdentifier,
                ["relationship_list"] = relationshipList,
                ["feature_check_list"] = featureCheckList,
                ["vector_group_list"] = vectorGroupList,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "PrepareQdasDataList",
            request,
            Transport.PrepareQdasDataListResult.Parser,
            cancellationToken);
    }

    public Task RenameGeneralFileAsync(
        FileReference sourceFileName,
        FileReference destinationFileName,
        bool overwrite = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RenameGeneralFileRequest(),
            new Dictionary<string, object?>
            {
                ["source_file_name"] = sourceFileName,
                ["destination_file_name"] = destinationFileName,
                ["overwrite"] = overwrite,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "RenameGeneralFile",
            request,
            Transport.RenameGeneralFileResult.Parser,
            cancellationToken);
    }

    public Task SaveAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SaveRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "Save",
            request,
            Transport.SaveResult.Parser,
            cancellationToken);
    }

    public Task SaveAsReadOnlyTemplateAsync(
        FileReference templateFileName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SaveAsReadOnlyTemplateRequest(),
            new Dictionary<string, object?>
            {
                ["template_file_name"] = templateFileName,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "SaveAsReadOnlyTemplate",
            request,
            Transport.SaveAsReadOnlyTemplateResult.Parser,
            cancellationToken);
    }

    public Task SaveAsAsync(
        FileReference fileName,
        bool addSerialNumber = false,
        int optionalNumber = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SaveAsRequest(),
            new Dictionary<string, object?>
            {
                ["file_name"] = fileName,
                ["add_serial_number"] = addSerialNumber,
                ["optional_number"] = optionalNumber,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "SaveAs",
            request,
            Transport.SaveAsResult.Parser,
            cancellationToken);
    }

    public Task SetBooleanInDataShareFileAsync(
        FileReference dataShareFilePath,
        string booleanName = "",
        bool booleanValue = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetBooleanInDataShareFileRequest(),
            new Dictionary<string, object?>
            {
                ["data_share_file_path"] = dataShareFilePath,
                ["boolean_name"] = booleanName,
                ["boolean_value"] = booleanValue,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "SetBooleanInDataShareFile",
            request,
            Transport.SetBooleanInDataShareFileResult.Parser,
            cancellationToken);
    }

    public Task SetDoubleInDataShareFileAsync(
        FileReference dataShareFilePath,
        string doubleName = "",
        double doubleValue = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetDoubleInDataShareFileRequest(),
            new Dictionary<string, object?>
            {
                ["data_share_file_path"] = dataShareFilePath,
                ["double_name"] = doubleName,
                ["double_value"] = doubleValue,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "SetDoubleInDataShareFile",
            request,
            Transport.SetDoubleInDataShareFileResult.Parser,
            cancellationToken);
    }

    public Task SetIntegerInDataShareFileAsync(
        FileReference dataShareFilePath,
        string integerName = "",
        int integerValue = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetIntegerInDataShareFileRequest(),
            new Dictionary<string, object?>
            {
                ["data_share_file_path"] = dataShareFilePath,
                ["integer_name"] = integerName,
                ["integer_value"] = integerValue,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "SetIntegerInDataShareFile",
            request,
            Transport.SetIntegerInDataShareFileResult.Parser,
            cancellationToken);
    }

    public Task SetStringInDataShareFileAsync(
        FileReference dataShareFilePath,
        string stringName = "",
        string stringValue = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetStringInDataShareFileRequest(),
            new Dictionary<string, object?>
            {
                ["data_share_file_path"] = dataShareFilePath,
                ["string_name"] = stringName,
                ["string_value"] = stringValue,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "SetStringInDataShareFile",
            request,
            Transport.SetStringInDataShareFileResult.Parser,
            cancellationToken);
    }

    public Task SetTransformInDataShareFileAsync(
        FileReference dataShareFilePath,
        string transformName,
        Transform transformValue,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetTransformInDataShareFileRequest(),
            new Dictionary<string, object?>
            {
                ["data_share_file_path"] = dataShareFilePath,
                ["transform_name"] = transformName,
                ["transform_value"] = transformValue,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "SetTransformInDataShareFile",
            request,
            Transport.SetTransformInDataShareFileResult.Parser,
            cancellationToken);
    }

    public Task SetVectorInDataShareFileAsync(
        FileReference dataShareFilePath,
        string vectorName,
        Vector vectorValue,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetVectorInDataShareFileRequest(),
            new Dictionary<string, object?>
            {
                ["data_share_file_path"] = dataShareFilePath,
                ["vector_name"] = vectorName,
                ["vector_value"] = vectorValue,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "SetVectorInDataShareFile",
            request,
            Transport.SetVectorInDataShareFileResult.Parser,
            cancellationToken);
    }

    public Task TerminateAllRunningMPsAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.TerminateAllRunningMPsRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "TerminateAllRunningMPs",
            request,
            Transport.TerminateAllRunningMPsResult.Parser,
            cancellationToken);
    }

    public Task UseNrkxmlLibraryAsync(
        bool useLibrary = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.UseNrkxmlLibraryRequest(),
            new Dictionary<string, object?>
            {
                ["use_library"] = useLibrary,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "UseNrkxmlLibrary",
            request,
            Transport.UseNrkxmlLibraryResult.Parser,
            cancellationToken);
    }

    public Task VerifyGeneralFileExistsAsync(
        FileReference fileName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.VerifyGeneralFileExistsRequest(),
            new Dictionary<string, object?>
            {
                ["file_name"] = fileName,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "VerifyGeneralFileExists",
            request,
            Transport.VerifyGeneralFileExistsResult.Parser,
            cancellationToken);
    }

    public Task VerifyMpFileExistsAsync(
        FileReference mpFileName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.VerifyMpFileExistsRequest(),
            new Dictionary<string, object?>
            {
                ["mp_file_name"] = mpFileName,
            });
        return InvokeOperationAsync(
            "briosa.FileOperations",
            "VerifyMpFileExists",
            request,
            Transport.VerifyMpFileExistsResult.Parser,
            cancellationToken);
    }

    public Task RunSubroutineAsync(
        FileReference mpSubroutineFilePath,
        bool shareParentVariables = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RunSubroutineRequest(),
            new Dictionary<string, object?>
            {
                ["mp_subroutine_file_path"] = mpSubroutineFilePath,
                ["share_parent_variables"] = shareParentVariables,
            });
        return InvokeOperationAsync(
            "briosa.MpSubroutines",
            "RunSubroutine",
            request,
            Transport.RunSubroutineResult.Parser,
            cancellationToken);
    }

    public Task AddTaskOverviewItemAsync(
        string taskName = "",
        string commentText = "",
        double effortIndex = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddTaskOverviewItemRequest(),
            new Dictionary<string, object?>
            {
                ["task_name"] = taskName,
                ["comment_text"] = commentText,
                ["effort_index"] = effortIndex,
            });
        return InvokeOperationAsync(
            "briosa.MpTaskOverview",
            "AddTaskOverviewItem",
            request,
            Transport.AddTaskOverviewItemResult.Parser,
            cancellationToken);
    }

    public Task CreateClearTaskOverviewListAsync(
        Font taskNameFont,
        Font taskCommentFont,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreateClearTaskOverviewListRequest(),
            new Dictionary<string, object?>
            {
                ["task_name_font"] = taskNameFont,
                ["task_comment_font"] = taskCommentFont,
            });
        return InvokeOperationAsync(
            "briosa.MpTaskOverview",
            "CreateClearTaskOverviewList",
            request,
            Transport.CreateClearTaskOverviewListResult.Parser,
            cancellationToken);
    }

    public Task SetCurrentTaskAsync(
        int taskIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCurrentTaskRequest(),
            new Dictionary<string, object?>
            {
                ["task_index"] = taskIndex,
            });
        return InvokeOperationAsync(
            "briosa.MpTaskOverview",
            "SetCurrentTask",
            request,
            Transport.SetCurrentTaskResult.Parser,
            cancellationToken);
    }

    public Task SetOverviewImageAsync(
        FileReference imagePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetOverviewImageRequest(),
            new Dictionary<string, object?>
            {
                ["image_path"] = imagePath,
            });
        return InvokeOperationAsync(
            "briosa.MpTaskOverview",
            "SetOverviewImage",
            request,
            Transport.SetOverviewImageResult.Parser,
            cancellationToken);
    }

    public Task SetOverviewTitleAsync(
        string overviewTitle = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetOverviewTitleRequest(),
            new Dictionary<string, object?>
            {
                ["overview_title"] = overviewTitle,
            });
        return InvokeOperationAsync(
            "briosa.MpTaskOverview",
            "SetOverviewTitle",
            request,
            Transport.SetOverviewTitleResult.Parser,
            cancellationToken);
    }

    public Task SetTaskItemCommentAsync(
        int taskIndex = 0,
        string taskComment = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetTaskItemCommentRequest(),
            new Dictionary<string, object?>
            {
                ["task_index"] = taskIndex,
                ["task_comment"] = taskComment,
            });
        return InvokeOperationAsync(
            "briosa.MpTaskOverview",
            "SetTaskItemComment",
            request,
            Transport.SetTaskItemCommentResult.Parser,
            cancellationToken);
    }

    public Task SetTaskItemCompletionValuesAsync(
        int taskIndex = 0,
        int incrementsCompleted = 0,
        int totalIncrements = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetTaskItemCompletionValuesRequest(),
            new Dictionary<string, object?>
            {
                ["task_index"] = taskIndex,
                ["increments_completed"] = incrementsCompleted,
                ["total_increments"] = totalIncrements,
            });
        return InvokeOperationAsync(
            "briosa.MpTaskOverview",
            "SetTaskItemCompletionValues",
            request,
            Transport.SetTaskItemCompletionValuesResult.Parser,
            cancellationToken);
    }

    public Task SetTaskItemNameAsync(
        int taskItemIndex = 0,
        string taskName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetTaskItemNameRequest(),
            new Dictionary<string, object?>
            {
                ["task_item_index"] = taskItemIndex,
                ["task_name"] = taskName,
            });
        return InvokeOperationAsync(
            "briosa.MpTaskOverview",
            "SetTaskItemName",
            request,
            Transport.SetTaskItemNameResult.Parser,
            cancellationToken);
    }

    public Task ShowProgressForTaskItemAsync(
        int taskIndex = 0,
        bool showProgress = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowProgressForTaskItemRequest(),
            new Dictionary<string, object?>
            {
                ["task_index"] = taskIndex,
                ["show_progress"] = showProgress,
            });
        return InvokeOperationAsync(
            "briosa.MpTaskOverview",
            "ShowProgressForTaskItem",
            request,
            Transport.ShowProgressForTaskItemResult.Parser,
            cancellationToken);
    }

    public Task ShowTaskOverviewListAsync(
        bool show = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowTaskOverviewListRequest(),
            new Dictionary<string, object?>
            {
                ["show"] = show,
            });
        return InvokeOperationAsync(
            "briosa.MpTaskOverview",
            "ShowTaskOverviewList",
            request,
            Transport.ShowTaskOverviewListResult.Parser,
            cancellationToken);
    }

    public Task<double> AskForDoubleAsync(
        string questionToAsk,
        double initialValue,
        bool enforceMinMaxValues,
        double minValue,
        double maxValue,
        Font font,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AskForDoubleRequest(),
            new Dictionary<string, object?>
            {
                ["question_to_ask"] = questionToAsk,
                ["initial_value"] = initialValue,
                ["enforce_min_max_values"] = enforceMinMaxValues,
                ["min_value"] = minValue,
                ["max_value"] = maxValue,
                ["font"] = font,
            });
        return InvokeOperationAsync<double>(
            "briosa.ProcessFlowOperations",
            "AskForDouble",
            request,
            Transport.AskForDoubleResult.Parser,
            cancellationToken);
    }

    public Task<int> AskForIntegerAsync(
        string questionToAsk,
        int initialValue,
        bool enforceMinMaxValues,
        int minValue,
        int maxValue,
        Font font,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AskForIntegerRequest(),
            new Dictionary<string, object?>
            {
                ["question_to_ask"] = questionToAsk,
                ["initial_value"] = initialValue,
                ["enforce_min_max_values"] = enforceMinMaxValues,
                ["min_value"] = minValue,
                ["max_value"] = maxValue,
                ["font"] = font,
            });
        return InvokeOperationAsync<int>(
            "briosa.ProcessFlowOperations",
            "AskForInteger",
            request,
            Transport.AskForIntegerResult.Parser,
            cancellationToken);
    }

    public Task<PointName> AskForPointNameAsync(
        string questionToAsk,
        PointName initialValue,
        Font font,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AskForPointNameRequest(),
            new Dictionary<string, object?>
            {
                ["question_to_ask"] = questionToAsk,
                ["initial_value"] = initialValue,
                ["font"] = font,
            });
        return InvokeOperationAsync<PointName>(
            "briosa.ProcessFlowOperations",
            "AskForPointName",
            request,
            Transport.AskForPointNameResult.Parser,
            cancellationToken);
    }

    public Task<string> AskForStringAsync(
        string questionToAsk,
        bool passwordEntry,
        string initialAnswer,
        Font font,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AskForStringRequest(),
            new Dictionary<string, object?>
            {
                ["question_to_ask"] = questionToAsk,
                ["password_entry"] = passwordEntry,
                ["initial_answer"] = initialAnswer,
                ["font"] = font,
            });
        return InvokeOperationAsync<string>(
            "briosa.ProcessFlowOperations",
            "AskForString",
            request,
            Transport.AskForStringResult.Parser,
            cancellationToken);
    }

    public Task<AskForStringPullDownVersionResult> AskForStringPullDownVersionAsync(
        IEnumerable<string> questionOrStatement,
        IEnumerable<string> possibleAnswers,
        Font font,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AskForStringPullDownVersionRequest(),
            new Dictionary<string, object?>
            {
                ["question_or_statement"] = questionOrStatement,
                ["possible_answers"] = possibleAnswers,
                ["font"] = font,
            });
        return InvokeOperationAsync<AskForStringPullDownVersionResult>(
            "briosa.ProcessFlowOperations",
            "AskForStringPullDownVersion",
            request,
            Transport.AskForStringPullDownVersionResult.Parser,
            cancellationToken);
    }

    public Task<string> AskForUserDecisionFromImageAsync(
        FileReference imageFile,
        FileReference imageMapXmlFile,
        string windowCaption = "",
        int windowWidth0Default = 0,
        int windowHeight0Default = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AskForUserDecisionFromImageRequest(),
            new Dictionary<string, object?>
            {
                ["image_file"] = imageFile,
                ["image_map_xml_file"] = imageMapXmlFile,
                ["window_caption"] = windowCaption,
                ["window_width_0_default"] = windowWidth0Default,
                ["window_height_0_default"] = windowHeight0Default,
            });
        return InvokeOperationAsync<string>(
            "briosa.ProcessFlowOperations",
            "AskForUserDecisionFromImage",
            request,
            Transport.AskForUserDecisionFromImageResult.Parser,
            cancellationToken);
    }

    public Task<string> AskForUserDecisionFromStringsAsync(
        IEnumerable<string> questionOrStatement,
        Font font,
        string button1TextEmptyToHideButton = "",
        string button2TextEmptyToHideButton = "",
        string button3TextEmptyToHideButton = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AskForUserDecisionFromStringsRequest(),
            new Dictionary<string, object?>
            {
                ["question_or_statement"] = questionOrStatement,
                ["font"] = font,
                ["button1_text_empty_to_hide_button"] = button1TextEmptyToHideButton,
                ["button2_text_empty_to_hide_button"] = button2TextEmptyToHideButton,
                ["button3_text_empty_to_hide_button"] = button3TextEmptyToHideButton,
            });
        return InvokeOperationAsync<string>(
            "briosa.ProcessFlowOperations",
            "AskForUserDecisionFromStrings",
            request,
            Transport.AskForUserDecisionFromStringsResult.Parser,
            cancellationToken);
    }

    public Task<bool> ObjectExistenceTestCheckOnlyAsync(
        CollectionObjectName objectName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ObjectExistenceTestCheckOnlyRequest(),
            new Dictionary<string, object?>
            {
                ["object_name"] = objectName,
            });
        return InvokeOperationAsync<bool>(
            "briosa.ProcessFlowOperations",
            "ObjectExistenceTestCheckOnly",
            request,
            Transport.ObjectExistenceTestCheckOnlyResult.Parser,
            cancellationToken);
    }

    public Task EnableDisableRelationshipsForOptimizationAsync(
        IEnumerable<CollectionItemName> relationships,
        bool enable = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EnableDisableRelationshipsForOptimizationRequest(),
            new Dictionary<string, object?>
            {
                ["relationships"] = relationships,
                ["enable"] = enable,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "EnableDisableRelationshipsForOptimization",
            request,
            Transport.EnableDisableRelationshipsForOptimizationResult.Parser,
            cancellationToken);
    }

    public Task GeomRelationshipIgnoreInputPointsAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GeomRelationshipIgnoreInputPointsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "GeomRelationshipIgnoreInputPoints",
            request,
            Transport.GeomRelationshipIgnoreInputPointsResult.Parser,
            cancellationToken);
    }

    public Task GeomRelationshipReuseIgnoredInputPointsAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GeomRelationshipReuseIgnoredInputPointsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "GeomRelationshipReuseIgnoredInputPoints",
            request,
            Transport.GeomRelationshipReuseIgnoredInputPointsResult.Parser,
            cancellationToken);
    }

    public Task<GetGeomRelationshipAutoVectorsResult> GetGeomRelationshipAutoVectorsAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGeomRelationshipAutoVectorsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<GetGeomRelationshipAutoVectorsResult>(
            "briosa.RelationshipOperations",
            "GetGeomRelationshipAutoVectors",
            request,
            Transport.GetGeomRelationshipAutoVectorsResult.Parser,
            cancellationToken);
    }

    public Task<PointName[]> GetGeomRelationshipCardinalPointsAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGeomRelationshipCardinalPointsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<PointName[]>(
            "briosa.RelationshipOperations",
            "GetGeomRelationshipCardinalPoints",
            request,
            Transport.GetGeomRelationshipCardinalPointsResult.Parser,
            cancellationToken);
    }

    public Task<GetGeomRelationshipCriteriaResult> GetGeomRelationshipCriteriaAsync(
        CollectionObjectName relationshipName,
        string criteria = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGeomRelationshipCriteriaRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["criteria"] = criteria,
            });
        return InvokeOperationAsync<GetGeomRelationshipCriteriaResult>(
            "briosa.RelationshipOperations",
            "GetGeomRelationshipCriteria",
            request,
            Transport.GetGeomRelationshipCriteriaResult.Parser,
            cancellationToken);
    }

    public Task<PointName> GetGeomRelationshipMeasuredAvgPointAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGeomRelationshipMeasuredAvgPointRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<PointName>(
            "briosa.RelationshipOperations",
            "GetGeomRelationshipMeasuredAvgPoint",
            request,
            Transport.GetGeomRelationshipMeasuredAvgPointResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName> GetGeomRelationshipMeasuredGeometryAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGeomRelationshipMeasuredGeometryRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<CollectionObjectName>(
            "briosa.RelationshipOperations",
            "GetGeomRelationshipMeasuredGeometry",
            request,
            Transport.GetGeomRelationshipMeasuredGeometryResult.Parser,
            cancellationToken);
    }

    public Task<PointName> GetGeomRelationshipNominalAvgPointAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGeomRelationshipNominalAvgPointRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<PointName>(
            "briosa.RelationshipOperations",
            "GetGeomRelationshipNominalAvgPoint",
            request,
            Transport.GetGeomRelationshipNominalAvgPointResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName> GetGeomRelationshipNominalGeometryAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGeomRelationshipNominalGeometryRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<CollectionObjectName>(
            "briosa.RelationshipOperations",
            "GetGeomRelationshipNominalGeometry",
            request,
            Transport.GetGeomRelationshipNominalGeometryResult.Parser,
            cancellationToken);
    }

    public Task<GetGeomRelationshipPointListResult> GetGeomRelationshipPointListAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGeomRelationshipPointListRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<GetGeomRelationshipPointListResult>(
            "briosa.RelationshipOperations",
            "GetGeomRelationshipPointList",
            request,
            Transport.GetGeomRelationshipPointListResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName> GetGeomRelationshipProjectionPlaneAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGeomRelationshipProjectionPlaneRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<CollectionObjectName>(
            "briosa.RelationshipOperations",
            "GetGeomRelationshipProjectionPlane",
            request,
            Transport.GetGeomRelationshipProjectionPlaneResult.Parser,
            cancellationToken);
    }

    public Task<GetPipeRelationshipCutStatusResult> GetPipeRelationshipCutStatusAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPipeRelationshipCutStatusRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<GetPipeRelationshipCutStatusResult>(
            "briosa.RelationshipOperations",
            "GetPipeRelationshipCutStatus",
            request,
            Transport.GetPipeRelationshipCutStatusResult.Parser,
            cancellationToken);
    }

    public Task<GetPipeRelationshipPropertiesResult> GetPipeRelationshipPropertiesAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPipeRelationshipPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<GetPipeRelationshipPropertiesResult>(
            "briosa.RelationshipOperations",
            "GetPipeRelationshipProperties",
            request,
            Transport.GetPipeRelationshipPropertiesResult.Parser,
            cancellationToken);
    }

    public Task<GetPipeRelationshipWeightsResult> GetPipeRelationshipWeightsAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPipeRelationshipWeightsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<GetPipeRelationshipWeightsResult>(
            "briosa.RelationshipOperations",
            "GetPipeRelationshipWeights",
            request,
            Transport.GetPipeRelationshipWeightsResult.Parser,
            cancellationToken);
    }

    public Task<GetRelationshipFitConstraintsScalarTypeResult> GetRelationshipFitConstraintsScalarTypeAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipFitConstraintsScalarTypeRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<GetRelationshipFitConstraintsScalarTypeResult>(
            "briosa.RelationshipOperations",
            "GetRelationshipFitConstraintsScalarType",
            request,
            Transport.GetRelationshipFitConstraintsScalarTypeResult.Parser,
            cancellationToken);
    }

    public Task<GetRelationshipOutlierRejectionScalarTypeResult> GetRelationshipOutlierRejectionScalarTypeAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipOutlierRejectionScalarTypeRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<GetRelationshipOutlierRejectionScalarTypeResult>(
            "briosa.RelationshipOperations",
            "GetRelationshipOutlierRejectionScalarType",
            request,
            Transport.GetRelationshipOutlierRejectionScalarTypeResult.Parser,
            cancellationToken);
    }

    public Task<GetRelationshipProjectionOptionsResult> GetRelationshipProjectionOptionsAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipProjectionOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<GetRelationshipProjectionOptionsResult>(
            "briosa.RelationshipOperations",
            "GetRelationshipProjectionOptions",
            request,
            Transport.GetRelationshipProjectionOptionsResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName> GetRelationshipReportingFrameAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipReportingFrameRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<CollectionObjectName>(
            "briosa.RelationshipOperations",
            "GetRelationshipReportingFrame",
            request,
            Transport.GetRelationshipReportingFrameResult.Parser,
            cancellationToken);
    }

    public Task<GetRelationshipSubSamplingOptionsResult> GetRelationshipSubSamplingOptionsAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipSubSamplingOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<GetRelationshipSubSamplingOptionsResult>(
            "briosa.RelationshipOperations",
            "GetRelationshipSubSamplingOptions",
            request,
            Transport.GetRelationshipSubSamplingOptionsResult.Parser,
            cancellationToken);
    }

    public Task<GetRelationshipToleranceScalarTypeResult> GetRelationshipToleranceScalarTypeAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipToleranceScalarTypeRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<GetRelationshipToleranceScalarTypeResult>(
            "briosa.RelationshipOperations",
            "GetRelationshipToleranceScalarType",
            request,
            Transport.GetRelationshipToleranceScalarTypeResult.Parser,
            cancellationToken);
    }

    public Task<GetRelationshipToleranceVectorTypeResult> GetRelationshipToleranceVectorTypeAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipToleranceVectorTypeRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<GetRelationshipToleranceVectorTypeResult>(
            "briosa.RelationshipOperations",
            "GetRelationshipToleranceVectorType",
            request,
            Transport.GetRelationshipToleranceVectorTypeResult.Parser,
            cancellationToken);
    }

    public Task<string> GetRelationshipTypeAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipTypeRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<string>(
            "briosa.RelationshipOperations",
            "GetRelationshipType",
            request,
            Transport.GetRelationshipTypeResult.Parser,
            cancellationToken);
    }

    public Task<double> GetRelationshipWeightingAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipWeightingRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<double>(
            "briosa.RelationshipOperations",
            "GetRelationshipWeighting",
            request,
            Transport.GetRelationshipWeightingResult.Parser,
            cancellationToken);
    }

    public Task MakePipeFittingRelationshipAsync(
        CollectionObjectName relationshipName,
        CollectionObjectName pipe1ObjectName,
        CollectionObjectName pipe2ObjectName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePipeFittingRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["pipe_1_object_name"] = pipe1ObjectName,
                ["pipe_2_object_name"] = pipe2ObjectName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakePipeFittingRelationship",
            request,
            Transport.MakePipeFittingRelationshipResult.Parser,
            cancellationToken);
    }

    public Task MakePipeRelationshipCutAsync(
        CollectionObjectName relationshipName,
        bool pipe1MakeCut,
        bool pipe1CreateFrame,
        CollectionObjectName pipe1FrameName,
        bool pipe2MakeCut,
        bool pipe2CreateFrame,
        CollectionObjectName pipe2FrameName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePipeRelationshipCutRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["pipe_1_make_cut"] = pipe1MakeCut,
                ["pipe_1_create_frame"] = pipe1CreateFrame,
                ["pipe_1_frame_name"] = pipe1FrameName,
                ["pipe_2_make_cut"] = pipe2MakeCut,
                ["pipe_2_create_frame"] = pipe2CreateFrame,
                ["pipe_2_frame_name"] = pipe2FrameName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakePipeRelationshipCut",
            request,
            Transport.MakePipeRelationshipCutResult.Parser,
            cancellationToken);
    }

    public Task PipeRelationshipForceCutToFrameAsync(
        CollectionObjectName relationshipName,
        bool pipe1ForceCutToFrame,
        CollectionObjectName pipe1FrameName,
        bool pipe2ForceCutToFrame,
        CollectionObjectName pipe2FrameName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.PipeRelationshipForceCutToFrameRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["pipe_1_force_cut_to_frame"] = pipe1ForceCutToFrame,
                ["pipe_1_frame_name"] = pipe1FrameName,
                ["pipe_2_force_cut_to_frame"] = pipe2ForceCutToFrame,
                ["pipe_2_frame_name"] = pipe2FrameName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "PipeRelationshipForceCutToFrame",
            request,
            Transport.PipeRelationshipForceCutToFrameResult.Parser,
            cancellationToken);
    }

    public Task SetGeomRelationshipAutoMeasureNominalFeatureAsync(
        CollectionObjectName relationshipName,
        bool trapCloudsFalseGeometry,
        CollectionInstrumentId instrumentId,
        string measurementMode = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGeomRelationshipAutoMeasureNominalFeatureRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["trap_clouds_false_geometry"] = trapCloudsFalseGeometry,
                ["instrument_id"] = instrumentId,
                ["measurement_mode"] = measurementMode,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetGeomRelationshipAutoMeasureNominalFeature",
            request,
            Transport.SetGeomRelationshipAutoMeasureNominalFeatureResult.Parser,
            cancellationToken);
    }

    public Task SetGeomRelationshipAutoVectorsNominalAvnAsync(
        CollectionObjectName relationshipName,
        bool createAutoVectorsAvn,
        PointFilterInputType pointsType,
        bool useVectorGroupCustomPrefix = false,
        string vectorGroupCustomPrefix = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGeomRelationshipAutoVectorsNominalAvnRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["create_auto_vectors_avn"] = createAutoVectorsAvn,
                ["points_type"] = pointsType,
                ["use_vector_group_custom_prefix"] = useVectorGroupCustomPrefix,
                ["vector_group_custom_prefix"] = vectorGroupCustomPrefix,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetGeomRelationshipAutoVectorsNominalAvn",
            request,
            Transport.SetGeomRelationshipAutoVectorsNominalAvnResult.Parser,
            cancellationToken);
    }

    public Task SetGeomRelationshipCardinalPointsAsync(
        CollectionObjectName relationshipName,
        bool createCardinalPtsWhenFitting = true,
        bool prefixCardinalPtsNameWithRelName = true,
        string cardinalPtsGroupName = "GR-Cardinal Pts",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGeomRelationshipCardinalPointsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["create_cardinal_pts_when_fitting"] = createCardinalPtsWhenFitting,
                ["prefix_cardinal_pts_name_with_rel_name"] = prefixCardinalPtsNameWithRelName,
                ["cardinal_pts_group_name"] = cardinalPtsGroupName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetGeomRelationshipCardinalPoints",
            request,
            Transport.SetGeomRelationshipCardinalPointsResult.Parser,
            cancellationToken);
    }

    public Task SetGeomRelationshipCriteriaAsync(
        CollectionObjectName relationshipName,
        string criteria = "",
        bool showInReport = true,
        ToleranceScalarOptions toleranceOptions = default,
        double optimizationDeltaWeight = 0.000000,
        double optimizationOutOfToleranceWeight = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGeomRelationshipCriteriaRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["criteria"] = criteria,
                ["show_in_report"] = showInReport,
                ["tolerance_options"] = toleranceOptions,
                ["optimization_delta_weight"] = optimizationDeltaWeight,
                ["optimization_out_of_tolerance_weight"] = optimizationOutOfToleranceWeight,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetGeomRelationshipCriteria",
            request,
            Transport.SetGeomRelationshipCriteriaResult.Parser,
            cancellationToken);
    }

    public Task SetGeomRelationshipMeasuredGeometryAsync(
        CollectionObjectName relationshipName,
        CollectionObjectName measuredGeometry,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGeomRelationshipMeasuredGeometryRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["measured_geometry"] = measuredGeometry,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetGeomRelationshipMeasuredGeometry",
            request,
            Transport.SetGeomRelationshipMeasuredGeometryResult.Parser,
            cancellationToken);
    }

    public Task SetGeomRelationshipNominalAvgPointAsync(
        CollectionObjectName relationshipName,
        bool compareToNominal,
        PointName nominalAveragePoint,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGeomRelationshipNominalAvgPointRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["compare_to_nominal"] = compareToNominal,
                ["nominal_average_point"] = nominalAveragePoint,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetGeomRelationshipNominalAvgPoint",
            request,
            Transport.SetGeomRelationshipNominalAvgPointResult.Parser,
            cancellationToken);
    }

    public Task SetGeomRelationshipNominalGeometryAsync(
        CollectionObjectName relationshipName,
        bool compareToNominal,
        CollectionObjectName nominalGeometry,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGeomRelationshipNominalGeometryRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["compare_to_nominal"] = compareToNominal,
                ["nominal_geometry"] = nominalGeometry,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetGeomRelationshipNominalGeometry",
            request,
            Transport.SetGeomRelationshipNominalGeometryResult.Parser,
            cancellationToken);
    }

    public Task SetGeomRelationshipProjectionPlaneAsync(
        CollectionObjectName relationshipName,
        bool projectToPlane,
        CollectionObjectName projectionPlaneName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGeomRelationshipProjectionPlaneRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["project_to_plane"] = projectToPlane,
                ["projection_plane_name"] = projectionPlaneName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetGeomRelationshipProjectionPlane",
            request,
            Transport.SetGeomRelationshipProjectionPlaneResult.Parser,
            cancellationToken);
    }

    public Task SetObjectToObjectDirectionRelationshipFitConstraintsAsync(
        CollectionObjectName relationshipName,
        FitConstraintScalarOptions angleBetweenVectorsFitConstraints = default,
        FitConstraintScalarOptions mutualPerpendicularLengthFitConstraints = default,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetObjectToObjectDirectionRelationshipFitConstraintsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["angle_between_vectors_fit_constraints"] = angleBetweenVectorsFitConstraints,
                ["mutual_perpendicular_length_fit_constraints"] = mutualPerpendicularLengthFitConstraints,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetObjectToObjectDirectionRelationshipFitConstraints",
            request,
            Transport.SetObjectToObjectDirectionRelationshipFitConstraintsResult.Parser,
            cancellationToken);
    }

    public Task SetPipeRelationshipSegmentPropertiesAsync(
        CollectionObjectName relationshipName,
        double pipe1InnerDiameter = 0.000000,
        double pipe1OuterDiameter = 0.000000,
        double pipe1CutBegin = 0.000000,
        double pipe1CutEnd = 0.000000,
        double pipe2InnerDiameter = 0.000000,
        double pipe2OuterDiameter = 0.000000,
        double pipe2CutBegin = 0.000000,
        double pipe2CutEnd = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPipeRelationshipSegmentPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["pipe_1_inner_diameter"] = pipe1InnerDiameter,
                ["pipe_1_outer_diameter"] = pipe1OuterDiameter,
                ["pipe_1_cut_begin"] = pipe1CutBegin,
                ["pipe_1_cut_end"] = pipe1CutEnd,
                ["pipe_2_inner_diameter"] = pipe2InnerDiameter,
                ["pipe_2_outer_diameter"] = pipe2OuterDiameter,
                ["pipe_2_cut_begin"] = pipe2CutBegin,
                ["pipe_2_cut_end"] = pipe2CutEnd,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetPipeRelationshipSegmentProperties",
            request,
            Transport.SetPipeRelationshipSegmentPropertiesResult.Parser,
            cancellationToken);
    }

    public Task SetPipeRelationshipWeightsAsync(
        CollectionObjectName relationshipName,
        double overallWeight = 1.000000,
        double axisOffset = 2.000000,
        double axisAlignment = 1.000000,
        double centerPull = 0.100000,
        double outOfMaterialWeight = 10.000000,
        double outOfMaterialOffset = 1.000000,
        bool constrainRegionAtOd = false,
        bool constrainIdOdOverlap = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPipeRelationshipWeightsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["overall_weight"] = overallWeight,
                ["axis_offset"] = axisOffset,
                ["axis_alignment"] = axisAlignment,
                ["center_pull"] = centerPull,
                ["out_of_material_weight"] = outOfMaterialWeight,
                ["out_of_material_offset"] = outOfMaterialOffset,
                ["constrain_region_at_od"] = constrainRegionAtOd,
                ["constrain_id_od_overlap"] = constrainIdOdOverlap,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetPipeRelationshipWeights",
            request,
            Transport.SetPipeRelationshipWeightsResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipAutoVectorsFitAvfAsync(
        CollectionObjectName relationshipName,
        bool createAutoVectorsAvf = false,
        bool useVectorGroupCustomPrefix = false,
        string vectorGroupCustomPrefix = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipAutoVectorsFitAvfRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["create_auto_vectors_avf"] = createAutoVectorsAvf,
                ["use_vector_group_custom_prefix"] = useVectorGroupCustomPrefix,
                ["vector_group_custom_prefix"] = vectorGroupCustomPrefix,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipAutoVectorsFitAvf",
            request,
            Transport.SetRelationshipAutoVectorsFitAvfResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipAutoVectorsGroupDefaultPrefixAsync(
        string geomRelAvnVgDefaultPrefix = "GR-AVN-",
        string geomRelAvfVgDefaultPrefix = "GR-AVF-",
        string nonGeomRelVgDefaultPrefix = "Auto Vectors: ",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipAutoVectorsGroupDefaultPrefixRequest(),
            new Dictionary<string, object?>
            {
                ["geom_rel_avn_vg_default_prefix"] = geomRelAvnVgDefaultPrefix,
                ["geom_rel_avf_vg_default_prefix"] = geomRelAvfVgDefaultPrefix,
                ["non_geom_rel_vg_default_prefix"] = nonGeomRelVgDefaultPrefix,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipAutoVectorsGroupDefaultPrefix",
            request,
            Transport.SetRelationshipAutoVectorsGroupDefaultPrefixResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipDesiredMeasCountAsync(
        CollectionObjectName relationshipName,
        int desiredMeasurementCount = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipDesiredMeasCountRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["desired_measurement_count"] = desiredMeasurementCount,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipDesiredMeasCount",
            request,
            Transport.SetRelationshipDesiredMeasCountResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipDormantStatusAsync(
        IEnumerable<CollectionItemName> relationships,
        bool dormantStatus = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipDormantStatusRequest(),
            new Dictionary<string, object?>
            {
                ["relationships"] = relationships,
                ["dormant_status"] = dormantStatus,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipDormantStatus",
            request,
            Transport.SetRelationshipDormantStatusResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipFitConstraintsScalarTypeAsync(
        CollectionObjectName relationshipName,
        FitConstraintScalarOptions fitConstraintOptions = default,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipFitConstraintsScalarTypeRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["fit_constraint_options"] = fitConstraintOptions,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipFitConstraintsScalarType",
            request,
            Transport.SetRelationshipFitConstraintsScalarTypeResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipOrientationFitConstraintsVectorTypeAsync(
        CollectionObjectName relationshipName,
        ToleranceVectorOptions orientationVectorConstraint,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipOrientationFitConstraintsVectorTypeRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["orientation_vector_constraint"] = orientationVectorConstraint,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipOrientationFitConstraintsVectorType",
            request,
            Transport.SetRelationshipOrientationFitConstraintsVectorTypeResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipOutlierRejectionScalarTypeAsync(
        CollectionObjectName relationshipName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipOutlierRejectionScalarTypeRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipOutlierRejectionScalarType",
            request,
            Transport.SetRelationshipOutlierRejectionScalarTypeResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipPositionFitConstraintsVectorTypeAsync(
        CollectionObjectName relationshipName,
        ToleranceVectorOptions positionVectorConstraint,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipPositionFitConstraintsVectorTypeRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["position_vector_constraint"] = positionVectorConstraint,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipPositionFitConstraintsVectorType",
            request,
            Transport.SetRelationshipPositionFitConstraintsVectorTypeResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipProjectionOptionsAsync(
        CollectionObjectName relationshipName,
        ProjectionOptions projectionOptions,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipProjectionOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["projection_options"] = projectionOptions,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipProjectionOptions",
            request,
            Transport.SetRelationshipProjectionOptionsResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipReportingFrameAsync(
        CollectionObjectName relationshipName,
        CollectionObjectName reportingFrame,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipReportingFrameRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["reporting_frame"] = reportingFrame,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipReportingFrame",
            request,
            Transport.SetRelationshipReportingFrameResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipSigmoidalGapFitConstraintsAsync(
        CollectionObjectName relationshipName,
        bool useSigmoidalGapConstraints = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipSigmoidalGapFitConstraintsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["use_sigmoidal_gap_constraints"] = useSigmoidalGapConstraints,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipSigmoidalGapFitConstraints",
            request,
            Transport.SetRelationshipSigmoidalGapFitConstraintsResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipSubSamplingOptionsAsync(
        CollectionObjectName relationshipName,
        bool useEveryIthPoint = false,
        int iValue = 20,
        bool useNoMoreThanNPoints = true,
        int nValue = 10000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipSubSamplingOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["use_every_ith_point"] = useEveryIthPoint,
                ["i_value"] = iValue,
                ["use_no_more_than_n_points"] = useNoMoreThanNPoints,
                ["n_value"] = nValue,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipSubSamplingOptions",
            request,
            Transport.SetRelationshipSubSamplingOptionsResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipToleranceScalarTypeAsync(
        CollectionObjectName relationshipName,
        ToleranceScalarOptions toleranceOptions = default,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipToleranceScalarTypeRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["tolerance_options"] = toleranceOptions,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipToleranceScalarType",
            request,
            Transport.SetRelationshipToleranceScalarTypeResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipToleranceVectorTypeAsync(
        CollectionObjectName relationshipName,
        ToleranceVectorOptions vectorTolerance,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipToleranceVectorTypeRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["vector_tolerance"] = vectorTolerance,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipToleranceVectorType",
            request,
            Transport.SetRelationshipToleranceVectorTypeResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipVoxelCloudDisplayAsync(
        CollectionObjectName relationshipName,
        bool enableVoxelCloudDisplay,
        double voxelSize10Autodetect,
        int minPtsCountPerVoxel,
        double voxelRenderingDiameter10Fast,
        SurfaceAnalysisMode surfaceAnalysisMode,
        ColorizationOptions colorizationOptions,
        bool showColorBarInView = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipVoxelCloudDisplayRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["enable_voxel_cloud_display"] = enableVoxelCloudDisplay,
                ["voxel_size_1_0_autodetect"] = voxelSize10Autodetect,
                ["min_pts_count_per_voxel"] = minPtsCountPerVoxel,
                ["voxel_rendering_diameter_1_0_fast"] = voxelRenderingDiameter10Fast,
                ["surface_analysis_mode"] = surfaceAnalysisMode,
                ["colorization_options"] = colorizationOptions,
                ["show_color_bar_in_view"] = showColorBarInView,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipVoxelCloudDisplay",
            request,
            Transport.SetRelationshipVoxelCloudDisplayResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipWeightingAsync(
        CollectionObjectName relationshipName,
        double weight = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipWeightingRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["weight"] = weight,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipWeighting",
            request,
            Transport.SetRelationshipWeightingResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipWeightsNormalizedAsync(
        CollectionName collectionName,
        RelWeightingMode pickWeightingMode,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipWeightsNormalizedRequest(),
            new Dictionary<string, object?>
            {
                ["collection_name"] = collectionName,
                ["pick_weighting_mode"] = pickWeightingMode,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipWeightsNormalized",
            request,
            Transport.SetRelationshipWeightsNormalizedResult.Parser,
            cancellationToken);
    }

    public Task AddChartsToReportBarAsync(
        IEnumerable<CollectionItemName> charts,
        bool clearExisting = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddChartsToReportBarRequest(),
            new Dictionary<string, object?>
            {
                ["charts"] = charts,
                ["clear_existing"] = clearExisting,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "AddChartsToReportBar",
            request,
            Transport.AddChartsToReportBarResult.Parser,
            cancellationToken);
    }

    public Task AddCustomTableToSaReportAsync(
        CollectionObjectName tableName,
        CollectionObjectName reportName,
        bool showReport = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddCustomTableToSaReportRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
                ["report_name"] = reportName,
                ["show_report"] = showReport,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "AddCustomTableToSaReport",
            request,
            Transport.AddCustomTableToSaReportResult.Parser,
            cancellationToken);
    }

    public Task AddCustomTablesToReportBarAsync(
        IEnumerable<CollectionItemName> customTablesToReport,
        bool clearExisting = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddCustomTablesToReportBarRequest(),
            new Dictionary<string, object?>
            {
                ["custom_tables_to_report"] = customTablesToReport,
                ["clear_existing"] = clearExisting,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "AddCustomTablesToReportBar",
            request,
            Transport.AddCustomTablesToReportBarResult.Parser,
            cancellationToken);
    }

    public Task AddDatumsToReportBarAsync(
        IEnumerable<CollectionObjectName> datums,
        bool clearExisting = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddDatumsToReportBarRequest(),
            new Dictionary<string, object?>
            {
                ["datums"] = datums,
                ["clear_existing"] = clearExisting,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "AddDatumsToReportBar",
            request,
            Transport.AddDatumsToReportBarResult.Parser,
            cancellationToken);
    }

    public Task AddEventsToReportBarAsync(
        IEnumerable<CollectionItemName> events,
        bool clearExisting = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddEventsToReportBarRequest(),
            new Dictionary<string, object?>
            {
                ["events"] = events,
                ["clear_existing"] = clearExisting,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "AddEventsToReportBar",
            request,
            Transport.AddEventsToReportBarResult.Parser,
            cancellationToken);
    }

    public Task AddFeatureChecksToReportBarAsync(
        IEnumerable<CollectionItemName> featureChecks,
        bool clearExisting = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddFeatureChecksToReportBarRequest(),
            new Dictionary<string, object?>
            {
                ["feature_checks"] = featureChecks,
                ["clear_existing"] = clearExisting,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "AddFeatureChecksToReportBar",
            request,
            Transport.AddFeatureChecksToReportBarResult.Parser,
            cancellationToken);
    }

    public Task AddItemToSaReportAtLocationAsync(
        CollectionObjectName reportName,
        CollectionObjectName itemName,
        int pageNumber = 0,
        double horizontalLocation = 1.000000,
        double verticalLocation = 1.000000,
        bool showReport = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddItemToSaReportAtLocationRequest(),
            new Dictionary<string, object?>
            {
                ["report_name"] = reportName,
                ["item_name"] = itemName,
                ["page_number"] = pageNumber,
                ["horizontal_location"] = horizontalLocation,
                ["vertical_location"] = verticalLocation,
                ["show_report"] = showReport,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "AddItemToSaReportAtLocation",
            request,
            Transport.AddItemToSaReportAtLocationResult.Parser,
            cancellationToken);
    }

    public Task AddObjectsToReportBarAsync(
        IEnumerable<CollectionObjectName> objects,
        bool clearExisting = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddObjectsToReportBarRequest(),
            new Dictionary<string, object?>
            {
                ["objects"] = objects,
                ["clear_existing"] = clearExisting,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "AddObjectsToReportBar",
            request,
            Transport.AddObjectsToReportBarResult.Parser,
            cancellationToken);
    }

    public Task AddPicturesToReportBarAsync(
        IEnumerable<CollectionItemName> pictures,
        bool clearExisting = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddPicturesToReportBarRequest(),
            new Dictionary<string, object?>
            {
                ["pictures"] = pictures,
                ["clear_existing"] = clearExisting,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "AddPicturesToReportBar",
            request,
            Transport.AddPicturesToReportBarResult.Parser,
            cancellationToken);
    }

    public Task AddRelationshipsToReportBarAsync(
        IEnumerable<CollectionItemName> relationships,
        bool clearExisting = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddRelationshipsToReportBarRequest(),
            new Dictionary<string, object?>
            {
                ["relationships"] = relationships,
                ["clear_existing"] = clearExisting,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "AddRelationshipsToReportBar",
            request,
            Transport.AddRelationshipsToReportBarResult.Parser,
            cancellationToken);
    }

    public Task AppendItemsToSaReportAsync(
        CollectionObjectName reportName,
        IEnumerable<CollectionObjectName> itemsToReport,
        bool showReport = false,
        bool beginOnNewPage = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AppendItemsToSaReportRequest(),
            new Dictionary<string, object?>
            {
                ["report_name"] = reportName,
                ["items_to_report"] = itemsToReport,
                ["show_report"] = showReport,
                ["begin_on_new_page"] = beginOnNewPage,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "AppendItemsToSaReport",
            request,
            Transport.AppendItemsToSaReportResult.Parser,
            cancellationToken);
    }

    public Task CaptureCurrentViewAsync(
        CollectionItemName pictureName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CaptureCurrentViewRequest(),
            new Dictionary<string, object?>
            {
                ["picture_name"] = pictureName,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "CaptureCurrentView",
            request,
            Transport.CaptureCurrentViewResult.Parser,
            cancellationToken);
    }

    public Task CaptureScreenToFileBmpJpgPngGifTiffAsync(
        FileReference fileToSaveTo,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CaptureScreenToFileBmpJpgPngGifTiffRequest(),
            new Dictionary<string, object?>
            {
                ["file_to_save_to"] = fileToSaveTo,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "CaptureScreenToFileBmpJpgPngGifTiff",
            request,
            Transport.CaptureScreenToFileBmpJpgPngGifTiffResult.Parser,
            cancellationToken);
    }

    public Task ClearCustomTableAsync(
        CollectionObjectName tableName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ClearCustomTableRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "ClearCustomTable",
            request,
            Transport.ClearCustomTableResult.Parser,
            cancellationToken);
    }

    public Task CloseAllReportsAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CloseAllReportsRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "CloseAllReports",
            request,
            Transport.CloseAllReportsResult.Parser,
            cancellationToken);
    }

    public Task CloseHtmlDisplayBoardAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CloseHtmlDisplayBoardRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "CloseHtmlDisplayBoard",
            request,
            Transport.CloseHtmlDisplayBoardResult.Parser,
            cancellationToken);
    }

    public Task CombineSaReportsAsync(
        IEnumerable<CollectionItemName> saReportsToCombine,
        CollectionObjectName outputSaReportName,
        bool showReport = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CombineSaReportsRequest(),
            new Dictionary<string, object?>
            {
                ["sa_reports_to_combine"] = saReportsToCombine,
                ["output_sa_report_name"] = outputSaReportName,
                ["show_report"] = showReport,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "CombineSaReports",
            request,
            Transport.CombineSaReportsResult.Parser,
            cancellationToken);
    }

    public Task CreateChartFromVectorGroupAsync(
        ChartName newChartName,
        CollectionObjectName vectorGroupName,
        ChartType chartType,
        DatasetType dataSetToChart,
        DatasetType auxDataSetToChart,
        ChartName templateChartNameOptional,
        bool showInterface = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreateChartFromVectorGroupRequest(),
            new Dictionary<string, object?>
            {
                ["new_chart_name"] = newChartName,
                ["vector_group_name"] = vectorGroupName,
                ["chart_type"] = chartType,
                ["data_set_to_chart"] = dataSetToChart,
                ["aux_data_set_to_chart"] = auxDataSetToChart,
                ["template_chart_name_optional"] = templateChartNameOptional,
                ["show_interface"] = showInterface,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "CreateChartFromVectorGroup",
            request,
            Transport.CreateChartFromVectorGroupResult.Parser,
            cancellationToken);
    }

    public Task DefineReportTemplateAsync(
        CollectionObjectName reportTemplateName,
        IEnumerable<string> title,
        ReportViewOptions graphicalViewOptions,
        IEnumerable<CollectionObjectName> itemsToReport,
        IEnumerable<CollectionItemName> relationshipsToReport,
        IEnumerable<CollectionItemName> eventsToReport,
        ReportOutputOptions reportOutputOptions,
        ReportPageSettings reportPageSettingsSaReportOnly,
        bool generateNow = false,
        bool showGeneratedReport = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DefineReportTemplateRequest(),
            new Dictionary<string, object?>
            {
                ["report_template_name"] = reportTemplateName,
                ["title"] = title,
                ["graphical_view_options"] = graphicalViewOptions,
                ["items_to_report"] = itemsToReport,
                ["relationships_to_report"] = relationshipsToReport,
                ["events_to_report"] = eventsToReport,
                ["report_output_options"] = reportOutputOptions,
                ["report_page_settings_sa_report_only"] = reportPageSettingsSaReportOnly,
                ["generate_now"] = generateNow,
                ["show_generated_report"] = showGeneratedReport,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "DefineReportTemplate",
            request,
            Transport.DefineReportTemplateResult.Parser,
            cancellationToken);
    }

    public Task DeleteChartAsync(
        CollectionObjectName chartName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteChartRequest(),
            new Dictionary<string, object?>
            {
                ["chart_name"] = chartName,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "DeleteChart",
            request,
            Transport.DeleteChartResult.Parser,
            cancellationToken);
    }

    public Task DeleteCustomTableAsync(
        CollectionObjectName tableName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteCustomTableRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "DeleteCustomTable",
            request,
            Transport.DeleteCustomTableResult.Parser,
            cancellationToken);
    }

    public Task DeletePictureAsync(
        CollectionItemName pictureName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeletePictureRequest(),
            new Dictionary<string, object?>
            {
                ["picture_name"] = pictureName,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "DeletePicture",
            request,
            Transport.DeletePictureResult.Parser,
            cancellationToken);
    }

    public Task DeleteSaDocAsync(
        CollectionObjectName docName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteSaDocRequest(),
            new Dictionary<string, object?>
            {
                ["doc_name"] = docName,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "DeleteSaDoc",
            request,
            Transport.DeleteSaDocResult.Parser,
            cancellationToken);
    }

    public Task DeleteSaReportAsync(
        CollectionObjectName reportName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteSaReportRequest(),
            new Dictionary<string, object?>
            {
                ["report_name"] = reportName,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "DeleteSaReport",
            request,
            Transport.DeleteSaReportResult.Parser,
            cancellationToken);
    }

    public Task DeleteSaReportTemplateAsync(
        CollectionObjectName reportTemplateName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteSaReportTemplateRequest(),
            new Dictionary<string, object?>
            {
                ["report_template_name"] = reportTemplateName,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "DeleteSaReportTemplate",
            request,
            Transport.DeleteSaReportTemplateResult.Parser,
            cancellationToken);
    }

    public Task GenerateQuickReportFromTabOrderAsync(
        ReportOutputOptions reportOutputOptions,
        bool openReport = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GenerateQuickReportFromTabOrderRequest(),
            new Dictionary<string, object?>
            {
                ["report_output_options"] = reportOutputOptions,
                ["open_report"] = openReport,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "GenerateQuickReportFromTabOrder",
            request,
            Transport.GenerateQuickReportFromTabOrderResult.Parser,
            cancellationToken);
    }

    public Task GenerateStandardHtmlReportAsync(
        FileReference htmlOutputFile,
        int decimalPrecision = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GenerateStandardHtmlReportRequest(),
            new Dictionary<string, object?>
            {
                ["html_output_file"] = htmlOutputFile,
                ["decimal_precision"] = decimalPrecision,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "GenerateStandardHtmlReport",
            request,
            Transport.GenerateStandardHtmlReportResult.Parser,
            cancellationToken);
    }

    public Task GenerateUpdateTemplatedReportAsync(
        CollectionObjectName reportTemplate,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GenerateUpdateTemplatedReportRequest(),
            new Dictionary<string, object?>
            {
                ["report_template"] = reportTemplate,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "GenerateUpdateTemplatedReport",
            request,
            Transport.GenerateUpdateTemplatedReportResult.Parser,
            cancellationToken);
    }

    public Task<double> GetCustomTableCellDoubleAsync(
        CollectionObjectName tableName,
        int row = 0,
        int column = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCustomTableCellDoubleRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
                ["row"] = row,
                ["column"] = column,
            });
        return InvokeOperationAsync<double>(
            "briosa.ReportingOperations",
            "GetCustomTableCellDouble",
            request,
            Transport.GetCustomTableCellDoubleResult.Parser,
            cancellationToken);
    }

    public Task<string> GetCustomTableCellStringAsync(
        CollectionObjectName tableName,
        int row = 0,
        int column = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCustomTableCellStringRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
                ["row"] = row,
                ["column"] = column,
            });
        return InvokeOperationAsync<string>(
            "briosa.ReportingOperations",
            "GetCustomTableCellString",
            request,
            Transport.GetCustomTableCellStringResult.Parser,
            cancellationToken);
    }

    public Task<string[]> GetDefinedReportTagsAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetDefinedReportTagsRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync<string[]>(
            "briosa.ReportingOperations",
            "GetDefinedReportTags",
            request,
            Transport.GetDefinedReportTagsResult.Parser,
            cancellationToken);
    }

    public Task<GetReportTagValueResult> GetReportTagValueAsync(
        string tagName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetReportTagValueRequest(),
            new Dictionary<string, object?>
            {
                ["tag_name"] = tagName,
            });
        return InvokeOperationAsync<GetReportTagValueResult>(
            "briosa.ReportingOperations",
            "GetReportTagValue",
            request,
            Transport.GetReportTagValueResult.Parser,
            cancellationToken);
    }

    public Task HtmlDisplayBoardAsync(
        FileReference inputHtmlFile,
        bool showBoard = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.HtmlDisplayBoardRequest(),
            new Dictionary<string, object?>
            {
                ["input_html_file"] = inputHtmlFile,
                ["show_board"] = showBoard,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "HtmlDisplayBoard",
            request,
            Transport.HtmlDisplayBoardResult.Parser,
            cancellationToken);
    }

    public Task MakeCustomTableAsync(
        CollectionObjectName tableName,
        int decimalPrecision = 6,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCustomTableRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
                ["decimal_precision"] = decimalPrecision,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "MakeCustomTable",
            request,
            Transport.MakeCustomTableResult.Parser,
            cancellationToken);
    }

    public Task MakeNewSaReportAsync(
        CollectionObjectName newSaReportName,
        CollectionObjectName saReportTemplateOptional,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeNewSaReportRequest(),
            new Dictionary<string, object?>
            {
                ["new_sa_report_name"] = newSaReportName,
                ["sa_report_template_optional"] = saReportTemplateOptional,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "MakeNewSaReport",
            request,
            Transport.MakeNewSaReportResult.Parser,
            cancellationToken);
    }

    public Task<bool> MakeUtilityChartAsync(
        FileReference asciiFilePath,
        string chartTitleOverride,
        CollectionItemName outputPictureName,
        bool showChartDialog = false,
        bool plotAdditionalXyValue = false,
        double xValue = 0.000000,
        double yValue = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeUtilityChartRequest(),
            new Dictionary<string, object?>
            {
                ["ascii_file_path"] = asciiFilePath,
                ["chart_title_override"] = chartTitleOverride,
                ["output_picture_name"] = outputPictureName,
                ["show_chart_dialog"] = showChartDialog,
                ["plot_additional_xy_value"] = plotAdditionalXyValue,
                ["x_value"] = xValue,
                ["y_value"] = yValue,
            });
        return InvokeOperationAsync<bool>(
            "briosa.ReportingOperations",
            "MakeUtilityChart",
            request,
            Transport.MakeUtilityChartResult.Parser,
            cancellationToken);
    }

    public Task NotifyUserDoubleAsync(
        string leadingText,
        Font font,
        double reportedValue = 0.000000,
        int decimalPrecision = 0,
        int displayTimeout = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.NotifyUserDoubleRequest(),
            new Dictionary<string, object?>
            {
                ["leading_text"] = leadingText,
                ["font"] = font,
                ["reported_value"] = reportedValue,
                ["decimal_precision"] = decimalPrecision,
                ["display_timeout"] = displayTimeout,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "NotifyUserDouble",
            request,
            Transport.NotifyUserDoubleResult.Parser,
            cancellationToken);
    }

    public Task NotifyUserHtmlAsync(
        FileReference htmlFile,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.NotifyUserHtmlRequest(),
            new Dictionary<string, object?>
            {
                ["html_file"] = htmlFile,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "NotifyUserHtml",
            request,
            Transport.NotifyUserHtmlResult.Parser,
            cancellationToken);
    }

    public Task NotifyUserIntegerAsync(
        string leadingText,
        Font font,
        int reportedValue = 0,
        int displayTimeout = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.NotifyUserIntegerRequest(),
            new Dictionary<string, object?>
            {
                ["leading_text"] = leadingText,
                ["font"] = font,
                ["reported_value"] = reportedValue,
                ["display_timeout"] = displayTimeout,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "NotifyUserInteger",
            request,
            Transport.NotifyUserIntegerResult.Parser,
            cancellationToken);
    }

    public Task NotifyUserTextArrayAsync(
        IEnumerable<string> notificationText,
        Font font,
        bool autoExpandToFitText = false,
        int displayTimeout = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.NotifyUserTextArrayRequest(),
            new Dictionary<string, object?>
            {
                ["notification_text"] = notificationText,
                ["font"] = font,
                ["auto_expand_to_fit_text"] = autoExpandToFitText,
                ["display_timeout"] = displayTimeout,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "NotifyUserTextArray",
            request,
            Transport.NotifyUserTextArrayResult.Parser,
            cancellationToken);
    }

    public Task OutputSaReportToExcelAsync(
        CollectionObjectName reportName,
        FileReference fileName,
        bool showFile = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.OutputSaReportToExcelRequest(),
            new Dictionary<string, object?>
            {
                ["report_name"] = reportName,
                ["file_name"] = fileName,
                ["show_file"] = showFile,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "OutputSaReportToExcel",
            request,
            Transport.OutputSaReportToExcelResult.Parser,
            cancellationToken);
    }

    public Task OutputSaReportToPdfAsync(
        CollectionObjectName reportName,
        FileReference fileName,
        bool showPdf = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.OutputSaReportToPdfRequest(),
            new Dictionary<string, object?>
            {
                ["report_name"] = reportName,
                ["file_name"] = fileName,
                ["show_pdf"] = showPdf,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "OutputSaReportToPdf",
            request,
            Transport.OutputSaReportToPdfResult.Parser,
            cancellationToken);
    }

    public Task QuickReportAsync(
        CollectionObjectName itemName,
        string reportNameOptional = "",
        bool openReport = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.QuickReportRequest(),
            new Dictionary<string, object?>
            {
                ["item_name"] = itemName,
                ["report_name_optional"] = reportNameOptional,
                ["open_report"] = openReport,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "QuickReport",
            request,
            Transport.QuickReportResult.Parser,
            cancellationToken);
    }

    public Task RefreshCalloutViewsInSaReportAsync(
        CollectionItemName reportName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RefreshCalloutViewsInSaReportRequest(),
            new Dictionary<string, object?>
            {
                ["report_name"] = reportName,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "RefreshCalloutViewsInSaReport",
            request,
            Transport.RefreshCalloutViewsInSaReportResult.Parser,
            cancellationToken);
    }

    public Task RefreshReportBarAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RefreshReportBarRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "RefreshReportBar",
            request,
            Transport.RefreshReportBarResult.Parser,
            cancellationToken);
    }

    public Task RemoveReportTagAsync(
        string tagName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RemoveReportTagRequest(),
            new Dictionary<string, object?>
            {
                ["tag_name"] = tagName,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "RemoveReportTag",
            request,
            Transport.RemoveReportTagResult.Parser,
            cancellationToken);
    }

    public Task RenamePictureAsync(
        CollectionItemName originalPictureName,
        CollectionItemName newPictureName,
        bool overwriteIfExists = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RenamePictureRequest(),
            new Dictionary<string, object?>
            {
                ["original_picture_name"] = originalPictureName,
                ["new_picture_name"] = newPictureName,
                ["overwrite_if_exists"] = overwriteIfExists,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "RenamePicture",
            request,
            Transport.RenamePictureResult.Parser,
            cancellationToken);
    }

    public Task SaveChartToJPegFileAsync(
        ChartName chartToSave,
        FileReference fileToSaveTo,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SaveChartToJPegFileRequest(),
            new Dictionary<string, object?>
            {
                ["chart_to_save"] = chartToSave,
                ["file_to_save_to"] = fileToSaveTo,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SaveChartToJPegFile",
            request,
            Transport.SaveChartToJPegFileResult.Parser,
            cancellationToken);
    }

    public Task SaveCurrentViewBmpJpgPngGifTiffAsync(
        FileReference fileToSaveTo,
        double renderScaleFactor10UsesWindowSize = 1.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SaveCurrentViewBmpJpgPngGifTiffRequest(),
            new Dictionary<string, object?>
            {
                ["file_to_save_to"] = fileToSaveTo,
                ["render_scale_factor_1_0_uses_window_size"] = renderScaleFactor10UsesWindowSize,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SaveCurrentViewBmpJpgPngGifTiff",
            request,
            Transport.SaveCurrentViewBmpJpgPngGifTiffResult.Parser,
            cancellationToken);
    }

    public Task SetCustomTableCellColorAsync(
        CollectionObjectName tableName,
        int row,
        int column,
        Color foregroundColorName,
        Color backgroundColorName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCustomTableCellColorRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
                ["row"] = row,
                ["column"] = column,
                ["foreground_color_name"] = foregroundColorName,
                ["background_color_name"] = backgroundColorName,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetCustomTableCellColor",
            request,
            Transport.SetCustomTableCellColorResult.Parser,
            cancellationToken);
    }

    public Task SetCustomTableCellDoubleAsync(
        CollectionObjectName tableName,
        int row = 0,
        int column = 0,
        double value = 0.000000,
        int span = 1,
        int decimalPrecision = -1,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCustomTableCellDoubleRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
                ["row"] = row,
                ["column"] = column,
                ["value"] = value,
                ["span"] = span,
                ["decimal_precision"] = decimalPrecision,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetCustomTableCellDouble",
            request,
            Transport.SetCustomTableCellDoubleResult.Parser,
            cancellationToken);
    }

    public Task SetCustomTableCellFontAsync(
        CollectionObjectName tableName,
        int row,
        int column,
        Font font,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCustomTableCellFontRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
                ["row"] = row,
                ["column"] = column,
                ["font"] = font,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetCustomTableCellFont",
            request,
            Transport.SetCustomTableCellFontResult.Parser,
            cancellationToken);
    }

    public Task SetCustomTableCellStringAsync(
        CollectionObjectName tableName,
        int row = 0,
        int column = 0,
        string value = "",
        int span = 1,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCustomTableCellStringRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
                ["row"] = row,
                ["column"] = column,
                ["value"] = value,
                ["span"] = span,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetCustomTableCellString",
            request,
            Transport.SetCustomTableCellStringResult.Parser,
            cancellationToken);
    }

    public Task SetCustomTableHeaderCellAsync(
        CollectionObjectName tableName,
        int row = 0,
        int column = 0,
        string headerText = "",
        int span = 1,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCustomTableHeaderCellRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
                ["row"] = row,
                ["column"] = column,
                ["header_text"] = headerText,
                ["span"] = span,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetCustomTableHeaderCell",
            request,
            Transport.SetCustomTableHeaderCellResult.Parser,
            cancellationToken);
    }

    public Task SetCustomTableHeaderRowAsync(
        CollectionObjectName tableName,
        int row = 0,
        string value = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCustomTableHeaderRowRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
                ["row"] = row,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetCustomTableHeaderRow",
            request,
            Transport.SetCustomTableHeaderRowResult.Parser,
            cancellationToken);
    }

    public Task SetCustomTableTitleAsync(
        CollectionObjectName tableName,
        string titleLine1 = "",
        string titleLine2 = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCustomTableTitleRequest(),
            new Dictionary<string, object?>
            {
                ["table_name"] = tableName,
                ["title_line_1"] = titleLine1,
                ["title_line_2"] = titleLine2,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetCustomTableTitle",
            request,
            Transport.SetCustomTableTitleResult.Parser,
            cancellationToken);
    }

    public Task SetPointGroupReportOptionsAsync(
        CollectionObjectName pointGroup,
        CoordinateSystemType coordinateSystem,
        bool showXComponent = true,
        bool showYComponent = true,
        bool showZComponent = true,
        bool showOffsets = false,
        bool showUncertainty = true,
        bool showNotes = false,
        bool showMeasurements = false,
        bool showMeasurementDetails = false,
        bool showPointingErrorWorstAngle = false,
        bool sortByPointNames = true,
        bool makeDefault = false,
        bool applyToAll = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPointGroupReportOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["point_group"] = pointGroup,
                ["coordinate_system"] = coordinateSystem,
                ["show_x_component"] = showXComponent,
                ["show_y_component"] = showYComponent,
                ["show_z_component"] = showZComponent,
                ["show_offsets"] = showOffsets,
                ["show_uncertainty"] = showUncertainty,
                ["show_notes"] = showNotes,
                ["show_measurements"] = showMeasurements,
                ["show_measurement_details"] = showMeasurementDetails,
                ["show_pointing_error_worst_angle"] = showPointingErrorWorstAngle,
                ["sort_by_point_names"] = sortByPointNames,
                ["make_default"] = makeDefault,
                ["apply_to_all"] = applyToAll,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetPointGroupReportOptions",
            request,
            Transport.SetPointGroupReportOptionsResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipReportOptionsAsync(
        CollectionObjectName relationshipName,
        PointDeltaReportOptions reportOptions,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipReportOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["report_options"] = reportOptions,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetRelationshipReportOptions",
            request,
            Transport.SetRelationshipReportOptionsResult.Parser,
            cancellationToken);
    }

    public Task SetReportBarVisibilityAsync(
        bool showReportBar = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetReportBarVisibilityRequest(),
            new Dictionary<string, object?>
            {
                ["show_report_bar"] = showReportBar,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetReportBarVisibility",
            request,
            Transport.SetReportBarVisibilityResult.Parser,
            cancellationToken);
    }

    public Task SetReportOptionsForObjectAsync(
        CollectionObjectName @object,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetReportOptionsForObjectRequest(),
            new Dictionary<string, object?>
            {
                ["object"] = @object,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetReportOptionsForObject",
            request,
            Transport.SetReportOptionsForObjectResult.Parser,
            cancellationToken);
    }

    public Task SetReportTagValueFromDoubleAsync(
        string tagName = "",
        double tagValue = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetReportTagValueFromDoubleRequest(),
            new Dictionary<string, object?>
            {
                ["tag_name"] = tagName,
                ["tag_value"] = tagValue,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetReportTagValueFromDouble",
            request,
            Transport.SetReportTagValueFromDoubleResult.Parser,
            cancellationToken);
    }

    public Task SetReportTagValueFromIntegerAsync(
        string tagName = "",
        int tagValue = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetReportTagValueFromIntegerRequest(),
            new Dictionary<string, object?>
            {
                ["tag_name"] = tagName,
                ["tag_value"] = tagValue,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetReportTagValueFromInteger",
            request,
            Transport.SetReportTagValueFromIntegerResult.Parser,
            cancellationToken);
    }

    public Task SetReportTagValueFromStringAsync(
        string tagName = "",
        string tagValue = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetReportTagValueFromStringRequest(),
            new Dictionary<string, object?>
            {
                ["tag_name"] = tagName,
                ["tag_value"] = tagValue,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetReportTagValueFromString",
            request,
            Transport.SetReportTagValueFromStringResult.Parser,
            cancellationToken);
    }

    public Task SetScaleForPictureAsync(
        CollectionItemName pictureName,
        double scale = 100.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetScaleForPictureRequest(),
            new Dictionary<string, object?>
            {
                ["picture_name"] = pictureName,
                ["scale"] = scale,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetScaleForPicture",
            request,
            Transport.SetScaleForPictureResult.Parser,
            cancellationToken);
    }

    public Task SetVectorGroupReportOptionsAsync(
        CollectionObjectName vectorGroup,
        PointDeltaReportOptions reportOptions,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetVectorGroupReportOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group"] = vectorGroup,
                ["report_options"] = reportOptions,
            });
        return InvokeOperationAsync(
            "briosa.ReportingOperations",
            "SetVectorGroupReportOptions",
            request,
            Transport.SetVectorGroupReportOptionsResult.Parser,
            cancellationToken);
    }

    public Task DeleteScaleBarAsync(
        CollectionObjectName scaleBarName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteScaleBarRequest(),
            new Dictionary<string, object?>
            {
                ["scale_bar_name"] = scaleBarName,
            });
        return InvokeOperationAsync(
            "briosa.ScaleBarOperations",
            "DeleteScaleBar",
            request,
            Transport.DeleteScaleBarResult.Parser,
            cancellationToken);
    }

    public Task<GetScaleBarStatsResult> GetScaleBarStatsAsync(
        CollectionObjectName scaleBarName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetScaleBarStatsRequest(),
            new Dictionary<string, object?>
            {
                ["scale_bar_name"] = scaleBarName,
            });
        return InvokeOperationAsync<GetScaleBarStatsResult>(
            "briosa.ScaleBarOperations",
            "GetScaleBarStats",
            request,
            Transport.GetScaleBarStatsResult.Parser,
            cancellationToken);
    }

    public Task<double> ScaleBarCheckAsync(
        PointName scaleBarPointA,
        PointName scaleBarPointB,
        double currentTemperatureF = 0.000000,
        double lengthOfBarAt68F = 0.000000,
        double materialCtePpmF = 0.000000,
        double tolerance = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ScaleBarCheckRequest(),
            new Dictionary<string, object?>
            {
                ["scale_bar_point_a"] = scaleBarPointA,
                ["scale_bar_point_b"] = scaleBarPointB,
                ["current_temperature_f"] = currentTemperatureF,
                ["length_of_bar_at_68f"] = lengthOfBarAt68F,
                ["material_cte_ppm_f"] = materialCtePpmF,
                ["tolerance"] = tolerance,
            });
        return InvokeOperationAsync<double>(
            "briosa.ScaleBarOperations",
            "ScaleBarCheck",
            request,
            Transport.ScaleBarCheckResult.Parser,
            cancellationToken);
    }

    public Task SetInwardPositiveNormalAsync(
        CollectionObjectName objectName,
        bool inwardPositive = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetInwardPositiveNormalRequest(),
            new Dictionary<string, object?>
            {
                ["object_name"] = objectName,
                ["inward_positive"] = inwardPositive,
            });
        return InvokeOperationAsync(
            "briosa.ScaleBarOperations",
            "SetInwardPositiveNormal",
            request,
            Transport.SetInwardPositiveNormalResult.Parser,
            cancellationToken);
    }

    public Task CloseAllWatchWindowsAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CloseAllWatchWindowsRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "CloseAllWatchWindows",
            request,
            Transport.CloseAllWatchWindowsResult.Parser,
            cancellationToken);
    }

    public Task DeleteFolderAsync(
        string folderPath = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteFolderRequest(),
            new Dictionary<string, object?>
            {
                ["folder_path"] = folderPath,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "DeleteFolder",
            request,
            Transport.DeleteFolderResult.Parser,
            cancellationToken);
    }

    public Task DeleteItemsAsync(
        IEnumerable<CollectionItemName> itemList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteItemsRequest(),
            new Dictionary<string, object?>
            {
                ["item_list"] = itemList,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "DeleteItems",
            request,
            Transport.DeleteItemsResult.Parser,
            cancellationToken);
    }

    public Task DeleteObjectsAsync(
        IEnumerable<CollectionObjectName> objectNames,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["object_names"] = objectNames,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "DeleteObjects",
            request,
            Transport.DeleteObjectsResult.Parser,
            cancellationToken);
    }

    public Task<GetActiveLanguageResult> GetActiveLanguageAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetActiveLanguageRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync<GetActiveLanguageResult>(
            "briosa.UtilityOperations",
            "GetActiveLanguage",
            request,
            Transport.GetActiveLanguageResult.Parser,
            cancellationToken);
    }

    public Task<ActiveUnits> GetActiveUnitsAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetActiveUnitsRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync<ActiveUnits>(
            "briosa.UtilityOperations",
            "GetActiveUnits",
            request,
            Transport.GetActiveUnitsResult.Parser,
            cancellationToken);
    }

    public Task<bool> GetAngularRepresentationAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetAngularRepresentationRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync<bool>(
            "briosa.UtilityOperations",
            "GetAngularRepresentation",
            request,
            Transport.GetAngularRepresentationResult.Parser,
            cancellationToken);
    }

    public Task<string[]> GetCollectionNotesAsync(
        CollectionName collection,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCollectionNotesRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
            });
        return InvokeOperationAsync<string[]>(
            "briosa.UtilityOperations",
            "GetCollectionNotes",
            request,
            Transport.GetCollectionNotesResult.Parser,
            cancellationToken);
    }

    public Task<string[]> GetFolderCollectionsAsync(
        string folderPath = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetFolderCollectionsRequest(),
            new Dictionary<string, object?>
            {
                ["folder_path"] = folderPath,
            });
        return InvokeOperationAsync<string[]>(
            "briosa.UtilityOperations",
            "GetFolderCollections",
            request,
            Transport.GetFolderCollectionsResult.Parser,
            cancellationToken);
    }

    public Task<string[]> GetFolderNotesAsync(
        string folderPath = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetFolderNotesRequest(),
            new Dictionary<string, object?>
            {
                ["folder_path"] = folderPath,
            });
        return InvokeOperationAsync<string[]>(
            "briosa.UtilityOperations",
            "GetFolderNotes",
            request,
            Transport.GetFolderNotesResult.Parser,
            cancellationToken);
    }

    public Task<string[]> GetFoldersByWildcardAsync(
        string searchString = "",
        bool caseSensitiveSearch = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetFoldersByWildcardRequest(),
            new Dictionary<string, object?>
            {
                ["search_string"] = searchString,
                ["case_sensitive_search"] = caseSensitiveSearch,
            });
        return InvokeOperationAsync<string[]>(
            "briosa.UtilityOperations",
            "GetFoldersByWildcard",
            request,
            Transport.GetFoldersByWildcardResult.Parser,
            cancellationToken);
    }

    public Task<string[]> GetObjectNotesAsync(
        CollectionObjectName @object,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetObjectNotesRequest(),
            new Dictionary<string, object?>
            {
                ["object"] = @object,
            });
        return InvokeOperationAsync<string[]>(
            "briosa.UtilityOperations",
            "GetObjectNotes",
            request,
            Transport.GetObjectNotesResult.Parser,
            cancellationToken);
    }

    public Task<double> GetOpcDaTagValueDoubleAsync(
        string opcServerDaTagName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetOpcDaTagValueDoubleRequest(),
            new Dictionary<string, object?>
            {
                ["opc_server_da_tag_name"] = opcServerDaTagName,
            });
        return InvokeOperationAsync<double>(
            "briosa.UtilityOperations",
            "GetOpcDaTagValueDouble",
            request,
            Transport.GetOpcDaTagValueDoubleResult.Parser,
            cancellationToken);
    }

    public Task<int> GetOpcDaTagValueIntegerAsync(
        string opcServerDaTagName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetOpcDaTagValueIntegerRequest(),
            new Dictionary<string, object?>
            {
                ["opc_server_da_tag_name"] = opcServerDaTagName,
            });
        return InvokeOperationAsync<int>(
            "briosa.UtilityOperations",
            "GetOpcDaTagValueInteger",
            request,
            Transport.GetOpcDaTagValueIntegerResult.Parser,
            cancellationToken);
    }

    public Task<string> GetOpcDaTagValueStringAsync(
        string opcServerDaTagName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetOpcDaTagValueStringRequest(),
            new Dictionary<string, object?>
            {
                ["opc_server_da_tag_name"] = opcServerDaTagName,
            });
        return InvokeOperationAsync<string>(
            "briosa.UtilityOperations",
            "GetOpcDaTagValueString",
            request,
            Transport.GetOpcDaTagValueStringResult.Parser,
            cancellationToken);
    }

    public Task<string[]> GetPointNotesAsync(
        PointName point,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointNotesRequest(),
            new Dictionary<string, object?>
            {
                ["point"] = point,
            });
        return InvokeOperationAsync<string[]>(
            "briosa.UtilityOperations",
            "GetPointNotes",
            request,
            Transport.GetPointNotesResult.Parser,
            cancellationToken);
    }

    public Task<GetScreenResolutionResult> GetScreenResolutionAsync(
        int display1Primary = -1,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetScreenResolutionRequest(),
            new Dictionary<string, object?>
            {
                ["display_1_primary"] = display1Primary,
            });
        return InvokeOperationAsync<GetScreenResolutionResult>(
            "briosa.UtilityOperations",
            "GetScreenResolution",
            request,
            Transport.GetScreenResolutionResult.Parser,
            cancellationToken);
    }

    public Task<WorkingFrameProperties> GetWorkingFramePropertiesAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetWorkingFramePropertiesRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync<WorkingFrameProperties>(
            "briosa.UtilityOperations",
            "GetWorkingFrameProperties",
            request,
            Transport.GetWorkingFramePropertiesResult.Parser,
            cancellationToken);
    }

    public Task<PointName> IncrementPointNameAsync(
        PointName basePointName,
        int increment = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.IncrementPointNameRequest(),
            new Dictionary<string, object?>
            {
                ["base_point_name"] = basePointName,
                ["increment"] = increment,
            });
        return InvokeOperationAsync<PointName>(
            "briosa.UtilityOperations",
            "IncrementPointName",
            request,
            Transport.IncrementPointNameResult.Parser,
            cancellationToken);
    }

    public Task LockImportedItemsAsync(
        bool lockItems = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LockImportedItemsRequest(),
            new Dictionary<string, object?>
            {
                ["lock_items"] = lockItems,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "LockImportedItems",
            request,
            Transport.LockImportedItemsResult.Parser,
            cancellationToken);
    }

    public Task LockUnlockSelectedItemsAsync(
        IEnumerable<CollectionItemName> itemList,
        IEnumerable<CollectionInstrumentId> instruments,
        bool lockItems = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LockUnlockSelectedItemsRequest(),
            new Dictionary<string, object?>
            {
                ["item_list"] = itemList,
                ["instruments"] = instruments,
                ["lock_items"] = lockItems,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "LockUnlockSelectedItems",
            request,
            Transport.LockUnlockSelectedItemsResult.Parser,
            cancellationToken);
    }

    public Task LockUnlockTrappingControlAsync(
        IEnumerable<CollectionItemName> relationshipRefList,
        IEnumerable<CollectionItemName> featureCheckRefList,
        IEnumerable<CollectionObjectName> datumRefList,
        bool lockOutTrapping = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LockUnlockTrappingControlRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_ref_list"] = relationshipRefList,
                ["feature_check_ref_list"] = featureCheckRefList,
                ["datum_ref_list"] = datumRefList,
                ["lock_out_trapping"] = lockOutTrapping,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "LockUnlockTrappingControl",
            request,
            Transport.LockUnlockTrappingControlResult.Parser,
            cancellationToken);
    }

    public Task MoveCollectionToFolderAsync(
        CollectionName collection,
        string folderPath = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveCollectionToFolderRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
                ["folder_path"] = folderPath,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "MoveCollectionToFolder",
            request,
            Transport.MoveCollectionToFolderResult.Parser,
            cancellationToken);
    }

    public Task MoveFolderToFolderAsync(
        string sourceFolderPath = "",
        string destinationFolderPath = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveFolderToFolderRequest(),
            new Dictionary<string, object?>
            {
                ["source_folder_path"] = sourceFolderPath,
                ["destination_folder_path"] = destinationFolderPath,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "MoveFolderToFolder",
            request,
            Transport.MoveFolderToFolderResult.Parser,
            cancellationToken);
    }

    public Task MoveInstrumentsDragGraphicallyAsync(
        IEnumerable<CollectionInstrumentId> instruments,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveInstrumentsDragGraphicallyRequest(),
            new Dictionary<string, object?>
            {
                ["instruments"] = instruments,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "MoveInstrumentsDragGraphically",
            request,
            Transport.MoveInstrumentsDragGraphicallyResult.Parser,
            cancellationToken);
    }

    public Task MoveObjectsDragGraphicallyAsync(
        IEnumerable<CollectionObjectName> objects,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveObjectsDragGraphicallyRequest(),
            new Dictionary<string, object?>
            {
                ["objects"] = objects,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "MoveObjectsDragGraphically",
            request,
            Transport.MoveObjectsDragGraphicallyResult.Parser,
            cancellationToken);
    }

    public Task ScaleObjectsAsync(
        IEnumerable<CollectionObjectName> objects,
        double scaleFactor = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ScaleObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["objects"] = objects,
                ["scale_factor"] = scaleFactor,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "ScaleObjects",
            request,
            Transport.ScaleObjectsResult.Parser,
            cancellationToken);
    }

    public Task SetActiveCustomLanguageAsync(
        FileReference languageFileName,
        Font font,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetActiveCustomLanguageRequest(),
            new Dictionary<string, object?>
            {
                ["language_file_name"] = languageFileName,
                ["font"] = font,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetActiveCustomLanguage",
            request,
            Transport.SetActiveCustomLanguageResult.Parser,
            cancellationToken);
    }

    public Task SetActiveUnitsAsync(
        DistanceUnits length,
        bool displayInchFractions,
        double inchFractionDenominator,
        bool simplifyInchFraction,
        TemperatureUnits temperature,
        AngularUnits angular,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetActiveUnitsRequest(),
            new Dictionary<string, object?>
            {
                ["length"] = length,
                ["display_inch_fractions"] = displayInchFractions,
                ["inch_fraction_denominator"] = inchFractionDenominator,
                ["simplify_inch_fraction"] = simplifyInchFraction,
                ["temperature"] = temperature,
                ["angular"] = angular,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetActiveUnits",
            request,
            Transport.SetActiveUnitsResult.Parser,
            cancellationToken);
    }

    public Task SetAngularRepresentationAsync(
        bool value0360False180 = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetAngularRepresentationRequest(),
            new Dictionary<string, object?>
            {
                ["value_0_360_false_180"] = value0360False180,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetAngularRepresentation",
            request,
            Transport.SetAngularRepresentationResult.Parser,
            cancellationToken);
    }

    public Task SetAutoEventCreationAsync(
        bool active = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetAutoEventCreationRequest(),
            new Dictionary<string, object?>
            {
                ["active"] = active,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetAutoEventCreation",
            request,
            Transport.SetAutoEventCreationResult.Parser,
            cancellationToken);
    }

    public Task SetAutomaticBackupStateAsync(
        bool autoJobFileRestorePointsActive = true,
        bool autoMeasurementsBackupActive = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetAutomaticBackupStateRequest(),
            new Dictionary<string, object?>
            {
                ["auto_job_file_restore_points_active"] = autoJobFileRestorePointsActive,
                ["auto_measurements_backup_active"] = autoMeasurementsBackupActive,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetAutomaticBackupState",
            request,
            Transport.SetAutomaticBackupStateResult.Parser,
            cancellationToken);
    }

    public Task SetAutomaticRelationshipConstructionStateAsync(
        bool active = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetAutomaticRelationshipConstructionStateRequest(),
            new Dictionary<string, object?>
            {
                ["active"] = active,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetAutomaticRelationshipConstructionState",
            request,
            Transport.SetAutomaticRelationshipConstructionStateResult.Parser,
            cancellationToken);
    }

    public Task SetCollectionNotesAsync(
        CollectionName collection,
        IEnumerable<string> notes,
        bool appendFalseOverwrite = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCollectionNotesRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
                ["notes"] = notes,
                ["append_false_overwrite"] = appendFalseOverwrite,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetCollectionNotes",
            request,
            Transport.SetCollectionNotesResult.Parser,
            cancellationToken);
    }

    public Task SetDecimalDigitsForDisplayAsync(
        int length = 4,
        int angle = 4,
        int scale = 6,
        int unitVector = 6,
        int weight = 3,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetDecimalDigitsForDisplayRequest(),
            new Dictionary<string, object?>
            {
                ["length"] = length,
                ["angle"] = angle,
                ["scale"] = scale,
                ["unit_vector"] = unitVector,
                ["weight"] = weight,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetDecimalDigitsForDisplay",
            request,
            Transport.SetDecimalDigitsForDisplayResult.Parser,
            cancellationToken);
    }

    public Task SetFolderNotesAsync(
        string folderPath,
        IEnumerable<string> notes,
        bool appendFalseOverwrite = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetFolderNotesRequest(),
            new Dictionary<string, object?>
            {
                ["folder_path"] = folderPath,
                ["notes"] = notes,
                ["append_false_overwrite"] = appendFalseOverwrite,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetFolderNotes",
            request,
            Transport.SetFolderNotesResult.Parser,
            cancellationToken);
    }

    public Task SetInteractionModeAsync(
        SaInteractionMode saInteractionMode,
        MpInteractionMode measurementPlanInteractionMode,
        MpDialogInteractionMode measurementPlanDialogInteractionMode,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetInteractionModeRequest(),
            new Dictionary<string, object?>
            {
                ["sa_interaction_mode"] = saInteractionMode,
                ["measurement_plan_interaction_mode"] = measurementPlanInteractionMode,
                ["measurement_plan_dialog_interaction_mode"] = measurementPlanDialogInteractionMode,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetInteractionMode",
            request,
            Transport.SetInteractionModeResult.Parser,
            cancellationToken);
    }

    public Task SetLoggingStateAsync(
        bool active = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetLoggingStateRequest(),
            new Dictionary<string, object?>
            {
                ["active"] = active,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetLoggingState",
            request,
            Transport.SetLoggingStateResult.Parser,
            cancellationToken);
    }

    public Task SetNotificationCancelOverrideAsync(
        bool prohibitCancel = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetNotificationCancelOverrideRequest(),
            new Dictionary<string, object?>
            {
                ["prohibit_cancel"] = prohibitCancel,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetNotificationCancelOverride",
            request,
            Transport.SetNotificationCancelOverrideResult.Parser,
            cancellationToken);
    }

    public Task SetObjectNotesAsync(
        CollectionObjectName @object,
        IEnumerable<string> notes,
        bool appendFalseOverwrite = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetObjectNotesRequest(),
            new Dictionary<string, object?>
            {
                ["object"] = @object,
                ["notes"] = notes,
                ["append_false_overwrite"] = appendFalseOverwrite,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetObjectNotes",
            request,
            Transport.SetObjectNotesResult.Parser,
            cancellationToken);
    }

    public Task SetOpcDaTagValueDoubleAsync(
        string opcServerDaTagName = "",
        double value = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetOpcDaTagValueDoubleRequest(),
            new Dictionary<string, object?>
            {
                ["opc_server_da_tag_name"] = opcServerDaTagName,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetOpcDaTagValueDouble",
            request,
            Transport.SetOpcDaTagValueDoubleResult.Parser,
            cancellationToken);
    }

    public Task SetOpcDaTagValueIntegerAsync(
        string opcServerDaTagName = "",
        int value = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetOpcDaTagValueIntegerRequest(),
            new Dictionary<string, object?>
            {
                ["opc_server_da_tag_name"] = opcServerDaTagName,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetOpcDaTagValueInteger",
            request,
            Transport.SetOpcDaTagValueIntegerResult.Parser,
            cancellationToken);
    }

    public Task SetOpcDaTagValueStringAsync(
        string opcServerDaTagName = "",
        string value = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetOpcDaTagValueStringRequest(),
            new Dictionary<string, object?>
            {
                ["opc_server_da_tag_name"] = opcServerDaTagName,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetOpcDaTagValueString",
            request,
            Transport.SetOpcDaTagValueStringResult.Parser,
            cancellationToken);
    }

    public Task SetPointNotesAsync(
        PointName point,
        IEnumerable<string> notes,
        bool appendFalseOverwrite = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPointNotesRequest(),
            new Dictionary<string, object?>
            {
                ["point"] = point,
                ["notes"] = notes,
                ["append_false_overwrite"] = appendFalseOverwrite,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetPointNotes",
            request,
            Transport.SetPointNotesResult.Parser,
            cancellationToken);
    }

    public Task SetUserInterfaceProfileAsync(
        string profileName,
        FileReference profileFileNameOptional,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetUserInterfaceProfileRequest(),
            new Dictionary<string, object?>
            {
                ["profile_name"] = profileName,
                ["profile_file_name_optional"] = profileFileNameOptional,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetUserInterfaceProfile",
            request,
            Transport.SetUserInterfaceProfileResult.Parser,
            cancellationToken);
    }

    public Task SetViewIdleUpdateFrequencyAsync(
        int idleCount = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetViewIdleUpdateFrequencyRequest(),
            new Dictionary<string, object?>
            {
                ["idle_count"] = idleCount,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetViewIdleUpdateFrequency",
            request,
            Transport.SetViewIdleUpdateFrequencyResult.Parser,
            cancellationToken);
    }

    public Task SetWildCardAsteriskModeAsync(
        bool autoWrapSearchString = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetWildCardAsteriskModeRequest(),
            new Dictionary<string, object?>
            {
                ["auto_wrap_search_string"] = autoWrapSearchString,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetWildCardAsteriskMode",
            request,
            Transport.SetWildCardAsteriskModeResult.Parser,
            cancellationToken);
    }

    public Task SetWorkingFrameAsync(
        CollectionObjectName newWorkingFrameName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetWorkingFrameRequest(),
            new Dictionary<string, object?>
            {
                ["new_working_frame_name"] = newWorkingFrameName,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "SetWorkingFrame",
            request,
            Transport.SetWorkingFrameResult.Parser,
            cancellationToken);
    }

    public Task StatusDialogAsync(
        string dialogTitle = "",
        string textMessage = "",
        int currentPosition = 0,
        int upperLimit = 0,
        bool suppressTimeRemaining = true,
        bool closeDialog = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StatusDialogRequest(),
            new Dictionary<string, object?>
            {
                ["dialog_title"] = dialogTitle,
                ["text_message"] = textMessage,
                ["current_position"] = currentPosition,
                ["upper_limit"] = upperLimit,
                ["suppress_time_remaining"] = suppressTimeRemaining,
                ["close_dialog"] = closeDialog,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "StatusDialog",
            request,
            Transport.StatusDialogResult.Parser,
            cancellationToken);
    }

    public Task TrimLogFileAsync(
        int numberOfEntriesToKeep = 10,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.TrimLogFileRequest(),
            new Dictionary<string, object?>
            {
                ["number_of_entries_to_keep"] = numberOfEntriesToKeep,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "TrimLogFile",
            request,
            Transport.TrimLogFileResult.Parser,
            cancellationToken);
    }

    public Task WriteToLogAsync(
        string logEntry = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.WriteToLogRequest(),
            new Dictionary<string, object?>
            {
                ["log_entry"] = logEntry,
            });
        return InvokeOperationAsync(
            "briosa.UtilityOperations",
            "WriteToLog",
            request,
            Transport.WriteToLogResult.Parser,
            cancellationToken);
    }

    public Task AddDoubleToNamedDoubleListVariableAsync(
        string name = "",
        double doubleValue = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddDoubleToNamedDoubleListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["double_value"] = doubleValue,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "AddDoubleToNamedDoubleListVariable",
            request,
            Transport.AddDoubleToNamedDoubleListVariableResult.Parser,
            cancellationToken);
    }

    public Task ClearNamedDoubleListVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ClearNamedDoubleListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "ClearNamedDoubleListVariable",
            request,
            Transport.ClearNamedDoubleListVariableResult.Parser,
            cancellationToken);
    }

    public Task DeleteVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "DeleteVariable",
            request,
            Transport.DeleteVariableResult.Parser,
            cancellationToken);
    }

    public Task DeleteVariablesWildcardMatchAsync(
        string variableWildcardCriteria = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteVariablesWildcardMatchRequest(),
            new Dictionary<string, object?>
            {
                ["variable_wildcard_criteria"] = variableWildcardCriteria,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "DeleteVariablesWildcardMatch",
            request,
            Transport.DeleteVariablesWildcardMatchResult.Parser,
            cancellationToken);
    }

    public Task<bool> GetBooleanVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetBooleanVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<bool>(
            "briosa.Variables",
            "GetBooleanVariable",
            request,
            Transport.GetBooleanVariableResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName> GetCollectionObjectNameVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCollectionObjectNameVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<CollectionObjectName>(
            "briosa.Variables",
            "GetCollectionObjectNameVariable",
            request,
            Transport.GetCollectionObjectNameVariableResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName[]> GetCollectionObjectRefListVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCollectionObjectRefListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<CollectionObjectName[]>(
            "briosa.Variables",
            "GetCollectionObjectRefListVariable",
            request,
            Transport.GetCollectionObjectRefListVariableResult.Parser,
            cancellationToken);
    }

    public Task<double> GetDoubleVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetDoubleVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<double>(
            "briosa.Variables",
            "GetDoubleVariable",
            request,
            Transport.GetDoubleVariableResult.Parser,
            cancellationToken);
    }

    public Task<int> GetIntegerVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetIntegerVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<int>(
            "briosa.Variables",
            "GetIntegerVariable",
            request,
            Transport.GetIntegerVariableResult.Parser,
            cancellationToken);
    }

    public Task<double[]> GetNamedDoubleListVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetNamedDoubleListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<double[]>(
            "briosa.Variables",
            "GetNamedDoubleListVariable",
            request,
            Transport.GetNamedDoubleListVariableResult.Parser,
            cancellationToken);
    }

    public Task<GetNamedDoubleListVariableMinMaxResult> GetNamedDoubleListVariableMinMaxAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetNamedDoubleListVariableMinMaxRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<GetNamedDoubleListVariableMinMaxResult>(
            "briosa.Variables",
            "GetNamedDoubleListVariableMinMax",
            request,
            Transport.GetNamedDoubleListVariableMinMaxResult.Parser,
            cancellationToken);
    }

    public Task<PointName[]> GetPointNameRefListVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointNameRefListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<PointName[]>(
            "briosa.Variables",
            "GetPointNameRefListVariable",
            request,
            Transport.GetPointNameRefListVariableResult.Parser,
            cancellationToken);
    }

    public Task<PointName> GetPointNameVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointNameVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<PointName>(
            "briosa.Variables",
            "GetPointNameVariable",
            request,
            Transport.GetPointNameVariableResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName[]> GetRelationshipRefListVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipRefListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<CollectionObjectName[]>(
            "briosa.Variables",
            "GetRelationshipRefListVariable",
            request,
            Transport.GetRelationshipRefListVariableResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName[]> GetReportItemsReferenceListVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetReportItemsReferenceListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<CollectionObjectName[]>(
            "briosa.Variables",
            "GetReportItemsReferenceListVariable",
            request,
            Transport.GetReportItemsReferenceListVariableResult.Parser,
            cancellationToken);
    }

    public Task<string[]> GetStringRefListVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetStringRefListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<string[]>(
            "briosa.Variables",
            "GetStringRefListVariable",
            request,
            Transport.GetStringRefListVariableResult.Parser,
            cancellationToken);
    }

    public Task<string> GetStringVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetStringVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<string>(
            "briosa.Variables",
            "GetStringVariable",
            request,
            Transport.GetStringVariableResult.Parser,
            cancellationToken);
    }

    public Task<Transform> GetTransformVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetTransformVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<Transform>(
            "briosa.Variables",
            "GetTransformVariable",
            request,
            Transport.GetTransformVariableResult.Parser,
            cancellationToken);
    }

    public Task<VectorName[]> GetVectorNameRefListVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetVectorNameRefListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<VectorName[]>(
            "briosa.Variables",
            "GetVectorNameRefListVariable",
            request,
            Transport.GetVectorNameRefListVariableResult.Parser,
            cancellationToken);
    }

    public Task<Vector> GetVectorVariableAsync(
        string name = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetVectorVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return InvokeOperationAsync<Vector>(
            "briosa.Variables",
            "GetVectorVariable",
            request,
            Transport.GetVectorVariableResult.Parser,
            cancellationToken);
    }

    public Task SetBooleanVariableAsync(
        string name = "",
        bool value = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetBooleanVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetBooleanVariable",
            request,
            Transport.SetBooleanVariableResult.Parser,
            cancellationToken);
    }

    public Task SetCollectionObjectNameVariableAsync(
        string name,
        CollectionObjectName value,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCollectionObjectNameVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetCollectionObjectNameVariable",
            request,
            Transport.SetCollectionObjectNameVariableResult.Parser,
            cancellationToken);
    }

    public Task SetCollectionObjectRefListVariableAsync(
        string name,
        IEnumerable<CollectionObjectName> value,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCollectionObjectRefListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetCollectionObjectRefListVariable",
            request,
            Transport.SetCollectionObjectRefListVariableResult.Parser,
            cancellationToken);
    }

    public Task SetDoubleVariableAsync(
        string name = "",
        double value = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetDoubleVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetDoubleVariable",
            request,
            Transport.SetDoubleVariableResult.Parser,
            cancellationToken);
    }

    public Task SetFontVariableAsync(
        string name,
        Font value,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetFontVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetFontVariable",
            request,
            Transport.SetFontVariableResult.Parser,
            cancellationToken);
    }

    public Task SetIntegerVariableAsync(
        string name = "",
        int value = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetIntegerVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetIntegerVariable",
            request,
            Transport.SetIntegerVariableResult.Parser,
            cancellationToken);
    }

    public Task SetNamedDoubleListVariableAsync(
        string name,
        IEnumerable<double> doubleListVariable,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetNamedDoubleListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["double_list_variable"] = doubleListVariable,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetNamedDoubleListVariable",
            request,
            Transport.SetNamedDoubleListVariableResult.Parser,
            cancellationToken);
    }

    public Task SetPointNameRefListVariableAsync(
        string name,
        IEnumerable<PointName> value,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPointNameRefListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetPointNameRefListVariable",
            request,
            Transport.SetPointNameRefListVariableResult.Parser,
            cancellationToken);
    }

    public Task SetPointNameVariableAsync(
        string name,
        PointName value,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPointNameVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetPointNameVariable",
            request,
            Transport.SetPointNameVariableResult.Parser,
            cancellationToken);
    }

    public Task SetRelationshipRefListVariableAsync(
        string name,
        IEnumerable<CollectionItemName> value,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipRefListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetRelationshipRefListVariable",
            request,
            Transport.SetRelationshipRefListVariableResult.Parser,
            cancellationToken);
    }

    public Task SetReportItemsReferenceListVariableAsync(
        string name,
        IEnumerable<CollectionItemName> value,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetReportItemsReferenceListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetReportItemsReferenceListVariable",
            request,
            Transport.SetReportItemsReferenceListVariableResult.Parser,
            cancellationToken);
    }

    public Task SetStringRefListVariableAsync(
        string name,
        IEnumerable<string> value,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetStringRefListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetStringRefListVariable",
            request,
            Transport.SetStringRefListVariableResult.Parser,
            cancellationToken);
    }

    public Task SetStringVariableAsync(
        string name = "",
        string value = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetStringVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetStringVariable",
            request,
            Transport.SetStringVariableResult.Parser,
            cancellationToken);
    }

    public Task SetTransformVariableAsync(
        string name,
        Transform value,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetTransformVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetTransformVariable",
            request,
            Transport.SetTransformVariableResult.Parser,
            cancellationToken);
    }

    public Task SetVectorNameRefListVariableAsync(
        string name,
        IEnumerable<VectorName> value,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetVectorNameRefListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetVectorNameRefListVariable",
            request,
            Transport.SetVectorNameRefListVariableResult.Parser,
            cancellationToken);
    }

    public Task SetVectorVariableAsync(
        string name,
        Vector value,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetVectorVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return InvokeOperationAsync(
            "briosa.Variables",
            "SetVectorVariable",
            request,
            Transport.SetVectorVariableResult.Parser,
            cancellationToken);
    }

    public Task AddAVectorToVectorNameRefListAsync(
        CollectionObjectName vectorGroupName,
        string vectorName,
        IEnumerable<VectorName> vectorNameList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddAVectorToVectorNameRefListRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group_name"] = vectorGroupName,
                ["vector_name"] = vectorName,
                ["vector_name_list"] = vectorNameList,
            });
        return InvokeOperationAsync(
            "briosa.VectorOperations",
            "AddAVectorToVectorNameRefList",
            request,
            Transport.AddAVectorToVectorNameRefListResult.Parser,
            cancellationToken);
    }

    public Task AutoRangeAndSetVectorGroupColorizationAllAsync(
        bool treatIndividually,
        ColorizationOptions colorizationOptionsUsesModeOnly,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoRangeAndSetVectorGroupColorizationAllRequest(),
            new Dictionary<string, object?>
            {
                ["treat_individually"] = treatIndividually,
                ["colorization_options_uses_mode_only"] = colorizationOptionsUsesModeOnly,
            });
        return InvokeOperationAsync(
            "briosa.VectorOperations",
            "AutoRangeAndSetVectorGroupColorizationAll",
            request,
            Transport.AutoRangeAndSetVectorGroupColorizationAllResult.Parser,
            cancellationToken);
    }

    public Task AutoRangeAndSetVectorGroupColorizationSelectedAsync(
        IEnumerable<CollectionVectorGroupName> vectorGroupsToBeSet,
        bool treatIndividually,
        ColorizationOptions colorizationOptionsUsesModeOnly,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoRangeAndSetVectorGroupColorizationSelectedRequest(),
            new Dictionary<string, object?>
            {
                ["vector_groups_to_be_set"] = vectorGroupsToBeSet,
                ["treat_individually"] = treatIndividually,
                ["colorization_options_uses_mode_only"] = colorizationOptionsUsesModeOnly,
            });
        return InvokeOperationAsync(
            "briosa.VectorOperations",
            "AutoRangeAndSetVectorGroupColorizationSelected",
            request,
            Transport.AutoRangeAndSetVectorGroupColorizationSelectedResult.Parser,
            cancellationToken);
    }

    public Task DeleteIthVectorFromVectorGroupAsync(
        CollectionObjectName vectorGroupName,
        int vectorIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteIthVectorFromVectorGroupRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group_name"] = vectorGroupName,
                ["vector_index"] = vectorIndex,
            });
        return InvokeOperationAsync(
            "briosa.VectorOperations",
            "DeleteIthVectorFromVectorGroup",
            request,
            Transport.DeleteIthVectorFromVectorGroupResult.Parser,
            cancellationToken);
    }

    public Task DeleteVectorByNameAsync(
        CollectionObjectName vectorGroupName,
        string vectorName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteVectorByNameRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group_name"] = vectorGroupName,
                ["vector_name"] = vectorName,
            });
        return InvokeOperationAsync(
            "briosa.VectorOperations",
            "DeleteVectorByName",
            request,
            Transport.DeleteVectorByNameResult.Parser,
            cancellationToken);
    }

    public Task DeleteVectorsAsync(
        IEnumerable<VectorName> vectorNameList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteVectorsRequest(),
            new Dictionary<string, object?>
            {
                ["vector_name_list"] = vectorNameList,
            });
        return InvokeOperationAsync(
            "briosa.VectorOperations",
            "DeleteVectors",
            request,
            Transport.DeleteVectorsResult.Parser,
            cancellationToken);
    }

    public Task<GetIthVectorFromVectorGroupResult> GetIthVectorFromVectorGroupAsync(
        CollectionObjectName vectorGroupName,
        int vectorIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetIthVectorFromVectorGroupRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group_name"] = vectorGroupName,
                ["vector_index"] = vectorIndex,
            });
        return InvokeOperationAsync<GetIthVectorFromVectorGroupResult>(
            "briosa.VectorOperations",
            "GetIthVectorFromVectorGroup",
            request,
            Transport.GetIthVectorFromVectorGroupResult.Parser,
            cancellationToken);
    }

    public Task<GetIthVectorFromVectorNameRefListResult> GetIthVectorFromVectorNameRefListAsync(
        IEnumerable<VectorName> vectorNameList,
        int vectorIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetIthVectorFromVectorNameRefListRequest(),
            new Dictionary<string, object?>
            {
                ["vector_name_list"] = vectorNameList,
                ["vector_index"] = vectorIndex,
            });
        return InvokeOperationAsync<GetIthVectorFromVectorNameRefListResult>(
            "briosa.VectorOperations",
            "GetIthVectorFromVectorNameRefList",
            request,
            Transport.GetIthVectorFromVectorNameRefListResult.Parser,
            cancellationToken);
    }

    public Task<int> GetNumberOfVectorsInVectorGroupAsync(
        CollectionObjectName vectorGroupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetNumberOfVectorsInVectorGroupRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group_name"] = vectorGroupName,
            });
        return InvokeOperationAsync<int>(
            "briosa.VectorOperations",
            "GetNumberOfVectorsInVectorGroup",
            request,
            Transport.GetNumberOfVectorsInVectorGroupResult.Parser,
            cancellationToken);
    }

    public Task<int> GetNumberOfVectorsInVectorNameRefListAsync(
        IEnumerable<VectorName> vectorNameList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetNumberOfVectorsInVectorNameRefListRequest(),
            new Dictionary<string, object?>
            {
                ["vector_name_list"] = vectorNameList,
            });
        return InvokeOperationAsync<int>(
            "briosa.VectorOperations",
            "GetNumberOfVectorsInVectorNameRefList",
            request,
            Transport.GetNumberOfVectorsInVectorNameRefListResult.Parser,
            cancellationToken);
    }

    public Task<GetVectorFromVectorGroupByNameResult> GetVectorFromVectorGroupByNameAsync(
        CollectionObjectName vectorGroupName,
        string vectorName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetVectorFromVectorGroupByNameRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group_name"] = vectorGroupName,
                ["vector_name"] = vectorName,
            });
        return InvokeOperationAsync<GetVectorFromVectorGroupByNameResult>(
            "briosa.VectorOperations",
            "GetVectorFromVectorGroupByName",
            request,
            Transport.GetVectorFromVectorGroupByNameResult.Parser,
            cancellationToken);
    }

    public Task<GetVectorGroupPropertiesResult> GetVectorGroupPropertiesAsync(
        CollectionObjectName vectorGroupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetVectorGroupPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group_name"] = vectorGroupName,
            });
        return InvokeOperationAsync<GetVectorGroupPropertiesResult>(
            "briosa.VectorOperations",
            "GetVectorGroupProperties",
            request,
            Transport.GetVectorGroupPropertiesResult.Parser,
            cancellationToken);
    }

    public Task SetVectorGroupColorizationOptionsAllAsync(
        ColorizationOptions colorizationOptions,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetVectorGroupColorizationOptionsAllRequest(),
            new Dictionary<string, object?>
            {
                ["colorization_options"] = colorizationOptions,
            });
        return InvokeOperationAsync(
            "briosa.VectorOperations",
            "SetVectorGroupColorizationOptionsAll",
            request,
            Transport.SetVectorGroupColorizationOptionsAllResult.Parser,
            cancellationToken);
    }

    public Task SetVectorGroupColorizationOptionsSelectedAsync(
        IEnumerable<CollectionVectorGroupName> vectorGroupsToBeSet,
        ColorizationOptions colorizationOptions,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetVectorGroupColorizationOptionsSelectedRequest(),
            new Dictionary<string, object?>
            {
                ["vector_groups_to_be_set"] = vectorGroupsToBeSet,
                ["colorization_options"] = colorizationOptions,
            });
        return InvokeOperationAsync(
            "briosa.VectorOperations",
            "SetVectorGroupColorizationOptionsSelected",
            request,
            Transport.SetVectorGroupColorizationOptionsSelectedResult.Parser,
            cancellationToken);
    }

    public Task<VectorName[]> SortVectorsAsync(
        IEnumerable<VectorName> sourceVectors,
        string sortMethod,
        CoordinateSystemType coordinateSystem,
        string primarySortCoordinate = "X (R)",
        string secondarySortCoordinate = "Y (Theta)",
        string tertiarySortCoordinate = "Z (Phi)",
        double primaryCoordinateGranularity = 0.000000,
        double secondaryCoordinateGranularity = 0.000000,
        double tertiaryCoordinateGranularity = 0.000000,
        bool ascending = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SortVectorsRequest(),
            new Dictionary<string, object?>
            {
                ["source_vectors"] = sourceVectors,
                ["sort_method"] = sortMethod,
                ["coordinate_system"] = coordinateSystem,
                ["primary_sort_coordinate"] = primarySortCoordinate,
                ["secondary_sort_coordinate"] = secondarySortCoordinate,
                ["tertiary_sort_coordinate"] = tertiarySortCoordinate,
                ["primary_coordinate_granularity"] = primaryCoordinateGranularity,
                ["secondary_coordinate_granularity"] = secondaryCoordinateGranularity,
                ["tertiary_coordinate_granularity"] = tertiaryCoordinateGranularity,
                ["ascending"] = ascending,
            });
        return InvokeOperationAsync<VectorName[]>(
            "briosa.VectorOperations",
            "SortVectors",
            request,
            Transport.SortVectorsResult.Parser,
            cancellationToken);
    }

    public Task AutoScaleAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoScaleRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "AutoScale",
            request,
            Transport.AutoScaleResult.Parser,
            cancellationToken);
    }

    public Task CenterGraphicsAboutObjectsAsync(
        ObjectType objectType,
        string collectionWildcardCriteria = "*",
        string objectWildcardCriteria = "*",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CenterGraphicsAboutObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["object_type"] = objectType,
                ["collection_wildcard_criteria"] = collectionWildcardCriteria,
                ["object_wildcard_criteria"] = objectWildcardCriteria,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "CenterGraphicsAboutObjects",
            request,
            Transport.CenterGraphicsAboutObjectsResult.Parser,
            cancellationToken);
    }

    public Task CenterGraphicsAboutPointAsync(
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CenterGraphicsAboutPointRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "CenterGraphicsAboutPoint",
            request,
            Transport.CenterGraphicsAboutPointResult.Parser,
            cancellationToken);
    }

    public Task DefinePointOfViewAsync(
        ViewName viewName,
        double rotationX,
        double rotationY,
        double rotationZ,
        bool restoreZoomSettings,
        double scaleFactor,
        double originX,
        double originY,
        bool restoreRenderMode,
        RenderModeType renderingMode,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DefinePointOfViewRequest(),
            new Dictionary<string, object?>
            {
                ["view_name"] = viewName,
                ["rotation_x"] = rotationX,
                ["rotation_y"] = rotationY,
                ["rotation_z"] = rotationZ,
                ["restore_zoom_settings"] = restoreZoomSettings,
                ["scale_factor"] = scaleFactor,
                ["origin_x"] = originX,
                ["origin_y"] = originY,
                ["restore_render_mode"] = restoreRenderMode,
                ["rendering_mode"] = renderingMode,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "DefinePointOfView",
            request,
            Transport.DefinePointOfViewResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName[]> GetActiveClippingPlanesAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetActiveClippingPlanesRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync<CollectionObjectName[]>(
            "briosa.ViewControl",
            "GetActiveClippingPlanes",
            request,
            Transport.GetActiveClippingPlanesResult.Parser,
            cancellationToken);
    }

    public Task<GetPointOfViewParametersResult> GetPointOfViewParametersAsync(
        ViewName viewName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointOfViewParametersRequest(),
            new Dictionary<string, object?>
            {
                ["view_name"] = viewName,
            });
        return InvokeOperationAsync<GetPointOfViewParametersResult>(
            "briosa.ViewControl",
            "GetPointOfViewParameters",
            request,
            Transport.GetPointOfViewParametersResult.Parser,
            cancellationToken);
    }

    public Task HideAllCalloutViewsAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.HideAllCalloutViewsRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "HideAllCalloutViews",
            request,
            Transport.HideAllCalloutViewsResult.Parser,
            cancellationToken);
    }

    public Task HideObjectsAsync(
        IEnumerable<CollectionObjectName> objectsToHide,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.HideObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["objects_to_hide"] = objectsToHide,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "HideObjects",
            request,
            Transport.HideObjectsResult.Parser,
            cancellationToken);
    }

    public Task HighlightObjectsAsync(
        IEnumerable<CollectionObjectName> objectNamesEmptyToClearAll,
        bool highLightObjects = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.HighlightObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["object_names_empty_to_clear_all"] = objectNamesEmptyToClearAll,
                ["high_light_objects"] = highLightObjects,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "HighlightObjects",
            request,
            Transport.HighlightObjectsResult.Parser,
            cancellationToken);
    }

    public Task HighlightPointAsync(
        PointName pointNameEmptyToClearAll,
        bool showPoint = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.HighlightPointRequest(),
            new Dictionary<string, object?>
            {
                ["point_name_empty_to_clear_all"] = pointNameEmptyToClearAll,
                ["show_point"] = showPoint,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "HighlightPoint",
            request,
            Transport.HighlightPointResult.Parser,
            cancellationToken);
    }

    public Task HighlightRelationshipsAsync(
        IEnumerable<CollectionItemName> relationshipsEmptyToClearAll,
        bool highLightRelationships = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.HighlightRelationshipsRequest(),
            new Dictionary<string, object?>
            {
                ["relationships_empty_to_clear_all"] = relationshipsEmptyToClearAll,
                ["high_light_relationships"] = highLightRelationships,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "HighlightRelationships",
            request,
            Transport.HighlightRelationshipsResult.Parser,
            cancellationToken);
    }

    public Task LoadRibbonBarFromXmlFileAsync(
        FileReference filePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LoadRibbonBarFromXmlFileRequest(),
            new Dictionary<string, object?>
            {
                ["file_path"] = filePath,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "LoadRibbonBarFromXmlFile",
            request,
            Transport.LoadRibbonBarFromXmlFileResult.Parser,
            cancellationToken);
    }

    public Task RefreshViewsAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RefreshViewsRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "RefreshViews",
            request,
            Transport.RefreshViewsResult.Parser,
            cancellationToken);
    }

    public Task ResetRibbonBarToDefaultAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ResetRibbonBarToDefaultRequest(),
            new Dictionary<string, object?>
            { });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ResetRibbonBarToDefault",
            request,
            Transport.ResetRibbonBarToDefaultResult.Parser,
            cancellationToken);
    }

    public Task SavePointOfViewAsync(
        ViewName viewName,
        bool restoreZoomSettings = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SavePointOfViewRequest(),
            new Dictionary<string, object?>
            {
                ["view_name"] = viewName,
                ["restore_zoom_settings"] = restoreZoomSettings,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SavePointOfView",
            request,
            Transport.SavePointOfViewResult.Parser,
            cancellationToken);
    }

    public Task SetBackgroundColorAsync(
        Color solidColorName,
        Color gradientStartColorName,
        Color gradientEndColorName,
        Color highlightColor,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetBackgroundColorRequest(),
            new Dictionary<string, object?>
            {
                ["solid_color_name"] = solidColorName,
                ["gradient_start_color_name"] = gradientStartColorName,
                ["gradient_end_color_name"] = gradientEndColorName,
                ["highlight_color"] = highlightColor,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetBackgroundColor",
            request,
            Transport.SetBackgroundColorResult.Parser,
            cancellationToken);
    }

    public Task SetMpWindowStateAsync(
        WindowState mpWindowState,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetMpWindowStateRequest(),
            new Dictionary<string, object?>
            {
                ["mp_window_state"] = mpWindowState,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetMpWindowState",
            request,
            Transport.SetMpWindowStateResult.Parser,
            cancellationToken);
    }

    public Task SetObjectsColorAsync(
        IEnumerable<CollectionObjectName> objectsToChange,
        Color newWorkingColorName,
        bool autoIncrement = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetObjectsColorRequest(),
            new Dictionary<string, object?>
            {
                ["objects_to_change"] = objectsToChange,
                ["new_working_color_name"] = newWorkingColorName,
                ["auto_increment"] = autoIncrement,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetObjectsColor",
            request,
            Transport.SetObjectsColorResult.Parser,
            cancellationToken);
    }

    public Task SetObjectsTranslucencyAsync(
        IEnumerable<CollectionObjectName> objectsToChange,
        TranslucencyType renderingType,
        double opacityValue = 0.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetObjectsTranslucencyRequest(),
            new Dictionary<string, object?>
            {
                ["objects_to_change"] = objectsToChange,
                ["rendering_type"] = renderingType,
                ["opacity_value"] = opacityValue,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetObjectsTranslucency",
            request,
            Transport.SetObjectsTranslucencyResult.Parser,
            cancellationToken);
    }

    public Task SetPointOfViewAsync(
        ViewName viewName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPointOfViewRequest(),
            new Dictionary<string, object?>
            {
                ["view_name"] = viewName,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetPointOfView",
            request,
            Transport.SetPointOfViewResult.Parser,
            cancellationToken);
    }

    public Task SetPointOfViewFromFrameAsync(
        CollectionObjectName frame,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPointOfViewFromFrameRequest(),
            new Dictionary<string, object?>
            {
                ["frame"] = frame,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetPointOfViewFromFrame",
            request,
            Transport.SetPointOfViewFromFrameResult.Parser,
            cancellationToken);
    }

    public Task SetPointOfViewFromInstrumentUpdatesAsync(
        CollectionInstrumentId instrumentId,
        bool displayViewControl,
        bool enableSetViewpointFromInstrumentUpdates,
        double updateViewPercent,
        bool clipBehindProbe,
        bool automaticZoomWhenTrapping,
        bool enableDirectionalCloudPoints,
        double angleResetThreshold,
        int animationSteps,
        CollectionObjectName referenceFrameObject,
        bool useScanStripeForViewFocus = true,
        double zoomFactor = 1.000000,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPointOfViewFromInstrumentUpdatesRequest(),
            new Dictionary<string, object?>
            {
                ["instrument_id"] = instrumentId,
                ["display_view_control"] = displayViewControl,
                ["enable_set_viewpoint_from_instrument_updates"] = enableSetViewpointFromInstrumentUpdates,
                ["update_view_percent"] = updateViewPercent,
                ["clip_behind_probe"] = clipBehindProbe,
                ["automatic_zoom_when_trapping"] = automaticZoomWhenTrapping,
                ["enable_directional_cloud_points"] = enableDirectionalCloudPoints,
                ["angle_reset_threshold"] = angleResetThreshold,
                ["animation_steps"] = animationSteps,
                ["reference_frame_object"] = referenceFrameObject,
                ["use_scan_stripe_for_view_focus"] = useScanStripeForViewFocus,
                ["zoom_factor"] = zoomFactor,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetPointOfViewFromInstrumentUpdates",
            request,
            Transport.SetPointOfViewFromInstrumentUpdatesResult.Parser,
            cancellationToken);
    }

    public Task SetRenderModeTypeAsync(
        RenderModeType renderingMode,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRenderModeTypeRequest(),
            new Dictionary<string, object?>
            {
                ["rendering_mode"] = renderingMode,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetRenderModeType",
            request,
            Transport.SetRenderModeTypeResult.Parser,
            cancellationToken);
    }

    public Task SetSaWindowPosAsync(
        int posX = 0,
        int posY = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetSaWindowPosRequest(),
            new Dictionary<string, object?>
            {
                ["pos_x"] = posX,
                ["pos_y"] = posY,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetSaWindowPos",
            request,
            Transport.SetSaWindowPosResult.Parser,
            cancellationToken);
    }

    public Task SetSaWindowSizeAsync(
        int width = 0,
        int height = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetSaWindowSizeRequest(),
            new Dictionary<string, object?>
            {
                ["width"] = width,
                ["height"] = height,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetSaWindowSize",
            request,
            Transport.SetSaWindowSizeResult.Parser,
            cancellationToken);
    }

    public Task SetSaWindowStateAsync(
        WindowState saWindowState,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetSaWindowStateRequest(),
            new Dictionary<string, object?>
            {
                ["sa_window_state"] = saWindowState,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetSaWindowState",
            request,
            Transport.SetSaWindowStateResult.Parser,
            cancellationToken);
    }

    public Task SetTargetLabelsUseFullNamesAsync(
        bool useFullNames = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetTargetLabelsUseFullNamesRequest(),
            new Dictionary<string, object?>
            {
                ["use_full_names"] = useFullNames,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetTargetLabelsUseFullNames",
            request,
            Transport.SetTargetLabelsUseFullNamesResult.Parser,
            cancellationToken);
    }

    public Task SetToolkitVisibilityAsync(
        bool showToolkit = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetToolkitVisibilityRequest(),
            new Dictionary<string, object?>
            {
                ["show_toolkit"] = showToolkit,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetToolkitVisibility",
            request,
            Transport.SetToolkitVisibilityResult.Parser,
            cancellationToken);
    }

    public Task SetViewClippingPlaneAsync(
        CollectionObjectName @object,
        bool removeClippingPlane = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetViewClippingPlaneRequest(),
            new Dictionary<string, object?>
            {
                ["object"] = @object,
                ["remove_clipping_plane"] = removeClippingPlane,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetViewClippingPlane",
            request,
            Transport.SetViewClippingPlaneResult.Parser,
            cancellationToken);
    }

    public Task SetWorkingColorAsync(
        Color newWorkingColorName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetWorkingColorRequest(),
            new Dictionary<string, object?>
            {
                ["new_working_color_name"] = newWorkingColorName,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetWorkingColor",
            request,
            Transport.SetWorkingColorResult.Parser,
            cancellationToken);
    }

    public Task SetWorkingColorAutoIncrementAsync(
        bool autoIncrement = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetWorkingColorAutoIncrementRequest(),
            new Dictionary<string, object?>
            {
                ["auto_increment"] = autoIncrement,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "SetWorkingColorAutoIncrement",
            request,
            Transport.SetWorkingColorAutoIncrementResult.Parser,
            cancellationToken);
    }

    public Task ShowHideByObjectTypeAsync(
        bool allCollections,
        CollectionName specificCollection,
        ObjectType objectTypeToShowHide,
        bool hideShowFalse = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowHideByObjectTypeRequest(),
            new Dictionary<string, object?>
            {
                ["all_collections"] = allCollections,
                ["specific_collection"] = specificCollection,
                ["object_type_to_show_hide"] = objectTypeToShowHide,
                ["hide_show_false"] = hideShowFalse,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowHideByObjectType",
            request,
            Transport.ShowHideByObjectTypeResult.Parser,
            cancellationToken);
    }

    public Task ShowHideCalloutViewAsync(
        CollectionItemName calloutViewToShow,
        bool showCalloutView = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowHideCalloutViewRequest(),
            new Dictionary<string, object?>
            {
                ["callout_view_to_show"] = calloutViewToShow,
                ["show_callout_view"] = showCalloutView,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowHideCalloutView",
            request,
            Transport.ShowHideCalloutViewResult.Parser,
            cancellationToken);
    }

    public Task ShowHideDimensionAsync(
        CollectionItemName dimensionName,
        bool showDimension = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowHideDimensionRequest(),
            new Dictionary<string, object?>
            {
                ["dimension_name"] = dimensionName,
                ["show_dimension"] = showDimension,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowHideDimension",
            request,
            Transport.ShowHideDimensionResult.Parser,
            cancellationToken);
    }

    public Task ShowHidePointsAsync(
        IEnumerable<PointName> pointNames,
        bool showHideFalse = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowHidePointsRequest(),
            new Dictionary<string, object?>
            {
                ["point_names"] = pointNames,
                ["show_hide_false"] = showHideFalse,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowHidePoints",
            request,
            Transport.ShowHidePointsResult.Parser,
            cancellationToken);
    }

    public Task ShowByObjectTypeAsync(
        CollectionObjectName objectTypeToShow,
        bool allCollections = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowByObjectTypeRequest(),
            new Dictionary<string, object?>
            {
                ["object_type_to_show"] = objectTypeToShow,
                ["all_collections"] = allCollections,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowByObjectType",
            request,
            Transport.ShowByObjectTypeResult.Parser,
            cancellationToken);
    }

    public Task ShowItemsInTreeAsync(
        bool collapseAllOtherItems,
        IEnumerable<PointName> points,
        IEnumerable<CollectionObjectName> objects,
        IEnumerable<CollectionInstrumentId> instruments,
        IEnumerable<CollectionItemName> featureChecks,
        IEnumerable<CollectionObjectName> datums,
        IEnumerable<string> collections,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowItemsInTreeRequest(),
            new Dictionary<string, object?>
            {
                ["collapse_all_other_items"] = collapseAllOtherItems,
                ["points"] = points,
                ["objects"] = objects,
                ["instruments"] = instruments,
                ["feature_checks"] = featureChecks,
                ["datums"] = datums,
                ["collections"] = collections,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowItemsInTree",
            request,
            Transport.ShowItemsInTreeResult.Parser,
            cancellationToken);
    }

    public Task ShowLabelsAsync(
        bool pointLabelsOn = false,
        bool objectsLabelsOn = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowLabelsRequest(),
            new Dictionary<string, object?>
            {
                ["point_labels_on"] = pointLabelsOn,
                ["objects_labels_on"] = objectsLabelsOn,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowLabels",
            request,
            Transport.ShowLabelsResult.Parser,
            cancellationToken);
    }

    public Task ShowObjectsAsync(
        IEnumerable<CollectionObjectName> objectsToShow,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["objects_to_show"] = objectsToShow,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowObjects",
            request,
            Transport.ShowObjectsResult.Parser,
            cancellationToken);
    }

    public Task ShowHideAnnotationsForDatumsAsync(
        IEnumerable<CollectionObjectName> datumNameList,
        bool show = false,
        bool highlight = false,
        bool setInspectionView = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowHideAnnotationsForDatumsRequest(),
            new Dictionary<string, object?>
            {
                ["datum_name_list"] = datumNameList,
                ["show"] = show,
                ["highlight"] = highlight,
                ["set_inspection_view"] = setInspectionView,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowHideAnnotationsForDatums",
            request,
            Transport.ShowHideAnnotationsForDatumsResult.Parser,
            cancellationToken);
    }

    public Task ShowHideAnnotationsForFeatureChecksAsync(
        IEnumerable<CollectionItemName> featureCheckNameList,
        bool show = false,
        bool highlight = false,
        bool setInspectionView = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowHideAnnotationsForFeatureChecksRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check_name_list"] = featureCheckNameList,
                ["show"] = show,
                ["highlight"] = highlight,
                ["set_inspection_view"] = setInspectionView,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowHideAnnotationsForFeatureChecks",
            request,
            Transport.ShowHideAnnotationsForFeatureChecksResult.Parser,
            cancellationToken);
    }

    public Task ShowHideInspectionBarAsync(
        bool showInspectionBar = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowHideInspectionBarRequest(),
            new Dictionary<string, object?>
            {
                ["show_inspection_bar"] = showInspectionBar,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowHideInspectionBar",
            request,
            Transport.ShowHideInspectionBarResult.Parser,
            cancellationToken);
    }

    public Task ShowHideInstrumentInterfaceAsync(
        CollectionInstrumentId instrumentId,
        bool minimizeInterface = false,
        bool hideInterface = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowHideInstrumentInterfaceRequest(),
            new Dictionary<string, object?>
            {
                ["instrument_id"] = instrumentId,
                ["minimize_interface"] = minimizeInterface,
                ["hide_interface"] = hideInterface,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowHideInstrumentInterface",
            request,
            Transport.ShowHideInstrumentInterfaceResult.Parser,
            cancellationToken);
    }

    public Task ShowHideInstrumentProbeTipAsync(
        bool showInstrumentProbeTip = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowHideInstrumentProbeTipRequest(),
            new Dictionary<string, object?>
            {
                ["show_instrument_probe_tip"] = showInstrumentProbeTip,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowHideInstrumentProbeTip",
            request,
            Transport.ShowHideInstrumentProbeTipResult.Parser,
            cancellationToken);
    }

    public Task ShowHideInstrumentsAsync(
        IEnumerable<CollectionInstrumentId> instrumentIDs,
        bool showInstruments = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowHideInstrumentsRequest(),
            new Dictionary<string, object?>
            {
                ["instrument_i_ds"] = instrumentIDs,
                ["show_instruments"] = showInstruments,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowHideInstruments",
            request,
            Transport.ShowHideInstrumentsResult.Parser,
            cancellationToken);
    }

    public Task ShowHideRelationshipReportAsync(
        CollectionName collectionName,
        bool showRelationshipReport = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowHideRelationshipReportRequest(),
            new Dictionary<string, object?>
            {
                ["collection_name"] = collectionName,
                ["show_relationship_report"] = showRelationshipReport,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowHideRelationshipReport",
            request,
            Transport.ShowHideRelationshipReportResult.Parser,
            cancellationToken);
    }

    public Task ShowHideRelationshipWatchAsync(
        CollectionObjectName relationshipName,
        bool showRelationshipWatch,
        CollectionObjectName relationshipWatchWindowProperties,
        int windowTopLeftXPosition = 0,
        int windowTopLeftYPosition = 0,
        int windowWidth = 0,
        int windowHeight = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShowHideRelationshipWatchRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["show_relationship_watch"] = showRelationshipWatch,
                ["relationship_watch_window_properties"] = relationshipWatchWindowProperties,
                ["window_top_left_x_position"] = windowTopLeftXPosition,
                ["window_top_left_y_position"] = windowTopLeftYPosition,
                ["window_width"] = windowWidth,
                ["window_height"] = windowHeight,
            });
        return InvokeOperationAsync(
            "briosa.ViewControl",
            "ShowHideRelationshipWatch",
            request,
            Transport.ShowHideRelationshipWatchResult.Parser,
            cancellationToken);
    }

    internal static IReadOnlyList<string> WaveAOperationNames { get; } =
    [
        "AngleBetweenLineAndPlane",
        "AngleBetweenTwoLines",
        "AngleBetweenTwoPlanesNormals",
        "BestFitTransformationGroupToGroup",
        "ComputeGroupToGroupOrientationRxRyRz",
        "CreatePointUncertaintyCloudPointSets",
        "CreatePointUncertaintyFields",
        "FitGeometryToPointGroup",
        "FitGeometryToPointGroupProjectedToPlane",
        "FitGeometryToPoints",
        "GetBSplineProperties",
        "GetCircleProperties",
        "GetConeProperties",
        "GetCoordinateForIthPointInPointSet",
        "GetCylinderProperties",
        "GetEllipseProperties",
        "GetEulerParametersForFrame",
        "GetEulerParametersForIthFrameInFrameSet",
        "GetIthCollectionName",
        "GetIthPointFromGroup",
        "GetLineProperties",
        "GetMeasurementAuxiliaryData",
        "GetMeasurementInfoData",
        "GetMeasurementWeatherData",
        "GetNumberOfCollections",
        "GetNumberOfFramesInFrameSet",
        "GetNumberOfPointsInGroup",
        "GetNumberOfPointsInPointSet",
        "GetObjectReportingFrame",
        "GetPlaneProperties",
        "GetPointCoordinate",
        "GetPointCoordinateCylindrical",
        "GetPointCoordinatePolar",
        "GetPointProperties",
        "GetPointToLineDistance",
        "GetPointToPointDistance",
        "GetPointTolerance",
        "GetSlotProperties",
        "GetSphereProperties",
        "GetSurfacePhysicalStats",
        "GetTimestampForIthFrameInFrameSet",
        "GetTimestampForIthPointInPointSet",
        "GetTorusProperties",
        "GetTransformForIthFrameInFrameSet",
        "GroupToSurfaceFit",
        "ImportGeometryFitProfiles",
        "IsObjectOfType",
        "MakeCircleFitProfile",
        "MakeConeFitProfile",
        "MakeCylinderFitProfile",
        "MakeEllipseFitProfile",
        "MakeLineFitProfile",
        "MakeParaboloidFitProfile",
        "MakePlaneFitProfile",
        "MakeSlotFitProfile",
        "MakeSphereFitProfile",
        "MushroomTargetHoleInspection",
        "PatchNormalShiftHolePin",
        "PatchNormalShiftPoint",
        "QueryCloudsToObjects",
        "QueryCloudsToSurface",
        "QueryFrameToFrame",
        "QueryGroupsToObjects",
        "QueryPointToObjects",
        "QueryPointToPointAlongCurve",
        "QueryPointsToCircle",
        "QueryPointsToObjects",
        "QueryPointsToSinglePoint",
        "ReComputeCalculatedItems",
        "RenamePointsBasedOnInterPointDistanceToReferencePoints",
        "RenamePointsBasedOnProximityToReferencePoints",
        "ReverseBSplines",
        "ReversePlaneNormals",
        "ReverseSurfaceNormals",
        "SetCircleProperties",
        "SetConeProperties",
        "SetCylinderProperties",
        "SetDefaultColorizationOptions",
        "SetEllipseProperties",
        "SetGeometryRelationshipFitProfile",
        "SetLineProperties",
        "SetMeasurementAuxiliaryData",
        "SetObjectReportingFrame",
        "SetPointProperties",
        "SetPointWeightsFromUncertainties",
        "SetTransformForIthFrameInFrameSet",
        "SphereAxisCheck",
        "TemperatureCompensateAGroup",
        "TransformObjectsFrameToFrame",
        "TransformObjectsByDeltaAboutWorkingFrame",
        "TransformObjectsByDeltaWorldTransformOperator",
        "TranslateObjectsByDelta",
        "DeleteDimension",
        "GetDimensionValue",
        "SetDimensionTolerance",
        "DeleteEvent",
        "ExportEventRefList",
        "GetIthEventFromEventRefList",
        "GetNumberOfEventsInEventRefList",
        "RenameEvent",
        "BackupNow",
        "CopyGeneralFile",
        "DeleteGeneralFile",
        "DirectCadAccess",
        "ExportAsciiFrameSet",
        "ExportAsciiFrames",
        "ExportAsciiPointClouds",
        "ExportAsciiPointSet",
        "ExportAsciiPoints",
        "ExportDxf",
        "ExportEmbeddedFile",
        "ExportHiddenPointBarXmlFile",
        "ExportIgesFileEntireModel",
        "ExportIgesFilePartialModel",
        "ExportPtxPointClouds",
        "ExportQdasCharacteristics",
        "ExportQdasDataList",
        "ExportScanStripeMeshToStlFile",
        "ExportStepFileEntireModel",
        "ExportStepFilePartialModel",
        "ExportVdaFsFileEntireModel",
        "ExportVdaFsFilePartialModel",
        "ExportVectorContainerToAsciiFile",
        "FindFilesInDirectory",
        "FindSubDirectoriesInDirectory",
        "GetBooleanFromDataShareFile",
        "GetDoubleFromDataShareFile",
        "GetIntegerFromDataShareFile",
        "GetQdasCatalogEntries",
        "GetStringFromDataShareFile",
        "GetTransformFromDataShareFile",
        "GetVectorFromDataShareFile",
        "GetWorkingDirectory",
        "ImportAsciiPredefinedFormats",
        "ImportAsciiPredefinedFrameSetFormats",
        "ImportE57File",
        "ImportFileAsEmbeddedFile",
        "ImportFileAsPicture",
        "ImportHiddenPointBarXmlFile",
        "ImportIgesFile",
        "ImportLeicaGsiFile",
        "ImportLeicaSdbFile",
        "ImportMpFileAsEmbeddedMp",
        "ImportNominalsFromXmlFile",
        "ImportPolyworksFile",
        "ImportQdasCatalogFile",
        "ImportSaFile",
        "ImportSaWindowsPlacement",
        "ImportSatFile",
        "ImportStepFile",
        "ImportStlFile",
        "ImportVdaFsFile",
        "ImportVstarsXyzFile",
        "ImportVstarsCameras",
        "LoadHtmlForm",
        "LoadHtmlFormInEdgeBrowser",
        "MakeEmbeddedFileNameList",
        "MergeMeasurementsIntoXmlFile",
        "NewSaFile",
        "OpenSaFile",
        "OpenTemplateFile",
        "PopPolyBayAnalysisWindow",
        "PrepareQdasDataList",
        "RenameGeneralFile",
        "Save",
        "SaveAsReadOnlyTemplate",
        "SaveAs",
        "SetBooleanInDataShareFile",
        "SetDoubleInDataShareFile",
        "SetIntegerInDataShareFile",
        "SetStringInDataShareFile",
        "SetTransformInDataShareFile",
        "SetVectorInDataShareFile",
        "TerminateAllRunningMPs",
        "UseNrkxmlLibrary",
        "VerifyGeneralFileExists",
        "VerifyMpFileExists",
        "RunSubroutine",
        "AddTaskOverviewItem",
        "CreateClearTaskOverviewList",
        "SetCurrentTask",
        "SetOverviewImage",
        "SetOverviewTitle",
        "SetTaskItemComment",
        "SetTaskItemCompletionValues",
        "SetTaskItemName",
        "ShowProgressForTaskItem",
        "ShowTaskOverviewList",
        "AskForDouble",
        "AskForInteger",
        "AskForPointName",
        "AskForString",
        "AskForStringPullDownVersion",
        "AskForUserDecisionFromImage",
        "AskForUserDecisionFromStrings",
        "ObjectExistenceTestCheckOnly",
        "EnableDisableRelationshipsForOptimization",
        "GeomRelationshipIgnoreInputPoints",
        "GeomRelationshipReuseIgnoredInputPoints",
        "GetGeomRelationshipAutoVectors",
        "GetGeomRelationshipCardinalPoints",
        "GetGeomRelationshipCriteria",
        "GetGeomRelationshipMeasuredAvgPoint",
        "GetGeomRelationshipMeasuredGeometry",
        "GetGeomRelationshipNominalAvgPoint",
        "GetGeomRelationshipNominalGeometry",
        "GetGeomRelationshipPointList",
        "GetGeomRelationshipProjectionPlane",
        "GetPipeRelationshipCutStatus",
        "GetPipeRelationshipProperties",
        "GetPipeRelationshipWeights",
        "GetRelationshipFitConstraintsScalarType",
        "GetRelationshipOutlierRejectionScalarType",
        "GetRelationshipProjectionOptions",
        "GetRelationshipReportingFrame",
        "GetRelationshipSubSamplingOptions",
        "GetRelationshipToleranceScalarType",
        "GetRelationshipToleranceVectorType",
        "GetRelationshipType",
        "GetRelationshipWeighting",
        "MakePipeFittingRelationship",
        "MakePipeRelationshipCut",
        "PipeRelationshipForceCutToFrame",
        "SetGeomRelationshipAutoMeasureNominalFeature",
        "SetGeomRelationshipAutoVectorsNominalAvn",
        "SetGeomRelationshipCardinalPoints",
        "SetGeomRelationshipCriteria",
        "SetGeomRelationshipMeasuredGeometry",
        "SetGeomRelationshipNominalAvgPoint",
        "SetGeomRelationshipNominalGeometry",
        "SetGeomRelationshipProjectionPlane",
        "SetObjectToObjectDirectionRelationshipFitConstraints",
        "SetPipeRelationshipSegmentProperties",
        "SetPipeRelationshipWeights",
        "SetRelationshipAutoVectorsFitAvf",
        "SetRelationshipAutoVectorsGroupDefaultPrefix",
        "SetRelationshipDesiredMeasCount",
        "SetRelationshipDormantStatus",
        "SetRelationshipFitConstraintsScalarType",
        "SetRelationshipOrientationFitConstraintsVectorType",
        "SetRelationshipOutlierRejectionScalarType",
        "SetRelationshipPositionFitConstraintsVectorType",
        "SetRelationshipProjectionOptions",
        "SetRelationshipReportingFrame",
        "SetRelationshipSigmoidalGapFitConstraints",
        "SetRelationshipSubSamplingOptions",
        "SetRelationshipToleranceScalarType",
        "SetRelationshipToleranceVectorType",
        "SetRelationshipVoxelCloudDisplay",
        "SetRelationshipWeighting",
        "SetRelationshipWeightsNormalized",
        "AddChartsToReportBar",
        "AddCustomTableToSaReport",
        "AddCustomTablesToReportBar",
        "AddDatumsToReportBar",
        "AddEventsToReportBar",
        "AddFeatureChecksToReportBar",
        "AddItemToSaReportAtLocation",
        "AddObjectsToReportBar",
        "AddPicturesToReportBar",
        "AddRelationshipsToReportBar",
        "AppendItemsToSaReport",
        "CaptureCurrentView",
        "CaptureScreenToFileBmpJpgPngGifTiff",
        "ClearCustomTable",
        "CloseAllReports",
        "CloseHtmlDisplayBoard",
        "CombineSaReports",
        "CreateChartFromVectorGroup",
        "DefineReportTemplate",
        "DeleteChart",
        "DeleteCustomTable",
        "DeletePicture",
        "DeleteSaDoc",
        "DeleteSaReport",
        "DeleteSaReportTemplate",
        "GenerateQuickReportFromTabOrder",
        "GenerateStandardHtmlReport",
        "GenerateUpdateTemplatedReport",
        "GetCustomTableCellDouble",
        "GetCustomTableCellString",
        "GetDefinedReportTags",
        "GetReportTagValue",
        "HtmlDisplayBoard",
        "MakeCustomTable",
        "MakeNewSaReport",
        "MakeUtilityChart",
        "NotifyUserDouble",
        "NotifyUserHtml",
        "NotifyUserInteger",
        "NotifyUserTextArray",
        "OutputSaReportToExcel",
        "OutputSaReportToPdf",
        "QuickReport",
        "RefreshCalloutViewsInSaReport",
        "RefreshReportBar",
        "RemoveReportTag",
        "RenamePicture",
        "SaveChartToJPegFile",
        "SaveCurrentViewBmpJpgPngGifTiff",
        "SetCustomTableCellColor",
        "SetCustomTableCellDouble",
        "SetCustomTableCellFont",
        "SetCustomTableCellString",
        "SetCustomTableHeaderCell",
        "SetCustomTableHeaderRow",
        "SetCustomTableTitle",
        "SetPointGroupReportOptions",
        "SetRelationshipReportOptions",
        "SetReportBarVisibility",
        "SetReportOptionsForObject",
        "SetReportTagValueFromDouble",
        "SetReportTagValueFromInteger",
        "SetReportTagValueFromString",
        "SetScaleForPicture",
        "SetVectorGroupReportOptions",
        "DeleteScaleBar",
        "GetScaleBarStats",
        "ScaleBarCheck",
        "SetInwardPositiveNormal",
        "CloseAllWatchWindows",
        "DeleteFolder",
        "DeleteItems",
        "DeleteObjects",
        "GetActiveLanguage",
        "GetActiveUnits",
        "GetAngularRepresentation",
        "GetCollectionNotes",
        "GetFolderCollections",
        "GetFolderNotes",
        "GetFoldersByWildcard",
        "GetObjectNotes",
        "GetOpcDaTagValueDouble",
        "GetOpcDaTagValueInteger",
        "GetOpcDaTagValueString",
        "GetPointNotes",
        "GetScreenResolution",
        "GetWorkingFrameProperties",
        "IncrementPointName",
        "LockImportedItems",
        "LockUnlockSelectedItems",
        "LockUnlockTrappingControl",
        "MoveCollectionToFolder",
        "MoveFolderToFolder",
        "MoveInstrumentsDragGraphically",
        "MoveObjectsDragGraphically",
        "ScaleObjects",
        "SetActiveCustomLanguage",
        "SetActiveUnits",
        "SetAngularRepresentation",
        "SetAutoEventCreation",
        "SetAutomaticBackupState",
        "SetAutomaticRelationshipConstructionState",
        "SetCollectionNotes",
        "SetDecimalDigitsForDisplay",
        "SetFolderNotes",
        "SetInteractionMode",
        "SetLoggingState",
        "SetNotificationCancelOverride",
        "SetObjectNotes",
        "SetOpcDaTagValueDouble",
        "SetOpcDaTagValueInteger",
        "SetOpcDaTagValueString",
        "SetPointNotes",
        "SetUserInterfaceProfile",
        "SetViewIdleUpdateFrequency",
        "SetWildCardAsteriskMode",
        "SetWorkingFrame",
        "StatusDialog",
        "TrimLogFile",
        "WriteToLog",
        "AddDoubleToNamedDoubleListVariable",
        "ClearNamedDoubleListVariable",
        "DeleteVariable",
        "DeleteVariablesWildcardMatch",
        "GetBooleanVariable",
        "GetCollectionObjectNameVariable",
        "GetCollectionObjectRefListVariable",
        "GetDoubleVariable",
        "GetIntegerVariable",
        "GetNamedDoubleListVariable",
        "GetNamedDoubleListVariableMinMax",
        "GetPointNameRefListVariable",
        "GetPointNameVariable",
        "GetRelationshipRefListVariable",
        "GetReportItemsReferenceListVariable",
        "GetStringRefListVariable",
        "GetStringVariable",
        "GetTransformVariable",
        "GetVectorNameRefListVariable",
        "GetVectorVariable",
        "SetBooleanVariable",
        "SetCollectionObjectNameVariable",
        "SetCollectionObjectRefListVariable",
        "SetDoubleVariable",
        "SetFontVariable",
        "SetIntegerVariable",
        "SetNamedDoubleListVariable",
        "SetPointNameRefListVariable",
        "SetPointNameVariable",
        "SetRelationshipRefListVariable",
        "SetReportItemsReferenceListVariable",
        "SetStringRefListVariable",
        "SetStringVariable",
        "SetTransformVariable",
        "SetVectorNameRefListVariable",
        "SetVectorVariable",
        "AddAVectorToVectorNameRefList",
        "AutoRangeAndSetVectorGroupColorizationAll",
        "AutoRangeAndSetVectorGroupColorizationSelected",
        "DeleteIthVectorFromVectorGroup",
        "DeleteVectorByName",
        "DeleteVectors",
        "GetIthVectorFromVectorGroup",
        "GetIthVectorFromVectorNameRefList",
        "GetNumberOfVectorsInVectorGroup",
        "GetNumberOfVectorsInVectorNameRefList",
        "GetVectorFromVectorGroupByName",
        "GetVectorGroupProperties",
        "SetVectorGroupColorizationOptionsAll",
        "SetVectorGroupColorizationOptionsSelected",
        "SortVectors",
        "AutoScale",
        "CenterGraphicsAboutObjects",
        "CenterGraphicsAboutPoint",
        "DefinePointOfView",
        "GetActiveClippingPlanes",
        "GetPointOfViewParameters",
        "HideAllCalloutViews",
        "HideObjects",
        "HighlightObjects",
        "HighlightPoint",
        "HighlightRelationships",
        "LoadRibbonBarFromXmlFile",
        "RefreshViews",
        "ResetRibbonBarToDefault",
        "SavePointOfView",
        "SetBackgroundColor",
        "SetMpWindowState",
        "SetObjectsColor",
        "SetObjectsTranslucency",
        "SetPointOfView",
        "SetPointOfViewFromFrame",
        "SetPointOfViewFromInstrumentUpdates",
        "SetRenderModeType",
        "SetSaWindowPos",
        "SetSaWindowSize",
        "SetSaWindowState",
        "SetTargetLabelsUseFullNames",
        "SetToolkitVisibility",
        "SetViewClippingPlane",
        "SetWorkingColor",
        "SetWorkingColorAutoIncrement",
        "ShowHideByObjectType",
        "ShowHideCalloutView",
        "ShowHideDimension",
        "ShowHidePoints",
        "ShowByObjectType",
        "ShowItemsInTree",
        "ShowLabels",
        "ShowObjects",
        "ShowHideAnnotationsForDatums",
        "ShowHideAnnotationsForFeatureChecks",
        "ShowHideInspectionBar",
        "ShowHideInstrumentInterface",
        "ShowHideInstrumentProbeTip",
        "ShowHideInstruments",
        "ShowHideRelationshipReport",
        "ShowHideRelationshipWatch",
    ];
}
