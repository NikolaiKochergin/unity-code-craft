using Unity.Burst;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    public partial struct DeathCooldownSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) => 
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = SystemAPI
                .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach ((RefRW<DeathCooldown> cooldown, Entity entity) in SystemAPI
                    .Query<RefRW<DeathCooldown>>()
                    .WithEntityAccess())
            {
                cooldown.ValueRW.Time -= deltaTime;
                if(cooldown.ValueRW.Time <= 0)
                    ecb.DestroyEntity(entity);
            }
        }
    }
}