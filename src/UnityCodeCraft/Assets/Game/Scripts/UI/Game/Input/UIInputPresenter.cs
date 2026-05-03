using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public class UIInputPresenter : IGameUITick
    {
        private readonly PlayerContext _playerContext;

        public UIInputPresenter(PlayerContext playerContext) => 
            _playerContext = playerContext;

        public void Tick(IGameUI ui, float deltaTime) =>
            _playerContext
                .GetValue(PlayerContextAPI.Character)
                .GetValue(GameEntityAPI.MoveRequest)
                .Invoke(ui.GetValue(GameUIAPI.MoveJoystick).Direction);
    }
}