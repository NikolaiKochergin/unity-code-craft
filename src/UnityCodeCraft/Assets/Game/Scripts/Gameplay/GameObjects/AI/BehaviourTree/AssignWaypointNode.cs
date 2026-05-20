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
        [BlackboardValueKey(typeof(Vector3))]
        private string _positionKey;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (!_blackboard.TryGetValue(BlackboardAPI.Waypoints, out List<GameObject> waypoints) ||
                !_blackboard.TryGetValue(BlackboardAPI.WaypointIndex, out int index))
                return BehaviourResult.Failure;
            
            Transform destination = waypoints[index].transform;
            _blackboard.SetReferenceValue(_positionKey, destination);
            return BehaviourResult.Success;
        }
    }
}