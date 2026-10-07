using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameContextInstaller : MonoInstaller
    {
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private PlayerCharacterSpawner _playerCharacterSpawner;
        
        [SerializeField] private GameCycle _gameCycle;
        [SerializeField] private GameStartController _gameStartController;
        [SerializeField] private GameFinishController _gameFinishController;

        [SerializeField] private Store _store;
        [SerializeField] private MoneyStorage _moneyStorage;
        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private EnemyCharacterSpawner _enemyCharacterSpawner;
        
        [SerializeField] private ProjectileViewPool _projectilePool;

        public override void InstallBindings()
        {
            Container.Bind<PlayerManager>().FromInstance(_playerManager).AsSingle();
            Container.Bind<PlayerCharacterSpawner>().FromInstance(_playerCharacterSpawner).AsSingle();
            
            Container.Bind<GameCycle>().FromInstance(_gameCycle).AsSingle();
            Container.Bind<GameStartController>().FromInstance(_gameStartController).AsSingle();
            Container.Bind<GameFinishController>().FromInstance(_gameFinishController).AsSingle();

            Container.Bind<Store>().FromInstance(_store).AsSingle();
            Container.Bind<MoneyStorage>().FromInstance(_moneyStorage).AsSingle();
            Container.Bind<EnemyManager>().FromInstance(_enemyManager).AsSingle();
            Container.Bind<EnemyCharacterSpawner>().FromInstance(_enemyCharacterSpawner).AsSingle();
            
            Container.Bind<ProjectileViewPool>().FromInstance(_projectilePool).AsSingle();
        }
    }
}