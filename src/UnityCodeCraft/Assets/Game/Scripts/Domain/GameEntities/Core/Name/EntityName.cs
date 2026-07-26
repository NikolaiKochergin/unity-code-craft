using Unity.Collections;
using Unity.Entities;

namespace Game
{
    public struct EntityName : IComponentData { public FixedString64Bytes value; }
}