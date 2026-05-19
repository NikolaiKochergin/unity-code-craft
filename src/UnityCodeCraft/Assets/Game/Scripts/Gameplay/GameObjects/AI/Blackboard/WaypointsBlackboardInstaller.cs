using System;
using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class WaypointsBlackboardInstaller : IBlackboardInstaller
    {
        [SerializeField] private int _waypointIndex;
        [SerializeField] private List<Vector3> _waypoints;
        
        public void Install(Blackboard blackboard)
        {
            blackboard.SetReferenceValue(BlackboardAPI.Waypoints, _waypoints);
            blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, _waypointIndex);
        }
    }
}