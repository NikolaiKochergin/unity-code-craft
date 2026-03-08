using System;
using Modules;

namespace Gameplay.GameContext
{
    public class GameCycle
    {
        private readonly CoinManager _coinManager;
        private readonly IDifficulty _difficulty;
        private ISnake _snake;
        public event Action<bool> OnGameOver;
        public bool IsWin { get; private set; }

        public GameCycle(
            CoinManager coinManager, 
            IDifficulty difficulty,
            ISnake snake)
        {
            _snake = snake;
            _difficulty = difficulty;
            _coinManager = coinManager;
        }
        
        public void StartGame()
        {
            _snake.SetActive(true);
            InitNextLevel();
            _coinManager.OnCoinsOver += InitNextLevel;
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

        public void FinishGame()
        {
            _snake.SetActive(false);
            IsWin = _difficulty.Current == _difficulty.Max && _coinManager.ActiveCoinsCount == 0;
            OnGameOver?.Invoke(IsWin);
        }
    }
}