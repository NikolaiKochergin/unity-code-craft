using System;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Game
{
    public class SnakeWorldBoundsCollideObserver : IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly IWorldBounds _worldBounds;
        private readonly GameCycle _gameCycle;

        public SnakeWorldBoundsCollideObserver(
            ISnake snake, 
            IWorldBounds worldBounds, 
            GameCycle gameCycle)
        {
            _snake = snake;
            _worldBounds = worldBounds;
            _gameCycle = gameCycle;
        }

        public void Initialize() => 
            _snake.OnMoved += OnSnakeMoved;

        public void Dispose() => 
            _snake.OnMoved -= OnSnakeMoved;

        private void OnSnakeMoved(Vector2Int position)
        {
            if (!_worldBounds.IsInBounds(position))
                _gameCycle.StopGame(withWin: false);
        }
    }
}