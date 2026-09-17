#pragma warning disable CA2000 // Test doubles are owned and disposed by the client session.
using Transport = Briosa.Client.Transport;

namespace Briosa.Client.Tests;

public sealed partial class ClientContractTests
{
    [Theory]
    [InlineData("FilterCloudsToVectorGroupsResolvePointsAsync", "includeProximityPoints")]
    [InlineData("ReComputeCalculatedItemsAsync", "refreshFilteredCloudData")]
    [InlineData("MakeCylinderFitProfileAsync", "constrainToNominalAxis")]
    [InlineData("MakeCylinderFitProfileAsync", "alignWithNominal")]
    [InlineData("DoRelationshipFitAsync", "enableRandomizedStart")]
    [InlineData("AutoFilterCloudsToNominalGeometry2DAsync", "useFeatureSpecificFilterSettings")]
    [InlineData("AutoFilterCloudsToNominalGeometry3DAsync", "useFeatureSpecificFilterSettings")]
    [InlineData("SetGeomRelationshipAutoVectorsNominalAvnAsync", "vectorGroupCustomPrefix")]
    [InlineData("SetRelationshipAutoVectorsFitAvfAsync", "vectorGroupCustomPrefix")]
    [InlineData("ConstructPointCloudFromExistingCloudsAsync", "setCloudPointRgbFromVoxels")]
    [InlineData("ExportAsciiPointCloudsAsync", "includeCloudPointLabeling")]
    [InlineData("SetFeatureCheckReportingOptionsAsync", "onlyCreateFailedVectors")]
    public void LegacySignaturesOmitLaterInputs(string methodName, string parameterName)
    {
        var methods = new[] { typeof(BriosaClient), typeof(BriosaConstructionOperations), typeof(BriosaGdtOperations) }.SelectMany(t => t.GetMethods());
        var method = Assert.Single(methods, m => m.Name == methodName);
        Assert.DoesNotContain(method.GetParameters(), p => p.Name == parameterName);
    }

    [Fact]
    public async Task LegacySurfaceConstructionPreservesSevenExplicitFalseValues()
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        await client.ConstructObjectsFromSurfaceFacesRuntimeSelectAsync(false, false, false, false, false, false, false);
        var request = Assert.IsType<Transport.ConstructObjectsFromSurfaceFacesRuntimeSelectRequest>(transport.LastOperationRequest);
        Assert.True(request.HasConstructPlanes && request.HasConstructCylinders && request.HasConstructSpheres &&
            request.HasConstructCones && request.HasConstructLines && request.HasConstructPoints && request.HasConstructCircles);
        Assert.False(request.ConstructPlanes || request.ConstructCylinders || request.ConstructSpheres ||
            request.ConstructCones || request.ConstructLines || request.ConstructPoints || request.ConstructCircles);
        Assert.All(typeof(BriosaClient).GetMethod(nameof(BriosaClient.ConstructObjectsFromSurfaceFacesRuntimeSelectAsync))!
            .GetParameters().Where(p => p.ParameterType == typeof(bool)), p => Assert.False(p.IsOptional));
    }

    [Fact]
    public async Task LegacyInstrumentCommandsPreserveInputsAndMaterializeOnce()
    {
        var transport = new FakeTransport();
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var instrument = new CollectionInstrumentId { CollectionName = "Inspection", InstrumentId = 2 };
        await client.RunCribSheetAsync(new CollectionName { Name = "Inspection" }, "", instrument);
        var crib = Assert.IsType<Transport.RunCribSheetRequest>(transport.LastOperationRequest);
        Assert.True(crib.HasCribSheetName);
        Assert.Equal("", crib.CribSheetName);
        Assert.Equal(2, crib.Instrument.InstrumentId);

        var enumerations = 0;
        IEnumerable<CollectionObjectName> Objects()
        {
            enumerations++;
            yield return new CollectionObjectName { CollectionName = "Inspection", ObjectName = "F1", ObjectType = ObjectType.Frame };
            yield return new CollectionObjectName { CollectionName = "Inspection", ObjectName = "F1", ObjectType = ObjectType.Frame };
        }
        await client.ProjectObjectsAsync(instrument, Objects());
        var projection = Assert.IsType<Transport.ProjectObjectsRequest>(transport.LastOperationRequest);
        Assert.Equal(1, enumerations);
        Assert.Equal(2, projection.ObjectsToProject.Count);
        Assert.Equal(projection.ObjectsToProject[0], projection.ObjectsToProject[1]);
        await client.ProjectObjectsAsync(instrument, []);
        Assert.Empty(Assert.IsType<Transport.ProjectObjectsRequest>(transport.LastOperationRequest).ObjectsToProject);
        await client.StopProjectionAsync(instrument);
        Assert.Equal("StopProjection", transport.LastOperationRpc);
        Assert.Equal(2, Assert.IsType<Transport.StopProjectionRequest>(transport.LastOperationRequest).Instrument.InstrumentId);
    }

    [Fact]
    public async Task LegacyStatisticsRetainSignedMaxWithoutAssumingAbsoluteValue()
    {
        var transport = new FakeTransport
        {
            OperationResponse = new Transport.GetGeneralRelationshipStatisticsResult
            {
                MaxDeviation = -2,
                Rms = 1,
                HasSignedDeviation = false,
                SignedMaxDeviation = 0,
                SignedMinDeviation = 0,
            },
        };
        await using var client = CreateClient(new FakeServerLauncher(), transport);
        await client.StartAsync();
        var result = await client.GetGeneralRelationshipStatisticsAsync(new CollectionItemName { CollectionName = "Inspection", ItemName = "R1" });
        Assert.Equal(-2, result.MaxDeviation);
        transport.OperationResponse = new Transport.GetGeneralRelationshipStatisticsResult { Rms = 1 };
        await Assert.ThrowsAsync<BriosaProtocolException>(() => client.GetGeneralRelationshipStatisticsAsync(
            new CollectionItemName { CollectionName = "Inspection", ItemName = "R1" }));
    }

    [Fact]
    public void LegacyChoicesRetainWireGapsAndRequiredCadPresence()
    {
        Assert.False(Enum.IsDefined(typeof(ObjectType), 5));
        Assert.False(Enum.IsDefined(typeof(ItemType), 10));
        Assert.Equal(6, (int)ObjectType.ScanStripeCloud);
        Assert.Equal(11, (int)ItemType.ScanStripeCloud);
        Assert.Equal(11, (int)SystemString.UserName);
        Assert.False(Enum.IsDefined(typeof(SystemString), 12));
        var parameter = typeof(BriosaClient).GetMethod(nameof(BriosaClient.DirectCadAccessAsync))!
            .GetParameters().Single(p => p.Name == "surfaceCompatibilityMode");
        Assert.False(parameter.IsOptional);
    }

    [Fact]
    public void LegacyPackageRejects2026ServerIdentity()
    {
        var info = new Transport.GetServerInfoResponse
        {
            Version = new Transport.VersionCoordinates
            {
                BriosaVersion = Transport.BriosaProtocolIdentity.BriosaVersion,
                SourceRevision = Transport.BriosaProtocolIdentity.SourceRevision,
                ProtocolPackage = "briosa",
                SpatialAnalyzerTarget = "2026.1.0529.7",
            },
        };
        var error = Assert.Throws<BriosaCompatibilityException>(() => BriosaProtocolCompatibility.Validate(info, new Transport.ListCapabilitiesResponse()));
        Assert.Equal("server-sa-target-mismatch", error.DiagnosticCode);
    }
}
