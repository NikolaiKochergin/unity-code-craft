using Unity.Entities;

namespace Game
{
    public struct DetectionRadius : IComponentData { public float Value; }
    public struct DetectionCooldown : IComponentData { public float Time, Duration; }
}