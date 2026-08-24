using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct UnitSpawnPoint : IBufferElementData { public float3 Position; }
    public struct UnitBuyRequest : IComponentData, IEnableableComponent { public FixedString32Bytes PrefabName; }
    public struct UnitSpawnRequest : IComponentData, IEnableableComponent { public Entity Prefab; }
}