using System.Collections.Generic;
using Game;
using Modules.AI;
using UnityEngine;

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

        public override void Handle(ref InputContext context)
        {
            if (Input.GetKey(_keyCode) && context.leftClick)
            {
              
                if (context.point != null)
                {
                    // TODO: Point destination
                    
                    Debug.Log("<color=orange> Enque " + context.point);
                    if(!_blackboard.TryGetValue(BlackboardAPI.Waypoints, out List<Vector3> waypoints) &&
                       !context.point.HasValue)
                        return;
                    
                    waypoints.Add(context.point.Value);
                    
                    _commandMarkerView.ShowPatrolMarker(context.point.Value);
                }
                else if (context.target != null && context.target != _character)
                {
                    // TODO: Target destination
                    
                    _commandMarkerView.ShowPatrolMarker(context.target.transform);
                }

                if (context.enqueueCommand)
                {
                    // TODO: If current command is patrol the add waypoint else enqueue command
                }
                else
                {
                    // TODO: Switch to patrol
                }
            }
            else if (_next)
                _next.Handle(ref context);
        }
    }
}