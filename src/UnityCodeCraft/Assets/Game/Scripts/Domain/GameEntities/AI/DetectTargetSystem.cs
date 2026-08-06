using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Game
{
    [BurstCompile]
    [UpdateAfter(typeof(BuildSpatialHashSystem))]
    public partial struct DetectTargetSystem : ISystem
    {
        private ComponentLookup<LocalTransform> _transformLookup;
        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<CurrentHealth> _healthLookup;

        public void OnCreate(ref SystemState state)
        {
            _transformLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
            _teamLookup = state.GetComponentLookup<Team>(isReadOnly: true);
            _healthLookup = state.GetComponentLookup<CurrentHealth>(isReadOnly: true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _transformLookup.Update(ref state);
            _teamLookup.Update(ref state);
            _healthLookup.Update(ref state);

            state.Dependency = new DetectJob
            {
                TransformLookup = _transformLookup,
                TeamLookup = _teamLookup,
                HealthLookup = _healthLookup
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        public partial struct DetectJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<LocalTransform> TransformLookup;
            
            [ReadOnly]
            public ComponentLookup<Team> TeamLookup;

            [ReadOnly]
            public ComponentLookup<CurrentHealth> HealthLookup;

            private void Execute(
                Entity entity,
                in LocalTransform transform,
                in Team team,
                in DetectionRadius detectionRadius,
                ref TargetEntity target
            )
            {
                IsEnemyPredicate condition = new(
                    entity,
                    team.Value,
                    TeamLookup,
                    HealthLookup
                );

                target.Value = SpatialHash.FindClosest(
                    transform.Position,
                    detectionRadius.Value,
                    in condition,
                    in TransformLookup
                );
            }
        }
    }
}