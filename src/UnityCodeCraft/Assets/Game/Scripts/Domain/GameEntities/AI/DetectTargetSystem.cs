using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    [BurstCompile]
    public partial struct DetectTargetSystem : ISystem
    {
        private const float CellSize = 3f;
        private NativeParallelMultiHashMap<int, Entity> _gridMap;
        
        private ComponentLookup<LocalTransform> _transformLookup;
        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<CurrentHealth> _healthLookup;

        public void OnCreate(ref SystemState state)
        {
            _gridMap = new NativeParallelMultiHashMap<int, Entity>(128, Allocator.Persistent);
            
            _transformLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
            _teamLookup = state.GetComponentLookup<Team>(isReadOnly: true);
            _healthLookup = state.GetComponentLookup<CurrentHealth>(isReadOnly: true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _gridMap.Clear();
            
            foreach ((
                         RefRO<LocalTransform> transform, 
                         Entity entity) 
                     in SystemAPI.Query<
                         RefRO<LocalTransform>>()
                         .WithEntityAccess())
            {
                int2 cell = SpatialHashUtility.GetCell(transform.ValueRO.Position, CellSize);
                _gridMap.Add(SpatialHashUtility.Hash(cell), entity);
            }
            
            _transformLookup.Update(ref state);
            _teamLookup.Update(ref state);
            _healthLookup.Update(ref state);

            state.Dependency = new DetectJob
            {
                GridMap = _gridMap,
                TransformLookup = _transformLookup,
                TeamLookup = _teamLookup,
                HealthLookup = _healthLookup,
                DeltaTime = SystemAPI.Time.DeltaTime,
            }.ScheduleParallel(state.Dependency);
        }

        public void OnDestroy(ref SystemState state)
        {
            _gridMap.Dispose();
        }

        [BurstCompile]
        public partial struct DetectJob : IJobEntity
        {
            [ReadOnly]
            public NativeParallelMultiHashMap<int, Entity> GridMap;
            
            [ReadOnly]
            public ComponentLookup<LocalTransform> TransformLookup;
            
            [ReadOnly]
            public ComponentLookup<Team> TeamLookup;

            [ReadOnly]
            public ComponentLookup<CurrentHealth> HealthLookup;
            
            public float DeltaTime;

            private void Execute(
                Entity entity,
                in LocalTransform transform,
                in Team team,
                in DetectionRadius detectionRadius,
                ref TargetEntity target,
                ref DetectionCooldown cooldown
            )
            {
                cooldown.Time -= DeltaTime;
                
                if(cooldown.Time > 0)
                    return;

                cooldown.Time = cooldown.Duration;
                
                IsEnemyPredicate condition = new(
                    entity,
                    team.Value,
                    TeamLookup,
                    HealthLookup
                );

                target.Value = SpatialHashUtility.FindClosest(
                    GridMap,
                    transform.Position,
                    detectionRadius.Value,
                    CellSize,
                    in condition,
                    in TransformLookup
                );
            }
        }
    }
}