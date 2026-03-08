using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private WorldBounds _worldBounds;
        [SerializeField] private Snake _snake;
        [SerializeField] private GameUI _gameUI;
        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private int _maxLevel = 9;
        
        public override void InstallBindings()
        {
            Container.Bind<IDifficulty>().To<Difficulty>().AsSingle().WithArguments(_maxLevel);
            Container.Bind<IScore>().To<Score>().AsSingle();
            
            Container.BindInterfacesTo<InputController>().AsSingle();
            Container.Bind<ISnake>().FromInstance(_snake).AsSingle();
            
            Container.BindInterfacesTo<GameUIPresenter>().AsCached().WithArguments(_gameUI);
            
            Container.Bind<GameCycle>().AsSingle();

            Container.Bind<CoinManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<CoinPoints>().FromInstance(new CoinPoints(_worldBounds)).AsSingle();
            Container.BindInterfacesTo<CoinCollector>().AsSingle().NonLazy();
            Container
                .BindMemoryPool<Coin, CoinsPool>()
                .FromComponentInNewPrefab(_coinPrefab)
                .UnderTransform(_worldBounds.transform)
                .AsSingle();
            
        }
    }
}
