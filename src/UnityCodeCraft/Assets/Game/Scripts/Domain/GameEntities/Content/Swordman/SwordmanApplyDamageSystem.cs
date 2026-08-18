using Unity.Burst;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    public partial struct SwordmanApplyDamageSystem : ISystem
    {
        private BufferLookup<TakeDamageRequest> _takeDamageRequests;
        
        public void OnCreate(ref SystemState state)
        {
            _takeDamageRequests = SystemAPI.GetBufferLookup<TakeDamageRequest>(isReadOnly: false);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _takeDamageRequests.Update(ref state);

            foreach ((
                         EnabledRefRW<FireDelay> delayEnabled,
                         RefRW<FireDelay> delay,
                         RefRO<FireRequest> requestValue,
                         RefRO<Damage> damage,
                         Entity entity)
                     in SystemAPI.Query<
                         EnabledRefRW<FireDelay>,
                         RefRW<FireDelay>,
                         RefRO<FireRequest>,
                         RefRO<Damage>>()
                         .WithPresent<Swordman>()
                         .WithPresent<FireRequest>()
                         .WithEntityAccess())
            {
                // Condition
                if (delay.ValueRO.IsPlaying())
                    continue;

                delayEnabled.ValueRW = false;
                delay.ValueRW.ResetTime();

                Entity target = requestValue.ValueRO.Target;
                if (target == Entity.Null ||
                    !SystemAPI.Exists(target))
                    continue;
                
                // Action
                if (!_takeDamageRequests.TryGetBuffer(target, out DynamicBuffer<TakeDamageRequest> requests))
                    continue;

                requests.Add(new TakeDamageRequest
                {
                    Damage = damage.ValueRO.Value,
                    Instigator = entity
                });
            }
        }
    }
}