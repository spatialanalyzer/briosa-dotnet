namespace Briosa;

/// <summary>How the MP command itself completed.</summary>
public enum MpExecutionState
{
    /// <summary>No state was reported.</summary>
    Unspecified,

    /// <summary>The MP command succeeded.</summary>
    Succeeded,

    /// <summary>SpatialAnalyzer rejected the MP step before it ran.</summary>
    ExecuteStepRejected,

    /// <summary>The MP command ran and reported failure.</summary>
    Failed,

    /// <summary>The MP result could not be read.</summary>
    ResultUnavailable,

    /// <summary>SpatialAnalyzer rejected an MP argument.</summary>
    ArgumentRejected,

    /// <summary>
    /// SpatialAnalyzer returned MP code 4 (PARTIAL SUCCESS): the command did its
    /// work but did not have its full intended effect. A call that completes this
    /// way is a success and is never replayed.
    /// </summary>
    PartialSuccess,
}

/// <summary>What happened when Briosa read one result-only argument.</summary>
public enum OutputRetrievalState
{
    /// <summary>No state was reported.</summary>
    Unspecified,

    /// <summary>The output was read.</summary>
    Retrieved,

    /// <summary>Briosa did not try to read the output.</summary>
    NotAttempted,

    /// <summary>Reading the output failed.</summary>
    Failed,
}

/// <summary>The retrieval of one operation output.</summary>
public sealed record OutputRetrieval
{
    /// <summary>Gets the result field's protocol name, for example <c>directory</c>.</summary>
    public required string FieldName { get; init; }

    /// <summary>Gets what happened when Briosa read the output.</summary>
    public required OutputRetrievalState State { get; init; }

    /// <summary>Gets the stable diagnostic code of a failed retrieval, if any.</summary>
    public string? DiagnosticCode { get; init; }
}

/// <summary>
/// How the MP command of one successful call completed. Pass an
/// <c>onMpCompletion</c> observer to an MP method to receive it.
/// </summary>
/// <remarks>
/// <see cref="State"/> is <see cref="MpExecutionState.Succeeded"/>, or <see
/// cref="MpExecutionState.PartialSuccess"/> when SpatialAnalyzer returned MP code 4:
/// the command did its work and the call returned its outputs, but the command
/// did not have its full intended effect.
/// </remarks>
public sealed record MpCompletion
{
    /// <summary>Gets how the MP command completed.</summary>
    public required MpExecutionState State { get; init; }

    /// <summary>Gets the MP result code, when SpatialAnalyzer returned one.</summary>
    public int? MpResultCode { get; init; }
}

/// <summary>
/// The MP execution details that accompany a typed operation failure that is
/// related to the MP command.
/// </summary>
public sealed record MpExecutionDetails
{
    /// <summary>Gets how the MP command completed.</summary>
    public required MpExecutionState State { get; init; }

    /// <summary>Gets the MP result code, when SpatialAnalyzer returned one.</summary>
    public int? MpResultCode { get; init; }

    /// <summary>Gets the retrieval of each output, in result field order.</summary>
    public required IReadOnlyList<OutputRetrieval> OutputRetrievals { get; init; }
}
