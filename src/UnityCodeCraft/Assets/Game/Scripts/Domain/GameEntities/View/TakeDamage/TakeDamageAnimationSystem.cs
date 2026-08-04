using Rukhanka;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    public partial struct TakeDamageAnimationSystem : ISystem
    {
        private FastAnimatorParameter _takeDamageParam;
        private BufferLookup<TakeDamageEvent> _takeDamageEventLookup;

        public void OnCreate(ref SystemState state)
        {
            _takeDamageParam = new FastAnimatorParameter(UnitAnimation.TakeDamage);
            _takeDamageEventLookup = SystemAPI.GetBufferLookup<TakeDamageEvent>(isReadOnly: true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            _takeDamageEventLookup.Update(ref state);

            state.Dependency = new TakeDamageAnimationJob
            {
                TakeDamageEventLookup = _takeDamageEventLookup,
                TakeDamageParam = _takeDamageParam,
            }.Schedule(state.Dependency);
        }

        [BurstCompile]
        private partial struct TakeDamageAnimationJob : IJobEntity
        {
            [ReadOnly]
            public BufferLookup<TakeDamageEvent> TakeDamageEventLookup;
            public FastAnimatorParameter TakeDamageParam;

            private void Execute(
                in ModelEntity modelEntity,
                DynamicBuffer<AnimatorControllerParameterComponent> animatorParametersBuf,
                in AnimatorControllerParameterIndexTableComponent animatorParametersIndexTable
            )
            {
                AnimatorParametersAspect paramAspect = new(animatorParametersBuf, animatorParametersIndexTable);
                DynamicBuffer<TakeDamageEvent> buffer = TakeDamageEventLookup[modelEntity.Value];
                if(!buffer.IsEmpty)
                    paramAspect.SetTrigger(TakeDamageParam);
            }
        }
    }
}