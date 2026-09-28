using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class NetworkRunnerInstaller : MonoInstaller
    {
        [SerializeField] private HitboxManager _hitboxManager;
        [SerializeField] private KillNotificator _killNotificator;
        
        public override void InstallBindings()
        {
            Container.Bind<KillNotificator>().FromInstance(_killNotificator).AsSingle();
            Container.Bind<HitboxManager>().FromInstance(_hitboxManager).AsSingle();
        }
    }
}