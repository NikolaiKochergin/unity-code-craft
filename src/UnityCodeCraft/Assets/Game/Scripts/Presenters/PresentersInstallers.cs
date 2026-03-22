using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    [CreateAssetMenu(
        fileName = "PresentersInstallers",
        menuName = "Zenject/New PresentersInstallers"
    )]
    public sealed class PresentersInstallers : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<MoneyPresenter>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<PlanetsPresenter>()
                .AsSingle();

            Container
                .Bind<PlanetPopupPresenter>()
                .AsSingle();
            
            Container
                .BindFactory<Planet, PlanetPresenter, PlanetPresenterFactory>()
                .AsSingle();
        }
    }
}