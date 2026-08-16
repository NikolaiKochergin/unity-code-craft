using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Game
{
    [BurstCompile]
    public partial struct ProjectileSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _transformLookup;
        private BufferLookup<TakeDamageRequest> _takeDamageRequests;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();

            _transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(isReadOnly: true);
            _takeDamageRequests = SystemAPI.GetBufferLookup<TakeDamageRequest>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            
            _transformLookup.Update(ref state);
            _takeDamageRequests.Update(ref state);

            float deltaTime = SystemAPI.Time.DeltaTime;

            EntityCommandBuffer ecb = SystemAPI
                .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach ((
                    RefRW<LocalTransform> transform, 
                    RefRO<MoveSpeed> moveSpeed,
                    RefRO<TargetEntity> targetRef, 
                    RefRO<StoppingDistance> stoppingDistanceRef, 
                    RefRO<Damage> damage, 
                    RefRO<TargetOffset> offset,
                    Entity entity) 
                in SystemAPI.Query<
                    RefRW<LocalTransform>,
                    RefRO<MoveSpeed>,
                    RefRO<TargetEntity>,
                    RefRO<StoppingDistance>,
                    RefRO<Damage>,
                    RefRO<TargetOffset>>()
                    .WithPresent<Projectile>()
                    .WithEntityAccess())
            {
                Entity target = targetRef.ValueRO.Value;

                if (target == Entity.Null ||
                    !_transformLookup.TryGetComponent(target, out LocalTransform targetTransform))
                {
                    MoveUseCase.MoveStep(
                        ref transform.ValueRW, 
                        transform.ValueRO.Forward(), 
                        in moveSpeed.ValueRO,
                        deltaTime);
                    continue;
                }

                if (_takeDamageRequests.TryGetBuffer(target, out DynamicBuffer<TakeDamageRequest> damageRequests))
                {
                    damageRequests.Add(new TakeDamageRequest
                    {
                        Damage = damage.ValueRO.Value,
                        Instigator = entity
                    });
                }
                
                ecb.DestroyEntity(entity);
            }
        }
    }
}