using Unity.Entities;

namespace Game
{
    public struct ProjectilePrefab : IComponentData { public Entity Value; }
    public struct StoppingDistance : IComponentData { public float Value; }
}