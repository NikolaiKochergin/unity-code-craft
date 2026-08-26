using Unity.Burst;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    public partial struct GameOverSystem : ISystem
    {
        private ComponentLookup<CurrentHealth> _healthLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            _healthLookup = SystemAPI.GetComponentLookup<CurrentHealth>(isReadOnly: true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _healthLookup.Update(ref state);
            
            EntityCommandBuffer ecb = SystemAPI
                .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach ((
                         RefRO<Player> player, 
                         RefRO<Team> team, 
                         Entity entity) 
                     in SystemAPI.Query<
                         RefRO<Player>,
                         RefRO<Team>>()
                         .WithEntityAccess())
            {
                Entity castleEntity = player.ValueRO.Castle;

                if (castleEntity != Entity.Null &&
                    SystemAPI.Exists(castleEntity) &&
                    (!_healthLookup.TryGetComponent(castleEntity, out var health) || health.Value > 0)) 
                    continue;
                
                ecb.DestroyEntity(entity);

                foreach ((
                             RefRO<Team> unitTeam, 
                             Entity unitEntity) 
                         in SystemAPI.Query<
                             RefRO<Team>>()
                             .WithPresent<Unit>()
                             .WithEntityAccess())
                {
                    if(unitTeam.ValueRO.Value == team.ValueRO.Value)
                        ecb.DestroyEntity(unitEntity);
                }

                foreach (
                    EnabledRefRW<IncomeTickCooldown> income 
                    in SystemAPI.Query<EnabledRefRW<IncomeTickCooldown>>()) 
                    income.ValueRW = false;
            }
        }
    }
}