using System.Text.Json;
using Identity = Briosa.Client.Transport.BriosaProtocolIdentity;

namespace Briosa;

internal static class ServerDiscovery
{
    internal static string ResolveExecutablePath() => ResolveExecutablePath(
        Environment.GetEnvironmentVariable(BriosaServerLauncher.ServerPathEnvironmentVariable),
        AppContext.BaseDirectory,
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

    internal static string ResolveExecutablePath(
        string? configured, string baseDirectory, string localAppData, string commonAppData)
    {
        foreach (var candidate in new[]
        {
            Executable(configured),
            Executable(Path.Combine(baseDirectory, "briosa-server", "Briosa.Server.exe")),
        })
        {
            if (candidate is not null) return candidate;
        }

        foreach (var root in new[] { localAppData, commonAppData })
        {
            if (!Path.IsPathFullyQualified(root)) continue;
            var candidate = ManagedExecutable(Path.Combine(root, "Briosa", "Packages"));
            if (candidate is not null) return candidate;
        }

        if (Path.IsPathFullyQualified(localAppData))
        {
            var legacy = Executable(Path.Combine(localAppData, "Briosa", "servers",
                Identity.BriosaVersion, $"sa-{Identity.SpatialAnalyzerTarget}", "Briosa.Server.exe"));
            if (legacy is not null) return legacy;
        }
        throw new BriosaStartupException("server-distribution-not-found");
    }

    private static string? Executable(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return null;
        try
        {
            var fullPath = Path.GetFullPath(path);
            return string.Equals(Path.GetFileName(fullPath), "Briosa.Server.exe",
                StringComparison.OrdinalIgnoreCase) && File.Exists(fullPath) ? fullPath : null;
        }
        catch (Exception exception) when (exception is ArgumentException or IOException or NotSupportedException)
        {
            return null;
        }
    }

    private static string? ManagedExecutable(string store)
    {
        var id = $"briosa-{Identity.BriosaVersion}-sa-{Identity.SpatialAnalyzerTarget}-win-x64";
        var product = Path.Combine(store, "products", id);
        var payload = Path.Combine(product, "payload");
        try
        {
            using var receiptDocument = JsonDocument.Parse(File.ReadAllText(Path.Combine(product, "receipt.json")));
            using var manifestDocument = JsonDocument.Parse(File.ReadAllText(Path.Combine(payload, "manifest.json")));
            var receipt = receiptDocument.RootElement;
            var manifest = manifestDocument.RootElement;
            var package = Property(receipt, "package");
            if (!Schema(receipt, 1) || !Schema(manifest, 2) ||
                !Matches(package, "id", id) || !Matches(package, "component", "server") ||
                !Matches(package, "version", Identity.BriosaVersion) ||
                !Matches(package, "runtimeIdentifier", "win-x64") ||
                !Matches(package, "spatialAnalyzerTarget", Identity.SpatialAnalyzerTarget) ||
                !Matches(manifest, "artifactName", id) ||
                !Matches(manifest, "briosaVersion", Identity.BriosaVersion) ||
                !Matches(manifest, "spatialAnalyzerTarget", Identity.SpatialAnalyzerTarget) ||
                !Matches(manifest, "runtimeIdentifier", "win-x64") ||
                !Matches(manifest, "sourceRevision", Identity.SourceRevision) ||
                !Matches(manifest, "protocolPackage", "briosa") ||
                Property(manifest, "spatialAnalyzerBundled").ValueKind != JsonValueKind.False)
                return null;

            var files = Property(receipt, "files");
            foreach (var name in new[] { "manifest.json", "Briosa.Server.exe", "Briosa.Worker.exe" })
            {
                var digest = Property(files, name);
                if (digest.ValueKind != JsonValueKind.String ||
                    digest.GetString() is not { Length: 64 } hash ||
                    !hash.All(character => character is >= '0' and <= '9' or >= 'a' and <= 'f') ||
                    !File.Exists(Path.Combine(payload, name))) return null;
            }
            return Path.GetFullPath(Path.Combine(payload, "Briosa.Server.exe"));
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or JsonException)
        {
            return null;
        }
    }

    private static JsonElement Property(JsonElement element, string name) =>
        element.ValueKind == JsonValueKind.Object && element.TryGetProperty(name, out var value) ? value : default;

    private static bool Schema(JsonElement element, int expected)
    {
        var value = Property(element, "schemaVersion");
        return value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out var version) && version == expected;
    }

    private static bool Matches(JsonElement element, string name, string expected)
    {
        var value = Property(element, name);
        return value.ValueKind == JsonValueKind.String && value.GetString() == expected;
    }
}
