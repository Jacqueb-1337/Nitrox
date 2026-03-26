using System.Reflection;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using Nitrox.Model.DataStructures;
using Nitrox.Model.DataStructures.Unity;
using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic;

/// <summary>
/// When a <see cref="MapRoomCameraDocking"/> creates its camera drone on Start, assign it a
/// <see cref="NitroxEntity"/> and broadcast the entity to the server so that clients who join
/// later can restore it. This fires only when the local player just built the scanner room
/// (the natural guard is: the parent <see cref="MapRoomFunctionality"/> must already have a
/// NitroxId, which is set in the same frame as construction completes — before this Start
/// fires on the next frame). Joining clients execute this path too but their
/// <see cref="MapRoomFunctionality"/> has no NitroxId yet at that point, so the early return
/// below silently skips them; the spawner (<see cref="MapRoomCameraEntitySpawner"/>) handles
/// ID assignment for joining clients a moment later via <c>RestoreMapRoom</c>.
/// </summary>
public sealed partial class MapRoomCameraDocking_Start_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((MapRoomCameraDocking t) => t.Start());

    public static void Postfix(MapRoomCameraDocking __instance)
    {
        MapRoomCamera camera = __instance.camera;
        if (!camera)
        {
            // deserialized == true path: camera was loaded from save, Start() skipped creation.
            return;
        }

        // If a NitroxId was already assigned (e.g., by MapRoomCameraEntitySpawner), don't re-broadcast.
        if (camera.gameObject.TryGetNitroxId(out _))
        {
            return;
        }

        // The MapRoomFunctionality parent must have a NitroxId before we can parent this camera.
        // On joining clients the NitroxId is assigned later (RestoreMapRoom), so Start() fires
        // before the ID is set — this guard returns early and the spawner handles it instead.
        MapRoomFunctionality mapRoomFunctionality = __instance.GetComponentInParent<MapRoomFunctionality>(true);
        if (!mapRoomFunctionality || !mapRoomFunctionality.TryGetNitroxId(out NitroxId mapRoomId))
        {
            return;
        }

        MapRoomCameraDocking[] allDocks = mapRoomFunctionality.GetComponentsInChildren<MapRoomCameraDocking>(true);
        int dockIndex = System.Array.IndexOf(allDocks, __instance);

        NitroxId cameraId = new();
        NitroxEntity.SetNewId(camera.gameObject, cameraId);

        NitroxTransform worldTransform = new()
        {
            LocalPosition = camera.transform.position.ToDto(),
            LocalRotation = camera.transform.rotation.ToDto(),
            LocalScale = camera.transform.localScale.ToDto()
        };

        MapRoomCameraEntity cameraEntity = new(cameraId, mapRoomId, dockIndex, isDocked: true, worldTransform);
        Resolve<Entities>().BroadcastEntitySpawnedByClient(cameraEntity);
    }
}
