using Unity.Burst;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct TakeDamageEventCleanup : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state) => 
            state.Dependency = new TakeDamageEventCleanupJob().ScheduleParallel(state.Dependency);

        [BurstCompile]
        private partial struct TakeDamageEventCleanupJob : IJobEntity
        {
            private static void Execute(DynamicBuffer<TakeDamageEvent> events) =>
                events.Clear();
        }
    }
}