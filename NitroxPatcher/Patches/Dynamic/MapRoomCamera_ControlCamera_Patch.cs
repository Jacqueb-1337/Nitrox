using System.Reflection;
using NitroxClient.MonoBehaviours;
using Nitrox.Model.DataStructures;

namespace NitroxPatcher.Patches.Dynamic;

/// <summary>
/// When the local player starts controlling a <see cref="MapRoomCamera"/>, register it with
/// <see cref="MovementBroadcaster"/> so its position/rotation are broadcast to other clients
/// at 30 Hz via the <see cref="Nitrox.Model.Subnautica.Packets.VehicleMovements"/> packet.
///
/// Also removes the <see cref="MapRoomCameraMovementReplicator"/> that was added for receiving
/// remote movement, because the replicator disables <see cref="WorldForces"/> and would conflict
/// with the camera's own physics while the local player is piloting it.
/// </summary>
public sealed partial class MapRoomCamera_ControlCamera_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((MapRoomCamera t) => t.ControlCamera(default));

    public static void Postfix(MapRoomCamera __instance)
    {
        if (!__instance.gameObject.TryGetNitroxId(out NitroxId id))
        {
            return;
        }

        // Remove any incoming-movement replicator — WorldForces and physics must run normally
        // for the local player to actually steer the camera.
        if (__instance.gameObject.TryGetComponent(out MapRoomCameraMovementReplicator replicator))
        {
            UnityEngine.Object.Destroy(replicator);
        }

        MovementBroadcaster.RegisterWatched(__instance.gameObject, id);
    }
}
