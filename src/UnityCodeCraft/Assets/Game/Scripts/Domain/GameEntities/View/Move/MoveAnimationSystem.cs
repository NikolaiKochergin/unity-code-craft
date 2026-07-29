using Rukhanka;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game
{
    [BurstCompile]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct MoveAnimationSystem : ISystem
    {
        private ComponentLookup<MoveEvent> _moveEventLookup;
        private FastAnimatorParameter _moveParam;

        public void OnCreate(ref SystemState state)
        {
            _moveParam = new FastAnimatorParameter(UnitAnimation.IsMoving);
            _moveEventLookup = SystemAPI.GetComponentLookup<MoveEvent>(isReadOnly: true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            _moveEventLookup.Update(ref state);

            state.Dependency = new MoveAnimationJob
            {
                MoveEventLookup = _moveEventLookup,
                MoveParam = _moveParam
            }.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        private partial struct MoveAnimationJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<MoveEvent> MoveEventLookup;
            public FastAnimatorParameter MoveParam;
            
            private void Execute(
                in ModelEntity modelEntity,
                DynamicBuffer<AnimatorControllerParameterComponent> animatorParametersBuf,
                in AnimatorControllerParameterIndexTableComponent animatorParametersIndexTable
            )
            {
                AnimatorParametersAspect paramAspect = new(animatorParametersBuf, animatorParametersIndexTable);
                paramAspect.SetParameterValue(
                    MoveParam, 
                    MoveEventLookup.IsComponentEnabled(modelEntity.Value));
            }
        } 
    }
}