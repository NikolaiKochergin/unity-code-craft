using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct UnitSpawnPoint : IComponentData { }
    public struct UnitSpawnPointPosition : IComponentData { public float3 Value; }
    public struct UnitSpawnPointCount : IComponentData { public int Value; }
    public struct UnitSpawnRequest : IComponentData, IEnableableComponent { public Entity Prefab; }
}