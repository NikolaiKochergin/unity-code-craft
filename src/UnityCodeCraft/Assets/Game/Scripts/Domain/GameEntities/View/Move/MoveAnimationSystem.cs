using Rukhanka;
using Unity.Burst;
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
            _moveParam = new FastAnimatorParameter("IsMoving");
            _moveEventLookup = SystemAPI.GetComponentLookup<MoveEvent>(isReadOnly: true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            _moveEventLookup.Update(ref state);

            foreach ((RefRO<ModelEntity> modelEntityRef,
                         DynamicBuffer<AnimatorControllerParameterComponent> animatorParametersBuf, 
                         AnimatorControllerParameterIndexTableComponent animatorParametersIndexTable) 
                     in SystemAPI
                         .Query<RefRO<ModelEntity>,
                         DynamicBuffer<AnimatorControllerParameterComponent>,
                         AnimatorControllerParameterIndexTableComponent>())
            {
                Entity modelEntity = modelEntityRef.ValueRO.Value;
                
                AnimatorParametersAspect paramAspect = new(animatorParametersBuf, animatorParametersIndexTable);
                
                paramAspect.SetParameterValue(_moveParam, _moveEventLookup.IsComponentEnabled(modelEntity));
            }
        }
    }
}