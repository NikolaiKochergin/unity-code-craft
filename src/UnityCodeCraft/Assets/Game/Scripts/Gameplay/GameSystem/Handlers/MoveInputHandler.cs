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
        private Blackboard _blackboard;
        
        [SerializeField]
        private GameObject _character;

        [SerializeField]
        private InputHandler _next;

        public override void Handle(ref InputContext context)
        {
            if (context.rightClick)
            {
                if (context.target != null && context.target != _character)
                {
                    
                    // TODO: Move to target
                    _blackboard.SetReferenceValue(BlackboardAPI.MoveTarget, context.target);
                    
                    _commandMarkerView.ShowMoveMarker(context.target.transform);
                }
                else if (context.point != null)
                {
                    
                    // TODO: Move to point
                    _blackboard.SetPrimitiveValue(BlackboardAPI.MoveTargetPosition, context.point.Value);
                    
                    _commandMarkerView.ShowMoveMarker(context.point.Value);
                }
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}