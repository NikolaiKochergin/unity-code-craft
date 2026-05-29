using System;
using System.Collections.Generic;
using Game;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
    public class WayPointCommand : IUnitCommand
    {
        private readonly List<GameObject> _waypoints;
        private readonly Action<List<GameObject>> _onStop;

        public WayPointCommand(List<GameObject> waypoints, Action<List<GameObject>> onStop)
        {
            _onStop = onStop;
            _waypoints = waypoints;
        }

        public void Start(Blackboard blackboard)
        {
            blackboard.SetReferenceValue(BlackboardAPI.Waypoints, _waypoints);
            blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, 0);
        }

        public void Stop(Blackboard blackboard)
        {
            _onStop?.Invoke(blackboard.GetValue(BlackboardAPI.Waypoints));
            _waypoints.Clear();
        }
    }
}