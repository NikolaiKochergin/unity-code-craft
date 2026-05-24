using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public class HealthScreenViewPresenter : IGameUIInit, IGameUIDispose
    {
        private Subscription<int> _subscription;

        public void Init(IGameUI ui) =>
            _subscription = ui
                .GetValue(GameUIAPI.GameContext)
                .GetValue(GameContextAPI.Character)
                .GetValue(GameEntityAPI.TakeDamageCommand)
                .Subscribe(damage =>
                {
                    HealthScreenView healthScreen = ui.GetValue(GameUIAPI.HealthScreenView);
                    healthScreen.TakeDamage(damage);

                    int currentHealth = ui
                        .GetValue(GameUIAPI.GameContext)
                        .GetValue(GameContextAPI.Character)
                        .GetValue(GameEntityAPI.CurrentHealth).Value;
                    
                    int maxHealth = ui
                        .GetValue(GameUIAPI.GameContext)
                        .GetValue(GameContextAPI.Character)
                        .GetValue(GameEntityAPI.MaxHealth).Value;
                    
                    healthScreen.ChangePercent((float)currentHealth / maxHealth);
                });

        public void Dispose(IGameUI entity) => 
            _subscription.Dispose();
    }
}