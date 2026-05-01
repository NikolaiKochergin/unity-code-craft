using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public class UIInputPresenter : IEntityTick
    {
        private readonly IGameUI _ui;
        private readonly PlayerContext _playerContext;

        public UIInputPresenter(IGameUI ui, PlayerContext playerContext)
        {
            _ui = ui;
            _playerContext = playerContext;
        }

        public void Tick(IEntity entity, float deltaTime) =>
            _playerContext
                .GetValue(PlayerContextAPI.Character)
                .GetValue(GameEntityAPI.MoveRequest)
                .Invoke(_ui.GetValue(GameUIAPI.MoveJoystick).Direction);
    }
}