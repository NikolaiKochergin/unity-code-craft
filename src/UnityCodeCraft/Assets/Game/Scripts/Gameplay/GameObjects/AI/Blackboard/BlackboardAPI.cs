using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace Game
{
    [BlackboardAPI]
    public static class BlackboardAPI
    {
        public static readonly BlackboardValueKey<GameObject> Character = new(nameof(Character));
        
        // Combat
        public static readonly BlackboardValueKey<GameObject> Enemy = new(nameof(Enemy));
        public static readonly BlackboardValueKey<float> RangeAttackDistance = new(nameof(RangeAttackDistance));
        
        // Patrol
        public static readonly BlackboardValueKey<List<Vector3>> Waypoints = new(nameof(Waypoints));
        public static readonly BlackboardValueKey<int> WaypointIndex = new(nameof(WaypointIndex));
        
        // Move
        public static readonly BlackboardValueKey<GameObject> MoveTarget = new(nameof(MoveTarget));
        public static readonly BlackboardValueKey<Vector3> MoveTargetPosition = new(nameof(MoveTargetPosition));
        public static readonly BlackboardValueKey<Vector3> TargetPoint = new(nameof(TargetPoint));
        public static readonly BlackboardValueKey<float> StoppingDistance = new(nameof(StoppingDistance));
        
        // Sensing
        public static readonly BlackboardValueKey<Collider[]> ColliderBuffer = new(nameof(ColliderBuffer));
        public static readonly BlackboardValueKey<int> ColliderCount = new(nameof(ColliderCount));
    }
}