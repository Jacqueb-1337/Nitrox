using Nitrox.Model.Subnautica.Packets;

namespace NitroxClient.MonoBehaviours;

/// <summary>
/// Receives movement snapshots for a remote player's <see cref="MapRoomCamera"/> and
/// interpolates the drone to the correct world position.  Position and rotation are
/// handled entirely by the base <see cref="MovementReplicator.Update"/> loop; cameras
/// have no additional driven state (steering wheel, throttle, etc.) to apply.
/// </summary>
public class MapRoomCameraMovementReplicator : MovementReplicator
{
    public override void ApplyNewMovementData(MovementData newMovementData)
    {
        // Intentionally empty: the base movementReplicator already sets transform.position
        // and transform.rotation via lerp. Cameras have no extra physics state to mirror.
    }
}
