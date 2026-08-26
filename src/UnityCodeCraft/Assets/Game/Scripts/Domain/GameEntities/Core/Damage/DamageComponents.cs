using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public struct Damage : IComponentData { public int Value; }
    public struct DamageRadius : IComponentData { public float Value; }
    public struct AreaDamageRequest : IComponentData, IEnableableComponent { public float3 Position; public Entity Instigator; }
    
    [InternalBufferCapacity(4)]
    public struct TakeDamageRequest : IBufferElementData { public int Damage; public Entity Instigator; }
    
    [InternalBufferCapacity(4)]
    public struct TakeDamageEvent : IBufferElementData { public int Damage; public Entity Instigator; }
}