using SampleGame;
using Unity.Entities;

namespace Game
{
    public struct DetectionTeam : IComponentData { public TeamType TargetTeam; }
    public struct DetectionRadius : IComponentData { public float Value; }
    public struct DetectionCooldown : IComponentData { public float Time, Duration; }
}