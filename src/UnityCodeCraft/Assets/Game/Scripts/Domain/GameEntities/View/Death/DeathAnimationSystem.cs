using Rukhanka;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct DeathAnimationSystem : ISystem
    {
        private ComponentLookup<DeathEvent> _deathEventLookup;
        private FastAnimatorParameter _deathParam;

        public void OnCreate(ref SystemState state)
        {
            _deathEventLookup = SystemAPI.GetComponentLookup<DeathEvent>(isReadOnly: true);
            _deathParam = new FastAnimatorParameter(UnitAnimation.Death);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            _deathEventLookup.Update(ref state);

            state.Dependency = new DeathAnimationJob
            {
                DeathEventLookup = _deathEventLookup,
                DeathParam = _deathParam
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct DeathAnimationJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<DeathEvent> DeathEventLookup;
            public FastAnimatorParameter DeathParam;

            private void Execute(
                in ModelEntity modelEntity,
                DynamicBuffer<AnimatorControllerParameterComponent> animatorParametersBuf,
                in AnimatorControllerParameterIndexTableComponent animatorParametersIndexTable
            )
            {
                AnimatorParametersAspect paramAspect = new(animatorParametersBuf, animatorParametersIndexTable);
                if(DeathEventLookup.IsComponentEnabled(modelEntity.Value))
                    paramAspect.SetTrigger(DeathParam);
            }
        }
    }
}