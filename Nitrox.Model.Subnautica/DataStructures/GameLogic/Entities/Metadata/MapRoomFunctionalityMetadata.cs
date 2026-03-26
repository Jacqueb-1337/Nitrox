using System;
using System.Runtime.Serialization;
using BinaryPack.Attributes;

namespace Nitrox.Model.Subnautica.DataStructures.GameLogic.Entities.Metadata;

[Serializable]
[DataContract]
public class MapRoomFunctionalityMetadata : EntityMetadata
{
    [DataMember(Order = 1)]
    public NitroxTechType ScanTarget { get; }

    [IgnoreConstructor]
    protected MapRoomFunctionalityMetadata()
    {
    }

    public MapRoomFunctionalityMetadata(NitroxTechType scanTarget)
    {
        ScanTarget = scanTarget;
    }

    public override string ToString()
    {
        return $"[MapRoomFunctionalityMetadata ScanTarget: {ScanTarget}]";
    }
}
