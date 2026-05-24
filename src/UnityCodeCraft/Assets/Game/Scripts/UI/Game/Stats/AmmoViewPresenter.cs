using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;

namespace Game.UI
{
    public class AmmoViewPresenter : IGameUIInit, IGameUIDispose
    {
        private Subscription<int> _subscription;
        
        public void Init(IGameUI ui)
        {
            _subscription = ui
                .GetValue(GameUIAPI.GameContext)
                .GetValue(GameContextAPI.Character)
                .GetValue(GameEntityAPI.Weapon).Value
                .GetValue(GameEntityAPI.Ammo)
                .Observe(curren =>
                {
                    int max = ui
                        .GetValue(GameUIAPI.GameContext)
                        .GetValue(GameContextAPI.Character)
                        .GetValue(GameEntityAPI.Weapon).Value
                        .GetValue(GameEntityAPI.MaxAmmo).Value;

                    StatView ammoView = ui.GetValue(GameUIAPI.AmmoView);
                    ammoView.SetText(curren.ToString());
                    ammoView.SetProgress((float)curren / max);
                });
        }

        public void Dispose(IGameUI entity) => 
            _subscription.Dispose();
    }
}