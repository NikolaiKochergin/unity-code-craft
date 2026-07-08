using Game;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
    public sealed class HoldPositionHandler : InputHandler
    {
        [SerializeField]
        private KeyCode _keyCode = KeyCode.H;

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
            if (Input.GetKeyDown(_keyCode))
            {
                if (!context.enqueueCommand)
                    AICommandUseCase.ResetQueue(_blackboard);
                
                _blackboard
                    .GetValue(BlackboardAPI.CommandQueue)
                    .Enqueue(new HoldPositionCommand());
            }
            else if (_next) 
                _next.Handle(ref context);
        }
    }
}