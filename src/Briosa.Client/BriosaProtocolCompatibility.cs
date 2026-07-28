using Briosa.Core.V1Alpha1;

namespace Briosa.Client;

internal static class BriosaProtocolCompatibility
{
    public static void Validate(
        GetServerInfoResponse serverInfo,
        ListCapabilitiesResponse capabilities)
    {
        ArgumentNullException.ThrowIfNull(serverInfo);
        ArgumentNullException.ThrowIfNull(capabilities);

        var version = serverInfo.Version;
        if (version is null)
        {
            throw new BriosaCompatibilityException("server-version-missing");
        }

        Require(version.CoreProtocolPackage, BriosaProtocolIdentity.CoreProtocolPackage, "core-protocol-package-mismatch");
        Require(version.SpatialAnalyzerTarget, BriosaProtocolIdentity.SpatialAnalyzerTarget, "server-sa-target-mismatch");
        Require(version.TargetProtocolPackage, BriosaProtocolIdentity.TargetProtocolPackage, "server-target-package-mismatch");
        Require(version.CatalogRevision, BriosaProtocolIdentity.CatalogRevision, "server-catalog-revision-mismatch");
        Require(capabilities.CatalogId, BriosaProtocolIdentity.CatalogId, "capability-catalog-id-mismatch");
        Require(capabilities.CatalogRevision, BriosaProtocolIdentity.CatalogRevision, "capability-catalog-revision-mismatch");
        Require(capabilities.SpatialAnalyzerTarget, BriosaProtocolIdentity.SpatialAnalyzerTarget, "capability-sa-target-mismatch");
        Require(capabilities.TargetProtocolPackage, BriosaProtocolIdentity.TargetProtocolPackage, "capability-target-package-mismatch");
    }

    private static void Require(string actual, string expected, string diagnosticCode)
    {
        if (!string.Equals(actual, expected, StringComparison.Ordinal))
        {
            throw new BriosaCompatibilityException(diagnosticCode);
        }
    }
}
