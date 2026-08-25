using Unity.Entities;

namespace Game
{
    public struct Mana : IComponentData { public float Value; }
    public struct MaxMana : IComponentData { public float Value; }
    public struct RestoreManaPerSecond : IComponentData { public float Value; }
}