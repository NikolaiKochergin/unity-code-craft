using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public class UIInputPresenter : IEntityTick
    {
        private readonly IGameUI _ui;
        private readonly GameContext _gameContext;

        public UIInputPresenter(IGameUI ui, GameContext gameContext)
        {
            _ui = ui;
            _gameContext = gameContext;
        }

        public void Tick(IEntity entity, float deltaTime) =>
            _gameContext
                .GetValue(GameContextAPI.PlayerContext)
                .GetValue(PlayerContextAPI.Character)
                .GetValue(GameEntityAPI.MoveRequest)
                .Invoke(_ui.GetValue(GameUIAPI.MoveJoystick).Direction);
    }
}