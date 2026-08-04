using Unity.Entities;

namespace Game
{
    public struct FireRequest : IComponentData, IEnableableComponent { public Entity Target; }
    public struct FireEvent : IComponentData, IEnableableComponent { }
    public struct FireCooldown : IComponentData { public float Time, Duration; }
    public struct FireDelay : IComponentData { public float Time, Duration; }
    public struct FireOffset : IComponentData { public float Value; }
}