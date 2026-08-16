// Drafted mechanically from the approved Briosa documentation contract.
#pragma warning disable CA1819 // MP list outputs are deliberately detached arrays.
#pragma warning disable CS1591 // Full API reference is maintained in briosa-docs.
namespace Briosa;

public sealed record ActiveUnits
{
    public required string Length { get; init; }

    public required string Angular { get; init; }

    public required string Temperature { get; init; }
}

public sealed record AskForStringPullDownVersionResult
{
    public required string Answer { get; init; }

    public required int AnswerIndex { get; init; }
}

public sealed record BestFitTransformationGroupToGroupResult
{
    public required Transform TransformInWorking { get; init; }

    public required WorldTransform OptimumTransform { get; init; }

    public required double RmsDeviation { get; init; }

    public required double MaximumAbsoluteDeviation { get; init; }

    public required int NumberOfUnknowns { get; init; }

    public required int NumberOfEquations { get; init; }

    public required double Robustness { get; init; }
}

public sealed record ComputeGroupToGroupOrientationRxRyRzResult
{
    public required double Rx { get; init; }

    public required double Ry { get; init; }

    public required double Rz { get; init; }
}

public sealed record CreatePointUncertaintyCloudPointSetsResult
{
    public required CollectionObjectName[] PointGroups { get; init; }

    public required CollectionObjectName[] PointSets { get; init; }

    public required CollectionObjectName[] PointClouds { get; init; }
}

public sealed record DirectCadAccessResult
{
    public required bool ImportWarnings { get; init; }

    public required string ImportWarningMessages { get; init; }

    public required Vector ExtentsMin { get; init; }

    public required Vector ExtentsMax { get; init; }
}

public sealed record GetActiveLanguageResult
{
    public required FileReference LanguageFileName { get; init; }

    public required bool CustomLanguage { get; init; }
}

public sealed record GetBSplinePropertiesResult
{
    public required int Degree { get; init; }

    public required int Knots { get; init; }

    public required int ControlPoints { get; init; }

    public required double RangeMin { get; init; }

    public required double RangeMax { get; init; }

    public required double Length { get; init; }
}

public sealed record GetCirclePropertiesResult
{
    public required Vector CenterCoordinate { get; init; }

    public required Vector NormalDirection { get; init; }

    public required double Radius { get; init; }

    public required double Diameter { get; init; }
}

public sealed record GetConePropertiesResult
{
    public required Vector ConeEndPointInWorkingCoordinates { get; init; }

    public required Vector ConeAxisInWorkingCoordinates { get; init; }

    public required double ConeLength { get; init; }

    public required double ConeThetaStart { get; init; }

    public required double ConeThetaSpan { get; init; }

    public required double ConeIncludedAngle { get; init; }

    public required double CutLengthFromApex { get; init; }
}

public sealed record GetCoordinateForIthPointInPointSetResult
{
    public required string PointName { get; init; }

    public required Vector PointCoordinates { get; init; }
}

public sealed record GetCylinderPropertiesResult
{
    public required Vector BeginCoordinate { get; init; }

    public required Vector EndCoordinate { get; init; }

    public required Vector AxisDirection { get; init; }

    public required double Length { get; init; }

    public required double Radius { get; init; }

    public required double Diameter { get; init; }

    public required bool NominalsPointInward { get; init; }

    public required int Facets { get; init; }

    public required bool EnableThetaExtentDisplayMode { get; init; }

    public required double ThetaStartInDegrees { get; init; }

    public required double ThetaSpanInDegrees { get; init; }
}

public sealed record GetDimensionValueResult
{
    public required double DimensionsValue { get; init; }

    public required bool NominalValueEnabled { get; init; }

    public required bool HighToleranceEnabled { get; init; }

    public required bool LowToleranceEnabled { get; init; }

    public required double NominalValue { get; init; }

    public required double HighTolerance { get; init; }

    public required double LowTolerance { get; init; }
}

public sealed record GetEllipsePropertiesResult
{
    public required Vector CenterCoordinate { get; init; }

    public required Vector NormalDirection { get; init; }

    public required double MajorAxisRadius { get; init; }

    public required double MinorAxisRadius { get; init; }
}

public sealed record GetEulerParametersForFrameResult
{
    public required double X { get; init; }

    public required double Y { get; init; }

    public required double Z { get; init; }

    public required double E1 { get; init; }

    public required double E2 { get; init; }

    public required double E3 { get; init; }

    public required double E4 { get; init; }
}

public sealed record GetEulerParametersForIthFrameInFrameSetResult
{
    public required double X { get; init; }

    public required double Y { get; init; }

    public required double Z { get; init; }

    public required double E1 { get; init; }

    public required double E2 { get; init; }

    public required double E3 { get; init; }

    public required double E4 { get; init; }
}

public sealed record GetGeomRelationshipAutoVectorsResult
{
    public required bool AutoVectorsNominalAvnEnabled { get; init; }

    public required CollectionObjectName AutoVectorsNominalAvnName { get; init; }

    public required bool AutoVectorsFitAvfEnabled { get; init; }

    public required CollectionObjectName AutoVectorsFitAvfName { get; init; }

    public required string PointsType { get; init; }
}

public sealed record GetGeomRelationshipCriteriaResult
{
    public required double Nominal { get; init; }

    public required double Measured { get; init; }

    public required double Delta { get; init; }

    public required double LowTolerance { get; init; }

    public required double HighTolerance { get; init; }

    public required double OptimizationDeltaWeight { get; init; }

    public required double OptimizationOutOfToleranceWeight { get; init; }

    public required string IsWithinTolerance { get; init; }

    public required bool HasUncertainty { get; init; }

    public required double Uncertainty { get; init; }
}

public sealed record GetGeomRelationshipPointListResult
{
    public required PointName[] AllPoints { get; init; }

    public required PointName[] UsedPoints { get; init; }

    public required PointName[] IgnoredPoints { get; init; }
}

public sealed record GetIthPointFromGroupResult
{
    public required PointName CompletePointName { get; init; }

    public required string PointNameOnly { get; init; }

    public required Vector VectorInWorking { get; init; }
}

public sealed record GetIthVectorFromVectorGroupResult
{
    public required string VectorName { get; init; }

    public required Vector BeginInWorking { get; init; }

    public required Vector EndInWorking { get; init; }

    public required Vector TotalDeltaInWorking { get; init; }

    public required Vector IjkUnitVectorInWorking { get; init; }

    public required double Magnitude { get; init; }
}

public sealed record GetIthVectorFromVectorNameRefListResult
{
    public required CollectionObjectName VectorGroupName { get; init; }

    public required string VectorName { get; init; }

    public required Vector BeginInWorking { get; init; }

    public required Vector EndInWorking { get; init; }

    public required Vector TotalDeltaInWorking { get; init; }

    public required Vector IjkUnitVectorInWorking { get; init; }

    public required double Magnitude { get; init; }
}

public sealed record GetLinePropertiesResult
{
    public required Vector BeginCoordinate { get; init; }

    public required Vector EndCoordinate { get; init; }

    public required Vector DeltaComponents { get; init; }

    public required double Length { get; init; }

    public required double AngleAboutXFromYInYzPlane { get; init; }

    public required double AngleAboutYFromZInXzPlane { get; init; }

    public required double AngleAboutZFromXInXyPlane { get; init; }
}

public sealed record GetMeasurementAuxiliaryDataResult
{
    public required double Value { get; init; }

    public required string Units { get; init; }
}

public sealed record GetMeasurementWeatherDataResult
{
    public required double TemperatureDegF { get; init; }

    public required double PressureInHg { get; init; }

    public required double HumidityRh { get; init; }
}

public sealed record GetNamedDoubleListVariableMinMaxResult
{
    public required double MinimumValue { get; init; }

    public required double MaximumValue { get; init; }
}

public sealed record GetPipeRelationshipCutStatusResult
{
    public required bool Pipe1CutAvailable { get; init; }

    public required bool Pipe1CutActive { get; init; }

    public required bool Pipe2CutAvailable { get; init; }

    public required bool Pipe2CutActive { get; init; }
}

public sealed record GetPipeRelationshipPropertiesResult
{
    public required CollectionObjectName Pipe1ObjectName { get; init; }

    public required double Pipe1InnerDiameter { get; init; }

    public required double Pipe1OuterDiameter { get; init; }

    public required double Pipe1CutBegin { get; init; }

    public required double Pipe1CutEnd { get; init; }

    public required CollectionObjectName Pipe2ObjectName { get; init; }

    public required double Pipe2InnerDiameter { get; init; }

    public required double Pipe2OuterDiameter { get; init; }

    public required double Pipe2CutBegin { get; init; }

    public required double Pipe2CutEnd { get; init; }
}

public sealed record GetPipeRelationshipWeightsResult
{
    public required double OverallWeight { get; init; }

    public required double AxisOffset { get; init; }

    public required double AxisAlignment { get; init; }

    public required double CenterPull { get; init; }

    public required double OutOfMaterialWeight { get; init; }

    public required double OutOfMaterialStaticOffset { get; init; }

    public required bool ConstrainRegionAtOd { get; init; }

    public required bool ConstrainIdOdOverlap { get; init; }
}

public sealed record GetPlanePropertiesResult
{
    public required Vector NormalDirection { get; init; }

    public required Vector PointOnPlane { get; init; }

    public required double DParameter { get; init; }
}

public sealed record GetPointCoordinateCylindricalResult
{
    public required double RadiusValue { get; init; }

    public required double ThetaValue { get; init; }

    public required double ZValue { get; init; }
}

public sealed record GetPointCoordinatePolarResult
{
    public required double RadiusValue { get; init; }

    public required double ThetaValue { get; init; }

    public required double PhiValue { get; init; }
}

public sealed record GetPointCoordinateResult
{
    public required Vector VectorRepresentation { get; init; }

    public required double XValue { get; init; }

    public required double YValue { get; init; }

    public required double ZValue { get; init; }
}

public sealed record GetPointOfViewParametersResult
{
    public required double RotationX { get; init; }

    public required double RotationY { get; init; }

    public required double RotationZ { get; init; }

    public required bool RestoreZoomSettings { get; init; }

    public required double ScaleFactor { get; init; }

    public required double OriginX { get; init; }

    public required double OriginY { get; init; }

    public required bool RestoreRenderMode { get; init; }
}

public sealed record GetPointPropertiesResult
{
    public required double PlanarOffset { get; init; }

    public required double RadialOffset { get; init; }

    public required double Ux { get; init; }

    public required double Uy { get; init; }

    public required double Uz { get; init; }

    public required double Umag { get; init; }

    public required ToleranceVectorOptions PositionTolerance { get; init; }

    public required Vector ComponentWeights { get; init; }
}

public sealed record GetPointToLineDistanceResult
{
    public required Vector VectorRepresentation { get; init; }

    public required double XValue { get; init; }

    public required double YValue { get; init; }

    public required double ZValue { get; init; }

    public required double Magnitude { get; init; }
}

public sealed record GetPointToPointDistanceResult
{
    public required Vector VectorRepresentation { get; init; }

    public required double XValue { get; init; }

    public required double YValue { get; init; }

    public required double ZValue { get; init; }

    public required double Magnitude { get; init; }
}

public sealed record GetPointToleranceResult
{
    public required bool UseHighXTolerance { get; init; }

    public required double HighXTolerance { get; init; }

    public required bool UseHighYTolerance { get; init; }

    public required double HighYTolerance { get; init; }

    public required bool UseHighZTolerance { get; init; }

    public required double HighZTolerance { get; init; }

    public required bool UseHighMagTolerance { get; init; }

    public required double HighMagTolerance { get; init; }

    public required bool UseLowXTolerance { get; init; }

    public required double LowXTolerance { get; init; }

    public required bool UseLowYTolerance { get; init; }

    public required double LowYTolerance { get; init; }

    public required bool UseLowZTolerance { get; init; }

    public required double LowZTolerance { get; init; }

    public required bool UseLowMagTolerance { get; init; }

    public required double LowMagTolerance { get; init; }

    public required ToleranceVectorOptions VectorTolerance { get; init; }
}

public sealed record GetRelationshipFitConstraintsScalarTypeResult
{
    public required bool UseHighTolerance { get; init; }

    public required double HighTolerance { get; init; }

    public required bool UseLowTolerance { get; init; }

    public required double LowTolerance { get; init; }

    public required FitConstraintScalarOptions FitConstraintOptions { get; init; }
}

public sealed record GetRelationshipOutlierRejectionScalarTypeResult
{
    public required bool UseHighLimit { get; init; }

    public required double HighLimit { get; init; }

    public required bool UseLowLimit { get; init; }

    public required double LowLimit { get; init; }
}

public sealed record GetRelationshipProjectionOptionsResult
{
    public required bool IgnoreEdgeProjections { get; init; }

    public required bool ProbeOffsetsOverrideTargetValues { get; init; }

    public required double ProbeOffsetsOverrideValue { get; init; }

    public required bool AddExtraMaterial { get; init; }

    public required double ExtraMaterialThickness { get; init; }
}

public sealed record GetRelationshipSubSamplingOptionsResult
{
    public required bool UseEveryIthPoint { get; init; }

    public required int IValue { get; init; }

    public required bool UseNoMoreThanNPoints { get; init; }

    public required int NValue { get; init; }
}

public sealed record GetRelationshipToleranceScalarTypeResult
{
    public required bool UseHighTolerance { get; init; }

    public required double HighTolerance { get; init; }

    public required bool UseLowTolerance { get; init; }

    public required double LowTolerance { get; init; }

    public required ToleranceScalarOptions ToleranceOptions { get; init; }
}

public sealed record GetRelationshipToleranceVectorTypeResult
{
    public required bool UseHighXTolerance { get; init; }

    public required double HighXTolerance { get; init; }

    public required bool UseHighYTolerance { get; init; }

    public required double HighYTolerance { get; init; }

    public required bool UseHighZTolerance { get; init; }

    public required double HighZTolerance { get; init; }

    public required bool UseHighMagTolerance { get; init; }

    public required double HighMagTolerance { get; init; }

    public required bool UseLowXTolerance { get; init; }

    public required double LowXTolerance { get; init; }

    public required bool UseLowYTolerance { get; init; }

    public required double LowYTolerance { get; init; }

    public required bool UseLowZTolerance { get; init; }

    public required double LowZTolerance { get; init; }

    public required bool UseLowMagTolerance { get; init; }

    public required double LowMagTolerance { get; init; }

    public required ToleranceVectorOptions VectorTolerance { get; init; }
}

public sealed record GetReportTagValueResult
{
    public required string TagValueAsString { get; init; }

    public required int TagValueAsInteger { get; init; }

    public required double TagValueAsDouble { get; init; }
}

public sealed record GetScaleBarStatsResult
{
    public required double NominalLength { get; init; }

    public required double ActualLength { get; init; }

    public required double Deviation { get; init; }
}

public sealed record GetScreenResolutionResult
{
    public required int IntegerWindowTopLeftXPosition { get; init; }

    public required int IntegerWindowTopLeftYPosition { get; init; }

    public required int IntegerWidth { get; init; }

    public required int IntegerHeight { get; init; }

    public required int ViewWidth { get; init; }

    public required int ViewHeight { get; init; }
}

public sealed record GetSlotPropertiesResult
{
    public required Transform SlotTransformInWorkingCoordinates { get; init; }

    public required Vector CenterInWorkingCoordinates { get; init; }

    public required Vector NormalDirectionInWorkingCoordinates { get; init; }

    public required double SlotLength { get; init; }

    public required double SlotWidth { get; init; }

    public required bool RoundSlotType { get; init; }

    public required Vector CenterlinePt1InWorkingCoordinates { get; init; }

    public required Vector CenterlinePt2InWorkingCoordinates { get; init; }
}

public sealed record GetSpherePropertiesResult
{
    public required Vector CenterCoordinate { get; init; }

    public required double Radius { get; init; }

    public required double Diameter { get; init; }
}

public sealed record GetSurfacePhysicalStatsResult
{
    public required double Volume { get; init; }

    public required double Area { get; init; }
}

public sealed record GetTorusPropertiesResult
{
    public required Vector CenterCoordinate { get; init; }

    public required Vector NormalDirection { get; init; }

    public required double MajorRadius { get; init; }

    public required double MinorRadius { get; init; }
}

public sealed record GetVectorFromVectorGroupByNameResult
{
    public required Vector BeginInWorking { get; init; }

    public required Vector EndInWorking { get; init; }

    public required Vector TotalDeltaInWorking { get; init; }

    public required Vector IjkUnitVectorInWorking { get; init; }

    public required double Magnitude { get; init; }
}

public sealed record GetVectorGroupPropertiesResult
{
    public required int TotalVectors { get; init; }

    public required int VectorsInTolerance { get; init; }

    public required int VectorsOutOfTolerance { get; init; }

    public required int InvalidVectors { get; init; }

    public required double VectorsInTolerance2 { get; init; }

    public required double VectorsOutOfTolerance2 { get; init; }

    public required double AbsoluteMaxMagnitude { get; init; }

    public required double AbsoluteMinMagnitude { get; init; }

    public required double MaxMagnitude { get; init; }

    public required double MinMagnitude { get; init; }

    public required double StandardDeviationFromZero { get; init; }

    public required double StandardDeviationFromMean { get; init; }

    public required double AvgMagnitude { get; init; }

    public required double AvgOfAbsMagnitude { get; init; }

    public required double HighToleranceValue { get; init; }

    public required double LowToleranceValue { get; init; }

    public required double RmsValue { get; init; }
}

public sealed record GroupToSurfaceFitResult
{
    public required WorldTransform OptimumTransform { get; init; }

    public required double RmsDeviation { get; init; }

    public required double MaximumAbsoluteDeviation { get; init; }
}

public sealed record MushroomTargetHoleInspectionResult
{
    public required double SphereFitRmsError { get; init; }

    public required double SphereFitMaxError { get; init; }
}

public sealed record QueryCloudsToObjectsResult
{
    public required double RmsDeviation { get; init; }

    public required double MaximumAbsoluteDeviation { get; init; }
}

public sealed record QueryCloudsToSurfaceResult
{
    public required double RmsDeviation { get; init; }

    public required double MaximumAbsoluteDeviation { get; init; }
}

public sealed record QueryFrameToFrameResult
{
    public required double X { get; init; }

    public required double Y { get; init; }

    public required double Z { get; init; }

    public required double RxRoll { get; init; }

    public required double RyPitch { get; init; }

    public required double RzYaw { get; init; }
}

public sealed record QueryGroupsToObjectsResult
{
    public required double RmsDeviation { get; init; }

    public required double MaxAbsoluteDeviation { get; init; }

    public required double AverageDeviation { get; init; }

    public required double StandardDeviation { get; init; }
}

public sealed record QueryPointToObjectsResult
{
    public required double DX { get; init; }

    public required double DY { get; init; }

    public required double DZ { get; init; }

    public required double DMag { get; init; }

    public required CollectionObjectName ResultantObject { get; init; }
}

public sealed record QueryPointsToObjectsResult
{
    public required double RmsDeviation { get; init; }

    public required double MaxAbsoluteDeviation { get; init; }

    public required double AverageDeviation { get; init; }

    public required double StandardDeviation { get; init; }
}

public sealed record SphereAxisCheckResult
{
    public required double SphereFitRmsError { get; init; }

    public required double SphereFitMaxError { get; init; }

    public required Vector VectorRepresentation { get; init; }

    public required double XValue { get; init; }

    public required double YValue { get; init; }

    public required double ZValue { get; init; }

    public required double Magnitude { get; init; }
}

public sealed record WorkingFrameProperties
{
    public required string FrameName { get; init; }

    public required string CollectionName { get; init; }

    public required CollectionObjectName WorkingFrame { get; init; }
}
