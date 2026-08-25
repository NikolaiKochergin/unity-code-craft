using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public partial struct UnitTakeHealSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach ((
                         RefRW<CurrentHealth> health, 
                         RefRO<MaxHealth> maxHealth, 
                         DynamicBuffer<TakeHealRequest> requests, 
                         DynamicBuffer<TakeHealEvent> events) 
                     in SystemAPI.Query<
                         RefRW<CurrentHealth>,
                         RefRO<MaxHealth>,
                         DynamicBuffer<TakeHealRequest>,
                         DynamicBuffer<TakeHealEvent>>()
                         .WithPresent<Unit>())
            {
                for (int i = 0; i < requests.Length && health.ValueRW.IsAlive(); i++)
                {
                    TakeHealRequest request = requests[i];
                    health.ValueRW.Value = math.clamp(health.ValueRW.Value + request.Value, 0, maxHealth.ValueRO.Value);

                    events.Add(new TakeHealEvent
                    {
                        Value = request.Value,
                    });
                }
                
                requests.Clear();
            }
        }
    }
}