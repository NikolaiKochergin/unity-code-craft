using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameWorldInstaller : MonoInstaller
    {
        [SerializeField] private NetworkObject _portal;
        [SerializeField] private SpawnPointService _playerSpawnPointService;
        [SerializeField] private SpawnPointService _enemySpawnPointService;

        public override void InstallBindings()
        {
            Container.Bind<NetworkObject>().WithId(Tags.Portal).FromInstance(_portal);
            Container.Bind<SpawnPointService>().WithId(Tags.Player).FromInstance(_playerSpawnPointService);
            Container.Bind<SpawnPointService>().WithId(Tags.Enemy).FromInstance(_enemySpawnPointService);
        }
    }
}