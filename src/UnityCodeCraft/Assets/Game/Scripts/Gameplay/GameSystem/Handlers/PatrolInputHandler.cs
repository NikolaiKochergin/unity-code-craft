using System.Collections.Generic;
using Game;
using Modules.AI;
using UnityEngine;
using UnityEngine.Pool;

namespace SampleGame
{
    public sealed class PatrolInputHandler : InputHandler
    {
        [SerializeField] 
        private CommandMarkerView _commandMarkerView;
        
        [SerializeField] 
        private Blackboard _blackboard;
        
        [SerializeField]
        private KeyCode _keyCode = KeyCode.P;
        
        [SerializeField]
        private GameObject _character;

        [SerializeField]
        private InputHandler _next;
        
        private UnitCommandQueue _commandQueue;

        private readonly List<GameObject> _waypoints = new();
        private readonly ObjectPool<GameObject> _waypointsPool = new(() => new GameObject("Waypoint"));

        private void Start()
        {
            Blackboard blackboard = _character.GetComponentInChildren<Blackboard>();
            
            if (blackboard == null) 
                Debug.LogError("No Blackboard component found on " + _character);
            
            _commandQueue = blackboard?.GetValue(BlackboardAPI.CommandQueue);
            
            if (_commandQueue == null)
                Debug.LogError("CommandQueue doesn't exist on " + _character);
        }

        public override void Handle(ref InputContext context)
        {
            if (Input.GetKey(_keyCode) && context.leftClick)
            {
                GameObject point = null;
                
                if (context.point != null)
                {
                    point = _waypointsPool.Get();
                    point.transform.position = context.point.Value;
                    
                    _commandMarkerView.ShowPatrolMarker(context.point.Value);
                }
                else if (context.target != null && context.target != _character)
                {
                    point = context.target;
                    
                    _commandMarkerView.ShowPatrolMarker(context.target.transform);
                }

                if (context.enqueueCommand)
                {
                }
                else
                {
                    _commandQueue.Reset();
                    _waypoints.Clear();

                    _waypoints.Add(point);
                    
                    point = _waypointsPool.Get();
                    point.transform.position = _character.transform.position;
                }

                _waypoints.Add(point);
                _commandQueue.Add(new WayPointCommand(_waypoints, OnStop));
            }
            else if (_next)
                _next.Handle(ref context);
        }

        private void OnStop(List<GameObject> waypoints)
        {
            foreach (GameObject waypoint in waypoints)
                _waypointsPool.Release(waypoint);
        }
    }
}