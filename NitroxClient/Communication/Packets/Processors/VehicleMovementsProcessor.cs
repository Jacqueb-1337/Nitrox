using Nitrox.Model.Subnautica.DataStructures;
using Nitrox.Model.Subnautica.Packets;
using NitroxClient.Communication.Packets.Processors.Core;
using NitroxClient.MonoBehaviours;
using UnityEngine;

namespace NitroxClient.Communication.Packets.Processors;

internal sealed class VehicleMovementsProcessor : IClientPacketProcessor<VehicleMovements>
{
    public Task Process(ClientProcessorContext context, VehicleMovements packet)
    {
        if (!MovementBroadcaster.Instance)
        {
            return Task.CompletedTask;
        }

        foreach (MovementData movementData in packet.Data)
        {
            if (MovementBroadcaster.Instance.Replicators.TryGetValue(movementData.Id, out MovementReplicator movementReplicator))
            {
                // When LargeWorld's cell streaming has deactivated the vehicle's GameObject on this client
                // (because the driver moved it far away), Update() stops running and the transform freezes at
                // the old position. LargeWorld then keeps the GO in the wrong cell indefinitely, so it only
                // re-activates by coincidence when this client loads a cell near the old spawn point.
                // Fix: while the GO is inactive, push the transform to the current server position and re-register
                // the entity with LargeWorld so it correctly tracks which cell the vehicle is in. When this
                // client's cells catch up to the vehicle's actual position, LargeWorld will activate the GO there.
                if (!movementReplicator.gameObject.activeSelf && LargeWorldStreamer.main)
                {
                    movementReplicator.transform.position = movementData.Position.ToUnity();
                    movementReplicator.transform.rotation = movementData.Rotation.ToUnity();
                    if (movementReplicator.gameObject.TryGetComponent(out LargeWorldEntity lwe))
                    {
                        lwe.UpdateCell(LargeWorldStreamer.main);
                    }
                }

                movementReplicator.AddSnapshot(movementData, (float)packet.RealTime);
            }
        }
        return Task.CompletedTask;
    }
}
