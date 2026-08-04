using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    [BurstCompile]
    public partial struct SwordmanTakeDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((
                         RefRW<CurrentHealth> health, 
                         DynamicBuffer<TakeDamageRequest> requests, 
                         DynamicBuffer<TakeDamageEvent> events, 
                         RefRO<ArmorMultiplier> armor) 
                     in SystemAPI.Query<
                         RefRW<CurrentHealth>,
                         DynamicBuffer<TakeDamageRequest>,
                         DynamicBuffer<TakeDamageEvent>,
                         RefRO<ArmorMultiplier>>()
                         .WithPresent<Swordman>())
            {
                for (int i = 0; i < requests.Length && health.ValueRW.IsAlive(); i++)
                {
                    TakeDamageRequest request = requests[i];

                    int damage = (int)math.round(request.Damage * (1f - armor.ValueRO.Value));

                    health.ValueRW.Reduce(damage);

                    events.Add(new TakeDamageEvent
                    {
                        Damage = damage,
                        Instigator = request.Instigator,
                    });
                }
                
                requests.Clear();
            }
        }
    }
}