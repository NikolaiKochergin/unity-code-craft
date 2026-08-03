using Unity.Entities;

namespace Game
{
    [InternalBufferCapacity(4)]
    public struct TakeDamageRequest : IBufferElementData { public int Damage; public Entity Instigator; }
    
    [InternalBufferCapacity(4)]
    public struct TakeDamageEvent : IBufferElementData { public int Damage; public Entity Instigator; }
}