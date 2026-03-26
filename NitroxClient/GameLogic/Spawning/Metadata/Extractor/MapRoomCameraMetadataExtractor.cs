using Nitrox.Model.DataStructures.Unity;
using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using NitroxClient.GameLogic.Spawning.Metadata.Extractor.Abstract;

namespace NitroxClient.GameLogic.Spawning.Metadata.Extractor;

public class MapRoomCameraMetadataExtractor : EntityMetadataExtractor<MapRoomCamera, MapRoomCameraMetadata>
{
    public override MapRoomCameraMetadata Extract(MapRoomCamera camera)
    {
        bool isDocked = camera.dockingPoint;
        return new MapRoomCameraMetadata(isDocked, camera.transform.position.ToDto(), camera.transform.rotation.ToDto());
    }
}
