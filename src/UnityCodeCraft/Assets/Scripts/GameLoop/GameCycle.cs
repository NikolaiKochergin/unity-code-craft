using System;
using Modules;

namespace Gameplay.GameContext
{
    public class GameCycle
    {
        private readonly CoinManager _coinManager;
        private IDifficulty _difficulty;
        public event Action<bool> OnGameOver;
        public bool IsWin { get; private set; }

        public GameCycle(
            CoinManager coinManager, 
            IDifficulty difficulty)
        {
            _difficulty = difficulty;
            _coinManager = coinManager;
        }
        
        public void StartGame()
        {
            _coinManager.SpawnCoins();
        }

        public void FinishGame()
        {
            IsWin = _difficulty.Current == _difficulty.Max && _coinManager.ActiveCoins == 0;
            OnGameOver?.Invoke(IsWin);
        }
    }
}