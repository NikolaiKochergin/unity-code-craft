using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    [BurstCompile]
    public partial struct FireCooldownSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (RefRW<FireCooldown> cooldown in SystemAPI.Query<RefRW<FireCooldown>>()) 
                cooldown.ValueRW.Time = math.max(cooldown.ValueRW.Time - deltaTime, 0f);
        }
    }
}