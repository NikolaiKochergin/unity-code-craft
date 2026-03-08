using System;
using Modules;
using SnakeGame;
using Zenject;

namespace Game
{
    public class GameUIPresenter : IInitializable, IDisposable
    {
        private readonly IGameUI _gameUI;
        private readonly IScore _score;
        private readonly IDifficulty _difficulty;
        private readonly GameCycle _gameCycle;

        public GameUIPresenter(
            IGameUI gameUI, 
            IScore score, 
            IDifficulty difficulty,
            GameCycle gameCycle)
        {
            _difficulty = difficulty;
            _gameCycle = gameCycle;
            _score = score;
            _gameUI = gameUI;
        }

        public void Initialize()
        {
            _score.OnStateChanged += OnScoreChanged;
            _difficulty.OnStateChanged += OnDifficultyChanged;
            _gameCycle.OnGameOver += OnGameOver;
            
            OnScoreChanged(_score.Current);
            OnDifficultyChanged();
        }

        public void Dispose()
        {
            _score.OnStateChanged -= OnScoreChanged;
            _difficulty.OnStateChanged -= OnDifficultyChanged;
            _gameCycle.OnGameOver -= OnGameOver;
        }

        private void OnScoreChanged(int value) => 
            _gameUI.SetScore(value.ToString());

        private void OnDifficultyChanged() => 
            _gameUI.SetDifficulty(_difficulty.Current, _difficulty.Max);

        private void OnGameOver(bool isWin) => 
            _gameUI.GameOver(isWin);
    }
}