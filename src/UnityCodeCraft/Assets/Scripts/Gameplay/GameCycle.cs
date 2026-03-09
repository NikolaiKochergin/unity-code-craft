using System;
using Modules;

namespace Game
{
    public class GameCycle
    {
        private readonly CoinManager _coinManager;
        private readonly IDifficulty _difficulty;
        private readonly ISnake _snake;

        public GameCycle(
            CoinManager coinManager, 
            IDifficulty difficulty,
            ISnake snake)
        {
            _snake = snake;
            _difficulty = difficulty;
            _coinManager = coinManager;
        }
        
        public event Action<bool> OnGameOver;
        
        public void StartGame()
        {
            InitNextLevel();
            _snake.SetActive(true);
            _coinManager.OnCoinsOver += InitNextLevel;
        }

        public void StopGame(bool withWin)
        {
            _snake.SetActive(false);
            _coinManager.OnCoinsOver -= InitNextLevel;
            OnGameOver?.Invoke(withWin);
        }

        private void InitNextLevel()
        {
            if (_difficulty.Next(out int currentLevel))
            {
                _coinManager.SpawnCoins(currentLevel);
                _snake.SetSpeed(currentLevel);
            }
            else
            {
                StopGame(withWin: true);
            }
        }
    }
}