using Modules;
using SnakeGame;
using UI;
using UnityEngine;
using Zenject;

namespace Gameplay.GameContext
{
    public class GameContextInstaller : MonoInstaller
    {
        [SerializeField] private WorldBounds _worldBounds;
        [SerializeField] private Snake _snake;
        [SerializeField] private GameUI _gameUI;
        [SerializeField] private Coin _coinPrefab;
        [SerializeField] private int _maxLevel = 9;
        
        public override void InstallBindings()
        {
            Container.Bind<IWorldBounds>().FromInstance(_worldBounds).AsSingle();
            Container.Bind<ISnake>().FromInstance(_snake).AsSingle();
            
            Container.Bind<IDifficulty>().To<Difficulty>().AsSingle().WithArguments(_maxLevel);
            Container.Bind<IScore>().To<Score>().AsSingle();
            
            Container.BindInterfacesTo<InputController>().AsSingle();
            Container.BindInterfacesTo<GameOverController>().AsSingle();

            Container.Bind<CoinFactory>().AsSingle().WithArguments(_coinPrefab);
            
            Container.Bind<CoinManager>().AsSingle();
            
            Container
                .BindInterfacesTo<GameUIPresenter>()
                .AsCached()
                .WithArguments(_gameUI);
            
            Container
                .Bind<GameCycle>()
                .AsSingle();

            Container
                .BindMemoryPool<MonoMemoryPool<Coin>>();
        }
    }
}
