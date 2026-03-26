using System;
using System.Runtime.Serialization;
using BinaryPack.Attributes;
using Nitrox.Model.DataStructures.Unity;

namespace Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;

[Serializable, DataContract]
public class MapRoomCameraMetadata : EntityMetadata
{
    [DataMember(Order = 1)]
    public bool IsDocked { get; }

    [DataMember(Order = 2)]
    public NitroxVector3 Position { get; }

    [DataMember(Order = 3)]
    public NitroxQuaternion Rotation { get; }

    [IgnoreConstructor]
    protected MapRoomCameraMetadata()
    {
    }

    public MapRoomCameraMetadata(bool isDocked, NitroxVector3 position, NitroxQuaternion rotation)
    {
        IsDocked = isDocked;
        Position = position;
        Rotation = rotation;
    }

    public override string ToString() => $"[MapRoomCameraMetadata IsDocked: {IsDocked} Position: {Position}]";
}
