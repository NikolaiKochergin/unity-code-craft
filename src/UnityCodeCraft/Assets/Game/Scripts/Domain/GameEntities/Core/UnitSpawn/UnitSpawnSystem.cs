using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    public partial struct UnitSpawnSystem : ISystem
    {
        private Random _random;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            _random = Random.CreateFromIndex(135735);
        }

        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach ((
                         EnabledRefRW<UnitSpawnRequest> spawnRequestEnabled, 
                         RefRO<UnitSpawnRequest> spawnRequestValue, 
                         RefRO<Team> team) 
                     in SystemAPI.Query<
                         EnabledRefRW<UnitSpawnRequest>,
                         RefRO<UnitSpawnRequest>,
                         RefRO<Team>>()
                         .WithPresent<Player>())
            {
                spawnRequestEnabled.ValueRW = false;
                
                float3 unitSpawnPosition = default;

                int index = 0;
                
                foreach ((
                             RefRO<Team> pointTeam,
                             RefRO<UnitSpawnPosition> spawnPosition, 
                             RefRO<UnitSpawnPointCount> spawnPointCount)
                         in SystemAPI.Query<
                             RefRO<Team>,
                             RefRO<UnitSpawnPosition>,
                             RefRO<UnitSpawnPointCount>>())
                {
                    if(pointTeam.ValueRO.Value != team.ValueRO.Value)
                        continue;
                    // int i = _random.NextInt(index, spawnPointCount.ValueRO.Value);
                    // if(i > index)
                        unitSpawnPosition = spawnPosition.ValueRO.Value;
                    index++;
                }
                
                var unit = ecb.Instantiate(spawnRequestValue.ValueRO.Prefab);
                ecb.SetComponent(unit, LocalTransform.FromPositionRotation(unitSpawnPosition, quaternion.identity));
                ecb.SetComponent(unit, new Team { Value = team.ValueRO.Value });
            }
        }
    }
}