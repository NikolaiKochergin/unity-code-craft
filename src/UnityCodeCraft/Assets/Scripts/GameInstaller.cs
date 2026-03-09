using SnakeGame;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private WorldBounds _worldBounds;
        [SerializeField] private GameUI _gameUI;
        [SerializeField] private SnakeInstaller _snakeInstaller;
        [SerializeField] private CoinsInstaller _coinsInstaller;
        [SerializeField] private GameplayInstaller _gameplayInstaller;
        
        public override void InstallBindings()
        {
            Container.Install(_snakeInstaller);
            Container.Install(_coinsInstaller);
            Container.Install(_gameplayInstaller);
            
            Container
                .BindInterfacesTo<WorldBounds>()
                .FromInstance(_worldBounds)
                .AsSingle();
            
            Container
                .BindInterfacesTo<GameUIPresenter>()
                .AsCached()
                .WithArguments(_gameUI);
        }
    }
}
