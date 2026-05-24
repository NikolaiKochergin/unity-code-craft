using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public class KillsViewPresenter : IGameUIInit, IGameUIDispose
    {
        private Subscription<int> _subscription;

        public void Init(IGameUI entity) =>
            _subscription = entity
                .GetValue(GameUIAPI.GameContext)
                .GetValue(GameContextAPI.Score)
                .Observe(score => entity
                    .GetValue(GameUIAPI.KillsView)
                    .SetText(score.ToString()));

        public void Dispose(IGameUI entity) => 
            _subscription.Dispose();
    }
}