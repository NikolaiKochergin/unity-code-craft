using Unity.Entities;

namespace Game
{
    public struct CurrentHealth : IComponentData { public int Value; }
    public struct MaxHealth : IComponentData { public int Value; }
}