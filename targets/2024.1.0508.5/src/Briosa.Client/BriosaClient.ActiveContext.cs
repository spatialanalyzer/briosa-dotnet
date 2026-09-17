using Transport = Briosa.Client.Transport;

namespace Briosa;

public sealed partial class BriosaClient
{
    /// <summary>Gets the name of the active SpatialAnalyzer collection.</summary>
    /// <param name="cancellationToken">Cancels the client wait for the operation.</param>
    /// <returns>The currently active collection name.</returns>
    public Task<string> GetActiveCollectionNameAsync(
        CancellationToken cancellationToken = default)
    {
        return InvokeOperationAsync<string>(
            "briosa.ConstructionOperations",
            "GetActiveCollectionName",
            new Transport.GetActiveCollectionNameRequest(),
            Transport.GetActiveCollectionNameResult.Parser,
            cancellationToken);
    }
}
