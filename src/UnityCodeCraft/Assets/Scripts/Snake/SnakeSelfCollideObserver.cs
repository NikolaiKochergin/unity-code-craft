using System;
using Modules;
using Zenject;

namespace Game
{
    public class SnakeSelfCollideObserver : IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly GameCycle _gameCycle;

        public SnakeSelfCollideObserver(ISnake snake, GameCycle gameCycle)
        {
            _gameCycle = gameCycle;
            _snake = snake;
        }

        public void Initialize() => 
            _snake.OnSelfCollided += OnSnakeCollidedSelf;

        public void Dispose() => 
            _snake.OnSelfCollided -= OnSnakeCollidedSelf;

        private void OnSnakeCollidedSelf() => 
            _gameCycle.StopGame(withWin: false);
    }
}