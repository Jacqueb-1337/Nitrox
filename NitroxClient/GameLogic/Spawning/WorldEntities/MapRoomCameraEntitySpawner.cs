using System.Collections;
using NitroxClient.GameLogic.Spawning.Abstract;
using NitroxClient.MonoBehaviours;
using Nitrox.Model.DataStructures;
using Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities;
using UnityEngine;

namespace NitroxClient.GameLogic.Spawning.WorldEntities;

/// <summary>
/// Assigns a <see cref="NitroxEntity"/> to a <see cref="MapRoomCamera"/> that was already created
/// by <see cref="MapRoomCameraDocking.Start"/> and, if the camera was deployed when the world was
/// saved, restores it to its saved world position.
/// </summary>
public class MapRoomCameraEntitySpawner : EntitySpawner<MapRoomCameraEntity>
{
    protected override IEnumerator SpawnAsync(MapRoomCameraEntity entity, TaskResult<Optional<GameObject>> result)
    {
        if (!NitroxEntity.TryGetObjectFrom(entity.ParentId, out GameObject mapRoomGO))
        {
            Log.Error($"[{nameof(MapRoomCameraEntitySpawner)}] Could not find MapRoomFunctionality game object for parent id {entity.ParentId}");
            yield break;
        }

        MapRoomCameraDocking[] dockingPoints = mapRoomGO.GetComponentsInChildren<MapRoomCameraDocking>(true);
        if (entity.DockIndex >= dockingPoints.Length)
        {
            Log.Error($"[{nameof(MapRoomCameraEntitySpawner)}] Dock index {entity.DockIndex} is out of range (found {dockingPoints.Length} docking points)");
            yield break;
        }

        MapRoomCameraDocking dockingPoint = dockingPoints[entity.DockIndex];
        MapRoomCamera camera = dockingPoint.camera;
        if (!camera)
        {
            Log.Warn($"[{nameof(MapRoomCameraEntitySpawner)}] Docking point at index {entity.DockIndex} has no camera (skipping)");
            yield break;
        }

        NitroxEntity.SetNewId(camera.gameObject, entity.Id);

        // Add the replicator so this client can receive movement snapshots while a remote
        // player is piloting the camera. BeginControl removes it (and adds it back on FreeCamera)
        // if the local player ever takes control, to avoid conflicting with local physics.
        camera.gameObject.AddComponent<MapRoomCameraMovementReplicator>();

        if (!entity.IsDocked)
        {
            // Camera was deployed when the session was saved — move it back to its world position.
            dockingPoint.UndockCamera();
            camera.transform.position = entity.WorldTransform.LocalPosition.ToUnity();
            camera.transform.rotation = entity.WorldTransform.LocalRotation.ToUnity();
        }

        result.Set(Optional.Of(camera.gameObject));
    }

    protected override bool SpawnsOwnChildren(MapRoomCameraEntity entity) => false;
}
