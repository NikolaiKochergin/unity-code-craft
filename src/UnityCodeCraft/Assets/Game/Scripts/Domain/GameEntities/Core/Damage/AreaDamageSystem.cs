using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    public partial struct AreaDamageSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach ((
                         EnabledRefRW<AreaDamageRequest> requestEnabled, 
                         RefRW<AreaDamageRequest> requestValue, 
                         RefRO<Damage> damage, 
                         RefRO<DamageRadius> damageRadius, 
                         RefRO<Team> team) 
                     in SystemAPI.Query<
                         EnabledRefRW<AreaDamageRequest>,
                         RefRW<AreaDamageRequest>,
                         RefRO<Damage>,
                         RefRO<DamageRadius>,
                         RefRO<Team>>())
            {
                requestEnabled.ValueRW = false;

                foreach ((
                             RefRO<LocalTransform> transform, 
                             RefRO<Team> targetTeam, 
                             DynamicBuffer<TakeDamageRequest> requests) 
                         in SystemAPI.Query<
                             RefRO<LocalTransform>,
                             RefRO<Team>,
                             DynamicBuffer<TakeDamageRequest>>())
                {
                    if(team.ValueRO.Value == targetTeam.ValueRO.Value)
                        continue;
                    
                    float distance = damageRadius.ValueRO.Value;
                    float3 delta = transform.ValueRO.Position - requestValue.ValueRO.Position;
                    if(math.lengthsq(delta) > distance * distance)
                        continue;

                    requests.Add(new TakeDamageRequest
                    {
                        Damage = damage.ValueRO.Value,
                        Instigator = requestValue.ValueRO.Instigator,
                    });
                }
            }
        }
    }
}