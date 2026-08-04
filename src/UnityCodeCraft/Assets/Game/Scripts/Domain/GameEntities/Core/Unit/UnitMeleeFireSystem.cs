using SampleGame;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    [BurstCompile]
    public partial struct UnitMeleeFireSystem : ISystem
    {
        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<LocalTransform> _transformLookup;
        private ComponentLookup<FireEvent> _fireEventLookup;
        private BufferLookup<TakeDamageRequest> _takeDamageRequests;

        public void OnCreate(ref SystemState state)
        {
            _teamLookup = SystemAPI.GetComponentLookup<Team>(isReadOnly: true);
            _transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(isReadOnly: true);
            _fireEventLookup = SystemAPI.GetComponentLookup<FireEvent>(isReadOnly: false);
            _takeDamageRequests = SystemAPI.GetBufferLookup<TakeDamageRequest>(isReadOnly: false);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _teamLookup.Update(ref state);
            _transformLookup.Update(ref state);
            _fireEventLookup.Update(ref state);
            _takeDamageRequests.Update(ref state);

            foreach ((
                         EnabledRefRW<FireRequest> requestEnabled, 
                         RefRO<FireRequest> requestValue, 
                         RefRW<FireCooldown> cooldown, 
                         RefRO<Team> team, 
                         RefRO<AttackDistance> attackDistance, 
                         RefRO<LocalTransform> transform, 
                         RefRO<Damage> damage, 
                         Entity entity) 
                     in SystemAPI.Query<
                         EnabledRefRW<FireRequest>,
                         RefRO<FireRequest>,
                         RefRW<FireCooldown>,
                         RefRO<Team>,
                         RefRO<AttackDistance>,
                         RefRO<LocalTransform>,
                         RefRO<Damage>>()
                         .WithPresent<Unit>()
                         .WithEntityAccess())
            {
                // Request
                requestEnabled.ValueRW = false;
                
                // Condition
                if(cooldown.ValueRO.IsPlaying())
                    continue;

                Entity target = requestValue.ValueRO.Target;
                if(target == Entity.Null ||
                   !SystemAPI.Exists(target) ||
                   !_transformLookup.TryGetComponent(target, out LocalTransform targetTransform))
                    continue;

                TeamType myTeam = team.ValueRO.Value;
                if(!_teamLookup.TryGetComponent(target, out Team targetTeam) || targetTeam.Value == myTeam)
                    continue;

                float distance = attackDistance.ValueRO.Value;
                float3 delta = targetTransform.Position - transform.ValueRO.Position;
                if(math.lengthsq(delta) > distance * distance)
                    continue;
                
                // Action
                if(!_takeDamageRequests.TryGetBuffer(target, out DynamicBuffer<TakeDamageRequest> requests))
                    continue;

                requests.Add(new TakeDamageRequest
                {
                    Damage = damage.ValueRO.Value,
                    Instigator = entity
                });

                _fireEventLookup.GetEnabledRefRW<FireEvent>(entity).ValueRW = true;
                
                // TODO:
                // где то тут должен вызываться ивент для запуска анимации
                
                cooldown.ValueRW.ResetTime();
            }
        }
    }
}