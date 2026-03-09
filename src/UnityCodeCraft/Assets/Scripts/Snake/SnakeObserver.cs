using System;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Game
{
    public class SnakeObserver : IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly CoinManager _coinManager;
        private readonly IScore _score;
        private readonly IWorldBounds _worldBounds;
        private readonly GameCycle _gameCycle;

        public SnakeObserver(
            ISnake snake, 
            CoinManager coinManager, 
            IScore score,
            IWorldBounds worldBounds,
            GameCycle gameCycle)
        {
            _worldBounds = worldBounds;
            _gameCycle = gameCycle;
            _snake = snake;
            _coinManager = coinManager;
            _score = score;
        }

        public void Initialize()
        {
            _snake.OnSelfCollided += OnSnakeCollidedSelf;
            _snake.OnMoved += OnSnakeMoved;
        }

        public void Dispose()
        {
            _snake.OnSelfCollided -= OnSnakeCollidedSelf;
            _snake.OnMoved -= OnSnakeMoved;
        }

        private void OnSnakeCollidedSelf()
        {
            _gameCycle.StopGame(withWin: false);
        }

        private void OnSnakeMoved(Vector2Int position)
        {
            HandleCoinPickAt(position);
            HandleWorldBoundsAt(position);
        }

        private void HandleCoinPickAt(Vector2Int position)
        {
            if (!_coinManager.TryGetCoinAt(position, out ICoin coin))
                return;

            _score.Add(coin.Score);
            _snake.Expand(coin.Bones);
            _coinManager.DespawnCoin(coin);
        }

        private void HandleWorldBoundsAt(Vector2Int position)
        {
            if (_worldBounds.IsInBounds(position))
                return;
            
            _gameCycle.StopGame(withWin: false);
        }
    }
}