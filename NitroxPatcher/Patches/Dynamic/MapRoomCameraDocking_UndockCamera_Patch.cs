using System.Reflection;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using Nitrox.Model.DataStructures;

namespace NitroxPatcher.Patches.Dynamic;

/// <summary>
/// When a camera is undocked (player drives it out, picks it up, or a stalker grabs it),
/// broadcast a metadata update so the server records <c>IsDocked: false</c> and the
/// camera's world position. Uses a Prefix to capture the camera reference before
/// <see cref="MapRoomCameraDocking.UndockCamera"/> nulls <c>__instance.camera</c>.
/// </summary>
public sealed partial class MapRoomCameraDocking_UndockCamera_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((MapRoomCameraDocking t) => t.UndockCamera());

    public static void Prefix(MapRoomCameraDocking __instance, out MapRoomCamera __state)
    {
        __state = __instance.camera;
    }

    public static void Postfix(MapRoomCameraDocking __instance, MapRoomCamera __state)
    {
        if (!__state || !__state.gameObject.TryGetNitroxId(out NitroxId id))
        {
            return;
        }

        Resolve<Entities>().EntityMetadataChanged(__state, id);
    }
}
