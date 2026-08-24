using Unity.Entities;

namespace Game
{
    public partial struct UnitBuySystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach ((
                         EnabledRefRW<UnitBuyRequest> unitBuyRequestEnabled, 
                         EnabledRefRW<UnitSpawnRequest> unitSpawnRequestEnabled, 
                         RefRO<UnitBuyRequest> unitBuyRequestValue, 
                         RefRW<UnitSpawnRequest> unitSpawnRequestValue, 
                         RefRW<Money> money)
                     in SystemAPI.Query<
                         EnabledRefRW<UnitBuyRequest>,
                         EnabledRefRW<UnitSpawnRequest>,
                         RefRO<UnitBuyRequest>,
                         RefRW<UnitSpawnRequest>,
                         RefRW<Money>>()
                         .WithPresent<UnitSpawnRequest>()
                         .WithPresent<Player>())
            {
                unitBuyRequestEnabled.ValueRW = false;
                
                foreach ((
                             RefRO<UnitConfig> unitConfig, 
                             RefRO<UnitPrice> price, 
                             RefRO<UnitPrefab> prefab) 
                         in SystemAPI.Query<
                             RefRO<UnitConfig>,
                             RefRO<UnitPrice>,
                             RefRO<UnitPrefab>>())
                {
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