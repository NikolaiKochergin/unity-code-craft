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
            blackboard.SetReferenceValue(BlackboardAPI.PointsPool, new ObjectPool<GameObject>(OnCreate));
        }

        private GameObject OnCreate() => new("Waypoint");
    }
}