using Unity.Entities;

namespace Game
{
    public struct UnitSpawnPoint : IComponentData { }
    public struct UnitSpawnPointCount : IComponentData { public int Value; }
    public struct UnitSpawnRequest : IComponentData, IEnableableComponent { public Entity Prefab; }
}