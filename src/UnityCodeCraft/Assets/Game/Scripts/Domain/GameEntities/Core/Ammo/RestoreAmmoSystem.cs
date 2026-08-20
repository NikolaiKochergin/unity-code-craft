using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    public partial struct RestoreAmmoSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach ((
                         EnabledRefRW<RestoreAmmoCooldown> cooldownEnabled, 
                         RefRW<Ammo> ammo, 
                         RefRO<MaxAmmo> maxAmmo) 
                     in SystemAPI.Query<
                         EnabledRefRW<RestoreAmmoCooldown>,
                         RefRW<Ammo>,
                         RefRO<MaxAmmo>>()
                         .WithPresent<RestoreAmmoCooldown>())
            {
                if(cooldownEnabled.ValueRO)
                    continue;
                
                if(ammo.ValueRO.Value < maxAmmo.ValueRO.Value)
                    cooldownEnabled.ValueRW = true;
            }

            foreach ((
                         EnabledRefRW<RestoreAmmoCooldown> cooldownEnabled, 
                         RefRW<RestoreAmmoCooldown> cooldown,
                         RefRW<Ammo> ammo)
                     in SystemAPI.Query<
                         EnabledRefRW<RestoreAmmoCooldown>,
                         RefRW<RestoreAmmoCooldown>,
                         RefRW<Ammo>>())
            {
                cooldown.ValueRW.Time = math.max(cooldown.ValueRW.Time - deltaTime, 0f);
                
                if(cooldown.ValueRO.Time > 0f)
                    continue;

                cooldownEnabled.ValueRW = false;
                cooldown.ValueRW.Time = cooldown.ValueRO.Duration;
                ammo.ValueRW.Value++;
            }
        }
    }
}