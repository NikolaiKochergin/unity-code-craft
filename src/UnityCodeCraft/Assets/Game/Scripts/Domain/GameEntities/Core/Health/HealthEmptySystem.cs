using Unity.Burst;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    public partial struct HealthEmptySystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (
                (
                    RefRO<CurrentHealth> currentHealth,
                    RefRW<DeathCooldown> deathCooldown, 
                    EnabledRefRW<DeathCooldown> deathCooldownEnabled, 
                    EnabledRefRW<DeathEvent> deathEventEnabled) 
                in SystemAPI.Query<
                    RefRO<CurrentHealth>,
                    RefRW<DeathCooldown>,
                    EnabledRefRW<DeathCooldown>,
                    EnabledRefRW<DeathEvent>>()
                    .WithDisabled<DeathCooldown>()
                    .WithPresent<DeathEvent>())
            {
                if(!currentHealth.ValueRO.IsDead())
                    continue;

                deathEventEnabled.ValueRW = true;

                deathCooldownEnabled.ValueRW = true;
                deathCooldown.ValueRW.Time = deathCooldown.ValueRO.Duration;
            }
        }
    }
}