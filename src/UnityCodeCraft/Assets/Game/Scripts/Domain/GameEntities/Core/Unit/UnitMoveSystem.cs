using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    [BurstCompile]
    public partial struct UnitMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new MoveJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel(state.Dependency);
        }

        [WithPresent(typeof(MoveEvent))]
        [WithAll(typeof(Unit))]
        [BurstCompile]
        private partial struct MoveJob : IJobEntity
        {
            public float DeltaTime;

            private void Execute(
                EnabledRefRW<MoveRequest> requestEnabled,
                EnabledRefRW<MoveEvent> eventEnabled,
                ref MoveRequest request,
                ref LocalTransform transform,
                in MoveSpeed moveSpeed,
                in RotationSpeed rotationSpeed
            )
            {
                // Request
                requestEnabled.ValueRW = false;
                
                // Condition
                float3 direction = request.Direction;
                if(math.all(direction == float3.zero))
                    return;
                
                // TODO:
                // if(health.IsDead())
                //     return;
                
                // Action
                MoveUseCase.MoveStep(
                    ref transform,
                    in direction,
                    in moveSpeed,
                    DeltaTime);
                
                RotationUseCase.RotateStep(
                    ref transform,
                    in direction,
                    in rotationSpeed,
                    DeltaTime
                );
                
                // Event
                eventEnabled.ValueRW = true;
            }
        }
    }
}