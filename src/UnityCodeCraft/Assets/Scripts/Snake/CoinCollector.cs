using System;
using Modules;
using UnityEngine;
using Zenject;

namespace Gameplay.GameContext
{
    public class CoinCollector : IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly CoinManager _coinManager;
        private readonly IScore _score;

        public CoinCollector(ISnake snake, CoinManager coinManager, IScore score)
        {
            _snake = snake;
            _coinManager = coinManager;
            _score = score;
        }

        public void Initialize() => 
            _snake.OnMoved += OnSnakeMoved;

        public void Dispose() => 
            _snake.OnMoved -= OnSnakeMoved;

        private void OnSnakeMoved(Vector2Int position)
        {
            if (!_coinManager.TryGetCoinAt(position, out ICoin coin))
                return;

            _score.Add(coin.Score);
            _snake.Expand(coin.Bones);
            _coinManager.DespawnCoin(coin);
        }
    }
}