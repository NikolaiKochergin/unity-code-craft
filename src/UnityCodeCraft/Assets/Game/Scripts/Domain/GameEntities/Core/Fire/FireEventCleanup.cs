using Unity.Burst;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct FireEventCleanup : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new FireEventCleanupJob().Schedule(state.Dependency);
            state.Dependency.Complete();
        }

        [BurstCompile]
        private partial struct FireEventCleanupJob : IJobEntity
        {
            private static void Execute(EnabledRefRW<FireEvent> fireEvent) => 
                fireEvent.ValueRW = false;
        }
    }
}