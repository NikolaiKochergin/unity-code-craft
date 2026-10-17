using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class NetworkRunnerInstaller : MonoInstaller
    {
        [SerializeField] private KillProcessor _killProcessor;
        
        public override void InstallBindings()
        {
            Container.Bind<KillProcessor>().FromInstance(_killProcessor).AsSingle();
        }
    }
}