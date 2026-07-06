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

        private Blackboard _blackboard;

        private void Start()
        {
            _blackboard = _character.GetComponentInChildren<Blackboard>();
            
            if (_blackboard == null) 
                Debug.LogError("No Blackboard component found on " + _character);
        }

        public override void Handle(ref InputContext context)
        {
            if (context.rightClick)
            {
                IPoint targetPoint = null;
                
                if (context.target && context.target != _character)
                {
                    targetPoint = new TargetPoint(context.target.transform);
                    _commandMarkerView.ShowMoveMarker(context.target.transform);
                }
                else if (context.point != null)
                {
                    targetPoint = new PositionPoint(context.point.Value);
                    _commandMarkerView.ShowMoveMarker(context.point.Value);
                }
                
                if (!context.enqueueCommand)
                    ResetUseCase.ResetQueue(_blackboard);
                
                _blackboard
                    .GetValue(BlackboardAPI.CommandQueue)
                    .Enqueue(new MoveCommand(targetPoint));
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}