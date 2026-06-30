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
        
        private AiCommandQueue _commandQueue;

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
                if(!_blackboard.TryGetValue(BlackboardAPI.Waypoints, out List<IWayPoint> wayPoints))
                    return;

                IWayPoint wayPoint = null;

                if (context.point != null)
                {
                    wayPoint = new PositionWayPoint(context.point.Value);
                    _commandMarkerView.ShowPatrolMarker(context.point.Value);
                }
                else if (context.target != null && context.target != _character)
                {
                    wayPoint = new TargetWayPoint(context.target.transform);
                    _commandMarkerView.ShowPatrolMarker(context.target.transform);
                }
                
                if (context.enqueueCommand)
                {
                    if (_commandQueue.CurrentCommand is WayPointCommand)
                    {
                        wayPoints.Add(wayPoint);
                        return;
                    }
                }
                else
                {
                    _commandQueue.Reset();
                }
                
                wayPoints.Clear();
                wayPoints.Add(new PositionWayPoint(_character.transform.position));
                wayPoints.Add(wayPoint);
                
                _commandQueue.Add(new WayPointCommand());
            }
            else if (_next)
                _next.Handle(ref context);
        }
    }
}