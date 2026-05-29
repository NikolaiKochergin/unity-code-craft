using System;
using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class WaypointsBlackboardInstaller : IBlackboardInstaller
    {
        public void Install(Blackboard blackboard)
        {
            blackboard.SetReferenceValue(BlackboardAPI.Waypoints, new List<GameObject>());
            blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, 0);
        }
    }
}