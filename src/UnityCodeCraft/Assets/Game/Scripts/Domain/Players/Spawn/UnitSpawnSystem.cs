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
                
                foreach ((
                             RefRO<Team> pointTeam,
                             DynamicBuffer<UnitSpawnPoint> spawnPoints)
                         in SystemAPI.Query<
                             RefRO<Team>,
                             DynamicBuffer<UnitSpawnPoint>>())
                {
                    if(pointTeam.ValueRO.Value != team.ValueRO.Value)
                        continue;
                    
                    if(spawnPoints.Length == 0)
                        continue;

                    int randomIndex = _random.NextInt(spawnPoints.Length);
                    unitSpawnPosition = spawnPoints[randomIndex].Position;
                }
                
                Entity unit = ecb.Instantiate(spawnRequestValue.ValueRO.Prefab);
                ecb.SetComponent(unit, LocalTransform.FromPositionRotation(unitSpawnPosition, quaternion.identity));
                ecb.SetComponent(unit, new Team { Value = team.ValueRO.Value });
            }
        }
    }
}