using Game;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
    public sealed class FollowInputHandler : InputHandler
    {
        [SerializeField] 
        private CommandMarkerView _commandMarkerView;
        
        [SerializeField]
        private KeyCode _keyCode = KeyCode.F;

        [SerializeField]
        private GameObject _character;

        [SerializeField]
        private InputHandler _next;
        
        private Blackboard _blackboard;

        private void Start()
        {
            _blackboard = _character.GetComponentInChildren<Blackboard>();
            
            if (!_blackboard) 
                Debug.LogError("No Blackboard component found on " + _character);
        }

        public override void Handle(ref InputContext context)
        {
            if (Input.GetKey(_keyCode) && context.leftClick)
            {
                if (!context.target || context.target == _character) 
                    return;
                
                if (!context.enqueueCommand)
                    ResetUseCase.ResetQueue(_blackboard);
                
                _blackboard
                    .GetValue(BlackboardAPI.CommandQueue)
                    .Enqueue(new FollowCommand(context.target));
                    
                _commandMarkerView.ShowFollowMarker(context.target.transform);
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}