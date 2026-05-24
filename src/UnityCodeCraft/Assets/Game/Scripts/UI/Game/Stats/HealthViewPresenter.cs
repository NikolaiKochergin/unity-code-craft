using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public class HealthViewPresenter : IGameUIInit, IGameUIDispose
    {
        private Subscription<int> _subscription;

        public void Init(IGameUI ui) =>
            _subscription = ui
                .GetValue(GameUIAPI.GameContext)
                .GetValue(GameContextAPI.Character)
                .GetValue(GameEntityAPI.CurrentHealth)
                .Observe(current =>
                {
                    int max = ui
                        .GetValue(GameUIAPI.GameContext)
                        .GetValue(GameContextAPI.Character)
                        .GetValue(GameEntityAPI.MaxHealth).Value;

                    StatView healthView = ui.GetValue(GameUIAPI.HealthView);
                    healthView.SetText(current.ToString());
                    healthView.SetProgress((float)current / max);
                });

        public void Dispose(IGameUI entity) => 
            _subscription.Dispose();
    }
}