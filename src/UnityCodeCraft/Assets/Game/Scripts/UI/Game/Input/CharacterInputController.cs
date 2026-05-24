using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Game.UI
{
    public sealed class CharacterInputController : IGameUIInit, IGameUITick
    {
        private IGameUI _ui;
        private IGameEntity _character;
        private InputMap _inputMap;

        public void Init(IGameUI ui)
        {
            _ui = ui;
            _character = ui.GetValue(GameUIAPI.GameContext).GetValue(GameContextAPI.Character);
            _inputMap = ui.GetValue(GameUIAPI.InputMap);
        }

        public void Tick(IGameUI context, float deltaTime)
        {
            ProcessMove();
            ProcessAim();
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
    }
}