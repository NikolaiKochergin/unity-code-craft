using Game;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
    public sealed class AttackInputHandler : InputHandler
    {
        [SerializeField] 
        private CommandMarkerView _commandMarkerView;
        
        [SerializeField]
        private KeyCode _keyCode = KeyCode.A;

        [SerializeField]
        private GameObject _character;

        [SerializeField] 
        private Blackboard _blackboard;

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
                if (!context.enqueueCommand)
                    _commandQueue.Reset();
                
                if (context.point != null)
                {
                    _commandQueue.Add(new AttackCommand(new PositionWayPoint(context.point.Value)));
                    
                    _commandMarkerView.ShowAttackMarker(context.point.Value);
                }
                else if (context.target != null && context.target != _character)
                {
                    _commandQueue.Add(new AttackCommand(new TargetWayPoint(context.target.transform)));
                    
                    _commandMarkerView.ShowAttackMarker(context.target.transform);
                }
            }
            else if (_next)
                _next.Handle(ref context);
        }
    }
}