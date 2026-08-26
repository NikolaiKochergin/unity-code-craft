using Unity.Entities;

namespace Game
{
    public struct Heal : IComponentData { public int Value; }
    public struct SpellCost : IComponentData { public int Value; }
    
    [InternalBufferCapacity(4)]
    public struct TakeHealRequest : IBufferElementData { public int Value; }
    
    [InternalBufferCapacity(4)]
    public struct TakeHealEvent : IBufferElementData { public int Value; }
}