using System.Text.Json;
using System.Text.Json.Nodes;
using Identity = Briosa.Client.Transport.BriosaProtocolIdentity;

namespace Briosa.Client.Tests;

public sealed class ServerDiscoveryTests : IDisposable
{
    private static readonly string[] RequiredFiles = ["manifest.json", "Briosa.Server.exe", "Briosa.Worker.exe"];
    private readonly string _root = Path.Combine(Path.GetTempPath(), "briosa-discovery-" + Guid.NewGuid());
    private string User => Path.Combine(_root, "user");
    private string Machine => Path.Combine(_root, "machine");
    private string Local => Path.Combine(_root, "client");
    private static string Id => $"briosa-{Identity.BriosaVersion}-sa-{Identity.SpatialAnalyzerTarget}-win-x64";
    private string Resolve(string? configured = null) => ServerDiscovery.ResolveExecutablePath(configured, Local, User, Machine);

    [Fact]
    public void PrecedencePreservesExplicitLocalAndLegacyWhileAddingBothStores()
    {
        var legacy = Touch(Path.Combine(User, "Briosa", "servers", Identity.BriosaVersion,
            $"sa-{Identity.SpatialAnalyzerTarget}", "Briosa.Server.exe"));
        Assert.Equal(legacy, Resolve());
        var machine = Install(Machine);
        Assert.Equal(machine, Resolve());
        var user = Install(User);
        Assert.Equal(user, Resolve());
        var local = Touch(Path.Combine(Local, "briosa-server", "Briosa.Server.exe"));
        Assert.Equal(local, Resolve());
        var custom = Touch(Path.Combine(_root, "custom", "Briosa.Server.exe"));
        Assert.Equal(custom, Resolve(custom));
        Assert.Equal(local, Resolve(Path.Combine(_root, "absent", "Briosa.Server.exe")));
        Assert.Equal(local, Resolve(Touch(Path.Combine(_root, "not-Briosa.Server.exe"))));
    }

    [Theory]
    [InlineData("missing-receipt")]
    [InlineData("malformed-receipt")]
    [InlineData("receipt-array")]
    [InlineData("receipt-schema")]
    [InlineData("receipt-package")]
    [InlineData("receipt-id")]
    [InlineData("receipt-component")]
    [InlineData("receipt-version")]
    [InlineData("receipt-target")]
    [InlineData("receipt-rid")]
    [InlineData("receipt-files")]
    [InlineData("receipt-digest")]
    [InlineData("missing-manifest")]
    [InlineData("malformed-manifest")]
    [InlineData("manifest-array")]
    [InlineData("manifest-schema")]
    [InlineData("manifest-id")]
    [InlineData("manifest-version")]
    [InlineData("manifest-target")]
    [InlineData("manifest-rid")]
    [InlineData("manifest-revision")]
    [InlineData("manifest-protocol")]
    [InlineData("manifest-bundled")]
    [InlineData("missing-server")]
    [InlineData("missing-worker")]
    [InlineData("directory-server")]
    public void InvalidUserInstallationIsSkippedBeforeLaunch(string defect)
    {
        var server = Install(User);
        var payload = Path.GetDirectoryName(server)!;
        var receiptPath = Path.Combine(payload, "..", "receipt.json");
        var manifestPath = Path.Combine(payload, "manifest.json");
        var receipt = JsonNode.Parse(File.ReadAllText(receiptPath))!;
        var manifest = JsonNode.Parse(File.ReadAllText(manifestPath))!;
        switch (defect)
        {
            case "missing-receipt": File.Delete(receiptPath); break;
            case "malformed-receipt": File.WriteAllText(receiptPath, "{"); break;
            case "receipt-array": File.WriteAllText(receiptPath, "[]"); break;
            case "missing-manifest": File.Delete(manifestPath); break;
            case "malformed-manifest": File.WriteAllText(manifestPath, "{"); break;
            case "manifest-array": File.WriteAllText(manifestPath, "[]"); break;
            case "missing-server": File.Delete(server); break;
            case "directory-server": File.Delete(server); Directory.CreateDirectory(server); break;
            case "missing-worker": File.Delete(Path.Combine(payload, "Briosa.Worker.exe")); break;
            default:
                switch (defect)
                {
                    case "receipt-schema": receipt["schemaVersion"] = true; break;
                    case "receipt-package": receipt["package"] = null; break;
                    case "receipt-id": receipt["package"]!["id"] = "wrong"; break;
                    case "receipt-component": receipt["package"]!["component"] = "installer"; break;
                    case "receipt-version": receipt["package"]!["version"] = "99.0.0"; break;
                    case "receipt-target": receipt["package"]!["spatialAnalyzerTarget"] = "other"; break;
                    case "receipt-rid": receipt["package"]!["runtimeIdentifier"] = "win-arm64"; break;
                    case "receipt-files": receipt["files"] = new JsonObject(); break;
                    case "receipt-digest": receipt["files"]!["Briosa.Worker.exe"] = "invalid"; break;
                    case "manifest-schema": manifest["schemaVersion"] = 99; break;
                    case "manifest-id": manifest["artifactName"] = "wrong"; break;
                    case "manifest-version": manifest["briosaVersion"] = "99.0.0"; break;
                    case "manifest-target": manifest["spatialAnalyzerTarget"] = "other"; break;
                    case "manifest-rid": manifest["runtimeIdentifier"] = "win-arm64"; break;
                    case "manifest-revision": manifest["sourceRevision"] = "wrong"; break;
                    case "manifest-protocol": manifest["protocolPackage"] = "wrong"; break;
                    case "manifest-bundled": manifest["spatialAnalyzerBundled"] = true; break;
                    default: throw new InvalidOperationException(defect);
                }
                File.WriteAllText(receiptPath, receipt.ToJsonString());
                File.WriteAllText(manifestPath, manifest.ToJsonString());
                break;
        }
        Assert.Throws<BriosaStartupException>(() => Resolve());
        Assert.Equal(Install(Machine), Resolve());
    }

    [Fact]
    public void OtherVersionsTargetsAndUncommittedProductsAreNotDiscovered()
    {
        var server = Install(User);
        var product = Path.GetFullPath(Path.Combine(server, "..", ".."));
        var other = product + "-other";
        Directory.Move(product, other);
        Assert.Throws<BriosaStartupException>(() => Resolve());
        var staging = Path.Combine(User, "Briosa", "Packages", "transactions", "pending", Id);
        Directory.CreateDirectory(Path.GetDirectoryName(staging)!);
        Directory.Move(other, staging);
        Assert.Throws<BriosaStartupException>(() => Resolve());
        Assert.Throws<BriosaStartupException>(() => ServerDiscovery.ResolveExecutablePath(null, Local, "", ""));
        Assert.Throws<BriosaStartupException>(() => ServerDiscovery.ResolveExecutablePath(null, Local, "relative", "relative"));
    }

    private static string Install(string root)
    {
        var product = Path.Combine(root, "Briosa", "Packages", "products", Id);
        var payload = Path.Combine(product, "payload");
        var server = Touch(Path.Combine(payload, "Briosa.Server.exe"));
        Touch(Path.Combine(payload, "Briosa.Worker.exe"));
        File.WriteAllText(Path.Combine(payload, "manifest.json"), JsonSerializer.Serialize(new
        {
            schemaVersion = 2,
            artifactName = Id,
            briosaVersion = Identity.BriosaVersion,
            spatialAnalyzerTarget = Identity.SpatialAnalyzerTarget,
            runtimeIdentifier = "win-x64",
            sourceRevision = Identity.SourceRevision,
            protocolPackage = "briosa",
            spatialAnalyzerBundled = false,
        }));
        File.WriteAllText(Path.Combine(product, "receipt.json"), JsonSerializer.Serialize(new
        {
            schemaVersion = 1,
            package = new
            {
                id = Id,
                component = "server",
                version = Identity.BriosaVersion,
                spatialAnalyzerTarget = Identity.SpatialAnalyzerTarget,
                runtimeIdentifier = "win-x64"
            },
            files = RequiredFiles.ToDictionary(name => name, _ => new string('a', 64)),
        }));
        return server;
    }

    private static string Touch(string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, "fixture");
        return path;
    }

    public void Dispose()
    {
        if (Directory.Exists(_root)) Directory.Delete(_root, recursive: true);
    }
}
