using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct MoveSpeed : IComponentData { public float Value; }
    public struct MoveRequest : IComponentData, IEnableableComponent { public float3 Direction; }
    public struct MoveEvent : IComponentData, IEnableableComponent { }
}