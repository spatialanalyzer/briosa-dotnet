using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Identity = Briosa.Client.Transport.BriosaProtocolIdentity;
using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

public sealed class ServerDiscoveryTests : IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly string _root = Path.Combine(Path.GetTempPath(), "briosa-selection-" + Guid.NewGuid());

    [Fact]
    public void SharedSelectionVectorsHaveIdenticalOutcomes()
    {
        using var document = JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "selection-cases.json")));
        foreach (var scenario in document.RootElement.GetProperty("cases").EnumerateArray())
        {
            var options = scenario.GetProperty("options").Deserialize<BriosaServerSelection>(
                JsonOptions)!;
            options.Validate();
            var candidates = scenario.GetProperty("candidates").EnumerateArray().Select(c => new BriosaInstallation(
                Text(c, "id")!, Text(c, "path")!, Text(c, "version")!, Text(c, "sourceRevision")!,
                Text(c, "target")!, Text(c, "rid")!, c.GetProperty("major").GetUInt32(),
                c.GetProperty("revision").GetUInt32(), Text(c, "manifestSha256")!, Text(c, "scope")!)).ToArray();
            var (selected, code) = ServerSelectionPolicy.Select(candidates, options,
                Text(scenario, "target")!, scenario.GetProperty("requiredMajor").GetUInt32(),
                scenario.GetProperty("minimumRevision").GetUInt32());
            Assert.Equal(Text(scenario, "selectedId"), selected?.InstallationId);
            Assert.Equal(Text(scenario, "error"), code);
        }
    }

    [Fact]
    public void ManagedInstallationRequiresCommittedIdentityAndManifestDigest()
    {
        var path = Install();
        var selected = InstalledServerDiscovery.ReadExecutable(path, "user");
        Assert.Equal("0.7.0", selected.Version);
        Assert.Equal(Identity.SpatialAnalyzerTarget, selected.SpatialAnalyzerTarget);
        var manifestPath = Path.Combine(Path.GetDirectoryName(path)!, "manifest.json");
        var manifest = JsonNode.Parse(File.ReadAllText(manifestPath))!;
        manifest["sourceRevision"] = new string('c', 40);
        File.WriteAllText(manifestPath, manifest.ToJsonString());
        Assert.Throws<IOException>(() => InstalledServerDiscovery.ReadExecutable(path, "user"));
    }

    [Fact]
    public void StagedOrMissingFilesCannotBecomeInstallations()
    {
        var path = Install();
        File.Delete(Path.Combine(Path.GetDirectoryName(path)!, "Briosa.Worker.exe"));
        Assert.Throws<FileNotFoundException>(() => InstalledServerDiscovery.ReadExecutable(path, "user"));
    }

    [Fact]
    public void ExplicitMissingPathNeverFallsBackToInstalledProducts()
    {
        var report = BriosaInstallations.Discover(new BriosaServerSelection
        {
            ExecutablePath = Path.Combine(_root, "missing", "Briosa.Server.exe")
        });
        Assert.Null(report.Selected);
        Assert.Equal(OperatingSystem.IsWindows() ? "server-installation-invalid" : "server-platform-unsupported", report.DiagnosticCode);
    }

    [Fact]
    public void BuildIdentityIsIndependentOfCompatibleContractButStillMatchesSelectedInstallation()
    {
        var (server, capabilities) = Snapshot();
        BriosaProtocolCompatibility.Validate(server, capabilities);
        var installation = new BriosaInstallation("id", "path", server.Version.BriosaVersion,
            server.Version.SourceRevision, Identity.SpatialAnalyzerTarget, "win-x64", 1, 0, "hash", "user");
        BriosaProtocolCompatibility.ValidateInstallation(server, installation);
        server.Version.SourceRevision = new string('b', 40);
        BriosaProtocolCompatibility.Validate(server, capabilities);
        Assert.Throws<BriosaCompatibilityException>(() => BriosaProtocolCompatibility.ValidateInstallation(server, installation));
        server.Compatibility.Major = 2;
        Assert.Throws<BriosaCompatibilityException>(() => BriosaProtocolCompatibility.Validate(server, capabilities));
    }

    [Fact]
    public void MissingContractRequiresTheExactLegacyException()
    {
        var (server, capabilities) = Snapshot();
        server.Compatibility = null;
        Assert.Throws<BriosaCompatibilityException>(() => BriosaProtocolCompatibility.Validate(server, capabilities));
        server.Version.BriosaVersion = ServerSelectionPolicy.LegacyVersion;
        server.Version.SourceRevision = ServerSelectionPolicy.LegacyRevision;
        BriosaProtocolCompatibility.Validate(server, capabilities);
        server.Version.SourceRevision = new string('a', 40);
        Assert.Throws<BriosaCompatibilityException>(() => BriosaProtocolCompatibility.Validate(server, capabilities));
    }

    private static (Transport.GetServerInfoResponse, Transport.ListCapabilitiesResponse) Snapshot() =>
        (new Transport.GetServerInfoResponse
        {
            Version = new Transport.VersionCoordinates
            {
                BriosaVersion = "0.9.0",
                SourceRevision = new string('a', 40),
                ProtocolPackage = "briosa",
                SpatialAnalyzerTarget = Identity.SpatialAnalyzerTarget
            },
            Compatibility = new Transport.CompatibilityContract { Major = 1 },
            TargetIsolationMode = Transport.TargetIsolationMode.SingleTenant
        }, new Transport.ListCapabilitiesResponse { ProtocolPackage = "briosa", SpatialAnalyzerTarget = Identity.SpatialAnalyzerTarget });

    private string Install()
    {
        var id = "briosa-0.7.0-sa-" + Identity.SpatialAnalyzerTarget + "-win-x64";
        var product = Path.Combine(_root, "products", id);
        var payload = Path.Combine(product, "payload");
        Directory.CreateDirectory(payload);
        var manifest = JsonSerializer.SerializeToUtf8Bytes(new
        {
            schemaVersion = 3,
            artifactName = id,
            briosaVersion = "0.7.0",
            sourceRevision = new string('a', 40),
            spatialAnalyzerTarget = Identity.SpatialAnalyzerTarget,
            runtimeIdentifier = "win-x64",
            protocolPackage = "briosa",
            spatialAnalyzerBundled = false,
            compatibility = new { major = 1, revision = 0 }
        });
        File.WriteAllBytes(Path.Combine(payload, "manifest.json"), manifest);
        File.WriteAllText(Path.Combine(payload, "Briosa.Server.exe"), "inert");
        File.WriteAllText(Path.Combine(payload, "Briosa.Worker.exe"), "inert");
        File.WriteAllText(Path.Combine(product, "receipt.json"), JsonSerializer.Serialize(new
        {
            schemaVersion = 1,
            package = new { id, component = "server", version = "0.7.0", spatialAnalyzerTarget = Identity.SpatialAnalyzerTarget, runtimeIdentifier = "win-x64" },
            files = new Dictionary<string, string>
            {
                ["manifest.json"] = Convert.ToHexStringLower(SHA256.HashData(manifest)),
                ["Briosa.Server.exe"] = new string('a', 64),
                ["Briosa.Worker.exe"] = new string('a', 64)
            }
        }));
        return Path.Combine(payload, "Briosa.Server.exe");
    }

    private static string? Text(JsonElement value, string name) =>
        value.TryGetProperty(name, out var property) && property.ValueKind == JsonValueKind.String ? property.GetString() : null;

    public void Dispose()
    {
        if (Directory.Exists(_root)) Directory.Delete(_root, recursive: true);
    }
}
