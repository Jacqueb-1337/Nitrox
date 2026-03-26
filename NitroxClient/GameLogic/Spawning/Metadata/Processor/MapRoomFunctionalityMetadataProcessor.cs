using NitroxClient.Communication;
using NitroxClient.GameLogic.Spawning.Metadata.Processor.Abstract;
using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;
using Nitrox.Model.Subnautica.Packets;
using UnityEngine;

namespace NitroxClient.GameLogic.Spawning.Metadata.Processor;

public class MapRoomFunctionalityMetadataProcessor : EntityMetadataProcessor<MapRoomFunctionalityMetadata>
{
    public override void ProcessMetadata(GameObject gameObject, MapRoomFunctionalityMetadata metadata)
    {
        if (!gameObject.TryGetComponent(out MapRoomFunctionality mapRoomFunctionality))
        {
            Log.ErrorOnce($"[{nameof(MapRoomFunctionalityMetadataProcessor)}] Could not find {nameof(MapRoomFunctionality)} on {gameObject}");
            return;
        }

        using (PacketSuppressor<EntityMetadataUpdate>.Suppress())
        {
            mapRoomFunctionality.StartScanning(metadata.ScanTarget.ToUnity());
        }
    }
}
