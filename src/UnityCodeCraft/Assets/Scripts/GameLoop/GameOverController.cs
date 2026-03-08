using System;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Gameplay.GameContext
{
    public class GameOverController : IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly IWorldBounds _worldBounds;
        private readonly GameCycle _gameCycle;

        public GameOverController(
            ISnake snake, 
            IWorldBounds worldBounds,
            GameCycle gameCycle)
        {
            _snake = snake;
            _worldBounds = worldBounds;
            _gameCycle = gameCycle;
        }
        
        public void Initialize()
        {
            _snake.OnSelfCollided += SetGameOver;
            _snake.OnMoved += OnSnakeMoved;
        }

        public void Dispose()
        {
            _snake.OnSelfCollided -= SetGameOver;
            _snake.OnMoved -= OnSnakeMoved;
        }

        private void OnSnakeMoved(Vector2Int position)
        {
            if(!_worldBounds.IsInBounds(position))
                SetGameOver();
        }

        private void SetGameOver()
        {
            _snake.SetSpeed(0);
            _gameCycle.FinishGame();
        }
    }
}