using Game.Scripts.Domain.GameEntities.Core.Fire;
using Rukhanka;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct FireAnimationSystem : ISystem
    {
        private FastAnimatorParameter _fireParam;
        private ComponentLookup<FireEvent> _fireEventLookup;

        public void OnCreate(ref SystemState state)
        {
            _fireParam = new FastAnimatorParameter(UnitAnimation.Fire);
            _fireEventLookup = SystemAPI.GetComponentLookup<FireEvent>(isReadOnly: true);
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            _fireEventLookup.Update(ref state);
            
            state.Dependency = new FireAnimationJob()
            {
                FireEventLookup =  _fireEventLookup,
                FireParam = _fireParam,
            }.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        private partial struct FireAnimationJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<FireEvent> FireEventLookup;
            public FastAnimatorParameter FireParam;
            
            private void Execute(
                in ModelEntity modelEntity,
                DynamicBuffer<AnimatorControllerParameterComponent> animatorParametersBuf,
                in AnimatorControllerParameterIndexTableComponent animatorParametersIndexTable
            )
            {
                AnimatorParametersAspect paramAspect = new(animatorParametersBuf, animatorParametersIndexTable);
                if(FireEventLookup.IsComponentEnabled(modelEntity.Value))
                    paramAspect.SetTrigger(FireParam);
            }
        }
    }
}