using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameContextInstaller : MonoInstaller
    {
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private PlayerCharacterSpawner _characterSpawner;
        
        [SerializeField] private GameCycle _gameCycle;
        [SerializeField] private GameStartController _gameStartController;
        [SerializeField] private GameFinishController _gameFinishController;
        
        [SerializeField] private MoneyStorage _moneyStorage;

        public override void InstallBindings()
        {
            Container.Bind<PlayerManager>().FromInstance(_playerManager).AsSingle();
            Container.Bind<PlayerCharacterSpawner>().FromInstance(_characterSpawner).AsSingle();
            
            Container.Bind<GameCycle>().FromInstance(_gameCycle).AsSingle();
            Container.Bind<GameStartController>().FromInstance(_gameStartController).AsSingle();
            Container.Bind<GameFinishController>().FromInstance(_gameFinishController).AsSingle();
            Container.Bind<MoneyStorage>().FromInstance(_moneyStorage).AsSingle();
        }
    }
}