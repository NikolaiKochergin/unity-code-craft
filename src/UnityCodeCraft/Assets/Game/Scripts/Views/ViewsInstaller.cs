using UnityEngine;
using Zenject;

namespace Game.Views
{
    public sealed class ViewsInstaller : MonoInstaller
    {
        [SerializeField] private MoneyParticleAnimatorPresenter _moneyParticleAnimatorPresenter;
        [SerializeField] private PlanetPopupPresenter _planetPopupPresenter;

        public override void InstallBindings()
        {
            Container
                .Bind<MoneyParticleAnimatorPresenter>()
                .FromInstance(_moneyParticleAnimatorPresenter)
                .AsCached();

            Container
                .Bind<PlanetPopupPresenter>()
                .FromInstance(_planetPopupPresenter)
                .AsSingle();
        }
    }
}