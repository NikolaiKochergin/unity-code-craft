using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameContextInstaller : MonoInstaller
    {
        [SerializeField] private Portal _portal;
        
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private PlayerCharacterSpawner _characterSpawner;
        [SerializeField] private SpawnPointService _spawnPointService;
        
        [SerializeField] private GameCycle _gameCycle;
        [SerializeField] private GameStartController _gameStartController;
        [SerializeField] private GameFinishController _gameFinishController;

        public override void InstallBindings()
        {
            Container.Bind<PlayerManager>().FromInstance(_playerManager).AsSingle();
            Container.Bind<PlayerCharacterSpawner>().FromInstance(_characterSpawner).AsSingle();
            Container.Bind<SpawnPointService>().FromInstance(_spawnPointService).AsSingle();
            
            Container.Bind<GameCycle>().FromInstance(_gameCycle).AsSingle();
            Container.Bind<GameStartController>().FromInstance(_gameStartController).AsSingle();
            Container.Bind<GameFinishController>().FromInstance(_gameFinishController).AsSingle();
            
            Container.Bind<Portal>().FromInstance(_portal).AsSingle();
            
            _gameFinishController.AddPlayerTeamUnit(_portal.GetBehaviour<NetworkObject>());
        }
    }
}