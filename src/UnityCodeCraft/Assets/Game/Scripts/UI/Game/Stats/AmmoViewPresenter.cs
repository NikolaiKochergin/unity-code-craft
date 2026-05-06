using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public class AmmoViewPresenter : IGameUIInit, IGameUIDispose
    {
        private readonly IPlayerContext _playerContext;
        private Subscription<int> _subscription;

        public AmmoViewPresenter(IPlayerContext playerContext) => 
            _playerContext = playerContext;
        
        public void Init(IGameUI ui)
        {
            _subscription = _playerContext
                .GetValue(PlayerContextAPI.Character)
                .GetValue(GameEntityAPI.Weapon).Value
                .GetValue(GameEntityAPI.Ammo)
                .Observe(curren =>
                {
                    int max = 8;

                    StatView ammoView = ui.GetValue(GameUIAPI.AmmoView);
                    ammoView.SetText(curren.ToString());
                    ammoView.SetProgress((float)curren / max);
                });
        }

        public void Dispose(IGameUI entity) => 
            _subscription.Dispose();
    }
}