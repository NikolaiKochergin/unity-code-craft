using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Game
{
    [BurstCompile]
    public partial struct OpponentAISystem : ISystem
    {
        private Random _random;

        public void OnCreate(ref SystemState state)
        {
            _random = Random.CreateFromIndex(156353);
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((
                         EnabledRefRW<SelectedUnit> selectedUnitEnabled, 
                         EnabledRefRW<UnitBuyRequest> unitToBuyRequestEnabled, 
                         RefRW<SelectedUnit> selectedUnitValue,
                         RefRW<UnitBuyRequest> unitToBuyRequestValue,
                         RefRW<Money> money)
                     in SystemAPI.Query<
                         EnabledRefRW<SelectedUnit>,
                         EnabledRefRW<UnitBuyRequest>,
                         RefRW<SelectedUnit>,
                         RefRW<UnitBuyRequest>,
                         RefRW<Money>>()
                         .WithPresent<SelectedUnit>())
            {
                if (selectedUnitEnabled.ValueRO)
                {
                    foreach ((
                                 RefRO<UnitConfig> config,
                                 RefRO<UnitPrice> price) 
                             in SystemAPI.Query<
                                 RefRO<UnitConfig>,
                                 RefRO<UnitPrice>>())
                    {
                        if(selectedUnitValue.ValueRO.Name != config.ValueRO.Name)
                            continue;
                        
                        if (money.ValueRO.Value < price.ValueRO.Value)
                            continue;

                        selectedUnitEnabled.ValueRW = false;
                        unitToBuyRequestEnabled.ValueRW = true;
                        unitToBuyRequestValue.ValueRW.PrefabName = config.ValueRO.Name;
                    }
                }
                else
                {
                    foreach (RefRO<UnitConfig> config in SystemAPI.Query<RefRO<UnitConfig>>())
                    {
                        if (_random.NextBool())
                            continue;

                        selectedUnitEnabled.ValueRW = true;
                        selectedUnitValue.ValueRW.Name = config.ValueRO.Name;
                        break;
                    }
                }
            }
        }
    }
}