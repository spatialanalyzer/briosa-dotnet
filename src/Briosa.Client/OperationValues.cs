#pragma warning disable CA1008 // Zero is intentionally not a valid public MP choice.
#pragma warning disable CA1819 // MP list values are deliberately detached arrays.
#pragma warning disable CS1591 // Full API reference is maintained in briosa-docs.

namespace Briosa;

// These handwritten public values keep generated protobuf messages private.

public enum AngularUnits
{
    Degrees = 1,
    DegreesMinutesSeconds = 2,
    Radians = 3,
    Milliradians = 4,
    GonsGrad = 5,
    Mils = 6,
    Arcseconds = 7,
    DegreesMinutes = 8,
}

public enum AsciiFileFormat
{
    XYZ = 1,
    XYZOffsetOffset2 = 2,
    XYZNotes = 3,
    RadiusThetaPhi = 4,
    RadiusThetaZ = 5,
    PointNameXYZ = 6,
    PointNameXYZNotes = 7,
    PointNameXYZOffsetOffset2 = 8,
    PointNameXYZUxUyUz = 9,
    PointNameXYZTxTyTzTd = 10,
    PointNameXYZWxWyWzWmag = 11,
    PointNameXYZHighLowTolerance = 12,
    PointNameXYZTxTyTzTdWxWyWz = 13,
    PointNameXYZWxWyWzTxTyTzTd = 14,
    PointNameXYZHighLowToleranceWxWyWz = 15,
    PointNameXYZWxWyWzHighLowTolerance = 16,
    PointNameRadiusThetaPhi = 17,
    PointNameRadiusThetaZ = 18,
    PointNameXYZGroupName = 19,
    PointNameYXZGroupName = 20,
    GroupNamePointNameXYZ = 21,
    GroupNamePointNameXYZOffsetOffset2 = 22,
    GroupNamePointNameXYZNotes = 23,
    GroupNamePointNameXYZUxUyUz = 24,
    GroupNamePointNameRadiusThetaPhi = 25,
    GroupNamePointNameRadiusThetaZ = 26,
    CollectionGroupPointXYZ = 27,
    CollectionGroupPointXYZNotes = 28,
    CollectionGroupPointRadiusThetaPhi = 29,
    CollectionGroupPointRadiusThetaZ = 30,
    XYZIjk = 31,
    VectorNameXYZIjk = 32,
    VectorNameXYZDxDyDzSignedMagnitude = 33,
    VectorGroupNameVectorNameXYZIjk = 34,
    VectorGroupNameVectorNameXYZDxDyDzSignedMagnitude = 35,
    FrameNameXYZRxRyRzTimestamp = 36,
    FrameNameXYZEulerXYZTimestamp = 37,
    FrameNameXYZEulerZYXTimestamp = 38,
    FrameNameXYZEulerZYZTimestamp = 39,
    FrameNameXYZEulerZXZTimestamp = 40,
    FrameNameTransformationMatrixTimestamp = 41,
    TransformationMatrixTimestamp = 42,
    FrameNameXYZQuaternionTimestamp = 43,
    PlaneNameXYZDxDyDzPlaneSize = 44,
}

public enum ChartType { RunChart = 1, IndividualXMovingRange = 2, BullseyeChart = 3 }
public enum CoordinateSystemType { Cartesian = 1, Cylindric = 2, Polar = 3 }
public enum DatasetType { X = 1, Y = 2, Z = 3, Magnitude = 4 }
public enum DistanceUnits { Meters = 1, Centimeters = 2, Millimeters = 3, Feet = 4, Inches = 5, UsSurveyFeet = 6 }
public enum ExportDataDelimeterType { Space = 1, Comma = 2, Tab = 3 }
public enum ExportTargetNameFormat { CollectionGroupTarget = 1, GroupTarget = 2, Target = 3, None = 4 }
public enum ExportVectorNameFormat { CollectionGroupVector = 1, GroupVector = 2, Vector = 3, None = 4 }
public enum BaseColorType { Red = 1, Green = 2, Blue = 3 }
public enum BaseMidColorType { Red = 1, Green = 2, Gray = 3, Blue = 4 }

public enum ColorRangeMethod
{
    SingleColor = 1,
    Continuous = 2,
    TolerancedContinuous = 3,
    TolerancedGoNoGo = 4,
    TolerancedGoNoGoWithWarning = 5,
    DiscreteColors = 6,
}

public enum GeometryType { Line = 1, Plane = 2, Circle = 3, Sphere = 4, Cylinder = 5, Cone = 6, Paraboloid = 7, Ellipse = 8, Slot = 9, Torus = 10 }

public enum ObjectType
{
    Any = 1,
    BSpline = 2,
    Circle = 3,
    Cloud = 4,
    EnhancedCloud = 5,
    ScanStripeCloud = 6,
    CrossSectionCloud = 7,
    Cone = 8,
    Cylinder = 9,
    Datum = 10,
    Ellipse = 11,
    Frame = 12,
    FrameSet = 13,
    Line = 14,
    Paraboloid = 15,
    Perimeter = 16,
    Plane = 17,
    PointGroup = 18,
    PointSet = 19,
    PolySurface = 20,
    ScanStripeMesh = 21,
    Slot = 22,
    Sphere = 23,
    Surface = 24,
    Torus = 25,
    VectorGroup = 26,
}

public enum ItemType
{
    Any = 1,
    Alignment = 2,
    Annotation = 3,
    BSpline = 4,
    CalibrationApplianceNode = 5,
    CalloutView = 6,
    Chart = 7,
    Circle = 8,
    Cloud = 9,
    EnhancedCloud = 10,
    ScanStripeCloud = 11,
    CrossSectionCloud = 12,
    Cone = 13,
    Cylinder = 14,
    Datum = 15,
    Dimension = 16,
    Ellipse = 17,
    Event = 18,
    FeatureCheck = 19,
    Frame = 20,
    FrameSet = 21,
    Line = 22,
    Paraboloid = 23,
    Perimeter = 24,
    Picture = 25,
    Plane = 26,
    PointGroup = 27,
    PointSet = 28,
    PolySurface = 29,
    Relationship = 30,
    SaDoc = 31,
    SaReport = 32,
    SaReportTemplate = 33,
    ScaleBar = 34,
    ScanStripeMesh = 35,
    Slot = 36,
    Sphere = 37,
    Surface = 38,
    Table = 39,
    TcpFixture = 40,
    Torus = 41,
    VectorGroup = 42,
}

public enum PointFilterInputType { CardinalPoints = 1, InputPoints = 2, NominalCardinalPoints = 3 }
public enum RelWeightingMode { NormalizeEquationCount = 1, NormalizeEquationCountAndToleranceWidth = 2, ResetAllWeights = 3, NormalizeSquareRootEquationCount = 4, NormalizeSquareRootAndToleranceWidth = 5 }
public enum RenderModeType { Wireframe = 1, HiddenLineRemoved = 2, SolidAndEdges = 3, Solid = 4 }
public enum ReportOutputType { None = 1, SaReport = 2, SaDocument = 3, Pdf = 4, Rtf = 5 }
public enum ReportPageSettings { Portrait = 1, Landscape = 2 }
public enum ReportViewType { None = 1, CurrentView = 2, CalloutView = 3 }
public enum SurfaceAnalysisMode { None = 1, Relationship = 2, Normals = 3, Curvature = 4, DeviationRms = 5, DeviationMax = 6, DeviationAverage = 7, DeviationMin = 8, DeviationMaxAbsolute = 9, DeviationMaxDelta = 10, PseudoSurface = 11 }
public enum TemperatureUnits { Fahrenheit = 1, Celsius = 2 }
public enum TranslucencyType { Solid = 1, Translucent = 2, Wireframe = 3 }
public enum CompTechnique { Standard = 1, MaxInscribed = 2, MinCircumscribed = 3 }
public enum DegreeOfFreedom { Any = 1, LockFocusLocation = 2, LockVertexLocation = 3 }
public enum FitMethod { MinimumRms = 1, BestAxis = 2 }
public enum MeasuredSideForPlanarOffset { AbovePlane = 1, ProbeCenter = 2, BelowPlane = 3 }
public enum MeasuredSideForRadialOffset { Inside = 1, ProbeCenter = 2, Outside = 3 }
public enum MpDialogInteractionMode { BlockApplicationInteraction = 1, AllowApplicationInteraction = 2 }
public enum MpInteractionMode { HaltOnFailureOnly = 1, HaltOnFailureOrPartialSuccess = 2, NeverHalt = 3 }
public enum NormalDirection { ProbingDirection = 1, WorkingOriginPositive = 2, RightHandRule = 3 }
public enum SaInteractionMode { Manual = 1, Automatic = 2, Silent = 3 }
public enum SlotType { Round = 1, Square = 2 }
public enum SphereFitComputationMode { Standard = 1, MaxInscribed = 2, MinCircumscribed = 3 }
public enum WindowState { Maximize = 1, Minimize = 2, Restore = 3, Show = 4, Hide = 5 }

public sealed record ChartName { public required string Name { get; init; } }
public sealed record CollectionName { public required string Name { get; init; } }
public sealed record FrameName { public required string Name { get; init; } }
public sealed record ViewName { public required string Name { get; init; } }

public sealed record PointName
{
    public required string CollectionName { get; init; }
    public required string GroupName { get; init; }
    public required string TargetName { get; init; }
}

public sealed record CollectionInstrumentId
{
    public required string CollectionName { get; init; }
    public required int InstrumentId { get; init; }
}

public sealed record CollectionGroupName
{
    public required string CollectionName { get; init; }
    public required string GroupName { get; init; }
}

public sealed record CollectionObjectName
{
    public required string CollectionName { get; init; }
    public required string ObjectName { get; init; }
    public required ObjectType ObjectType { get; init; }
}

public sealed record CollectionItemName
{
    public required string CollectionName { get; init; }
    public required string ItemName { get; init; }
    public ItemType? ItemType { get; init; }
}

public sealed record CollectionVectorGroupName
{
    public required string CollectionName { get; init; }
    public required string VectorGroupName { get; init; }
}

public sealed record VectorName
{
    public required string CollectionName { get; init; }
    public required string GroupName { get; init; }
    public required string Name { get; init; }
}

public sealed record Vector
{
    public required double X { get; init; }
    public required double Y { get; init; }
    public required double Z { get; init; }
}

public sealed record Transform
{
    public required double[] Values { get; init; }
}

public sealed record WorldTransform
{
    public required Transform Transform { get; init; }
    public required double ScaleFactor { get; init; }
}

public sealed record Color
{
    public required uint Red { get; init; }
    public required uint Green { get; init; }
    public required uint Blue { get; init; }
}

public sealed record ColorizationOptions
{
    public ColorRangeMethod ColorRangeMethod { get; init; } = ColorRangeMethod.Continuous;
    public BaseColorType BaseHighColor { get; init; } = BaseColorType.Blue;
    public BaseMidColorType BaseMidColor { get; init; } = BaseMidColorType.Green;
    public BaseColorType BaseLowColor { get; init; } = BaseColorType.Red;
    public bool DrawTubes { get; init; }
    public bool DrawArrowheads { get; init; } = true;
    public bool IndicateValues { get; init; }
    public double VectorMagnification { get; init; } = 100;
    public int VectorWidth { get; init; } = 1;
    public bool DrawBlotches { get; init; }
    public double BlotchSize { get; init; } = 0.1;
    public bool ShowOutOfToleranceOnly { get; init; }
    public bool ShowColorBarInView { get; init; }
    public bool ShowColorBarPercentages { get; init; } = true;
    public bool ShowColorBarFractions { get; init; }
    public double HighSaturationLimit { get; init; } = 0.5;
    public double LowSaturationLimit { get; init; } = -0.5;
    public double HighTolerance { get; init; } = 0.03;
    public double LowTolerance { get; init; } = -0.03;
}

public sealed record FileReference
{
    public required string Path { get; init; }
    public bool EmbeddedFile { get; init; }
}

public sealed record Font
{
    public string FontName { get; init; } = "MS Shell Dlg";
    public uint Size { get; init; } = 8;
    public Color Color { get; init; } = new() { Red = 0, Green = 0, Blue = 0 };
}

public readonly record struct ScalarToleranceLimit(bool Enabled = false, double Value = 0);
public readonly record struct ToleranceLimit(bool Enabled = false, double Value = 0);

public sealed record ToleranceVectorOptions
{
    public ToleranceLimit HighX { get; init; }
    public ToleranceLimit HighY { get; init; }
    public ToleranceLimit HighZ { get; init; }
    public ToleranceLimit HighMagnitude { get; init; }
    public ToleranceLimit LowX { get; init; }
    public ToleranceLimit LowY { get; init; }
    public ToleranceLimit LowZ { get; init; }
    public ToleranceLimit LowMagnitude { get; init; }
}

public readonly record struct FitConstraintScalarOptions(
    ScalarToleranceLimit High = default,
    ScalarToleranceLimit Low = default);

public readonly record struct ToleranceScalarOptions(
    ScalarToleranceLimit High = default,
    ScalarToleranceLimit Low = default);

public sealed record EmbeddedReportFile
{
    public required string CollectionName { get; init; }
    public required string FileName { get; init; }
}

public sealed record ReportOutputOptions
{
    public ReportOutputType OutputType { get; init; } = ReportOutputType.SaReport;
    public string? ExternalPath { get; init; }
    public EmbeddedReportFile? EmbeddedFile { get; init; }
}

public sealed record ReportViewOptions
{
    public required ReportViewType ViewType { get; init; }
    public string CollectionName { get; init; } = string.Empty;
    public string CalloutName { get; init; } = string.Empty;
}

public sealed record ProjectionOptions
{
    public string ProjectionType { get; init; } = "Object To Probe Vectors";
    public bool IgnoreEdgeProjections { get; init; }
    public bool OverrideTargetOffsets { get; init; }
    public double OverrideTargetOffsetsValue { get; init; }
    public bool AddExtraMaterialThickness { get; init; }
    public double ExtraMaterialThicknessValue { get; init; }
}

public sealed record PointDeltaReportOptions
{
    public CoordinateSystemType CoordinateSystem { get; init; } = CoordinateSystemType.Cartesian;
    public string DetailsFormat { get; init; } = "Single";
    public bool ShowPointA { get; init; } = true;
    public bool ShowPointB { get; init; } = true;
    public bool ShowDelta { get; init; } = true;
    public bool ShowMagnitude { get; init; } = true;
    public bool ShowComponent1 { get; init; } = true;
    public bool ShowComponent2 { get; init; } = true;
    public bool ShowComponent3 { get; init; } = true;
    public bool SortPointNames { get; init; }
    public bool ShowToleranceFields { get; init; } = true;
    public bool ColorizeInToleranceFields { get; init; } = true;
}
