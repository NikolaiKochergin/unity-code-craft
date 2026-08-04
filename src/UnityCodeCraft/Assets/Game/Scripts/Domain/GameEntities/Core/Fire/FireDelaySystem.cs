using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    [BurstCompile]
    public partial struct FireDelaySystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (RefRW<FireDelay> delay in SystemAPI.Query<RefRW<FireDelay>>()) 
                delay.ValueRW.Time = math.max(delay.ValueRW.Time - deltaTime, 0f);
        }
    }
}