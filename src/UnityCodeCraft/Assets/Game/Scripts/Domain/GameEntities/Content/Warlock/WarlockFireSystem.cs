using SampleGame;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    [BurstCompile]
    public partial struct WarlockFireSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _transformLookup;
        private ComponentLookup<Mana> _manaLookup;
        private ComponentLookup<SpellCost> _spellCostLookup;
        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<FireDelay> _fireDelayLookup;
        private ComponentLookup<FireEvent> _fireEventLookup;
        private BufferLookup<TakeDamageRequest> _takeDamageRequests;

        public void OnCreate(ref SystemState state)
        {
            _transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(isReadOnly: true);
            _manaLookup = SystemAPI.GetComponentLookup<Mana>(isReadOnly: false);
            _spellCostLookup = SystemAPI.GetComponentLookup<SpellCost>(isReadOnly: true);
            _teamLookup = SystemAPI.GetComponentLookup<Team>(isReadOnly: true);
            _fireDelayLookup = SystemAPI.GetComponentLookup<FireDelay>(isReadOnly: false);
            _fireEventLookup = SystemAPI.GetComponentLookup<FireEvent>(isReadOnly: false);
            _takeDamageRequests = SystemAPI.GetBufferLookup<TakeDamageRequest>(isReadOnly: false);
        }

        public void OnUpdate(ref SystemState state)
        {
            _transformLookup.Update(ref state);
            _manaLookup.Update(ref state);
            _spellCostLookup.Update(ref state);
            _teamLookup.Update(ref state);
            _fireDelayLookup.Update(ref state);
            _fireEventLookup.Update(ref state);
            _takeDamageRequests.Update(ref state);
            
            foreach ((
                         EnabledRefRW<FireRequest> requestEnabled,
                         RefRO<FireRequest> requestValue,
                         RefRW<FireCooldown> cooldown,
                         RefRO<Team> team,
                         RefRO<CurrentHealth> health,
                         RefRO<AttackDistance> attackDistance,
                         RefRW<LocalTransform> transform,
                         Entity entity)
                     in SystemAPI.Query<
                         EnabledRefRW<FireRequest>,
                         RefRO<FireRequest>,
                         RefRW<FireCooldown>,
                         RefRO<Team>,
                         RefRO<CurrentHealth>,
                         RefRO<AttackDistance>,
                         RefRW<LocalTransform>>()
                         .WithPresent<Warlock>()
                         .WithEntityAccess())
            {
                // Request
                requestEnabled.ValueRW = false;
                
                // Condition
                if(cooldown.ValueRO.IsPlaying())
                    continue;
                
                if(health.ValueRO.IsDead())
                    continue;

                RefRW<Mana> mana = _manaLookup.GetRefRW(entity);
                RefRO<SpellCost> healCost = _spellCostLookup.GetRefRO(entity);
                
                if(mana.ValueRO.Value < healCost.ValueRO.Value)
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
                
                transform.ValueRW.Rotation = quaternion.LookRotation(math.normalize(delta), math.up());
                
                // Action
                _fireEventLookup.GetEnabledRefRW<FireEvent>(entity).ValueRW = true;
                _fireDelayLookup.GetEnabledRefRW<FireDelay>(entity).ValueRW = true;

                cooldown.ValueRW.ResetTime();
                mana.ValueRW.Value -= healCost.ValueRO.Value;
            }
            
            foreach ((
                         EnabledRefRW<FireDelay> delayEnabled,
                         EnabledRefRW<AreaDamageRequest> areaDamageRequestEnabled,
                         RefRW<AreaDamageRequest> areaDamageRequestValue,
                         RefRW<FireDelay> delay,
                         RefRO<FireRequest> requestValue,
                         Entity entity)
                     in SystemAPI.Query<
                         EnabledRefRW<FireDelay>,
                         EnabledRefRW<AreaDamageRequest>,
                         RefRW<AreaDamageRequest>,
                         RefRW<FireDelay>,
                         RefRO<FireRequest>>()
                         .WithPresent<Warlock>()
                         .WithPresent<FireRequest>()
                         .WithEntityAccess())
            {
                if(delay.ValueRO.IsPlaying())
                    continue;
                
                delayEnabled.ValueRW = false;
                delay.ValueRW.ResetTime();
                
                Entity target = requestValue.ValueRO.Target;
                if (target == Entity.Null ||
                    !SystemAPI.Exists(target))
                    continue;

                RefRO<LocalTransform> targetTransform = _transformLookup.GetRefRO(target);

                areaDamageRequestEnabled.ValueRW = true;
                areaDamageRequestValue.ValueRW.Position = targetTransform.ValueRO.Position; 
                areaDamageRequestValue.ValueRW.Instigator = entity;
            }
        }
    }
}