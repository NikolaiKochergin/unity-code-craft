using Game;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
    public sealed class MoveInputHandler : InputHandler
    {
        [SerializeField] 
        private CommandMarkerView _commandMarkerView;
        
        [SerializeField]
        private GameObject _character;

        [SerializeField]
        private InputHandler _next;

        private UnitCommandQueue _commandQueue;

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
            if (context.rightClick)
            {
                if (context.target != null && context.target != _character)
                {
                    if (!context.enqueueCommand)
                        _commandQueue.Reset();
                    
                    _commandQueue.Add(new MoveCommand(context.target));
                    
                    _commandMarkerView.ShowMoveMarker(context.target.transform);
                }
                else if (context.point != null)
                {
                    if (!context.enqueueCommand)
                        _commandQueue.Reset();
                    
                    _commandQueue.Add(new MoveCommand(context.point.Value));
                    
                    _commandMarkerView.ShowMoveMarker(context.point.Value);
                }
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}