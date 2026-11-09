// Drafted mechanically from the approved Briosa documentation contract.
#pragma warning disable CA1008 // Zero is intentionally not a public MP choice.
#pragma warning disable CA1720
#pragma warning disable CA1805 // Explicit initializers preserve reviewed MP defaults.
#pragma warning disable CS1591
using Transport = Briosa.Client.Transport;

namespace Briosa;

public sealed partial class BriosaClient
{
    /// <remarks>
    /// <c>thin</c>: MP qualifier: Draw Increment.<br/>
    /// </remarks>
    public Task CloudDisplayControlAsync(
        int thin = 1,
        int pointSize = 1,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CloudDisplayControlRequest(),
            new Dictionary<string, object?>
            {
                ["thin"] = thin,
                ["point_size"] = pointSize,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "CloudDisplayControl",
            request,
            Transport.CloudDisplayControlResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<ResetCloudBoundingBoxResult> ResetCloudBoundingBoxAsync(
        CollectionObjectName cloudName,
        CloudBoxType cloudBoxType = CloudBoxType.WorldAxisAlignedBox,
        bool showBoundingBox = true,
        bool useAllPoints = false,
        int desiredPointCount = 1000,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ResetCloudBoundingBoxRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_name"] = cloudName,
                ["cloud_box_type"] = cloudBoxType,
                ["show_bounding_box"] = showBoundingBox,
                ["use_all_points"] = useAllPoints,
                ["desired_point_count"] = desiredPointCount,
            });
        return InvokeOperationAsync<ResetCloudBoundingBoxResult>(
            "briosa.CloudAndMeshOperations",
            "ResetCloudBoundingBox",
            request,
            Transport.ResetCloudBoundingBoxResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<GetCloudPointCountResult> GetCloudPointCountAsync(
        CollectionObjectName cloudName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCloudPointCountRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_name"] = cloudName,
            });
        return InvokeOperationAsync<GetCloudPointCountResult>(
            "briosa.CloudAndMeshOperations",
            "GetCloudPointCount",
            request,
            Transport.GetCloudPointCountResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task SetCloudDefaultClippingPlaneAsync(
        bool enableCloudClipping = false,
        CollectionObjectName? referenceObject = null,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCloudDefaultClippingPlaneRequest(),
            new Dictionary<string, object?>
            {
                ["enable_cloud_clipping"] = enableCloudClipping,
                ["reference_object"] = referenceObject,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "SetCloudDefaultClippingPlane",
            request,
            Transport.SetCloudDefaultClippingPlaneResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<string> RasterScanEdgeInspectionAsync(
        IEnumerable<CollectionObjectName> cloudNames,
        CollectionObjectName edgeSurfaceName,
        IEnumerable<CollectionObjectName> bSplineEdgeList,
        CollectionObjectName prefixForOutputGroups,
        double tolerance = 0.0,
        int minimumGoodPointsPerUnitLength = 0,
        double maximumBadPointsPercentage = 0.0,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RasterScanEdgeInspectionRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_names"] = cloudNames,
                ["edge_surface_name"] = edgeSurfaceName,
                ["b_spline_edge_list"] = bSplineEdgeList,
                ["prefix_for_output_groups"] = prefixForOutputGroups,
                ["tolerance"] = tolerance,
                ["minimum_good_points_per_unit_length"] = minimumGoodPointsPerUnitLength,
                ["maximum_bad_points_percentage"] = maximumBadPointsPercentage,
            });
        return InvokeOperationAsync<string>(
            "briosa.CloudAndMeshOperations",
            "RasterScanEdgeInspection",
            request,
            Transport.RasterScanEdgeInspectionResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<string> NewRasterScanEdgeInspectionAsync(
        IEnumerable<CollectionObjectName> edgeCloudNames,
        CollectionObjectName edgeSurfaceName,
        CollectionObjectName edgeBSplineName,
        CollectionObjectName outputPrefix,
        double inspectionIncrement = 0.0,
        double proximityFilterDistance = 0.0,
        double edgeBiasValue = 0.0,
        double errorTolerance = 0.0,
        bool useCosineProjectionMethod = false,
        int minimumEdgePointsPerSegment = 0,
        FileReference? intermediateCalculationResultsFile = null,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.NewRasterScanEdgeInspectionRequest(),
            new Dictionary<string, object?>
            {
                ["edge_cloud_names"] = edgeCloudNames,
                ["edge_surface_name"] = edgeSurfaceName,
                ["edge_b_spline_name"] = edgeBSplineName,
                ["output_prefix"] = outputPrefix,
                ["inspection_increment"] = inspectionIncrement,
                ["proximity_filter_distance"] = proximityFilterDistance,
                ["edge_bias_value"] = edgeBiasValue,
                ["error_tolerance"] = errorTolerance,
                ["use_cosine_projection_method"] = useCosineProjectionMethod,
                ["minimum_edge_points_per_segment"] = minimumEdgePointsPerSegment,
                ["intermediate_calculation_results_file"] = intermediateCalculationResultsFile,
            });
        return InvokeOperationAsync<string>(
            "briosa.CloudAndMeshOperations",
            "NewRasterScanEdgeInspection",
            request,
            Transport.NewRasterScanEdgeInspectionResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task ClearCloudPointDeviationsAsync(
        CollectionObjectName cloudName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ClearCloudPointDeviationsRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_name"] = cloudName,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "ClearCloudPointDeviations",
            request,
            Transport.ClearCloudPointDeviationsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task EnableAllCloudCrossSectionsAsync(
        CollectionObjectName crossSectionCloudName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EnableAllCloudCrossSectionsRequest(),
            new Dictionary<string, object?>
            {
                ["cross_section_cloud_name"] = crossSectionCloudName,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "EnableAllCloudCrossSections",
            request,
            Transport.EnableAllCloudCrossSectionsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task EnableDisableCloudCrossSectionsAsync(
        CollectionObjectName crossSectionCloudName,
        int crossSectionId = 0,
        bool enable = true,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EnableDisableCloudCrossSectionsRequest(),
            new Dictionary<string, object?>
            {
                ["cross_section_cloud_name"] = crossSectionCloudName,
                ["cross_section_id"] = crossSectionId,
                ["enable"] = enable,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "EnableDisableCloudCrossSections",
            request,
            Transport.EnableDisableCloudCrossSectionsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task EnableSingleCloudCrossSectionAsync(
        CollectionObjectName crossSectionCloudName,
        int crossSectionId = 0,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EnableSingleCloudCrossSectionRequest(),
            new Dictionary<string, object?>
            {
                ["cross_section_cloud_name"] = crossSectionCloudName,
                ["cross_section_id"] = crossSectionId,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "EnableSingleCloudCrossSection",
            request,
            Transport.EnableSingleCloudCrossSectionResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<int> GetNumberOfCrossSectionsInCrossSectionCloudAsync(
        CollectionObjectName crossSectionCloudName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetNumberOfCrossSectionsInCrossSectionCloudRequest(),
            new Dictionary<string, object?>
            {
                ["cross_section_cloud_name"] = crossSectionCloudName,
            });
        return InvokeOperationAsync<int>(
            "briosa.CloudAndMeshOperations",
            "GetNumberOfCrossSectionsInCrossSectionCloud",
            request,
            Transport.GetNumberOfCrossSectionsInCrossSectionCloudResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task FilterCloudsToPlaneAsync(
        IEnumerable<CollectionObjectName> cloudNames,
        CollectionObjectName filterPlaneName,
        CollectionObjectName outputGroupName,
        double proximity = 0.0,
        OffsetDirectionType allowableOffsetDirection = OffsetDirectionType.Both,
        PointOutputType outputType = PointOutputType.Points,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FilterCloudsToPlaneRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_names"] = cloudNames,
                ["filter_plane_name"] = filterPlaneName,
                ["output_group_name"] = outputGroupName,
                ["proximity"] = proximity,
                ["allowable_offset_direction"] = allowableOffsetDirection,
                ["output_type"] = outputType,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "FilterCloudsToPlane",
            request,
            Transport.FilterCloudsToPlaneResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task FilterCloudsToGroupAsync(
        IEnumerable<CollectionObjectName> cloudNames,
        CollectionObjectName filterGroupName,
        CollectionObjectName outputGroupName,
        double proximity = 0.0,
        int maximumNumberOfPoints = 0,
        PointOutputType outputType = PointOutputType.Points,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FilterCloudsToGroupRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_names"] = cloudNames,
                ["filter_group_name"] = filterGroupName,
                ["output_group_name"] = outputGroupName,
                ["proximity"] = proximity,
                ["maximum_number_of_points"] = maximumNumberOfPoints,
                ["output_type"] = outputType,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "FilterCloudsToGroup",
            request,
            Transport.FilterCloudsToGroupResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task FilterCloudsToSurfaceAsync(
        IEnumerable<CollectionObjectName> cloudNames,
        CollectionObjectName filterSurfaceName,
        CollectionObjectName outputGroupName,
        double lowProximity = 0.0,
        double highProximity = 0.0,
        int skipFactor = 0,
        PointOutputType outputType = PointOutputType.Points,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FilterCloudsToSurfaceRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_names"] = cloudNames,
                ["filter_surface_name"] = filterSurfaceName,
                ["output_group_name"] = outputGroupName,
                ["low_proximity"] = lowProximity,
                ["high_proximity"] = highProximity,
                ["skip_factor"] = skipFactor,
                ["output_type"] = outputType,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "FilterCloudsToSurface",
            request,
            Transport.FilterCloudsToSurfaceResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task FilterCloudsToBSplinesAsync(
        IEnumerable<CollectionObjectName> cloudNames,
        IEnumerable<CollectionObjectName> filterBSplineNames,
        CollectionObjectName outputGroupName,
        double minimumProximity = 0.0,
        double maximumProximity = 0.0,
        PointOutputType outputType = PointOutputType.Points,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FilterCloudsToBSplinesRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_names"] = cloudNames,
                ["filter_b_spline_names"] = filterBSplineNames,
                ["output_group_name"] = outputGroupName,
                ["minimum_proximity"] = minimumProximity,
                ["maximum_proximity"] = maximumProximity,
                ["output_type"] = outputType,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "FilterCloudsToBSplines",
            request,
            Transport.FilterCloudsToBSplinesResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task FilterCloudsToLineSegmentAsync(
        IEnumerable<CollectionObjectName> cloudNames,
        PointName firstLineEndPoint,
        PointName secondLineEndPoint,
        CollectionObjectName outputGroupName,
        double minimumProximity = 0.0,
        double maximumProximity = 0.0,
        PointOutputType outputType = PointOutputType.Points,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FilterCloudsToLineSegmentRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_names"] = cloudNames,
                ["first_line_end_point"] = firstLineEndPoint,
                ["second_line_end_point"] = secondLineEndPoint,
                ["output_group_name"] = outputGroupName,
                ["minimum_proximity"] = minimumProximity,
                ["maximum_proximity"] = maximumProximity,
                ["output_type"] = outputType,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "FilterCloudsToLineSegment",
            request,
            Transport.FilterCloudsToLineSegmentResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task FilterCloudsToVectorGroupsResolvePointsAsync(
        IEnumerable<CollectionObjectName> cloudNames,
        IEnumerable<CollectionObjectName> vectorGroupNames,
        CollectionObjectName outputGroupName,
        double minimumProximity = 0.0,
        double maximumProximity = 0.0,
        double maximumDistanceFromVectorBegin = 0.0,
        int minimumNumberOfRequiredPoints = 0,
        PointOutputType outputType = PointOutputType.Points,
        bool includeProximityPoints = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FilterCloudsToVectorGroupsResolvePointsRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_names"] = cloudNames,
                ["vector_group_names"] = vectorGroupNames,
                ["output_group_name"] = outputGroupName,
                ["minimum_proximity"] = minimumProximity,
                ["maximum_proximity"] = maximumProximity,
                ["maximum_distance_from_vector_begin"] = maximumDistanceFromVectorBegin,
                ["minimum_number_of_required_points"] = minimumNumberOfRequiredPoints,
                ["output_type"] = outputType,
                ["include_proximity_points"] = includeProximityPoints,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "FilterCloudsToVectorGroupsResolvePoints",
            request,
            Transport.FilterCloudsToVectorGroupsResolvePointsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> FilterCloudsToVectorGroupsResolveCloudsAsync(
        IEnumerable<CollectionObjectName> cloudNames,
        IEnumerable<CollectionObjectName> vectorGroupNames,
        string outputCollectionName,
        double radialCutoff = 0.1,
        double lowerCutoff = -0.1,
        double upperCutoff = 0.1,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FilterCloudsToVectorGroupsResolveCloudsRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_names"] = cloudNames,
                ["vector_group_names"] = vectorGroupNames,
                ["radial_cutoff"] = radialCutoff,
                ["lower_cutoff"] = lowerCutoff,
                ["upper_cutoff"] = upperCutoff,
                ["output_collection_name"] = outputCollectionName,
            });
        return InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.CloudAndMeshOperations",
            "FilterCloudsToVectorGroupsResolveClouds",
            request,
            Transport.FilterCloudsToVectorGroupsResolveCloudsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task RGBCloudPointFilterAsync(
        IEnumerable<CollectionObjectName> cloudsToBeFiltered,
        string filterName = "Default Filter",
        bool redEnabled = true,
        bool redHighEnabled = false,
        int redHighThreshold = 255,
        bool redLowEnabled = false,
        int redLowThreshold = 0,
        bool greenEnabled = true,
        bool greenHighEnabled = false,
        int greenHighThreshold = 255,
        bool greenLowEnabled = false,
        int greenLowThreshold = 0,
        bool blueEnabled = true,
        bool blueHighEnabled = false,
        int blueHighThreshold = 255,
        bool blueLowEnabled = false,
        int blueLowThreshold = 0,
        bool grayScaleEnabled = false,
        bool grayScaleHighEnabled = false,
        int grayScaleHighThreshold = 255,
        bool grayScaleLowEnabled = false,
        int grayScaleLowThreshold = 0,
        RGBFilterOperation rgbFilterOperation = RGBFilterOperation.ResetAndApplyFilter,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RGBCloudPointFilterRequest(),
            new Dictionary<string, object?>
            {
                ["filter_name"] = filterName,
                ["clouds_to_be_filtered"] = cloudsToBeFiltered,
                ["red_enabled"] = redEnabled,
                ["red_high_enabled"] = redHighEnabled,
                ["red_high_threshold"] = redHighThreshold,
                ["red_low_enabled"] = redLowEnabled,
                ["red_low_threshold"] = redLowThreshold,
                ["green_enabled"] = greenEnabled,
                ["green_high_enabled"] = greenHighEnabled,
                ["green_high_threshold"] = greenHighThreshold,
                ["green_low_enabled"] = greenLowEnabled,
                ["green_low_threshold"] = greenLowThreshold,
                ["blue_enabled"] = blueEnabled,
                ["blue_high_enabled"] = blueHighEnabled,
                ["blue_high_threshold"] = blueHighThreshold,
                ["blue_low_enabled"] = blueLowEnabled,
                ["blue_low_threshold"] = blueLowThreshold,
                ["gray_scale_enabled"] = grayScaleEnabled,
                ["gray_scale_high_enabled"] = grayScaleHighEnabled,
                ["gray_scale_high_threshold"] = grayScaleHighThreshold,
                ["gray_scale_low_enabled"] = grayScaleLowEnabled,
                ["gray_scale_low_threshold"] = grayScaleLowThreshold,
                ["rgb_filter_operation"] = rgbFilterOperation,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "RGBCloudPointFilter",
            request,
            Transport.RGBCloudPointFilterResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<GetCloudRGBValuesResult> GetCloudRGBValuesAsync(
        CollectionObjectName sourceCloudName,
        RGBColorChannel rgbColorChannel = RGBColorChannel.Intensity,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCloudRGBValuesRequest(),
            new Dictionary<string, object?>
            {
                ["source_cloud_name"] = sourceCloudName,
                ["rgb_color_channel"] = rgbColorChannel,
            });
        return InvokeOperationAsync<GetCloudRGBValuesResult>(
            "briosa.CloudAndMeshOperations",
            "GetCloudRGBValues",
            request,
            Transport.GetCloudRGBValuesResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<GetCloudRGBValuesNearPointResult> GetCloudRGBValuesNearPointAsync(
        CollectionObjectName sourceCloudName,
        PointName singlePoint,
        double diameter = 10.0,
        RGBColorChannel rgbColorChannel = RGBColorChannel.Intensity,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCloudRGBValuesNearPointRequest(),
            new Dictionary<string, object?>
            {
                ["source_cloud_name"] = sourceCloudName,
                ["single_point"] = singlePoint,
                ["diameter"] = diameter,
                ["rgb_color_channel"] = rgbColorChannel,
            });
        return InvokeOperationAsync<GetCloudRGBValuesNearPointResult>(
            "briosa.CloudAndMeshOperations",
            "GetCloudRGBValuesNearPoint",
            request,
            Transport.GetCloudRGBValuesNearPointResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task SubdivideCloudByPointSpacingAsync(
        CollectionObjectName sourceCloudName,
        CollectionObjectName newCloudName,
        double pointSpacing = 0.0,
        int minimumPointsPerGroup = 0,
        bool keepAllGroups = true,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SubdivideCloudByPointSpacingRequest(),
            new Dictionary<string, object?>
            {
                ["source_cloud_name"] = sourceCloudName,
                ["point_spacing"] = pointSpacing,
                ["minimum_points_per_group"] = minimumPointsPerGroup,
                ["new_cloud_name"] = newCloudName,
                ["keep_all_groups"] = keepAllGroups,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "SubdivideCloudByPointSpacing",
            request,
            Transport.SubdivideCloudByPointSpacingResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task DeleteCloudPointsByRadialDistanceFromPointsAsync(
        IEnumerable<CollectionObjectName> cloudNames,
        IEnumerable<PointName> points,
        double radius = 0.0,
        bool deleteInside = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteCloudPointsByRadialDistanceFromPointsRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_names"] = cloudNames,
                ["points"] = points,
                ["radius"] = radius,
                ["delete_inside"] = deleteInside,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "DeleteCloudPointsByRadialDistanceFromPoints",
            request,
            Transport.DeleteCloudPointsByRadialDistanceFromPointsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task GenerateGeneralMeshAsync(
        CollectionObjectName outputMeshName,
        IEnumerable<CollectionObjectName> cloudsToMesh,
        double maximumTriangleSize = 0.05,
        double smallestHoleDiameter = 0.25,
        bool finalize = true,
        bool useScanDirectionForPointNormal = true,
        FileReference? jsonFile = null,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GenerateGeneralMeshRequest(),
            new Dictionary<string, object?>
            {
                ["output_mesh_name"] = outputMeshName,
                ["clouds_to_mesh"] = cloudsToMesh,
                ["maximum_triangle_size"] = maximumTriangleSize,
                ["smallest_hole_diameter"] = smallestHoleDiameter,
                ["finalize"] = finalize,
                ["use_scan_direction_for_point_normal"] = useScanDirectionForPointNormal,
                ["json_file"] = jsonFile,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "GenerateGeneralMesh",
            request,
            Transport.GenerateGeneralMeshResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task ConsolidateMeshAsync(
        CollectionObjectName mesh,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConsolidateMeshRequest(),
            new Dictionary<string, object?>
            {
                ["mesh"] = mesh,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "ConsolidateMesh",
            request,
            Transport.ConsolidateMeshResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<MeshVolumeResult> MeshVolumeAsync(
        CollectionObjectName mesh,
        CollectionObjectName plane,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MeshVolumeRequest(),
            new Dictionary<string, object?>
            {
                ["mesh"] = mesh,
                ["plane"] = plane,
            });
        return InvokeOperationAsync<MeshVolumeResult>(
            "briosa.CloudAndMeshOperations",
            "MeshVolume",
            request,
            Transport.MeshVolumeResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MeshFillHolesAsync(
        CollectionObjectName mesh,
        double maximumTriangleLength = -1.0,
        double tension = 0.0,
        bool unconditionalFilling = false,
        bool fillAllHoles = true,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MeshFillHolesRequest(),
            new Dictionary<string, object?>
            {
                ["mesh"] = mesh,
                ["maximum_triangle_length"] = maximumTriangleLength,
                ["tension"] = tension,
                ["unconditional_filling"] = unconditionalFilling,
                ["fill_all_holes"] = fillAllHoles,
            });
        return InvokeOperationAsync(
            "briosa.CloudAndMeshOperations",
            "MeshFillHoles",
            request,
            Transport.MeshFillHolesResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task CopyObjectAsync(
        CollectionObjectName sourceObject,
        CollectionObjectName newObjectName,
        bool overwriteIfExists = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CopyObjectRequest(),
            new Dictionary<string, object?>
            {
                ["source_object"] = sourceObject,
                ["new_object_name"] = newObjectName,
                ["overwrite_if_exists"] = overwriteIfExists,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "CopyObject",
            request,
            Transport.CopyObjectResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task CopyObjectsPointToPointDeltaAsync(
        IEnumerable<CollectionObjectName> objectsToCopy,
        PointName firstDeltaPoint,
        PointName secondDeltaPoint,
        CollectionName? destinationCollectionName = null,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CopyObjectsPointToPointDeltaRequest(),
            new Dictionary<string, object?>
            {
                ["objects_to_copy"] = objectsToCopy,
                ["first_delta_point"] = firstDeltaPoint,
                ["second_delta_point"] = secondDeltaPoint,
                ["destination_collection_name"] = destinationCollectionName,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "CopyObjectsPointToPointDelta",
            request,
            Transport.CopyObjectsPointToPointDeltaResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task CopyObjectsToACollectionAsync(
        IEnumerable<CollectionObjectName> sourceObjects,
        CollectionName destinationCollectionName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CopyObjectsToACollectionRequest(),
            new Dictionary<string, object?>
            {
                ["source_objects"] = sourceObjects,
                ["destination_collection_name"] = destinationCollectionName,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "CopyObjectsToACollection",
            request,
            Transport.CopyObjectsToACollectionResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task DeletePointsAsync(
        IEnumerable<PointName> pointNames,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeletePointsRequest(),
            new Dictionary<string, object?>
            {
                ["point_names"] = pointNames,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "DeletePoints",
            request,
            Transport.DeletePointsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task DeletePointsWildcardSelectionAsync(
        IEnumerable<CollectionObjectName> groupsToDeleteFrom,
        PointName wildcardSelectionNames,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeletePointsWildcardSelectionRequest(),
            new Dictionary<string, object?>
            {
                ["groups_to_delete_from"] = groupsToDeleteFrom,
                ["wildcard_selection_names"] = wildcardSelectionNames,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "DeletePointsWildcardSelection",
            request,
            Transport.DeletePointsWildcardSelectionResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MoveObjectsPointToPointDeltaAsync(
        IEnumerable<CollectionObjectName> objectsToMove,
        PointName firstDeltaPoint,
        PointName secondDeltaPoint,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveObjectsPointToPointDeltaRequest(),
            new Dictionary<string, object?>
            {
                ["objects_to_move"] = objectsToMove,
                ["first_delta_point"] = firstDeltaPoint,
                ["second_delta_point"] = secondDeltaPoint,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "MoveObjectsPointToPointDelta",
            request,
            Transport.MoveObjectsPointToPointDeltaResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MoveObjectsToACollectionAsync(
        IEnumerable<CollectionObjectName> sourceObjects,
        CollectionName destinationCollectionName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveObjectsToACollectionRequest(),
            new Dictionary<string, object?>
            {
                ["source_objects"] = sourceObjects,
                ["destination_collection_name"] = destinationCollectionName,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "MoveObjectsToACollection",
            request,
            Transport.MoveObjectsToACollectionResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task RenameCollectionAsync(
        CollectionName originalCollectionName,
        CollectionName newCollectionName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RenameCollectionRequest(),
            new Dictionary<string, object?>
            {
                ["original_collection_name"] = originalCollectionName,
                ["new_collection_name"] = newCollectionName,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "RenameCollection",
            request,
            Transport.RenameCollectionResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task RenameItemAsync(
        CollectionItemName originalItemName,
        CollectionItemName newItemName,
        bool overwriteIfExists = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RenameItemRequest(),
            new Dictionary<string, object?>
            {
                ["original_item_name"] = originalItemName,
                ["new_item_name"] = newItemName,
                ["overwrite_if_exists"] = overwriteIfExists,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "RenameItem",
            request,
            Transport.RenameItemResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task RenameObjectAsync(
        CollectionObjectName originalObjectName,
        CollectionObjectName newObjectName,
        bool overwriteIfExists = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RenameObjectRequest(),
            new Dictionary<string, object?>
            {
                ["original_object_name"] = originalObjectName,
                ["new_object_name"] = newObjectName,
                ["overwrite_if_exists"] = overwriteIfExists,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "RenameObject",
            request,
            Transport.RenameObjectResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task RenamePointAsync(
        PointName originalPointName,
        PointName newPointName,
        bool overwriteIfExists = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RenamePointRequest(),
            new Dictionary<string, object?>
            {
                ["original_point_name"] = originalPointName,
                ["new_point_name"] = newPointName,
                ["overwrite_if_exists"] = overwriteIfExists,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "RenamePoint",
            request,
            Transport.RenamePointResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task RenamePointsWithNamePatternAsync(
        IEnumerable<PointName> pointNames,
        string namePattern = "NewName_%d",
        int startValue = 1,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RenamePointsWithNamePatternRequest(),
            new Dictionary<string, object?>
            {
                ["point_names"] = pointNames,
                ["name_pattern"] = namePattern,
                ["start_value"] = startValue,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "RenamePointsWithNamePattern",
            request,
            Transport.RenamePointsWithNamePatternResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task ConstructObjectsFromSurfaceFacesRuntimeSelectAsync(
        ConstructObjectType objectType,
        double pointOffset = 0.0,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructObjectsFromSurfaceFacesRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["object_type"] = objectType,
                ["point_offset"] = pointOffset,
            });
        return InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructObjectsFromSurfaceFacesRuntimeSelect",
            request,
            Transport.ConstructObjectsFromSurfaceFacesRuntimeSelectResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task AutoFilterCloudsToNominalGeometry2DAsync(
        IEnumerable<CollectionItemName> autoFilterTargetRelationships,
        IEnumerable<CollectionObjectName> clouds,
        FilterProximitySettings filterProximitySettings2D,
        CloudThinningOptions? cloudThinningSettings = null,
        double geometryExtractionTolerance = 0.01,
        bool useFeatureSpecificFilterSettings = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoFilterCloudsToNominalGeometry2DRequest(),
            new Dictionary<string, object?>
            {
                ["auto_filter_target_relationships"] = autoFilterTargetRelationships,
                ["clouds"] = clouds,
                ["cloud_thinning_settings"] = cloudThinningSettings,
                ["filter_proximity_settings_2d"] = filterProximitySettings2D,
                ["geometry_extraction_tolerance"] = geometryExtractionTolerance,
                ["use_feature_specific_filter_settings"] = useFeatureSpecificFilterSettings,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "AutoFilterCloudsToNominalGeometry2D",
            request,
            Transport.AutoFilterCloudsToNominalGeometry2DResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task AutoFilterCloudsToNominalGeometry3DAsync(
        IEnumerable<CollectionItemName> autoFilterTargetRelationships,
        IEnumerable<CollectionObjectName> clouds,
        FilterProximitySettings filterProximitySettings3D,
        CloudThinningOptions? cloudThinningSettings = null,
        bool useFeatureSpecificFilterSettings = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoFilterCloudsToNominalGeometry3DRequest(),
            new Dictionary<string, object?>
            {
                ["auto_filter_target_relationships"] = autoFilterTargetRelationships,
                ["clouds"] = clouds,
                ["cloud_thinning_settings"] = cloudThinningSettings,
                ["filter_proximity_settings_3d"] = filterProximitySettings3D,
                ["use_feature_specific_filter_settings"] = useFeatureSpecificFilterSettings,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "AutoFilterCloudsToNominalGeometry3D",
            request,
            Transport.AutoFilterCloudsToNominalGeometry3DResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task AutoFilterPointsGroupsCloudsToSurfaceFacesAsync(
        IEnumerable<CollectionObjectName> surfaces,
        IEnumerable<PointName> points,
        IEnumerable<CollectionObjectName> groups,
        IEnumerable<CollectionObjectName> clouds,
        OffsetDirectionType offsetDirection,
        double surfaceOffset = 0.1,
        double edgeOffset = 0.1,
        bool enforceMaxPointsPerFaceInOutput = false,
        int maxPointsPerFace = 0,
        CloudThinningOptions? cloudThinningSettings = null,
        string outputCloudBaseName = "InspAutoFilteredCloud",
        bool useFaceIdsForSuffix = true,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoFilterPointsGroupsCloudsToSurfaceFacesRequest(),
            new Dictionary<string, object?>
            {
                ["points"] = points,
                ["groups"] = groups,
                ["clouds"] = clouds,
                ["surface_offset"] = surfaceOffset,
                ["edge_offset"] = edgeOffset,
                ["offset_direction"] = offsetDirection,
                ["enforce_max_points_per_face_in_output"] = enforceMaxPointsPerFaceInOutput,
                ["max_points_per_face"] = maxPointsPerFace,
                ["surfaces"] = surfaces,
                ["cloud_thinning_settings"] = cloudThinningSettings,
                ["output_cloud_base_name"] = outputCloudBaseName,
                ["use_face_ids_for_suffix"] = useFaceIdsForSuffix,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "AutoFilterPointsGroupsCloudsToSurfaceFaces",
            request,
            Transport.AutoFilterPointsGroupsCloudsToSurfaceFacesResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task AutoFilterPointsToNominalGeometry3DAsync(
        IEnumerable<CollectionItemName> autoFilterTargetRelationships,
        IEnumerable<PointName> points,
        FilterProximitySettings filterProximitySettings3D,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoFilterPointsToNominalGeometry3DRequest(),
            new Dictionary<string, object?>
            {
                ["auto_filter_target_relationships"] = autoFilterTargetRelationships,
                ["points"] = points,
                ["filter_proximity_settings_3d"] = filterProximitySettings3D,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "AutoFilterPointsToNominalGeometry3D",
            request,
            Transport.AutoFilterPointsToNominalGeometry3DResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task ComputeGeometryRelationshipUncertaintiesAsync(
        CollectionItemName relationshipName,
        bool displayResults = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ComputeGeometryRelationshipUncertaintiesRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["display_results"] = displayResults,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "ComputeGeometryRelationshipUncertainties",
            request,
            Transport.ComputeGeometryRelationshipUncertaintiesResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task CreatePointsToObjectsMapAsync(
        string pointsToObjectsMapName,
        IEnumerable<CollectionObjectName> objects,
        IEnumerable<PointName> points,
        IEnumerable<CollectionObjectName> groups,
        double proximityTolerance = 0.0,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreatePointsToObjectsMapRequest(),
            new Dictionary<string, object?>
            {
                ["points"] = points,
                ["groups"] = groups,
                ["objects"] = objects,
                ["proximity_tolerance"] = proximityTolerance,
                ["points_to_objects_map_name"] = pointsToObjectsMapName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "CreatePointsToObjectsMap",
            request,
            Transport.CreatePointsToObjectsMapResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task DeleteRelationshipAsync(
        CollectionItemName relationshipName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "DeleteRelationship",
            request,
            Transport.DeleteRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<RelationshipFitResult> DoRelationshipFitAsync(
        string collectionContainingRelationships,
        IEnumerable<CollectionObjectName> objectsToMove,
        IEnumerable<CollectionInstrumentId> instrumentsToMove,
        FitDofOptions motionToAllow,
        SolverMode solverMode = SolverMode.GaussNewton,
        bool enableRandomizedStart = false,
        bool useFitDialog = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DoRelationshipFitRequest(),
            new Dictionary<string, object?>
            {
                ["collection_containing_relationships"] = collectionContainingRelationships,
                ["objects_to_move"] = objectsToMove,
                ["instruments_to_move"] = instrumentsToMove,
                ["solver_mode"] = solverMode,
                ["motion_to_allow"] = motionToAllow,
                ["enable_randomized_start"] = enableRandomizedStart,
                ["use_fit_dialog"] = useFitDialog,
            });
        return InvokeOperationAsync<RelationshipFitResult>(
            "briosa.RelationshipOperations",
            "DoRelationshipFit",
            request,
            Transport.DoRelationshipFitResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task ExtractGeometryFromPointCloudsAsync(
        CollectionItemName relationshipName,
        CollectionObjectName cloudName,
        IEnumerable<PointName> seedPoints,
        GeometryType geometryType,
        IEnumerable<PointName> boundingPoints,
        double tolerance = 0.1,
        bool reverseNormal = false,
        int planarPointCount = 1000,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExtractGeometryFromPointCloudsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["geometry_type"] = geometryType,
                ["cloud_name"] = cloudName,
                ["bounding_points"] = boundingPoints,
                ["seed_points"] = seedPoints,
                ["tolerance"] = tolerance,
                ["reverse_normal"] = reverseNormal,
                ["planar_point_count"] = planarPointCount,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "ExtractGeometryFromPointClouds",
            request,
            Transport.ExtractGeometryFromPointCloudsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<GeometryRelationshipOutlierFilterMetrics> FilterGeometryRelationshipOutlierCloudPointsAsync(
        CollectionItemName relationshipName,
        double sigmaThreshold = 3.0,
        bool modifyExistingInputClouds = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FilterGeometryRelationshipOutlierCloudPointsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["sigma_threshold"] = sigmaThreshold,
                ["modify_existing_input_clouds"] = modifyExistingInputClouds,
            });
        return InvokeOperationAsync<GeometryRelationshipOutlierFilterMetrics>(
            "briosa.RelationshipOperations",
            "FilterGeometryRelationshipOutlierCloudPoints",
            request,
            Transport.FilterGeometryRelationshipOutlierCloudPointsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task GenerateGeometryRelationshipSummaryAsync(
        IEnumerable<CollectionItemName> relationshipRefList,
        string summaryTableName = "Geometry Relationship Summary",
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GenerateGeometryRelationshipSummaryRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_ref_list"] = relationshipRefList,
                ["summary_table_name"] = summaryTableName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "GenerateGeometryRelationshipSummary",
            request,
            Transport.GenerateGeometryRelationshipSummaryResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<GeneralRelationshipStatistics> GetGeneralRelationshipStatisticsAsync(
        CollectionItemName relationshipName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGeneralRelationshipStatisticsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<GeneralRelationshipStatistics>(
            "briosa.RelationshipOperations",
            "GetGeneralRelationshipStatistics",
            request,
            Transport.GetGeneralRelationshipStatisticsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<IReadOnlyList<string>> GetGeomRelationshipCriteriaNameListAsync(
        CollectionItemName relationshipName,
        bool includeAllCriteria = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGeomRelationshipCriteriaNameListRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["include_all_criteria"] = includeAllCriteria,
            });
        return InvokeOperationAsync<IReadOnlyList<string>>(
            "briosa.RelationshipOperations",
            "GetGeomRelationshipCriteriaNameList",
            request,
            Transport.GetGeomRelationshipCriteriaNameListResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> GetObjectsFromPointsToObjectsMapPointListAsync(
        string pointsToObjectsMapName,
        IEnumerable<PointName> points,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetObjectsFromPointsToObjectsMapPointListRequest(),
            new Dictionary<string, object?>
            {
                ["points_to_objects_map_name"] = pointsToObjectsMapName,
                ["points"] = points,
            });
        return InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.RelationshipOperations",
            "GetObjectsFromPointsToObjectsMapPointList",
            request,
            Transport.GetObjectsFromPointsToObjectsMapPointListResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<PointToPointRelationshipStatistics> GetPointToPointRelationshipStatisticsAsync(
        CollectionItemName relationshipName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointToPointRelationshipStatisticsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<PointToPointRelationshipStatistics>(
            "briosa.RelationshipOperations",
            "GetPointToPointRelationshipStatistics",
            request,
            Transport.GetPointToPointRelationshipStatisticsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<PointsToObjectsRelationshipStatistics> GetPointsToObjectsRelationshipStatisticsAsync(
        CollectionItemName relationshipName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointsToObjectsRelationshipStatisticsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<PointsToObjectsRelationshipStatistics>(
            "briosa.RelationshipOperations",
            "GetPointsToObjectsRelationshipStatistics",
            request,
            Transport.GetPointsToObjectsRelationshipStatisticsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<PointsToPointsRelationshipAssociatedData> GetPointsToPointsRelationshipAssociatedDataAsync(
        CollectionItemName relationshipName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPointsToPointsRelationshipAssociatedDataRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<PointsToPointsRelationshipAssociatedData>(
            "briosa.RelationshipOperations",
            "GetPointsToPointsRelationshipAssociatedData",
            request,
            Transport.GetPointsToPointsRelationshipAssociatedDataResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<RelationshipAssociatedData> GetRelationshipAssociatedDataAsync(
        CollectionItemName relationshipName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipAssociatedDataRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<RelationshipAssociatedData>(
            "briosa.RelationshipOperations",
            "GetRelationshipAssociatedData",
            request,
            Transport.GetRelationshipAssociatedDataResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<RelationshipStatusFlags> GetRelationshipStatusAsync(
        CollectionItemName relationshipName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipStatusRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<RelationshipStatusFlags>(
            "briosa.RelationshipOperations",
            "GetRelationshipStatus",
            request,
            Transport.GetRelationshipStatusResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeAveragePointRelationshipAsync(
        CollectionItemName relationshipName,
        IEnumerable<PointName> pointsInRelationship,
        PointName? averagePointName = null,
        PointName? nominalPointName = null,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeAveragePointRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["points_in_relationship"] = pointsInRelationship,
                ["average_point_name"] = averagePointName,
                ["nominal_point_name"] = nominalPointName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeAveragePointRelationship",
            request,
            Transport.MakeAveragePointRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeCloudToSwatchRelationshipAsync(
        CollectionItemName relationshipName,
        CollectionObjectName inputCloudName,
        string surfaceFaceList,
        PointName referencePoint,
        CollectionObjectName cardinalPointGroupName,
        double maximumRadialOffset = 0.125,
        double minimumAxialOffset = -0.125,
        double maximumAxialOffset = 0.125,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCloudToSwatchRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["input_cloud_name"] = inputCloudName,
                ["surface_face_list"] = surfaceFaceList,
                ["reference_point"] = referencePoint,
                ["maximum_radial_offset"] = maximumRadialOffset,
                ["minimum_axial_offset"] = minimumAxialOffset,
                ["maximum_axial_offset"] = maximumAxialOffset,
                ["cardinal_point_group_name"] = cardinalPointGroupName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeCloudToSwatchRelationship",
            request,
            Transport.MakeCloudToSwatchRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeDynamicCircleRelationshipAsync(
        CollectionItemName relationshipName,
        CollectionObjectName firstReferenceGeometry,
        CollectionObjectName secondReferenceGeometry,
        DynamicCircleMode constructionMode,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeDynamicCircleRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["construction_mode"] = constructionMode,
                ["first_reference_geometry"] = firstReferenceGeometry,
                ["second_reference_geometry"] = secondReferenceGeometry,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeDynamicCircleRelationship",
            request,
            Transport.MakeDynamicCircleRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeDynamicEllipseRelationshipAsync(
        CollectionItemName relationshipName,
        CollectionObjectName firstReferenceGeometry,
        CollectionObjectName secondReferenceGeometry,
        DynamicEllipseMode constructionMode,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeDynamicEllipseRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["construction_mode"] = constructionMode,
                ["first_reference_geometry"] = firstReferenceGeometry,
                ["second_reference_geometry"] = secondReferenceGeometry,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeDynamicEllipseRelationship",
            request,
            Transport.MakeDynamicEllipseRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeDynamicLineRelationshipAsync(
        CollectionItemName relationshipName,
        CollectionObjectName firstReferenceGeometry,
        CollectionObjectName secondReferenceGeometry,
        DynamicLineMode constructionMode,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeDynamicLineRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["construction_mode"] = constructionMode,
                ["first_reference_geometry"] = firstReferenceGeometry,
                ["second_reference_geometry"] = secondReferenceGeometry,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeDynamicLineRelationship",
            request,
            Transport.MakeDynamicLineRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeDynamicPlaneRelationshipAsync(
        CollectionItemName relationshipName,
        CollectionObjectName firstReferenceGeometry,
        CollectionObjectName secondReferenceGeometry,
        DynamicPlaneMode constructionMode,
        double offsetPlaneOffset = 0.0,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeDynamicPlaneRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["construction_mode"] = constructionMode,
                ["first_reference_geometry"] = firstReferenceGeometry,
                ["second_reference_geometry"] = secondReferenceGeometry,
                ["offset_plane_offset"] = offsetPlaneOffset,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeDynamicPlaneRelationship",
            request,
            Transport.MakeDynamicPlaneRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeDynamicPointRelationshipAsync(
        CollectionItemName relationshipName,
        CollectionObjectName firstReferenceGeometry,
        CollectionObjectName secondReferenceGeometry,
        DynamicPointMode constructionMode,
        CollectionObjectName thirdReferenceGeometry,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeDynamicPointRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["construction_mode"] = constructionMode,
                ["first_reference_geometry"] = firstReferenceGeometry,
                ["second_reference_geometry"] = secondReferenceGeometry,
                ["third_reference_geometry"] = thirdReferenceGeometry,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeDynamicPointRelationship",
            request,
            Transport.MakeDynamicPointRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeFrameToFrameRelationshipAsync(
        CollectionItemName relationshipName,
        CollectionObjectName firstFrameName,
        CollectionObjectName secondFrameName,
        ToleranceScalarOptions orientationTolerance,
        ToleranceVectorOptions positionTolerance,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeFrameToFrameRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["first_frame_name"] = firstFrameName,
                ["second_frame_name"] = secondFrameName,
                ["orientation_tolerance"] = orientationTolerance,
                ["position_tolerance"] = positionTolerance,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeFrameToFrameRelationship",
            request,
            Transport.MakeFrameToFrameRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeGeometryCompareOnlyRelationshipAsync(
        CollectionItemName relationshipName,
        CollectionObjectName nominalGeometry,
        CollectionObjectName measuredGeometry,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeGeometryCompareOnlyRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["nominal_geometry"] = nominalGeometry,
                ["measured_geometry"] = measuredGeometry,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeGeometryCompareOnlyRelationship",
            request,
            Transport.MakeGeometryCompareOnlyRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeGeometryFitAndCompareToNominalRelationshipAsync(
        CollectionItemName relationshipName,
        CollectionObjectName nominalGeometry,
        IEnumerable<CollectionObjectName> pointGroupsToFit,
        CollectionObjectName? resultingObjectName = null,
        string? fitProfileName = null,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeGeometryFitAndCompareToNominalRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["nominal_geometry"] = nominalGeometry,
                ["point_groups_to_fit"] = pointGroupsToFit,
                ["resulting_object_name"] = resultingObjectName,
                ["fit_profile_name"] = fitProfileName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeGeometryFitAndCompareToNominalRelationship",
            request,
            Transport.MakeGeometryFitAndCompareToNominalRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeGeometryFitOnlyRelationshipAsync(
        CollectionItemName relationshipName,
        IEnumerable<CollectionObjectName> pointGroupsToFit,
        GeometryType geometryType,
        CollectionObjectName? resultingObjectName = null,
        string? fitProfileName = null,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeGeometryFitOnlyRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["point_groups_to_fit"] = pointGroupsToFit,
                ["geometry_type"] = geometryType,
                ["resulting_object_name"] = resultingObjectName,
                ["fit_profile_name"] = fitProfileName,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeGeometryFitOnlyRelationship",
            request,
            Transport.MakeGeometryFitOnlyRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeGroupToGroupRelationshipAsync(
        CollectionItemName relationshipName,
        CollectionObjectName firstGroupName,
        CollectionObjectName secondGroupName,
        ToleranceVectorOptions tolerance,
        ToleranceVectorOptions constraint,
        bool autoUpdateAVectorGroup = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeGroupToGroupRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["first_group_name"] = firstGroupName,
                ["second_group_name"] = secondGroupName,
                ["auto_update_a_vector_group"] = autoUpdateAVectorGroup,
                ["tolerance"] = tolerance,
                ["constraint"] = constraint,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeGroupToGroupRelationship",
            request,
            Transport.MakeGroupToGroupRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeGroupToNominalGroupRelationshipAsync(
        CollectionItemName relationshipName,
        CollectionObjectName nominalGroupName,
        CollectionObjectName measuredGroupName,
        ToleranceVectorOptions tolerance,
        ToleranceVectorOptions constraint,
        bool autoUpdateAVectorGroup = false,
        bool useClosestPoint = true,
        bool displayClosestPointWatchWindow = false,
        bool useViewZoomingWithProximity = false,
        bool ignorePointsBeyondThreshold = false,
        double proximityThreshold = 0.01,
        double fitWeight = 1.0,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeGroupToNominalGroupRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["nominal_group_name"] = nominalGroupName,
                ["measured_group_name"] = measuredGroupName,
                ["auto_update_a_vector_group"] = autoUpdateAVectorGroup,
                ["use_closest_point"] = useClosestPoint,
                ["display_closest_point_watch_window"] = displayClosestPointWatchWindow,
                ["use_view_zooming_with_proximity"] = useViewZoomingWithProximity,
                ["ignore_points_beyond_threshold"] = ignorePointsBeyondThreshold,
                ["proximity_threshold"] = proximityThreshold,
                ["tolerance"] = tolerance,
                ["constraint"] = constraint,
                ["fit_weight"] = fitWeight,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeGroupToNominalGroupRelationship",
            request,
            Transport.MakeGroupToNominalGroupRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeGroupsToObjectsRelationshipAsync(
        CollectionItemName relationshipName,
        IEnumerable<CollectionObjectName> pointGroupsInRelationship,
        IEnumerable<CollectionObjectName> objectsInRelationship,
        ProjectionOptions projectionOptions,
        bool autoUpdateAVectorGroup = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeGroupsToObjectsRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["point_groups_in_relationship"] = pointGroupsInRelationship,
                ["objects_in_relationship"] = objectsInRelationship,
                ["projection_options"] = projectionOptions,
                ["auto_update_a_vector_group"] = autoUpdateAVectorGroup,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeGroupsToObjectsRelationship",
            request,
            Transport.MakeGroupsToObjectsRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeObjectToObjectDirectionRelationshipAsync(
        CollectionItemName relationshipName,
        CollectionObjectName firstObjectInRelationship,
        CollectionObjectName secondObjectInRelationship,
        double nominalAngle = 0.0,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeObjectToObjectDirectionRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["first_object_in_relationship"] = firstObjectInRelationship,
                ["second_object_in_relationship"] = secondObjectInRelationship,
                ["nominal_angle"] = nominalAngle,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeObjectToObjectDirectionRelationship",
            request,
            Transport.MakeObjectToObjectDirectionRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakePointCloudsToObjectsRelationshipAsync(
        CollectionItemName relationshipName,
        IEnumerable<CollectionObjectName> pointCloudsInRelationship,
        IEnumerable<CollectionObjectName> objectsInRelationship,
        ProjectionOptions projectionOptions,
        bool autoUpdateAVectorGroup = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePointCloudsToObjectsRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["point_clouds_in_relationship"] = pointCloudsInRelationship,
                ["objects_in_relationship"] = objectsInRelationship,
                ["projection_options"] = projectionOptions,
                ["auto_update_a_vector_group"] = autoUpdateAVectorGroup,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakePointCloudsToObjectsRelationship",
            request,
            Transport.MakePointCloudsToObjectsRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakePointToPointRelationshipAsync(
        CollectionItemName relationshipName,
        PointName firstPointName,
        PointName secondPointName,
        ToleranceVectorOptions tolerance,
        ToleranceVectorOptions constraint,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePointToPointRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["first_point_name"] = firstPointName,
                ["second_point_name"] = secondPointName,
                ["tolerance"] = tolerance,
                ["constraint"] = constraint,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakePointToPointRelationship",
            request,
            Transport.MakePointToPointRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakePointsToObjectsRelationshipAsync(
        CollectionItemName relationshipName,
        IEnumerable<PointName> pointsInRelationship,
        IEnumerable<CollectionObjectName> objectsInRelationship,
        ProjectionOptions projectionOptions,
        bool autoUpdateAVectorGroup = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePointsToObjectsRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["points_in_relationship"] = pointsInRelationship,
                ["objects_in_relationship"] = objectsInRelationship,
                ["projection_options"] = projectionOptions,
                ["auto_update_a_vector_group"] = autoUpdateAVectorGroup,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakePointsToObjectsRelationship",
            request,
            Transport.MakePointsToObjectsRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakePointsToPointsRelationshipAsync(
        CollectionItemName relationshipName,
        IEnumerable<PointName> nominalPoints,
        IEnumerable<PointName> measuredPoints,
        ToleranceVectorOptions tolerance,
        ToleranceVectorOptions constraint,
        bool autoUpdateAVectorGroup = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePointsToPointsRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["nominal_points"] = nominalPoints,
                ["measured_points"] = measuredPoints,
                ["auto_update_a_vector_group"] = autoUpdateAVectorGroup,
                ["tolerance"] = tolerance,
                ["constraint"] = constraint,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakePointsToPointsRelationship",
            request,
            Transport.MakePointsToPointsRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MakeVectorGroupToVectorGroupRelationshipAsync(
        CollectionItemName newVgToVgRelationship,
        CollectionObjectName referenceVectorGroup,
        CollectionObjectName correspondingVectorGroup,
        bool setOpposingVectorGroupPolarity = true,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeVectorGroupToVectorGroupRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["new_vg_to_vg_relationship"] = newVgToVgRelationship,
                ["reference_vector_group"] = referenceVectorGroup,
                ["corresponding_vector_group"] = correspondingVectorGroup,
                ["set_opposing_vector_group_polarity"] = setOpposingVectorGroupPolarity,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MakeVectorGroupToVectorGroupRelationship",
            request,
            Transport.MakeVectorGroupToVectorGroupRelationshipResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task MoveCollectionsByMinimizingRelationshipsAsync(
        IEnumerable<string> collectionsToMove,
        IEnumerable<CollectionItemName> relationshipsToMinimize,
        FitDofOptions motionToAllow,
        SolverMode solverMode = SolverMode.GaussNewton,
        bool useFitDialog = false,
        double convergenceThreshold = 0.0,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveCollectionsByMinimizingRelationshipsRequest(),
            new Dictionary<string, object?>
            {
                ["collections_to_move"] = collectionsToMove,
                ["relationships_to_minimize"] = relationshipsToMinimize,
                ["solver_mode"] = solverMode,
                ["motion_to_allow"] = motionToAllow,
                ["use_fit_dialog"] = useFitDialog,
                ["convergence_threshold"] = convergenceThreshold,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "MoveCollectionsByMinimizingRelationships",
            request,
            Transport.MoveCollectionsByMinimizingRelationshipsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    /// <remarks>
    /// <c>showDeviationX</c>: MP qualifier: Rx.<br/>
    /// <c>showDeviationY</c>: MP qualifier: Ry.<br/>
    /// <c>showDeviationZ</c>: MP qualifier: Rz.<br/>
    /// </remarks>
    public Task RelationshipWatchWindowTemplateAsync(
        CollectionObjectName watchWindowTemplateName,
        RelationshipWatchWindowUdpSettings udpNetworkTransmitSettings,
        RelationshipWatchWindowTemplateOptions? options = null,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        options ??= new RelationshipWatchWindowTemplateOptions();
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RelationshipWatchWindowTemplateRequest(),
            new Dictionary<string, object?>
            {
                ["watch_window_template_name"] = watchWindowTemplateName,
                ["linear_precision"] = options.LinearPrecision,
                ["angular_precision"] = options.AngularPrecision,
                ["font"] = options.Font,
                ["text_color"] = options.TextColor,
                ["background_color"] = options.BackgroundColor,
                ["highlight_color"] = options.HighlightColor,
                ["show_deviation_x"] = options.ShowDeviationX,
                ["show_deviation_y"] = options.ShowDeviationY,
                ["show_deviation_z"] = options.ShowDeviationZ,
                ["show_deviation_magnitude"] = options.ShowDeviationMagnitude,
                ["udp_network_transmit_settings"] = udpNetworkTransmitSettings,
                ["transparent_background"] = options.TransparentBackground,
                ["hide_units"] = options.HideUnits,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "RelationshipWatchWindowTemplate",
            request,
            Transport.RelationshipWatchWindowTemplateResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task SetGroupToNominalGroupViewZoomingAsync(
        CollectionItemName relationshipName,
        bool useClosestPoint = true,
        bool showClosestPointWatchWindow = false,
        bool useViewZooming = true,
        bool ignorePointsBeyondThreshold = true,
        double proximityThreshold = 0.01,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGroupToNominalGroupViewZoomingRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["use_closest_point"] = useClosestPoint,
                ["show_closest_point_watch_window"] = showClosestPointWatchWindow,
                ["use_view_zooming"] = useViewZooming,
                ["ignore_points_beyond_threshold"] = ignorePointsBeyondThreshold,
                ["proximity_threshold"] = proximityThreshold,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetGroupToNominalGroupViewZooming",
            request,
            Transport.SetGroupToNominalGroupViewZoomingResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task SetObjectToObjectDirectionRelationshipTolerancesAsync(
        CollectionItemName relationshipName,
        ToleranceScalarOptions angleBetweenVectorsTolerances,
        ToleranceScalarOptions mutualPerpendicularLengthTolerances,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetObjectToObjectDirectionRelationshipTolerancesRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["angle_between_vectors_tolerances"] = angleBetweenVectorsTolerances,
                ["mutual_perpendicular_length_tolerances"] = mutualPerpendicularLengthTolerances,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetObjectToObjectDirectionRelationshipTolerances",
            request,
            Transport.SetObjectToObjectDirectionRelationshipTolerancesResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task SetOptimizationPerturbationParametersAsync(
        double lengthPerturbation = 0.0001,
        double angularPerturbation = 0.0001,
        double damping = 1.0,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetOptimizationPerturbationParametersRequest(),
            new Dictionary<string, object?>
            {
                ["length_perturbation"] = lengthPerturbation,
                ["angular_perturbation"] = angularPerturbation,
                ["damping"] = damping,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetOptimizationPerturbationParameters",
            request,
            Transport.SetOptimizationPerturbationParametersResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task SetOptimizationSearchOptionsAsync(
        int maxNumberOfStepSizeReduction = 5,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetOptimizationSearchOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["max_number_of_step_size_reduction"] = maxNumberOfStepSizeReduction,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetOptimizationSearchOptions",
            request,
            Transport.SetOptimizationSearchOptionsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task SetPointsToPointsRelationshipAssociatedDataAsync(
        CollectionItemName relationshipName,
        IEnumerable<PointName> nominalPoints,
        IEnumerable<PointName> actualPoints,
        bool ignoreEmptyArguments = true,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPointsToPointsRelationshipAssociatedDataRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["nominal_points"] = nominalPoints,
                ["actual_points"] = actualPoints,
                ["ignore_empty_arguments"] = ignoreEmptyArguments,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetPointsToPointsRelationshipAssociatedData",
            request,
            Transport.SetPointsToPointsRelationshipAssociatedDataResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task SetRelationshipAssociatedDataAsync(
        CollectionItemName relationshipName,
        IEnumerable<PointName> individualPoints,
        IEnumerable<CollectionObjectName> pointGroups,
        IEnumerable<CollectionObjectName> pointClouds,
        IEnumerable<CollectionObjectName> objects,
        bool ignoreEmptyArguments = true,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRelationshipAssociatedDataRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["individual_points"] = individualPoints,
                ["point_groups"] = pointGroups,
                ["point_clouds"] = pointClouds,
                ["objects"] = objects,
                ["ignore_empty_arguments"] = ignoreEmptyArguments,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetRelationshipAssociatedData",
            request,
            Transport.SetRelationshipAssociatedDataResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task SetVectorGroupToVectorGroupCylindricalZoneAsync(
        CollectionItemName vgToVgRelationship,
        double radialOffset = 1.0,
        double minimumAxialOffset = -10.0,
        double maximumAxialOffset = 10.0,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetVectorGroupToVectorGroupCylindricalZoneRequest(),
            new Dictionary<string, object?>
            {
                ["vg_to_vg_relationship"] = vgToVgRelationship,
                ["radial_offset"] = radialOffset,
                ["minimum_axial_offset"] = minimumAxialOffset,
                ["maximum_axial_offset"] = maximumAxialOffset,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetVectorGroupToVectorGroupCylindricalZone",
            request,
            Transport.SetVectorGroupToVectorGroupCylindricalZoneResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task SetVectorGroupToVectorGroupFitGradientFactorAsync(
        CollectionItemName vgToVgRelationship,
        double fitGradientFactor = 50.0,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetVectorGroupToVectorGroupFitGradientFactorRequest(),
            new Dictionary<string, object?>
            {
                ["vg_to_vg_relationship"] = vgToVgRelationship,
                ["fit_gradient_factor"] = fitGradientFactor,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetVectorGroupToVectorGroupFitGradientFactor",
            request,
            Transport.SetVectorGroupToVectorGroupFitGradientFactorResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task SetVectorGroupToVectorGroupFitWeightsAsync(
        CollectionItemName vgToVgRelationship,
        double minimumGap = 0.0,
        double minimumGapFitWeight = 10.0,
        double maximumGap = 0.0,
        double maximumGapFitWeight = 10.0,
        double nominalGap = 0.0,
        double nominalGapFitWeight = 1.0,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetVectorGroupToVectorGroupFitWeightsRequest(),
            new Dictionary<string, object?>
            {
                ["vg_to_vg_relationship"] = vgToVgRelationship,
                ["minimum_gap"] = minimumGap,
                ["minimum_gap_fit_weight"] = minimumGapFitWeight,
                ["maximum_gap"] = maximumGap,
                ["maximum_gap_fit_weight"] = maximumGapFitWeight,
                ["nominal_gap"] = nominalGap,
                ["nominal_gap_fit_weight"] = nominalGapFitWeight,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetVectorGroupToVectorGroupFitWeights",
            request,
            Transport.SetVectorGroupToVectorGroupFitWeightsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task SetVectorGroupToVectorGroupRelativePolarityAsync(
        CollectionItemName vgToVgRelationship,
        bool setOpposingVectorGroupPolarity = true,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetVectorGroupToVectorGroupRelativePolarityRequest(),
            new Dictionary<string, object?>
            {
                ["vg_to_vg_relationship"] = vgToVgRelationship,
                ["set_opposing_vector_group_polarity"] = setOpposingVectorGroupPolarity,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "SetVectorGroupToVectorGroupRelativePolarity",
            request,
            Transport.SetVectorGroupToVectorGroupRelativePolarityResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task StartStopRelationshipTrappingAsync(
        CollectionItemName relationshipName,
        CollectionInstrumentId instrumentId,
        bool startTrapping = false,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StartStopRelationshipTrappingRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["instrument_id"] = instrumentId,
                ["start_trapping"] = startTrapping,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "StartStopRelationshipTrapping",
            request,
            Transport.StartStopRelationshipTrappingResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task EditGeometryRelationshipPointListAsync(
        CollectionItemName relationshipName,
        GeometryRelationshipPointEditMode pointEditMode = GeometryRelationshipPointEditMode.PointList,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EditGeometryRelationshipPointListRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["point_edit_mode"] = pointEditMode,
            });
        return InvokeOperationAsync(
            "briosa.RelationshipOperations",
            "EditGeometryRelationshipPointList",
            request,
            Transport.EditGeometryRelationshipPointListResult.Parser,
            onMpCompletion,
            cancellationToken);
    }

    public Task<SigmoidalGapFitConstraints> GetRelationshipSigmoidalGapFitConstraintsAsync(
        CollectionItemName relationshipName,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRelationshipSigmoidalGapFitConstraintsRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
            });
        return InvokeOperationAsync<SigmoidalGapFitConstraints>(
            "briosa.RelationshipOperations",
            "GetRelationshipSigmoidalGapFitConstraints",
            request,
            Transport.GetRelationshipSigmoidalGapFitConstraintsResult.Parser,
            onMpCompletion,
            cancellationToken);
    }
}
