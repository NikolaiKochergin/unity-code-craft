using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public partial struct RestoreManaSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach ((
                         EnabledRefRW<ManaRestoreCooldown> cooldownEnabled, 
                         RefRW<Mana> mana) 
                     in SystemAPI.Query<
                         EnabledRefRW<ManaRestoreCooldown>,
                         RefRW<Mana>>()
                         .WithPresent<ManaRestoreCooldown>())
            {
                if(cooldownEnabled.ValueRO)
                    continue;

                if (mana.ValueRO.Value <= 0)
                    cooldownEnabled.ValueRW = true;
            }

            foreach ((
                         EnabledRefRW<ManaRestoreCooldown> cooldownEnabled, 
                         RefRW<ManaRestoreCooldown> cooldown, 
                         RefRW<Mana> mana, 
                         RefRO<MaxMana> maxMana) 
                     in SystemAPI.Query<
                         EnabledRefRW<ManaRestoreCooldown>,
                         RefRW<ManaRestoreCooldown>,
                         RefRW<Mana>,
                         RefRO<MaxMana>>())
            {
                cooldown.ValueRW.Time = math.max(cooldown.ValueRO.Time - deltaTime, 0f);
                
                if(cooldown.ValueRO.Time > 0f)
                    continue;
                
                cooldownEnabled.ValueRW = false;
                cooldown.ValueRW.Time = cooldown.ValueRO.Duration;
                mana.ValueRW.Value = maxMana.ValueRO.Value;
            }
        }
    }
}