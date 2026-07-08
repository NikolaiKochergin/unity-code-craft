using System.Collections.Generic;
using Game;
using Modules.AI;
using UnityEngine;

namespace SampleGame
{
    public sealed class StopCommandHandler : InputHandler
    {
        [SerializeField]
        private KeyCode _keyCode = KeyCode.S;
        
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
                AICommandUseCase.ResetQueue(_blackboard);

                Vector3 characterPosition = _character.transform.position;
                _blackboard.SetReferenceValue(BlackboardAPI.BasePoint, new PositionPoint(characterPosition));
            }
            else if (_next)
                _next.Handle(ref context);
        }
    }
}