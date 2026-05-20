using System;
using System.Collections.Generic;
using Modules.AI;
using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    [Serializable]
    public class WaypointsBlackboardInstaller : IBlackboardInstaller
    {
        public void Install(Blackboard blackboard)
        {
            blackboard.SetReferenceValue(BlackboardAPI.Waypoints, new List<GameObject>());
            blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, 0);
            blackboard.SetReferenceValue(BlackboardAPI.WaypointPool, new ObjectPool<Transform>(OnCreate));
        }

        private Transform OnCreate() => 
            new GameObject("Waypoint").transform;
    }
}