using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public class KillsViewPresenter : IGameUIInit, IGameUIDispose
    {
        private readonly IPlayerContext _playerContext;
        private Subscription<int> _subscription;

        public KillsViewPresenter(IPlayerContext playerContext) => 
            _playerContext = playerContext;

        public void Init(IGameUI entity) =>
            _subscription = _playerContext
                .GetValue(PlayerContextAPI.Score)
                .Observe(score => entity
                    .GetValue(GameUIAPI.KillsView)
                    .SetText(score.ToString()));

        public void Dispose(IGameUI entity) => 
            _subscription.Dispose();
    }
}