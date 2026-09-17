// Drafted mechanically from the approved Briosa documentation contract.
#pragma warning disable CA1008 // Zero is intentionally not a public MP choice.
#pragma warning disable CA1720
#pragma warning disable CA1805 // Explicit initializers preserve reviewed MP defaults.
#pragma warning disable CS1591
using Transport = Briosa.Client.Transport;

namespace Briosa;

public sealed record CalibrationApplianceNodeStatus
{
    public required bool InstrumentConnected { get; init; }
    public required bool CalibrationApplianceConnected { get; init; }
}

public sealed record CalloutPosition
{
    public required int XPosition { get; init; }
    public required int YPosition { get; init; }
    public required int XAnchorPosition { get; init; }
    public required int YAnchorPosition { get; init; }
    public required int CalloutWidth { get; init; }
    public required int CalloutHeight { get; init; }
}

public sealed record ConstructVectorGroupGroupToGroupCompareResult
{
    public required int VectorCount { get; init; }
    public required double RmsDeviation { get; init; }
    public required double MaxAbsoluteDeviation { get; init; }
    public required double AverageDeviation { get; init; }
}

public sealed record DeleteCollectionsByWildcardResult
{
    public required int NumDeleted { get; init; }
    public required int NumFailed { get; init; }
}

public sealed record DeleteFoldersByWildcardResult
{
    public required int NumDeleted { get; init; }
    public required int NumFailed { get; init; }
}

public sealed record DriftCheckResult
{
    public required double MaximumError { get; init; }
    public required double RmsError { get; init; }
    public required bool InstrumentAdded { get; init; }
    public CollectionInstrumentId? NewInstrument { get; init; }
}

public sealed record EulerXyzTransformComponents
{
    public required double X { get; init; }
    public required double Y { get; init; }
    public required double Z { get; init; }
    public required double Rx { get; init; }
    public required double Ry { get; init; }
    public required double Rz { get; init; }
}

public sealed record EulerZxzTransformComponents
{
    public required double X { get; init; }
    public required double Y { get; init; }
    public required double Z { get; init; }
    public required double FirstRz { get; init; }
    public required double Rx { get; init; }
    public required double SecondRz { get; init; }
}

public sealed record EulerZyxTransformComponents
{
    public required double X { get; init; }
    public required double Y { get; init; }
    public required double Z { get; init; }
    public required double Rz { get; init; }
    public required double Ry { get; init; }
    public required double Rx { get; init; }
}

public sealed record EulerZyzTransformComponents
{
    public required double X { get; init; }
    public required double Y { get; init; }
    public required double Z { get; init; }
    public required double FirstRz { get; init; }
    public required double Ry { get; init; }
    public required double SecondRz { get; init; }
}

public sealed record EvaluateFeatureCheckResult
{
    public required bool CheckEvaluated { get; init; }
    public required string CheckResult { get; init; }
    public required bool NonUniqueResult { get; init; }
    public required double MeasuredDeviationUpper { get; init; }
    public required double DistanceOutOfToleranceUpper { get; init; }
    public required WorldTransform EvalDeltaTransformUpper { get; init; }
    public required double MeasuredDeviationLower { get; init; }
    public required double DistanceOutOfToleranceLower { get; init; }
    public required WorldTransform EvalDeltaTransformLower { get; init; }
    public required string CheckType { get; init; }
    public required string ToleranceType { get; init; }
    public required double ToleranceSimple { get; init; }
    public required double ToleranceCompositeUpper { get; init; }
    public required double ToleranceCompositeLower { get; init; }
    public required double ToleranceRangeMin { get; init; }
    public required double ToleranceRangeMax { get; init; }
    public required double ToleranceNominalPlusMinusNominal { get; init; }
    public required double ToleranceNominalPlusMinusMinus { get; init; }
    public required double ToleranceNominalPlusMinusPlus { get; init; }
}

public sealed record EvaluateFeatureChecksResult
{
    public required int TotalPassed { get; init; }
    public required int TotalFailed { get; init; }
    public required int TotalIncomplete { get; init; }
}

public sealed record FeatureCheckDatumReferencesResult
{
    public required FeatureCheckDatumReference Datum1 { get; init; }
    public required FeatureCheckDatumReference Datum2 { get; init; }
    public required FeatureCheckDatumReference Datum3 { get; init; }
}

public sealed record FitErrorResult
{
    public required double RmsError { get; init; }
    public required double MaximumError { get; init; }
}

public sealed record FixedXyzTransformComponents
{
    public required double X { get; init; }
    public required double Y { get; init; }
    public required double Z { get; init; }
    public required double Rx { get; init; }
    public required double Ry { get; init; }
    public required double Rz { get; init; }
}

public sealed record FixedXyzTransformVectors
{
    public required Vector PositionInWorking { get; init; }
    public required Vector OrientationInWorking { get; init; }
}

public sealed record GeneralRelationshipStatistics
{
    public required double MaxDeviation { get; init; }
    public required double Rms { get; init; }
    public required bool HasSignedDeviation { get; init; }
    public required double SignedMaxDeviation { get; init; }
    public required double SignedMinDeviation { get; init; }
}

public sealed record GetCloudPointCountResult
{
    public required int PointsCount { get; init; }

    public required double PlanarOffset { get; init; }

    public required double RadialOffset { get; init; }

    public required int ActiveClippingPlanes { get; init; }

}

public sealed record GetCloudRGBValuesNearPointResult
{
    public required int LowValue { get; init; }

    public required int HighValue { get; init; }

    public required int AverageValue { get; init; }

    public required int StandardDeviation { get; init; }

}

public sealed record GetCloudRGBValuesResult
{
    public required int LowValue { get; init; }

    public required int HighValue { get; init; }

    public required int AverageValue { get; init; }

    public required int StandardDeviation { get; init; }

}

public sealed record InstrumentBestFitResult
{
    public required Transform TransformInWorking { get; init; }
    public required WorldTransform OptimumTransform { get; init; }
    public required double RmsDeviation { get; init; }
    public required double MaximumAbsoluteDeviation { get; init; }
    public required int NumberOfUnknowns { get; init; }
    public required int NumberOfEquations { get; init; }
    public required double Robustness { get; init; }
}

public sealed record InstrumentModelResult
{
    public required string Name { get; init; }
    public required string Model { get; init; }
}

public sealed record InstrumentPositionUpdate
{
    public required double XOrR { get; init; }
    public required double YOrThetaDegrees { get; init; }
    public required double ZOrPhiDegrees { get; init; }
    public required double TimeSinceUpdateSeconds { get; init; }
    public required string TimestampApproximate { get; init; }
}

public sealed record InstrumentTargetsAndModeProfiles
{
    public required IReadOnlyList<string> ModeProfiles { get; init; }
    public required IReadOnlyList<string> TargetNames { get; init; }
}

public sealed record InstrumentWeatherSetting
{
    public required double TemperatureFahrenheit { get; init; }
    public required double PressureMmHg { get; init; }
    public required double RelativeHumidityPercent { get; init; }
    public required bool SetAutomatically { get; init; }
}

public sealed record InstrumentXyzUncertainties
{
    public required double XUncertainty { get; init; }
    public required double YUncertainty { get; init; }
    public required double ZUncertainty { get; init; }
}

public sealed record LastInstrumentIndexResult
{
    public required int InstrumentIndex { get; init; }
    public required CollectionInstrumentId Instrument { get; init; }
}

public sealed record LrSelfTestResult
{
    public required double ReferenceArmLengthInches { get; init; }
    public required double ReferenceArmQuality { get; init; }
    public required int MirrorMeasurementCount { get; init; }
    public required double MirrorMeasurementRangeMeanInches { get; init; }
    public required double MirrorMeasurementRangeStandardDeviationInches { get; init; }
    public required double MirrorMeasurementQualityMean { get; init; }
    public required double MirrorMeasurementQualityStandardDeviation { get; init; }
    public required bool PassedReferenceArmQualityThreshold { get; init; }
    public required bool PassedMirrorOffsetDeltaThreshold { get; init; }
    public required bool PassedMirrorOffsetStandardDeviationThreshold { get; init; }
    public required bool PassedMirrorMeanQualityThreshold { get; init; }
    public required bool PassedOverall { get; init; }
}

public sealed record MeshVolumeResult
{
    public required double Above { get; init; }

    public required double Below { get; init; }

}

public sealed record PointComparisonResult
{
    public required Vector VectorRepresentation { get; init; }
    public required double XValue { get; init; }
    public required double YValue { get; init; }
    public required double ZValue { get; init; }
    public required double Magnitude { get; init; }
    public required PointName ResultingPointName { get; init; }
}

public sealed record PointToPointRelationshipStatistics
{
    public required double DeltaX { get; init; }
    public required double DeltaY { get; init; }
    public required double DeltaZ { get; init; }
    public required double DeltaMagnitude { get; init; }
    public required CollectionObjectName ReferenceFrame { get; init; }
}

public sealed record PointsToObjectsRelationshipStatistics
{
    public required double AbsoluteMaxDeviation { get; init; }
    public required double MaxDeviation { get; init; }
    public required double MinDeviation { get; init; }
    public required double Rms { get; init; }
    public required int CandidatePointCount { get; init; }
    public required int SampledPointCount { get; init; }
    public required int RejectedPointCount { get; init; }
    public required int UsedPointCount { get; init; }
    public required int OutOfTolerancePointCount { get; init; }
}

public sealed record RelationshipFitResult
{
    public required Transform TransformInReference { get; init; }
    public required WorldTransform TransformInWorking { get; init; }
    public required WorldTransform TransformInWorld { get; init; }
    public required double FitObjectiveValue { get; init; }
}

public sealed record ResetCloudBoundingBoxResult
{
    public required double XAxisDimension { get; init; }

    public required double YAxisDimension { get; init; }

    public required double ZAxisDimension { get; init; }

    public required Vector XAxisInWorld { get; init; }

    public required Vector YAxisInWorld { get; init; }

    public required Vector ZAxisInWorld { get; init; }

    public required Vector CentroidInWorld { get; init; }

    public required Transform ReferenceTransformInWorld { get; init; }

    public required Transform ReferenceTransformInWorking { get; init; }

    public required int PointsUsedForBoundingBox { get; init; }

}

public sealed record RobotModelLinkParameters
{
    public required RobotModelLinkConfiguration Configuration { get; init; }
    public required double EncoderValue { get; init; }
}

public sealed record TrackerEdmTheodoliteUncertainties
{
    public required double ThetaDispersionArcseconds { get; init; }
    public required double ThetaThreshold { get; init; }
    public required double PhiDispersionArcseconds { get; init; }
    public required double PhiThreshold { get; init; }
    public required double DistancePpm { get; init; }
    public required double DistanceThreshold { get; init; }
}

public sealed record TransformAxes
{
    public required Vector Origin { get; init; }
    public required Vector XAxis { get; init; }
    public required Vector YAxis { get; init; }
    public required Vector ZAxis { get; init; }
}

public sealed record WorldFixedXyzTransformComponents
{
    public required double X { get; init; }
    public required double Y { get; init; }
    public required double Z { get; init; }
    public required double Rx { get; init; }
    public required double Ry { get; init; }
    public required double Rz { get; init; }
    public required double Scale { get; init; }
}

public sealed record WorldFixedXyzTransformVectors
{
    public required Vector PositionInWorking { get; init; }
    public required Vector OrientationInWorking { get; init; }
    public required double Scale { get; init; }
}
