using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public class HealthScreenViewPresenter : IGameUIInit, IGameUIDispose
    {
        private readonly IPlayerContext _playerContext;
        private Subscription<int> _subscription;

        public HealthScreenViewPresenter(IPlayerContext playerContext) => 
            _playerContext = playerContext;

        public void Init(IGameUI ui) =>
            _subscription = _playerContext
                .GetValue(PlayerContextAPI.Character)
                .GetValue(GameEntityAPI.TakeDamageCommand)
                .Subscribe(damage =>
                {
                    HealthScreenView healthScreen = ui.GetValue(GameUIAPI.HealthScreenView);
                    healthScreen.TakeDamage(damage);

                    int currentHealth = _playerContext
                        .GetValue(PlayerContextAPI.Character)
                        .GetValue(GameEntityAPI.CurrentHealth).Value;
                    
                    int maxHealth = _playerContext
                        .GetValue(PlayerContextAPI.Character)
                        .GetValue(GameEntityAPI.MaxHealth).Value;
                    
                    healthScreen.ChangePercent((float)currentHealth / maxHealth);
                });

        public void Dispose(IGameUI entity) => 
            _subscription.Dispose();
    }
}