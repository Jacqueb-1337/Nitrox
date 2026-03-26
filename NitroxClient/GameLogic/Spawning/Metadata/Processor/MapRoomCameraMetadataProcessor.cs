using NitroxClient.Communication;
using NitroxClient.GameLogic.Spawning.Metadata.Processor.Abstract;
using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using Nitrox.Model.Subnautica.Packets;
using UnityEngine;

namespace NitroxClient.GameLogic.Spawning.Metadata.Processor;

public class MapRoomCameraMetadataProcessor : EntityMetadataProcessor<MapRoomCameraMetadata>
{
    public override void ProcessMetadata(GameObject gameObject, MapRoomCameraMetadata metadata)
    {
        if (!gameObject.TryGetComponent(out MapRoomCamera camera))
        {
            Log.ErrorOnce($"[{nameof(MapRoomCameraMetadataProcessor)}] Could not find {nameof(MapRoomCamera)} on {gameObject.name}");
            return;
        }

        using (PacketSuppressor<EntityMetadataUpdate>.Suppress())
        {
            if (!metadata.IsDocked)
            {
                // Camera was deployed — undock it and restore its world position.
                if (camera.dockingPoint)
                {
                    camera.dockingPoint.UndockCamera();
                }
                camera.transform.position = metadata.Position.ToUnity();
                camera.transform.rotation = metadata.Rotation.ToUnity();
            }
            // IsDocked: true — camera is already in its cradle after spawning; nothing to do.
        }
    }
}
