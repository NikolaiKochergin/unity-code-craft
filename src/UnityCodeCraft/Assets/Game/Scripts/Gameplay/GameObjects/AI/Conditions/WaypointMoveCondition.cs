using System;
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
            _blackboard.GetValue(BlackboardAPI.Waypoints).Count > 0;
    }
}