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
        if (!ServerReleaseVersion.IsValid(version.BriosaVersion))
            throw new BriosaCompatibilityException("server-version-invalid");
        if (version.SourceRevision.Length != 40 ||
            !version.SourceRevision.All(c => c is >= '0' and <= '9' or >= 'a' and <= 'f'))
            throw new BriosaCompatibilityException("server-source-revision-invalid");
        Require(version.ProtocolPackage, Transport.BriosaProtocolIdentity.ProtocolPackage, "server-protocol-package-mismatch");
        Require(version.SpatialAnalyzerTarget, Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget, "server-sa-target-mismatch");
        Require(capabilities.ProtocolPackage, Transport.BriosaProtocolIdentity.ProtocolPackage, "capability-protocol-package-mismatch");
        Require(capabilities.SpatialAnalyzerTarget, Transport.BriosaProtocolIdentity.SpatialAnalyzerTarget, "capability-sa-target-mismatch");
        var contract = serverInfo.Compatibility;
        if (contract is { Major: 0 } || !ServerSelectionPolicy.Compatible(
            contract?.Major ?? 0, contract?.Revision ?? 0, version.BriosaVersion, version.SourceRevision))
            throw new BriosaCompatibilityException("server-contract-incompatible");
        if (serverInfo.TargetIsolationMode != Transport.TargetIsolationMode.SingleTenant)
            throw new BriosaCompatibilityException("target-isolation-mode-mismatch");
    }

    internal static void ValidateInstallation(Transport.GetServerInfoResponse serverInfo, BriosaInstallation? installation)
    {
        if (installation is null) return; // Test transports do not launch a distribution.
        var version = serverInfo.Version ?? throw new BriosaCompatibilityException("server-version-missing");
        Require(version.BriosaVersion, installation.Version, "server-installation-version-mismatch");
        Require(version.SourceRevision, installation.SourceRevision, "server-installation-source-mismatch");
        Require(version.SpatialAnalyzerTarget, installation.SpatialAnalyzerTarget, "server-installation-target-mismatch");
        if ((serverInfo.Compatibility?.Major ?? 0) != installation.ContractMajor ||
            (serverInfo.Compatibility?.Revision ?? 0) != installation.ContractRevision)
            throw new BriosaCompatibilityException("server-installation-contract-mismatch");
    }

    private static void Require(string actual, string expected, string diagnosticCode)
    {
        if (!string.Equals(actual, expected, StringComparison.Ordinal))
            throw new BriosaCompatibilityException(diagnosticCode);
    }
}
