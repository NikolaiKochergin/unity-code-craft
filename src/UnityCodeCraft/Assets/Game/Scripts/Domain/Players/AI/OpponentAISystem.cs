using Unity.Burst;
using Unity.Collections;
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
                         RefRW<SelectedUnit> selectedUnit,
                         RefRW<Money> money,
                         Entity entity)
                     in SystemAPI.Query<
                         RefRW<SelectedUnit>,
                         RefRW<Money>>()
                         .WithPresent<SelectedUnit>()
                         .WithEntityAccess())
            {
                if (!SystemAPI.IsComponentEnabled<SelectedUnit>(entity))
                {
                    ChooseRandomUnit(ref state, entity, ref selectedUnit.ValueRW);
                    continue;
                }

                foreach ((
                             RefRO<UnitConfig> config, 
                             RefRO<UnitPrice> price) 
                         in SystemAPI.Query<
                             RefRO<UnitConfig>,
                             RefRO<UnitPrice>>())
                {
                    if(config.ValueRO.Name != selectedUnit.ValueRO.Name)
                        continue;
                    
                    if(money.ValueRO.Value < price.ValueRO.Value)
                        break;
                    
                    SystemAPI.SetComponentEnabled<UnitBuyRequest>(entity, true);
                    SystemAPI.SetComponent(entity, new UnitBuyRequest { PrefabName = config.ValueRO.Name });
                    SystemAPI.SetComponentEnabled<SelectedUnit>(entity, false);
                    
                    break;
                }
            }
        }

        private void ChooseRandomUnit(
            ref SystemState state, 
            Entity entity, 
            ref SelectedUnit selectedUnit)
        {
            NativeList<UnitConfig> units = new(Allocator.Temp);

            foreach (RefRO<UnitConfig> config in SystemAPI.Query<RefRO<UnitConfig>>()) 
                units.Add(config.ValueRO);
            
            if(units.Length == 0)
                return;
            
            var index = _random.NextInt(units.Length);
            selectedUnit.Name = units[index].Name;
            
            SystemAPI.SetComponentEnabled<SelectedUnit>(entity, true);
        }
    }
}