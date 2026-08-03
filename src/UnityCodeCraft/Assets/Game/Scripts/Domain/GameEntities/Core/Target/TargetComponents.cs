using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct TargetEntity : IComponentData { public Entity Value; }
    public struct TargetOffset : IComponentData { public float3 Value; }
}