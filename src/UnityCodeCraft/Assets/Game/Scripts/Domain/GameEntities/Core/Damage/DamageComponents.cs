using Unity.Entities;

namespace Game
{
    public struct Damage : IComponentData { public int Value; }
    
    [InternalBufferCapacity(4)]
    public struct TakeDamageRequest : IBufferElementData { public int Damage; public Entity Instigator; }
    
    [InternalBufferCapacity(4)]
    public struct TakeDamageEvent : IBufferElementData { public int Damage; public Entity Instigator; }
}