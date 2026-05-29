using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace Game
{
    public class AssignWaypointNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        [SerializeField]
        [BlackboardValueKey(typeof(GameObject))]
        private string _positionKey;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (!_blackboard.TryGetValue(BlackboardAPI.Waypoints, out List<GameObject> waypoints) ||
                !_blackboard.TryGetValue(BlackboardAPI.WaypointIndex, out int index))
                return BehaviourResult.Failure;
            
            GameObject destination = waypoints[index];
            _blackboard.SetReferenceValue(_positionKey, destination);
            return BehaviourResult.Success;
        }
    }
}