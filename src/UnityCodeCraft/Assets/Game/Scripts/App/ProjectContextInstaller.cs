using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private GameSessionClient _sessionClient;

        public override void InstallBindings()
        {
            Container.Bind<GameSessionClient>().FromInstance(_sessionClient).AsSingle();
        }
    }
}