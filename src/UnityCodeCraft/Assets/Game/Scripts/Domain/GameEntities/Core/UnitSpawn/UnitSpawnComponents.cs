using Unity.Entities;

namespace Game
{
    public struct UnitSpawnPoint : IComponentData { }
    public struct UnitSpawnRequest : IComponentData, IEnableableComponent { public Entity Prefab; }
}