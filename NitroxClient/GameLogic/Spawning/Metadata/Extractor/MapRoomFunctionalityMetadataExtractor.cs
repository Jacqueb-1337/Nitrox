using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using NitroxClient.GameLogic.Spawning.Metadata.Extractor.Abstract;

namespace NitroxClient.GameLogic.Spawning.Metadata.Extractor;

public class MapRoomFunctionalityMetadataExtractor : EntityMetadataExtractor<MapRoomFunctionality, MapRoomFunctionalityMetadata>
{
    public override MapRoomFunctionalityMetadata Extract(MapRoomFunctionality entity)
    {
        return new(entity.typeToScan.ToDto());
    }
}
