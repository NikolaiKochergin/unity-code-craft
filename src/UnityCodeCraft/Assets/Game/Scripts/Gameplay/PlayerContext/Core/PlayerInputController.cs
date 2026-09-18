using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class PlayerInputController : NetworkBehaviour
    {
        [Networked]
        private NetworkButtons _previousButtons { get; set; }

        private PlayerCharacterProvider _characterProvider;
        
        [Inject]
        public void Construct(PlayerCharacterProvider characterProvider)
        {
            _characterProvider = characterProvider;
        }
        
        public override void FixedUpdateNetwork()
        {
            NetworkObject character = _characterProvider.Character;
            if(character == null)
                return;
            
            if(GetInput(out InputData inputData))
            {
                NetworkButtons inputButtons = inputData.buttons;
                ProcessMove(character, inputData.moveDirection);
                
                _previousButtons = inputButtons;
            }
            else
            {
                StopMove(character);
            }
        }

        private void ProcessMove(NetworkObject character, Vector2 inputDirection)
        {
            MoveComponent moveComponent = character.GetBehaviour<MoveComponent>();
            Vector3 moveDirection = new(inputDirection.x, 0, inputDirection.y);
            moveComponent.Move(moveDirection);
        }

        private void StopMove(NetworkObject character)
        {
            character.GetBehaviour<MoveComponent>().Stop();
        }
    }
}