using UnityEngine;
using Zenject;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        [SerializeField] private CoinParticleView _coinParticle;
        [SerializeField] private PlanetPopupPresenter _planetPopupPresenter;

        public override void InstallBindings()
        {
            Container
                .Bind<CoinParticleView>()
                .FromInstance(_coinParticle)
                .AsCached();

            Container
                .Bind<PlanetPopupPresenter>()
                .FromInstance(_planetPopupPresenter)
                .AsSingle();
        }
    }
}