using Unity.Burst;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    public partial struct UnitBuySystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((
                         EnabledRefRW<UnitBuyRequest> unitBuyRequestEnabled, 
                         EnabledRefRW<UnitSpawnRequest> unitSpawnRequestEnabled, 
                         RefRO<UnitBuyRequest> unitBuyRequestValue, 
                         RefRW<UnitSpawnRequest> unitSpawnRequestValue,
                         RefRO<Team> playerTeam,
                         RefRW<Money> money)
                     in SystemAPI.Query<
                         EnabledRefRW<UnitBuyRequest>,
                         EnabledRefRW<UnitSpawnRequest>,
                         RefRO<UnitBuyRequest>,
                         RefRW<UnitSpawnRequest>,
                         RefRO<Team>,
                         RefRW<Money>>()
                         .WithPresent<UnitSpawnRequest>()
                         .WithPresent<Player>())
            {
                unitBuyRequestEnabled.ValueRW = false;
                
                foreach ((
                             RefRO<UnitConfig> unitConfig, 
                             RefRO<Team> configTeam, 
                             RefRO<UnitPrice> price, 
                             RefRO<UnitPrefab> prefab) 
                         in SystemAPI.Query<
                             RefRO<UnitConfig>,
                             RefRO<Team>,
                             RefRO<UnitPrice>,
                             RefRO<UnitPrefab>>())
                {
                    if(playerTeam.ValueRO.Value != configTeam.ValueRO.Value)
                        continue;
                    
                    if(unitBuyRequestValue.ValueRO.PrefabName != unitConfig.ValueRO.Name)
                        continue;
                        
                    if(money.ValueRO.Value < price.ValueRO.Value)
                        continue;
                    
                    money.ValueRW.Value -= price.ValueRO.Value;
                    
                    unitSpawnRequestValue.ValueRW.Prefab = prefab.ValueRO.Value;
                    unitSpawnRequestEnabled.ValueRW = true;
                }
            }
        }
    }
}