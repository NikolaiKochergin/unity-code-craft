using Unity.Entities;

namespace Game
{
    public struct Mana : IComponentData { public int Value; }
    public struct MaxMana : IComponentData { public int Value; }
    public struct ManaRestoreCooldown : IComponentData, IEnableableComponent { public float Time, Duration; }
}