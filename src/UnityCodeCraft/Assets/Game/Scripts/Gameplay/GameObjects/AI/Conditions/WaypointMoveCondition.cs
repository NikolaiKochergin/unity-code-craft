using System;
using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class WaypointMoveCondition : ICondition
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        public bool Invoke() => 
            _blackboard.TryGetValue(BlackboardAPI.Waypoints, out List<IWayPoint> waypoints) && 
            waypoints.Count > 0;
    }
}