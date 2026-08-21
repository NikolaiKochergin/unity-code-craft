using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public partial struct IncomeSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            
            foreach ((
                         RefRW<Money> money, 
                         RefRO<MoneyIncome> income, 
                         RefRW<IncomeTickCooldown> cooldown) 
                     in SystemAPI.Query<
                         RefRW<Money>,
                         RefRO<MoneyIncome>,
                         RefRW<IncomeTickCooldown>>())
            {
                cooldown.ValueRW.Time = math.max(cooldown.ValueRW.Time - deltaTime, 0f);
                
                if(cooldown.ValueRW.Time > 0f)
                    continue;

                money.ValueRW.Value += income.ValueRO.Value;
                cooldown.ValueRW.Time = cooldown.ValueRO.Duration;
            }
        }
    }
}