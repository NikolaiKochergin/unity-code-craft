using System.Collections.Generic;
using Modules.AI;
using SampleGame;
using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    [BlackboardAPI]
    public static class BlackboardAPI
    {
        public static readonly BlackboardValueKey<GameObject> Character = new(nameof(Character));
        public static readonly BlackboardValueKey<IWayPoint> BasePoint = new(nameof(BasePoint));
        public static readonly BlackboardValueKey<AiCommandQueue> CommandQueue = new(nameof(CommandQueue));
        
        // Combat
        public static readonly BlackboardValueKey<TeamType> Team = new(nameof(Team));
        public static readonly BlackboardValueKey<GameObject> Enemy = new(nameof(Enemy));
        public static readonly BlackboardValueKey<GameObject> AttackTarget = new(nameof(AttackTarget));
        public static readonly BlackboardValueKey<float> AttackDistance = new(nameof(AttackDistance));
        
        // Patrol
        public static readonly BlackboardValueKey<ObjectPool<GameObject>> PointsPool = new(nameof(PointsPool));
        public static readonly BlackboardValueKey<List<IWayPoint>> Waypoints = new(nameof(Waypoints));
        public static readonly BlackboardValueKey<int> WaypointIndex = new(nameof(WaypointIndex));
        
        // Move
        public static readonly BlackboardValueKey<IWayPoint> MovePoint = new(nameof(MovePoint));
        public static readonly BlackboardValueKey<float> StoppingDistance = new(nameof(StoppingDistance));
        public static readonly BlackboardValueKey<float> TargetStoppingDistance = new(nameof(TargetStoppingDistance));
        
        // Follow
        public static readonly BlackboardValueKey<GameObject> FollowPoint = new(nameof(FollowPoint));

        // Holding
        public static readonly BlackboardValueKey<IWayPoint> HoldPoint = new(nameof(HoldPoint));

        // Sensing
        public static readonly BlackboardValueKey<Collider[]> ColliderBuffer = new(nameof(ColliderBuffer));
        public static readonly BlackboardValueKey<int> ColliderCount = new(nameof(ColliderCount));
    }
}