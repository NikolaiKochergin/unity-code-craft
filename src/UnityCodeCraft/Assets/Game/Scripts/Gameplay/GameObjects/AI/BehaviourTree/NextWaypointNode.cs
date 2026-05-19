using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace Game
{
    public class NextWaypointNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (!_blackboard.TryGetValue(BlackboardAPI.Waypoints, out List<Vector3> waypoints))
                return BehaviourResult.Failure;

            _blackboard.TryGetValue(BlackboardAPI.WaypointIndex, out int index);
            _blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, (index + 1) % waypoints.Count);
            return BehaviourResult.Success;
        }
    }
}