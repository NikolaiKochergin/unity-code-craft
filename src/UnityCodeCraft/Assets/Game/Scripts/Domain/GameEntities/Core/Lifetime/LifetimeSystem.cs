using Unity.Burst;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    public partial struct LifetimeSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltatime = SystemAPI.Time.DeltaTime;
            EntityCommandBuffer ecb = SystemAPI
                .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach ((
                         RefRW<Lifetime> lifetime, 
                         Entity entity) 
                     in SystemAPI.Query<
                         RefRW<Lifetime>>()
                         .WithEntityAccess())
            {
                ref float time = ref lifetime.ValueRW.Value;
                time -= deltatime;
                if (time <= 0)
                    ecb.DestroyEntity(entity);
            }
        }
    }
}