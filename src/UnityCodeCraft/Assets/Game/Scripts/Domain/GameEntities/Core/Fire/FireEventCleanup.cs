using Unity.Burst;
using Unity.Entities;

namespace Game.Scripts.Domain.GameEntities.Core.Fire
{
    [BurstCompile]
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct FireEventCleanup : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (EnabledRefRW<FireEvent> fireEvent in SystemAPI.Query<EnabledRefRW<FireEvent>>()) 
                fireEvent.ValueRW = false;
        }
    }
}