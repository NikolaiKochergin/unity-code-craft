using System;
using Modules;
using SnakeGame;
using UnityEngine;

namespace Game
{
    public class GameCycle
    {
        private readonly CoinManager _coinManager;
        private readonly IDifficulty _difficulty;
        private readonly IWorldBounds _worldBounds;
        private readonly ISnake _snake;
        public event Action<bool> OnGameOver;
        public bool IsWin { get; private set; }

        public GameCycle(
            CoinManager coinManager, 
            IDifficulty difficulty,
            IWorldBounds worldBounds,
            ISnake snake)
        {
            _snake = snake;
            _difficulty = difficulty;
            _worldBounds = worldBounds;
            _coinManager = coinManager;
        }
        
        public void StartGame()
        {
            _snake.SetActive(true);
            InitNextLevel();
            _coinManager.OnCoinsOver += InitNextLevel;
            _snake.OnSelfCollided += FinishGame;
            _snake.OnMoved += OnSnakeMoved;
        }

        private void InitNextLevel()
        {
            if (!_difficulty.Next(out int currentLevel))
            {
                FinishGame();
                return;
            }
            
            _coinManager.SpawnCoins(currentLevel);
            _snake.SetSpeed(currentLevel);
        }

        private void FinishGame()
        {
            _snake.OnSelfCollided -= FinishGame;
            _snake.OnMoved -= OnSnakeMoved;
            
            _snake.SetActive(false);
            IsWin = _difficulty.Current == _difficulty.Max && _coinManager.ActiveCoinsCount == 0;
            OnGameOver?.Invoke(IsWin);
        }
        
        private void OnSnakeMoved(Vector2Int position)
        {
            if(!_worldBounds.IsInBounds(position))
                FinishGame();
        }
    }
}