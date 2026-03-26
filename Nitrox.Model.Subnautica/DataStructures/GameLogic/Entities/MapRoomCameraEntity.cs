using System;
using System.Runtime.Serialization;
using BinaryPack.Attributes;
using Nitrox.Model.DataStructures;
using Nitrox.Model.DataStructures.Unity;

namespace Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities;

[Serializable, DataContract]
public class MapRoomCameraEntity : Entity
{
    /// <summary>
    /// Index into <see cref="MapRoomCameraDocking"/> children of the parent <see cref="MapRoomFunctionality"/>.
    /// Identifies which docking slot this camera belongs to.
    /// </summary>
    [DataMember(Order = 1)]
    public int DockIndex { get; set; }

    /// <summary>Whether the camera is currently in its docking cradle.</summary>
    [DataMember(Order = 2)]
    public bool IsDocked { get; set; }

    /// <summary>World-space transform of the camera (only meaningful when <see cref="IsDocked"/> is false).</summary>
    [DataMember(Order = 3)]
    public NitroxTransform WorldTransform { get; set; }

    [IgnoreConstructor]
    protected MapRoomCameraEntity()
    {
        // Constructor for serialization. Has to be "protected" for json serialization.
    }

    public MapRoomCameraEntity(NitroxId id, NitroxId parentId, int dockIndex, bool isDocked, NitroxTransform worldTransform)
    {
        Id = id;
        ParentId = parentId;
        DockIndex = dockIndex;
        IsDocked = isDocked;
        WorldTransform = worldTransform;
    }

    public override string ToString() => $"[MapRoomCameraEntity DockIndex: {DockIndex} IsDocked: {IsDocked} {base.ToString()}]";
}
