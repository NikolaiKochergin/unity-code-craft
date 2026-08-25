using SampleGame;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    [BurstCompile]
    public partial struct MageFireSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _transformLookup;
        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<FireDelay> _fireDelayLookup;
        private ComponentLookup<FireEvent> _fireEventLookup;
        private ComponentLookup<Mana> _manaLookup;
        private ComponentLookup<HealCost> _healCostLookup;
        private BufferLookup<TakeHealRequest> _takeHealRequests;

        public void OnCreate(ref SystemState state)
        {
            _transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(isReadOnly: true);
            _teamLookup = SystemAPI.GetComponentLookup<Team>(isReadOnly: true);
            _fireDelayLookup = SystemAPI.GetComponentLookup<FireDelay>(isReadOnly: false);
            _fireEventLookup = SystemAPI.GetComponentLookup<FireEvent>(isReadOnly: false);
            _manaLookup = SystemAPI.GetComponentLookup<Mana>(isReadOnly: false);
            _healCostLookup = SystemAPI.GetComponentLookup<HealCost>(isReadOnly: true);
            _takeHealRequests = SystemAPI.GetBufferLookup<TakeHealRequest>(isReadOnly: false);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _transformLookup.Update(ref state);
            _teamLookup.Update(ref state);
            _fireDelayLookup.Update(ref state);
            _fireEventLookup.Update(ref state);
            _manaLookup.Update(ref state);
            _healCostLookup.Update(ref state);
            _takeHealRequests.Update(ref state);
            
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
                         .WithPresent<Mage>()
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
                RefRO<HealCost> healCost = _healCostLookup.GetRefRO(entity);
                
                if(mana.ValueRO.Value < healCost.ValueRO.Value)
                    continue;
                
                Entity target = requestValue.ValueRO.Target;
                if(target == Entity.Null ||
                   !SystemAPI.Exists(target) ||
                   !_transformLookup.TryGetComponent(target, out LocalTransform targetTransform))
                    continue;
                
                TeamType myTeam = team.ValueRO.Value;
                if(!_teamLookup.TryGetComponent(target, out Team targetTeam) || targetTeam.Value != myTeam)
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
                         RefRW<FireDelay> delay,
                         RefRO<FireRequest> requestValue,
                         RefRO<Heal> heal)
                     in SystemAPI.Query<
                         EnabledRefRW<FireDelay>,
                         RefRW<FireDelay>,
                         RefRO<FireRequest>,
                         RefRO<Heal>>()
                         .WithPresent<Mage>()
                         .WithPresent<FireRequest>())
            {
                if(delay.ValueRO.IsPlaying())
                    continue;
                
                delayEnabled.ValueRW = false;
                delay.ValueRW.ResetTime();
                
                Entity target = requestValue.ValueRO.Target;
                if (target == Entity.Null ||
                    !SystemAPI.Exists(target))
                    continue;
                
                if (!_takeHealRequests.TryGetBuffer(target, out DynamicBuffer<TakeHealRequest> requests))
                    continue;

                requests.Add(new TakeHealRequest
                {
                    Value = heal.ValueRO.Value,
                });
            }
        }
    }
}