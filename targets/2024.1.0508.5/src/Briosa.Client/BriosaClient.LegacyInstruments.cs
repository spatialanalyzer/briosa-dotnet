using Transport = Briosa.Client.Transport;

namespace Briosa;

public sealed partial class BriosaClient
{
    /// <summary>Runs the named crib sheet with the selected instrument (Run Crib Sheet).</summary>
    public Task RunCribSheetAsync(
        CollectionName collection,
        string cribSheetName,
        CollectionInstrumentId instrument,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.RunCribSheetRequest(),
            new Dictionary<string, object?>
            {
                ["collection"] = collection,
                ["crib_sheet_name"] = cribSheetName,
                ["instrument"] = instrument,
            });
        return InvokeOperationAsync(
            "briosa.InstrumentOperations", "RunCribSheet", request,
            Transport.RunCribSheetResult.Parser, onMpCompletion, cancellationToken);
    }

    /// <summary>Projects the supplied objects using the selected instrument (Project Objects).</summary>
    public Task ProjectObjectsAsync(
        CollectionInstrumentId instrument,
        IEnumerable<CollectionObjectName> objectsToProject,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.ProjectObjectsRequest(),
            new Dictionary<string, object?>
            {
                ["instrument"] = instrument,
                ["objects_to_project"] = objectsToProject,
            });
        return InvokeOperationAsync(
            "briosa.InstrumentOperations", "ProjectObjects", request,
            Transport.ProjectObjectsResult.Parser, onMpCompletion, cancellationToken);
    }

    /// <summary>Stops projection on the selected instrument (Stop Projection).</summary>
    public Task StopProjectionAsync(
        CollectionInstrumentId instrument,
        Action<MpCompletion>? onMpCompletion = null,
        CancellationToken cancellationToken = default)
    {
        var request = OperationProtocolMapper.BuildRequest(
            new Transport.StopProjectionRequest(),
            new Dictionary<string, object?> { ["instrument"] = instrument });
        return InvokeOperationAsync(
            "briosa.InstrumentOperations", "StopProjection", request,
            Transport.StopProjectionResult.Parser, onMpCompletion, cancellationToken);
    }
}
