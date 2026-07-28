using System.Diagnostics.CodeAnalysis;

namespace Briosa.Client;

/// <summary>Reports a mismatch with the exact protocol/catalog identity pinned by this package.</summary>
[SuppressMessage(
    "Design",
    "CA1032:Implement standard exception constructors",
    Justification = "Instances require a stable compatibility diagnostic code.")]
public sealed class BriosaCompatibilityException : Exception
{
    internal BriosaCompatibilityException(string diagnosticCode)
        : base($"The Briosa server is incompatible with this client ({diagnosticCode}).")
    {
        DiagnosticCode = diagnosticCode;
    }

    /// <summary>Gets a stable, value-free mismatch classification.</summary>
    public string DiagnosticCode { get; }
}
