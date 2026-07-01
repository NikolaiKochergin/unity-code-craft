using System.Collections.Generic;
using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    [BlackboardAPI]
    public static class BlackboardAPI
    {
        public static readonly BlackboardValueKey<GameObject> Character = new(nameof(Character));
        public static readonly BlackboardValueKey<IPoint> BasePoint = new(nameof(BasePoint));
        public static readonly BlackboardValueKey<AICommandQueue> CommandQueue = new(nameof(CommandQueue));
        
        // Combat
        public static readonly BlackboardValueKey<TeamType> Team = new(nameof(Team));
        public static readonly BlackboardValueKey<GameObject> Enemy = new(nameof(Enemy));
        public static readonly BlackboardValueKey<IPoint> AttackTarget = new(nameof(AttackTarget));
        public static readonly BlackboardValueKey<float> AttackDistance = new(nameof(AttackDistance));
        
        // Patrol
        public static readonly BlackboardValueKey<List<IPoint>> Waypoints = new(nameof(Waypoints));
        public static readonly BlackboardValueKey<int> WaypointIndex = new(nameof(WaypointIndex));
        
        // Move
        public static readonly BlackboardValueKey<IPoint> MovePoint = new(nameof(MovePoint));
        public static readonly BlackboardValueKey<float> StoppingDistance = new(nameof(StoppingDistance));
        public static readonly BlackboardValueKey<float> TargetStoppingDistance = new(nameof(TargetStoppingDistance));
        
        // Follow
        public static readonly BlackboardValueKey<GameObject> FollowTarget = new(nameof(FollowTarget));

        // Holding
        public static readonly BlackboardValueKey<IPoint> HoldPoint = new(nameof(HoldPoint));

        // Sensing
        public static readonly BlackboardValueKey<Collider[]> ColliderBuffer = new(nameof(ColliderBuffer));
        public static readonly BlackboardValueKey<int> ColliderBufferSize = new(nameof(ColliderBufferSize));
    }
}