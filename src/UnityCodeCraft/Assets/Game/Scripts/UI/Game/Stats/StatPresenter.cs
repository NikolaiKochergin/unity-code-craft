using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public class StatPresenter : IGameUIInit, IGameUIDispose
    {
        private readonly PlayerContext _playerContext;
        private Subscription<int> _subscription;

        public StatPresenter(PlayerContext playerContext) => 
            _playerContext = playerContext;

        public void Init(IGameUI ui)
        {
            _subscription = _playerContext
                .GetValue(PlayerContextAPI.Character)
                .GetValue(GameEntityAPI.CurrentHealth)
                .Observe(current =>
                {
                    int max = _playerContext
                        .GetValue(PlayerContextAPI.Character)
                        .GetValue(GameEntityAPI.MaxHealth).Value;

                    StatView healthView = ui.GetValue(GameUIAPI.HealthView);
                    healthView.SetText(current.ToString());
                    healthView.SetProgress((float)current / max);
                });
        }

        public void Dispose(IGameUI entity) => 
            _subscription.Dispose();
    }
}