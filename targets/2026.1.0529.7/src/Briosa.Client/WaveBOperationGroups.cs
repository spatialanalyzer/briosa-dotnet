// Drafted mechanically from the approved Briosa documentation contract.
#pragma warning disable CA1008 // Zero is intentionally not a public MP choice.
#pragma warning disable CA1720
#pragma warning disable CA1805 // Explicit initializers preserve reviewed MP defaults.
#pragma warning disable CS1591
using Transport = Briosa.Client.Transport;

namespace Briosa;

public sealed class BriosaConstructionOperations
{
    private readonly BriosaClient _client;
    internal BriosaConstructionOperations(BriosaClient client) => _client = client;

    public Task AddSurfaceToMeshOffsetAlongReferenceDirectionAsync(
        IEnumerable<CollectionObjectName> referenceFrameNames,
        CollectionObjectName surfaceForOffsetDistanceComputation,
        string collectionForResultFrames,
        CollectionObjectName objectProvidingDirectionReference,
        CollectionObjectName meshServingAsProjectionTarget,
        double surfaceOffsetRange = 10,
        bool biDirectionalProjection = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddSurfaceToMeshOffsetAlongReferenceDirectionRequest(),
            new Dictionary<string, object?>
            {
                ["reference_frame_names"] = referenceFrameNames,
                ["surface_for_offset_distance_computation"] = surfaceForOffsetDistanceComputation,
                ["surface_offset_range"] = surfaceOffsetRange,
                ["collection_for_result_frames"] = collectionForResultFrames,
                ["object_providing_direction_reference"] = objectProvidingDirectionReference,
                ["bi_directional_projection"] = biDirectionalProjection,
                ["mesh_serving_as_projection_target"] = meshServingAsProjectionTarget,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "AddSurfaceToMeshOffsetAlongReferenceDirection",
            request,
            Transport.AddSurfaceToMeshOffsetAlongReferenceDirectionResult.Parser,
            cancellationToken);
    }

    public Task AutoArrangeCalloutViewAsync(
        CollectionItemName calloutView,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoArrangeCalloutViewRequest(),
            new Dictionary<string, object?>
            {
                ["callout_view"] = calloutView,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "AutoArrangeCalloutView",
            request,
            Transport.AutoArrangeCalloutViewResult.Parser,
            cancellationToken);
    }

    public Task ClearHiddenPointBarDatabaseAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ClearHiddenPointBarDatabaseRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ClearHiddenPointBarDatabase",
            request,
            Transport.ClearHiddenPointBarDatabaseResult.Parser,
            cancellationToken);
    }

    public Task ConstructBSplineFromIntersectionOfPlaneAndSurfaceAsync(
        CollectionObjectName resultingBSplineName,
        CollectionObjectName planeName,
        CollectionObjectName surfaceName,
        double approximationTolerance = 0.0001,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructBSplineFromIntersectionOfPlaneAndSurfaceRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_b_spline_name"] = resultingBSplineName,
                ["plane_name"] = planeName,
                ["surface_name"] = surfaceName,
                ["approximation_tolerance"] = approximationTolerance,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructBSplineFromIntersectionOfPlaneAndSurface",
            request,
            Transport.ConstructBSplineFromIntersectionOfPlaneAndSurfaceResult.Parser,
            cancellationToken);
    }

    public Task ConstructBSplineFromIntersectionOfSurfacesAsync(
        CollectionObjectName resultingBSplineName,
        CollectionObjectName firstSurfaceName,
        CollectionObjectName secondSurfaceName,
        double approximationTolerance = 0.0001,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructBSplineFromIntersectionOfSurfacesRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_b_spline_name"] = resultingBSplineName,
                ["first_surface_name"] = firstSurfaceName,
                ["second_surface_name"] = secondSurfaceName,
                ["approximation_tolerance"] = approximationTolerance,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructBSplineFromIntersectionOfSurfaces",
            request,
            Transport.ConstructBSplineFromIntersectionOfSurfacesResult.Parser,
            cancellationToken);
    }

    public Task ConstructBSplineFromPointSetAsync(
        CollectionObjectName resultingBSplineName,
        CollectionObjectName pointSetContainer,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructBSplineFromPointSetRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_b_spline_name"] = resultingBSplineName,
                ["b_spline_fit_options"] = new BSplineFitOptions(),
                ["point_set_container"] = pointSetContainer,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructBSplineFromPointSet",
            request,
            Transport.ConstructBSplineFromPointSetResult.Parser,
            cancellationToken);
    }

    public Task ConstructBSplineFromPointSetAsync(
        CollectionObjectName resultingBSplineName,
        BSplineFitOptions bSplineFitOptions,
        CollectionObjectName pointSetContainer,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructBSplineFromPointSetRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_b_spline_name"] = resultingBSplineName,
                ["b_spline_fit_options"] = bSplineFitOptions,
                ["point_set_container"] = pointSetContainer,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructBSplineFromPointSet",
            request,
            Transport.ConstructBSplineFromPointSetResult.Parser,
            cancellationToken);
    }

    public Task ConstructBSplineFromPointsAsync(
        CollectionObjectName resultingBSplineName,
        IEnumerable<PointName> pointList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructBSplineFromPointsRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_b_spline_name"] = resultingBSplineName,
                ["b_spline_fit_options"] = new BSplineFitOptions(),
                ["point_list"] = pointList,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructBSplineFromPoints",
            request,
            Transport.ConstructBSplineFromPointsResult.Parser,
            cancellationToken);
    }

    public Task ConstructBSplineFromPointsAsync(
        CollectionObjectName resultingBSplineName,
        BSplineFitOptions bSplineFitOptions,
        IEnumerable<PointName> pointList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructBSplineFromPointsRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_b_spline_name"] = resultingBSplineName,
                ["b_spline_fit_options"] = bSplineFitOptions,
                ["point_list"] = pointList,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructBSplineFromPoints",
            request,
            Transport.ConstructBSplineFromPointsResult.Parser,
            cancellationToken);
    }

    public Task ConstructBSplineFromSeveralBSplinesAsync(
        CollectionObjectName resultingBSplineName,
        IEnumerable<CollectionObjectName> bSplineList,
        bool closeResultingBSpline = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructBSplineFromSeveralBSplinesRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_b_spline_name"] = resultingBSplineName,
                ["b_spline_list"] = bSplineList,
                ["close_resulting_b_spline"] = closeResultingBSpline,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructBSplineFromSeveralBSplines",
            request,
            Transport.ConstructBSplineFromSeveralBSplinesResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> ConstructBSplinesFromIntersectionOfPlaneAndMeshAsync(
        CollectionObjectName resultingBSplineName,
        CollectionObjectName planeName,
        CollectionObjectName meshName,
        int closedLineSegmentLimit = 3,
        int unclosedLineSegmentLimit = 3,
        bool createIntersectionPoints = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructBSplinesFromIntersectionOfPlaneAndMeshRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_b_spline_name"] = resultingBSplineName,
                ["plane_name"] = planeName,
                ["mesh_name"] = meshName,
                ["closed_line_segment_limit"] = closedLineSegmentLimit,
                ["unclosed_line_segment_limit"] = unclosedLineSegmentLimit,
                ["create_intersection_points"] = createIntersectionPoints,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "ConstructBSplinesFromIntersectionOfPlaneAndMesh",
            request,
            Transport.ConstructBSplinesFromIntersectionOfPlaneAndMeshResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> ConstructBSplinesFromLinesAsync(
        IEnumerable<CollectionObjectName> lineList,
        string? resultingBSplineNamePrefix = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructBSplinesFromLinesRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_b_spline_name_prefix"] = resultingBSplineNamePrefix,
                ["line_list"] = lineList,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "ConstructBSplinesFromLines",
            request,
            Transport.ConstructBSplinesFromLinesResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> ConstructBSplinesFromSurfacesAsync(
        IEnumerable<CollectionObjectName> surfaceList,
        string? resultingBSplineNamePrefix = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructBSplinesFromSurfacesRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_b_spline_name_prefix"] = resultingBSplineNamePrefix,
                ["surface_list"] = surfaceList,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "ConstructBSplinesFromSurfaces",
            request,
            Transport.ConstructBSplinesFromSurfacesResult.Parser,
            cancellationToken);
    }

    public Task ConstructBoundaryPointsFromCloudAsync(
        CollectionObjectName sourceCloudName,
        CollectionObjectName destinationCloudName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructBoundaryPointsFromCloudRequest(),
            new Dictionary<string, object?>
            {
                ["source_cloud_name"] = sourceCloudName,
                ["destination_cloud_name"] = destinationCloudName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructBoundaryPointsFromCloud",
            request,
            Transport.ConstructBoundaryPointsFromCloudResult.Parser,
            cancellationToken);
    }

    public Task ConstructCircleAsync(
        CollectionObjectName circleName,
        Vector circleCenter,
        Vector circleNormal,
        double circleRadius,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructCircleRequest(),
            new Dictionary<string, object?>
            {
                ["circle_name"] = circleName,
                ["circle_center"] = circleCenter,
                ["circle_normal"] = circleNormal,
                ["circle_radius"] = circleRadius,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructCircle",
            request,
            Transport.ConstructCircleResult.Parser,
            cancellationToken);
    }

    public Task ConstructCirclesFromSurfaceFacesRuntimeSelectAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructCirclesFromSurfaceFacesRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructCirclesFromSurfaceFacesRuntimeSelect",
            request,
            Transport.ConstructCirclesFromSurfaceFacesRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> ConstructCirclesLinesFromSurfacesAsync(
        IEnumerable<CollectionObjectName> surfaces,
        CircleLineMode circleLineMode,
        double minimumDiameter = 0.0,
        double maximumDiameter = 0.0,
        double tolerance = 0.02,
        bool singleSurface = false,
        CollectionName? destinationCollectionName = null,
        string baseName = "Geometry Object",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructCirclesLinesFromSurfacesRequest(),
            new Dictionary<string, object?>
            {
                ["surfaces"] = surfaces,
                ["minimum_diameter"] = minimumDiameter,
                ["maximum_diameter"] = maximumDiameter,
                ["tolerance"] = tolerance,
                ["single_surface"] = singleSurface,
                ["circle_line_mode"] = circleLineMode,
                ["destination_collection_name"] = destinationCollectionName,
                ["base_name"] = baseName,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "ConstructCirclesLinesFromSurfaces",
            request,
            Transport.ConstructCirclesLinesFromSurfacesResult.Parser,
            cancellationToken);
    }

    public Task ConstructCollectionAsync(
        CollectionName collectionName,
        string folderPath = "",
        bool makeDefaultCollection = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructCollectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_name"] = collectionName,
                ["folder_path"] = folderPath,
                ["make_default_collection"] = makeDefaultCollection,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructCollection",
            request,
            Transport.ConstructCollectionResult.Parser,
            cancellationToken);
    }

    public Task ConstructConeAsync(
        CollectionObjectName coneName,
        Vector coneEndPoint,
        Vector coneAxis,
        double coneLength,
        double coneThetaStart,
        double coneThetaSpan,
        double coneIncludedAngle,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructConeRequest(),
            new Dictionary<string, object?>
            {
                ["cone_name"] = coneName,
                ["cone_end_point"] = coneEndPoint,
                ["cone_axis"] = coneAxis,
                ["cone_length"] = coneLength,
                ["cone_theta_start"] = coneThetaStart,
                ["cone_theta_span"] = coneThetaSpan,
                ["cone_included_angle"] = coneIncludedAngle,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructCone",
            request,
            Transport.ConstructConeResult.Parser,
            cancellationToken);
    }

    public Task ConstructConesFromSurfaceFacesRuntimeSelectAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructConesFromSurfaceFacesRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructConesFromSurfaceFacesRuntimeSelect",
            request,
            Transport.ConstructConesFromSurfaceFacesRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task ConstructCrossSectionCloudAsync(
        CollectionObjectName crossSectionCloudName,
        IEnumerable<CollectionObjectName> inputClouds,
        bool cylindricalCrossSectionMode = false,
        double startDistance = 0.0,
        double sectionSpacing = 0.0,
        double proximityThreshold = 0.0,
        int maximumSectionCount = 0,
        bool limitCrossSectionExtent = false,
        double radiusLimit = 0.0,
        bool projectToReferenceSurface = false,
        CollectionObjectName? referenceObject = null,
        CloudThinningOptions? cloudThinningSettings = null,
        bool updateExistingCloud = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructCrossSectionCloudRequest(),
            new Dictionary<string, object?>
            {
                ["cross_section_cloud_name"] = crossSectionCloudName,
                ["cylindrical_cross_section_mode"] = cylindricalCrossSectionMode,
                ["start_distance"] = startDistance,
                ["section_spacing"] = sectionSpacing,
                ["proximity_threshold"] = proximityThreshold,
                ["maximum_section_count"] = maximumSectionCount,
                ["limit_cross_section_extent"] = limitCrossSectionExtent,
                ["radius_limit"] = radiusLimit,
                ["project_to_reference_surface"] = projectToReferenceSurface,
                ["reference_object"] = referenceObject,
                ["input_clouds"] = inputClouds,
                ["cloud_thinning_settings"] = cloudThinningSettings,
                ["update_existing_cloud"] = updateExistingCloud,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructCrossSectionCloud",
            request,
            Transport.ConstructCrossSectionCloudResult.Parser,
            cancellationToken);
    }

    public Task ConstructCrossSectionCloudUserSelectAsync(
        CollectionObjectName crossSectionCloudName,
        IEnumerable<CollectionObjectName> referencePlanes,
        IEnumerable<CollectionObjectName> inputClouds,
        double proximityThreshold = 0.0,
        bool limitCrossSectionExtent = false,
        double radiusLimit = 0.0,
        bool projectToReferenceSurface = false,
        CloudThinningOptions? cloudThinningSettings = null,
        bool updateExistingCloud = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructCrossSectionCloudUserSelectRequest(),
            new Dictionary<string, object?>
            {
                ["cross_section_cloud_name"] = crossSectionCloudName,
                ["proximity_threshold"] = proximityThreshold,
                ["limit_cross_section_extent"] = limitCrossSectionExtent,
                ["radius_limit"] = radiusLimit,
                ["project_to_reference_surface"] = projectToReferenceSurface,
                ["reference_planes"] = referencePlanes,
                ["input_clouds"] = inputClouds,
                ["cloud_thinning_settings"] = cloudThinningSettings,
                ["update_existing_cloud"] = updateExistingCloud,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructCrossSectionCloudUserSelect",
            request,
            Transport.ConstructCrossSectionCloudUserSelectResult.Parser,
            cancellationToken);
    }

    public Task ConstructCylinderAsync(
        CollectionObjectName cylinderName,
        Vector cylinderEndPoint,
        Vector cylinderAxis,
        double cylinderDiameter,
        double cylinderLength,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructCylinderRequest(),
            new Dictionary<string, object?>
            {
                ["cylinder_name"] = cylinderName,
                ["cylinder_end_point"] = cylinderEndPoint,
                ["cylinder_axis"] = cylinderAxis,
                ["cylinder_diameter"] = cylinderDiameter,
                ["cylinder_length"] = cylinderLength,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructCylinder",
            request,
            Transport.ConstructCylinderResult.Parser,
            cancellationToken);
    }

    public Task ConstructCylinderFromEndPointsAsync(
        CollectionObjectName cylinderName,
        Vector cylinderEndPointA,
        Vector cylinderEndPointB,
        double cylinderDiameter,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructCylinderFromEndPointsRequest(),
            new Dictionary<string, object?>
            {
                ["cylinder_name"] = cylinderName,
                ["cylinder_end_point_a"] = cylinderEndPointA,
                ["cylinder_end_point_b"] = cylinderEndPointB,
                ["cylinder_diameter"] = cylinderDiameter,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructCylinderFromEndPoints",
            request,
            Transport.ConstructCylinderFromEndPointsResult.Parser,
            cancellationToken);
    }

    public Task ConstructCylindersFromSurfaceFacesRuntimeSelectAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructCylindersFromSurfaceFacesRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructCylindersFromSurfaceFacesRuntimeSelect",
            request,
            Transport.ConstructCylindersFromSurfaceFacesRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task ConstructEllipseAsync(
        CollectionObjectName ellipseName,
        Vector centerCoordinate,
        Vector normalDirection,
        double majorAxisRadius,
        double minorAxisRadius,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructEllipseRequest(),
            new Dictionary<string, object?>
            {
                ["ellipse_name"] = ellipseName,
                ["center_coordinate"] = centerCoordinate,
                ["normal_direction"] = normalDirection,
                ["major_axis_radius"] = majorAxisRadius,
                ["minor_axis_radius"] = minorAxisRadius,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructEllipse",
            request,
            Transport.ConstructEllipseResult.Parser,
            cancellationToken);
    }

    public Task ConstructEllipsoidAsync(
        CollectionObjectName ellipseName,
        double xAxisRadius = 5.0,
        double yAxisRadius = 4.0,
        double zAxisRadius = 3.0,
        double magnification = 1.0,
        bool uncertaintyEllipsoid = false,
        Transform? transformInWorkingCoordinates = null,
        Color? ellipseColor = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructEllipsoidRequest(),
            new Dictionary<string, object?>
            {
                ["ellipse_name"] = ellipseName,
                ["x_axis_radius"] = xAxisRadius,
                ["y_axis_radius"] = yAxisRadius,
                ["z_axis_radius"] = zAxisRadius,
                ["magnification"] = magnification,
                ["uncertainty_ellipsoid"] = uncertaintyEllipsoid,
                ["transform_in_working_coordinates"] = transformInWorkingCoordinates,
                ["ellipse_color"] = ellipseColor,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructEllipsoid",
            request,
            Transport.ConstructEllipsoidResult.Parser,
            cancellationToken);
    }

    public Task ConstructFoldersAsync(
        string folderPath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFoldersRequest(),
            new Dictionary<string, object?>
            {
                ["folder_path"] = folderPath,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFolders",
            request,
            Transport.ConstructFoldersResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameAsync(
        CollectionObjectName newFrameName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameRequest(),
            new Dictionary<string, object?>
            {
                ["new_frame_name"] = newFrameName,
                ["transform_in_working_coordinates"] = new Transform(),
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrame",
            request,
            Transport.ConstructFrameResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameAsync(
        CollectionObjectName newFrameName,
        Transform transformInWorkingCoordinates,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameRequest(),
            new Dictionary<string, object?>
            {
                ["new_frame_name"] = newFrameName,
                ["transform_in_working_coordinates"] = transformInWorkingCoordinates,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrame",
            request,
            Transport.ConstructFrameResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameAtPointWithWorkingZAndClockedAxisAsync(
        PointName originPoint,
        AxisIdentifier clockedAxis,
        PointName clockingPoint,
        string? frameName = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameAtPointWithWorkingZAndClockedAxisRequest(),
            new Dictionary<string, object?>
            {
                ["origin_point"] = originPoint,
                ["clocked_axis"] = clockedAxis,
                ["clocking_point"] = clockingPoint,
                ["frame_name"] = frameName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameAtPointWithWorkingZAndClockedAxis",
            request,
            Transport.ConstructFrameAtPointWithWorkingZAndClockedAxisResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameAtRobotLinkAsync(
        CollectionMachineId machineId,
        string linkName,
        CollectionObjectName resultingFrame,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameAtRobotLinkRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["link_name"] = linkName,
                ["resulting_frame"] = resultingFrame,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameAtRobotLink",
            request,
            Transport.ConstructFrameAtRobotLinkResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameAverageOfOtherObjectFramesAsync(
        IEnumerable<CollectionObjectName> objects,
        CollectionObjectName? frameName = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameAverageOfOtherObjectFramesRequest(),
            new Dictionary<string, object?>
            {
                ["objects"] = objects,
                ["frame_name"] = frameName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameAverageOfOtherObjectFrames",
            request,
            Transport.ConstructFrameAverageOfOtherObjectFramesResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameCopyAndMakeLeftHandedAsync(
        CollectionObjectName referenceFrame,
        FrameAxis axisToReverse,
        CollectionObjectName? frameName = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameCopyAndMakeLeftHandedRequest(),
            new Dictionary<string, object?>
            {
                ["reference_frame"] = referenceFrame,
                ["frame_name"] = frameName,
                ["axis_to_reverse"] = axisToReverse,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameCopyAndMakeLeftHanded",
            request,
            Transport.ConstructFrameCopyAndMakeLeftHandedResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameFromPointMeasurementProbingFramesAsync(
        IEnumerable<PointName> pointList,
        bool showFrame = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameFromPointMeasurementProbingFramesRequest(),
            new Dictionary<string, object?>
            {
                ["point_list"] = pointList,
                ["show_frame"] = showFrame,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameFromPointMeasurementProbingFrames",
            request,
            Transport.ConstructFrameFromPointMeasurementProbingFramesResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameFromTransformInWorldAsync(
        CollectionObjectName newFrameName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameFromTransformInWorldRequest(),
            new Dictionary<string, object?>
            {
                ["new_frame_name"] = newFrameName,
                ["transform_in_world_coordinates"] = new Transform(),
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameFromTransformInWorld",
            request,
            Transport.ConstructFrameFromTransformInWorldResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameFromTransformInWorldAsync(
        CollectionObjectName newFrameName,
        Transform transformInWorldCoordinates,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameFromTransformInWorldRequest(),
            new Dictionary<string, object?>
            {
                ["new_frame_name"] = newFrameName,
                ["transform_in_world_coordinates"] = transformInWorldCoordinates,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameFromTransformInWorld",
            request,
            Transport.ConstructFrameFromTransformInWorldResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameKnownOriginObjectDirectionObjectDirectionAsync(
        PointName knownPoint,
        Vector knownPointValueInNewFrame,
        CollectionObjectName primaryAxisObject,
        AxisIdentifier primaryAxisDefinesWhichAxis,
        CollectionObjectName secondaryAxisObject,
        AxisIdentifier secondaryAxisDefinesWhichAxis,
        CollectionObjectName? frameName = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameKnownOriginObjectDirectionObjectDirectionRequest(),
            new Dictionary<string, object?>
            {
                ["known_point"] = knownPoint,
                ["known_point_value_in_new_frame"] = knownPointValueInNewFrame,
                ["primary_axis_object"] = primaryAxisObject,
                ["primary_axis_defines_which_axis"] = primaryAxisDefinesWhichAxis,
                ["secondary_axis_object"] = secondaryAxisObject,
                ["secondary_axis_defines_which_axis"] = secondaryAxisDefinesWhichAxis,
                ["frame_name"] = frameName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameKnownOriginObjectDirectionObjectDirection",
            request,
            Transport.ConstructFrameKnownOriginObjectDirectionObjectDirectionResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameOnInstrumentBaseAsync(
        CollectionInstrumentId instrumentId,
        string? frameName = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameOnInstrumentBaseRequest(),
            new Dictionary<string, object?>
            {
                ["instrument_id"] = instrumentId,
                ["frame_name"] = frameName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameOnInstrumentBase",
            request,
            Transport.ConstructFrameOnInstrumentBaseResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameOnObjectAsync(
        CollectionObjectName referenceObject,
        CollectionObjectName? frameName = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameOnObjectRequest(),
            new Dictionary<string, object?>
            {
                ["reference_object"] = referenceObject,
                ["frame_name"] = frameName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameOnObject",
            request,
            Transport.ConstructFrameOnObjectResult.Parser,
            cancellationToken);
    }

    public Task ConstructFramePickOriginAndPointOnXAxisClockZAlongWorkingZAsync(
        PointName originPoint,
        PointName pointOnXAxis,
        string? frameName = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFramePickOriginAndPointOnXAxisClockZAlongWorkingZRequest(),
            new Dictionary<string, object?>
            {
                ["origin_point"] = originPoint,
                ["point_on_x_axis"] = pointOnXAxis,
                ["frame_name"] = frameName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFramePickOriginAndPointOnXAxisClockZAlongWorkingZ",
            request,
            Transport.ConstructFramePickOriginAndPointOnXAxisClockZAlongWorkingZResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameThreePlanesAsync(
        CollectionObjectName xPlane,
        double xValueOnPlane,
        CollectionObjectName yPlane,
        double yValueOnPlane,
        CollectionObjectName zPlane,
        double zValueOnPlane,
        CollectionObjectName? frameName = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameThreePlanesRequest(),
            new Dictionary<string, object?>
            {
                ["x_plane"] = xPlane,
                ["x_value_on_plane"] = xValueOnPlane,
                ["y_plane"] = yPlane,
                ["y_value_on_plane"] = yValueOnPlane,
                ["z_plane"] = zPlane,
                ["z_value_on_plane"] = zValueOnPlane,
                ["frame_name"] = frameName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameThreePlanes",
            request,
            Transport.ConstructFrameThreePlanesResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameThreePointsAsync(
        FrameConstructionMethod constructionMethod,
        PointName originPoint,
        PointName primaryAxisPoint,
        PointName secondaryAxisPoint,
        string? frameName = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameThreePointsRequest(),
            new Dictionary<string, object?>
            {
                ["construction_method"] = constructionMethod,
                ["origin_point"] = originPoint,
                ["primary_axis_point"] = primaryAxisPoint,
                ["secondary_axis_point"] = secondaryAxisPoint,
                ["frame_name"] = frameName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameThreePoints",
            request,
            Transport.ConstructFrameThreePointsResult.Parser,
            cancellationToken);
    }

    public Task ConstructFrameWithWizardAsync(
        CollectionObjectName newFrameName,
        bool waitForCompletion = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFrameWithWizardRequest(),
            new Dictionary<string, object?>
            {
                ["new_frame_name"] = newFrameName,
                ["wait_for_completion"] = waitForCompletion,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructFrameWithWizard",
            request,
            Transport.ConstructFrameWithWizardResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> ConstructFramesByProjectingFramesOnMeshAlongFrameDirectionAsync(
        IEnumerable<CollectionObjectName> referenceFrameNames,
        CollectionObjectName baseNameForProjectedFrames,
        CollectionObjectName meshServingAsProjectionTarget,
        bool biDirectionalProjection = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFramesByProjectingFramesOnMeshAlongFrameDirectionRequest(),
            new Dictionary<string, object?>
            {
                ["reference_frame_names"] = referenceFrameNames,
                ["base_name_for_projected_frames"] = baseNameForProjectedFrames,
                ["bi_directional_projection"] = biDirectionalProjection,
                ["mesh_serving_as_projection_target"] = meshServingAsProjectionTarget,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "ConstructFramesByProjectingFramesOnMeshAlongFrameDirection",
            request,
            Transport.ConstructFramesByProjectingFramesOnMeshAlongFrameDirectionResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> ConstructFramesByProjectingFramesOnMeshAlongReferenceDirectionAsync(
        IEnumerable<CollectionObjectName> referenceFrameNames,
        CollectionObjectName baseNameForProjectedFrames,
        CollectionObjectName objectProvidingDirectionReference,
        CollectionObjectName meshServingAsProjectionTarget,
        bool biDirectionalProjection = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructFramesByProjectingFramesOnMeshAlongReferenceDirectionRequest(),
            new Dictionary<string, object?>
            {
                ["reference_frame_names"] = referenceFrameNames,
                ["base_name_for_projected_frames"] = baseNameForProjectedFrames,
                ["object_providing_direction_reference"] = objectProvidingDirectionReference,
                ["bi_directional_projection"] = biDirectionalProjection,
                ["mesh_serving_as_projection_target"] = meshServingAsProjectionTarget,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "ConstructFramesByProjectingFramesOnMeshAlongReferenceDirection",
            request,
            Transport.ConstructFramesByProjectingFramesOnMeshAlongReferenceDirectionResult.Parser,
            cancellationToken);
    }

    public Task ConstructLineCenterOfSlotAsync(
        CollectionObjectName lineName,
        CollectionObjectName slotName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructLineCenterOfSlotRequest(),
            new Dictionary<string, object?>
            {
                ["line_name"] = lineName,
                ["slot_name"] = slotName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructLineCenterOfSlot",
            request,
            Transport.ConstructLineCenterOfSlotResult.Parser,
            cancellationToken);
    }

    public Task ConstructLineFromInstrumentShotAsync(
        PointName pointName,
        CollectionObjectName lineName,
        int observationIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructLineFromInstrumentShotRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
                ["observation_index"] = observationIndex,
                ["line_name"] = lineName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructLineFromInstrumentShot",
            request,
            Transport.ConstructLineFromInstrumentShotResult.Parser,
            cancellationToken);
    }

    public Task ConstructLineNormalToObjectAsync(
        CollectionObjectName lineName,
        CollectionObjectName @object,
        double lineLength = 1,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructLineNormalToObjectRequest(),
            new Dictionary<string, object?>
            {
                ["line_name"] = lineName,
                ["line_length"] = lineLength,
                ["object"] = @object,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructLineNormalToObject",
            request,
            Transport.ConstructLineNormalToObjectResult.Parser,
            cancellationToken);
    }

    public Task ConstructLineNormalToObjectThroughPointAsync(
        CollectionObjectName lineToCreate,
        CollectionObjectName objectName,
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructLineNormalToObjectThroughPointRequest(),
            new Dictionary<string, object?>
            {
                ["line_to_create"] = lineToCreate,
                ["object_name"] = objectName,
                ["point_name"] = pointName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructLineNormalToObjectThroughPoint",
            request,
            Transport.ConstructLineNormalToObjectThroughPointResult.Parser,
            cancellationToken);
    }

    public Task ConstructLineProjectLineToObjectReferencePlaneAsync(
        CollectionObjectName lineToCreate,
        CollectionObjectName lineToProject,
        CollectionObjectName objectToProjectTo,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructLineProjectLineToObjectReferencePlaneRequest(),
            new Dictionary<string, object?>
            {
                ["line_to_create"] = lineToCreate,
                ["line_to_project"] = lineToProject,
                ["object_to_project_to"] = objectToProjectTo,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructLineProjectLineToObjectReferencePlane",
            request,
            Transport.ConstructLineProjectLineToObjectReferencePlaneResult.Parser,
            cancellationToken);
    }

    public Task ConstructLineTwoPlaneIntersectionAsync(
        CollectionObjectName lineName,
        CollectionObjectName firstPlane,
        CollectionObjectName secondPlane,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructLineTwoPlaneIntersectionRequest(),
            new Dictionary<string, object?>
            {
                ["line_name"] = lineName,
                ["first_plane"] = firstPlane,
                ["second_plane"] = secondPlane,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructLineTwoPlaneIntersection",
            request,
            Transport.ConstructLineTwoPlaneIntersectionResult.Parser,
            cancellationToken);
    }

    public Task ConstructLineTwoPointsAsync(
        CollectionObjectName lineName,
        PointName firstPoint,
        PointName secondPoint,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructLineTwoPointsRequest(),
            new Dictionary<string, object?>
            {
                ["line_name"] = lineName,
                ["first_point"] = firstPoint,
                ["second_point"] = secondPoint,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructLineTwoPoints",
            request,
            Transport.ConstructLineTwoPointsResult.Parser,
            cancellationToken);
    }

    public Task ConstructLineTwoPointsVectorNotationAsync(
        CollectionObjectName lineName,
        Vector firstVector,
        Vector secondVector,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructLineTwoPointsVectorNotationRequest(),
            new Dictionary<string, object?>
            {
                ["line_name"] = lineName,
                ["first_vector"] = firstVector,
                ["second_vector"] = secondVector,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructLineTwoPointsVectorNotation",
            request,
            Transport.ConstructLineTwoPointsVectorNotationResult.Parser,
            cancellationToken);
    }

    public Task ConstructLinesFromSurfaceFacesRuntimeSelectAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructLinesFromSurfaceFacesRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructLinesFromSurfaceFacesRuntimeSelect",
            request,
            Transport.ConstructLinesFromSurfaceFacesRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<double> ConstructMirrorCubeFrameAsync(
        CollectionObjectName mirrorCubeFrameName,
        PointName pointName,
        bool useCurrentMeasurementsMarkedAsMirrorShots = true,
        double nominalCubeFaceAngle = 90,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructMirrorCubeFrameRequest(),
            new Dictionary<string, object?>
            {
                ["mirror_cube_frame_name"] = mirrorCubeFrameName,
                ["point_name"] = pointName,
                ["use_current_measurements_marked_as_mirror_shots"] = useCurrentMeasurementsMarkedAsMirrorShots,
                ["nominal_cube_face_angle"] = nominalCubeFaceAngle,
            });
        return _client.InvokeOperationAsync<double>(
            "briosa.ConstructionOperations",
            "ConstructMirrorCubeFrame",
            request,
            Transport.ConstructMirrorCubeFrameResult.Parser,
            cancellationToken);
    }

    public Task ConstructPerimeterFromPointsAsync(
        CollectionObjectName resultingPerimeterName,
        IEnumerable<PointName> pointList,
        bool openPerimeter = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPerimeterFromPointsRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_perimeter_name"] = resultingPerimeterName,
                ["point_list"] = pointList,
                ["open_perimeter"] = openPerimeter,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPerimeterFromPoints",
            request,
            Transport.ConstructPerimeterFromPointsResult.Parser,
            cancellationToken);
    }

    public Task ConstructPlaneAsync(
        CollectionObjectName planeName,
        Vector planeCenter,
        Vector planeNormal,
        double planeEdgeDimension = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPlaneRequest(),
            new Dictionary<string, object?>
            {
                ["plane_name"] = planeName,
                ["plane_center"] = planeCenter,
                ["plane_normal"] = planeNormal,
                ["plane_edge_dimension"] = planeEdgeDimension,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPlane",
            request,
            Transport.ConstructPlaneResult.Parser,
            cancellationToken);
    }

    public Task ConstructPlaneNormalToObjectThroughPointAsync(
        CollectionObjectName resultantPlaneName,
        CollectionObjectName normalToObjectName,
        PointName throughPointName,
        double planeEdgeDimension = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPlaneNormalToObjectThroughPointRequest(),
            new Dictionary<string, object?>
            {
                ["resultant_plane_name"] = resultantPlaneName,
                ["normal_to_object_name"] = normalToObjectName,
                ["through_point_name"] = throughPointName,
                ["plane_edge_dimension"] = planeEdgeDimension,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPlaneNormalToObjectThroughPoint",
            request,
            Transport.ConstructPlaneNormalToObjectThroughPointResult.Parser,
            cancellationToken);
    }

    public Task ConstructPlanesBisectTwoPlanesAsync(
        CollectionObjectName resultantPlaneName,
        CollectionObjectName firstPlane,
        CollectionObjectName secondPlane,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPlanesBisectTwoPlanesRequest(),
            new Dictionary<string, object?>
            {
                ["resultant_plane_name"] = resultantPlaneName,
                ["first_plane"] = firstPlane,
                ["second_plane"] = secondPlane,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPlanesBisectTwoPlanes",
            request,
            Transport.ConstructPlanesBisectTwoPlanesResult.Parser,
            cancellationToken);
    }

    public Task ConstructPlanesBoundingPointGroupAsync(
        CollectionObjectName referencePlaneName,
        CollectionObjectName groupToBound,
        CollectionObjectName? resultingHighPlaneName = null,
        CollectionObjectName? resultingLowPlaneName = null,
        bool overrideTargetPointOffsets = false,
        double offsetValue = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPlanesBoundingPointGroupRequest(),
            new Dictionary<string, object?>
            {
                ["reference_plane_name"] = referencePlaneName,
                ["group_to_bound"] = groupToBound,
                ["resulting_high_plane_name"] = resultingHighPlaneName,
                ["resulting_low_plane_name"] = resultingLowPlaneName,
                ["override_target_point_offsets"] = overrideTargetPointOffsets,
                ["offset_value"] = offsetValue,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPlanesBoundingPointGroup",
            request,
            Transport.ConstructPlanesBoundingPointGroupResult.Parser,
            cancellationToken);
    }

    public Task ConstructPlanesFromSurfaceFacesRuntimeSelectAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPlanesFromSurfaceFacesRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPlanesFromSurfaceFacesRuntimeSelect",
            request,
            Transport.ConstructPlanesFromSurfaceFacesRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointAtCircleCenterAsync(
        CollectionObjectName circleName,
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointAtCircleCenterRequest(),
            new Dictionary<string, object?>
            {
                ["circle_name"] = circleName,
                ["point_name"] = pointName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointAtCircleCenter",
            request,
            Transport.ConstructPointAtCircleCenterResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointAtIntersectionOfBSplineAndSurfacesAsync(
        CollectionObjectName bSplineName,
        IEnumerable<CollectionObjectName> surfaceList,
        PointName pointName,
        double approximationTolerance = 0.001,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointAtIntersectionOfBSplineAndSurfacesRequest(),
            new Dictionary<string, object?>
            {
                ["b_spline_name"] = bSplineName,
                ["surface_list"] = surfaceList,
                ["approximation_tolerance"] = approximationTolerance,
                ["point_name"] = pointName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointAtIntersectionOfBSplineAndSurfaces",
            request,
            Transport.ConstructPointAtIntersectionOfBSplineAndSurfacesResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointAtIntersectionOfPlaneAndLineAsync(
        CollectionObjectName planeName,
        CollectionObjectName lineName,
        PointName resultingPointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointAtIntersectionOfPlaneAndLineRequest(),
            new Dictionary<string, object?>
            {
                ["plane_name"] = planeName,
                ["line_name"] = lineName,
                ["resulting_point_name"] = resultingPointName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointAtIntersectionOfPlaneAndLine",
            request,
            Transport.ConstructPointAtIntersectionOfPlaneAndLineResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointAtIntersectionOfPlanesAsync(
        CollectionObjectName plane1Name,
        CollectionObjectName plane2Name,
        CollectionObjectName plane3Name,
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointAtIntersectionOfPlanesRequest(),
            new Dictionary<string, object?>
            {
                ["plane_1_name"] = plane1Name,
                ["plane_2_name"] = plane2Name,
                ["plane_3_name"] = plane3Name,
                ["point_name"] = pointName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointAtIntersectionOfPlanes",
            request,
            Transport.ConstructPointAtIntersectionOfPlanesResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointAtIntersectionOfTwoBSplinesAsync(
        CollectionObjectName firstBSplineName,
        CollectionObjectName secondBSplineName,
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointAtIntersectionOfTwoBSplinesRequest(),
            new Dictionary<string, object?>
            {
                ["first_b_spline_name"] = firstBSplineName,
                ["second_b_spline_name"] = secondBSplineName,
                ["point_name"] = pointName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointAtIntersectionOfTwoBSplines",
            request,
            Transport.ConstructPointAtIntersectionOfTwoBSplinesResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointAtIntersectionOfTwoLinesAsync(
        CollectionObjectName firstLineName,
        CollectionObjectName secondLineName,
        PointName resultingPointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointAtIntersectionOfTwoLinesRequest(),
            new Dictionary<string, object?>
            {
                ["first_line_name"] = firstLineName,
                ["second_line_name"] = secondLineName,
                ["resulting_point_name"] = resultingPointName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointAtIntersectionOfTwoLines",
            request,
            Transport.ConstructPointAtIntersectionOfTwoLinesResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointAtLineMidpointAsync(
        CollectionObjectName lineName,
        PointName pointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointAtLineMidpointRequest(),
            new Dictionary<string, object?>
            {
                ["line_name"] = lineName,
                ["point_name"] = pointName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointAtLineMidpoint",
            request,
            Transport.ConstructPointAtLineMidpointResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointAtProjectionOfPointOntoObjectAsync(
        PointName pointToProject,
        CollectionObjectName objectName,
        PointName resultingPointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointAtProjectionOfPointOntoObjectRequest(),
            new Dictionary<string, object?>
            {
                ["point_to_project"] = pointToProject,
                ["object_name"] = objectName,
                ["resulting_point_name"] = resultingPointName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointAtProjectionOfPointOntoObject",
            request,
            Transport.ConstructPointAtProjectionOfPointOntoObjectResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointCloudFromExistingCloudsAsync(
        IEnumerable<CollectionObjectName> existingPointCloudList,
        CollectionObjectName newCloudName,
        CloudThinningOptions? cloudThinningSettings = null,
        bool hideOriginalPointClouds = true,
        bool setCloudPointRgbFromVoxels = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointCloudFromExistingCloudsRequest(),
            new Dictionary<string, object?>
            {
                ["existing_point_cloud_list"] = existingPointCloudList,
                ["new_cloud_name"] = newCloudName,
                ["cloud_thinning_settings"] = cloudThinningSettings,
                ["hide_original_point_clouds"] = hideOriginalPointClouds,
                ["set_cloud_point_rgb_from_voxels"] = setCloudPointRgbFromVoxels,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointCloudFromExistingClouds",
            request,
            Transport.ConstructPointCloudFromExistingCloudsResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointCloudFromVisibleCloudPointsAsync(
        IEnumerable<CollectionObjectName> sourceClouds,
        CollectionObjectName destinationCloudName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointCloudFromVisibleCloudPointsRequest(),
            new Dictionary<string, object?>
            {
                ["source_clouds"] = sourceClouds,
                ["destination_cloud_name"] = destinationCloudName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointCloudFromVisibleCloudPoints",
            request,
            Transport.ConstructPointCloudFromVisibleCloudPointsResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointCloudLimitingProbingDirectionsAsync(
        CollectionObjectName sourceCloudName,
        CollectionObjectName normalToObjectName,
        CollectionObjectName destinationCloudName,
        double acceptanceAngle = 30.0,
        bool hideSourceCloud = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointCloudLimitingProbingDirectionsRequest(),
            new Dictionary<string, object?>
            {
                ["source_cloud_name"] = sourceCloudName,
                ["normal_to_object_name"] = normalToObjectName,
                ["acceptance_angle"] = acceptanceAngle,
                ["destination_cloud_name"] = destinationCloudName,
                ["hide_source_cloud"] = hideSourceCloud,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointCloudLimitingProbingDirections",
            request,
            Transport.ConstructPointCloudLimitingProbingDirectionsResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointCloudsFromExistingCloudPointsRuntimeSelectAsync(
        CollectionObjectName cloudName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointCloudsFromExistingCloudPointsRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_name"] = cloudName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointCloudsFromExistingCloudPointsRuntimeSelect",
            request,
            Transport.ConstructPointCloudsFromExistingCloudPointsRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointCloudsFromExistingCloudsUniformSpacingAsync(
        IEnumerable<CollectionObjectName> existingPointCloudList,
        CollectionObjectName newCloudName,
        double desiredPointSpacing = 0.02,
        int minimumPointsPerOutputPoint = 3,
        bool hideOriginalPointClouds = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointCloudsFromExistingCloudsUniformSpacingRequest(),
            new Dictionary<string, object?>
            {
                ["existing_point_cloud_list"] = existingPointCloudList,
                ["desired_point_spacing"] = desiredPointSpacing,
                ["minimum_points_per_output_point"] = minimumPointsPerOutputPoint,
                ["new_cloud_name"] = newCloudName,
                ["hide_original_point_clouds"] = hideOriginalPointClouds,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointCloudsFromExistingCloudsUniformSpacing",
            request,
            Transport.ConstructPointCloudsFromExistingCloudsUniformSpacingResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointCloudsFromExistingPointGroupAsync(
        CollectionObjectName pointGroupName,
        CollectionObjectName cloudName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointCloudsFromExistingPointGroupRequest(),
            new Dictionary<string, object?>
            {
                ["point_group_name"] = pointGroupName,
                ["cloud_name"] = cloudName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointCloudsFromExistingPointGroup",
            request,
            Transport.ConstructPointCloudsFromExistingPointGroupResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointFitToPointsAsync(
        IEnumerable<PointName> pointNames,
        PointName resultingPointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointFitToPointsRequest(),
            new Dictionary<string, object?>
            {
                ["point_names"] = pointNames,
                ["resulting_point_name"] = resultingPointName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointFitToPoints",
            request,
            Transport.ConstructPointFitToPointsResult.Parser,
            cancellationToken);
    }

    public Task<Vector> ConstructPointFromCloudPointRuntimeSelectAsync(
        string selectionPrompt = "Select cloud point",
        bool constructPoint = false,
        PointName? constructedPointName = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointFromCloudPointRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["selection_prompt"] = selectionPrompt,
                ["construct_point"] = constructPoint,
                ["constructed_point_name"] = constructedPointName,
            });
        return _client.InvokeOperationAsync<Vector>(
            "briosa.ConstructionOperations",
            "ConstructPointFromCloudPointRuntimeSelect",
            request,
            Transport.ConstructPointFromCloudPointRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointFromSurveyTargetCenterAsync(
        CollectionObjectName cloudContainingTarget,
        PointName referenceSeedPoint,
        PointName resultCenterPointName,
        SurveyTargetType surveyTargetType = SurveyTargetType.Triangle,
        double searchDiameter = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointFromSurveyTargetCenterRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_containing_target"] = cloudContainingTarget,
                ["reference_seed_point"] = referenceSeedPoint,
                ["survey_target_type"] = surveyTargetType,
                ["search_diameter"] = searchDiameter,
                ["result_center_point_name"] = resultCenterPointName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointFromSurveyTargetCenter",
            request,
            Transport.ConstructPointFromSurveyTargetCenterResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointGroupFromPointCloudAsync(
        CollectionObjectName cloudName,
        CollectionObjectName pointGroupName,
        string pointPrefix = "pt",
        int startingPointNumber = 0,
        double pointOffset = 0.0,
        bool subSampling = false,
        double subSamplingDistance = 0.5,
        bool showProgress = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointGroupFromPointCloudRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_name"] = cloudName,
                ["point_group_name"] = pointGroupName,
                ["point_prefix"] = pointPrefix,
                ["starting_point_number"] = startingPointNumber,
                ["point_offset"] = pointOffset,
                ["sub_sampling"] = subSampling,
                ["sub_sampling_distance"] = subSamplingDistance,
                ["show_progress"] = showProgress,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointGroupFromPointCloud",
            request,
            Transport.ConstructPointGroupFromPointCloudResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointGroupFromPointNameRefListAsync(
        IEnumerable<PointName> pointNameList,
        CollectionObjectName groupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointGroupFromPointNameRefListRequest(),
            new Dictionary<string, object?>
            {
                ["point_name_list"] = pointNameList,
                ["group_name"] = groupName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointGroupFromPointNameRefList",
            request,
            Transport.ConstructPointGroupFromPointNameRefListResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> ConstructPointGroupsFromVectorGroupsAsync(
        IEnumerable<CollectionObjectName> vectorGroups,
        string optionalGroupNameSuffix = "",
        bool makeVectorBeginPoints = false,
        bool makeVectorEndPoints = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointGroupsFromVectorGroupsRequest(),
            new Dictionary<string, object?>
            {
                ["vector_groups"] = vectorGroups,
                ["optional_group_name_suffix"] = optionalGroupNameSuffix,
                ["make_vector_begin_points"] = makeVectorBeginPoints,
                ["make_vector_end_points"] = makeVectorEndPoints,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "ConstructPointGroupsFromVectorGroups",
            request,
            Transport.ConstructPointGroupsFromVectorGroupsResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointInWorkingCoordinatesAsync(
        PointName pointName,
        Vector workingCoordinates,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointInWorkingCoordinatesRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
                ["working_coordinates"] = workingCoordinates,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointInWorkingCoordinates",
            request,
            Transport.ConstructPointInWorkingCoordinatesResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsAtIntersectionOfCircleAndLineAsync(
        CollectionObjectName circleName,
        CollectionObjectName lineName,
        PointName basePointNameForResults,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsAtIntersectionOfCircleAndLineRequest(),
            new Dictionary<string, object?>
            {
                ["circle_name"] = circleName,
                ["line_name"] = lineName,
                ["base_point_name_for_results"] = basePointNameForResults,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsAtIntersectionOfCircleAndLine",
            request,
            Transport.ConstructPointsAtIntersectionOfCircleAndLineResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsAtIntersectionOfPrincipalObjectAxesAndSurfacesAsync(
        IEnumerable<CollectionObjectName> axisObjectList,
        IEnumerable<CollectionObjectName> surfaceList,
        CollectionObjectName resultantGroupName,
        string pointSuffix = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsAtIntersectionOfPrincipalObjectAxesAndSurfacesRequest(),
            new Dictionary<string, object?>
            {
                ["axis_object_list"] = axisObjectList,
                ["surface_list"] = surfaceList,
                ["point_suffix"] = pointSuffix,
                ["resultant_group_name"] = resultantGroupName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsAtIntersectionOfPrincipalObjectAxesAndSurfaces",
            request,
            Transport.ConstructPointsAtIntersectionOfPrincipalObjectAxesAndSurfacesResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsAtProjectionOnSurfacesParallelToWcfAxisAsync(
        IEnumerable<CollectionObjectName> surfaceList,
        IEnumerable<PointName> pointNames,
        WcfAxis axis,
        string groupNameToContainNewPoints = "",
        string pointNamePrefix = "",
        string pointNameSuffix = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsAtProjectionOnSurfacesParallelToWcfAxisRequest(),
            new Dictionary<string, object?>
            {
                ["surface_list"] = surfaceList,
                ["point_names"] = pointNames,
                ["group_name_to_contain_new_points"] = groupNameToContainNewPoints,
                ["point_name_prefix"] = pointNamePrefix,
                ["point_name_suffix"] = pointNameSuffix,
                ["axis"] = axis,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsAtProjectionOnSurfacesParallelToWcfAxis",
            request,
            Transport.ConstructPointsAtProjectionOnSurfacesParallelToWcfAxisResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsAtProjectionOnSurfacesRadialFromWcfAxisAsync(
        IEnumerable<CollectionObjectName> surfaceList,
        IEnumerable<PointName> pointNames,
        WcfAxis axis,
        string groupNameToContainNewPoints = "",
        string pointNamePrefix = "",
        string pointNameSuffix = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsAtProjectionOnSurfacesRadialFromWcfAxisRequest(),
            new Dictionary<string, object?>
            {
                ["surface_list"] = surfaceList,
                ["point_names"] = pointNames,
                ["group_name_to_contain_new_points"] = groupNameToContainNewPoints,
                ["point_name_prefix"] = pointNamePrefix,
                ["point_name_suffix"] = pointNameSuffix,
                ["axis"] = axis,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsAtProjectionOnSurfacesRadialFromWcfAxis",
            request,
            Transport.ConstructPointsAtProjectionOnSurfacesRadialFromWcfAxisResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsAtProjectionOnSurfacesSphericalFromWcfOriginAsync(
        IEnumerable<CollectionObjectName> surfaceList,
        IEnumerable<PointName> pointNames,
        string groupNameToContainNewPoints = "",
        string pointNamePrefix = "",
        string pointNameSuffix = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsAtProjectionOnSurfacesSphericalFromWcfOriginRequest(),
            new Dictionary<string, object?>
            {
                ["surface_list"] = surfaceList,
                ["point_names"] = pointNames,
                ["group_name_to_contain_new_points"] = groupNameToContainNewPoints,
                ["point_name_prefix"] = pointNamePrefix,
                ["point_name_suffix"] = pointNameSuffix,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsAtProjectionOnSurfacesSphericalFromWcfOrigin",
            request,
            Transport.ConstructPointsAtProjectionOnSurfacesSphericalFromWcfOriginResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsAutoCorrespondTwoGroupsInterPointDistanceAsync(
        CollectionObjectName referenceGroup,
        CollectionObjectName groupToBeCopied,
        CollectionObjectName groupToContainMatchedPoints,
        double samePointTolerance = 0.1,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsAutoCorrespondTwoGroupsInterPointDistanceRequest(),
            new Dictionary<string, object?>
            {
                ["reference_group"] = referenceGroup,
                ["group_to_be_copied"] = groupToBeCopied,
                ["same_point_tolerance"] = samePointTolerance,
                ["group_to_contain_matched_points"] = groupToContainMatchedPoints,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsAutoCorrespondTwoGroupsInterPointDistance",
            request,
            Transport.ConstructPointsAutoCorrespondTwoGroupsInterPointDistanceResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsAutoCorrespondTwoGroupsProximityAsync(
        CollectionObjectName referenceGroup,
        CollectionObjectName groupToBeCopied,
        CollectionObjectName groupToContainMatchedPoints,
        double samePointTolerance = 0.25,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsAutoCorrespondTwoGroupsProximityRequest(),
            new Dictionary<string, object?>
            {
                ["reference_group"] = referenceGroup,
                ["group_to_be_copied"] = groupToBeCopied,
                ["same_point_tolerance"] = samePointTolerance,
                ["group_to_contain_matched_points"] = groupToContainMatchedPoints,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsAutoCorrespondTwoGroupsProximity",
            request,
            Transport.ConstructPointsAutoCorrespondTwoGroupsProximityResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<PointName>> ConstructPointsByProjectingPointsOnMeshAlongDirectionAsync(
        IEnumerable<PointName> referencePointNames,
        CollectionObjectName groupNameForProjectedPoints,
        CollectionObjectName objectProvidingDirectionReference,
        CollectionObjectName meshServingAsProjectionTarget,
        bool biDirectionalProjection = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsByProjectingPointsOnMeshAlongDirectionRequest(),
            new Dictionary<string, object?>
            {
                ["reference_point_names"] = referencePointNames,
                ["group_name_for_projected_points"] = groupNameForProjectedPoints,
                ["object_providing_direction_reference"] = objectProvidingDirectionReference,
                ["bi_directional_projection"] = biDirectionalProjection,
                ["mesh_serving_as_projection_target"] = meshServingAsProjectionTarget,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<PointName>>(
            "briosa.ConstructionOperations",
            "ConstructPointsByProjectingPointsOnMeshAlongDirection",
            request,
            Transport.ConstructPointsByProjectingPointsOnMeshAlongDirectionResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsCylindricallyShiftedAsync(
        CollectionObjectName referenceObjectName,
        IEnumerable<PointName> originalPoints,
        CollectionObjectName groupForNewPoints,
        double radialShift = 0.0,
        double thetaShiftDegrees = 0.0,
        double planarShift = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsCylindricallyShiftedRequest(),
            new Dictionary<string, object?>
            {
                ["reference_object_name"] = referenceObjectName,
                ["original_points"] = originalPoints,
                ["group_for_new_points"] = groupForNewPoints,
                ["radial_shift"] = radialShift,
                ["theta_shift_degrees"] = thetaShiftDegrees,
                ["planar_shift"] = planarShift,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsCylindricallyShifted",
            request,
            Transport.ConstructPointsCylindricallyShiftedResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsFromCylinderAsync(
        CollectionObjectName cylinderName,
        CollectionObjectName groupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsFromCylinderRequest(),
            new Dictionary<string, object?>
            {
                ["cylinder_name"] = cylinderName,
                ["group_name"] = groupName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsFromCylinder",
            request,
            Transport.ConstructPointsFromCylinderResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsFromSurfaceFacesRuntimeSelectAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsFromSurfaceFacesRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsFromSurfaceFacesRuntimeSelect",
            request,
            Transport.ConstructPointsFromSurfaceFacesRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsFromSurfacesOnUvGridAsync(
        IEnumerable<CollectionObjectName> surfaceList,
        string uvPointGroupBaseName = "UV Points",
        bool makeEachLineSeparateGroup = false,
        int numberOfUGrids = 5,
        int numberOfVGrids = 5,
        EdgePointMode edgePointMode = EdgePointMode.IncludeEdges,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsFromSurfacesOnUvGridRequest(),
            new Dictionary<string, object?>
            {
                ["surface_list"] = surfaceList,
                ["uv_point_group_base_name"] = uvPointGroupBaseName,
                ["make_each_line_separate_group"] = makeEachLineSeparateGroup,
                ["number_of_u_grids"] = numberOfUGrids,
                ["number_of_v_grids"] = numberOfVGrids,
                ["edge_point_mode"] = edgePointMode,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsFromSurfacesOnUvGrid",
            request,
            Transport.ConstructPointsFromSurfacesOnUvGridResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsLayoutOnGridAsync(
        CollectionObjectName groupName,
        string pointPrefix = "p",
        double xMin = 0.0,
        double xMax = 100.0,
        int xCount = 10,
        double yMin = 0.0,
        double yMax = 50.0,
        int yCount = 10,
        double zMin = 0.0,
        double zMax = 0.0,
        int zCount = 1,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsLayoutOnGridRequest(),
            new Dictionary<string, object?>
            {
                ["group_name"] = groupName,
                ["point_prefix"] = pointPrefix,
                ["x_min"] = xMin,
                ["x_max"] = xMax,
                ["x_count"] = xCount,
                ["y_min"] = yMin,
                ["y_max"] = yMax,
                ["y_count"] = yCount,
                ["z_min"] = zMin,
                ["z_max"] = zMax,
                ["z_count"] = zCount,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsLayoutOnGrid",
            request,
            Transport.ConstructPointsLayoutOnGridResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsNSpacedOnCurvesAsync(
        IEnumerable<CollectionObjectName> bSplineList,
        CollectionObjectName resultantGroupName,
        int numberOfEvenlySpacedPoints = 10,
        string resultantPointNamePrefix = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsNSpacedOnCurvesRequest(),
            new Dictionary<string, object?>
            {
                ["b_spline_list"] = bSplineList,
                ["number_of_evenly_spaced_points"] = numberOfEvenlySpacedPoints,
                ["resultant_group_name"] = resultantGroupName,
                ["resultant_point_name_prefix"] = resultantPointNamePrefix,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsNSpacedOnCurves",
            request,
            Transport.ConstructPointsNSpacedOnCurvesResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsOnCurvesUsingMaxChordalDeviationAsync(
        IEnumerable<CollectionObjectName> bSplineList,
        CollectionObjectName resultantGroupName,
        double maximumChordalDeviation = 0.05,
        double maximumTrimEdgeAngle = 15.0,
        double maximumChordLength = 0.0,
        string resultantPointNamePrefix = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsOnCurvesUsingMaxChordalDeviationRequest(),
            new Dictionary<string, object?>
            {
                ["b_spline_list"] = bSplineList,
                ["maximum_chordal_deviation"] = maximumChordalDeviation,
                ["maximum_trim_edge_angle"] = maximumTrimEdgeAngle,
                ["maximum_chord_length"] = maximumChordLength,
                ["resultant_group_name"] = resultantGroupName,
                ["resultant_point_name_prefix"] = resultantPointNamePrefix,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsOnCurvesUsingMaxChordalDeviation",
            request,
            Transport.ConstructPointsOnCurvesUsingMaxChordalDeviationResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsOnObjectVerticesAsync(
        IEnumerable<CollectionObjectName> objectNameList,
        CollectionObjectName resultantGroupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsOnObjectVerticesRequest(),
            new Dictionary<string, object?>
            {
                ["object_name_list"] = objectNameList,
                ["resultant_group_name"] = resultantGroupName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsOnObjectVertices",
            request,
            Transport.ConstructPointsOnObjectVerticesResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsOnSurfacesByClickingAsync(
        CollectionObjectName groupNameForPoints,
        string firstPointName = "p0",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsOnSurfacesByClickingRequest(),
            new Dictionary<string, object?>
            {
                ["group_name_for_points"] = groupNameForPoints,
                ["first_point_name"] = firstPointName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsOnSurfacesByClicking",
            request,
            Transport.ConstructPointsOnSurfacesByClickingResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsShiftedInWorkingFrameAsync(
        IEnumerable<PointName> originalPoints,
        CollectionObjectName groupForNewPoints,
        Vector? shiftVector = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsShiftedInWorkingFrameRequest(),
            new Dictionary<string, object?>
            {
                ["original_points"] = originalPoints,
                ["group_for_new_points"] = groupForNewPoints,
                ["shift_vector"] = shiftVector,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsShiftedInWorkingFrame",
            request,
            Transport.ConstructPointsShiftedInWorkingFrameResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsSpacedAtDistanceOnCurvesAsync(
        IEnumerable<CollectionObjectName> bSplineList,
        CollectionObjectName resultantGroupName,
        double distanceBetweenPoints = 0.5,
        string resultantPointNamePrefix = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsSpacedAtDistanceOnCurvesRequest(),
            new Dictionary<string, object?>
            {
                ["b_spline_list"] = bSplineList,
                ["distance_between_points"] = distanceBetweenPoints,
                ["resultant_group_name"] = resultantGroupName,
                ["resultant_point_name_prefix"] = resultantPointNamePrefix,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsSpacedAtDistanceOnCurves",
            request,
            Transport.ConstructPointsSpacedAtDistanceOnCurvesResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsSubsetWithGreatestSpacingAsync(
        IEnumerable<PointName> pointsToSubsample,
        int subsetSize = 10,
        CollectionObjectName? groupForSubset = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsSubsetWithGreatestSpacingRequest(),
            new Dictionary<string, object?>
            {
                ["points_to_subsample"] = pointsToSubsample,
                ["subset_size"] = subsetSize,
                ["group_for_subset"] = groupForSubset,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsSubsetWithGreatestSpacing",
            request,
            Transport.ConstructPointsSubsetWithGreatestSpacingResult.Parser,
            cancellationToken);
    }

    public Task ConstructPointsWildcardSelectionAsync(
        IEnumerable<CollectionObjectName> groupsToSelectFrom,
        PointName wildcardSelectionNames,
        CollectionObjectName groupForNewPoints,
        bool includePriorCompleteName = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointsWildcardSelectionRequest(),
            new Dictionary<string, object?>
            {
                ["groups_to_select_from"] = groupsToSelectFrom,
                ["wildcard_selection_names"] = wildcardSelectionNames,
                ["group_for_new_points"] = groupForNewPoints,
                ["include_prior_complete_name"] = includePriorCompleteName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPointsWildcardSelection",
            request,
            Transport.ConstructPointsWildcardSelectionResult.Parser,
            cancellationToken);
    }

    public Task ConstructSphereAsync(
        CollectionObjectName sphereName,
        Vector sphereCenterInWorkingCoordinates,
        double sphereRadius,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSphereRequest(),
            new Dictionary<string, object?>
            {
                ["sphere_name"] = sphereName,
                ["sphere_center_in_working_coordinates"] = sphereCenterInWorkingCoordinates,
                ["sphere_radius"] = sphereRadius,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSphere",
            request,
            Transport.ConstructSphereResult.Parser,
            cancellationToken);
    }

    public Task ConstructSpheresFromSurfaceFacesRuntimeSelectAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSpheresFromSurfaceFacesRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSpheresFromSurfaceFacesRuntimeSelect",
            request,
            Transport.ConstructSpheresFromSurfaceFacesRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> ConstructSurfaceByDissectingSurfacesAsync(
        SurfaceDissectionMode dissectionMode,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfaceByDissectingSurfacesRequest(),
            new Dictionary<string, object?>
            {
                ["dissection_mode"] = dissectionMode,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "ConstructSurfaceByDissectingSurfaces",
            request,
            Transport.ConstructSurfaceByDissectingSurfacesResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfaceByOffsettingSurfaceAsync(
        IEnumerable<CollectionObjectName> referenceSurface,
        double surfaceOffset = 0.0,
        bool hideOriginalSurface = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfaceByOffsettingSurfaceRequest(),
            new Dictionary<string, object?>
            {
                ["reference_surface"] = referenceSurface,
                ["surface_offset"] = surfaceOffset,
                ["hide_original_surface"] = hideOriginalSurface,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfaceByOffsettingSurface",
            request,
            Transport.ConstructSurfaceByOffsettingSurfaceResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfaceFitFromNominalSurfacesAndActualDataAsync(
        CollectionObjectName nominalSurface,
        IEnumerable<PointName> actualDataPointList,
        CollectionObjectName resultingSurfaceName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfaceFitFromNominalSurfacesAndActualDataRequest(),
            new Dictionary<string, object?>
            {
                ["nominal_surface"] = nominalSurface,
                ["actual_data_point_list"] = actualDataPointList,
                ["resulting_surface_name"] = resultingSurfaceName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfaceFitFromNominalSurfacesAndActualData",
            request,
            Transport.ConstructSurfaceFitFromNominalSurfacesAndActualDataResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfaceFromAnnotationLinksAsync(
        IEnumerable<CollectionObjectName> annotationList,
        CollectionObjectName resultingSurfaceName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfaceFromAnnotationLinksRequest(),
            new Dictionary<string, object?>
            {
                ["annotation_list"] = annotationList,
                ["resulting_surface_name"] = resultingSurfaceName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfaceFromAnnotationLinks",
            request,
            Transport.ConstructSurfaceFromAnnotationLinksResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfaceFromBSplinesAsync(
        CollectionObjectName resultingSurfaceName,
        IEnumerable<CollectionObjectName> bSplineList,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfaceFromBSplinesRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_surface_name"] = resultingSurfaceName,
                ["b_spline_list"] = bSplineList,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfaceFromBSplines",
            request,
            Transport.ConstructSurfaceFromBSplinesResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfaceFromCollectionOfSurfacesAsync(
        IEnumerable<CollectionObjectName> surfacesToCombine,
        CollectionObjectName resultingSurfaceName,
        bool hideOriginalSurfaces = true,
        bool deleteOriginalSurfaces = false,
        bool enableSewingTolerance = false,
        double sewingTolerance = -1.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfaceFromCollectionOfSurfacesRequest(),
            new Dictionary<string, object?>
            {
                ["surfaces_to_combine"] = surfacesToCombine,
                ["resulting_surface_name"] = resultingSurfaceName,
                ["hide_original_surfaces"] = hideOriginalSurfaces,
                ["delete_original_surfaces"] = deleteOriginalSurfaces,
                ["enable_sewing_tolerance"] = enableSewingTolerance,
                ["sewing_tolerance"] = sewingTolerance,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfaceFromCollectionOfSurfaces",
            request,
            Transport.ConstructSurfaceFromCollectionOfSurfacesResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfaceFromConeAsync(
        CollectionObjectName resultingSurfaceName,
        CollectionObjectName coneName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfaceFromConeRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_surface_name"] = resultingSurfaceName,
                ["cone_name"] = coneName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfaceFromCone",
            request,
            Transport.ConstructSurfaceFromConeResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfaceFromCylinderAsync(
        CollectionObjectName resultingSurfaceName,
        CollectionObjectName cylinderName,
        bool internalCylinder = true,
        bool useThetaExtentMode = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfaceFromCylinderRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_surface_name"] = resultingSurfaceName,
                ["cylinder_name"] = cylinderName,
                ["internal_cylinder"] = internalCylinder,
                ["use_theta_extent_mode"] = useThetaExtentMode,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfaceFromCylinder",
            request,
            Transport.ConstructSurfaceFromCylinderResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfaceFromPlaneAsync(
        CollectionObjectName resultingSurfaceName,
        CollectionObjectName planeName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfaceFromPlaneRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_surface_name"] = resultingSurfaceName,
                ["plane_name"] = planeName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfaceFromPlane",
            request,
            Transport.ConstructSurfaceFromPlaneResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfaceFromPointGroupsAsync(
        IEnumerable<CollectionObjectName> groupNameList,
        CollectionObjectName resultingSurfaceName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfaceFromPointGroupsRequest(),
            new Dictionary<string, object?>
            {
                ["group_name_list"] = groupNameList,
                ["b_spline_fit_options"] = new BSplineFitOptions(),
                ["resulting_surface_name"] = resultingSurfaceName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfaceFromPointGroups",
            request,
            Transport.ConstructSurfaceFromPointGroupsResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfaceFromPointGroupsAsync(
        IEnumerable<CollectionObjectName> groupNameList,
        BSplineFitOptions bSplineFitOptions,
        CollectionObjectName resultingSurfaceName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfaceFromPointGroupsRequest(),
            new Dictionary<string, object?>
            {
                ["group_name_list"] = groupNameList,
                ["b_spline_fit_options"] = bSplineFitOptions,
                ["resulting_surface_name"] = resultingSurfaceName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfaceFromPointGroups",
            request,
            Transport.ConstructSurfaceFromPointGroupsResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfaceFromSphereAsync(
        CollectionObjectName resultingSurfaceName,
        CollectionObjectName sphereName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfaceFromSphereRequest(),
            new Dictionary<string, object?>
            {
                ["resulting_surface_name"] = resultingSurfaceName,
                ["sphere_name"] = sphereName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfaceFromSphere",
            request,
            Transport.ConstructSurfaceFromSphereResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> ConstructSurfacesByDissectingSurfacesFromRefListAsync(
        IEnumerable<CollectionObjectName> surfacesToDissect,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfacesByDissectingSurfacesFromRefListRequest(),
            new Dictionary<string, object?>
            {
                ["surfaces_to_dissect"] = surfacesToDissect,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "ConstructSurfacesByDissectingSurfacesFromRefList",
            request,
            Transport.ConstructSurfacesByDissectingSurfacesFromRefListResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfacesByProjectingPointsAsync(
        IEnumerable<CollectionObjectName> projectionTargetNameList,
        IEnumerable<PointName> pointList,
        CollectionObjectName resultingSurfaceName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfacesByProjectingPointsRequest(),
            new Dictionary<string, object?>
            {
                ["projection_target_name_list"] = projectionTargetNameList,
                ["point_list"] = pointList,
                ["resulting_surface_name"] = resultingSurfaceName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfacesByProjectingPoints",
            request,
            Transport.ConstructSurfacesByProjectingPointsResult.Parser,
            cancellationToken);
    }

    public Task ConstructSurfacesFromObjectsAsync(
        IEnumerable<CollectionObjectName> objects,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructSurfacesFromObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["objects"] = objects,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructSurfacesFromObjects",
            request,
            Transport.ConstructSurfacesFromObjectsResult.Parser,
            cancellationToken);
    }

    public Task ConstructVectorGroupAreaProfileCheckAsync(
        IEnumerable<VectorName> referenceVectors,
        IEnumerable<CollectionVectorGroupName> vectorGroupsToCheck,
        CollectionVectorGroupName resultantVectorGroupName,
        double areaRadius = 0.0,
        double areaTolerance = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructVectorGroupAreaProfileCheckRequest(),
            new Dictionary<string, object?>
            {
                ["reference_vectors"] = referenceVectors,
                ["vector_groups_to_check"] = vectorGroupsToCheck,
                ["area_radius"] = areaRadius,
                ["area_tolerance"] = areaTolerance,
                ["resultant_vector_group_name"] = resultantVectorGroupName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructVectorGroupAreaProfileCheck",
            request,
            Transport.ConstructVectorGroupAreaProfileCheckResult.Parser,
            cancellationToken);
    }

    public Task ConstructVectorGroupFromRelationshipAsync(
        CollectionObjectName relationshipName,
        CollectionVectorGroupName vectorGroupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructVectorGroupFromRelationshipRequest(),
            new Dictionary<string, object?>
            {
                ["relationship_name"] = relationshipName,
                ["vector_group_name"] = vectorGroupName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructVectorGroupFromRelationship",
            request,
            Transport.ConstructVectorGroupFromRelationshipResult.Parser,
            cancellationToken);
    }

    public Task ConstructVectorGroupFromVectorNameRefListAsync(
        IEnumerable<VectorName> vectorNameList,
        CollectionVectorGroupName resultantVectorGroupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructVectorGroupFromVectorNameRefListRequest(),
            new Dictionary<string, object?>
            {
                ["vector_name_list"] = vectorNameList,
                ["resultant_vector_group_name"] = resultantVectorGroupName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructVectorGroupFromVectorNameRefList",
            request,
            Transport.ConstructVectorGroupFromVectorNameRefListResult.Parser,
            cancellationToken);
    }

    public Task<ConstructVectorGroupGroupToGroupCompareResult> ConstructVectorGroupGroupToGroupCompareAsync(
        CollectionObjectName vectorGroupName,
        CollectionObjectName groupA,
        CollectionObjectName groupB,
        double rmsDeviationTolerance = 0.0,
        double maxAbsoluteDeviationTolerance = 0.0,
        double averageDeviationTolerance = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructVectorGroupGroupToGroupCompareRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group_name"] = vectorGroupName,
                ["group_a"] = groupA,
                ["group_b"] = groupB,
                ["rms_deviation_tolerance"] = rmsDeviationTolerance,
                ["max_absolute_deviation_tolerance"] = maxAbsoluteDeviationTolerance,
                ["average_deviation_tolerance"] = averageDeviationTolerance,
            });
        return _client.InvokeOperationAsync<ConstructVectorGroupGroupToGroupCompareResult>(
            "briosa.ConstructionOperations",
            "ConstructVectorGroupGroupToGroupCompare",
            request,
            Transport.ConstructVectorGroupGroupToGroupCompareResult.Parser,
            cancellationToken);
    }

    public Task ConstructVectorInWorkingCoordinatesBeginDeltaAsync(
        CollectionObjectName vectorGroupName,
        string newVectorName,
        Vector beginInWorkingCoordinates,
        Vector deltaInWorkingCoordinates,
        bool isMagnitudeNegative = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructVectorInWorkingCoordinatesBeginDeltaRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group_name"] = vectorGroupName,
                ["new_vector_name"] = newVectorName,
                ["begin_in_working_coordinates"] = beginInWorkingCoordinates,
                ["delta_in_working_coordinates"] = deltaInWorkingCoordinates,
                ["is_magnitude_negative"] = isMagnitudeNegative,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructVectorInWorkingCoordinatesBeginDelta",
            request,
            Transport.ConstructVectorInWorkingCoordinatesBeginDeltaResult.Parser,
            cancellationToken);
    }

    public Task ConstructVectorInWorkingCoordinatesBeginDirectionMagnitudeAsync(
        CollectionObjectName vectorGroupName,
        string newVectorName,
        Vector beginInWorkingCoordinates,
        Vector directionInWorkingCoordinates,
        double signedMagnitude = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructVectorInWorkingCoordinatesBeginDirectionMagnitudeRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group_name"] = vectorGroupName,
                ["new_vector_name"] = newVectorName,
                ["begin_in_working_coordinates"] = beginInWorkingCoordinates,
                ["direction_in_working_coordinates"] = directionInWorkingCoordinates,
                ["signed_magnitude"] = signedMagnitude,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructVectorInWorkingCoordinatesBeginDirectionMagnitude",
            request,
            Transport.ConstructVectorInWorkingCoordinatesBeginDirectionMagnitudeResult.Parser,
            cancellationToken);
    }

    public Task CopyGroupsExcludingObscuredPointsAsync(
        CollectionInstrumentId instrumentId,
        IEnumerable<CollectionObjectName> groupNames,
        CollectionName newCollectionName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CopyGroupsExcludingObscuredPointsRequest(),
            new Dictionary<string, object?>
            {
                ["instrument_id"] = instrumentId,
                ["group_names"] = groupNames,
                ["new_collection_name"] = newCollectionName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "CopyGroupsExcludingObscuredPoints",
            request,
            Transport.CopyGroupsExcludingObscuredPointsResult.Parser,
            cancellationToken);
    }

    public Task CreateHiddenPointAsync(
        PointName endAPointName,
        PointName endBPointName,
        PointName pointNameToCreate,
        int hiddenPointRodIndex = 0,
        bool overwriteExistingPoint = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreateHiddenPointRequest(),
            new Dictionary<string, object?>
            {
                ["end_a_point_name"] = endAPointName,
                ["end_b_point_name"] = endBPointName,
                ["hidden_point_rod_index"] = hiddenPointRodIndex,
                ["overwrite_existing_point"] = overwriteExistingPoint,
                ["point_name_to_create"] = pointNameToCreate,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "CreateHiddenPoint",
            request,
            Transport.CreateHiddenPointResult.Parser,
            cancellationToken);
    }

    public Task<int> CreateHiddenPointRodAsync(
        string hiddenPointRodName,
        double targetToTargetDistance = 0.0,
        double targetToTipDistance = 0.0,
        double interPointTolerance = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreateHiddenPointRodRequest(),
            new Dictionary<string, object?>
            {
                ["hidden_point_rod_name"] = hiddenPointRodName,
                ["target_to_target_distance"] = targetToTargetDistance,
                ["target_to_tip_distance"] = targetToTipDistance,
                ["inter_point_tolerance"] = interPointTolerance,
            });
        return _client.InvokeOperationAsync<int>(
            "briosa.ConstructionOperations",
            "CreateHiddenPointRod",
            request,
            Transport.CreateHiddenPointRodResult.Parser,
            cancellationToken);
    }

    public Task CreateMinMaxVectorGroupCalloutAsync(
        CollectionItemName destinationCalloutView,
        CollectionObjectName vectorGroupName,
        int numberOfVectorsWithHighestMag = 1,
        int numberOfVectorsWithLowestMag = 1,
        bool showCollection = false,
        bool showVectorGroup = false,
        bool showVectorName = true,
        bool showDx = false,
        bool showDy = false,
        bool showDz = false,
        bool showDMag = true,
        bool showToleranceColor = true,
        bool toleranceColorBlueGreenRed = false,
        bool showOutOfToleranceValue = true,
        bool showToleranceRange = false,
        bool showVectorColor = true,
        bool showStartPoint = false,
        bool showEndPoint = false,
        bool showUnits = false,
        bool attachCalloutToEndPoint = true,
        bool useDefaultPlacement = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreateMinMaxVectorGroupCalloutRequest(),
            new Dictionary<string, object?>
            {
                ["destination_callout_view"] = destinationCalloutView,
                ["vector_group_name"] = vectorGroupName,
                ["number_of_vectors_with_highest_mag"] = numberOfVectorsWithHighestMag,
                ["number_of_vectors_with_lowest_mag"] = numberOfVectorsWithLowestMag,
                ["show_collection"] = showCollection,
                ["show_vector_group"] = showVectorGroup,
                ["show_vector_name"] = showVectorName,
                ["show_dx"] = showDx,
                ["show_dy"] = showDy,
                ["show_dz"] = showDz,
                ["show_d_mag"] = showDMag,
                ["show_tolerance_color"] = showToleranceColor,
                ["tolerance_color_blue_green_red"] = toleranceColorBlueGreenRed,
                ["show_out_of_tolerance_value"] = showOutOfToleranceValue,
                ["show_tolerance_range"] = showToleranceRange,
                ["show_vector_color"] = showVectorColor,
                ["show_start_point"] = showStartPoint,
                ["show_end_point"] = showEndPoint,
                ["show_units"] = showUnits,
                ["attach_callout_to_end_point"] = attachCalloutToEndPoint,
                ["use_default_placement"] = useDefaultPlacement,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "CreateMinMaxVectorGroupCallout",
            request,
            Transport.CreateMinMaxVectorGroupCalloutResult.Parser,
            cancellationToken);
    }

    public Task CreatePictureCalloutAsync(
        CollectionItemName destinationCalloutView,
        CollectionItemName pictureName,
        double viewXPosition = 0.4,
        double viewYPosition = 0.6,
        int scaleImagePercent = 100,
        CollectionItemName? objectForCalloutAnchorPoint = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreatePictureCalloutRequest(),
            new Dictionary<string, object?>
            {
                ["destination_callout_view"] = destinationCalloutView,
                ["picture_name"] = pictureName,
                ["view_x_position"] = viewXPosition,
                ["view_y_position"] = viewYPosition,
                ["scale_image_percent"] = scaleImagePercent,
                ["object_for_callout_anchor_point"] = objectForCalloutAnchorPoint,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "CreatePictureCallout",
            request,
            Transport.CreatePictureCalloutResult.Parser,
            cancellationToken);
    }

    public Task CreatePointCalloutAsync(
        CollectionItemName destinationCalloutView,
        PointName point,
        double viewXPosition = 0.0,
        double viewYPosition = 0.0,
        bool showPointCollection = false,
        bool showPointGroup = true,
        bool showPointTarget = true,
        bool showX = true,
        bool showY = true,
        bool showZ = true,
        bool showUnits = false,
        bool showUx = false,
        bool showUy = false,
        bool showUz = false,
        bool showUMag = false,
        CoordinateSystemType desiredCoordinateSystem = CoordinateSystemType.Cartesian,
        IEnumerable<string>? notes = null,
        bool useDefaultPlacement = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreatePointCalloutRequest(),
            new Dictionary<string, object?>
            {
                ["destination_callout_view"] = destinationCalloutView,
                ["point"] = point,
                ["view_x_position"] = viewXPosition,
                ["view_y_position"] = viewYPosition,
                ["show_point_collection"] = showPointCollection,
                ["show_point_group"] = showPointGroup,
                ["show_point_target"] = showPointTarget,
                ["show_x_r"] = showX,
                ["show_y_theta"] = showY,
                ["show_z_phi"] = showZ,
                ["show_units"] = showUnits,
                ["show_ux_ur"] = showUx,
                ["show_uy_utheta"] = showUy,
                ["show_uz_uphi"] = showUz,
                ["show_umag"] = showUMag,
                ["desired_coordinate_system"] = desiredCoordinateSystem,
                ["notes"] = notes,
                ["use_default_placement"] = useDefaultPlacement,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "CreatePointCallout",
            request,
            Transport.CreatePointCalloutResult.Parser,
            cancellationToken);
    }

    public Task CreatePointComparisonCalloutAsync(
        CollectionItemName destinationCalloutView,
        PointName firstPoint,
        PointName secondPoint,
        double viewXPosition = 0.0,
        double viewYPosition = 0.0,
        bool showFirstPointCollection = false,
        bool showFirstPointGroup = true,
        bool showFirstPointTarget = true,
        bool showFirstPointCoordinates = false,
        bool showSecondPointCollection = false,
        bool showSecondPointGroup = true,
        bool showSecondPointTarget = true,
        bool showSecondPointCoordinates = false,
        bool showDx = true,
        bool showDy = true,
        bool showDz = true,
        bool showDMag = true,
        string? additionalXComments = null,
        string? additionalYComments = null,
        string? additionalZComments = null,
        IEnumerable<string>? additionalNotes = null,
        bool useDefaultPlacement = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreatePointComparisonCalloutRequest(),
            new Dictionary<string, object?>
            {
                ["destination_callout_view"] = destinationCalloutView,
                ["first_point"] = firstPoint,
                ["second_point"] = secondPoint,
                ["view_x_position"] = viewXPosition,
                ["view_y_position"] = viewYPosition,
                ["show_first_point_collection"] = showFirstPointCollection,
                ["show_first_point_group"] = showFirstPointGroup,
                ["show_first_point_target"] = showFirstPointTarget,
                ["show_first_point_coordinates"] = showFirstPointCoordinates,
                ["show_second_point_collection"] = showSecondPointCollection,
                ["show_second_point_group"] = showSecondPointGroup,
                ["show_second_point_target"] = showSecondPointTarget,
                ["show_second_point_coordinates"] = showSecondPointCoordinates,
                ["show_dx"] = showDx,
                ["show_dy"] = showDy,
                ["show_dz"] = showDz,
                ["show_d_mag"] = showDMag,
                ["additional_x_comments"] = additionalXComments,
                ["additional_y_comments"] = additionalYComments,
                ["additional_z_comments"] = additionalZComments,
                ["additional_notes"] = additionalNotes,
                ["use_default_placement"] = useDefaultPlacement,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "CreatePointComparisonCallout",
            request,
            Transport.CreatePointComparisonCalloutResult.Parser,
            cancellationToken);
    }

    public Task CreateRelationshipCalloutAsync(
        CollectionItemName destinationCalloutView,
        CollectionItemName relationshipName,
        double viewXPosition = 0.0,
        double viewYPosition = 0.0,
        IEnumerable<string>? additionalNotes = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreateRelationshipCalloutRequest(),
            new Dictionary<string, object?>
            {
                ["destination_callout_view"] = destinationCalloutView,
                ["relationship_name"] = relationshipName,
                ["view_x_position"] = viewXPosition,
                ["view_y_position"] = viewYPosition,
                ["additional_notes"] = additionalNotes,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "CreateRelationshipCallout",
            request,
            Transport.CreateRelationshipCalloutResult.Parser,
            cancellationToken);
    }

    public Task CreateTextCalloutAsync(
        CollectionItemName destinationCalloutView,
        IEnumerable<string> text,
        double viewXPosition = 0.4,
        double viewYPosition = 0.6,
        PointName? calloutAnchorPoint = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreateTextCalloutRequest(),
            new Dictionary<string, object?>
            {
                ["destination_callout_view"] = destinationCalloutView,
                ["text"] = text,
                ["view_x_position"] = viewXPosition,
                ["view_y_position"] = viewYPosition,
                ["callout_anchor_point"] = calloutAnchorPoint,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "CreateTextCallout",
            request,
            Transport.CreateTextCalloutResult.Parser,
            cancellationToken);
    }

    public Task CreateVectorCalloutAsync(
        CollectionItemName destinationCalloutView,
        CollectionObjectName vectorGroupName,
        string vectorName,
        double viewXPosition = 0.0,
        double viewYPosition = 0.0,
        bool showCollection = false,
        bool showVectorGroup = false,
        bool showVectorName = true,
        bool showDx = true,
        bool showDy = true,
        bool showDz = true,
        bool showDMag = true,
        bool showToleranceColor = true,
        bool showOutOfToleranceValue = false,
        bool showToleranceRange = false,
        bool showVectorColor = false,
        bool showStartPoint = false,
        bool showEndPoint = false,
        bool showUnits = false,
        IEnumerable<string>? additionalNotes = null,
        bool attachCalloutToEndPoint = false,
        bool useDefaultPlacement = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreateVectorCalloutRequest(),
            new Dictionary<string, object?>
            {
                ["destination_callout_view"] = destinationCalloutView,
                ["vector_group_name"] = vectorGroupName,
                ["vector_name"] = vectorName,
                ["view_x_position"] = viewXPosition,
                ["view_y_position"] = viewYPosition,
                ["show_collection"] = showCollection,
                ["show_vector_group"] = showVectorGroup,
                ["show_vector_name"] = showVectorName,
                ["show_dx"] = showDx,
                ["show_dy"] = showDy,
                ["show_dz"] = showDz,
                ["show_d_mag"] = showDMag,
                ["show_tolerance_color"] = showToleranceColor,
                ["show_out_of_tolerance_value"] = showOutOfToleranceValue,
                ["show_tolerance_range"] = showToleranceRange,
                ["show_vector_color"] = showVectorColor,
                ["show_start_point"] = showStartPoint,
                ["show_end_point"] = showEndPoint,
                ["show_units"] = showUnits,
                ["additional_notes"] = additionalNotes,
                ["attach_callout_to_end_point"] = attachCalloutToEndPoint,
                ["use_default_placement"] = useDefaultPlacement,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "CreateVectorCallout",
            request,
            Transport.CreateVectorCalloutResult.Parser,
            cancellationToken);
    }

    public Task<EulerXyzTransformComponents> DecomposeTransformIntoDoublesEulerXyzAsync(
        Transform inputTransform,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DecomposeTransformIntoDoublesEulerXyzRequest(),
            new Dictionary<string, object?>
            {
                ["input_transform"] = inputTransform,
            });
        return _client.InvokeOperationAsync<EulerXyzTransformComponents>(
            "briosa.ConstructionOperations",
            "DecomposeTransformIntoDoublesEulerXyz",
            request,
            Transport.DecomposeTransformIntoDoublesEulerXyzResult.Parser,
            cancellationToken);
    }

    public Task<EulerZxzTransformComponents> DecomposeTransformIntoDoublesEulerZxzAsync(
        Transform inputTransform,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DecomposeTransformIntoDoublesEulerZxzRequest(),
            new Dictionary<string, object?>
            {
                ["input_transform"] = inputTransform,
            });
        return _client.InvokeOperationAsync<EulerZxzTransformComponents>(
            "briosa.ConstructionOperations",
            "DecomposeTransformIntoDoublesEulerZxz",
            request,
            Transport.DecomposeTransformIntoDoublesEulerZxzResult.Parser,
            cancellationToken);
    }

    public Task<EulerZyxTransformComponents> DecomposeTransformIntoDoublesEulerZyxAsync(
        Transform inputTransform,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DecomposeTransformIntoDoublesEulerZyxRequest(),
            new Dictionary<string, object?>
            {
                ["input_transform"] = inputTransform,
            });
        return _client.InvokeOperationAsync<EulerZyxTransformComponents>(
            "briosa.ConstructionOperations",
            "DecomposeTransformIntoDoublesEulerZyx",
            request,
            Transport.DecomposeTransformIntoDoublesEulerZyxResult.Parser,
            cancellationToken);
    }

    public Task<EulerZyzTransformComponents> DecomposeTransformIntoDoublesEulerZyzAsync(
        Transform inputTransform,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DecomposeTransformIntoDoublesEulerZyzRequest(),
            new Dictionary<string, object?>
            {
                ["input_transform"] = inputTransform,
            });
        return _client.InvokeOperationAsync<EulerZyzTransformComponents>(
            "briosa.ConstructionOperations",
            "DecomposeTransformIntoDoublesEulerZyz",
            request,
            Transport.DecomposeTransformIntoDoublesEulerZyzResult.Parser,
            cancellationToken);
    }

    public Task<FixedXyzTransformComponents> DecomposeTransformIntoDoublesFixedXyzAsync(
        Transform inputTransform,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DecomposeTransformIntoDoublesFixedXyzRequest(),
            new Dictionary<string, object?>
            {
                ["input_transform"] = inputTransform,
            });
        return _client.InvokeOperationAsync<FixedXyzTransformComponents>(
            "briosa.ConstructionOperations",
            "DecomposeTransformIntoDoublesFixedXyz",
            request,
            Transport.DecomposeTransformIntoDoublesFixedXyzResult.Parser,
            cancellationToken);
    }

    public Task<FixedXyzTransformVectors> DecomposeTransformIntoVectorsFixedXyzAsync(
        Transform inputTransform,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DecomposeTransformIntoVectorsFixedXyzRequest(),
            new Dictionary<string, object?>
            {
                ["input_transform"] = inputTransform,
            });
        return _client.InvokeOperationAsync<FixedXyzTransformVectors>(
            "briosa.ConstructionOperations",
            "DecomposeTransformIntoVectorsFixedXyz",
            request,
            Transport.DecomposeTransformIntoVectorsFixedXyzResult.Parser,
            cancellationToken);
    }

    public Task<TransformAxes> DecomposeTransformIntoVectorsOriginAndAxesAsync(
        Transform transform,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DecomposeTransformIntoVectorsOriginAndAxesRequest(),
            new Dictionary<string, object?>
            {
                ["transform"] = transform,
            });
        return _client.InvokeOperationAsync<TransformAxes>(
            "briosa.ConstructionOperations",
            "DecomposeTransformIntoVectorsOriginAndAxes",
            request,
            Transport.DecomposeTransformIntoVectorsOriginAndAxesResult.Parser,
            cancellationToken);
    }

    public Task<WorldFixedXyzTransformComponents> DecomposeWorldTransformOperatorIntoDoublesFixedXyzInWorldAsync(
        WorldTransform inputWorldTransformOperator,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DecomposeWorldTransformOperatorIntoDoublesFixedXyzInWorldRequest(),
            new Dictionary<string, object?>
            {
                ["input_world_transform_operator"] = inputWorldTransformOperator,
            });
        return _client.InvokeOperationAsync<WorldFixedXyzTransformComponents>(
            "briosa.ConstructionOperations",
            "DecomposeWorldTransformOperatorIntoDoublesFixedXyzInWorld",
            request,
            Transport.DecomposeWorldTransformOperatorIntoDoublesFixedXyzInWorldResult.Parser,
            cancellationToken);
    }

    public Task<WorldFixedXyzTransformVectors> DecomposeWorldTransformOperatorIntoVectorsFixedXyzInWorldAsync(
        WorldTransform inputWorldTransformOperator,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DecomposeWorldTransformOperatorIntoVectorsFixedXyzInWorldRequest(),
            new Dictionary<string, object?>
            {
                ["input_world_transform_operator"] = inputWorldTransformOperator,
            });
        return _client.InvokeOperationAsync<WorldFixedXyzTransformVectors>(
            "briosa.ConstructionOperations",
            "DecomposeWorldTransformOperatorIntoVectorsFixedXyzInWorld",
            request,
            Transport.DecomposeWorldTransformOperatorIntoVectorsFixedXyzInWorldResult.Parser,
            cancellationToken);
    }

    public Task DeleteCalloutViewAsync(
        CollectionItemName calloutView,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteCalloutViewRequest(),
            new Dictionary<string, object?>
            {
                ["callout_view"] = calloutView,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "DeleteCalloutView",
            request,
            Transport.DeleteCalloutViewResult.Parser,
            cancellationToken);
    }

    public Task DeleteCollectionAsync(
        CollectionName collectionName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteCollectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_name"] = collectionName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "DeleteCollection",
            request,
            Transport.DeleteCollectionResult.Parser,
            cancellationToken);
    }

    public Task<DeleteCollectionsByWildcardResult> DeleteCollectionsByWildcardAsync(
        string searchString,
        bool caseSensitiveSearch = true,
        bool allowDeletingAllCollections = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteCollectionsByWildcardRequest(),
            new Dictionary<string, object?>
            {
                ["search_string"] = searchString,
                ["case_sensitive_search"] = caseSensitiveSearch,
                ["allow_deleting_all_collections"] = allowDeletingAllCollections,
            });
        return _client.InvokeOperationAsync<DeleteCollectionsByWildcardResult>(
            "briosa.ConstructionOperations",
            "DeleteCollectionsByWildcard",
            request,
            Transport.DeleteCollectionsByWildcardResult.Parser,
            cancellationToken);
    }

    public Task<DeleteFoldersByWildcardResult> DeleteFoldersByWildcardAsync(
        string searchString,
        bool caseSensitiveSearch = true,
        bool allowDeletingAllFolders = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteFoldersByWildcardRequest(),
            new Dictionary<string, object?>
            {
                ["search_string"] = searchString,
                ["case_sensitive_search"] = caseSensitiveSearch,
                ["allow_deleting_all_folders"] = allowDeletingAllFolders,
            });
        return _client.InvokeOperationAsync<DeleteFoldersByWildcardResult>(
            "briosa.ConstructionOperations",
            "DeleteFoldersByWildcard",
            request,
            Transport.DeleteFoldersByWildcardResult.Parser,
            cancellationToken);
    }

    public Task DeleteHiddenPointRodAsync(
        int hiddenPointRodIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteHiddenPointRodRequest(),
            new Dictionary<string, object?>
            {
                ["hidden_point_rod_index"] = hiddenPointRodIndex,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "DeleteHiddenPointRod",
            request,
            Transport.DeleteHiddenPointRodResult.Parser,
            cancellationToken);
    }

    public Task<int> ExtractSphereCentersFromPointCloudAsync(
        CollectionObjectName cloudName,
        CollectionObjectName groupNameForPoints,
        double desiredDiameter = 0.0,
        double extractionTolerance = 0.0,
        int minimumPointCount = 50,
        bool performFinalFit = true,
        double finalFitConeAngle = 120.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExtractSphereCentersFromPointCloudRequest(),
            new Dictionary<string, object?>
            {
                ["cloud_name"] = cloudName,
                ["desired_diameter"] = desiredDiameter,
                ["extraction_tolerance"] = extractionTolerance,
                ["minimum_point_count"] = minimumPointCount,
                ["group_name_for_points"] = groupNameForPoints,
                ["perform_final_fit"] = performFinalFit,
                ["final_fit_cone_angle"] = finalFitConeAngle,
            });
        return _client.InvokeOperationAsync<int>(
            "briosa.ConstructionOperations",
            "ExtractSphereCentersFromPointCloud",
            request,
            Transport.ExtractSphereCentersFromPointCloudResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionInstrumentId>> GetCollectionInstrumentRefListVariableAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCollectionInstrumentRefListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionInstrumentId>>(
            "briosa.ConstructionOperations",
            "GetCollectionInstrumentRefListVariable",
            request,
            Transport.GetCollectionInstrumentRefListVariableResult.Parser,
            cancellationToken);
    }

    public Task<int> GetHiddenPointRodIndexByNameAsync(
        string hiddenPointRodName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetHiddenPointRodIndexByNameRequest(),
            new Dictionary<string, object?>
            {
                ["hidden_point_rod_name"] = hiddenPointRodName,
            });
        return _client.InvokeOperationAsync<int>(
            "briosa.ConstructionOperations",
            "GetHiddenPointRodIndexByName",
            request,
            Transport.GetHiddenPointRodIndexByNameResult.Parser,
            cancellationToken);
    }

    public Task<CalloutPosition> GetIthCalloutPositionInCalloutViewAsync(
        CollectionItemName calloutView,
        int calloutViewIndex,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetIthCalloutPositionInCalloutViewRequest(),
            new Dictionary<string, object?>
            {
                ["callout_view"] = calloutView,
                ["callout_view_index"] = calloutViewIndex,
            });
        return _client.InvokeOperationAsync<CalloutPosition>(
            "briosa.ConstructionOperations",
            "GetIthCalloutPositionInCalloutView",
            request,
            Transport.GetIthCalloutPositionInCalloutViewResult.Parser,
            cancellationToken);
    }

    public Task<int> GetNumberOfCalloutsInCalloutViewAsync(
        CollectionItemName calloutView,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetNumberOfCalloutsInCalloutViewRequest(),
            new Dictionary<string, object?>
            {
                ["callout_view"] = calloutView,
            });
        return _client.InvokeOperationAsync<int>(
            "briosa.ConstructionOperations",
            "GetNumberOfCalloutsInCalloutView",
            request,
            Transport.GetNumberOfCalloutsInCalloutViewResult.Parser,
            cancellationToken);
    }

    public Task<Transform> GetWorkingTransformOfObjectFixedXyzAsync(
        CollectionObjectName objectName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetWorkingTransformOfObjectFixedXyzRequest(),
            new Dictionary<string, object?>
            {
                ["object_name"] = objectName,
            });
        return _client.InvokeOperationAsync<Transform>(
            "briosa.ConstructionOperations",
            "GetWorkingTransformOfObjectFixedXyz",
            request,
            Transport.GetWorkingTransformOfObjectFixedXyzResult.Parser,
            cancellationToken);
    }

    public Task<Transform> InvertTransformAsync(
        Transform transform,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.InvertTransformRequest(),
            new Dictionary<string, object?>
            {
                ["transform"] = transform,
            });
        return _client.InvokeOperationAsync<Transform>(
            "briosa.ConstructionOperations",
            "InvertTransform",
            request,
            Transport.InvertTransformResult.Parser,
            cancellationToken);
    }

    public Task<CollectionInstrumentId> MakeCollectionInstrumentIdRuntimeSelectAsync(
        string userPrompt = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionInstrumentIdRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["user_prompt"] = userPrompt,
            });
        return _client.InvokeOperationAsync<CollectionInstrumentId>(
            "briosa.ConstructionOperations",
            "MakeCollectionInstrumentIdRuntimeSelect",
            request,
            Transport.MakeCollectionInstrumentIdRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionInstrumentId>> MakeCollectionInstrumentRefListRuntimeSelectAsync(
        string userPrompt = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionInstrumentRefListRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["user_prompt"] = userPrompt,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionInstrumentId>>(
            "briosa.ConstructionOperations",
            "MakeCollectionInstrumentRefListRuntimeSelect",
            request,
            Transport.MakeCollectionInstrumentRefListRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionItemName>> MakeCollectionItemNameRefListWildcardSelectionAsync(
        string collectionWildcardCriteria = "*",
        string itemWildcardCriteria = "*",
        ItemType itemType = ItemType.Any,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionItemNameRefListWildcardSelectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_wildcard_criteria"] = collectionWildcardCriteria,
                ["item_wildcard_criteria"] = itemWildcardCriteria,
                ["item_type"] = itemType,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionItemName>>(
            "briosa.ConstructionOperations",
            "MakeCollectionItemNameRefListWildcardSelection",
            request,
            Transport.MakeCollectionItemNameRefListWildcardSelectionResult.Parser,
            cancellationToken);
    }

    public Task<CollectionName> MakeCollectionNameRuntimeSelectAsync(
        string userPrompt = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionNameRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["user_prompt"] = userPrompt,
            });
        return _client.InvokeOperationAsync<CollectionName>(
            "briosa.ConstructionOperations",
            "MakeCollectionNameRuntimeSelect",
            request,
            Transport.MakeCollectionNameRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> MakeCollectionObjectNameRefListByTypeAsync(
        string collection,
        ObjectType objectType = ObjectType.Any,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionObjectNameRefListByTypeRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
                ["object_type"] = objectType,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "MakeCollectionObjectNameRefListByType",
            request,
            Transport.MakeCollectionObjectNameRefListByTypeResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> MakeCollectionObjectNameRefListByTypeAndColorAsync(
        string collection,
        ObjectType objectType = ObjectType.Any,
        Color? objectColor = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionObjectNameRefListByTypeAndColorRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
                ["object_type"] = objectType,
                ["object_color"] = objectColor,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "MakeCollectionObjectNameRefListByTypeAndColor",
            request,
            Transport.MakeCollectionObjectNameRefListByTypeAndColorResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> MakeCollectionObjectNameRefListFromAllGroupsInCollectionAsync(
        CollectionName collectionName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionObjectNameRefListFromAllGroupsInCollectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_name"] = collectionName,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "MakeCollectionObjectNameRefListFromAllGroupsInCollection",
            request,
            Transport.MakeCollectionObjectNameRefListFromAllGroupsInCollectionResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> MakeCollectionObjectNameRefListRuntimeSelectAsync(
        string userPrompt = "",
        ObjectType objectType = ObjectType.Any,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionObjectNameRefListRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["user_prompt"] = userPrompt,
                ["object_type"] = objectType,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "MakeCollectionObjectNameRefListRuntimeSelect",
            request,
            Transport.MakeCollectionObjectNameRefListRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> MakeCollectionObjectNameRefListWildcardSelectionAsync(
        string collectionWildcardCriteria = "*",
        string objectWildcardCriteria = "*",
        ObjectType objectType = ObjectType.Any,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionObjectNameRefListWildcardSelectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_wildcard_criteria"] = collectionWildcardCriteria,
                ["object_wildcard_criteria"] = objectWildcardCriteria,
                ["object_type"] = objectType,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "MakeCollectionObjectNameRefListWildcardSelection",
            request,
            Transport.MakeCollectionObjectNameRefListWildcardSelectionResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName> MakeCollectionObjectNameRuntimeSelectAsync(
        string userPrompt = "",
        ObjectType objectType = ObjectType.Any,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionObjectNameRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["user_prompt"] = userPrompt,
                ["object_type"] = objectType,
            });
        return _client.InvokeOperationAsync<CollectionObjectName>(
            "briosa.ConstructionOperations",
            "MakeCollectionObjectNameRuntimeSelect",
            request,
            Transport.MakeCollectionObjectNameRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionVectorGroupName>> MakeCollectionVectorGroupNameRefListRuntimeSelectAsync(
        string userPrompt = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionVectorGroupNameRefListRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["user_prompt"] = userPrompt,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionVectorGroupName>>(
            "briosa.ConstructionOperations",
            "MakeCollectionVectorGroupNameRefListRuntimeSelect",
            request,
            Transport.MakeCollectionVectorGroupNameRefListRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionItemName>> MakeEventRefListWildcardSelectionAsync(
        string collectionWildcardCriteria = "*",
        string eventWildcardCriteria = "*",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeEventRefListWildcardSelectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_wildcard_criteria"] = collectionWildcardCriteria,
                ["event_wildcard_criteria"] = eventWildcardCriteria,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionItemName>>(
            "briosa.ConstructionOperations",
            "MakeEventRefListWildcardSelection",
            request,
            Transport.MakeEventRefListWildcardSelectionResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionItemName>> MakePictureNameRefListRuntimeSelectAsync(
        string userPrompt = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePictureNameRefListRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["user_prompt"] = userPrompt,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionItemName>>(
            "briosa.ConstructionOperations",
            "MakePictureNameRefListRuntimeSelect",
            request,
            Transport.MakePictureNameRefListRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<PointName>> MakePointNameRefListFromGroupAsync(
        CollectionObjectName groupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePointNameRefListFromGroupRequest(),
            new Dictionary<string, object?>
            {
                ["group_name"] = groupName,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<PointName>>(
            "briosa.ConstructionOperations",
            "MakePointNameRefListFromGroup",
            request,
            Transport.MakePointNameRefListFromGroupResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<PointName>> MakePointNameRefListRuntimeSelectAsync(
        string userPrompt = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePointNameRefListRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["user_prompt"] = userPrompt,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<PointName>>(
            "briosa.ConstructionOperations",
            "MakePointNameRefListRuntimeSelect",
            request,
            Transport.MakePointNameRefListRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<PointName>> MakePointNameRefListWildcardSelectAsync(
        string collectionWildcardCriteria = "*",
        string groupNameWildcardCriteria = "*",
        string pointNameWildcardCriteria = "*",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePointNameRefListWildcardSelectRequest(),
            new Dictionary<string, object?>
            {
                ["collection_wildcard_criteria"] = collectionWildcardCriteria,
                ["group_name_wildcard_criteria"] = groupNameWildcardCriteria,
                ["point_name_wildcard_criteria"] = pointNameWildcardCriteria,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<PointName>>(
            "briosa.ConstructionOperations",
            "MakePointNameRefListWildcardSelect",
            request,
            Transport.MakePointNameRefListWildcardSelectResult.Parser,
            cancellationToken);
    }

    public Task<PointName> MakePointNameRuntimeSelectAsync(
        string userPrompt = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePointNameRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["user_prompt"] = userPrompt,
            });
        return _client.InvokeOperationAsync<PointName>(
            "briosa.ConstructionOperations",
            "MakePointNameRuntimeSelect",
            request,
            Transport.MakePointNameRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionItemName>> MakeReportRefListFromCollectionAsync(
        CollectionName collectionName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeReportRefListFromCollectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_name"] = collectionName,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionItemName>>(
            "briosa.ConstructionOperations",
            "MakeReportRefListFromCollection",
            request,
            Transport.MakeReportRefListFromCollectionResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionItemName>> MakeReportRefListRuntimeSelectAsync(
        string userPrompt = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeReportRefListRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["user_prompt"] = userPrompt,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionItemName>>(
            "briosa.ConstructionOperations",
            "MakeReportRefListRuntimeSelect",
            request,
            Transport.MakeReportRefListRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<string> MakeSystemStringAsync(
        SystemString stringContent,
        string? formatString = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeSystemStringRequest(),
            new Dictionary<string, object?>
            {
                ["string_content"] = stringContent,
                ["format_string"] = formatString,
            });
        return _client.InvokeOperationAsync<string>(
            "briosa.ConstructionOperations",
            "MakeSystemString",
            request,
            Transport.MakeSystemStringResult.Parser,
            cancellationToken);
    }

    public Task<Transform> MakeTransformFromDoublesEulerParametersAsync(
        double x = 0,
        double y = 0,
        double z = 0,
        double e1 = 0,
        double e2 = 0,
        double e3 = 0,
        double e4 = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeTransformFromDoublesEulerParametersRequest(),
            new Dictionary<string, object?>
            {
                ["x"] = x,
                ["y"] = y,
                ["z"] = z,
                ["e1"] = e1,
                ["e2"] = e2,
                ["e3"] = e3,
                ["e4"] = e4,
            });
        return _client.InvokeOperationAsync<Transform>(
            "briosa.ConstructionOperations",
            "MakeTransformFromDoublesEulerParameters",
            request,
            Transport.MakeTransformFromDoublesEulerParametersResult.Parser,
            cancellationToken);
    }

    public Task<Transform> MakeTransformFromDoublesFixedXyzAsync(
        double x = 0,
        double y = 0,
        double z = 0,
        double rx = 0,
        double ry = 0,
        double rz = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeTransformFromDoublesFixedXyzRequest(),
            new Dictionary<string, object?>
            {
                ["x"] = x,
                ["y"] = y,
                ["z"] = z,
                ["rx"] = rx,
                ["ry"] = ry,
                ["rz"] = rz,
            });
        return _client.InvokeOperationAsync<Transform>(
            "briosa.ConstructionOperations",
            "MakeTransformFromDoublesFixedXyz",
            request,
            Transport.MakeTransformFromDoublesFixedXyzResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<VectorName>> MakeVectorNameRefListFromVectorGroupAsync(
        CollectionObjectName vectorGroupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeVectorNameRefListFromVectorGroupRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group_name"] = vectorGroupName,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<VectorName>>(
            "briosa.ConstructionOperations",
            "MakeVectorNameRefListFromVectorGroup",
            request,
            Transport.MakeVectorNameRefListFromVectorGroupResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<VectorName>> MakeVectorNameRefListRuntimeSelectAsync(
        string userPrompt = " Select Vectors (ENTER when done) ",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeVectorNameRefListRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["user_prompt"] = userPrompt,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<VectorName>>(
            "briosa.ConstructionOperations",
            "MakeVectorNameRefListRuntimeSelect",
            request,
            Transport.MakeVectorNameRefListRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task MakeVectorNamesUniqueInVectorGroupAsync(
        CollectionObjectName vectorGroupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeVectorNamesUniqueInVectorGroupRequest(),
            new Dictionary<string, object?>
            {
                ["vector_group_name"] = vectorGroupName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "MakeVectorNamesUniqueInVectorGroup",
            request,
            Transport.MakeVectorNamesUniqueInVectorGroupResult.Parser,
            cancellationToken);
    }

    public Task RenameCalloutViewAsync(
        CollectionItemName originalCalloutViewName,
        CollectionItemName newCalloutViewName,
        bool overwriteIfExists = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RenameCalloutViewRequest(),
            new Dictionary<string, object?>
            {
                ["original_callout_view_name"] = originalCalloutViewName,
                ["new_callout_view_name"] = newCalloutViewName,
                ["overwrite_if_exists"] = overwriteIfExists,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "RenameCalloutView",
            request,
            Transport.RenameCalloutViewResult.Parser,
            cancellationToken);
    }

    public Task SetCollectionInstrumentRefListVariableAsync(
        string name,
        IEnumerable<CollectionInstrumentId> value,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCollectionInstrumentRefListVariableRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
                ["value"] = value,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "SetCollectionInstrumentRefListVariable",
            request,
            Transport.SetCollectionInstrumentRefListVariableResult.Parser,
            cancellationToken);
    }

    public Task SetIthCalloutPositionInCalloutViewAsync(
        CollectionItemName calloutView,
        int calloutViewIndex,
        int xPosition,
        int yPosition,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetIthCalloutPositionInCalloutViewRequest(),
            new Dictionary<string, object?>
            {
                ["callout_view"] = calloutView,
                ["callout_view_index"] = calloutViewIndex,
                ["x_position"] = xPosition,
                ["y_position"] = yPosition,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "SetIthCalloutPositionInCalloutView",
            request,
            Transport.SetIthCalloutPositionInCalloutViewResult.Parser,
            cancellationToken);
    }

    public Task SetOrConstructDefaultCollectionAsync(
        CollectionName collectionName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetOrConstructDefaultCollectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_name"] = collectionName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "SetOrConstructDefaultCollection",
            request,
            Transport.SetOrConstructDefaultCollectionResult.Parser,
            cancellationToken);
    }

    public Task SetPointPositionInWorkingCoordinatesAsync(
        PointName pointName,
        Vector positionInWorkingCoordinates,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPointPositionInWorkingCoordinatesRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
                ["position_in_working_coordinates"] = positionInWorkingCoordinates,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "SetPointPositionInWorkingCoordinates",
            request,
            Transport.SetPointPositionInWorkingCoordinatesResult.Parser,
            cancellationToken);
    }

    public Task ShiftPlaneAsync(
        CollectionObjectName plane,
        double shiftAlongNormal = 0.0,
        double growBoundsByFactor = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ShiftPlaneRequest(),
            new Dictionary<string, object?>
            {
                ["plane"] = plane,
                ["shift_along_normal"] = shiftAlongNormal,
                ["grow_bounds_by_factor"] = growBoundsByFactor,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ShiftPlane",
            request,
            Transport.ShiftPlaneResult.Parser,
            cancellationToken);
    }

    public Task TransformPointsByDeltaAboutWorkingFrameAsync(
        IEnumerable<PointName> pointNameList,
        Vector deltaInWorkingCoordinates,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.TransformPointsByDeltaAboutWorkingFrameRequest(),
            new Dictionary<string, object?>
            {
                ["point_name_list"] = pointNameList,
                ["delta_in_working_coordinates"] = deltaInWorkingCoordinates,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "TransformPointsByDeltaAboutWorkingFrame",
            request,
            Transport.TransformPointsByDeltaAboutWorkingFrameResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionInstrumentId>> AddCollectionInstrumentsToRefListWildcardSelectionAsync(
        IEnumerable<CollectionInstrumentId> collectionInstrumentRefList,
        string collectionWildcardCriteria = "*",
        string instrumentWildcardCriteria = "*",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddCollectionInstrumentsToRefListWildcardSelectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_instrument_ref_list"] = collectionInstrumentRefList,
                ["collection_wildcard_criteria"] = collectionWildcardCriteria,
                ["instrument_wildcard_criteria"] = instrumentWildcardCriteria,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionInstrumentId>>(
            "briosa.ConstructionOperations",
            "AddCollectionInstrumentsToRefListWildcardSelection",
            request,
            Transport.AddCollectionInstrumentsToRefListWildcardSelectionResult.Parser,
            cancellationToken);
    }

    public Task<GroupAverageResult> AverageSetOfGroupsAsync(
        IEnumerable<CollectionObjectName> groupNames,
        CollectionObjectName resultingGroupName,
        double rmsTolerance = 0.0,
        double maximumAbsoluteTolerance = 0.0,
        double maximumAverageTolerance = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AverageSetOfGroupsRequest(),
            new Dictionary<string, object?>
            {
                ["group_names"] = groupNames,
                ["resulting_group_name"] = resultingGroupName,
                ["rms_tolerance"] = rmsTolerance,
                ["maximum_absolute_tolerance"] = maximumAbsoluteTolerance,
                ["maximum_average_tolerance"] = maximumAverageTolerance,
            });
        return _client.InvokeOperationAsync<GroupAverageResult>(
            "briosa.ConstructionOperations",
            "AverageSetOfGroups",
            request,
            Transport.AverageSetOfGroupsResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> ConstructGeometryFromSurfacesAsync(
        IEnumerable<CollectionObjectName> surfaces,
        double minimumDiameter = 0.0,
        double maximumDiameter = 0.0,
        CollectionObjectName? referenceFrame = null,
        CollectionName? destinationCollectionName = null,
        string baseName = "Geometry Object",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructGeometryFromSurfacesRequest(),
            new Dictionary<string, object?>
            {
                ["surfaces"] = surfaces,
                ["minimum_diameter"] = minimumDiameter,
                ["maximum_diameter"] = maximumDiameter,
                ["reference_frame"] = referenceFrame,
                ["destination_collection_name"] = destinationCollectionName,
                ["base_name"] = baseName,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.ConstructionOperations",
            "ConstructGeometryFromSurfaces",
            request,
            Transport.ConstructGeometryFromSurfacesResult.Parser,
            cancellationToken);
    }

    public Task<ObjectOriginResult> ConstructPointAtObjectOriginAsync(
        CollectionObjectName objectName,
        PointName resultantPointName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPointAtObjectOriginRequest(),
            new Dictionary<string, object?>
            {
                ["object_name"] = objectName,
                ["resultant_point_name"] = resultantPointName,
            });
        return _client.InvokeOperationAsync<ObjectOriginResult>(
            "briosa.ConstructionOperations",
            "ConstructPointAtObjectOrigin",
            request,
            Transport.ConstructPointAtObjectOriginResult.Parser,
            cancellationToken);
    }

    public Task<ProjectedPointGradient> GetGradientAtProjectedPointOnSurfaceAsync(
        PointName pointToProject,
        CollectionObjectName surfaceName,
        bool generateOutputVectorLines = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGradientAtProjectedPointOnSurfaceRequest(),
            new Dictionary<string, object?>
            {
                ["point_to_project"] = pointToProject,
                ["surface_name"] = surfaceName,
                ["generate_output_vector_lines"] = generateOutputVectorLines,
            });
        return _client.InvokeOperationAsync<ProjectedPointGradient>(
            "briosa.ConstructionOperations",
            "GetGradientAtProjectedPointOnSurface",
            request,
            Transport.GetGradientAtProjectedPointOnSurfaceResult.Parser,
            cancellationToken);
    }

    public Task<ProjectedPointGradient> GetGradientAtProjectedPointOnSurfaceEdgeAsync(
        PointName pointToProject,
        CollectionObjectName surfaceEdgeBSpline,
        CollectionObjectName surfaceName,
        Vector? edgeOffsetDirection = null,
        double edgeOffsetDistance = 0.01,
        bool generateOutputVectorLines = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGradientAtProjectedPointOnSurfaceEdgeRequest(),
            new Dictionary<string, object?>
            {
                ["point_to_project"] = pointToProject,
                ["surface_edge_b_spline"] = surfaceEdgeBSpline,
                ["surface_name"] = surfaceName,
                ["edge_offset_direction"] = edgeOffsetDirection,
                ["edge_offset_distance"] = edgeOffsetDistance,
                ["generate_output_vector_lines"] = generateOutputVectorLines,
            });
        return _client.InvokeOperationAsync<ProjectedPointGradient>(
            "briosa.ConstructionOperations",
            "GetGradientAtProjectedPointOnSurfaceEdge",
            request,
            Transport.GetGradientAtProjectedPointOnSurfaceEdgeResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionItemName>> MakeCalloutViewRefListWildcardSelectionAsync(
        string collectionWildcardCriteria = "*",
        string calloutViewWildcardCriteria = "*",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCalloutViewRefListWildcardSelectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_wildcard_criteria"] = collectionWildcardCriteria,
                ["callout_view_wildcard_criteria"] = calloutViewWildcardCriteria,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionItemName>>(
            "briosa.ConstructionOperations",
            "MakeCalloutViewRefListWildcardSelection",
            request,
            Transport.MakeCalloutViewRefListWildcardSelectionResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName> MakeCollectionObjectNameEnsureUniqueAsync(
        CollectionObjectName collectionObjectName,
        bool useNumberSuffix = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionObjectNameEnsureUniqueRequest(),
            new Dictionary<string, object?>
            {
                ["collection_object_name"] = collectionObjectName,
                ["use_number_suffix"] = useNumberSuffix,
            });
        return _client.InvokeOperationAsync<CollectionObjectName>(
            "briosa.ConstructionOperations",
            "MakeCollectionObjectNameEnsureUnique",
            request,
            Transport.MakeCollectionObjectNameEnsureUniqueResult.Parser,
            cancellationToken);
    }

    public Task<PointName> MakePointNameEnsureUniqueAsync(
        PointName pointName,
        bool useNumberSuffix = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakePointNameEnsureUniqueRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
                ["use_number_suffix"] = useNumberSuffix,
            });
        return _client.InvokeOperationAsync<PointName>(
            "briosa.ConstructionOperations",
            "MakePointNameEnsureUnique",
            request,
            Transport.MakePointNameEnsureUniqueResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionItemName>> MakeRelationshipRefListRuntimeSelectAsync(
        string userPrompt = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeRelationshipRefListRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
                ["user_prompt"] = userPrompt,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionItemName>>(
            "briosa.ConstructionOperations",
            "MakeRelationshipRefListRuntimeSelect",
            request,
            Transport.MakeRelationshipRefListRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionItemName>> MakeRelationshipRefListWildcardSelectionAsync(
        string collectionWildcardCriteria = "*",
        string relationshipWildcardCriteria = "*",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeRelationshipRefListWildcardSelectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_wildcard_criteria"] = collectionWildcardCriteria,
                ["relationship_wildcard_criteria"] = relationshipWildcardCriteria,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionItemName>>(
            "briosa.ConstructionOperations",
            "MakeRelationshipRefListWildcardSelection",
            request,
            Transport.MakeRelationshipRefListWildcardSelectionResult.Parser,
            cancellationToken);
    }

    public Task SetDefaultCalloutViewPropertiesAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetDefaultCalloutViewPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["default_callout_view_name"] = "Callout 1",
                ["properties"] = new CalloutViewProperties(),
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "SetDefaultCalloutViewProperties",
            request,
            Transport.SetDefaultCalloutViewPropertiesResult.Parser,
            cancellationToken);
    }

    public Task SetDefaultCalloutViewPropertiesAsync(
        string defaultCalloutViewName,
        CalloutViewProperties properties,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetDefaultCalloutViewPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["default_callout_view_name"] = defaultCalloutViewName,
                ["properties"] = properties,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "SetDefaultCalloutViewProperties",
            request,
            Transport.SetDefaultCalloutViewPropertiesResult.Parser,
            cancellationToken);
    }

    public Task SetCalloutViewPropertiesAsync(
        IEnumerable<CollectionItemName> calloutViews,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalloutViewPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["callout_views"] = calloutViews,
                ["properties"] = new CalloutViewProperties(),
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "SetCalloutViewProperties",
            request,
            Transport.SetCalloutViewPropertiesResult.Parser,
            cancellationToken);
    }

    public Task SetCalloutViewPropertiesAsync(
        IEnumerable<CollectionItemName> calloutViews,
        CalloutViewProperties properties,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalloutViewPropertiesRequest(),
            new Dictionary<string, object?>
            {
                ["callout_views"] = calloutViews,
                ["properties"] = properties,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "SetCalloutViewProperties",
            request,
            Transport.SetCalloutViewPropertiesResult.Parser,
            cancellationToken);
    }

    public Task ConstructPolygonizedSurfaceFromPointCloudsAsync(
        IEnumerable<CollectionObjectName> pointCloudList,
        MeshOrientationType meshOrientation,
        CollectionObjectName polygonizedSurfaceName,
        double gridResolution = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPolygonizedSurfaceFromPointCloudsRequest(),
            new Dictionary<string, object?>
            {
                ["point_cloud_list"] = pointCloudList,
                ["mesh_orientation"] = meshOrientation,
                ["grid_resolution"] = gridResolution,
                ["polygonized_surface_name"] = polygonizedSurfaceName,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructPolygonizedSurfaceFromPointClouds",
            request,
            Transport.ConstructPolygonizedSurfaceFromPointCloudsResult.Parser,
            cancellationToken);
    }

    public Task ConstructScaleBarAsync(
        CollectionItemName scaleBarName,
        PointName beginTarget,
        PointName endTarget,
        double length = 0.0,
        double uncertainty = 0.0,
        bool useRelativeTolerances = true,
        bool useHighTolerances = false,
        bool useLowTolerances = false,
        double highTolerance = 0.0,
        double lowTolerance = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructScaleBarRequest(),
            new Dictionary<string, object?>
            {
                ["scale_bar_name"] = scaleBarName,
                ["begin_target"] = beginTarget,
                ["end_target"] = endTarget,
                ["length"] = length,
                ["uncertainty"] = uncertainty,
                ["use_relative_tolerances"] = useRelativeTolerances,
                ["use_high_tolerances"] = useHighTolerances,
                ["use_low_tolerances"] = useLowTolerances,
                ["high_tolerance"] = highTolerance,
                ["low_tolerance"] = lowTolerance,
            });
        return _client.InvokeOperationAsync(
            "briosa.ConstructionOperations",
            "ConstructScaleBar",
            request,
            Transport.ConstructScaleBarResult.Parser,
            cancellationToken);
    }
}

public sealed class BriosaGdtOperations
{
    private readonly BriosaClient _client;
    internal BriosaGdtOperations(BriosaClient client) => _client = client;

    public Task DatumAlignmentAsync(
        CollectionItemName featureCheck,
        IEnumerable<CollectionObjectName> objectsToMove,
        IEnumerable<CollectionInstrumentId> instrumentsToMove,
        bool applyFeatureCheckTransform = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DatumAlignmentRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
                ["objects_to_move"] = objectsToMove,
                ["instruments_to_move"] = instrumentsToMove,
                ["apply_feature_check_transform"] = applyFeatureCheckTransform,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "DatumAlignment",
            request,
            Transport.DatumAlignmentResult.Parser,
            cancellationToken);
    }

    public Task DeleteFeatureChecksAsync(
        IEnumerable<CollectionItemName> featureChecks,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteFeatureChecksRequest(),
            new Dictionary<string, object?>
            {
                ["feature_checks"] = featureChecks,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "DeleteFeatureChecks",
            request,
            Transport.DeleteFeatureChecksResult.Parser,
            cancellationToken);
    }

    public Task EnableDisableDatumAlignmentForFeatureCheckAsync(
        CollectionItemName featureCheck,
        bool enableDatumAlignment = true,
        bool enableCustomInitialAlignment = false,
        bool enableInitialDatumAlignment = true,
        CollectionItemName? alignment = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EnableDisableDatumAlignmentForFeatureCheckRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
                ["enable_datum_alignment"] = enableDatumAlignment,
                ["enable_custom_initial_alignment"] = enableCustomInitialAlignment,
                ["enable_initial_datum_alignment"] = enableInitialDatumAlignment,
                ["alignment"] = alignment,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "EnableDisableDatumAlignmentForFeatureCheck",
            request,
            Transport.EnableDisableDatumAlignmentForFeatureCheckResult.Parser,
            cancellationToken);
    }

    public Task<EvaluateFeatureCheckResult> EvaluateFeatureCheckAsync(
        CollectionItemName featureCheck,
        bool performEvaluation = true,
        bool simultaneousEvaluation = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EvaluateFeatureCheckRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
                ["perform_evaluation"] = performEvaluation,
                ["simultaneous_evaluation"] = simultaneousEvaluation,
            });
        return _client.InvokeOperationAsync<EvaluateFeatureCheckResult>(
            "briosa.GdtOperations",
            "EvaluateFeatureCheck",
            request,
            Transport.EvaluateFeatureCheckResult.Parser,
            cancellationToken);
    }

    public Task<EvaluateFeatureChecksResult> EvaluateFeatureChecksAsync(
        IEnumerable<CollectionItemName> featureCheckList,
        bool simultaneousEvaluation = false,
        bool restrictEvaluationsToListedChecks = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EvaluateFeatureChecksRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check_list"] = featureCheckList,
                ["simultaneous_evaluation"] = simultaneousEvaluation,
                ["restrict_evaluations_to_listed_checks"] = restrictEvaluationsToListedChecks,
            });
        return _client.InvokeOperationAsync<EvaluateFeatureChecksResult>(
            "briosa.GdtOperations",
            "EvaluateFeatureChecks",
            request,
            Transport.EvaluateFeatureChecksResult.Parser,
            cancellationToken);
    }

    public Task FeatureInspectionAutoFilterAsync(
        IEnumerable<PointName>? pointNames = null,
        IEnumerable<CollectionObjectName>? groupNames = null,
        IEnumerable<CollectionObjectName>? cloudNames = null,
        double surfaceOffset = 0.1,
        double edgeOffset = 0.1,
        OffsetDirectionType offsetDirection = OffsetDirectionType.Both,
        bool includePointsWithinCylinderAxisProximity = false,
        bool enforceMaxPointsPerFaceInOutput = false,
        int maxPointsPerFace = 0,
        IEnumerable<CollectionItemName>? featureCheckNameList = null,
        bool includeDatums = true,
        bool createCloudForEachDatumOrCheck = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FeatureInspectionAutoFilterRequest(),
            new Dictionary<string, object?>
            {
                ["point_names"] = pointNames,
                ["group_names"] = groupNames,
                ["cloud_names"] = cloudNames,
                ["surface_offset"] = surfaceOffset,
                ["edge_offset"] = edgeOffset,
                ["offset_direction"] = offsetDirection,
                ["include_points_within_cylinder_axis_proximity"] = includePointsWithinCylinderAxisProximity,
                ["enforce_max_points_per_face_in_output"] = enforceMaxPointsPerFaceInOutput,
                ["max_points_per_face"] = maxPointsPerFace,
                ["feature_check_name_list"] = featureCheckNameList,
                ["include_datums"] = includeDatums,
                ["create_cloud_for_each_datum_or_check"] = createCloudForEachDatumOrCheck,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "FeatureInspectionAutoFilter",
            request,
            Transport.FeatureInspectionAutoFilterResult.Parser,
            cancellationToken);
    }

    public Task GenerateFeatureCheckSummaryAsync(
        IEnumerable<CollectionItemName> featureCheckList,
        string summaryTableName = "GDT Feature Check Summary",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GenerateFeatureCheckSummaryRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check_list"] = featureCheckList,
                ["summary_table_name"] = summaryTableName,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "GenerateFeatureCheckSummary",
            request,
            Transport.GenerateFeatureCheckSummaryResult.Parser,
            cancellationToken);
    }

    public Task<GdtMeasurements> GetDatumMeasurementsAsync(
        CollectionItemName datum,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetDatumMeasurementsRequest(),
            new Dictionary<string, object?>
            {
                ["datum"] = datum,
            });
        return _client.InvokeOperationAsync<GdtMeasurements>(
            "briosa.GdtOperations",
            "GetDatumMeasurements",
            request,
            Transport.GetDatumMeasurementsResult.Parser,
            cancellationToken);
    }

    public Task<FeatureCheckCylinderEvalOptions> GetFeatureCheckCylinderEvalOptionsAsync(
        CollectionItemName featureCheck,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetFeatureCheckCylinderEvalOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
            });
        return _client.InvokeOperationAsync<FeatureCheckCylinderEvalOptions>(
            "briosa.GdtOperations",
            "GetFeatureCheckCylinderEvalOptions",
            request,
            Transport.GetFeatureCheckCylinderEvalOptionsResult.Parser,
            cancellationToken);
    }

    public Task<FeatureCheckDatumReferencesResult> GetFeatureCheckDatumReferencesAsync(
        CollectionItemName featureCheck,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetFeatureCheckDatumReferencesRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
            });
        return _client.InvokeOperationAsync<FeatureCheckDatumReferencesResult>(
            "briosa.GdtOperations",
            "GetFeatureCheckDatumReferences",
            request,
            Transport.GetFeatureCheckDatumReferencesResult.Parser,
            cancellationToken);
    }

    public Task<GdtMeasurements> GetFeatureCheckMeasurementsAsync(
        CollectionItemName featureCheck,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetFeatureCheckMeasurementsRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
            });
        return _client.InvokeOperationAsync<GdtMeasurements>(
            "briosa.GdtOperations",
            "GetFeatureCheckMeasurements",
            request,
            Transport.GetFeatureCheckMeasurementsResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName> GetFeatureCheckReportingFrameAsync(
        CollectionItemName featureCheck,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetFeatureCheckReportingFrameRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
            });
        return _client.InvokeOperationAsync<CollectionObjectName>(
            "briosa.GdtOperations",
            "GetFeatureCheckReportingFrame",
            request,
            Transport.GetFeatureCheckReportingFrameResult.Parser,
            cancellationToken);
    }

    public Task<bool> GetGdtExtendedOptionsAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGdtExtendedOptionsRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync<bool>(
            "briosa.GdtOperations",
            "GetGdtExtendedOptions",
            request,
            Transport.GetGdtExtendedOptionsResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionItemName>> MakeAnnotationRefListFromCollectionAsync(
        CollectionName collection,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeAnnotationRefListFromCollectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionItemName>>(
            "briosa.GdtOperations",
            "MakeAnnotationRefListFromCollection",
            request,
            Transport.MakeAnnotationRefListFromCollectionResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionItemName>> MakeAnnotationRefListWildcardSelectionAsync(
        string collectionWildcardCriteria = "*",
        string annotationWildcardCriteria = "*",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeAnnotationRefListWildcardSelectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_wildcard_criteria"] = collectionWildcardCriteria,
                ["annotation_wildcard_criteria"] = annotationWildcardCriteria,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionItemName>>(
            "briosa.GdtOperations",
            "MakeAnnotationRefListWildcardSelection",
            request,
            Transport.MakeAnnotationRefListWildcardSelectionResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> MakeDatumRefListFromCollectionAsync(
        CollectionName collection,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeDatumRefListFromCollectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.GdtOperations",
            "MakeDatumRefListFromCollection",
            request,
            Transport.MakeDatumRefListFromCollectionResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionItemName>> MakeFeatureCheckRefListFromCollectionAsync(
        CollectionName collection,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeFeatureCheckRefListFromCollectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionItemName>>(
            "briosa.GdtOperations",
            "MakeFeatureCheckRefListFromCollection",
            request,
            Transport.MakeFeatureCheckRefListFromCollectionResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionItemName>> MakeFeatureCheckReferenceListWildcardSelectionAsync(
        string collectionWildcardCriteria = "*",
        string featureCheckWildcardCriteria = "*",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeFeatureCheckReferenceListWildcardSelectionRequest(),
            new Dictionary<string, object?>
            {
                ["collection_wildcard_criteria"] = collectionWildcardCriteria,
                ["feature_check_wildcard_criteria"] = featureCheckWildcardCriteria,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionItemName>>(
            "briosa.GdtOperations",
            "MakeFeatureCheckReferenceListWildcardSelection",
            request,
            Transport.MakeFeatureCheckReferenceListWildcardSelectionResult.Parser,
            cancellationToken);
    }

    public Task MakeFeatureChecksAsync(
        CollectionName collection,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeFeatureChecksRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "MakeFeatureChecks",
            request,
            Transport.MakeFeatureChecksResult.Parser,
            cancellationToken);
    }

    public Task MakeGdtDatumAnnotationAsync(
        MakeGdtDatumAnnotationOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeGdtDatumAnnotationRequest(),
            new Dictionary<string, object?>
            {
                ["datum_name"] = options.DatumName,
                ["objects"] = options.Objects,
                ["geometry_relationships"] = options.GeometryRelationships,
                ["surface_faces"] = options.SurfaceFaces,
                ["auxiliary_object"] = options.AuxiliaryObject,
                ["auxiliary_geometry_relationship"] = options.AuxiliaryGeometryRelationship,
                ["is_slot"] = options.IsSlot,
                ["force_surface_feature"] = options.ForceSurfaceFeature,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "MakeGdtDatumAnnotation",
            request,
            Transport.MakeGdtDatumAnnotationResult.Parser,
            cancellationToken);
    }

    public Task MakeGdtFeatureCheckAnnotationAsync(
        MakeGdtFeatureCheckAnnotationOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeGdtFeatureCheckAnnotationRequest(),
            new Dictionary<string, object?>
            {
                ["feature_annotation_name"] = options.FeatureAnnotationName,
                ["feature_type"] = options.FeatureType,
                ["objects"] = options.Objects,
                ["geometry_relationships"] = options.GeometryRelationships,
                ["surface_faces"] = options.SurfaceFaces,
                ["decompose_multiple_features"] = options.DecomposeMultipleFeatures,
                ["auto_create_diameter_checks"] = options.AutoCreateDiameterChecks,
                ["auto_create_slot_width_checks"] = options.AutoCreateSlotWidthChecks,
                ["auto_create_slot_length_checks"] = options.AutoCreateSlotLengthChecks,
                ["datum_references"] = options.DatumReferences,
                ["tolerance"] = options.Tolerance,
                ["is_slot"] = options.IsSlot,
                ["per_unit_length_or_area"] = options.PerUnitLengthOrArea,
                ["circular_area"] = options.CircularArea,
                ["per_unit_area_length_distance"] = options.PerUnitAreaLengthDistance,
                ["per_unit_area_length_step_over_percent"] = options.PerUnitAreaLengthStepOverPercent,
                ["per_unit_area_width_distance"] = options.PerUnitAreaWidthDistance,
                ["per_unit_area_width_step_over_percent"] = options.PerUnitAreaWidthStepOverPercent,
                ["per_unit_area_circle_diameter"] = options.PerUnitAreaCircleDiameter,
                ["per_unit_area_diameter_step_over"] = options.PerUnitAreaDiameterStepOver,
                ["auxiliary_object"] = options.AuxiliaryObject,
                ["auxiliary_geometry_relationship"] = options.AuxiliaryGeometryRelationship,
                ["use_nominal_for_dimension_tolerance"] = options.UseNominalForDimensionTolerance,
                ["use_reference_object_for_nominal"] = options.UseReferenceObjectForNominal,
                ["nominal_dimension_tolerance"] = options.NominalDimensionTolerance,
                ["low_dimension_tolerance"] = options.LowDimensionTolerance,
                ["high_dimension_tolerance"] = options.HighDimensionTolerance,
                ["tolerance_zone_type"] = options.ToleranceZoneType,
                ["use_projected_tolerance_zone"] = options.UseProjectedToleranceZone,
                ["projected_tolerance_zone"] = options.ProjectedToleranceZone,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "MakeGdtFeatureCheckAnnotation",
            request,
            Transport.MakeGdtFeatureCheckAnnotationResult.Parser,
            cancellationToken);
    }

    public Task<SurfaceFaceList> MakeSurfaceFaceListFromSurfaceAsync(
        CollectionObjectName surface,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeSurfaceFaceListFromSurfaceRequest(),
            new Dictionary<string, object?>
            {
                ["surface"] = surface,
            });
        return _client.InvokeOperationAsync<SurfaceFaceList>(
            "briosa.GdtOperations",
            "MakeSurfaceFaceListFromSurface",
            request,
            Transport.MakeSurfaceFaceListFromSurfaceResult.Parser,
            cancellationToken);
    }

    public Task<SurfaceFaceList> MakeSurfaceFaceListRuntimeSelectAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeSurfaceFaceListRuntimeSelectRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync<SurfaceFaceList>(
            "briosa.GdtOperations",
            "MakeSurfaceFaceListRuntimeSelect",
            request,
            Transport.MakeSurfaceFaceListRuntimeSelectResult.Parser,
            cancellationToken);
    }

    public Task RefreshDatumsFeatureChecksFromAnnotationsAsync(
        CollectionName collection,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RefreshDatumsFeatureChecksFromAnnotationsRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "RefreshDatumsFeatureChecksFromAnnotations",
            request,
            Transport.RefreshDatumsFeatureChecksFromAnnotationsResult.Parser,
            cancellationToken);
    }

    public Task SetDatumMeasurementsAsync(
        CollectionItemName datum,
        IEnumerable<PointName> pointNames,
        IEnumerable<CollectionObjectName> cloudNames,
        bool replaceExistingMeasurements = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetDatumMeasurementsRequest(),
            new Dictionary<string, object?>
            {
                ["datum"] = datum,
                ["point_names"] = pointNames,
                ["cloud_names"] = cloudNames,
                ["replace_existing_measurements"] = replaceExistingMeasurements,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "SetDatumMeasurements",
            request,
            Transport.SetDatumMeasurementsResult.Parser,
            cancellationToken);
    }

    public Task SetFeatureCheckCylinderEvalOptionsAsync(
        CollectionItemName featureCheck,
        bool enableActualDiameterOverride = false,
        double actualDiameterOverride = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetFeatureCheckCylinderEvalOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
                ["enable_actual_diameter_override"] = enableActualDiameterOverride,
                ["actual_diameter_override"] = actualDiameterOverride,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "SetFeatureCheckCylinderEvalOptions",
            request,
            Transport.SetFeatureCheckCylinderEvalOptionsResult.Parser,
            cancellationToken);
    }

    public Task SetFeatureCheckMeasurementsAsync(
        CollectionItemName featureCheck,
        IEnumerable<PointName> pointNames,
        IEnumerable<CollectionObjectName> cloudNames,
        bool replaceExistingMeasurements = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetFeatureCheckMeasurementsRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
                ["point_names"] = pointNames,
                ["cloud_names"] = cloudNames,
                ["replace_existing_measurements"] = replaceExistingMeasurements,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "SetFeatureCheckMeasurements",
            request,
            Transport.SetFeatureCheckMeasurementsResult.Parser,
            cancellationToken);
    }

    public Task SetFeatureCheckReportingFrameAsync(
        CollectionItemName featureCheck,
        CollectionObjectName reportingFrame,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetFeatureCheckReportingFrameRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
                ["reporting_frame"] = reportingFrame,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "SetFeatureCheckReportingFrame",
            request,
            Transport.SetFeatureCheckReportingFrameResult.Parser,
            cancellationToken);
    }

    public Task SetGdtExtendedOptionsAsync(
        bool useExtendedOptions = true,
        GdtExtendedEvaluationMethod circle = GdtExtendedEvaluationMethod.LeastSquares,
        GdtExtendedEvaluationMethod cone = GdtExtendedEvaluationMethod.LeastSquares,
        GdtExtendedEvaluationMethod cylinder = GdtExtendedEvaluationMethod.LeastSquares,
        GdtExtendedEvaluationMethod ellipse = GdtExtendedEvaluationMethod.LeastSquares,
        GdtExtendedEvaluationMethod line = GdtExtendedEvaluationMethod.LeastSquares,
        GdtExtendedEvaluationMethod openSlot = GdtExtendedEvaluationMethod.LeastSquares,
        GdtExtendedEvaluationMethod plane = GdtExtendedEvaluationMethod.LeastSquares,
        GdtExtendedEvaluationMethod slot = GdtExtendedEvaluationMethod.LeastSquares,
        GdtExtendedEvaluationMethod sphere = GdtExtendedEvaluationMethod.LeastSquares,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGdtExtendedOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["use_extended_options"] = useExtendedOptions,
                ["circle_extended_options"] = circle,
                ["cone_extended_options"] = cone,
                ["cylinder_extended_options"] = cylinder,
                ["ellipse_extended_options"] = ellipse,
                ["line_extended_options"] = line,
                ["open_slot_extended_options"] = openSlot,
                ["plane_extended_options"] = plane,
                ["slot_extended_options"] = slot,
                ["sphere_extended_options"] = sphere,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "SetGdtExtendedOptions",
            request,
            Transport.SetGdtExtendedOptionsResult.Parser,
            cancellationToken);
    }

    public Task SetGdtOptionsAsync(
        bool useHighPoints = false,
        bool extrapolateAxialExtent = true,
        bool excludeFromAutoEvaluation = true,
        GdtDistanceBetweenMode distanceBetweenMode = GdtDistanceBetweenMode.Centroid,
        GdtEvaluationMethod evaluationMethod = GdtEvaluationMethod.None,
        bool createActualFeatures = false,
        bool createSolvedPoints = false,
        double crossSectionCriteria = 0.039370,
        bool enableAutoFeatureDetection = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGdtOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["use_high_points"] = useHighPoints,
                ["extrapolate_axial_extent"] = extrapolateAxialExtent,
                ["exclude_from_auto_evaluation"] = excludeFromAutoEvaluation,
                ["distance_between_mode"] = distanceBetweenMode,
                ["evaluation_method"] = evaluationMethod,
                ["create_actual_features"] = createActualFeatures,
                ["create_solved_points"] = createSolvedPoints,
                ["cross_section_criteria"] = crossSectionCriteria,
                ["enable_auto_feature_detection"] = enableAutoFeatureDetection,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "SetGdtOptions",
            request,
            Transport.SetGdtOptionsResult.Parser,
            cancellationToken);
    }

    public Task SetGlobalForceSimultaneousEvaluationAsync(
        bool globalSimultaneousEvaluation = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetGlobalForceSimultaneousEvaluationRequest(),
            new Dictionary<string, object?>
            {
                ["global_simultaneous_evaluation"] = globalSimultaneousEvaluation,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "SetGlobalForceSimultaneousEvaluation",
            request,
            Transport.SetGlobalForceSimultaneousEvaluationResult.Parser,
            cancellationToken);
    }

    public Task StartStopFeatureCheckTrappingAsync(
        CollectionItemName featureCheck,
        CollectionInstrumentId instrumentId,
        bool startTrapping = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StartStopFeatureCheckTrappingRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
                ["instrument_id"] = instrumentId,
                ["start_trapping"] = startTrapping,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "StartStopFeatureCheckTrapping",
            request,
            Transport.StartStopFeatureCheckTrappingResult.Parser,
            cancellationToken);
    }

    public Task<FeatureCheckReportingOptions> GetFeatureCheckReportingOptionsAsync(
        CollectionItemName featureCheck,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetFeatureCheckReportingOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
            });
        return _client.InvokeOperationAsync<FeatureCheckReportingOptions>(
            "briosa.GdtOperations",
            "GetFeatureCheckReportingOptions",
            request,
            Transport.GetFeatureCheckReportingOptionsResult.Parser,
            cancellationToken);
    }

    public Task<GdtOptions> GetGdtOptionsAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetGdtOptionsRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync<GdtOptions>(
            "briosa.GdtOperations",
            "GetGdtOptions",
            request,
            Transport.GetGdtOptionsResult.Parser,
            cancellationToken);
    }

    public Task SetFeatureCheckReportingOptionsAsync(
        CollectionItemName featureCheck,
        bool showFeatureControlFrameSummary = true,
        bool includeTitle = false,
        bool showDatumAndToleranceSummary = false,
        bool showFeatureSummary = false,
        bool onlyCreateFailedVectors = false,
        bool showPointDetailsSummary = false,
        bool showLowerTierTables = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetFeatureCheckReportingOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["feature_check"] = featureCheck,
                ["show_feature_control_frame_summary"] = showFeatureControlFrameSummary,
                ["include_title"] = includeTitle,
                ["show_datum_and_tolerance_summary"] = showDatumAndToleranceSummary,
                ["show_feature_summary"] = showFeatureSummary,
                ["only_create_failed_vectors"] = onlyCreateFailedVectors,
                ["show_point_details_summary"] = showPointDetailsSummary,
                ["show_lower_tier_tables"] = showLowerTierTables,
            });
        return _client.InvokeOperationAsync(
            "briosa.GdtOperations",
            "SetFeatureCheckReportingOptions",
            request,
            Transport.SetFeatureCheckReportingOptionsResult.Parser,
            cancellationToken);
    }
}

public sealed class BriosaInstrumentOperations
{
    private readonly BriosaClient _client;
    internal BriosaInstrumentOperations(BriosaClient client) => _client = client;

    public Task ActivateDeactivateInstrumentToolbarAsync(
        CollectionInstrumentId instrument,
        bool deactivateToolbar = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ActivateDeactivateInstrumentToolbarRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["deactivate_toolbar"] = deactivateToolbar,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "ActivateDeactivateInstrumentToolbar",
            request,
            Transport.ActivateDeactivateInstrumentToolbarResult.Parser,
            cancellationToken);
    }

    public Task<CollectionInstrumentId> AddNewInstrumentAsync(
        InstrumentTypeName instrumentType,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddNewInstrumentRequest(),
            new Dictionary<string, object?>
            {
                ["instrument_type"] = instrumentType,
            });
        return _client.InvokeOperationAsync<CollectionInstrumentId>(
            "briosa.InstrumentOperations",
            "AddNewInstrument",
            request,
            Transport.AddNewInstrumentResult.Parser,
            cancellationToken);
    }

    public Task AddNominalPointToTcpFixtureAsync(
        CollectionObjectName tcpFixture,
        string nominalPointName,
        Vector nominalPointLocation,
        double varXx = 0.0,
        double varYy = 0.0,
        double varZz = 0.0,
        double covarXy = 0.0,
        double covarXz = 0.0,
        double covarYz = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddNominalPointToTcpFixtureRequest(),
            new Dictionary<string, object?>
            {
                ["tcp_fixture"] = tcpFixture,
                ["nominal_point_name"] = nominalPointName,
                ["nominal_point_location"] = nominalPointLocation,
                ["var_xx"] = varXx,
                ["var_yy"] = varYy,
                ["var_zz"] = varZz,
                ["covar_xy"] = covarXy,
                ["covar_xz"] = covarXz,
                ["covar_yz"] = covarYz,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "AddNominalPointToTcpFixture",
            request,
            Transport.AddNominalPointToTcpFixtureResult.Parser,
            cancellationToken);
    }

    public Task<CloudToCadAlignmentResult> AlignCloudToCadAsync(
        CollectionObjectName cloud,
        IEnumerable<CollectionObjectName> surfaces,
        double maximumCoarseCadMeshEdgeLength = 0.0,
        bool useFineCadMesh = false,
        bool executeAlignment = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AlignCloudToCadRequest(),
            new Dictionary<string, object?>
            {
                ["cloud"] = cloud,
                ["surfaces"] = surfaces,
                ["maximum_coarse_cad_mesh_edge_length"] = maximumCoarseCadMeshEdgeLength,
                ["use_fine_cad_mesh"] = useFineCadMesh,
                ["execute_alignment"] = executeAlignment,
            });
        return _client.InvokeOperationAsync<CloudToCadAlignmentResult>(
            "briosa.InstrumentOperations",
            "AlignCloudToCad",
            request,
            Transport.AlignCloudToCadResult.Parser,
            cancellationToken);
    }

    public Task AlignLaserProjectorAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName group,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AlignLaserProjectorRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["group"] = group,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "AlignLaserProjector",
            request,
            Transport.AlignLaserProjectorResult.Parser,
            cancellationToken);
    }

    public Task AlignTwoTargetsWithAxisWcfXAsync(
        CollectionInstrumentId instrument,
        PointName firstPointOnAxis,
        PointName secondPointOnAxis,
        CollectionObjectName initialMeasuredGroup,
        ToleranceVectorOptions? rotationalTolerance = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AlignTwoTargetsWithAxisWcfXRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["first_point_on_axis"] = firstPointOnAxis,
                ["second_point_on_axis"] = secondPointOnAxis,
                ["initial_measured_group"] = initialMeasuredGroup,
                ["rotational_tolerance"] = rotationalTolerance,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "AlignTwoTargetsWithAxisWcfX",
            request,
            Transport.AlignTwoTargetsWithAxisWcfXResult.Parser,
            cancellationToken);
    }

    public Task AssociateObjectsWithInstrumentAsync(
        CollectionInstrumentId instrument,
        IEnumerable<CollectionObjectName> objects,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AssociateObjectsWithInstrumentRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["objects"] = objects,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "AssociateObjectsWithInstrument",
            request,
            Transport.AssociateObjectsWithInstrumentResult.Parser,
            cancellationToken);
    }

    public Task AutoCorrespondClosestPointAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName referenceGroup,
        CollectionObjectName actualsGroup,
        bool waitForCompletion = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoCorrespondClosestPointRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["reference_group"] = referenceGroup,
                ["actuals_group"] = actualsGroup,
                ["wait_for_completion"] = waitForCompletion,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "AutoCorrespondClosestPoint",
            request,
            Transport.AutoCorrespondClosestPointResult.Parser,
            cancellationToken);
    }

    public Task AutoCorrespondWithProximityTriggerAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName nominalGroup,
        CollectionObjectName resultsGroup,
        double pointDistanceThreshold = 0.5,
        double vectorAxisThreshold = 0.25,
        bool projectResultsToNominalVector = false,
        double warblerRampStartDistance = 12.0,
        bool showWatchWindow = false,
        string? deviationVectorGroupName = null,
        bool makeUnmeasuredGroup = false,
        bool measureEachPointOnlyOnce = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoCorrespondWithProximityTriggerRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["nominal_group"] = nominalGroup,
                ["results_group"] = resultsGroup,
                ["point_distance_threshold"] = pointDistanceThreshold,
                ["vector_axis_threshold"] = vectorAxisThreshold,
                ["project_results_to_nominal_vector"] = projectResultsToNominalVector,
                ["warbler_ramp_start_distance"] = warblerRampStartDistance,
                ["show_watch_window"] = showWatchWindow,
                ["deviation_vector_group_name"] = deviationVectorGroupName,
                ["make_unmeasured_group"] = makeUnmeasuredGroup,
                ["measure_each_point_only_once"] = measureEachPointOnlyOnce,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "AutoCorrespondWithProximityTrigger",
            request,
            Transport.AutoCorrespondWithProximityTriggerResult.Parser,
            cancellationToken);
    }

    public Task AutoMeasureBatchOfFeaturesAsync(
        CollectionInstrumentId instrument,
        IEnumerable<CollectionItemName> features,
        bool waitForComplete = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoMeasureBatchOfFeaturesRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["features"] = features,
                ["wait_for_complete"] = waitForComplete,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "AutoMeasureBatchOfFeatures",
            request,
            Transport.AutoMeasureBatchOfFeaturesResult.Parser,
            cancellationToken);
    }

    public Task AutoMeasurePointsAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName referenceGroup,
        CollectionObjectName actualsGroup,
        bool forceExistingGroup = false,
        bool showCompleteDialog = false,
        bool waitForCompletion = true,
        bool autoStart = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoMeasurePointsRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["reference_group"] = referenceGroup,
                ["actuals_group"] = actualsGroup,
                ["force_existing_group"] = forceExistingGroup,
                ["show_complete_dialog"] = showCompleteDialog,
                ["wait_for_completion"] = waitForCompletion,
                ["auto_start"] = autoStart,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "AutoMeasurePoints",
            request,
            Transport.AutoMeasurePointsResult.Parser,
            cancellationToken);
    }

    public Task AutoMeasureSpecifiedGeometryAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName geometry,
        string modeProfile,
        bool waitForComplete = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoMeasureSpecifiedGeometryRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["geometry"] = geometry,
                ["mode_profile"] = modeProfile,
                ["wait_for_complete"] = waitForComplete,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "AutoMeasureSpecifiedGeometry",
            request,
            Transport.AutoMeasureSpecifiedGeometryResult.Parser,
            cancellationToken);
    }

    public Task AutoMeasureSurfaceVectorIntersectionsAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName vectorGroup,
        CollectionObjectName resultantGroup,
        bool waitForComplete = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoMeasureSurfaceVectorIntersectionsRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["vector_group"] = vectorGroup,
                ["resultant_group"] = resultantGroup,
                ["wait_for_complete"] = waitForComplete,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "AutoMeasureSurfaceVectorIntersections",
            request,
            Transport.AutoMeasureSurfaceVectorIntersectionsResult.Parser,
            cancellationToken);
    }

    public Task AutoMeasureVectorsAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName vectorGroup,
        CollectionObjectName actualsGroup,
        bool projectPointToVector = false,
        double angleTolerance = 0.0,
        double highTolerance = 0.0,
        double lowTolerance = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AutoMeasureVectorsRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["vector_group"] = vectorGroup,
                ["actuals_group"] = actualsGroup,
                ["project_point_to_vector"] = projectPointToVector,
                ["angle_tolerance"] = angleTolerance,
                ["high_tolerance"] = highTolerance,
                ["low_tolerance"] = lowTolerance,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "AutoMeasureVectors",
            request,
            Transport.AutoMeasureVectorsResult.Parser,
            cancellationToken);
    }

    public Task BuildTargetAsync(
        CollectionInstrumentId instrument,
        PointName outputTargetName,
        PointName nominalPoint,
        ToleranceVectorOptions? tolerance = null,
        FileReference? htmlPromptFile = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.BuildTargetRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["output_target_name"] = outputTargetName,
                ["nominal_point"] = nominalPoint,
                ["tolerance"] = tolerance,
                ["html_prompt_file"] = htmlPromptFile,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "BuildTarget",
            request,
            Transport.BuildTargetResult.Parser,
            cancellationToken);
    }

    public Task<TcpFixtureUncertainties> CalculateTcpFixtureUncertaintiesAsync(
        CollectionObjectName tcpFixture,
        IEnumerable<PointName> tcpMeasurements,
        Transform? tcpInWorking = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CalculateTcpFixtureUncertaintiesRequest(),
            new Dictionary<string, object?>
            {
                ["tcp_fixture"] = tcpFixture,
                ["tcp_in_working"] = tcpInWorking,
                ["tcp_measurements"] = tcpMeasurements,
            });
        return _client.InvokeOperationAsync<TcpFixtureUncertainties>(
            "briosa.InstrumentOperations",
            "CalculateTcpFixtureUncertainties",
            request,
            Transport.CalculateTcpFixtureUncertaintiesResult.Parser,
            cancellationToken);
    }

    public Task ClearCloudViewerAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ClearCloudViewerRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "ClearCloudViewer",
            request,
            Transport.ClearCloudViewerResult.Parser,
            cancellationToken);
    }

    public Task CloseAutoCorrespondClosestPointDialogAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CloseAutoCorrespondClosestPointDialogRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "CloseAutoCorrespondClosestPointDialog",
            request,
            Transport.CloseAutoCorrespondClosestPointDialogResult.Parser,
            cancellationToken);
    }

    public Task CollimationAsync(
        CollectionInstrumentId stationaryInstrument,
        CollectionInstrumentId movingInstrument,
        PointName collimationPoint,
        bool zeroMovingInstrument = false,
        CollimationTiltMode tiltMode = CollimationTiltMode.FullCollimation,
        CollimationBaselineMethod baselineMethod = CollimationBaselineMethod.DeterminedByValue,
        double baselineDistance = 0.0,
        PointName? scalePoint1 = null,
        PointName? scalePoint2 = null,
        PointName? notMeasuredByMovingInstrument = null,
        PointName? asMeasuredByMovingInstrument = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CollimationRequest(),
            new Dictionary<string, object?>
            {
                ["stationary_instrument"] = stationaryInstrument,
                ["moving_instrument"] = movingInstrument,
                ["collimation_point"] = collimationPoint,
                ["zero_moving_instrument"] = zeroMovingInstrument,
                ["tilt_mode"] = tiltMode,
                ["baseline_method"] = baselineMethod,
                ["baseline_distance"] = baselineDistance,
                ["scale_point_1"] = scalePoint1,
                ["scale_point_2"] = scalePoint2,
                ["not_measured_by_moving_instrument"] = notMeasuredByMovingInstrument,
                ["as_measured_by_moving_instrument"] = asMeasuredByMovingInstrument,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "Collimation",
            request,
            Transport.CollimationResult.Parser,
            cancellationToken);
    }

    public Task CombinePointGroupsAsync(
        IEnumerable<CollectionObjectName> groupsToCombine,
        CollectionObjectName combinedPointGroup,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CombinePointGroupsRequest(),
            new Dictionary<string, object?>
            {
                ["groups_to_combine"] = groupsToCombine,
                ["combined_point_group"] = combinedPointGroup,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "CombinePointGroups",
            request,
            Transport.CombinePointGroupsResult.Parser,
            cancellationToken);
    }

    public Task<double> ComputeCteScaleFactorAsync(
        double materialCtePerDegreeFahrenheit = 0.0,
        double initialTemperatureFahrenheit = 0.0,
        double finalTemperatureFahrenheit = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ComputeCteScaleFactorRequest(),
            new Dictionary<string, object?>
            {
                ["material_cte_per_degree_fahrenheit"] = materialCtePerDegreeFahrenheit,
                ["initial_temperature_fahrenheit"] = initialTemperatureFahrenheit,
                ["final_temperature_fahrenheit"] = finalTemperatureFahrenheit,
            });
        return _client.InvokeOperationAsync<double>(
            "briosa.InstrumentOperations",
            "ComputeCteScaleFactor",
            request,
            Transport.ComputeCteScaleFactorResult.Parser,
            cancellationToken);
    }

    public Task ConfigureAndMeasureAsync(
        CollectionInstrumentId instrument,
        PointName target,
        string measurementMode,
        bool measureImmediately = false,
        bool waitForCompletion = true,
        double timeoutSeconds = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConfigureAndMeasureRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["target"] = target,
                ["measurement_mode"] = measurementMode,
                ["measure_immediately"] = measureImmediately,
                ["wait_for_completion"] = waitForCompletion,
                ["timeout_seconds"] = timeoutSeconds,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "ConfigureAndMeasure",
            request,
            Transport.ConfigureAndMeasureResult.Parser,
            cancellationToken);
    }

    public Task ConstructMeasuredPointUncertaintyEllipsoidsAsync(
        IEnumerable<PointName> measurements,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructMeasuredPointUncertaintyEllipsoidsRequest(),
            new Dictionary<string, object?>
            {
                ["measurements"] = measurements,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "ConstructMeasuredPointUncertaintyEllipsoids",
            request,
            Transport.ConstructMeasuredPointUncertaintyEllipsoidsResult.Parser,
            cancellationToken);
    }

    public Task ConstructMirrorFromPlaneAsync(
        CollectionInstrumentId instrument,
        string mirrorName,
        CollectionObjectName plane,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructMirrorFromPlaneRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["mirror_name"] = mirrorName,
                ["plane"] = plane,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "ConstructMirrorFromPlane",
            request,
            Transport.ConstructMirrorFromPlaneResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName> ConstructMirrorFromTwoPointsAsync(
        CollectionInstrumentId instrument,
        string mirrorName,
        PointName pointMeasuredDirectly,
        PointName pointMeasuredThroughMirror,
        bool sendMirrorToInstrument = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructMirrorFromTwoPointsRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["mirror_name"] = mirrorName,
                ["point_measured_directly"] = pointMeasuredDirectly,
                ["point_measured_through_mirror"] = pointMeasuredThroughMirror,
                ["send_mirror_to_instrument"] = sendMirrorToInstrument,
            });
        return _client.InvokeOperationAsync<CollectionObjectName>(
            "briosa.InstrumentOperations",
            "ConstructMirrorFromTwoPoints",
            request,
            Transport.ConstructMirrorFromTwoPointsResult.Parser,
            cancellationToken);
    }

    public Task<PerimeterLists> ConstructPerimetersFromSurfaceFaceListAsync(
        SurfaceFaceList surfaceFaces,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructPerimetersFromSurfaceFaceListRequest(),
            new Dictionary<string, object?>
            {
                ["surface_faces"] = surfaceFaces,
            });
        return _client.InvokeOperationAsync<PerimeterLists>(
            "briosa.InstrumentOperations",
            "ConstructPerimetersFromSurfaceFaceList",
            request,
            Transport.ConstructPerimetersFromSurfaceFaceListResult.Parser,
            cancellationToken);
    }

    public Task<CollectionObjectName> ConstructTcpFixtureAsync(
        CollectionObjectName requestedTcpFixture,
        double pointMatchThreshold = 0.0,
        bool replaceExistingTcpFixture = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConstructTcpFixtureRequest(),
            new Dictionary<string, object?>
            {
                ["requested_tcp_fixture"] = requestedTcpFixture,
                ["point_match_threshold"] = pointMatchThreshold,
                ["replace_existing_tcp_fixture"] = replaceExistingTcpFixture,
            });
        return _client.InvokeOperationAsync<CollectionObjectName>(
            "briosa.InstrumentOperations",
            "ConstructTcpFixture",
            request,
            Transport.ConstructTcpFixtureResult.Parser,
            cancellationToken);
    }

    public Task CreateNewDynamicReferenceAsync(
        CollectionInstrumentId instrument,
        IEnumerable<PointName> pointsDefiningDynamicReference,
        string dynamicReferenceName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreateNewDynamicReferenceRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["points_defining_dynamic_reference"] = pointsDefiningDynamicReference,
                ["dynamic_reference_name"] = dynamicReferenceName,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "CreateNewDynamicReference",
            request,
            Transport.CreateNewDynamicReferenceResult.Parser,
            cancellationToken);
    }

    public Task CreateTemplatedInstrumentUsmnAsync(
        CollectionObjectName instrumentTemplateName,
        CollectionInstrumentId instrument,
        double overallInstrumentWeight = 1.0,
        bool moving = true,
        bool enableX = true,
        bool enableY = true,
        bool enableZ = true,
        bool enableRx = true,
        bool enableRy = true,
        bool enableRz = true,
        bool enableScale = false,
        bool enableComponentWeights = true,
        double azimuthWeight = 1.0,
        double elevationWeight = 1.0,
        double distanceWeight = 1.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreateTemplatedInstrumentUsmnRequest(),
            new Dictionary<string, object?>
            {
                ["instrument_template_name"] = instrumentTemplateName,
                ["instrument"] = instrument,
                ["overall_instrument_weight"] = overallInstrumentWeight,
                ["moving"] = moving,
                ["enable_x"] = enableX,
                ["enable_y"] = enableY,
                ["enable_z"] = enableZ,
                ["enable_rx"] = enableRx,
                ["enable_ry"] = enableRy,
                ["enable_rz"] = enableRz,
                ["enable_scale"] = enableScale,
                ["enable_component_weights"] = enableComponentWeights,
                ["azimuth_weight"] = azimuthWeight,
                ["elevation_weight"] = elevationWeight,
                ["distance_weight"] = distanceWeight,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "CreateTemplatedInstrumentUsmn",
            request,
            Transport.CreateTemplatedInstrumentUsmnResult.Parser,
            cancellationToken);
    }

    public Task DeleteInstrumentAsync(
        CollectionInstrumentId instrument,
        bool promptUserToConfirm = false,
        bool keepResultingPoints = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteInstrumentRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["prompt_user_to_confirm"] = promptUserToConfirm,
                ["keep_resulting_points"] = keepResultingPoints,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "DeleteInstrument",
            request,
            Transport.DeleteInstrumentResult.Parser,
            cancellationToken);
    }

    public Task DeleteMeasurementObservationAsync(
        PointName pointName,
        int observationIndex = 0,
        bool deletePointIfNoMeasurementsRemain = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteMeasurementObservationRequest(),
            new Dictionary<string, object?>
            {
                ["point_name"] = pointName,
                ["observation_index"] = observationIndex,
                ["delete_point_if_no_measurements_remain"] = deletePointIfNoMeasurementsRemain,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "DeleteMeasurementObservation",
            request,
            Transport.DeleteMeasurementObservationResult.Parser,
            cancellationToken);
    }

    public Task DeleteMeasurementsAsync(
        CollectionInstrumentId instrument,
        PointName pointName,
        bool deletePointIfNoMeasurementsRemain = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteMeasurementsRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["point_name"] = pointName,
                ["delete_point_if_no_measurements_remain"] = deletePointIfNoMeasurementsRemain,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "DeleteMeasurements",
            request,
            Transport.DeleteMeasurementsResult.Parser,
            cancellationToken);
    }

    public Task DisassociateObjectsFromInstrumentAsync(
        IEnumerable<CollectionObjectName> objects,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DisassociateObjectsFromInstrumentRequest(),
            new Dictionary<string, object?>
            {
                ["objects"] = objects,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "DisassociateObjectsFromInstrument",
            request,
            Transport.DisassociateObjectsFromInstrumentResult.Parser,
            cancellationToken);
    }

    public Task DissectPointGroupAsync(
        CollectionObjectName groupToDissect,
        string baseNameForDissectedGroups,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DissectPointGroupRequest(),
            new Dictionary<string, object?>
            {
                ["group_to_dissect"] = groupToDissect,
                ["base_name_for_dissected_groups"] = baseNameForDissectedGroups,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "DissectPointGroup",
            request,
            Transport.DissectPointGroupResult.Parser,
            cancellationToken);
    }

    public Task DockInstrumentInterfaceAsync(
        CollectionInstrumentId instrument,
        bool dockInterface = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DockInstrumentInterfaceRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["dock_interface"] = dockInterface,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "DockInstrumentInterface",
            request,
            Transport.DockInstrumentInterfaceResult.Parser,
            cancellationToken);
    }

    public Task<DriftCheckResult> DriftCheckAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName referenceGroup,
        CollectionObjectName actualsGroup,
        double tolerance = 0.0,
        int minimumPointCount = 0,
        bool useClosestReferencePoint = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DriftCheckRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["reference_group"] = referenceGroup,
                ["actuals_group"] = actualsGroup,
                ["tolerance"] = tolerance,
                ["minimum_point_count"] = minimumPointCount,
                ["use_closest_reference_point"] = useClosestReferencePoint,
            });
        return _client.InvokeOperationAsync<DriftCheckResult>(
            "briosa.InstrumentOperations",
            "DriftCheck",
            request,
            Transport.DriftCheckResult.Parser,
            cancellationToken);
    }

    public Task EdgeScanMeasurementAsync(
        CollectionInstrumentId instrument,
        PointName pointNearEdge,
        PointName edgeSearchDirectionPoint,
        string parameterSetName,
        CollectionObjectName pointGroup,
        string targetName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EdgeScanMeasurementRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["point_near_edge"] = pointNearEdge,
                ["edge_search_direction_point"] = edgeSearchDirectionPoint,
                ["parameter_set_name"] = parameterSetName,
                ["point_group"] = pointGroup,
                ["target_name"] = targetName,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "EdgeScanMeasurement",
            request,
            Transport.EdgeScanMeasurementResult.Parser,
            cancellationToken);
    }

    public Task EditScanPerimeterProfileAsync(
        CollectionInstrumentId instrument,
        IReadOnlyList<CollectionObjectName> scanPerimeters,
        IReadOnlyList<CollectionObjectName> exclusionPerimeters,
        string parameterSetName,
        string profileName,
        bool clearProfile = true,
        bool createNewProfile = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EditScanPerimeterProfileRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["scan_perimeters"] = scanPerimeters,
                ["exclusion_perimeters"] = exclusionPerimeters,
                ["parameter_set_name"] = parameterSetName,
                ["profile_name"] = profileName,
                ["clear_profile"] = clearProfile,
                ["create_new_profile"] = createNewProfile,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "EditScanPerimeterProfile",
            request,
            Transport.EditScanPerimeterProfileResult.Parser,
            cancellationToken);
    }

    public Task EnableDisableFrameSetScanModeAllInstrumentsAsync(
        bool enableFrameSetScanMode = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EnableDisableFrameSetScanModeAllInstrumentsRequest(),
            new Dictionary<string, object?>
            {
                ["enable_frame_set_scan_mode"] = enableFrameSetScanMode,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "EnableDisableFrameSetScanModeAllInstruments",
            request,
            Transport.EnableDisableFrameSetScanModeAllInstrumentsResult.Parser,
            cancellationToken);
    }

    public Task EnableDisableFrameSetScanModeByInstrumentAsync(
        CollectionInstrumentId instrument,
        bool enableFrameSetScanMode = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EnableDisableFrameSetScanModeByInstrumentRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["enable_frame_set_scan_mode"] = enableFrameSetScanMode,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "EnableDisableFrameSetScanModeByInstrument",
            request,
            Transport.EnableDisableFrameSetScanModeByInstrumentResult.Parser,
            cancellationToken);
    }

    public Task EnableDisablePointSetScanModeAsync(
        CollectionInstrumentId instrument,
        bool enablePointSetScanMode = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EnableDisablePointSetScanModeRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["enable_point_set_scan_mode"] = enablePointSetScanMode,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "EnableDisablePointSetScanMode",
            request,
            Transport.EnableDisablePointSetScanModeResult.Parser,
            cancellationToken);
    }

    public Task ExportInstrumentHistoryToXmlFileAsync(
        CollectionInstrumentId instrument,
        FileReference filePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ExportInstrumentHistoryToXmlFileRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["file_path"] = filePath,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "ExportInstrumentHistoryToXmlFile",
            request,
            Transport.ExportInstrumentHistoryToXmlFileResult.Parser,
            cancellationToken);
    }

    public Task FabricateObservationsAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName pointGroup,
        bool introduceInstrumentError = false,
        bool limitDistance = false,
        double minimumDistance = 0.0,
        double maximumDistance = 1000000.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.FabricateObservationsRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["point_group"] = pointGroup,
                ["introduce_instrument_error"] = introduceInstrumentError,
                ["limit_distance"] = limitDistance,
                ["minimum_distance"] = minimumDistance,
                ["maximum_distance"] = maximumDistance,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "FabricateObservations",
            request,
            Transport.FabricateObservationsResult.Parser,
            cancellationToken);
    }

    public Task<InstrumentPositionUpdate> GetCurrentInstrumentPositionUpdateAsync(
        CollectionInstrumentId instrument,
        InstrumentPositionReportingFrame reportingFrame = InstrumentPositionReportingFrame.InstrumentBase,
        bool polarCoordinates = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCurrentInstrumentPositionUpdateRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["reporting_frame"] = reportingFrame,
                ["polar_coordinates"] = polarCoordinates,
            });
        return _client.InvokeOperationAsync<InstrumentPositionUpdate>(
            "briosa.InstrumentOperations",
            "GetCurrentInstrumentPositionUpdate",
            request,
            Transport.GetCurrentInstrumentPositionUpdateResult.Parser,
            cancellationToken);
    }

    public Task<CurrentTrappingStatus> GetCurrentTrappingStatusAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCurrentTrappingStatusRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync<CurrentTrappingStatus>(
            "briosa.InstrumentOperations",
            "GetCurrentTrappingStatus",
            request,
            Transport.GetCurrentTrappingStatusResult.Parser,
            cancellationToken);
    }

    public Task<double> GetEstimatedScanTimeAsync(
        CollectionInstrumentId instrument,
        string profileName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetEstimatedScanTimeRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["profile_name"] = profileName,
            });
        return _client.InvokeOperationAsync<double>(
            "briosa.InstrumentOperations",
            "GetEstimatedScanTime",
            request,
            Transport.GetEstimatedScanTimeResult.Parser,
            cancellationToken);
    }

    public Task<bool> GetInspectionVerificationModeAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInspectionVerificationModeRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync<bool>(
            "briosa.InstrumentOperations",
            "GetInspectionVerificationMode",
            request,
            Transport.GetInspectionVerificationModeResult.Parser,
            cancellationToken);
    }

    public Task<UncertaintyCovarianceMatrix> GetInstrumentBaseUncertaintyCovarianceMatrixWrtWorldAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentBaseUncertaintyCovarianceMatrixWrtWorldRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<UncertaintyCovarianceMatrix>(
            "briosa.InstrumentOperations",
            "GetInstrumentBaseUncertaintyCovarianceMatrixWrtWorld",
            request,
            Transport.GetInstrumentBaseUncertaintyCovarianceMatrixWrtWorldResult.Parser,
            cancellationToken);
    }

    public Task<PointName> GetInstrumentGroupAndTargetAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentGroupAndTargetRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<PointName>(
            "briosa.InstrumentOperations",
            "GetInstrumentGroupAndTarget",
            request,
            Transport.GetInstrumentGroupAndTargetResult.Parser,
            cancellationToken);
    }

    public Task<CollectionInstrumentId> GetInstrumentIdFromNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentIdFromNameRequest(),
            new Dictionary<string, object?>
            {
                ["name"] = name,
            });
        return _client.InvokeOperationAsync<CollectionInstrumentId>(
            "briosa.InstrumentOperations",
            "GetInstrumentIdFromName",
            request,
            Transport.GetInstrumentIdFromNameResult.Parser,
            cancellationToken);
    }

    public Task<double> GetInstrumentInterfaceResponseTimeoutAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentInterfaceResponseTimeoutRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<double>(
            "briosa.InstrumentOperations",
            "GetInstrumentInterfaceResponseTimeout",
            request,
            Transport.GetInstrumentInterfaceResponseTimeoutResult.Parser,
            cancellationToken);
    }

    public Task<string> GetInstrumentMeasurementModeProfileAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentMeasurementModeProfileRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<string>(
            "briosa.InstrumentOperations",
            "GetInstrumentMeasurementModeProfile",
            request,
            Transport.GetInstrumentMeasurementModeProfileResult.Parser,
            cancellationToken);
    }

    public Task<InstrumentModelResult> GetInstrumentModelAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentModelRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<InstrumentModelResult>(
            "briosa.InstrumentOperations",
            "GetInstrumentModel",
            request,
            Transport.GetInstrumentModelResult.Parser,
            cancellationToken);
    }

    public Task<double> GetInstrumentPartTemperatureAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentPartTemperatureRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<double>(
            "briosa.InstrumentOperations",
            "GetInstrumentPartTemperature",
            request,
            Transport.GetInstrumentPartTemperatureResult.Parser,
            cancellationToken);
    }

    public Task<double> GetInstrumentScaleFactorAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentScaleFactorRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<double>(
            "briosa.InstrumentOperations",
            "GetInstrumentScaleFactor",
            request,
            Transport.GetInstrumentScaleFactorResult.Parser,
            cancellationToken);
    }

    public Task<InstrumentTargetStatus> GetInstrumentTargetStatusAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentTargetStatusRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<InstrumentTargetStatus>(
            "briosa.InstrumentOperations",
            "GetInstrumentTargetStatus",
            request,
            Transport.GetInstrumentTargetStatusResult.Parser,
            cancellationToken);
    }

    public Task<string> GetInstrumentTargetingAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentTargetingRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<string>(
            "briosa.InstrumentOperations",
            "GetInstrumentTargeting",
            request,
            Transport.GetInstrumentTargetingResult.Parser,
            cancellationToken);
    }

    public Task<InstrumentTargetsAndModeProfiles> GetInstrumentTargetsAndModeProfilesAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentTargetsAndModeProfilesRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<InstrumentTargetsAndModeProfiles>(
            "briosa.InstrumentOperations",
            "GetInstrumentTargetsAndModeProfiles",
            request,
            Transport.GetInstrumentTargetsAndModeProfilesResult.Parser,
            cancellationToken);
    }

    public Task<Transform> GetInstrumentTransformAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName referenceFrame,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentTransformRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["reference_frame"] = referenceFrame,
            });
        return _client.InvokeOperationAsync<Transform>(
            "briosa.InstrumentOperations",
            "GetInstrumentTransform",
            request,
            Transport.GetInstrumentTransformResult.Parser,
            cancellationToken);
    }

    public Task<InstrumentWeatherSetting> GetInstrumentWeatherSettingAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentWeatherSettingRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<InstrumentWeatherSetting>(
            "briosa.InstrumentOperations",
            "GetInstrumentWeatherSetting",
            request,
            Transport.GetInstrumentWeatherSettingResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionInstrumentId>> GetInstrumentsWithObservationsOnTargetAsync(
        PointName point,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetInstrumentsWithObservationsOnTargetRequest(),
            new Dictionary<string, object?>
            {
                ["point"] = point,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionInstrumentId>>(
            "briosa.InstrumentOperations",
            "GetInstrumentsWithObservationsOnTarget",
            request,
            Transport.GetInstrumentsWithObservationsOnTargetResult.Parser,
            cancellationToken);
    }

    public Task<LastInstrumentIndexResult> GetLastInstrumentIndexAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetLastInstrumentIndexRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync<LastInstrumentIndexResult>(
            "briosa.InstrumentOperations",
            "GetLastInstrumentIndex",
            request,
            Transport.GetLastInstrumentIndexResult.Parser,
            cancellationToken);
    }

    public Task<UncertaintyCovarianceMatrix> GetLastSolvedTcpFixtureUncertaintyCovarianceMatrixAsync(
        CollectionObjectName tcpFixture,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetLastSolvedTcpFixtureUncertaintyCovarianceMatrixRequest(),
            new Dictionary<string, object?>
            {
                ["tcp_fixture"] = tcpFixture,
            });
        return _client.InvokeOperationAsync<UncertaintyCovarianceMatrix>(
            "briosa.InstrumentOperations",
            "GetLastSolvedTcpFixtureUncertaintyCovarianceMatrix",
            request,
            Transport.GetLastSolvedTcpFixtureUncertaintyCovarianceMatrixResult.Parser,
            cancellationToken);
    }

    public Task<int> GetNumberOfObservationsOnTargetAsync(
        PointName point,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetNumberOfObservationsOnTargetRequest(),
            new Dictionary<string, object?>
            {
                ["point"] = point,
            });
        return _client.InvokeOperationAsync<int>(
            "briosa.InstrumentOperations",
            "GetNumberOfObservationsOnTarget",
            request,
            Transport.GetNumberOfObservationsOnTargetResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<PointName>> GetObscuredPointsFromInstrumentAsync(
        CollectionInstrumentId instrument,
        IEnumerable<PointName> candidatePoints,
        bool showObscuredShots = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetObscuredPointsFromInstrumentRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["candidate_points"] = candidatePoints,
                ["show_obscured_shots"] = showObscuredShots,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<PointName>>(
            "briosa.InstrumentOperations",
            "GetObscuredPointsFromInstrument",
            request,
            Transport.GetObscuredPointsFromInstrumentResult.Parser,
            cancellationToken);
    }

    public Task<ObservationInfo> GetObservationInfoAsync(
        PointName point,
        int observationIndex = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetObservationInfoRequest(),
            new Dictionary<string, object?>
            {
                ["point"] = point,
                ["observation_index"] = observationIndex,
            });
        return _client.InvokeOperationAsync<ObservationInfo>(
            "briosa.InstrumentOperations",
            "GetObservationInfo",
            request,
            Transport.GetObservationInfoResult.Parser,
            cancellationToken);
    }

    public Task<InstrumentXyzUncertainties> GetPcmmInstrumentXyzUncertaintiesAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetPcmmInstrumentXyzUncertaintiesRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<InstrumentXyzUncertainties>(
            "briosa.InstrumentOperations",
            "GetPcmmInstrumentXyzUncertainties",
            request,
            Transport.GetPcmmInstrumentXyzUncertaintiesResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<PointName>> GetTargetsMeasuredByInstrumentAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetTargetsMeasuredByInstrumentRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<PointName>>(
            "briosa.InstrumentOperations",
            "GetTargetsMeasuredByInstrument",
            request,
            Transport.GetTargetsMeasuredByInstrumentResult.Parser,
            cancellationToken);
    }

    public Task<TrackerEdmTheodoliteUncertainties> GetTrackerEdmTheodoliteUncertaintiesAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetTrackerEdmTheodoliteUncertaintiesRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<TrackerEdmTheodoliteUncertainties>(
            "briosa.InstrumentOperations",
            "GetTrackerEdmTheodoliteUncertainties",
            request,
            Transport.GetTrackerEdmTheodoliteUncertaintiesResult.Parser,
            cancellationToken);
    }

    public Task<WrtlChannelStatus> GetWrtlChannelAndStatusAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetWrtlChannelAndStatusRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<WrtlChannelStatus>(
            "briosa.InstrumentOperations",
            "GetWrtlChannelAndStatus",
            request,
            Transport.GetWrtlChannelAndStatusResult.Parser,
            cancellationToken);
    }

    public Task<InstrumentXyzUncertainties> GetXyzInstrumentUncertaintiesAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetXyzInstrumentUncertaintiesRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<InstrumentXyzUncertainties>(
            "briosa.InstrumentOperations",
            "GetXyzInstrumentUncertainties",
            request,
            Transport.GetXyzInstrumentUncertaintiesResult.Parser,
            cancellationToken);
    }

    public Task GuideObjectsIn6dBasedOnPointMeasurementsAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName destinationGroup,
        CollectionObjectName movingReferenceGroup,
        IEnumerable<CollectionObjectName> objectsToMove,
        CollectionObjectName? initialSurveyGroup = null,
        ToleranceVectorOptions? positionalTolerance = null,
        ToleranceVectorOptions? rotationalTolerance = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GuideObjectsIn6dBasedOnPointMeasurementsRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["destination_group"] = destinationGroup,
                ["moving_reference_group"] = movingReferenceGroup,
                ["objects_to_move"] = objectsToMove,
                ["initial_survey_group"] = initialSurveyGroup,
                ["positional_tolerance"] = positionalTolerance,
                ["rotational_tolerance"] = rotationalTolerance,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "GuideObjectsIn6dBasedOnPointMeasurements",
            request,
            Transport.GuideObjectsIn6dBasedOnPointMeasurementsResult.Parser,
            cancellationToken);
    }

    public Task InitiateServoGuideAsync(
        CollectionInstrumentId instrument,
        IEnumerable<PointName> nominalPoints,
        string groupNameSuffix = "",
        string targetNameSuffix = "",
        double tolerance = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.InitiateServoGuideRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["nominal_points"] = nominalPoints,
                ["group_name_suffix"] = groupNameSuffix,
                ["target_name_suffix"] = targetNameSuffix,
                ["tolerance"] = tolerance,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "InitiateServoGuide",
            request,
            Transport.InitiateServoGuideResult.Parser,
            cancellationToken);
    }

    public Task InstrumentOperationalCheckAsync(
        CollectionInstrumentId instrument,
        string checkType,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.InstrumentOperationalCheckRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["check_type"] = checkType,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "InstrumentOperationalCheck",
            request,
            Transport.InstrumentOperationalCheckResult.Parser,
            cancellationToken);
    }

    public Task IssueInstrumentActuatorCommandAsync(
        CollectionInstrumentId instrument,
        string command,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.IssueInstrumentActuatorCommandRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["command"] = command,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "IssueInstrumentActuatorCommand",
            request,
            Transport.IssueInstrumentActuatorCommandResult.Parser,
            cancellationToken);
    }

    public Task JumpInstrumentToNewLocationAsync(
        CollectionInstrumentId liveInstrument,
        bool hidePreviousInstrument = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.JumpInstrumentToNewLocationRequest(),
            new Dictionary<string, object?>
            {
                ["live_instrument"] = liveInstrument,
                ["hide_previous_instrument"] = hidePreviousInstrument,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "JumpInstrumentToNewLocation",
            request,
            Transport.JumpInstrumentToNewLocationResult.Parser,
            cancellationToken);
    }

    public Task LoadCloudViewerPointCloudFileAsync(
        CollectionInstrumentId instrument,
        string filePath,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LoadCloudViewerPointCloudFileRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["file_path"] = filePath,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "LoadCloudViewerPointCloudFile",
            request,
            Transport.LoadCloudViewerPointCloudFileResult.Parser,
            cancellationToken);
    }

    public Task LoadInstrumentConfigurationAsync(
        CollectionInstrumentId instrument,
        FileReference configurationFile,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LoadInstrumentConfigurationRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["configuration_file"] = configurationFile,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "LoadInstrumentConfiguration",
            request,
            Transport.LoadInstrumentConfigurationResult.Parser,
            cancellationToken);
    }

    public Task<InstrumentBestFitResult> LocateInstrumentBestFitGroupToGroupAsync(
        CollectionObjectName referenceGroup,
        CollectionObjectName correspondingGroup,
        bool showInterface = false,
        double rmsTolerance = 0.0,
        double maximumAbsoluteTolerance = 0.0,
        bool allowScale = false,
        bool allowX = true,
        bool allowY = true,
        bool allowZ = true,
        bool allowRx = true,
        bool allowRy = true,
        bool allowRz = true,
        bool lockDegreesOfFreedom = false,
        bool generateEvent = false,
        FileReference? csvReport = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LocateInstrumentBestFitGroupToGroupRequest(),
            new Dictionary<string, object?>
            {
                ["reference_group"] = referenceGroup,
                ["corresponding_group"] = correspondingGroup,
                ["show_interface"] = showInterface,
                ["rms_tolerance"] = rmsTolerance,
                ["maximum_absolute_tolerance"] = maximumAbsoluteTolerance,
                ["allow_scale"] = allowScale,
                ["allow_x"] = allowX,
                ["allow_y"] = allowY,
                ["allow_z"] = allowZ,
                ["allow_rx"] = allowRx,
                ["allow_ry"] = allowRy,
                ["allow_rz"] = allowRz,
                ["lock_degrees_of_freedom"] = lockDegreesOfFreedom,
                ["generate_event"] = generateEvent,
                ["csv_report"] = csvReport,
            });
        return _client.InvokeOperationAsync<InstrumentBestFitResult>(
            "briosa.InstrumentOperations",
            "LocateInstrumentBestFitGroupToGroup",
            request,
            Transport.LocateInstrumentBestFitGroupToGroupResult.Parser,
            cancellationToken);
    }

    public Task<InstrumentBestFitResult> LocateInstrumentBestFitNominalGeometryAsync(
        CollectionInstrumentId instrument,
        IEnumerable<CollectionObjectName> geometryRelationships,
        bool showInterface = false,
        double rmsTolerance = 0.0,
        double maximumAbsoluteTolerance = 0.0,
        bool allowScale = false,
        bool allowX = true,
        bool allowY = true,
        bool allowZ = true,
        bool allowRx = true,
        bool allowRy = true,
        bool allowRz = true,
        bool lockDegreesOfFreedom = false,
        bool generateEvent = false,
        FileReference? csvReport = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LocateInstrumentBestFitNominalGeometryRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["geometry_relationships"] = geometryRelationships,
                ["show_interface"] = showInterface,
                ["rms_tolerance"] = rmsTolerance,
                ["maximum_absolute_tolerance"] = maximumAbsoluteTolerance,
                ["allow_scale"] = allowScale,
                ["allow_x"] = allowX,
                ["allow_y"] = allowY,
                ["allow_z"] = allowZ,
                ["allow_rx"] = allowRx,
                ["allow_ry"] = allowRy,
                ["allow_rz"] = allowRz,
                ["lock_degrees_of_freedom"] = lockDegreesOfFreedom,
                ["generate_event"] = generateEvent,
                ["csv_report"] = csvReport,
            });
        return _client.InvokeOperationAsync<InstrumentBestFitResult>(
            "briosa.InstrumentOperations",
            "LocateInstrumentBestFitNominalGeometry",
            request,
            Transport.LocateInstrumentBestFitNominalGeometryResult.Parser,
            cancellationToken);
    }

    public Task<FitErrorResult> LocateInstrumentGroupToSurfaceQuickFitAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName measuredGroup,
        CollectionObjectName surfacePointsGroup,
        CollectionObjectName surfaceToFit,
        IEnumerable<CollectionObjectName>? otherObjectsToTransform = null,
        double rmsTolerance = 0.0,
        double maximumAbsoluteTolerance = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LocateInstrumentGroupToSurfaceQuickFitRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["measured_group"] = measuredGroup,
                ["surface_points_group"] = surfacePointsGroup,
                ["surface_to_fit"] = surfaceToFit,
                ["other_objects_to_transform"] = otherObjectsToTransform,
                ["rms_tolerance"] = rmsTolerance,
                ["maximum_absolute_tolerance"] = maximumAbsoluteTolerance,
            });
        return _client.InvokeOperationAsync<FitErrorResult>(
            "briosa.InstrumentOperations",
            "LocateInstrumentGroupToSurfaceQuickFit",
            request,
            Transport.LocateInstrumentGroupToSurfaceQuickFitResult.Parser,
            cancellationToken);
    }

    public Task LocateInstrumentRefTieInAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName referenceGroup,
        CollectionObjectName actualsGroup,
        double tolerance = 0.0,
        bool autoSurvey = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LocateInstrumentRefTieInRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["reference_group"] = referenceGroup,
                ["actuals_group"] = actualsGroup,
                ["tolerance"] = tolerance,
                ["auto_survey"] = autoSurvey,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "LocateInstrumentRefTieIn",
            request,
            Transport.LocateInstrumentRefTieInResult.Parser,
            cancellationToken);
    }

    public Task<FitErrorResult> LocateInstrumentsUsmnAsync(
        IEnumerable<CollectionInstrumentId> instruments,
        CollectionObjectName outputGroup,
        CollectionObjectName? nominalsGroup = null,
        bool moveInWorkingFrame = false,
        bool autoRejectOutliersAndResolve = false,
        ShowUsmnDialog showUsmnDialog = ShowUsmnDialog.No,
        double maximumAcceptableRmsError = 0.0,
        double maximumAcceptableError = 0.0,
        IEnumerable<CollectionObjectName>? excludedGroups = null,
        bool excludeSingleInstrumentPoints = false,
        bool runUncertaintyFieldAnalysis = false,
        int analysisSamples = 300,
        double analysisTimeLimitMinutes = 4.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LocateInstrumentsUsmnRequest(),
            new Dictionary<string, object?>
            {
                ["instruments"] = instruments,
                ["nominals_group"] = nominalsGroup,
                ["output_group"] = outputGroup,
                ["move_in_working_frame"] = moveInWorkingFrame,
                ["auto_reject_outliers_and_resolve"] = autoRejectOutliersAndResolve,
                ["show_usmn_dialog"] = showUsmnDialog,
                ["maximum_acceptable_rms_error"] = maximumAcceptableRmsError,
                ["maximum_acceptable_error"] = maximumAcceptableError,
                ["excluded_groups"] = excludedGroups,
                ["exclude_single_instrument_points"] = excludeSingleInstrumentPoints,
                ["run_uncertainty_field_analysis"] = runUncertaintyFieldAnalysis,
                ["analysis_samples"] = analysisSamples,
                ["analysis_time_limit_minutes"] = analysisTimeLimitMinutes,
            });
        return _client.InvokeOperationAsync<FitErrorResult>(
            "briosa.InstrumentOperations",
            "LocateInstrumentsUsmn",
            request,
            Transport.LocateInstrumentsUsmnResult.Parser,
            cancellationToken);
    }

    public Task<string> LrApdisActivateMcmCalibrationAsync(
        CollectionInstrumentId instrument,
        string calibrationName = "",
        int calibrationId = -1,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LrApdisActivateMcmCalibrationRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["calibration_name"] = calibrationName,
                ["calibration_id"] = calibrationId,
            });
        return _client.InvokeOperationAsync<string>(
            "briosa.InstrumentOperations",
            "LrApdisActivateMcmCalibration",
            request,
            Transport.LrApdisActivateMcmCalibrationResult.Parser,
            cancellationToken);
    }

    public Task<string> LrApdisGetActiveMcmCalibrationAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LrApdisGetActiveMcmCalibrationRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<string>(
            "briosa.InstrumentOperations",
            "LrApdisGetActiveMcmCalibration",
            request,
            Transport.LrApdisGetActiveMcmCalibrationResult.Parser,
            cancellationToken);
    }

    public Task LrApdisPerformMcmCalibrationAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName nominalGroup,
        bool useMatteToolingBall = true,
        string newCalibrationName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LrApdisPerformMcmCalibrationRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["nominal_group"] = nominalGroup,
                ["use_matte_tooling_ball"] = useMatteToolingBall,
                ["new_calibration_name"] = newCalibrationName,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "LrApdisPerformMcmCalibration",
            request,
            Transport.LrApdisPerformMcmCalibrationResult.Parser,
            cancellationToken);
    }

    public Task<LrSnrInfo> LrGetMostRecentSnrInfoAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LrGetMostRecentSnrInfoRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<LrSnrInfo>(
            "briosa.InstrumentOperations",
            "LrGetMostRecentSnrInfo",
            request,
            Transport.LrGetMostRecentSnrInfoResult.Parser,
            cancellationToken);
    }

    public Task LrHardwareConnectAsync(
        CollectionInstrumentId instrument,
        string host,
        int port,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LrHardwareConnectRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["host"] = host,
                ["port"] = port,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "LrHardwareConnect",
            request,
            Transport.LrHardwareConnectResult.Parser,
            cancellationToken);
    }

    public Task LrHardwareDisconnectAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LrHardwareDisconnectRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "LrHardwareDisconnect",
            request,
            Transport.LrHardwareDisconnectResult.Parser,
            cancellationToken);
    }

    public Task<LrSelfTestResult> LrSelfTestAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LrSelfTestRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<LrSelfTestResult>(
            "briosa.InstrumentOperations",
            "LrSelfTest",
            request,
            Transport.LrSelfTestResult.Parser,
            cancellationToken);
    }

    public Task<LrFlipTestResult> LrSelfTestFlipTestAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LrSelfTestFlipTestRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<LrFlipTestResult>(
            "briosa.InstrumentOperations",
            "LrSelfTestFlipTest",
            request,
            Transport.LrSelfTestFlipTestResult.Parser,
            cancellationToken);
    }

    public Task<double> LrSelfTestLinearizationAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LrSelfTestLinearizationRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<double>(
            "briosa.InstrumentOperations",
            "LrSelfTestLinearization",
            request,
            Transport.LrSelfTestLinearizationResult.Parser,
            cancellationToken);
    }

    public Task<LrLoSeparationTestResult> LrSelfTestLoSepAsync(
        CollectionInstrumentId instrument,
        int region = 0,
        int numRangeMeasurements = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LrSelfTestLoSepRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["region"] = region,
                ["num_range_measurements"] = numRangeMeasurements,
            });
        return _client.InvokeOperationAsync<LrLoSeparationTestResult>(
            "briosa.InstrumentOperations",
            "LrSelfTestLoSep",
            request,
            Transport.LrSelfTestLoSepResult.Parser,
            cancellationToken);
    }

    public Task LrSetRedLaserIntensityAsync(
        CollectionInstrumentId instrument,
        int intensity = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LrSetRedLaserIntensityRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["intensity"] = intensity,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "LrSetRedLaserIntensity",
            request,
            Transport.LrSetRedLaserIntensityResult.Parser,
            cancellationToken);
    }

    public Task<bool> LrVerifyHardwareConnectionAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.LrVerifyHardwareConnectionRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<bool>(
            "briosa.InstrumentOperations",
            "LrVerifyHardwareConnection",
            request,
            Transport.LrVerifyHardwareConnectionResult.Parser,
            cancellationToken);
    }

    public Task<IReadOnlyList<CollectionObjectName>> MakeCollectionObjectNameRefListFromObjectsAssociatedWithInstrumentsAsync(
        IEnumerable<CollectionInstrumentId> instruments,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeCollectionObjectNameRefListFromObjectsAssociatedWithInstrumentsRequest(),
            new Dictionary<string, object?>
            {
                ["instruments"] = instruments,
            });
        return _client.InvokeOperationAsync<IReadOnlyList<CollectionObjectName>>(
            "briosa.InstrumentOperations",
            "MakeCollectionObjectNameRefListFromObjectsAssociatedWithInstruments",
            request,
            Transport.MakeCollectionObjectNameRefListFromObjectsAssociatedWithInstrumentsResult.Parser,
            cancellationToken);
    }

    public Task<SurfaceFaceList> MakeSurfaceFaceListFromPointProximityAsync(
        IReadOnlyList<PointName> measuredPoints,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MakeSurfaceFaceListFromPointProximityRequest(),
            new Dictionary<string, object?>
            {
                ["measured_points"] = measuredPoints,
            });
        return _client.InvokeOperationAsync<SurfaceFaceList>(
            "briosa.InstrumentOperations",
            "MakeSurfaceFaceListFromPointProximity",
            request,
            Transport.MakeSurfaceFaceListFromPointProximityResult.Parser,
            cancellationToken);
    }

    public Task MeasureAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MeasureRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "Measure",
            request,
            Transport.MeasureResult.Parser,
            cancellationToken);
    }

    public Task<PointName> MeasureExistingSinglePointAsync(
        CollectionInstrumentId instrument,
        PointName existingTargetId,
        CollectionObjectName groupNameForNewPoint,
        bool measureImmediately = false,
        FileReference? htmlPromptFile = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MeasureExistingSinglePointRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["existing_target_id"] = existingTargetId,
                ["group_name_for_new_point"] = groupNameForNewPoint,
                ["measure_immediately"] = measureImmediately,
                ["html_prompt_file"] = htmlPromptFile,
            });
        return _client.InvokeOperationAsync<PointName>(
            "briosa.InstrumentOperations",
            "MeasureExistingSinglePoint",
            request,
            Transport.MeasureExistingSinglePointResult.Parser,
            cancellationToken);
    }

    public Task<PointComparisonResult> MeasureExistingSinglePointAndCompareAsync(
        CollectionInstrumentId instrument,
        PointName existingTargetId,
        CollectionObjectName groupNameForNewPoint,
        bool measureImmediately = false,
        FileReference? htmlPromptFile = null,
        double tolerance = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MeasureExistingSinglePointAndCompareRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["existing_target_id"] = existingTargetId,
                ["group_name_for_new_point"] = groupNameForNewPoint,
                ["measure_immediately"] = measureImmediately,
                ["html_prompt_file"] = htmlPromptFile,
                ["tolerance"] = tolerance,
            });
        return _client.InvokeOperationAsync<PointComparisonResult>(
            "briosa.InstrumentOperations",
            "MeasureExistingSinglePointAndCompare",
            request,
            Transport.MeasureExistingSinglePointAndCompareResult.Parser,
            cancellationToken);
    }

    public Task<PointName> MeasureExistingSinglePointManualGuideAsync(
        CollectionInstrumentId instrument,
        PointName existingTargetId,
        CollectionObjectName groupNameForNewPoint,
        bool measureImmediately = false,
        FileReference? htmlPromptFile = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MeasureExistingSinglePointManualGuideRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["existing_target_id"] = existingTargetId,
                ["group_name_for_new_point"] = groupNameForNewPoint,
                ["measure_immediately"] = measureImmediately,
                ["html_prompt_file"] = htmlPromptFile,
            });
        return _client.InvokeOperationAsync<PointName>(
            "briosa.InstrumentOperations",
            "MeasureExistingSinglePointManualGuide",
            request,
            Transport.MeasureExistingSinglePointManualGuideResult.Parser,
            cancellationToken);
    }

    public Task MeasureNominalFeatureAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName feature,
        PointName resultingPoint,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MeasureNominalFeatureRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["feature"] = feature,
                ["resulting_point"] = resultingPoint,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "MeasureNominalFeature",
            request,
            Transport.MeasureNominalFeatureResult.Parser,
            cancellationToken);
    }

    public Task MeasureSinglePointHereAsync(
        CollectionInstrumentId instrument,
        PointName targetId,
        bool measureImmediately = false,
        FileReference? htmlPromptFile = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MeasureSinglePointHereRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["target_id"] = targetId,
                ["measure_immediately"] = measureImmediately,
                ["html_prompt_file"] = htmlPromptFile,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "MeasureSinglePointHere",
            request,
            Transport.MeasureSinglePointHereResult.Parser,
            cancellationToken);
    }

    public Task MoveInstrumentToAnotherCollectionAsync(
        CollectionInstrumentId instrument,
        CollectionName collectionName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveInstrumentToAnotherCollectionRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["collection_name"] = collectionName,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "MoveInstrumentToAnotherCollection",
            request,
            Transport.MoveInstrumentToAnotherCollectionResult.Parser,
            cancellationToken);
    }

    public Task MoveMeasurementObservationAsync(
        PointName sourcePointName,
        PointName destinationPointName,
        int observationIndex = 0,
        bool deletePointIfNoMeasurementsRemain = false,
        bool forceObservationActive = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveMeasurementObservationRequest(),
            new Dictionary<string, object?>
            {
                ["source_point_name"] = sourcePointName,
                ["observation_index"] = observationIndex,
                ["delete_point_if_no_measurements_remain"] = deletePointIfNoMeasurementsRemain,
                ["destination_point_name"] = destinationPointName,
                ["force_observation_active"] = forceObservationActive,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "MoveMeasurementObservation",
            request,
            Transport.MoveMeasurementObservationResult.Parser,
            cancellationToken);
    }

    public Task MoveObjectsIn6dUsingInstrumentUpdatesAsync(
        CollectionInstrumentId instrument,
        IEnumerable<CollectionObjectName> objectsToMove,
        string measurementMode,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveObjectsIn6dUsingInstrumentUpdatesRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["objects_to_move"] = objectsToMove,
                ["measurement_mode"] = measurementMode,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "MoveObjectsIn6dUsingInstrumentUpdates",
            request,
            Transport.MoveObjectsIn6dUsingInstrumentUpdatesResult.Parser,
            cancellationToken);
    }

    public Task MultiMeasurementInitiateAsync(
        IEnumerable<CollectionInstrumentId> instruments,
        string measurementMode,
        bool waitForCompletion = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MultiMeasurementInitiateRequest(),
            new Dictionary<string, object?>
            {
                ["instruments"] = instruments,
                ["measurement_mode"] = measurementMode,
                ["wait_for_completion"] = waitForCompletion,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "MultiMeasurementInitiate",
            request,
            Transport.MultiMeasurementInitiateResult.Parser,
            cancellationToken);
    }

    public Task MultiMeasurementStopAsync(
        IEnumerable<CollectionInstrumentId> instruments,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MultiMeasurementStopRequest(),
            new Dictionary<string, object?>
            {
                ["instruments"] = instruments,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "MultiMeasurementStop",
            request,
            Transport.MultiMeasurementStopResult.Parser,
            cancellationToken);
    }

    public Task PointAtTargetAsync(
        CollectionInstrumentId instrument,
        PointName targetId,
        FileReference? htmlPromptFile = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.PointAtTargetRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["target_id"] = targetId,
                ["html_prompt_file"] = htmlPromptFile,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "PointAtTarget",
            request,
            Transport.PointAtTargetResult.Parser,
            cancellationToken);
    }

    public Task QuickAlignAsync(
        IEnumerable<CollectionInstrumentId> instruments,
        IEnumerable<CollectionObjectName> objects,
        IEnumerable<PointName>? nominalPoints = null,
        IEnumerable<string>? nominalPointOfViewNames = null,
        bool alignToIndividualFacesOnly = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.QuickAlignRequest(),
            new Dictionary<string, object?>
            {
                ["instruments"] = instruments,
                ["objects"] = objects,
                ["nominal_points"] = nominalPoints,
                ["nominal_point_of_view_names"] = nominalPointOfViewNames,
                ["align_to_individual_faces_only"] = alignToIndividualFacesOnly,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "QuickAlign",
            request,
            Transport.QuickAlignResult.Parser,
            cancellationToken);
    }

    public Task RenameInstrumentAsync(
        CollectionInstrumentId instrument,
        string newName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RenameInstrumentRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["new_name"] = newName,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "RenameInstrument",
            request,
            Transport.RenameInstrumentResult.Parser,
            cancellationToken);
    }

    public Task SaveCloudViewerPointCloudFileAsync(
        CollectionInstrumentId instrument,
        string filePath,
        bool saveAsAscii = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SaveCloudViewerPointCloudFileRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["file_path"] = filePath,
                ["save_as_ascii"] = saveAsAscii,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SaveCloudViewerPointCloudFile",
            request,
            Transport.SaveCloudViewerPointCloudFileResult.Parser,
            cancellationToken);
    }

    public Task SaveInstrumentConfigurationAsync(
        CollectionInstrumentId instrument,
        FileReference configurationFile,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SaveInstrumentConfigurationRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["configuration_file"] = configurationFile,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SaveInstrumentConfiguration",
            request,
            Transport.SaveInstrumentConfigurationResult.Parser,
            cancellationToken);
    }

    public Task ScanCadFacesAsync(
        CollectionInstrumentId instrument,
        SurfaceFaceList surfaceFaces,
        string parameterSetName,
        bool enableExclusions = true,
        bool waitForCompletion = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ScanCadFacesRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["surface_faces"] = surfaceFaces,
                ["parameter_set_name"] = parameterSetName,
                ["enable_exclusions"] = enableExclusions,
                ["wait_for_completion"] = waitForCompletion,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "ScanCadFaces",
            request,
            Transport.ScanCadFacesResult.Parser,
            cancellationToken);
    }

    public Task ScanWithinPerimeterAsync(
        CollectionInstrumentId instrument,
        IReadOnlyList<CollectionObjectName> scanPerimeters,
        IReadOnlyList<CollectionObjectName> exclusionPerimeters,
        string parameterSetName,
        CollectionObjectName pointGroup,
        bool waitForCompletion = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ScanWithinPerimeterRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["scan_perimeters"] = scanPerimeters,
                ["exclusion_perimeters"] = exclusionPerimeters,
                ["parameter_set_name"] = parameterSetName,
                ["point_group"] = pointGroup,
                ["wait_for_completion"] = waitForCompletion,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "ScanWithinPerimeter",
            request,
            Transport.ScanWithinPerimeterResult.Parser,
            cancellationToken);
    }

    public Task SendCloudToSaAsync(
        CollectionInstrumentId instrument,
        string cloudName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SendCloudToSaRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["cloud_name"] = cloudName,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SendCloudToSa",
            request,
            Transport.SendCloudToSaResult.Parser,
            cancellationToken);
    }

    public Task SetAbsoluteInstrumentScaleFactorAsync(
        CollectionInstrumentId instrument,
        double scaleFactor = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetAbsoluteInstrumentScaleFactorRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["scale_factor"] = scaleFactor,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetAbsoluteInstrumentScaleFactor",
            request,
            Transport.SetAbsoluteInstrumentScaleFactorResult.Parser,
            cancellationToken);
    }

    public Task SetAlignmentProjectorAsync(
        CollectionInstrumentId instrument,
        string projectorProfile,
        string userPrompt = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetAlignmentProjectorRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["projector_profile"] = projectorProfile,
                ["user_prompt"] = userPrompt,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetAlignmentProjector",
            request,
            Transport.SetAlignmentProjectorResult.Parser,
            cancellationToken);
    }

    public Task SetCloudViewerFilterAsync(
        CollectionInstrumentId instrument,
        int filterValue = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCloudViewerFilterRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["filter_value"] = filterValue,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetCloudViewerFilter",
            request,
            Transport.SetCloudViewerFilterResult.Parser,
            cancellationToken);
    }

    public Task SetInspectionVerificationModeAsync(
        bool verificationEnabled = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetInspectionVerificationModeRequest(),
            new Dictionary<string, object?>
            {
                ["verification_enabled"] = verificationEnabled,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetInspectionVerificationMode",
            request,
            Transport.SetInspectionVerificationModeResult.Parser,
            cancellationToken);
    }

    public Task SetInstrumentAxesAsync(
        CollectionInstrumentId instrumentToAdjust,
        IEnumerable<double> axisValues,
        int numberOfSteps = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetInstrumentAxesRequest(),
            new Dictionary<string, object?>
            {
                ["instrument_to_adjust"] = instrumentToAdjust,
                ["axis_values"] = axisValues,
                ["number_of_steps"] = numberOfSteps,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetInstrumentAxes",
            request,
            Transport.SetInstrumentAxesResult.Parser,
            cancellationToken);
    }

    public Task SetInstrumentBaseUncertaintyCovarianceMatrixWrtBaseAsync(
        CollectionInstrumentId instrument,
        UncertaintyCovarianceMatrix covarianceMatrix,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetInstrumentBaseUncertaintyCovarianceMatrixWrtBaseRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["covariance_matrix"] = covarianceMatrix,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetInstrumentBaseUncertaintyCovarianceMatrixWrtBase",
            request,
            Transport.SetInstrumentBaseUncertaintyCovarianceMatrixWrtBaseResult.Parser,
            cancellationToken);
    }

    public Task SetInstrumentBaseUncertaintyCovarianceMatrixWrtWorldAsync(
        CollectionInstrumentId instrument,
        UncertaintyCovarianceMatrix covarianceMatrix,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetInstrumentBaseUncertaintyCovarianceMatrixWrtWorldRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["covariance_matrix"] = covarianceMatrix,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetInstrumentBaseUncertaintyCovarianceMatrixWrtWorld",
            request,
            Transport.SetInstrumentBaseUncertaintyCovarianceMatrixWrtWorldResult.Parser,
            cancellationToken);
    }

    public Task SetInstrumentGroupAndTargetAsync(
        CollectionInstrumentId instrument,
        PointName point,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetInstrumentGroupAndTargetRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["point"] = point,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetInstrumentGroupAndTarget",
            request,
            Transport.SetInstrumentGroupAndTargetResult.Parser,
            cancellationToken);
    }

    public Task SetInstrumentInterfaceResponseTimeoutAsync(
        CollectionInstrumentId instrument,
        double timeoutSeconds = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetInstrumentInterfaceResponseTimeoutRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["timeout_seconds"] = timeoutSeconds,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetInstrumentInterfaceResponseTimeout",
            request,
            Transport.SetInstrumentInterfaceResponseTimeoutResult.Parser,
            cancellationToken);
    }

    public Task SetInstrumentMeasurementModeProfileAsync(
        CollectionInstrumentId instrument,
        string modeProfile,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetInstrumentMeasurementModeProfileRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["mode_profile"] = modeProfile,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetInstrumentMeasurementModeProfile",
            request,
            Transport.SetInstrumentMeasurementModeProfileResult.Parser,
            cancellationToken);
    }

    public Task SetInstrumentTargetingAsync(
        CollectionInstrumentId instrument,
        string targetingName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetInstrumentTargetingRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["targeting_name"] = targetingName,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetInstrumentTargeting",
            request,
            Transport.SetInstrumentTargetingResult.Parser,
            cancellationToken);
    }

    public Task SetInstrumentTransformAsync(
        CollectionInstrumentId instrument,
        Transform destinationTransform,
        CollectionObjectName referenceFrame,
        int numberOfSteps = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetInstrumentTransformRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["destination_transform"] = destinationTransform,
                ["reference_frame"] = referenceFrame,
                ["number_of_steps"] = numberOfSteps,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetInstrumentTransform",
            request,
            Transport.SetInstrumentTransformResult.Parser,
            cancellationToken);
    }

    public Task SetInstrumentWeatherSettingAsync(
        CollectionInstrumentId instrument,
        double temperatureFahrenheit = 0.0,
        double pressureMmHg = 0.0,
        double relativeHumidityPercent = 0.0,
        bool setAutomatically = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetInstrumentWeatherSettingRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["temperature_fahrenheit"] = temperatureFahrenheit,
                ["pressure_mmhg"] = pressureMmHg,
                ["relative_humidity_percent"] = relativeHumidityPercent,
                ["set_automatically"] = setAutomatically,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetInstrumentWeatherSetting",
            request,
            Transport.SetInstrumentWeatherSettingResult.Parser,
            cancellationToken);
    }

    public Task SetLadarAutoMeasPointAsync(
        CollectionInstrumentId instrument,
        int sampleTimeMilliseconds = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetLadarAutoMeasPointRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["sample_time_milliseconds"] = sampleTimeMilliseconds,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetLadarAutoMeasPoint",
            request,
            Transport.SetLadarAutoMeasPointResult.Parser,
            cancellationToken);
    }

    public Task SetLadarAutoMeasSphereAsync(
        CollectionInstrumentId instrument,
        double sphereRadius = 1.1875,
        double scanLineSpacing = 0.05,
        bool sendCenterPoint = true,
        bool sendSphere = false,
        bool sendMeasuredCloud = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetLadarAutoMeasSphereRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["sphere_radius"] = sphereRadius,
                ["scan_line_spacing"] = scanLineSpacing,
                ["send_center_point"] = sendCenterPoint,
                ["send_sphere"] = sendSphere,
                ["send_measured_cloud"] = sendMeasuredCloud,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetLadarAutoMeasSphere",
            request,
            Transport.SetLadarAutoMeasSphereResult.Parser,
            cancellationToken);
    }

    public Task SetLadarFeatureMeasCircleAsync(
        CollectionInstrumentId instrument,
        double scanLineSpacing = 0.05,
        double widthOfExtraAreaAroundScan = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetLadarFeatureMeasCircleRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["scan_line_spacing"] = scanLineSpacing,
                ["width_of_extra_area_around_scan"] = widthOfExtraAreaAroundScan,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetLadarFeatureMeasCircle",
            request,
            Transport.SetLadarFeatureMeasCircleResult.Parser,
            cancellationToken);
    }

    public Task SetLadarFeatureMeasCylinderAsync(
        CollectionInstrumentId instrument,
        double scanLineSpacing = 0.05,
        double widthOfExtraAreaAroundScan = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetLadarFeatureMeasCylinderRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["scan_line_spacing"] = scanLineSpacing,
                ["width_of_extra_area_around_scan"] = widthOfExtraAreaAroundScan,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetLadarFeatureMeasCylinder",
            request,
            Transport.SetLadarFeatureMeasCylinderResult.Parser,
            cancellationToken);
    }

    public Task SetLadarFeatureMeasSlotAsync(
        CollectionInstrumentId instrument,
        double scanLineSpacing = 0.05,
        double widthOfExtraAreaAroundScan = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetLadarFeatureMeasSlotRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["scan_line_spacing"] = scanLineSpacing,
                ["width_of_extra_area_around_scan"] = widthOfExtraAreaAroundScan,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetLadarFeatureMeasSlot",
            request,
            Transport.SetLadarFeatureMeasSlotResult.Parser,
            cancellationToken);
    }

    public Task SetLadarFeatureMeasSphereAsync(
        CollectionInstrumentId instrument,
        double scanLineSpacing = 0.05,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetLadarFeatureMeasSphereRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["scan_line_spacing"] = scanLineSpacing,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetLadarFeatureMeasSphere",
            request,
            Transport.SetLadarFeatureMeasSphereResult.Parser,
            cancellationToken);
    }

    public Task SetMultiplyInstrumentScaleFactorAsync(
        CollectionInstrumentId instrument,
        double scaleFactor = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetMultiplyInstrumentScaleFactorRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["scale_factor"] = scaleFactor,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetMultiplyInstrumentScaleFactor",
            request,
            Transport.SetMultiplyInstrumentScaleFactorResult.Parser,
            cancellationToken);
    }

    public Task SetObservationCollimationShotOptionsAsync(
        PointName point,
        int observationIndex = 0,
        bool isCollimationShot = false,
        CollectionInstrumentId? targetedInstrument = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetObservationCollimationShotOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["point"] = point,
                ["observation_index"] = observationIndex,
                ["is_collimation_shot"] = isCollimationShot,
                ["targeted_instrument"] = targetedInstrument,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetObservationCollimationShotOptions",
            request,
            Transport.SetObservationCollimationShotOptionsResult.Parser,
            cancellationToken);
    }

    public Task SetObservationMirrorCubeShotFaceAsync(
        PointName point,
        int observationIndex = 0,
        bool isMirrorCubeShot = false,
        int mirrorCubeShotFace = 1,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetObservationMirrorCubeShotFaceRequest(),
            new Dictionary<string, object?>
            {
                ["point"] = point,
                ["observation_index"] = observationIndex,
                ["is_mirror_cube_shot"] = isMirrorCubeShot,
                ["mirror_cube_shot_face"] = mirrorCubeShotFace,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetObservationMirrorCubeShotFace",
            request,
            Transport.SetObservationMirrorCubeShotFaceResult.Parser,
            cancellationToken);
    }

    public Task SetObservationStatusAsync(
        PointName point,
        int observationIndex = 0,
        bool active = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetObservationStatusRequest(),
            new Dictionary<string, object?>
            {
                ["point"] = point,
                ["observation_index"] = observationIndex,
                ["active"] = active,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetObservationStatus",
            request,
            Transport.SetObservationStatusResult.Parser,
            cancellationToken);
    }

    public Task SetPcmmInstrumentXyzUncertaintiesAsync(
        CollectionInstrumentId instrument,
        double xUncertainty = 0.001,
        double yUncertainty = 0.001,
        double zUncertainty = 0.001,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetPcmmInstrumentXyzUncertaintiesRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["x_uncertainty"] = xUncertainty,
                ["y_uncertainty"] = yUncertainty,
                ["z_uncertainty"] = zUncertainty,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetPcmmInstrumentXyzUncertainties",
            request,
            Transport.SetPcmmInstrumentXyzUncertaintiesResult.Parser,
            cancellationToken);
    }

    public Task SetProbeOffsetFrameOfflineAsync(
        CollectionInstrumentId instrument,
        string probeName,
        CollectionObjectName rawMeasuredFrame,
        CollectionObjectName offsetFrame,
        int faceId = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetProbeOffsetFrameOfflineRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["probe_name"] = probeName,
                ["face_id"] = faceId,
                ["raw_measured_frame"] = rawMeasuredFrame,
                ["offset_frame"] = offsetFrame,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetProbeOffsetFrameOffline",
            request,
            Transport.SetProbeOffsetFrameOfflineResult.Parser,
            cancellationToken);
    }

    public Task SetProbeOffsetFrameOnlineAsync(
        CollectionInstrumentId instrument,
        string probeName,
        CollectionObjectName offsetFrame,
        int faceId = 0,
        string measureProfileName = "",
        double timeoutSeconds = 15.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetProbeOffsetFrameOnlineRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["probe_name"] = probeName,
                ["face_id"] = faceId,
                ["measure_profile_name"] = measureProfileName,
                ["timeout_seconds"] = timeoutSeconds,
                ["offset_frame"] = offsetFrame,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetProbeOffsetFrameOnline",
            request,
            Transport.SetProbeOffsetFrameOnlineResult.Parser,
            cancellationToken);
    }

    public Task SetRemeasureFailedChecksOnlyAsync(
        CollectionName collection,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRemeasureFailedChecksOnlyRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetRemeasureFailedChecksOnly",
            request,
            Transport.SetRemeasureFailedChecksOnlyResult.Parser,
            cancellationToken);
    }

    public Task SetTargetComputationOptionsAsync(
        TargetComputationMethod computationMethod = TargetComputationMethod.UseMostRecentShotFromEachFace,
        bool ignoreDistanceMeasurements = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetTargetComputationOptionsRequest(),
            new Dictionary<string, object?>
            {
                ["computation_method"] = computationMethod,
                ["ignore_distance_measurements"] = ignoreDistanceMeasurements,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetTargetComputationOptions",
            request,
            Transport.SetTargetComputationOptionsResult.Parser,
            cancellationToken);
    }

    public Task SetTrackerEdmTheodoliteUncertaintiesAsync(
        CollectionInstrumentId instrument,
        double thetaDispersionArcseconds = 1.0,
        double thetaThreshold = 0.001,
        double phiDispersionArcseconds = 1.0,
        double phiThreshold = 0.001,
        double distancePpm = 2.5,
        double distanceThreshold = 0.0003,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetTrackerEdmTheodoliteUncertaintiesRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["theta_dispersion_arcseconds"] = thetaDispersionArcseconds,
                ["theta_threshold"] = thetaThreshold,
                ["phi_dispersion_arcseconds"] = phiDispersionArcseconds,
                ["phi_threshold"] = phiThreshold,
                ["distance_ppm"] = distancePpm,
                ["distance_threshold"] = distanceThreshold,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetTrackerEdmTheodoliteUncertainties",
            request,
            Transport.SetTrackerEdmTheodoliteUncertaintiesResult.Parser,
            cancellationToken);
    }

    public Task SetWrtlChannelAsync(
        CollectionInstrumentId instrument,
        int channel = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetWrtlChannelRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["channel"] = channel,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetWrtlChannel",
            request,
            Transport.SetWrtlChannelResult.Parser,
            cancellationToken);
    }

    public Task SetXyzInstrumentUncertaintiesAsync(
        CollectionInstrumentId instrument,
        double xUncertainty = 0.0005,
        double yUncertainty = 0.0005,
        double zUncertainty = 0.0005,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetXyzInstrumentUncertaintiesRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["x_uncertainty"] = xUncertainty,
                ["y_uncertainty"] = yUncertainty,
                ["z_uncertainty"] = zUncertainty,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetXyzInstrumentUncertainties",
            request,
            Transport.SetXyzInstrumentUncertaintiesResult.Parser,
            cancellationToken);
    }

    public Task SetXyzReferenceFrameInstrumentBaseAnchorFrameAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName anchorFrame,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetXyzReferenceFrameInstrumentBaseAnchorFrameRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["anchor_frame"] = anchorFrame,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SetXyzReferenceFrameInstrumentBaseAnchorFrame",
            request,
            Transport.SetXyzReferenceFrameInstrumentBaseAnchorFrameResult.Parser,
            cancellationToken);
    }

    public Task StartGdtInspectionAsync(
        CollectionInstrumentId instrument,
        CollectionName collection,
        InspectionFilter filter = InspectionFilter.All,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StartGdtInspectionRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["collection"] = collection,
                ["filter"] = filter,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "StartGdtInspection",
            request,
            Transport.StartGdtInspectionResult.Parser,
            cancellationToken);
    }

    public Task StartGdtInspectionDesignAsync(
        CollectionName collection,
        InspectionFilter filter = InspectionFilter.All,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StartGdtInspectionDesignRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
                ["filter"] = filter,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "StartGdtInspectionDesign",
            request,
            Transport.StartGdtInspectionDesignResult.Parser,
            cancellationToken);
    }

    public Task StartGdtInspectionRehearseAsync(
        CollectionName collection,
        InspectionFilter filter = InspectionFilter.All,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StartGdtInspectionRehearseRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
                ["filter"] = filter,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "StartGdtInspectionRehearse",
            request,
            Transport.StartGdtInspectionRehearseResult.Parser,
            cancellationToken);
    }

    public Task StartInstrumentInterfaceAsync(
        CollectionInstrumentId instrument,
        bool initializeAtStartup = false,
        string? deviceIpAddress = null,
        int interfaceType = 0,
        bool runInSimulation = false,
        bool allowStartWithoutInitializationRequirements = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StartInstrumentInterfaceRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["initialize_at_startup"] = initializeAtStartup,
                ["device_ip_address"] = deviceIpAddress,
                ["interface_type"] = interfaceType,
                ["run_in_simulation"] = runInSimulation,
                ["allow_start_without_initialization_requirements"] = allowStartWithoutInitializationRequirements,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "StartInstrumentInterface",
            request,
            Transport.StartInstrumentInterfaceResult.Parser,
            cancellationToken);
    }

    public Task StartTheodoliteInterfaceAsync(
        CollectionInstrumentId instrument,
        string theodoliteType,
        int commPort = 0,
        string? deviceIpAddress = null,
        bool simulation = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StartTheodoliteInterfaceRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["theodolite_type"] = theodoliteType,
                ["comm_port"] = commPort,
                ["device_ip_address"] = deviceIpAddress,
                ["simulation"] = simulation,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "StartTheodoliteInterface",
            request,
            Transport.StartTheodoliteInterfaceResult.Parser,
            cancellationToken);
    }

    public Task StopActiveMeasurementModeAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StopActiveMeasurementModeRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "StopActiveMeasurementMode",
            request,
            Transport.StopActiveMeasurementModeResult.Parser,
            cancellationToken);
    }

    public Task StopInstrumentInterfaceAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StopInstrumentInterfaceRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "StopInstrumentInterface",
            request,
            Transport.StopInstrumentInterfaceResult.Parser,
            cancellationToken);
    }

    public Task SynchronizedMeasurementMasterSlaveAsync(
        CollectionInstrumentId masterInstrument,
        CollectionInstrumentId slaveInstrument,
        string slaveGroupSuffix = "_Slave",
        bool locateOneOfTheInstruments = true,
        bool locateMaster = false,
        bool waitForCompletion = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SynchronizedMeasurementMasterSlaveRequest(),
            new Dictionary<string, object?>
            {
                ["master_instrument"] = masterInstrument,
                ["slave_instrument"] = slaveInstrument,
                ["slave_group_suffix"] = slaveGroupSuffix,
                ["locate_one_of_the_instruments"] = locateOneOfTheInstruments,
                ["locate_master"] = locateMaster,
                ["wait_for_completion"] = waitForCompletion,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "SynchronizedMeasurementMasterSlave",
            request,
            Transport.SynchronizedMeasurementMasterSlaveResult.Parser,
            cancellationToken);
    }

    public Task TrackTapeMeasurementAsync(
        CollectionInstrumentId instrument,
        PointName pointOnTape,
        PointName pointOnPart,
        PointName directionPoint,
        PointName terminationPoint,
        string parameterSetName,
        CollectionObjectName pointGroup,
        string initialTargetName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.TrackTapeMeasurementRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["point_on_tape"] = pointOnTape,
                ["point_on_part"] = pointOnPart,
                ["direction_point"] = directionPoint,
                ["termination_point"] = terminationPoint,
                ["parameter_set_name"] = parameterSetName,
                ["point_group"] = pointGroup,
                ["initial_target_name"] = initialTargetName,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "TrackTapeMeasurement",
            request,
            Transport.TrackTapeMeasurementResult.Parser,
            cancellationToken);
    }

    public Task TransformInstrumentByDeltaAsync(
        CollectionInstrumentId instrument,
        WorldTransform deltaTransform,
        bool applyScaleToInstrument = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.TransformInstrumentByDeltaRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["delta_transform"] = deltaTransform,
                ["apply_scale_to_instrument"] = applyScaleToInstrument,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "TransformInstrumentByDelta",
            request,
            Transport.TransformInstrumentByDeltaResult.Parser,
            cancellationToken);
    }

    public Task TransformInstrumentFrameToFrameAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName initialFrame,
        CollectionObjectName destinationFrame,
        int numberOfSteps = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.TransformInstrumentFrameToFrameRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["initial_frame"] = initialFrame,
                ["destination_frame"] = destinationFrame,
                ["number_of_steps"] = numberOfSteps,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "TransformInstrumentFrameToFrame",
            request,
            Transport.TransformInstrumentFrameToFrameResult.Parser,
            cancellationToken);
    }

    public Task TransformMultipleInstrumentsByDeltaAsync(
        IEnumerable<CollectionInstrumentId> instruments,
        WorldTransform deltaTransform,
        bool applyScaleToInstruments = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.TransformMultipleInstrumentsByDeltaRequest(),
            new Dictionary<string, object?>
            {
                ["instruments"] = instruments,
                ["delta_transform"] = deltaTransform,
                ["apply_scale_to_instruments"] = applyScaleToInstruments,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "TransformMultipleInstrumentsByDelta",
            request,
            Transport.TransformMultipleInstrumentsByDeltaResult.Parser,
            cancellationToken);
    }

    public Task<bool> VerifyInstrumentConnectionAsync(
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.VerifyInstrumentConnectionRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync<bool>(
            "briosa.InstrumentOperations",
            "VerifyInstrumentConnection",
            request,
            Transport.VerifyInstrumentConnectionResult.Parser,
            cancellationToken);
    }

    public Task WaitForTrappingToCompleteAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.WaitForTrappingToCompleteRequest(),
            new Dictionary<string, object?>
            {
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "WaitForTrappingToComplete",
            request,
            Transport.WaitForTrappingToCompleteResult.Parser,
            cancellationToken);
    }

    public Task WatchClosestPointAsync(
        CollectionInstrumentId instrument,
        IEnumerable<CollectionObjectName> groupsToConsider,
        CollectionObjectName watchWindowProperties,
        string measurementMode = "",
        bool pauseMpUntilClosed = false,
        int windowTopLeftX = 0,
        int windowTopLeftY = 0,
        int windowWidth = 0,
        int windowHeight = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.WatchClosestPointRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["groups_to_consider"] = groupsToConsider,
                ["watch_window_properties"] = watchWindowProperties,
                ["measurement_mode"] = measurementMode,
                ["pause_mp_until_closed"] = pauseMpUntilClosed,
                ["window_top_left_x"] = windowTopLeftX,
                ["window_top_left_y"] = windowTopLeftY,
                ["window_width"] = windowWidth,
                ["window_height"] = windowHeight,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "WatchClosestPoint",
            request,
            Transport.WatchClosestPointResult.Parser,
            cancellationToken);
    }

    public Task WatchInstrumentAsync(
        CollectionInstrumentId instrument,
        CollectionObjectName watchWindowProperties,
        bool pauseMpUntilClosed = false,
        int windowTopLeftX = 0,
        int windowTopLeftY = 0,
        int windowWidth = 0,
        int windowHeight = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.WatchInstrumentRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["pause_mp_until_closed"] = pauseMpUntilClosed,
                ["watch_window_properties"] = watchWindowProperties,
                ["window_top_left_x"] = windowTopLeftX,
                ["window_top_left_y"] = windowTopLeftY,
                ["window_width"] = windowWidth,
                ["window_height"] = windowHeight,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "WatchInstrument",
            request,
            Transport.WatchInstrumentResult.Parser,
            cancellationToken);
    }

    public Task WatchPointToEdgeAsync(
        CollectionInstrumentId instrument,
        IEnumerable<CollectionObjectName> projectionReferenceObjects,
        IEnumerable<CollectionObjectName> measurementReferenceObjects,
        ProjectionOptions projectionOptions,
        CollectionObjectName watchWindowProperties,
        string measurementMode = "",
        bool pauseMpUntilClosed = false,
        int windowTopLeftX = 0,
        int windowTopLeftY = 0,
        int windowWidth = 0,
        int windowHeight = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.WatchPointToEdgeRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["projection_reference_objects"] = projectionReferenceObjects,
                ["measurement_reference_objects"] = measurementReferenceObjects,
                ["projection_options"] = projectionOptions,
                ["watch_window_properties"] = watchWindowProperties,
                ["measurement_mode"] = measurementMode,
                ["pause_mp_until_closed"] = pauseMpUntilClosed,
                ["window_top_left_x"] = windowTopLeftX,
                ["window_top_left_y"] = windowTopLeftY,
                ["window_width"] = windowWidth,
                ["window_height"] = windowHeight,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "WatchPointToEdge",
            request,
            Transport.WatchPointToEdgeResult.Parser,
            cancellationToken);
    }

    public Task WatchPointToObjectsAsync(
        CollectionInstrumentId instrument,
        IEnumerable<CollectionObjectName> objectsToConsider,
        ProjectionOptions projectionOptions,
        CollectionObjectName watchWindowProperties,
        string measurementMode = "",
        bool pauseMpUntilClosed = false,
        int windowTopLeftX = 0,
        int windowTopLeftY = 0,
        int windowWidth = 0,
        int windowHeight = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.WatchPointToObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["objects_to_consider"] = objectsToConsider,
                ["projection_options"] = projectionOptions,
                ["watch_window_properties"] = watchWindowProperties,
                ["measurement_mode"] = measurementMode,
                ["pause_mp_until_closed"] = pauseMpUntilClosed,
                ["window_top_left_x"] = windowTopLeftX,
                ["window_top_left_y"] = windowTopLeftY,
                ["window_width"] = windowWidth,
                ["window_height"] = windowHeight,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "WatchPointToObjects",
            request,
            Transport.WatchPointToObjectsResult.Parser,
            cancellationToken);
    }

    public Task WatchPointToPointAsync(
        CollectionInstrumentId instrument,
        PointName referencePoint,
        CollectionObjectName watchWindowProperties,
        string measurementMode = "",
        bool pauseMpUntilClosed = false,
        int windowTopLeftX = 0,
        int windowTopLeftY = 0,
        int windowWidth = 0,
        int windowHeight = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.WatchPointToPointRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["reference_point"] = referencePoint,
                ["watch_window_properties"] = watchWindowProperties,
                ["measurement_mode"] = measurementMode,
                ["pause_mp_until_closed"] = pauseMpUntilClosed,
                ["window_top_left_x"] = windowTopLeftX,
                ["window_top_left_y"] = windowTopLeftY,
                ["window_width"] = windowWidth,
                ["window_height"] = windowHeight,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "WatchPointToPoint",
            request,
            Transport.WatchPointToPointResult.Parser,
            cancellationToken);
    }

    public Task WatchPointToPointWithViewZoomingAsync(
        CollectionInstrumentId instrument,
        PointName referencePoint,
        bool update = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.WatchPointToPointWithViewZoomingRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["reference_point"] = referencePoint,
                ["update"] = update,
            });
        return _client.InvokeOperationAsync(
            "briosa.InstrumentOperations",
            "WatchPointToPointWithViewZooming",
            request,
            Transport.WatchPointToPointWithViewZoomingResult.Parser,
            cancellationToken);
    }
}

public sealed class BriosaRobotCalibrationApplianceNodeOperations
{
    private readonly BriosaClient _client;
    internal BriosaRobotCalibrationApplianceNodeOperations(BriosaClient client) => _client = client;

    public Task AddCalibrationApplianceNodeAsync(
        CollectionObjectName calibrationApplianceNodeToAdd,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddCalibrationApplianceNodeRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node_to_add"] = calibrationApplianceNodeToAdd,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "AddCalibrationApplianceNode",
            request,
            Transport.AddCalibrationApplianceNodeResult.Parser,
            cancellationToken);
    }

    public Task ClearCalibrationApplianceNodeTrapManagerRequestsAsync(
        CollectionObjectName calibrationApplianceNode,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ClearCalibrationApplianceNodeTrapManagerRequestsRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "ClearCalibrationApplianceNodeTrapManagerRequests",
            request,
            Transport.ClearCalibrationApplianceNodeTrapManagerRequestsResult.Parser,
            cancellationToken);
    }

    public Task ConnectDisconnectCalibrationApplianceNodeAsync(
        CollectionObjectName calibrationApplianceNode,
        bool connect = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ConnectDisconnectCalibrationApplianceNodeRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["connect"] = connect,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "ConnectDisconnectCalibrationApplianceNode",
            request,
            Transport.ConnectDisconnectCalibrationApplianceNodeResult.Parser,
            cancellationToken);
    }

    public Task DeleteCalibrationApplianceNodeAsync(
        CollectionObjectName calibrationApplianceNodeToDelete,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteCalibrationApplianceNodeRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node_to_delete"] = calibrationApplianceNodeToDelete,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "DeleteCalibrationApplianceNode",
            request,
            Transport.DeleteCalibrationApplianceNodeResult.Parser,
            cancellationToken);
    }

    public Task EnableDisableCalibrationApplianceNodeInstrumentAutoPointAsync(
        CollectionObjectName calibrationApplianceNode,
        bool enableInstrumentAutoPoint = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EnableDisableCalibrationApplianceNodeInstrumentAutoPointRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["enable_instrument_auto_point"] = enableInstrumentAutoPoint,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "EnableDisableCalibrationApplianceNodeInstrumentAutoPoint",
            request,
            Transport.EnableDisableCalibrationApplianceNodeInstrumentAutoPointResult.Parser,
            cancellationToken);
    }

    public Task EnableDisableCalibrationApplianceNodeTrapManagerAsync(
        CollectionObjectName calibrationApplianceNode,
        bool enable = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.EnableDisableCalibrationApplianceNodeTrapManagerRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["enable"] = enable,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "EnableDisableCalibrationApplianceNodeTrapManager",
            request,
            Transport.EnableDisableCalibrationApplianceNodeTrapManagerResult.Parser,
            cancellationToken);
    }

    public Task<double[]> GetCalibrationApplianceNodeDataAsync(
        CollectionObjectName calibrationApplianceNode,
        int realValueCount,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCalibrationApplianceNodeDataRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["real_value_count"] = realValueCount,
            });
        return _client.InvokeOperationAsync<double[]>(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "GetCalibrationApplianceNodeData",
            request,
            Transport.GetCalibrationApplianceNodeDataResult.Parser,
            cancellationToken);
    }

    public Task<int> GetCalibrationApplianceNodeIntegerValueAsync(
        CollectionObjectName calibrationApplianceNode,
        int indexOffset = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCalibrationApplianceNodeIntegerValueRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["index_offset"] = indexOffset,
            });
        return _client.InvokeOperationAsync<int>(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "GetCalibrationApplianceNodeIntegerValue",
            request,
            Transport.GetCalibrationApplianceNodeIntegerValueResult.Parser,
            cancellationToken);
    }

    public Task<double> GetCalibrationApplianceNodeRealValueAsync(
        CollectionObjectName calibrationApplianceNode,
        int indexOffset = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCalibrationApplianceNodeRealValueRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["index_offset"] = indexOffset,
            });
        return _client.InvokeOperationAsync<double>(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "GetCalibrationApplianceNodeRealValue",
            request,
            Transport.GetCalibrationApplianceNodeRealValueResult.Parser,
            cancellationToken);
    }

    public Task<CalibrationApplianceNodeStatus> GetCalibrationApplianceNodeStatusAsync(
        CollectionObjectName calibrationApplianceNode,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCalibrationApplianceNodeStatusRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
            });
        return _client.InvokeOperationAsync<CalibrationApplianceNodeStatus>(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "GetCalibrationApplianceNodeStatus",
            request,
            Transport.GetCalibrationApplianceNodeStatusResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeCalibrationApplianceIpAddressAsync(
        CollectionObjectName calibrationApplianceNode,
        string calibrationApplianceIpAddress = "0.0.0.0",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeCalibrationApplianceIpAddressRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["calibration_appliance_ip_address"] = calibrationApplianceIpAddress,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeCalibrationApplianceIpAddress",
            request,
            Transport.SetCalibrationApplianceNodeCalibrationApplianceIpAddressResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeDisplayRobotAsync(
        CollectionObjectName calibrationApplianceNode,
        CollectionMachineId machineId,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeDisplayRobotRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["machine_id"] = machineId,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeDisplayRobot",
            request,
            Transport.SetCalibrationApplianceNodeDisplayRobotResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeInstrumentAsync(
        CollectionObjectName calibrationApplianceNode,
        CollectionInstrumentId instrument,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeInstrumentRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["instrument"] = instrument,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeInstrument",
            request,
            Transport.SetCalibrationApplianceNodeInstrumentResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeInstrumentDwellTimeAsync(
        CollectionObjectName calibrationApplianceNode,
        double measurementDwellTimeSeconds = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeInstrumentDwellTimeRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["measurement_dwell_time_seconds"] = measurementDwellTimeSeconds,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeInstrumentDwellTime",
            request,
            Transport.SetCalibrationApplianceNodeInstrumentDwellTimeResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeIntegerValueAsync(
        CollectionObjectName calibrationApplianceNode,
        int indexOffset = 0,
        int integerValue = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeIntegerValueRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["index_offset"] = indexOffset,
                ["integer_value"] = integerValue,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeIntegerValue",
            request,
            Transport.SetCalibrationApplianceNodeIntegerValueResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeMeasurementFrameAsync(
        CollectionObjectName calibrationApplianceNode,
        CollectionObjectName measurementReferenceFrame,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeMeasurementFrameRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["measurement_reference_frame"] = measurementReferenceFrame,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeMeasurementFrame",
            request,
            Transport.SetCalibrationApplianceNodeMeasurementFrameResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeMeasurementOffsetTransformAsync(
        CollectionObjectName calibrationApplianceNode,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeMeasurementOffsetTransformRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["measurement_offset_transform"] = new Transform(),
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeMeasurementOffsetTransform",
            request,
            Transport.SetCalibrationApplianceNodeMeasurementOffsetTransformResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeMeasurementOffsetTransformAsync(
        CollectionObjectName calibrationApplianceNode,
        Transform measurementOffsetTransform,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeMeasurementOffsetTransformRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["measurement_offset_transform"] = measurementOffsetTransform,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeMeasurementOffsetTransform",
            request,
            Transport.SetCalibrationApplianceNodeMeasurementOffsetTransformResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeMeasurementPointGroupAsync(
        CollectionObjectName calibrationApplianceNode,
        CollectionObjectName pointGroupName,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeMeasurementPointGroupRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["point_group_name"] = pointGroupName,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeMeasurementPointGroup",
            request,
            Transport.SetCalibrationApplianceNodeMeasurementPointGroupResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeMeasurementProfileAsync(
        CollectionObjectName calibrationApplianceNode,
        string measurementProfile = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeMeasurementProfileRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["measurement_profile"] = measurementProfile,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeMeasurementProfile",
            request,
            Transport.SetCalibrationApplianceNodeMeasurementProfileResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeMeasurementTargetAsync(
        CollectionObjectName calibrationApplianceNode,
        string measurementTarget = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeMeasurementTargetRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["measurement_target"] = measurementTarget,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeMeasurementTarget",
            request,
            Transport.SetCalibrationApplianceNodeMeasurementTargetResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeRealValueAsync(
        CollectionObjectName calibrationApplianceNode,
        int indexOffset = 0,
        double realValue = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeRealValueRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["index_offset"] = indexOffset,
                ["real_value"] = realValue,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeRealValue",
            request,
            Transport.SetCalibrationApplianceNodeRealValueResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeTrappingNodeIdAsync(
        CollectionObjectName calibrationApplianceNode,
        int trappingNodeId = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeTrappingNodeIdRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["trapping_node_id"] = trappingNodeId,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeTrappingNodeId",
            request,
            Transport.SetCalibrationApplianceNodeTrappingNodeIdResult.Parser,
            cancellationToken);
    }

    public Task SkipCalibrationApplianceNodeMeasurementAsync(
        CollectionObjectName calibrationApplianceNode,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SkipCalibrationApplianceNodeMeasurementRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SkipCalibrationApplianceNodeMeasurement",
            request,
            Transport.SkipCalibrationApplianceNodeMeasurementResult.Parser,
            cancellationToken);
    }

    public Task UpdateCalibrationApplianceNodeDisplayRobotJointsAsync(
        CollectionObjectName calibrationApplianceNode,
        bool enableDisplayRobotJointUpdates = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.UpdateCalibrationApplianceNodeDisplayRobotJointsRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["enable_display_robot_joint_updates"] = enableDisplayRobotJointUpdates,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "UpdateCalibrationApplianceNodeDisplayRobotJoints",
            request,
            Transport.UpdateCalibrationApplianceNodeDisplayRobotJointsResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeDataAsync(
        CollectionObjectName calibrationApplianceNode,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeDataRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["real_values"] = Array.Empty<double>(),
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeData",
            request,
            Transport.SetCalibrationApplianceNodeDataResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceNodeDataAsync(
        CollectionObjectName calibrationApplianceNode,
        IEnumerable<double> realValues,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceNodeDataRequest(),
            new Dictionary<string, object?>
            {
                ["calibration_appliance_node"] = calibrationApplianceNode,
                ["real_values"] = realValues,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotCalibrationApplianceNodeOperations",
            "SetCalibrationApplianceNodeData",
            request,
            Transport.SetCalibrationApplianceNodeDataResult.Parser,
            cancellationToken);
    }
}

public sealed class BriosaRobotOperations
{
    private readonly BriosaClient _client;
    internal BriosaRobotOperations(BriosaClient client) => _client = client;

    public Task AddRobotMachineManipKinAsync(
        FileReference manipKinFile,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddRobotMachineManipKinRequest(),
            new Dictionary<string, object?>
            {
                ["manip_kin_file"] = manipKinFile,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "AddRobotMachineManipKin",
            request,
            Transport.AddRobotMachineManipKinResult.Parser,
            cancellationToken);
    }

    public Task AddRobotMachineSaMachineAsync(
        FileReference saMachineFile,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.AddRobotMachineSaMachineRequest(),
            new Dictionary<string, object?>
            {
                ["sa_machine_file"] = saMachineFile,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "AddRobotMachineSaMachine",
            request,
            Transport.AddRobotMachineSaMachineResult.Parser,
            cancellationToken);
    }

    public Task<Transform> ComputeRobotMachineAdjustedGoalFrameAsync(
        CollectionObjectName originalGoalFrame,
        CollectionObjectName lastAdjustedGoalFrame,
        CollectionObjectName actualMeasuredFrame,
        CollectionObjectName modifiedGoalFrame,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ComputeRobotMachineAdjustedGoalFrameRequest(),
            new Dictionary<string, object?>
            {
                ["original_goal_frame"] = originalGoalFrame,
                ["last_adjusted_goal_frame"] = lastAdjustedGoalFrame,
                ["actual_measured_frame"] = actualMeasuredFrame,
                ["modified_goal_frame"] = modifiedGoalFrame,
            });
        return _client.InvokeOperationAsync<Transform>(
            "briosa.RobotOperations",
            "ComputeRobotMachineAdjustedGoalFrame",
            request,
            Transport.ComputeRobotMachineAdjustedGoalFrameResult.Parser,
            cancellationToken);
    }

    public Task CreateRobotCalibrationAsync(
        CollectionMachineId machineId,
        string calibrationName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.CreateRobotCalibrationRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = calibrationName,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "CreateRobotCalibration",
            request,
            Transport.CreateRobotCalibrationResult.Parser,
            cancellationToken);
    }

    public Task DeleteRobotCalibrationAsync(
        CollectionMachineId machineId,
        string calibrationName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteRobotCalibrationRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = calibrationName,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "DeleteRobotCalibration",
            request,
            Transport.DeleteRobotCalibrationResult.Parser,
            cancellationToken);
    }

    public Task DeleteRobotMachineAsync(
        CollectionMachineId machineId,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.DeleteRobotMachineRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "DeleteRobotMachine",
            request,
            Transport.DeleteRobotMachineResult.Parser,
            cancellationToken);
    }

    public Task<double[]> GetCalibrationApplianceDataAsync(
        int realValueCount,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCalibrationApplianceDataRequest(),
            new Dictionary<string, object?>
            {
                ["real_value_count"] = realValueCount,
            });
        return _client.InvokeOperationAsync<double[]>(
            "briosa.RobotOperations",
            "GetCalibrationApplianceData",
            request,
            Transport.GetCalibrationApplianceDataResult.Parser,
            cancellationToken);
    }

    public Task<int> GetCalibrationApplianceIntegerValueAsync(
        int indexOffset = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCalibrationApplianceIntegerValueRequest(),
            new Dictionary<string, object?>
            {
                ["index_offset"] = indexOffset,
            });
        return _client.InvokeOperationAsync<int>(
            "briosa.RobotOperations",
            "GetCalibrationApplianceIntegerValue",
            request,
            Transport.GetCalibrationApplianceIntegerValueResult.Parser,
            cancellationToken);
    }

    public Task<double> GetCalibrationApplianceRealValueAsync(
        int indexOffset = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetCalibrationApplianceRealValueRequest(),
            new Dictionary<string, object?>
            {
                ["index_offset"] = indexOffset,
            });
        return _client.InvokeOperationAsync<double>(
            "briosa.RobotOperations",
            "GetCalibrationApplianceRealValue",
            request,
            Transport.GetCalibrationApplianceRealValueResult.Parser,
            cancellationToken);
    }

    public Task<RobotModelLinkParameters> GetRobotMachineModelLinkParametersAsync(
        CollectionMachineId machineId,
        string linkName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRobotMachineModelLinkParametersRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["link_name"] = linkName,
            });
        return _client.InvokeOperationAsync<RobotModelLinkParameters>(
            "briosa.RobotOperations",
            "GetRobotMachineModelLinkParameters",
            request,
            Transport.GetRobotMachineModelLinkParametersResult.Parser,
            cancellationToken);
    }

    public Task<double> GetRobotMachineParameterAsync(
        CollectionMachineId machineId,
        string parameterName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRobotMachineParameterRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["parameter_name"] = parameterName,
            });
        return _client.InvokeOperationAsync<double>(
            "briosa.RobotOperations",
            "GetRobotMachineParameter",
            request,
            Transport.GetRobotMachineParameterResult.Parser,
            cancellationToken);
    }

    public Task ImportPosesMatchToFramesAsync(
        CollectionMachineId machineId,
        IEnumerable<CollectionObjectName> frameNames,
        FileReference csvJointSetFile,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportPosesMatchToFramesRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = "",
                ["frame_names"] = frameNames,
                ["csv_joint_set_file"] = csvJointSetFile,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "ImportPosesMatchToFrames",
            request,
            Transport.ImportPosesMatchToFramesResult.Parser,
            cancellationToken);
    }

    public Task ImportPosesMatchToFramesAsync(
        CollectionMachineId machineId,
        string calibrationName,
        IEnumerable<CollectionObjectName> frameNames,
        FileReference csvJointSetFile,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportPosesMatchToFramesRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = calibrationName,
                ["frame_names"] = frameNames,
                ["csv_joint_set_file"] = csvJointSetFile,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "ImportPosesMatchToFrames",
            request,
            Transport.ImportPosesMatchToFramesResult.Parser,
            cancellationToken);
    }

    public Task ImportPosesMatchToMeasurementsAsync(
        CollectionMachineId machineId,
        IEnumerable<PointName> pointNames,
        FileReference csvJointSetFile,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportPosesMatchToMeasurementsRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = "",
                ["point_names"] = pointNames,
                ["csv_joint_set_file"] = csvJointSetFile,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "ImportPosesMatchToMeasurements",
            request,
            Transport.ImportPosesMatchToMeasurementsResult.Parser,
            cancellationToken);
    }

    public Task ImportPosesMatchToMeasurementsAsync(
        CollectionMachineId machineId,
        string calibrationName,
        IEnumerable<PointName> pointNames,
        FileReference csvJointSetFile,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ImportPosesMatchToMeasurementsRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = calibrationName,
                ["point_names"] = pointNames,
                ["csv_joint_set_file"] = csvJointSetFile,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "ImportPosesMatchToMeasurements",
            request,
            Transport.ImportPosesMatchToMeasurementsResult.Parser,
            cancellationToken);
    }

    public Task MoveRobotMachineThroughPathAsync(
        CollectionMachineId machineId,
        IEnumerable<CollectionObjectName> pathFrames,
        bool useSaKinematics = true,
        bool linearSegments = false,
        bool acknowledgeArrival = true,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveRobotMachineThroughPathRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["path_frames"] = pathFrames,
                ["use_sa_kinematics"] = useSaKinematics,
                ["linear_segments"] = linearSegments,
                ["acknowledge_arrival"] = acknowledgeArrival,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "MoveRobotMachineThroughPath",
            request,
            Transport.MoveRobotMachineThroughPathResult.Parser,
            cancellationToken);
    }

    public Task<Transform> MoveRobotMachineToFrameAsync(
        CollectionMachineId machineId,
        CollectionObjectName destinationFrame,
        bool useSaKinematics = false,
        bool acknowledgeArrival = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveRobotMachineToFrameRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["destination_frame"] = destinationFrame,
                ["use_sa_kinematics"] = useSaKinematics,
                ["acknowledge_arrival"] = acknowledgeArrival,
            });
        return _client.InvokeOperationAsync<Transform>(
            "briosa.RobotOperations",
            "MoveRobotMachineToFrame",
            request,
            Transport.MoveRobotMachineToFrameResult.Parser,
            cancellationToken);
    }

    public Task MoveRobotMachineToJointPoseSixDofAsync(
        CollectionMachineId machineId,
        double joint1 = 0.0,
        double joint2 = 0.0,
        double joint3 = 0.0,
        double joint4 = 0.0,
        double joint5 = 0.0,
        double joint6 = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveRobotMachineToJointPoseSixDofRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["joint_1"] = joint1,
                ["joint_2"] = joint2,
                ["joint_3"] = joint3,
                ["joint_4"] = joint4,
                ["joint_5"] = joint5,
                ["joint_6"] = joint6,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "MoveRobotMachineToJointPoseSixDof",
            request,
            Transport.MoveRobotMachineToJointPoseSixDofResult.Parser,
            cancellationToken);
    }

    public Task<Transform> MoveRobotMachineToNamedDestinationAsync(
        CollectionMachineId machineId,
        string destinationName = "",
        bool acknowledgeArrival = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.MoveRobotMachineToNamedDestinationRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["destination_name"] = destinationName,
                ["acknowledge_arrival"] = acknowledgeArrival,
            });
        return _client.InvokeOperationAsync<Transform>(
            "briosa.RobotOperations",
            "MoveRobotMachineToNamedDestination",
            request,
            Transport.MoveRobotMachineToNamedDestinationResult.Parser,
            cancellationToken);
    }

    public Task<RobotCalibrationMetrics> PerformRobotCalibrationAlternateAsync(
        CollectionMachineId machineId,
        string calibrationName = "",
        bool setCurrentBaseAsNominal = false,
        string baseDegreesOfFreedom = "",
        string robotDegreesOfFreedom = "",
        string toolDegreesOfFreedom = "",
        bool showInterface = false,
        int allowedOutlierRejectionCount = 0,
        double allowableMaximumError = 0.0,
        double allowableAverageError = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.PerformRobotCalibrationAlternateRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = calibrationName,
                ["set_current_base_as_nominal"] = setCurrentBaseAsNominal,
                ["base_degrees_of_freedom"] = baseDegreesOfFreedom,
                ["robot_degrees_of_freedom"] = robotDegreesOfFreedom,
                ["tool_degrees_of_freedom"] = toolDegreesOfFreedom,
                ["show_interface"] = showInterface,
                ["allowed_outlier_rejection_count"] = allowedOutlierRejectionCount,
                ["allowable_maximum_error"] = allowableMaximumError,
                ["allowable_average_error"] = allowableAverageError,
            });
        return _client.InvokeOperationAsync<RobotCalibrationMetrics>(
            "briosa.RobotOperations",
            "PerformRobotCalibrationAlternate",
            request,
            Transport.PerformRobotCalibrationAlternateResult.Parser,
            cancellationToken);
    }

    public Task SetActiveRobotCalibrationAsync(
        CollectionMachineId machineId,
        string calibrationName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetActiveRobotCalibrationRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = calibrationName,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetActiveRobotCalibration",
            request,
            Transport.SetActiveRobotCalibrationResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceIntegerValueAsync(
        int indexOffset = 0,
        int integerValue = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceIntegerValueRequest(),
            new Dictionary<string, object?>
            {
                ["index_offset"] = indexOffset,
                ["integer_value"] = integerValue,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetCalibrationApplianceIntegerValue",
            request,
            Transport.SetCalibrationApplianceIntegerValueResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceRealValueAsync(
        int indexOffset = 0,
        double realValue = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceRealValueRequest(),
            new Dictionary<string, object?>
            {
                ["index_offset"] = indexOffset,
                ["real_value"] = realValue,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetCalibrationApplianceRealValue",
            request,
            Transport.SetCalibrationApplianceRealValueResult.Parser,
            cancellationToken);
    }

    public Task SetRobotCalibrationMeasurementOffsetInToolFrameAsync(
        CollectionMachineId machineId,
        string calibrationName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRobotCalibrationMeasurementOffsetInToolFrameRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = calibrationName,
                ["measurement_frame_relative_to_tool"] = new Transform(),
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetRobotCalibrationMeasurementOffsetInToolFrame",
            request,
            Transport.SetRobotCalibrationMeasurementOffsetInToolFrameResult.Parser,
            cancellationToken);
    }

    public Task SetRobotCalibrationMeasurementOffsetInToolFrameAsync(
        CollectionMachineId machineId,
        string calibrationName,
        Transform measurementFrameRelativeToTool,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRobotCalibrationMeasurementOffsetInToolFrameRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = calibrationName,
                ["measurement_frame_relative_to_tool"] = measurementFrameRelativeToTool,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetRobotCalibrationMeasurementOffsetInToolFrame",
            request,
            Transport.SetRobotCalibrationMeasurementOffsetInToolFrameResult.Parser,
            cancellationToken);
    }

    public Task SetRobotCalibrationToolFrameAsync(
        CollectionMachineId machineId,
        string calibrationName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRobotCalibrationToolFrameRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = calibrationName,
                ["tool_frame_relative_to_flange"] = new Transform(),
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetRobotCalibrationToolFrame",
            request,
            Transport.SetRobotCalibrationToolFrameResult.Parser,
            cancellationToken);
    }

    public Task SetRobotCalibrationToolFrameAsync(
        CollectionMachineId machineId,
        string calibrationName,
        Transform toolFrameRelativeToFlange,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRobotCalibrationToolFrameRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = calibrationName,
                ["tool_frame_relative_to_flange"] = toolFrameRelativeToFlange,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetRobotCalibrationToolFrame",
            request,
            Transport.SetRobotCalibrationToolFrameResult.Parser,
            cancellationToken);
    }

    public Task SetRobotMachineBaseTransformAsync(
        CollectionMachineId machineId,
        CollectionObjectName referenceFrame,
        int numberOfSteps = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRobotMachineBaseTransformRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["destination_transform"] = new Transform(),
                ["reference_frame"] = referenceFrame,
                ["number_of_steps"] = numberOfSteps,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetRobotMachineBaseTransform",
            request,
            Transport.SetRobotMachineBaseTransformResult.Parser,
            cancellationToken);
    }

    public Task SetRobotMachineBaseTransformAsync(
        CollectionMachineId machineId,
        Transform destinationTransform,
        CollectionObjectName referenceFrame,
        int numberOfSteps = 0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRobotMachineBaseTransformRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["destination_transform"] = destinationTransform,
                ["reference_frame"] = referenceFrame,
                ["number_of_steps"] = numberOfSteps,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetRobotMachineBaseTransform",
            request,
            Transport.SetRobotMachineBaseTransformResult.Parser,
            cancellationToken);
    }

    public Task SetRobotMachineModelLinkParametersAsync(
        CollectionMachineId machineId,
        string linkName = "",
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRobotMachineModelLinkParametersRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["link_name"] = linkName,
                ["link_type"] = RobotModelLinkType.Dh,
                ["dh_alpha_component"] = 0.000000,
                ["dh_a_component"] = 0.000000,
                ["dh_d_component"] = 0.000000,
                ["dh_theta_component"] = 0.000000,
                ["dh_x_axis_deflection_factor"] = 0.000000,
                ["dh_y_axis_deflection_factor"] = 0.000000,
                ["dh_z_axis_deflection_factor"] = 0.000000,
                ["six_dof_x_component"] = 0.000000,
                ["six_dof_y_component"] = 0.000000,
                ["six_dof_z_component"] = 0.000000,
                ["six_dof_rx_component"] = 0.000000,
                ["six_dof_ry_component"] = 0.000000,
                ["six_dof_rz_component"] = 0.000000,
                ["active_joint_component"] = RobotActiveJointComponent.None,
                ["encoder_offset_value"] = 0.000000,
                ["minimum_encoder_limit"] = 0.000000,
                ["maximum_encoder_limit"] = 0.000000,
                ["encoder_sense_negative"] = false,
                ["include_additional_encoder"] = false,
                ["additional_encoder_index_offset"] = 0,
                ["additional_encoder_sense_negative"] = false,
                ["segment_origin_mass_kg"] = 0.000000,
                ["segment_cg_mass_kg"] = 0.000000,
                ["segment_cg_in_segment"] = new Vector(),
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetRobotMachineModelLinkParameters",
            request,
            Transport.SetRobotMachineModelLinkParametersResult.Parser,
            cancellationToken);
    }

    public Task SetRobotMachineModelLinkParametersAsync(
        CollectionMachineId machineId,
        string linkName,
        RobotModelLinkConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRobotMachineModelLinkParametersRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["link_name"] = linkName,
                ["link_type"] = configuration.LinkType,
                ["dh_alpha_component"] = configuration.DhAlphaComponent,
                ["dh_a_component"] = configuration.DhAComponent,
                ["dh_d_component"] = configuration.DhDComponent,
                ["dh_theta_component"] = configuration.DhThetaComponent,
                ["dh_x_axis_deflection_factor"] = configuration.DhXAxisDeflectionFactor,
                ["dh_y_axis_deflection_factor"] = configuration.DhYAxisDeflectionFactor,
                ["dh_z_axis_deflection_factor"] = configuration.DhZAxisDeflectionFactor,
                ["six_dof_x_component"] = configuration.SixDofXComponent,
                ["six_dof_y_component"] = configuration.SixDofYComponent,
                ["six_dof_z_component"] = configuration.SixDofZComponent,
                ["six_dof_rx_component"] = configuration.SixDofRxComponent,
                ["six_dof_ry_component"] = configuration.SixDofRyComponent,
                ["six_dof_rz_component"] = configuration.SixDofRzComponent,
                ["active_joint_component"] = configuration.ActiveJointComponent,
                ["encoder_offset_value"] = configuration.EncoderOffsetValue,
                ["minimum_encoder_limit"] = configuration.MinimumEncoderLimit,
                ["maximum_encoder_limit"] = configuration.MaximumEncoderLimit,
                ["encoder_sense_negative"] = configuration.EncoderSenseNegative,
                ["include_additional_encoder"] = configuration.IncludeAdditionalEncoder,
                ["additional_encoder_index_offset"] = configuration.AdditionalEncoderIndexOffset,
                ["additional_encoder_sense_negative"] = configuration.AdditionalEncoderSenseNegative,
                ["segment_origin_mass_kg"] = configuration.SegmentOriginMassKg,
                ["segment_cg_mass_kg"] = configuration.SegmentCgMassKg,
                ["segment_cg_in_segment"] = configuration.SegmentCgInSegment,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetRobotMachineModelLinkParameters",
            request,
            Transport.SetRobotMachineModelLinkParametersResult.Parser,
            cancellationToken);
    }

    public Task SetRobotMachineParameterAsync(
        CollectionMachineId machineId,
        string parameterName = "",
        double parameterValue = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetRobotMachineParameterRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["parameter_name"] = parameterName,
                ["parameter_value"] = parameterValue,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetRobotMachineParameter",
            request,
            Transport.SetRobotMachineParameterResult.Parser,
            cancellationToken);
    }

    public Task SimulateRobotMachinePathOutputCsvFileAsync(
        CollectionMachineId machineId,
        IEnumerable<CollectionObjectName> pathFrames,
        FileReference outputCsvFile,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SimulateRobotMachinePathOutputCsvFileRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["path_frames"] = pathFrames,
                ["output_csv_file"] = outputCsvFile,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SimulateRobotMachinePathOutputCsvFile",
            request,
            Transport.SimulateRobotMachinePathOutputCsvFileResult.Parser,
            cancellationToken);
    }

    public Task StartRobotMachineInterfaceAsync(
        CollectionMachineId machineId,
        int interfaceType = 0,
        bool runInSimulation = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StartRobotMachineInterfaceRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["interface_type"] = interfaceType,
                ["run_in_simulation"] = runInSimulation,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "StartRobotMachineInterface",
            request,
            Transport.StartRobotMachineInterfaceResult.Parser,
            cancellationToken);
    }

    public Task StartStopRobotCalibrationTrappingAsync(
        CollectionMachineId machineId,
        CollectionInstrumentId instrumentId,
        bool startTrapping = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StartStopRobotCalibrationTrappingRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = "",
                ["instrument_id"] = instrumentId,
                ["start_trapping"] = startTrapping,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "StartStopRobotCalibrationTrapping",
            request,
            Transport.StartStopRobotCalibrationTrappingResult.Parser,
            cancellationToken);
    }

    public Task StartStopRobotCalibrationTrappingAsync(
        CollectionMachineId machineId,
        string calibrationName,
        CollectionInstrumentId instrumentId,
        bool startTrapping = false,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StartStopRobotCalibrationTrappingRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = calibrationName,
                ["instrument_id"] = instrumentId,
                ["start_trapping"] = startTrapping,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "StartStopRobotCalibrationTrapping",
            request,
            Transport.StartStopRobotCalibrationTrappingResult.Parser,
            cancellationToken);
    }

    public Task StopRobotMachineInterfaceAsync(
        CollectionMachineId machineId,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StopRobotMachineInterfaceRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "StopRobotMachineInterface",
            request,
            Transport.StopRobotMachineInterfaceResult.Parser,
            cancellationToken);
    }

    public Task<double[]> GetRobotPoseForAFrameAsync(
        CollectionMachineId machineId,
        CollectionObjectName goalFrame,
        int goalPoseCount,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRobotPoseForAFrameRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["goal_frame"] = goalFrame,
                ["reference_pose"] = Array.Empty<double>(),
                ["goal_pose_count"] = goalPoseCount,
            });
        return _client.InvokeOperationAsync<double[]>(
            "briosa.RobotOperations",
            "GetRobotPoseForAFrame",
            request,
            Transport.GetRobotPoseForAFrameResult.Parser,
            cancellationToken);
    }

    public Task<double[]> GetRobotPoseForAFrameAsync(
        CollectionMachineId machineId,
        CollectionObjectName goalFrame,
        IEnumerable<double> referencePose,
        int goalPoseCount,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.GetRobotPoseForAFrameRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["goal_frame"] = goalFrame,
                ["reference_pose"] = referencePose,
                ["goal_pose_count"] = goalPoseCount,
            });
        return _client.InvokeOperationAsync<double[]>(
            "briosa.RobotOperations",
            "GetRobotPoseForAFrame",
            request,
            Transport.GetRobotPoseForAFrameResult.Parser,
            cancellationToken);
    }

    public Task<RobotCalibrationMetrics> PerformRobotCalibrationAsync(
        CollectionMachineId machineId,
        string calibrationName = "",
        bool setCurrentBaseAsNominal = false,
        bool showInterface = false,
        int allowedOutlierRejectionCount = 0,
        double allowableMaximumError = 0.0,
        double allowableAverageError = 0.0,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.PerformRobotCalibrationRequest(),
            new Dictionary<string, object?>
            {
                ["machine_id"] = machineId,
                ["calibration_name"] = calibrationName,
                ["set_current_base_as_nominal"] = setCurrentBaseAsNominal,
                ["show_interface"] = showInterface,
                ["allowed_outlier_rejection_count"] = allowedOutlierRejectionCount,
                ["allowable_maximum_error"] = allowableMaximumError,
                ["allowable_average_error"] = allowableAverageError,
            });
        return _client.InvokeOperationAsync<RobotCalibrationMetrics>(
            "briosa.RobotOperations",
            "PerformRobotCalibration",
            request,
            Transport.PerformRobotCalibrationResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceDataAsync(
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceDataRequest(),
            new Dictionary<string, object?>
            {
                ["real_values"] = Array.Empty<double>(),
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetCalibrationApplianceData",
            request,
            Transport.SetCalibrationApplianceDataResult.Parser,
            cancellationToken);
    }

    public Task SetCalibrationApplianceDataAsync(
        IEnumerable<double> realValues,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.SetCalibrationApplianceDataRequest(),
            new Dictionary<string, object?>
            {
                ["real_values"] = realValues,
            });
        return _client.InvokeOperationAsync(
            "briosa.RobotOperations",
            "SetCalibrationApplianceData",
            request,
            Transport.SetCalibrationApplianceDataResult.Parser,
            cancellationToken);
    }
}
