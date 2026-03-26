using System.Reflection;
using NitroxClient.MonoBehaviours;
using Nitrox.Model.DataStructures;

namespace NitroxPatcher.Patches.Dynamic;

/// <summary>
/// When the local player exits a <see cref="MapRoomCamera"/> (energy depleted, manual exit,
/// or camera killed), stop broadcasting its movement and re-attach the
/// <see cref="MapRoomCameraMovementReplicator"/> so that future position updates from the server
/// (e.g., a different player controlling this same camera) can be received and interpolated.
/// </summary>
public sealed partial class MapRoomCamera_FreeCamera_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((MapRoomCamera t) => t.FreeCamera(default));

    public static void Postfix(MapRoomCamera __instance)
    {
        if (!__instance.gameObject.TryGetNitroxId(out NitroxId id))
        {
            return;
        }

        MovementBroadcaster.UnregisterWatched(id);

        // Re-attach the replicator so remote movement can be received again, but only if the
        // camera is still alive. If it's dead/destroyed, adding a component would throw.
        if (__instance.liveMixin.IsAlive())
        {
            __instance.gameObject.AddComponent<MapRoomCameraMovementReplicator>();
        }
    }
}
