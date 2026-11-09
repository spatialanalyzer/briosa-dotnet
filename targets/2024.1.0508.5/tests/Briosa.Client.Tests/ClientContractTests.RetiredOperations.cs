using Google.Protobuf.Reflection;
using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

public sealed partial class ClientContractTests
{
    // Contract major 3 retires these nine operations on both exact targets
    // (spatialanalyzer/briosa#320, #357 and #363). The protocol no longer carries
    // them, and their facades and value types go with them.
    [Theory]
    [InlineData("CloudAndMeshOperations", "DeleteCloudPointsByXYZRange")]
    [InlineData("ConstructionOperations", "MirrorObjects")]
    [InlineData("ConstructionOperations", "ConstructCirclesLinesFromSurfaces")]
    [InlineData("ConstructionOperations", "AddCollectionInstrumentsToRefListWildcardSelection")]
    [InlineData("ConstructionOperations", "MakeCalloutViewRefListWildcardSelection")]
    [InlineData("ConstructionOperations", "SetCalloutViewProperties")]
    [InlineData("ConstructionOperations", "ConstructPolygonizedSurfaceFromPointClouds")]
    [InlineData("ConstructionOperations", "MakeCollectionVectorGroupNameRefListRuntimeSelect")]
    [InlineData("ConstructionOperations", "MakeVectorNameRefListRuntimeSelect")]
    public void RetiredOperationsAreAbsentFromTheProtocolAndThePublicSurface(
        string service,
        string rpc)
    {
        ServiceDescriptor descriptor = service == "CloudAndMeshOperations"
            ? Transport.CloudAndMeshOperations.Descriptor
            : Transport.ConstructionOperations.Descriptor;
        Assert.Null(descriptor.FindMethodByName(rpc));
        Assert.DoesNotContain(
            FacadeTypes().SelectMany(type => type.GetMethods()),
            method => method.Name == rpc + "Async");
    }

    [Theory]
    [InlineData("CircleLineMode")]
    [InlineData("MirrorFramePlane")]
    [InlineData("MeshOrientationType")]
    public void RetiredOperationValueTypesAreRemoved(string typeName) =>
        Assert.DoesNotContain(FacadeTypes(), type => type.Name == typeName);

    // The handwritten public surface. Generated transport types live in
    // Briosa.Client.Transport and are not part of the supported API.
    private static IEnumerable<Type> FacadeTypes() =>
        typeof(BriosaClient).Assembly.GetExportedTypes()
            .Where(type => type.Namespace == "Briosa");
}
