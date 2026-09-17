// Drafted mechanically from the approved Briosa documentation contract.
#pragma warning disable CA1008 // Zero is intentionally not a public MP choice.
#pragma warning disable CA1720
#pragma warning disable CA1805 // Explicit initializers preserve reviewed MP defaults.
#pragma warning disable CS1591
using Transport = Briosa.Client.Transport;

namespace Briosa;

public enum AxisIdentifier
{
    PositiveX = 1,
    NegativeX = 2,
    PositiveY = 3,
    NegativeY = 4,
    PositiveZ = 5,
    NegativeZ = 6,
}

public enum BSplinePointSortMode
{
    UseSelectionOrder = 1,
    ClosestNeighborsFromFirstSelection = 2,
    ClosestNeighborsInCurveDirection = 3,
}

public enum CircleLineMode
{
    Circle = 1,
    Line = 2,
}

public enum CloudBoxType
{
    WorldAxisAlignedBox = 1,
    WorkAxisAlignedBox = 2,
    MinimumOrientedBoxUnconditional = 3,
    MinimumOrientedBoxVerifyVolume = 4,
}

public enum CloudThinningMode
{
    None = 1,
    Random = 2,
    NthPoint = 3,
}

public enum CollimationBaselineMethod
{
    DeterminedByValue = 1,
    DeterminedFromScale = 2,
    DeterminedFromKnownPoint = 3,
}

public enum CollimationTiltMode
{
    FullCollimation = 1,
    NoTiltCollimation = 2,
}

public enum ConstructObjectType
{
    Any = 1,
    Circles = 2,
    Cones = 3,
    Cylinders = 4,
    Lines = 5,
    Planes = 6,
    Slots = 7,
    Spheres = 8,
    CenterPoints = 9,
    SurfacePoints = 10,
    VertexPoints = 11,
}

public enum DynamicCircleMode
{
    CylinderAndPlaneHoldPlaneNormal = 1,
    CylinderAndPlaneHoldCylinderAxis = 2,
    ConeAndPlaneHoldPlaneNormal = 3,
    ConeAndPlaneHoldConeAxis = 4,
    SphereAndPlaneIntersection = 5,
    TwoConesIntersection = 6,
    ConeAndCylinderIntersection = 7,
}

public enum DynamicEllipseMode
{
    CylinderAndPlaneIntersection = 1,
    ConeAndPlaneIntersection = 2,
}

public enum DynamicLineMode
{
    ConeAxis = 1,
    CylinderAxis = 2,
    IntersectionOfTwoPlanes = 3,
    BisectTwoLines = 4,
    SlotCenterlineAlongLength = 5,
}

public enum DynamicPlaneMode
{
    BisectTwoPlanes = 1,
    TwoConesHoldNormalToBestFitPlane = 2,
    TwoConesHoldNormalToFirstConeAxis = 3,
    TwoConesHoldNormalToSecondConeAxis = 4,
    ConeAndCylinderHoldNormalToBestFitPlane = 5,
    ConeAndCylinderHoldNormalToConeAxis = 6,
    ConeAndCylinderHoldNormalToCylinderAxis = 7,
    OffsetPlaneFromPlane = 8,
}

public enum DynamicPointMode
{
    IntersectionLineAndPlane = 1,
    IntersectionCylinderAndPlane = 2,
    IntersectionConeAndPlane = 3,
    IntersectionThreePlanes = 4,
    MidPointPerpendicularToTwoLines = 5,
}

public enum EdgePointMode
{
    IncludeEdges = 1,
    ExcludeEdges = 2,
    EdgesOnly = 3,
}

public enum FrameAxis
{
    X = 1,
    Y = 2,
    Z = 3,
}

public enum FrameConstructionMethod
{
    OriginXXy = 1,
    OriginXXz = 2,
    OriginYYx = 3,
    OriginYYz = 4,
    OriginZZx = 5,
    OriginXZy = 6,
}

public enum GdtDistanceBetweenMode
{
    Centroid = 1,
    MinMax = 2,
}

public enum GdtEvaluationMethod
{
    None = 1,
    Asme1994 = 2,
    Asme2009 = 3,
    Asme2018 = 4,
    Iso1983 = 5,
    Iso2004 = 6,
    Iso2017 = 7,
}

public enum GdtExtendedEvaluationMethod
{
    LeastSquares = 1,
    HighPoint = 2,
    MinimumSeparation = 3,
    LeastSquaresHighPoint = 4,
    LeastSquares3D = 5,
    LeastSquaresHighPoint1StdDev = 6,
    LeastSquaresHighPoint2StdDev = 7,
    LeastSquaresHighPointHalfway = 8,
    MinimumSeparationHighPoint = 9,
    EqualizedHighPoint = 10,
    EqualizedLsqHighPoint = 11,
}

public enum GdtFeatureType
{
    Diameter = 1,
    Radius = 2,
    DistanceBetween = 3,
    Width = 4,
    Length = 5,
    AngleBetween = 6,
    Angularity = 7,
    Perpendicularity = 8,
    Parallelism = 9,
    Circularity = 10,
    Concentricity = 11,
    Cylindricity = 12,
    Straightness = 13,
    SurfaceProfile = 14,
    LineProfile = 15,
    CompositeSurfaceProfile = 16,
    Flatness = 17,
    TruePosition = 18,
    CompositeTruePosition = 19,
    CircularRunout = 20,
    TotalRunout = 21,
}

public enum GdtToleranceZoneType
{
    None = 1,
    Cylindrical = 2,
    Planar = 3,
    Spherical = 4,
    RadialArc = 5,
    RadialPlanar = 6,
    Boundary = 7,
    PlanarMedian = 8,
    Surface = 9,
}

public enum GeometryRelationshipPointEditMode
{
    PointList = 1,
    PointGraph = 2,
    SubSamplerSettings = 3,
}

public enum InspectionFilter
{
    All = 1,
    Checks = 2,
    Datums = 3,
}

public enum InstrumentPositionReportingFrame
{
    InstrumentBase = 1,
    World = 2,
    Working = 3,
}

public enum MeshOrientationType
{
    UseCurrentPointOfView = 1,
    UseCurrentWorkingFrame = 2,
}

public enum MirrorFramePlane
{
    XY = 1,
    XZ = 2,
    YZ = 3,
}

public enum OffsetDirectionType
{
    Both = 1,
    PositiveOnly = 2,
    NegativeOnly = 3,
}

public enum PointOutputType
{
    Points = 1,
    CloudPoints = 2,
}

public enum RGBColorChannel
{
    Red = 1,
    Green = 2,
    Blue = 3,
    Intensity = 4,
}

public enum RGBFilterOperation
{
    IncrementallyApplyFilter = 1,
    ResetAndApplyFilter = 2,
    ResetAllCloudPointsVisible = 3,
}

public enum RobotActiveJointComponent
{
    None = 1,
    X = 2,
    Y = 3,
    Z = 4,
    Rx = 5,
    Ry = 6,
    Rz = 7,
    Alpha = 8,
    A = 9,
    D = 10,
    Theta = 11,
}

public enum RobotModelLinkType
{
    Dh = 1,
    SixDof = 2,
}

public enum ShowUsmnDialog
{
    No = 1,
    Yes = 2,
    OnToleranceViolation = 3,
}

public enum SolverMode
{
    GaussNewton = 1,
    LevenbergMarquardt = 2,
    GaussNewtonWithGradientSearch = 3,
    DirectSearch = 4,
}

public enum SurfaceDissectionMode
{
    EntireSolid = 1,
    SelectFaces = 2,
}

public enum SurveyTargetType
{
    Triangle = 1,
    Circle = 2,
}

public enum SystemString
{
    SaVersion = 1,
    XitFilename = 2,
    MpFilename = 3,
    MpFilenameFullPath = 4,
    DateAndTime = 5,
    Date = 6,
    DateShort = 7,
    Time = 8,
    KeySerialNumber = 9,
    CompanyName = 10,
    UserName = 11,
}

public enum TargetComputationMethod
{
    UseMostRecentShotFromEachFace = 1,
    UseOnlyMostRecentShot = 2,
    DoNotChangePriorMeasurements = 3,
    ForceNewPointForEachMeasurement = 4,
    RemoveAllPriorShots = 5,
    DeactivateAllPriorShots = 6,
}

public enum WcfAxis
{
    X = 1,
    Y = 2,
    Z = 3,
}

public sealed record BSplineFitOptions
{
    public bool OpenCurve { get; init; } = true;
    public bool UseInterpolationForFit { get; init; } = true;
    public int NumberOfControlPoints { get; init; } = 8;
    public int DegreeOfCurve { get; init; } = 3;
    public BSplinePointSortMode SortMethod { get; init; } = BSplinePointSortMode.UseSelectionOrder;
    public bool SpanAnyGap { get; init; } = true;
    public double TerminationGapLength { get; init; } = 0.0;
    public bool IgnoreProximatePoints { get; init; } = false;
    public double ProximatePointThreshold { get; init; } = 0.0;
    public bool UseGlobalTessellationOptions { get; init; } = true;
    public double MaximumChordalDeviation { get; init; } = 0.05;
    public double MaximumTrimEdgeAngle { get; init; } = 15.0;
    public double TerminationAverageMultiplier { get; init; } = 10.0;
    public double Extension { get; init; } = 0.0;
}

public sealed record CalloutViewProperties
{
    public bool LockViewPoint { get; init; }
    public bool RecallWorkingFrame { get; init; }
    public bool RecallVisibleLayer { get; init; }
    public int CalloutLeaderThickness { get; init; } = 2;
    public Color CalloutLeaderColor { get; init; } = new(128, 128, 128);
    public int CalloutBorderThickness { get; init; } = 2;
    public Color CalloutBorderColor { get; init; } = new(0, 0, 255);
    public bool DivideTextWithLines { get; init; }
    public Font Font { get; init; } = new();
}

public sealed record CloudThinningOptions
{
    public CloudThinningMode Mode { get; init; } = CloudThinningMode.NthPoint;
    public int PointIncrement { get; init; } = 5;
    public int MinimumNumberOfPoints { get; init; } = 100;
    public int MaximumNumberOfPoints { get; init; } = 20000;
}

public sealed record CloudToCadAlignmentResult
{
    public required double RmsDeviation { get; init; }
    public required double AverageDeviation { get; init; }
    public required double MaximumAbsoluteDeviation { get; init; }
    public required Transform ResultantTransformInWorking { get; init; }
}

public sealed record CollectionMachineId
{
    public required string CollectionName { get; init; }
    public int MachineId { get; init; }
}

public sealed record CurrentTrappingStatus
{
    public required bool Active { get; init; }
    public CollectionItemName? FocusedItem { get; init; }
    public CollectionInstrumentId? Instrument { get; init; }
}

public sealed record DoubleVector6
{
    public required IReadOnlyList<double> Values { get; init; }
}

public sealed record FeatureCheckCylinderEvalOptions
{
    public required bool EnableActualDiameterOverride { get; init; }
    public required double ActualDiameterOverride { get; init; }
}

public sealed record FeatureCheckDatumReference
{
    public required string ReferenceString { get; init; }
    public required string CadFaces { get; init; }
    public required IReadOnlyList<CollectionObjectName> SaObjects { get; init; }
    public required IReadOnlyList<CollectionObjectName> AuxiliarySaObjects { get; init; }
    public required IReadOnlyList<CollectionItemName> GeometryRelationships { get; init; }
    public required IReadOnlyList<CollectionItemName> AuxiliaryGeometryRelationships { get; init; }
}

public sealed record FeatureCheckReportingOptions
{
    public required bool ShowFeatureControlFrameSummary { get; init; }
    public required bool IncludeTitle { get; init; }
    public required bool ShowDatumAndToleranceSummary { get; init; }
    public required bool ShowFeatureSummary { get; init; }
    public required bool ShowPointDetails { get; init; }
    public required bool ShowLowerTierTables { get; init; }
}

public sealed record FilterProximitySettings
{
    public double SurfaceInclusionProximity { get; init; } = 0.1;
    public double EdgeExclusionProximity { get; init; } = 0.1;
    public double PlanarInclusionProximity { get; init; } = 0.5;
    public double PlanarExclusionProximity { get; init; } = 0.1;
    public double RadialInclusionProximity { get; init; } = 0.1;
    public double GeometryExtractionTolerance { get; init; } = 0.01;
    public OffsetDirectionType SurfaceProximityMode { get; init; } = OffsetDirectionType.Both;
    public OffsetDirectionType PlanarProximityMode { get; init; } = OffsetDirectionType.Both;
    public OffsetDirectionType RadialProximityMode { get; init; } = OffsetDirectionType.Both;
    public bool ProjectToPlane { get; init; } = true;
    public bool AssertPlaneBoundaries { get; init; } = false;
}

public sealed record FitDofOptions
{
    public bool AllowX { get; init; } = true;
    public bool AllowY { get; init; } = true;
    public bool AllowZ { get; init; } = true;
    public bool AllowRx { get; init; } = true;
    public bool AllowRy { get; init; } = true;
    public bool AllowRz { get; init; } = true;
    public bool RotateAboutCentroid { get; init; } = true;
}

public sealed record GdtMeasurements
{
    public required IReadOnlyList<PointName> PointNames { get; init; }
    public required IReadOnlyList<CollectionObjectName> CloudNames { get; init; }
}

public sealed record GdtOptions
{
    public required bool UseHighPoints { get; init; }
    public required bool ExtrapolateAxialExtent { get; init; }
    public required bool ExcludeFromAutoEvaluation { get; init; }
    public GdtDistanceBetweenMode? DistanceBetweenMode { get; init; }
    public GdtEvaluationMethod? EvaluationMethod { get; init; }
    public required bool CreateActualFeatures { get; init; }
    public required bool CreateSolvedPoints { get; init; }
    public required double CrossSectionCriteria { get; init; }
    public required bool EnableAutoFeatureDetection { get; init; }
}

public sealed record GeometryRelationshipOutlierFilterMetrics
{
    public required double FirstPassRmsError { get; init; }
    public required double FirstPassMaximumError { get; init; }
    public required double FirstPassMinimumError { get; init; }
    public required double FirstPassAverageError { get; init; }
    public required double FinalPassRmsError { get; init; }
    public required double FinalPassMaximumError { get; init; }
    public required double FinalPassMinimumError { get; init; }
    public required double FinalPassAverageError { get; init; }
    public required int TotalInputPointCount { get; init; }
    public required int ExcludePointCount { get; init; }
}

public sealed record GroupAverageResult
{
    public required double RmsDeviation { get; init; }
    public required double MaxAbsoluteDeviation { get; init; }
    public required double AverageDeviation { get; init; }
}

public sealed record InstrumentTargetStatus
{
    public required bool IsLocked { get; init; }
    public required string Name { get; init; }
    public required int NumberOfFaces { get; init; }
    public required int LockedFace { get; init; }
}

public sealed record InstrumentTypeName
{
    public required string Value { get; init; }
}

public sealed record LrFlipTestResult
{
    public required double FrontRangeInches { get; init; }
    public required double FrontAzimuthDegrees { get; init; }
    public required double FrontElevationDegrees { get; init; }
    public required double FrontQuality { get; init; }
    public required double BackRangeInches { get; init; }
    public required double BackAzimuthDegrees { get; init; }
    public required double BackElevationDegrees { get; init; }
    public required double BackQuality { get; init; }
    public required double FrontBackDifferenceRangeInches { get; init; }
    public required double FrontBackDifferenceAzimuthDegrees { get; init; }
    public required double FrontBackDifferenceElevationDegrees { get; init; }
}

public sealed record LrLoSeparationTestResult
{
    public required int PrimaryLoIndex { get; init; }
    public required int SecondaryLoIndex { get; init; }
    public required int PrimaryLoMeasurementCount { get; init; }
    public required double PrimaryLoRangeMeanInches { get; init; }
    public required double PrimaryLoRangeStandardDeviationInches { get; init; }
    public required double PrimaryLoQualityMean { get; init; }
    public required double PrimaryLoQualityStandardDeviation { get; init; }
    public required int SecondaryLoMeasurementCount { get; init; }
    public required double SecondaryLoRangeMeanInches { get; init; }
    public required double SecondaryLoRangeStandardDeviationInches { get; init; }
    public required double SecondaryLoQualityMean { get; init; }
    public required double SecondaryLoQualityStandardDeviation { get; init; }
}

public sealed record LrSnrInfo
{
    public required double Snr { get; init; }
    public required int SizeOfDataArray { get; init; }
    public required int PeakValueIndex { get; init; }
    public required double PeakValueDb { get; init; }
    public required double MeasuredRangeMeters { get; init; }
}

public sealed record ObjectOriginResult
{
    public required Vector VectorRepresentation { get; init; }
    public required double XValue { get; init; }
    public required double YValue { get; init; }
    public required double ZValue { get; init; }
}

public sealed record ObservationInfo
{
    public required CollectionInstrumentId Instrument { get; init; }
    public required ObservationSphericalValues SphericalValues { get; init; }
    public required bool Active { get; init; }
    public required string Timestamp { get; init; }
    public required double RmsError { get; init; }
    public required double TemperatureFahrenheit { get; init; }
    public required double PressureInHg { get; init; }
    public required double RelativeHumidityPercent { get; init; }
    public required string InfoData { get; init; }
}

public sealed record ObservationSphericalValues
{
    public required double Distance { get; init; }
    public required double Azimuth { get; init; }
    public required double Elevation { get; init; }
}

public sealed record PerimeterLists
{
    public required IReadOnlyList<CollectionObjectName> ScanPerimeters { get; init; }
    public required IReadOnlyList<CollectionObjectName> ExclusionPerimeters { get; init; }
}

public sealed record PointsToPointsRelationshipAssociatedData
{
    public required IReadOnlyList<PointName> NominalPoints { get; init; }
    public required IReadOnlyList<PointName> ActualPoints { get; init; }
}

public sealed record ProjectedPointGradient
{
    public required Vector ProjectedPoint { get; init; }
    public required Vector NormalVector { get; init; }
    public required Vector UDirection { get; init; }
    public required Vector VDirection { get; init; }
}

public sealed record RelationshipAssociatedData
{
    public required string RelationshipType { get; init; }
    public required IReadOnlyList<PointName> IndividualPoints { get; init; }
    public required IReadOnlyList<CollectionObjectName> PointGroups { get; init; }
    public required IReadOnlyList<CollectionObjectName> PointClouds { get; init; }
    public required IReadOnlyList<CollectionObjectName> Objects { get; init; }
}

public sealed record RelationshipStatusFlags
{
    public required bool Dormant { get; init; }
    public required bool Success { get; init; }
    public required bool Measured { get; init; }
    public required bool Failed { get; init; }
    public required bool Unmeasured { get; init; }
}

public sealed record RelationshipWatchWindowUdpSettings
{
    public bool Enabled { get; init; }
    public bool Broadcast { get; init; } = true;
    public string IpAddress { get; init; } = "";
    public int Port { get; init; } = 10000;
}

public sealed record RobotCalibrationMetrics
{
    public required double XyzMax { get; init; }
    public required double XyzAverage { get; init; }
    public required double XyzRms { get; init; }
    public required double OrientMax { get; init; }
    public required double OrientAverage { get; init; }
    public required double OrientRms { get; init; }
    public required double Robustness { get; init; }
}

public sealed record SigmoidalGapFitConstraints
{
    public required bool UseSigmoidalGapConstraints { get; init; }
    public required double MinimumGapBoundary { get; init; }
    public required double MinimumGapWeight { get; init; }
    public required double MaximumGapBoundary { get; init; }
    public required double MaximumGapWeight { get; init; }
    public required double NominalGap { get; init; }
    public required double NominalGapWeight { get; init; }
    public required double GradientSteepnessFactor { get; init; }
}

public sealed record SurfaceFaceList
{
    public required string Value { get; init; }
}

public sealed record TcpFixtureUncertainties
{
    public required bool SolutionValid { get; init; }
    public required Transform RefinedTcpInWorking { get; init; }
    public required DoubleVector6 UncertaintiesInTcpFixtureFrame { get; init; }
    public required DoubleVector6 UncertaintiesInWorkingFrame { get; init; }
    public required double RmsError { get; init; }
    public required double MaximumAbsoluteError { get; init; }
    public required double GoodnessOfFit { get; init; }
    public required double Robustness { get; init; }
    public required IReadOnlyList<string> ResultNotes { get; init; }
}

public sealed record UncertaintyCovarianceMatrix
{
    public required DoubleVector6 Row1 { get; init; }
    public required DoubleVector6 Row2 { get; init; }
    public required DoubleVector6 Row3 { get; init; }
    public required DoubleVector6 Row4 { get; init; }
    public required DoubleVector6 Row5 { get; init; }
    public required DoubleVector6 Row6 { get; init; }
}

public sealed record WrtlChannelStatus
{
    public required bool ConnectionStatus { get; init; }
    public required int ActiveChannel { get; init; }
}

public sealed record MakeGdtDatumAnnotationOptions
{
    public required string DatumName { get; init; }
    public IReadOnlyList<CollectionObjectName> Objects { get; init; } = [];
    public IReadOnlyList<CollectionItemName> GeometryRelationships { get; init; } = [];
    public SurfaceFaceList? SurfaceFaces { get; init; }
    public CollectionObjectName? AuxiliaryObject { get; init; }
    public CollectionItemName? AuxiliaryGeometryRelationship { get; init; }
    public bool IsSlot { get; init; }
    public bool ForceSurfaceFeature { get; init; }
}

public sealed record MakeGdtFeatureCheckAnnotationOptions
{
    public required string FeatureAnnotationName { get; init; }
    public GdtFeatureType FeatureType { get; init; } = GdtFeatureType.TruePosition;
    public IReadOnlyList<CollectionObjectName> Objects { get; init; } = [];
    public IReadOnlyList<CollectionItemName> GeometryRelationships { get; init; } = [];
    public SurfaceFaceList? SurfaceFaces { get; init; }
    public bool DecomposeMultipleFeatures { get; init; }
    public bool AutoCreateDiameterChecks { get; init; }
    public bool AutoCreateSlotWidthChecks { get; init; }
    public bool AutoCreateSlotLengthChecks { get; init; }
    public string DatumReferences { get; init; } = "";
    public string Tolerance { get; init; } = "";
    public bool IsSlot { get; init; }
    public bool PerUnitLengthOrArea { get; init; }
    public bool CircularArea { get; init; }
    public double PerUnitAreaLengthDistance { get; init; }
    public double PerUnitAreaLengthStepOverPercent { get; init; } = 50.0;
    public double PerUnitAreaWidthDistance { get; init; }
    public double PerUnitAreaWidthStepOverPercent { get; init; } = 50.0;
    public double PerUnitAreaCircleDiameter { get; init; }
    public double PerUnitAreaDiameterStepOver { get; init; } = 50.0;
    public CollectionObjectName? AuxiliaryObject { get; init; }
    public CollectionItemName? AuxiliaryGeometryRelationship { get; init; }
    public bool UseNominalForDimensionTolerance { get; init; } = true;
    public bool UseReferenceObjectForNominal { get; init; } = true;
    public double NominalDimensionTolerance { get; init; }
    public double LowDimensionTolerance { get; init; } = -0.1;
    public double HighDimensionTolerance { get; init; } = 0.1;
    public GdtToleranceZoneType ToleranceZoneType { get; init; } = GdtToleranceZoneType.None;
    public bool UseProjectedToleranceZone { get; init; }
    public double ProjectedToleranceZone { get; init; }
}

public sealed record RelationshipWatchWindowTemplateOptions
{
    public int LinearPrecision { get; init; } = 4;
    public int AngularPrecision { get; init; } = 3;
    public Font Font { get; init; } = new();
    public Color TextColor { get; init; } = new(0, 0, 255);
    public Color BackgroundColor { get; init; } = new(255, 255, 255);
    public Color HighlightColor { get; init; } = new(255, 0, 0);
    public bool ShowDeviationXRx { get; init; } = true;
    public bool ShowDeviationYRy { get; init; } = true;
    public bool ShowDeviationZRz { get; init; } = true;
    public bool ShowDeviationMagnitude { get; init; } = true;
    public RelationshipWatchWindowUdpSettings UdpNetworkTransmitSettings { get; init; } = new();
    public bool TransparentBackground { get; init; }
    public bool HideUnits { get; init; }
}

public sealed record RobotModelLinkConfiguration
{
    public RobotModelLinkType LinkType { get; init; } = RobotModelLinkType.Dh;
    public double DhAlphaComponent { get; init; }
    public double DhAComponent { get; init; }
    public double DhDComponent { get; init; }
    public double DhThetaComponent { get; init; }
    public double DhXAxisDeflectionFactor { get; init; }
    public double DhYAxisDeflectionFactor { get; init; }
    public double DhZAxisDeflectionFactor { get; init; }
    public double SixDofXComponent { get; init; }
    public double SixDofYComponent { get; init; }
    public double SixDofZComponent { get; init; }
    public double SixDofRxComponent { get; init; }
    public double SixDofRyComponent { get; init; }
    public double SixDofRzComponent { get; init; }
    public RobotActiveJointComponent ActiveJointComponent { get; init; }
        = RobotActiveJointComponent.None;
    public double EncoderOffsetValue { get; init; }
    public double MinimumEncoderLimit { get; init; }
    public double MaximumEncoderLimit { get; init; }
    public bool EncoderSenseNegative { get; init; }
    public bool IncludeAdditionalEncoder { get; init; }
    public int AdditionalEncoderIndexOffset { get; init; }
    public bool AdditionalEncoderSenseNegative { get; init; }
    public double SegmentOriginMassKg { get; init; }
    public double SegmentCgMassKg { get; init; }
    public Vector SegmentCgInSegment { get; init; } = new(0.0, 0.0, 0.0);
}
