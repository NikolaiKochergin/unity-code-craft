using Unity.Entities;

namespace Game
{
    public struct DeathEvent : IComponentData, IEnableableComponent { }
    public struct DeathCooldown : IComponentData, IEnableableComponent { public float Time, Duration; }
}