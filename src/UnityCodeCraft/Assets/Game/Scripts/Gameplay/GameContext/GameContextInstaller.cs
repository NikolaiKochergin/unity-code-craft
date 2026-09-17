using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameContextInstaller : MonoInstaller
    {
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private PlayerCharacterSpawner _characterSpawner;
        [SerializeField] private SpawnPointService _spawnPointService;

        public override void InstallBindings()
        {
            Container.Bind<PlayerManager>().FromInstance(_playerManager).AsSingle();
            Container.Bind<PlayerCharacterSpawner>().FromInstance(_characterSpawner).AsSingle();
            Container.Bind<SpawnPointService>().FromInstance(_spawnPointService).AsSingle();
        }
    }
}