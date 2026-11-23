namespace Briosa;

/// <summary>
/// Which step of the server's active admission policy denied a call, an
/// operation, or a request option. A later protocol value maps to
/// <see cref="Unknown"/>.
/// </summary>
/// <remarks>
/// Each denied step names the same step as the <see cref="OperationAdmission"/>
/// value with the same suffix, for example <see cref="Flag"/> and
/// <see cref="OperationAdmission.DeniedFlag"/>.
/// </remarks>
public enum PolicyDenialRule
{
    /// <summary>A value that this client does not know. It is unknown metadata, never permission.</summary>
    Unknown = -1,

    /// <summary>The server sent no rule.</summary>
    Unspecified = 0,

    /// <summary>
    /// The profile, widened by flags set to allow, does not admit every risk
    /// flag, or the read-only effect filter excludes the operation.
    /// </summary>
    Profile = 1,

    /// <summary>A risk flag is set to deny.</summary>
    Flag = 2,

    /// <summary>A per-operation override is set to deny.</summary>
    Override = 3,

    /// <summary>
    /// An exclusive workflow, which no setting admits in the current isolation
    /// mode. A request whose options leave work running after the call returns
    /// is one.
    /// </summary>
    Exclusive = 4,

    /// <summary>
    /// The operation's classification metadata is not reviewed, or the request
    /// cannot be read against the operation's reviewed request options.
    /// </summary>
    Unreviewed = 5,
}

/// <summary>
/// The value-free detail of a policy denial that Server 0.10.1 and later send
/// with a denied call, a denied capability, and a denied request option. It holds
/// profile, flag, and field names only, never a request value.
/// </summary>
/// <remarks>
/// Server 0.10.0 sends none, so the properties that carry it are
/// <see langword="null"/> there. The detail is informational: an absent,
/// inconsistent, or unrecognized detail never changes a failure's kind,
/// disposition, or guidance, or a capability's admission.
/// </remarks>
public sealed record PolicyDenialDetails
{
    /// <summary>Gets the policy step that denied.</summary>
    public PolicyDenialRule Rule { get; init; }

    /// <summary>
    /// Gets the risk flag set to deny that decided a <see cref="PolicyDenialRule.Flag"/>
    /// denial, or <see langword="null"/> when the server named none.
    /// </summary>
    /// <remarks>
    /// When several flags are set to deny, the server names the first in risk
    /// flag declaration order. A flag name that this client does not know is
    /// <see cref="OperationRiskFlag.Unknown"/>; its text is in
    /// <see cref="DeniedFlagName"/>.
    /// </remarks>
    public OperationRiskFlag? DeniedFlag { get; init; }

    /// <summary>
    /// Gets the flag name exactly as the server sent it, the lowercase suffix of
    /// the protobuf risk flag value name such as <c>physical_motion</c>, or
    /// <see langword="null"/> when the server named none.
    /// </summary>
    public string? DeniedFlagName { get; init; }

    /// <summary>
    /// Gets the active admission profile for a <see cref="PolicyDenialRule.Profile"/>
    /// denial, such as <c>standard</c>, or <see langword="null"/> when the server
    /// named none. A profile denial names no flag, because a profile can exclude
    /// a request for more than one reason.
    /// </summary>
    public string? AdmissionProfile { get; init; }

    /// <summary>
    /// Gets the protobuf field name of the request option that the server
    /// attributed the denial to, such as <c>show_results_dialog</c>, or
    /// <see langword="null"/> when the operation's reviewed row decided.
    /// </summary>
    /// <remarks>
    /// When a request turns on several options with the deciding effect, the
    /// server names the first in its reviewed order, which need not be the only
    /// cause.
    /// </remarks>
    public string? RequestOptionField { get; init; }
}
