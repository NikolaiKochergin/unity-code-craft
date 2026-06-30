using System;
using System.Collections.Generic;
using Modules.AI;

namespace Game
{
    [Serializable]
    public class WaypointsBlackboardInstaller : IBlackboardInstaller
    {
        public void Install(Blackboard blackboard)
        {
            blackboard.SetReferenceValue(BlackboardAPI.Waypoints, new List<IWayPoint>());
            blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, 0);
        }
    }
}