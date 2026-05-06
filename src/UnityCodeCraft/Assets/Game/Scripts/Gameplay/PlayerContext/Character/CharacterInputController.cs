using Atomic.Entities;
using Game.UI;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class CharacterInputController : IPlayerContextInit, IPlayerContextTick
    {
        private readonly GameUI _ui;
        private IGameEntity _character;
        private InputMap _inputMap;

        public CharacterInputController(GameUI ui) => 
            _ui = ui;

        public void Init(IPlayerContext context)
        {
            _character = context.GetValue(PlayerContextAPI.Character);
            _inputMap = context.GetValue(PlayerContextAPI.InputMap);
        }

        public void Tick(IPlayerContext context, float deltaTime)
        {
            ProcessMove();
            ProcessAim();
            ProcessFire();
        }

        private void ProcessMove()
        {
            Vector2 mapDirection = _inputMap.GetMoveDirection();
            Vector2 uiDirection = _ui.GetMoveDirection();

            Vector2 direction = mapDirection == Vector2.zero ? uiDirection : mapDirection;
            _character
                .GetValue(GameEntityAPI.MoveRequest)
                .Invoke(new Vector3(direction.x, 0, direction.y));
        }

        private void ProcessAim()
        {
            Vector2 mapDirection = _inputMap.GetAimDirection();
            Vector2 uiDirection = _ui.GetAimDirection();
            
            Vector2 direction = mapDirection == Vector2.zero ? uiDirection : mapDirection;
            _character
                .GetValue(GameEntityAPI.AimRequest)
                .Invoke(new Vector3(direction.x, 0, direction.y));
        }

        private void ProcessFire()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _character
                    .GetValue(GameEntityAPI.FireRequest).Invoke();
            }
        }
    }
}