using Unity.Entities;

namespace Game
{
    public struct FireRequest : IComponentData, IEnableableComponent { public Entity Target; }
    public struct FireEvent : IComponentData, IEnableableComponent { }
    public struct FireCooldown : IComponentData, IEnableableComponent { public float Time, Duration; }
    public struct FireOffset : IComponentData, IEnableableComponent { public float Value; }
}