using Unity.Burst;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    public partial struct CastleTakeDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((
                         RefRW<CurrentHealth> health, 
                         DynamicBuffer<TakeDamageRequest> requests, 
                         DynamicBuffer<TakeDamageEvent> events) 
                     in SystemAPI.Query<
                         RefRW<CurrentHealth>,
                         DynamicBuffer<TakeDamageRequest>,
                         DynamicBuffer<TakeDamageEvent>>()
                         .WithPresent<Castle>())
            {
                for (int i = 0; i < requests.Length && health.ValueRW.IsAlive(); i++)
                {
                    TakeDamageRequest request = requests[i];
                    health.ValueRW.Reduce(request.Damage);

                    events.Add(new TakeDamageEvent
                    {
                        Damage = request.Damage,
                        Instigator = request.Instigator,
                    });
                }
                
                requests.Clear();
            }
        }
    }
}