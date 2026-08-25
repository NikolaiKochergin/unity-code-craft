using Unity.Burst;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    [UpdateInGroup(typeof(CleanupSystemGroup))]
    public partial struct TakeHealEventCleanup : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state) => 
            state.Dependency = new TakeHealEventCleanupJob().ScheduleParallel(state.Dependency);

        [BurstCompile]
        private partial struct TakeHealEventCleanupJob : IJobEntity
        {
            private static void Execute(DynamicBuffer<TakeHealEvent> events) => 
                events.Clear();
        }
    }
}