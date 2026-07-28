using Briosa.Core.V1Alpha1;

namespace Briosa.Client;

/// <summary>Pairs one server-state response with its capability identity.</summary>
public sealed record BriosaServerSnapshot(
    GetServerInfoResponse ServerInfo,
    ListCapabilitiesResponse Capabilities)
{
    /// <summary>Gets whether the worker owns a verified MP execution channel.</summary>
    public bool ReadyForMp =>
        ServerInfo.ReadyForMp &&
        ServerInfo.SpatialAnalyzerExecutionReadinessState ==
            SpatialAnalyzerExecutionReadinessState.ExecutionReady;
}
