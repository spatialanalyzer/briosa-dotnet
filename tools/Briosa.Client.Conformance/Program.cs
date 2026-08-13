using System.Text.Json;
using Transport = Briosa.Client.Transport;

var report = new
{
    schema_version = 2,
    implementation = "dotnet",
    protocol = new
    {
        artifact = Transport.BriosaProtocolIdentity.ArtifactName,
        source_revision = Transport.BriosaProtocolIdentity.SourceRevision,
        package = Transport.BriosaProtocolIdentity.ProtocolPackage,
        spatial_analyzer_target = Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget,
    },
    construction_is_dormant = true,
    owns_local_server = true,
    default_start = new[]
    {
        "start_server",
        "start_spatial_analyzer_sdk",
        "launch_spatial_analyzer",
        "connect_to_spatial_analyzer",
        "verify_mp_readiness",
    },
    lifecycle_methods = new[]
    {
        "get_spatial_analyzer_state",
        "launch_spatial_analyzer",
        "close_owned_spatial_analyzer",
        "get_spatial_analyzer_sdk_state",
        "start_spatial_analyzer_sdk",
        "connect_to_spatial_analyzer",
        "reconnect_to_spatial_analyzer",
        "stop_spatial_analyzer_sdk",
        "recover_spatial_analyzer_sdk",
    },
    stop_closes_spatial_analyzer = false,
    automatic_mp_replay = false,
};

await Console.Out.WriteLineAsync(JsonSerializer.Serialize(report)).ConfigureAwait(false);
