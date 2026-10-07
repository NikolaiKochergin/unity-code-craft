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
        private GameCycle _gameCycle;
        private Store _store;

        [Inject]
        public void Construct(
            PlayerCharacterProvider characterProvider, 
            GameCycle gameCycle,
            Store store)
        {
            _store = store;
            _gameCycle = gameCycle;
            _characterProvider = characterProvider;
        }
        
        public override void FixedUpdateNetwork()
        {
            if(_gameCycle.State != GameState.Run)
                return;
            
            NetworkObject character = _characterProvider.Character;
            if(character == null)
                return;
            
            if(GetInput(out InputData inputData))
            {
                NetworkButtons inputButtons = inputData.buttons;
                ProcessMove(character, inputData.moveDirection);
                ProcessSetMine(character, inputButtons);      
                ProcessSetArcher(character, inputButtons);      
                _previousButtons = inputButtons;
            }
            else
            {
                StopMove(character);
            }
        }

        private void ProcessSetMine(NetworkObject character, NetworkButtons inputButtons)
        {
            if (inputButtons.WasPressed(_previousButtons, InputButtons.Mine))
                _store.TryBuyMineFor(character);
        }

        private void ProcessSetArcher(NetworkObject character, NetworkButtons inputButtons)
        {
            if(inputButtons.WasPressed(_previousButtons, InputButtons.Turret))
                _store.TryBuyTurretFor(character);
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