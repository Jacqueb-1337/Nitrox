using System.Reflection;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using Nitrox.Model.DataStructures;

namespace NitroxPatcher.Patches.Dynamic;

/// <summary>
/// When a camera docks (player pilots it back into the cradle or it drifts back in),
/// broadcast a metadata update so the server records the new <c>IsDocked: true</c> state.
/// </summary>
public sealed partial class MapRoomCameraDocking_DockCamera_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((MapRoomCameraDocking t) => t.DockCamera(default));

    public static void Postfix(MapRoomCameraDocking __instance, MapRoomCamera camera)
    {
        if (!camera || !camera.gameObject.TryGetNitroxId(out NitroxId id))
        {
            return;
        }

        Resolve<Entities>().EntityMetadataChanged(camera, id);
    }
}
