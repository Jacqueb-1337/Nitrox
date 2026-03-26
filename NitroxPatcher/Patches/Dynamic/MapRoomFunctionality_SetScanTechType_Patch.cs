using System.Reflection;
using NitroxClient.GameLogic;
using Nitrox.Model.DataStructures;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class MapRoomFunctionality_StartScanning_Patch : NitroxPatch, IDynamicPatch
{
    public static readonly MethodInfo TARGET_METHOD = Reflect.Method((MapRoomFunctionality t) => t.StartScanning(default));

    public static bool Prefix(MapRoomFunctionality __instance, out TechType __state)
    {
        __state = __instance.typeToScan;
        return true;
    }

    public static void Postfix(MapRoomFunctionality __instance, TechType __state)
    {
        if (__state != __instance.typeToScan && __instance.TryGetIdOrWarn(out NitroxId id))
        {
            Resolve<Entities>().EntityMetadataChanged(__instance, id);
        }
    }
}
