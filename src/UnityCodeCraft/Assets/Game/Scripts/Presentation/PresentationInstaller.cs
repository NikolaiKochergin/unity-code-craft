using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    [CreateAssetMenu(
        fileName = "PresentationInstaller",
        menuName = "Zenject/New Presentation Installer"
    )]
    public class PresentationInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IControlsPresenter>()
                .To<ControlsPresenter>()
                .AsSingle();
        }
    }
}