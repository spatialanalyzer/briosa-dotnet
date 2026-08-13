#pragma warning disable CS1591 // Public contract is documented in briosa-docs.

namespace Briosa;

/// <summary>One detached, identity-validated discovery snapshot.</summary>
public sealed record BriosaServerSnapshot
{
    public required string BriosaVersion { get; init; }
    public required string SourceRevision { get; init; }
    public required string ProtocolPackage { get; init; }
    public required string SpatialAnalyzerTarget { get; init; }
    public bool ReadyForMp { get; init; }
    public WorkerRuntimeState WorkerState { get; init; }
    public SpatialAnalyzerConnectionState ConnectionState { get; init; }
    public SpatialAnalyzerExecutionReadinessState ExecutionReadinessState { get; init; }
    public TargetIsolationMode TargetIsolationMode { get; init; }
    public required IReadOnlyList<BriosaOperationCapability> Operations { get; init; }

    public bool Supports(string fullyQualifiedMethod) =>
        Operations.Any(item => string.Equals(
            item.FullyQualifiedMethod,
            fullyQualifiedMethod,
            StringComparison.Ordinal));
}

public sealed record BriosaOperationCapability
{
    public required string OperationId { get; init; }
    public required string GrpcService { get; init; }
    public required string Rpc { get; init; }
    public required string FullyQualifiedMethod { get; init; }
}

public enum WorkerRuntimeState
{
    Unspecified,
    Stopped,
    Starting,
    Ready,
    Degraded,
}

public enum TargetIsolationMode
{
    Unspecified,
    SingleTenant,
    LeaseIsolated,
}
