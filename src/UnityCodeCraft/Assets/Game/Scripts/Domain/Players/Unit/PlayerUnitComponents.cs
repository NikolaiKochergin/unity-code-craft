using Unity.Collections;
using Unity.Entities;

namespace Game
{
    public struct UnitConfig : IComponentData { public FixedString32Bytes Name; }
    public struct UnitPrice : IComponentData { public int Value; }
    public struct UnitPrefab : IComponentData { public Entity Value; }
}