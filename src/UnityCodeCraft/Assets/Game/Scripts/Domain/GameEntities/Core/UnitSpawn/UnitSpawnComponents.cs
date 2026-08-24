using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct UnitSpawnPosition : IComponentData { public float3 Value; }
    public struct UnitSpawnPointCount : IComponentData { public int Value; }
    public struct UnitBuyRequest : IComponentData, IEnableableComponent { public FixedString32Bytes PrefabName; }
    public struct UnitSpawnRequest : IComponentData, IEnableableComponent { public Entity Prefab; }
}