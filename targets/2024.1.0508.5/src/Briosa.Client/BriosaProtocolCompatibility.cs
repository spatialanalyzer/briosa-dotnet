using Transport = Briosa.Client.Transport;

namespace Briosa;

internal static class BriosaProtocolCompatibility
{
    public static void Validate(
        Transport.GetServerInfoResponse serverInfo,
        Transport.ListCapabilitiesResponse capabilities)
    {
        ArgumentNullException.ThrowIfNull(serverInfo);
        ArgumentNullException.ThrowIfNull(capabilities);

        var version = serverInfo.Version ??
            throw new BriosaCompatibilityException("server-version-missing");
        Require(version.BriosaVersion, Transport.BriosaProtocolIdentity.BriosaVersion, "server-version-mismatch");
        Require(version.SourceRevision, Transport.BriosaProtocolIdentity.SourceRevision, "server-source-revision-mismatch");
        Require(version.ProtocolPackage, Transport.BriosaProtocolIdentity.ProtocolPackage, "server-protocol-package-mismatch");
        Require(version.SpatialAnalyzerTarget, Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget, "server-sa-target-mismatch");
        Require(capabilities.ProtocolPackage, Transport.BriosaProtocolIdentity.ProtocolPackage, "capability-protocol-package-mismatch");
        Require(capabilities.SpatialAnalyzerTarget, Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget, "capability-sa-target-mismatch");
        if (serverInfo.TargetIsolationMode != Transport.TargetIsolationMode.SingleTenant)
        {
            throw new BriosaCompatibilityException("target-isolation-mode-mismatch");
        }
    }

    private static void Require(string actual, string expected, string diagnosticCode)
    {
        if (!string.Equals(actual, expected, StringComparison.Ordinal))
        {
            throw new BriosaCompatibilityException(diagnosticCode);
        }
    }
}
