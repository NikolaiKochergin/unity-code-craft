using SampleGame;
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
        private ComponentLookup<MaxHealth> _maxHealthLookup;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CastleReferences>();
            
            _gridMap = new NativeParallelMultiHashMap<int, Entity>(128, Allocator.Persistent);
            
            _transformLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
            _teamLookup = state.GetComponentLookup<Team>(isReadOnly: true);
            _healthLookup = state.GetComponentLookup<CurrentHealth>(isReadOnly: true);
            _maxHealthLookup = state.GetComponentLookup<MaxHealth>(isReadOnly: true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency.Complete();
            
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
            _maxHealthLookup.Update(ref state);

            CastleReferences castles = SystemAPI.GetSingleton<CastleReferences>();
            
            state.Dependency = new DetectJob
            {
                GridMap = _gridMap,
                TransformLookup = _transformLookup,
                TeamLookup = _teamLookup,
                HealthLookup = _healthLookup,
                MaxHealthLookup = _maxHealthLookup,
                DeltaTime = SystemAPI.Time.DeltaTime,
                BlueTeamCastle = castles.BlueCastle,
                RedTeamCastle = castles.RedCastle
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
            
            [ReadOnly]
            public ComponentLookup<MaxHealth> MaxHealthLookup;
            
            public Entity BlueTeamCastle;
            public Entity RedTeamCastle;
            
            public float DeltaTime;

            private void Execute(
                Entity entity,
                in LocalTransform transform,
                in Team team,
                in DetectionTeam detectionTeam,
                in DetectionRadius detectionRadius,
                ref TargetEntity target,
                ref DetectionCooldown cooldown
            )
            {
                cooldown.Time -= DeltaTime;
                
                if(cooldown.Time > 0)
                    return;

                cooldown.Time = cooldown.Duration;
                
                IsEnemyPredicate enemyCondition = new(
                    entity,
                    team.Value,
                    TeamLookup,
                    HealthLookup
                );
                
                IsFriendPredicate friendCondition = new(
                    entity,
                    team.Value,
                    TeamLookup,
                    HealthLookup,
                    MaxHealthLookup
                );

                target.Value = team.Value == detectionTeam.TargetTeam
                    ? SpatialHashUtility.FindClosest(
                        GridMap,
                        transform.Position,
                        detectionRadius.Value,
                        CellSize,
                        in friendCondition,
                        in TransformLookup)
                    : SpatialHashUtility.FindClosest(
                        GridMap,
                        transform.Position,
                        detectionRadius.Value,
                        CellSize,
                        in enemyCondition,
                        in TransformLookup);

                if (target.Value != Entity.Null) 
                    return;
                
                target.Value = team.Value == detectionTeam.TargetTeam
                    ? team.Value == TeamType.Blue
                        ? BlueTeamCastle
                        : RedTeamCastle
                    : team.Value == TeamType.Blue
                        ? RedTeamCastle
                        : BlueTeamCastle;
            }
        }
    }
}