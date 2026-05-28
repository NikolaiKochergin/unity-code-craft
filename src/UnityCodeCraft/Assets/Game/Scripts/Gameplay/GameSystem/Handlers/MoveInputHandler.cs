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
                UnitCommandQueue queue = _blackboard.GetValue(BlackboardAPI.CommandQueue);
                
                if (context.target != null && context.target != _character)
                {
                    if (!context.enqueueCommand)
                        queue.Reset();
                    
                    queue.Add(new MoveCommand(context.target));
                    
                    _commandMarkerView.ShowMoveMarker(context.target.transform);
                }
                else if (context.point != null)
                {
                    if (!context.enqueueCommand)
                        queue.Reset();
                    
                    queue.Add(new MoveCommand(context.point.Value));
                    
                    _commandMarkerView.ShowMoveMarker(context.point.Value);
                }
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}